using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Model.Clinical.Medical;
using MDVision.Model.Dashboard;
using MDVision.Model.User;

namespace MDVision.IEHR.Controls.Patient.Messages
{
    public class Patient_Message
    {
        private BLLMessage BLLMessageObj = null;
        private BLLSchedule BLLScheduleObj = null;
        private BLLRcopia BLLRcopiaObj = null;
        private BLLPatient BLLPatientObj = null;
        public Patient_Message()
        {
            BLLMessageObj = new BLLMessage();
            BLLScheduleObj = new BLLSchedule();
            BLLRcopiaObj = new BLLRcopia();
            BLLPatientObj = new BLLPatient();
        }


        #region Singleton
        private static Patient_Message _obj = null;

        public static Patient_Message Instance()
        {
            if (_obj == null)
            {
                _obj = new Patient_Message();

            }
            return _obj;
        }
        #endregion


        #region Private Functions

        /// <summary>
        /// Saves the Patient Message.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string SavePatientMessage(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSMessage dsMessage = new DSMessage();
                DSMessage.PatMessagesRow dr = dsMessage.PatMessages.NewPatMessagesRow();
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                    dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);
                dr.MsgDetail = MDVUtility.ToStr(SearchedfieldsJSON["txtMessage"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlType"]))
                    dr.MsgTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlType"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpCalledDate"]))
                    dr.CallDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCalledDate"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDateOfService"]))
                    dr.DOS = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDateOfService"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlStatus"]))
                    dr.MsgStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlStatus"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAlertType"]))
                    dr.AlertTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlAlertType"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignedto"]))
                    dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignedto"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPriority"]))
                    dr.PriorityId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPriority"]);
                dr.EntryDate = DateTime.Now;
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignedto"]))
                {
                    dr.UserName = SearchedfieldsJSON["ddlAssignedto_text"];
                    dr.UserId = SearchedfieldsJSON["ddlAssignedto"];
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMedication"]))
                    dr.MedicationId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlMedication"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPharmacy"]))
                    dr.PharmacyId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPharmacy"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlLabOrder"]))
                    dr.LabOrderId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlLabOrder"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlLab"]))
                    dr.LabId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlLab"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAmendmentSource"]))
                    dr.AmdtSourceId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlAmendmentSource"]);
                dr.VisToPatient = MDVUtility.ToStr(SearchedfieldsJSON["chkVisibleToPatient"]) == "True" ? true : false;
                dr.IsActive = true;//Utility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfUserMessagesId"]))
                    dr.UserMessagesId = MDVUtility.ToInt64(SearchedfieldsJSON["hfUserMessagesId"]);

                #region Database Insertion
                dsMessage.PatMessages.AddPatMessagesRow(dr);
                BLObject<DSMessage> obj = BLLMessageObj.InsertPatientMessage(dsMessage);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        MessageId = dsMessage.Tables[dsMessage.PatMessages.TableName].Rows[0][dsMessage.PatMessages.PatMsgIdColumn.ColumnName]
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
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Updates the Patient Message.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PatientId">The Patient identifier.</param>
        /// <param name="MessageID">The Message identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string UpdatePatientMessage(string fieldsJSON, Int64 MessageID)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (MessageID > 0)
                {

                    DSMessage dsMessage = new DSMessage();
                    //DSMessage.PatMessagesRow dr = dsMessage.PatMessages.NewPatMessagesRow();
                    BLObject<DSMessage> objLoad = BLLMessageObj.LoadPatientMessage(0, MessageID, "", 0, null, null);
                    dsMessage = objLoad.Data;
                    foreach (DSMessage.PatMessagesRow dr in dsMessage.Tables[dsMessage.PatMessages.TableName].Rows)
                    {
                        //dr.PatMsgId = MessageID;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                            dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);
                        dr.MsgDetail = MDVUtility.ToStr(SearchedfieldsJSON["txtMessage"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlType"]))
                            dr.MsgTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlType"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpCalledDate"]))
                            dr.CallDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCalledDate"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDateOfService"]))
                            dr.DOS = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDateOfService"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlStatus"]))
                            dr.MsgStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlStatus"]);
                        else
                            dr[dsMessage.PatMessages.MsgStatusIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAlertType"]))
                            dr.AlertTypeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlAlertType"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignedto"]))
                            dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignedto"]);
                        else
                            dr[dsMessage.PatMessages.AssignedToIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPriority"]))
                            dr.PriorityId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPriority"]);
                        //dr.EntryDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignedto"]))
                        {
                            dr.UserName = SearchedfieldsJSON["ddlAssignedto_text"];
                            dr.UserId = SearchedfieldsJSON["ddlAssignedto"];
                        }
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMedication"]))
                            dr.MedicationId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlMedication"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPharmacy"]))
                            dr.PharmacyId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPharmacy"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlLabOrder"]))
                            dr.LabOrderId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlLabOrder"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlLab"]))
                            dr.LabId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlLab"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAmendmentSource"]))
                            dr.AmdtSourceId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlAmendmentSource"]);
                        dr.VisToPatient = MDVUtility.ToStr(SearchedfieldsJSON["chkVisibleToPatient"]) == "True" ? true : false;
                        //dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                        dr.IsActive = true;//Utility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    //dsMessage.PatMessages.AddPatMessagesRow(dr);
                    //dsMessage.PatMessages.AcceptChanges();

                    if (dsMessage.Tables[dsMessage.PatMessages.TableName].Rows.Count > 0)
                    {
                        //dsMessage.PatMessages.Rows[0].SetModified();
                        BLObject<DSMessage> obj = BLLMessageObj.UpdatePatientMessage(dsMessage);
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
                        Message = "Message not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            //return "";
        }

        /// <summary>
        /// Deletes the Patient Message.
        /// </summary>
        /// <param name="MessageID">The Message identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string DeletePatientMessage(long MessageID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(MessageID)))
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
                    BLObject<string> obj = BLLMessageObj.DeletePatientMessage(MDVUtility.ToStr(MessageID));
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
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Saves the Patient Reply.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string SavePatientMessageReply(string fieldsJSON, Int64 MessageId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (MessageId > 0)
                {

                    DSMessage dsMessage = new DSMessage();
                    DSMessage.MsgReplyRow dr = dsMessage.MsgReply.NewMsgReplyRow();

                    dr.PatMsgId = MessageId;
                    dr.MsgDetail = MDVUtility.ToStr(SearchedfieldsJSON["txtMessage"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignedto"]))
                        dr.AssignedToId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAssignedto"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlStatus"]))
                        dr.MsgStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlStatus"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlAssignedto"]))
                    {
                        dr.UserName = SearchedfieldsJSON["ddlAssignedto_text"];
                        dr.UserId = SearchedfieldsJSON["ddlAssignedto"];
                    }
                    dr.SpkPatinet = MDVUtility.ToStr(SearchedfieldsJSON["chkVisibleToPatient"]) == "True" ? true : false;
                    dr.IsActive = true;//Utility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Insertion
                    dsMessage.MsgReply.AddMsgReplyRow(dr);
                    BLObject<DSMessage> obj = BLLMessageObj.InsertPatientMessageReply(dsMessage);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            MessageReplyId = dsMessage.Tables[dsMessage.MsgReply.TableName].Rows[0][dsMessage.MsgReply.MsgrIdColumn.ColumnName]
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
                        Message = "Message not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Load all the Patient Messages for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The Message identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        public string SearchPatientMessage(string fieldsJSON, Int64 PatientID, Int64 MessageID, Int64 AssignedToId, string MsgTypeId, int MsgStatusId, Int32 PageNumber, Int32 RowsPerPage)
        {
            try
            {

                DSMessage dsMessage = null;
                BLObject<DSMessage> obj;
                // obj = BLLMessageObj.LoadPatientMessage(PatientID, MessageID, MsgTypeId, MsgStatusId, null, null, 0);

                if (fieldsJSON == null || fieldsJSON.Length <= 2)
                {
                    obj = BLLMessageObj.searchPatientMessage(PatientID, MessageID, MsgTypeId, MsgStatusId, null, null, null, PageNumber, RowsPerPage);
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    DateTime? callDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpCalledDate"]) != "" ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCalledDate"]) : null;
                    DateTime? entryDate = MDVUtility.ToStr(SearchedfieldsJSON["dtpEntryDate"]) != "" ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpEntryDate"]) : null;
                    string IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]);
                    //obj = BLLMessageObj.LoadPatientMessage(PatientID, MessageID, SearchedfieldsJSON["ddlType"], MDVUtility.ToInt32(SearchedfieldsJSON["ddlStatus"]), callDate, entryDate, AssignedToId, "", PageNumber, RowsPerPage);
                    // System.Diagnostics.Debug.WriteLine("Start time " + DateTime.Now);
                    obj = BLLMessageObj.searchPatientMessage(PatientID, MessageID, SearchedfieldsJSON["ddlType"], MsgStatusId, callDate, entryDate, IsActive, PageNumber, RowsPerPage);
                    // System.Diagnostics.Debug.WriteLine("End time " + DateTime.Now);
                }

                dsMessage = obj.Data;
                if (obj.Data != null)
                {
                    if (dsMessage.Tables[dsMessage.PatMessagesSearch.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = dsMessage.Tables[dsMessage.PatMessagesSearch.TableName].Rows.Count,

                            iTotalDisplayRecords = (dsMessage.PatMessagesSearch.Rows.Count > 0) ? dsMessage.PatMessagesSearch.Rows[0][dsMessage.PatMessagesSearch.RecordCountColumn.ColumnName] : 0,
                            MessageLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.PatMessagesSearch.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            Message = "Record not found."
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
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string FillPatientMessage(Int64 MessageID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(MessageID)))
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
                    DSMessage dsMessage = null;
                    BLObject<DSMessage> obj = BLLMessageObj.LoadPatientMessage(0, MessageID, "", 0, null, null);
                    if (obj.Data != null)
                    {
                        dsMessage = obj.Data;
                        if (dsMessage.Tables[dsMessage.PatMessages.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsMessage.Tables[dsMessage.PatMessages.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtMessage", MDVUtility.ToStr(dr[dsMessage.PatMessages.MsgDetailColumn.ColumnName])},
                            { "hfPatientId", MDVUtility.ToStr(dr[dsMessage.PatMessages.PatientIdColumn.ColumnName])},
                            { "txtAccountNumber", MDVUtility.ToStr(dr[dsMessage.PatMessages.AccountNumberColumn.ColumnName])},
                            { "txtPatientName", MDVUtility.ToStr(dr[dsMessage.PatMessages.PatientNameColumn.ColumnName])},
                            { "ddlType", MDVUtility.ToStr(dr[dsMessage.PatMessages.MsgTypeIdColumn.ColumnName])},
                            { "ddlStatus", MDVUtility.ToStr(dr[dsMessage.PatMessages.MsgStatusIdColumn.ColumnName])},
                            { "dtpCalledDate", MDVUtility.ToStr(dr[dsMessage.PatMessages.CallDateColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsMessage.PatMessages.CallDateColumn.ColumnName]).ToShortDateString():""},
                            { "dtpDateOfService", MDVUtility.ToStr(dr[dsMessage.PatMessages.DOSColumn.ColumnName]) !="" ? MDVUtility.ToDateTime(dr[dsMessage.PatMessages.DOSColumn.ColumnName]).ToShortDateString():""},
                            { "ddlAssignedto", MDVUtility.ToStr(dr[dsMessage.PatMessages.AssignedToIdColumn.ColumnName])},
                            { "ddlAlertType", MDVUtility.ToStr(dr[dsMessage.PatMessages.AlertTypeIdColumn.ColumnName])},
                            { "ddlPriority", MDVUtility.ToStr(dr[dsMessage.PatMessages.PriorityIdColumn.ColumnName])},
                            { "chkVisibleToPatient", MDVUtility.ToStr(dr[dsMessage.PatMessages.VisToPatientColumn.ColumnName])},
                            { "ddlLabOrder", MDVUtility.ToStr(dr[dsMessage.PatMessages.LabOrderIdColumn.ColumnName])},
                            { "ddlLab", MDVUtility.ToStr(dr[dsMessage.PatMessages.LabIdColumn.ColumnName])},
                            { "ddlMedication", MDVUtility.ToStr(dr[dsMessage.PatMessages.MedicationIdColumn.ColumnName])},
                            { "ddlPharmacy", MDVUtility.ToStr(dr[dsMessage.PatMessages.PharmacyIdColumn.ColumnName])},
                            { "ddlAmendmentSource", MDVUtility.ToStr(dr[dsMessage.PatMessages.AmdtSourceIdColumn.ColumnName])},
                            { "hfPatDocId", MDVUtility.ToStr(dr[dsMessage.PatMessages.PatDocIdColumn.ColumnName])},
                            { "hfFilePath", MDVUtility.ToStr(dr[dsMessage.PatMessages.FilePathColumn.ColumnName])},
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                MessageFill_JSON = js.Serialize(keyValues),
                                ReplyLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.MsgReply.TableName], "", false),
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string UpdatePatientMessageIsActive(Int64 MessageID, Int64 pageNo, Int64 rpp, Int64 IsActive)
        {
            try
            {
                if (MessageID > 0)
                {

                    DSMessage dsMessage = null;
                    BLObject<DSMessage> obj = BLLMessageObj.LoadPatientMessage(0, MessageID, "", 0, null, null);
                    dsMessage = obj.Data;
                    if (dsMessage.Tables[dsMessage.PatMessages.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsMessage.Tables[dsMessage.PatMessages.TableName].Rows[0];
                        dr[dsMessage.PatMessages.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSMessage> objMessage = BLLMessageObj.UpdatePatientMessage(dsMessage);
                        string successMsg;
                        if (objMessage.Data != null)
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
                                Message = objMessage.Message
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
                        Message = "Message not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string RefreshCount()
        {
            try
            {
                Int64 AssignedToId = MDVSession.Current.AppUserId;

                string time = MDVSession.Current.RefreshTime;

                long MessageCount = 0;
                long TaskCount = 0;
                long DocCount = 0;

                List<UserMessagesCount> dsMessage = new List<UserMessagesCount>();
                BLObject<List<UserMessagesCount>> obj;

                obj = BLLMessageObj.LoadPatientMessageCount(0, 0, "", 2, null, null, AssignedToId);
                dsMessage = obj.Data;

                //Set Current User Messages Count
                MessageCount = dsMessage.Sum(p=> MDVUtility.ToLong( p.OtherCount));
                //Set Current User Tasks Count
                TaskCount = dsMessage.Sum(p => MDVUtility.ToLong(p.TaskCount));

                // Set Patient Document Count.
                List<DPandingPatientDoc> patDocList = new List<DPandingPatientDoc>();
                BLObject<DPatientDoucmnetModel> objDoc = BLLPatientObj.LoadDashboardDocument(null, null, 1, 15, null, MDVUtility.ToStr(MDVSession.Current.AppUserId), null, "Pending");
                if (objDoc.Data != null)
                {
                    DocCount = MDVUtility.ToLong(objDoc.Data.ListDDocumentModel[0].Pending);                   
                }
                              
                // Start 30/11/2015 Muhammad Irfan refresh Appointments and Notes count

                string AppointmentCount = "";
                Int32 NotesCount = 0;

                DSAppointment dsAppointment = null;
                BLObject<DSAppointment> objAppointment;

                string AppDate = DateTime.Now.Date.ToString();
                objAppointment = BLLScheduleObj.LoadAppointmentsVisits(0, 0, 0, AppDate, "", "", "", null, "0", 1, 15, "");
                dsAppointment = objAppointment.Data;
                if (dsAppointment.Tables[dsAppointment.AppointmentsVisits.TableName].Rows.Count > 0)
                {
                    AppointmentCount = MDVUtility.ToStr(dsAppointment.AppointmentsVisits.Rows[0][dsAppointment.AppointmentsVisits.RecordCountColumn.ColumnName]);
                }

                DSAppointment dsAppointmentNotes = null;
                BLObject<DSAppointment> objAppointmentNotes;
                objAppointmentNotes = BLLScheduleObj.LoadAppointmentsNotes("", "", "Draft", "", "", "", 0, "");
                dsAppointmentNotes = objAppointmentNotes.Data;
                if (dsAppointmentNotes.Tables[dsAppointmentNotes.AppointmentsVisits.TableName].Rows.Count > 0)
                {
                    NotesCount = dsAppointmentNotes.Tables[dsAppointmentNotes.AppointmentsVisits.TableName].Rows.Count;
                }
                NotificationCountModel NotificationCountModelObj = new NotificationCountModel();
                string prescriptionsRefillCount = "?";
                string pendingPrescriptionsCount = "?";
                try
                {
                    // End 30/11/2015 Muhammad Irfan refresh Appointments and Notes count
                    RcopiaModel rcopiaModel = new RcopiaModel();
                    RcopiaHelper helperRcopia = new RcopiaHelper();
                    rcopiaModel.IsPatientLastUpdateInfo = false;
                    DSRcopia dsRcopia = new DSRcopia();
                    BLObject<DSRcopia> objrcopia = BLLRcopiaObj.SelectGetUrls();
                    dsRcopia = objrcopia.Data;
                    if (objrcopia.Data != null)
                    {

                        rcopiaModel.EngineDownloadURL = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.Rcopia_GetUrl.TableName].Rows[0][dsRcopia.Rcopia_GetUrl.EngineDownloadURLColumn.ColumnName]);

                    }
                    //getting Count of Pending Prescriptions and Prescriptions Refill
                    NotificationCountModelObj = helperRcopia.DownloadNotificationCount(rcopiaModel);
                    prescriptionsRefillCount = NotificationCountModelObj.RefillPrescriptionCount.ToString();
                    pendingPrescriptionsCount = NotificationCountModelObj.PendingPrescriptionCount.ToString();
                }
                catch (Exception)
                {

                    prescriptionsRefillCount = "?";
                    pendingPrescriptionsCount = "?";
                }


                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        messageCount = MessageCount,
                        taskCount = TaskCount,
                        appointmentCount = AppointmentCount,
                        notesCount = NotesCount,
                        pendingDocCount= DocCount,
                        pendingPrescriptionCount = pendingPrescriptionsCount,
                        refillPrescriptionCount = prescriptionsRefillCount

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
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Patient Message Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            string privilegesMessage = string.Empty;
            switch (cammandAction)
            {
                case "SEARCH_PATIENT_MESSAGE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Tasks", "SEARCH")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["PatientMessageData"];
                            string page = context.Request["page"];
                            Int64 rpp = 15;//Utility.ToInt64(context.Request["rp"]);
                            Int64 pageNo = 1; //Utility.ToInt64(context.Request["rp"]);
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                            Int64 MessageID = MDVUtility.ToInt64(context.Request["MessageId"]);
                            Int64 AssignedToId = MDVUtility.ToInt64(context.Request["AssignedToId"]);
                            string MsgTypeId = context.Request["MsgTypeId"];
                            int MsgStatusId = MDVUtility.ToInt(context.Request["MsgStatusId"]);
                            Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                            Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);

                            strJSONData = SearchPatientMessage(fieldsJSON, PatientID, MessageID, AssignedToId, MsgTypeId, MsgStatusId, PageNumber, RowsPerPage);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_MESSAGE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Tasks", "VIEW")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            Int64 MessageID = MDVUtility.ToInt64(context.Request["MessageId"]);
                            strJSONData = FillPatientMessage(MessageID);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PATIENT_MESSAGE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Tasks", "ADD")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["PatientMessageData"];
                            strJSONData = SavePatientMessage(fieldsJSON);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_MESSAGE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Tasks", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["PatientMessageData"];
                            Int64 MessageID = MDVUtility.ToInt64(context.Request["MessageId"]);
                            strJSONData = UpdatePatientMessage(fieldsJSON, MessageID);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_MESSAGE_ACTIVE_INACTIVE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Tasks", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["MessageId"]);
                            Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                            Int64 rpp = 15;//Utility.ToInt64(context.Request["rp"]);
                            Int64 pageNo = 1; //Utility.ToInt64(context.Request["rp"]);

                            strJSONData = UpdatePatientMessageIsActive(PatientID, pageNo, rpp, IsActive);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PATIENT_MESSAGE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Tasks", "DELETE")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            Int64 PatientID = MDVUtility.ToInt64(context.Request["MessageId"]);
                            strJSONData = DeletePatientMessage(PatientID);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_MESSAGE_REPLY":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Message Reply", "ADD")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["MessageReplyData"];
                            Int64 MessageID = MDVUtility.ToInt64(context.Request["MessageID"]);
                            strJSONData = SavePatientMessageReply(fieldsJSON, MessageID);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "REFRESH_COUNT":
                    {
                        string strJSONData = RefreshCount();

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}