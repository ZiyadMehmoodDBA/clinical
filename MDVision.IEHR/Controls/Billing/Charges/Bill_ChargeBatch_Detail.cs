using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MDVision.IEHR.Controls.Billing
{
    public class Bill_ChargeBatch_Detail
    {
        private BLLBilling BLLBillingObj = null;
        private BLLVisits BLLVisitsObj = null;
        public Bill_ChargeBatch_Detail()
        {
            BLLBillingObj = new BLLBilling();
            BLLVisitsObj = new BLLVisits();
        }
        #region Singleton
        private static Bill_ChargeBatch_Detail _obj = null;
        public static Bill_ChargeBatch_Detail Instance()
        {
            if (_obj == null)
                _obj = new Bill_ChargeBatch_Detail();
            return _obj;
        }
        #endregion
        #region Service Command Handler

        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            string privilegesMessage = string.Empty;
            switch (cammandAction)
            {
                case "ADD_BATCHCHARGE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charge Batch", "ADD")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["BatchChargeData"];
                            strJSONData = SaveBatchCharge(fieldsJSON);
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
                        break;
                    }
                case "ADD_BATCHCHARGE_DOCUMENT":
                    {
                        string strJSONData = "";
                        try
                        {
                            if (context.Request.Files.Count > 0)
                            {
                                int BatchID = MDVUtility.ToInt32(context.Request["BatchID"]);
                                string fileName = context.Request["FileName"];
                                string fileType = context.Request["fileType"];
                                fileType = fileType == null ? "application/pdf" : fileType;
                                strJSONData = SaveBatchChargeDocument(BatchID, context.Request.Files, fileName, MDVUtility.ToStr(context.Request["base64"]), fileType);
                            }
                            else
                            {
                                int BatchID = MDVUtility.ToInt32(context.Request["BatchID"]);
                                string strBase64 = MDVUtility.ToStr(context.Request["base64"]);
                                string fileType = MDVUtility.ToStr(context.Request["fileType"]);
                                string fileName = context.Request["FileName"];
                                strJSONData = SaveBatchChargeDocument(BatchID, context.Request.Files, fileName, strBase64, fileType);
                            }
                        }
                        catch (Exception ex)
                        {
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "SEARCH_BATCH_CHARGE_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charge Batch", "SEARCH")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            int BatchDocId = MDVUtility.ToInt32(context.Request["BatchDocId"]);
                            int BatchId = MDVUtility.ToInt32(context.Request["BatchId"]);
                            string isFileStream = MDVUtility.ToStr(context.Request["isFileStream"]);
                            strJSONData = LoadBatchChargeDocument(BatchDocId, BatchId, isFileStream);
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
                        break;
                    }
                case "DELETE_BATCH_CHARGE_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charge Batch", "DELETE")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            int BatchDocId = MDVUtility.ToInt32(context.Request["BatchDocId"]);
                            strJSONData = DeleteBatchChargeDocument(BatchDocId);
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
                        break;
                    }
                case "FILL_BATCH_CHARGE":
                    {
                        long BatchID = MDVUtility.ToLong(context.Request["BatchId"]);
                        string strJSONData = FillBatchCharge(BatchID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "UPDATE_BATCH_CHARGE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charge Batch", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["batchChargeData"];
                            int BatchId = MDVUtility.ToInt(context.Request["BatchId"]);
                            strJSONData = UpdateBatchCharge(fieldsJSON, BatchId);
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
                        break;
                    }
                case "SEARCH_VISIT_CLAIM":
                    {
                        long PatientID = 0;
                        string ClaimNumber = MDVUtility.ToStr(context.Request["ClaimNumber"]);

                        if (context.Request["PatientID"] != null)
                            PatientID = MDVUtility.ToLong(context.Request["PatientID"]);

                        string strJSONData = SearchVisitClaims(ClaimNumber, PatientID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "UPDATE_BATCH_CHARGE_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charge Batch", "EDIT")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            string fieldsJSON = context.Request["batchChargeDocumentData"];
                            Int64 BatchDocId = MDVUtility.ToInt64(context.Request["BatchDocId"]);
                            strJSONData = UpdateBatchChargeDocument(fieldsJSON, BatchDocId);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "FILL_BATCH_CHARGE_DOCUMENT":
                    {
                        Int64 BatchDocId = MDVUtility.ToInt64(context.Request["BatchDocId"]);
                        string strJSONData = FillBatchChargeDocument(BatchDocId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                        break;
                    }
                case "DELETE_BATCH_CHARGE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Charge Batch", "DELETE")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            long BatchId = MDVUtility.ToLong(context.Request["BatchId"]);
                            strJSONData = DeleteBatchCharge(BatchId);
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
                        break;
                    }
            }
        }
        #endregion
        #region Private Function
        private string SaveBatchCharge(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSBatchCharge dsBatchCharge = new DSBatchCharge();
                DSBatchCharge.BatchesRow dr = dsBatchCharge.Batches.NewBatchesRow();

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"])))
                    dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtFacility"])))
                    dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtProvider"])))
                    dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpFromDOS"])))
                    dr.DOSFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpFromDOS"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpToDOS"])))
                    dr.DOSTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpToDOS"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlBiller"])))
                    dr.BillerId = MDVUtility.ToLong(SearchedfieldsJSON["ddlBiller"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtClaims"])))
                    dr.Claims = MDVUtility.ToInt32(SearchedfieldsJSON["txtClaims"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCharges"])))
                    dr.Charges = MDVUtility.ToInt32(SearchedfieldsJSON["txtCharges"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCopaymentCollected"])))
                    dr.CopaymentCollected = MDVUtility.ToDouble(SearchedfieldsJSON["txtCopaymentCollected"]);

                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlBatchStatus"])))
                    dr.BatchStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlBatchStatus"]);
                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsBatchCharge.Batches.AddBatchesRow(dr);
                BLObject<DSBatchCharge> obj = BLLBillingObj.InsertBatchCharge(dsBatchCharge);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        BatchNumber = dsBatchCharge.Tables[dsBatchCharge.Batches.TableName].Rows[0][dsBatchCharge.Batches.BatchNumberColumn.ColumnName],
                        BatchID = dsBatchCharge.Tables[dsBatchCharge.Batches.TableName].Rows[0][dsBatchCharge.Batches.BatchIdColumn.ColumnName],
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
        //public string UpdateBatchCharge(string fieldJSON , int batchID)
        //{
        //    try
        //    {
        //        DSBatchCharge dsBatchCharge = null;
        //        BLObject<DSBatchCharge> obj = null;
        //        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldJSON);
        //        obj = BLLBillingObj.LoadBatchCharge(MDVUtility.ToLong(SearchedfieldsJSON["hfBatchID"]), MDVUtility.ToStr(SearchedfieldsJSON["txtBatchNumber"]), 0, 0, 0, "");
        //        dsBatchCharge = obj.Data;
        //        if (dsBatchCharge.Tables[dsBatchCharge.Batches.TableName].Rows.Count > 0)
        //        {

        //            foreach (DataRow dr in dsBatchCharge.Tables[dsBatchCharge.Batches.TableName].Rows)
        //            {
        //                //if (SearchedfieldsJSON.ContainsKey("txtComments") && !string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
        //                //{
        //                //    dr[dsBatchCharge.PatientDocument.CommentsColumn] = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
        //                //}
        //            }

        //            obj = BLLBillingObj.UpdateBatchCharge(dsBatchCharge);

        //            if (obj.Data != null)
        //            {
        //                var response = new
        //                {
        //                    status = true,
        //                    message = Common.AppPrivileges.Update_Message
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = obj.Message
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = obj.Message
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }

        //}

        private string SaveBatchChargeDocument(int BatchID, HttpFileCollection files, string fileName, string strBase64 = null, string fileType = null)
        {
            try
            {
                int counter = 0;
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                DSBatchCharge dsBatchCharge = new DSBatchCharge();
                if (string.IsNullOrWhiteSpace(strBase64))
                {
                    foreach (string name in files)
                    {
                        HttpPostedFile file = files[name];
                        DSBatchCharge.BatchDocumentsRow dr = dsBatchCharge.BatchDocuments.NewBatchDocumentsRow();

                        dr = SaveBatchChargeDocumentFile(BatchID, dsBatchCharge, file, fileName.Split(',')[counter], fileType.Split(',')[counter]);
                        dsBatchCharge.BatchDocuments.AddBatchDocumentsRow(dr);
                        counter += 1;
                    }
                }
                else
                {
                    //foreach (string name in files)
                    //{
                    //    HttpPostedFile file = files[name];
                    byte[] currentFileStream = Convert.FromBase64String(strBase64);
                        DSBatchCharge.BatchDocumentsRow dr = dsBatchCharge.BatchDocuments.NewBatchDocumentsRow();
                        dr = SaveBatchChargeScanDocument(BatchID, dsBatchCharge, null, fileName.Split(',')[counter], fileType.Split(',')[counter], currentFileStream);

                        dsBatchCharge.BatchDocuments.AddBatchDocumentsRow(dr);
                    //}
                }
                #region Database Insertion
                BLObject<DSBatchCharge> obj = BLLBillingObj.InsertBatchChargeDocument(dsBatchCharge);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        BatchDocId = dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows[0][dsBatchCharge.BatchDocuments.BatchDocIdColumn.ColumnName]
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public DSBatchCharge.BatchDocumentsRow SaveBatchChargeScanDocument(int BatchID, DSBatchCharge dsBatchCharge, HttpPostedFile file, string FileName, string ContentType, byte[] currentFileStream)
        {
            DSBatchCharge.BatchDocumentsRow dr = dsBatchCharge.BatchDocuments.NewBatchDocumentsRow();
            dr.BatchId = BatchID;
            //byte[] currentFileStream = new byte[file.ContentLength];
            //int isRead = file.InputStream.Read(currentFileStream, 0, file.ContentLength);
            dr.FileType = ContentType;
            dr.FilePath = FileName.Split('.')[0];
            MemoryStream ms = new MemoryStream(currentFileStream);
            if (ContentType == "application/pdf")
                dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
            else
                dr.Pages = 1;
            dr.IsActive = true;
            dr.IsAttached = true;
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;
            dr.Url = CommonFunc.SaveDocumentToFolder(file, "Charge Batch Documents", "Encounters", BatchID, FileName, currentFileStream);

            return dr;
        }

        public DSBatchCharge.BatchDocumentsRow SaveBatchChargeDocumentFile(Int64 BatchID, DSBatchCharge dsBatchCharge, HttpPostedFile file, string FileName, string ContentType)
        {
            DSBatchCharge.BatchDocumentsRow dr = dsBatchCharge.BatchDocuments.NewBatchDocumentsRow();

            dr.BatchId = BatchID;
            byte[] currentFileStream = new byte[file.ContentLength];
            int isRead = file.InputStream.Read(currentFileStream, 0, file.ContentLength);
            //dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
            dr.FileType = ContentType;
            dr.FilePath = FileName.Split('.')[0];
            if (file.ContentType == "application/pdf")
                dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
            else
                dr.Pages = 1;
            dr.IsActive = true;
            dr.IsAttached = true;
            dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.CreatedOn = DateTime.Now;
            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            dr.ModifiedOn = DateTime.Now;
            dr.Url = CommonFunc.SaveDocumentToFolder(file, "Charge Batch Documents", "Encounters", BatchID, FileName, null);
            return dr;
        }

        private string LoadBatchChargeDocument(Int32 BatchDocId, Int32 BatchId, string isFileStream = "0")
        {

            try
            {

                DSBatchCharge dsBatchCharge = null;
                BLObject<DSBatchCharge> obj;
                obj = BLLBillingObj.LoadBatchChargeDocument(BatchDocId, BatchId, isFileStream);
                dsBatchCharge = obj.Data;
                if (obj.Data != null)
                {
                    if (dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows)
                        {
                            byte[] byteArr = dr["FileStream"] as byte[];
                            if (byteArr != null)
                            {
                                string strBase64 = Convert.ToBase64String(byteArr);
                                // Add a New Column to Store the Base64 String
                                if (!dr.Table.Columns.Contains("Base64FileStream"))
                                {
                                    dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                }
                                dr["Base64FileStream"] = strBase64;
                            }
                        }
                        var response = new
                        {
                            status = true,
                            BatchChargeDocumentCount = dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows.Count,
                            BatchChargeDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = obj.Message
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

        private string DeleteBatchChargeDocument(long BatchDocId)
        {

            try
            {
                if (BatchDocId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.DeleteBatchChargeDocument(BatchDocId);
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

        private string FillBatchCharge(long BatchId)
        {
            try
            {

                DSBatchCharge dsBatchCharge = null;
                BLObject<DSBatchCharge> obj = null;
                obj = BLLBillingObj.LoadBatchCharge(BatchId, "", "", 0, 0, 0, "", 0, "", null, null, null, null);
                dsBatchCharge = obj.Data;
                if (obj.Data != null)
                {
                    if (dsBatchCharge.Tables[dsBatchCharge.Batches.TableName].Rows.Count > 0)
                    {

                        DataRow dr = dsBatchCharge.Tables[dsBatchCharge.Batches.TableName].Rows[0];
                        var keyValues = new Dictionary<string, string>
                        {
                            { "txtBatchNumber", MDVUtility.ToStr(dr[dsBatchCharge.Batches.BatchNumberColumn.ColumnName])},
                            { "txtDescription", MDVUtility.ToStr(dr[dsBatchCharge.Batches.DescriptionColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsBatchCharge.Batches.FacilityIdColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsBatchCharge.Batches.FacilityNameColumn.ColumnName])},
                            { "hfPractice", MDVUtility.ToStr(dr[dsBatchCharge.Batches.PracticeIdColumn.ColumnName])},
                            { "txtPractice", MDVUtility.ToStr(dr[dsBatchCharge.Batches.PracticeNameColumn.ColumnName])},
                            { "txtProvider", MDVUtility.ToStr(dr[dsBatchCharge.Batches.ProviderNameColumn.ColumnName])},
                            { "hfProvider", MDVUtility.ToStr(dr[dsBatchCharge.Batches.ProviderIdColumn.ColumnName])},
                            //{ "dtpFromDOS", MDVUtility.ToDateTime(dr[dsBatchCharge.Batches.DOSFromColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "dtpFromDOS",  dr[dsBatchCharge.Batches.DOSFromColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsBatchCharge.Batches.DOSFromColumn.ColumnName]).ToShortDateString()},
                            { "ddlBiller", MDVUtility.ToStr(dr [dsBatchCharge.Batches.BillerIdColumn.ColumnName])},
                            { "txtClaims", MDVUtility.ToStr(dr[dsBatchCharge.Batches.ClaimsColumn.ColumnName])},
                            { "txtCharges", MDVUtility.ToStr(dr[dsBatchCharge.Batches.ChargesColumn.ColumnName])},
                            { "txtCopaymentCollected", MDVUtility.ToStr(dr[dsBatchCharge.Batches.CopaymentCollectedColumn.ColumnName])},
                            //{ "dtpToDOS", MDVUtility.ToDateTime(dr[dsBatchCharge.Batches.DOSToColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "dtpToDOS",  dr[dsBatchCharge.Batches.DOSToColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsBatchCharge.Batches.DOSToColumn.ColumnName]).ToShortDateString()},
                            { "hfBatchID", MDVUtility.ToStr(dr[dsBatchCharge.Batches.BatchIdColumn.ColumnName])},
                            { "txtClaimsEntered", MDVUtility.ToStr(dr[dsBatchCharge.Batches.ClaimsEnteredColumn.ColumnName])},
                            { "txtChargesEntered", MDVUtility.ToStr(dr[dsBatchCharge.Batches.ChargesEnteredColumn.ColumnName])},
                            { "txtCopaymentPosted", MDVUtility.ToStr(dr[dsBatchCharge.Batches.CopaymentPostedColumn.ColumnName])},
                            { "txtTotalCharges", MDVUtility.ToStr(dr[dsBatchCharge.Batches.TotalAmountColumn.ColumnName])},
                            { "ddlBatchStatus", MDVUtility.ToStr(dr[dsBatchCharge.Batches.BatchStatusIdColumn.ColumnName])},
                            //{ "dtpStarDate", MDVUtility.ToStr(dr[dsBatchCharge.Batches.StartDateColumn.ColumnName])},
                            { "dtpStarDate",  dr[dsBatchCharge.Batches.StartDateColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsBatchCharge.Batches.StartDateColumn.ColumnName]).ToShortDateString()},
                            //{ "dtpEndDate", MDVUtility.ToStr(dr[dsBatchCharge.Batches.EndDateColumn.ColumnName])},
                            { "dtpEndDate",  dr[dsBatchCharge.Batches.EndDateColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsBatchCharge.Batches.EndDateColumn.ColumnName]).ToShortDateString()},
                            { "txtHoursSpent", MDVUtility.ToStr(dr[dsBatchCharge.Batches.TotalHoursColumn.ColumnName])},
                            { "txtBatchEnteredBy", MDVUtility.ToStr(dr[dsBatchCharge.Batches.BatchEnteredByColumn.ColumnName])},
                            //{ "dtpBatchEntryDate", MDVUtility.ToDateTime(dr[dsBatchCharge.Batches.BatchEntryDateColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "dtpBatchEntryDate",  dr[dsBatchCharge.Batches.BatchEntryDateColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsBatchCharge.Batches.BatchEntryDateColumn.ColumnName]).ToShortDateString()},

                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;
                        var response = new
                        {
                            status = true,
                            BatchChargeCount = dsBatchCharge.Tables[dsBatchCharge.Batches.TableName].Rows.Count,
                            BatchChargeLoad_JSON = js.Serialize(keyValues),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = obj.Message
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

        private string UpdateBatchCharge(string fieldsJSON, int BatchId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSBatchCharge dsBatchCharge = new DSBatchCharge();
                BLObject<DSBatchCharge> objLoad = BLLBillingObj.LoadBatchCharge(BatchId, "", "", 0, 0, 0, "", 0, "", null, null, null, null);
                dsBatchCharge = objLoad.Data;
                foreach (DSBatchCharge.BatchesRow dr in dsBatchCharge.Tables[dsBatchCharge.Batches.TableName].Rows)
                {
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"])))
                        dr.Description = MDVUtility.ToStr(SearchedfieldsJSON["txtDescription"]);
                    else
                        dr[dsBatchCharge.Batches.DescriptionColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtFacility"])))
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtProvider"])))
                        dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                    else
                        dr[dsBatchCharge.Batches.ProviderIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpFromDOS"])))
                        dr.DOSFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpFromDOS"]);
                    else
                        dr[dsBatchCharge.Batches.DOSFromColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpToDOS"])))
                        dr.DOSTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpToDOS"]);
                    else
                        dr[dsBatchCharge.Batches.DOSToColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlBiller"])))
                        dr.BillerId = MDVUtility.ToLong(SearchedfieldsJSON["ddlBiller"]);
                    else
                        dr[dsBatchCharge.Batches.BillerIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtClaims"])))
                        dr.Claims = MDVUtility.ToInt32(SearchedfieldsJSON["txtClaims"]);
                    else
                        dr[dsBatchCharge.Batches.ClaimsColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCharges"])))
                        dr.Charges = MDVUtility.ToInt32(SearchedfieldsJSON["txtCharges"]);
                    else
                        dr[dsBatchCharge.Batches.ChargesColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCopaymentCollected"])))
                        dr.CopaymentCollected = MDVUtility.ToDouble(SearchedfieldsJSON["txtCopaymentCollected"]);
                    else
                        dr[dsBatchCharge.Batches.CopaymentCollectedColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlBatchStatus"])))
                        dr.BatchStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlBatchStatus"]);


                    dr.IsActive = true;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Updation



                if (dsBatchCharge.Tables[dsBatchCharge.Batches.TableName].Rows.Count > 0)
                {
                    //  dsPatient.Patients.Rows[0].SetModified();
                    BLObject<DSBatchCharge> obj = BLLBillingObj.UpdateBatchCharge(dsBatchCharge);
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

        public string SearchVisitClaims(string ClaimNumber, long PatientID)
        {
            try
            {
                DSVisitLookup dsVisitLookUp = null;
                BLObject<DSVisitLookup> obj;
                obj = BLLVisitsObj.LookupPatientVisits(PatientID, ClaimNumber, false);
                dsVisitLookUp = obj.Data;
                if (obj.Data != null)
                {
                    if (dsVisitLookUp.Tables[dsVisitLookUp.PatientVisits.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ClaimsCount = dsVisitLookUp.Tables[dsVisitLookUp.PatientVisits.TableName].Rows.Count,
                            ClaimsLoad_JSON = MDVUtility.JSON_DataTable(dsVisitLookUp.Tables[dsVisitLookUp.PatientVisits.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = 0,
                            Message = obj.Message
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

        private string UpdateBatchChargeDocument(string fieldsJSON, Int64 BatchDocId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSBatchCharge dsBatchCharge = new DSBatchCharge();
                BLObject<DSBatchCharge> objLoad = BLLBillingObj.LoadBatchChargeDocument(BatchDocId, 0, "1");
                dsBatchCharge = objLoad.Data;
                foreach (DSBatchCharge.BatchDocumentsRow dr in dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows)
                {
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtPatientName"])))
                        dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);
                    else
                        dr[dsBatchCharge.BatchDocuments.PatientIdColumn] = DBNull.Value;

                    if (MDVUtility.ToInt32(SearchedfieldsJSON["ddlAction"]) != 0)
                        dr.ActionId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlAction"]);
                    else
                        dr[dsBatchCharge.BatchDocuments.ActionIdColumn] = DBNull.Value;
                    if (MDVUtility.ToInt32(SearchedfieldsJSON["ddlReason"]) != 0)
                        dr.ReasonId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlReason"]);
                    else
                        dr[dsBatchCharge.BatchDocuments.ReasonIdColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtClaimNumber"])))
                    {
                        dr.ClaimNumber = SearchedfieldsJSON["txtClaimNumber"];
                        dr.VisitId = MDVUtility.ToInt64(SearchedfieldsJSON["hfVisitId"]);
                    }
                    else
                    {
                        dr[dsBatchCharge.BatchDocuments.ClaimNumberColumn] = DBNull.Value;
                        dr[dsBatchCharge.BatchDocuments.VisitIdColumn] = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtCaseNumber"])))
                        dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["hfCaseId"]);
                    else
                        dr[dsBatchCharge.BatchDocuments.CaseIdColumn] = DBNull.Value;

                    //                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["txtComments"])))
                    dr.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                    if (!string.IsNullOrWhiteSpace(SearchedfieldsJSON["txtFileName"]) && SearchedfieldsJSON.ContainsKey("txtFileName"))
                    {
                        dr[dsBatchCharge.BatchDocuments.FilePathColumn] = SearchedfieldsJSON["txtFileName"] + SearchedfieldsJSON["lnkFileNameExt"];
                    }


                    dr.IsActive = true;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Updation



                if (dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows.Count > 0)
                {
                    BLObject<DSBatchCharge> obj = BLLBillingObj.UpdateBatchChargeDocument(dsBatchCharge);
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

        private string FillBatchChargeDocument(Int64 BatchDocId)
        {
            try
            {

                DSBatchCharge dsBatchCharge = null;
                BLObject<DSBatchCharge> obj = null;
                obj = BLLBillingObj.LoadBatchChargeDocument(BatchDocId, 0, "1");
                dsBatchCharge = obj.Data;
                if (obj.Data != null)
                {
                    if (dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows.Count > 0)
                    {
                        string LoadPrevious = "0";
                        foreach (DataRow dtr in dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows)
                        {
                            byte[] byteArr = dtr["FileStream"] as byte[];
                            string UrlPath = dtr["Url"].ToString();
                            if (!String.IsNullOrEmpty(UrlPath))
                            {
                                byte[] file;
                                string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                                UrlPath = FilePath + UrlPath;
                                using (var stream = new FileStream(UrlPath, FileMode.Open, FileAccess.Read))
                                {
                                    using (var reader = new BinaryReader(stream))
                                    {
                                        file = reader.ReadBytes((int)stream.Length);

                                    }
                                }
                                if (file != null)
                                {
                                    if (!dtr.Table.Columns.Contains("Base64FileStream"))
                                    {
                                        dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                    }
                                    dtr["Base64FileStream"] = Convert.ToBase64String(file);
                                }
                                LoadPrevious = "1";
                            }
                            else if (byteArr != null)
                            {
                                string strBase64 = Convert.ToBase64String(byteArr);
                                // Add a New Column to Store the Base64 String
                                if (!dtr.Table.Columns.Contains("Base64FileStream"))
                                {
                                    dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                }
                                dtr["Base64FileStream"] = strBase64;
                                LoadPrevious = "1";
                            }
                            else
                            {
                            }
                        }
                        DataRow dr = dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows[0];
                        string patientName = null;
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.AccountNumberColumn.ColumnName])))
                        {
                            patientName = MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.AccountNumberColumn.ColumnName]) + " - " + MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.PatientNameColumn.ColumnName]);
                        }
                        string fileName = MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.FilePathColumn.ColumnName]);
                        int index = fileName.LastIndexOf('.');
                        string strFileName = string.Empty;
                        string strFileExt = string.Empty;
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            if (index > 0)
                            {
                                strFileName = fileName.Substring(0, index);
                                strFileExt = "." + fileName.Substring(index + 1);
                            }
                            else
                            {
                                strFileName = fileName;
                                strFileExt = "." + MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.FileTypeColumn.ColumnName]).Split('/')[1];
                            }
                        }
                        else
                        {
                            strFileExt = "." + MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.FileTypeColumn.ColumnName]).Split('/')[1];
                        }
                        var keyValues = new Dictionary<string, string>
                        {
                            { "txtLoadPrevious", LoadPrevious},
                            { "txtPatientName", patientName},
                            { "ddlAction", MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.ActionIdColumn.ColumnName])},
                            { "ddlReason", MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.ReasonIdColumn.ColumnName])},
                            //{ "dpDOSfrm", MDVUtility.ToDateTime(dr[dsBatchCharge.BatchDocuments.DOSFromColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "dpDOSfrm",  dr[dsBatchCharge.BatchDocuments.DOSFromColumn.ColumnName].ToString() ==""?"": MDVUtility.ToDateTime(dr[dsBatchCharge.BatchDocuments.DOSFromColumn.ColumnName]).ToShortDateString()},
                            { "txtClaimNumber", MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.ClaimNumberColumn.ColumnName])},
                            { "txtCaseNumber", MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.CaseNumberColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.CommentsColumn.ColumnName])},
                            { "hfPatientId", MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.PatientIdColumn.ColumnName])},
                            { "hfVisitId", MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.VisitIdColumn.ColumnName])},
                            { "hfCaseId", MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.CaseIdColumn.ColumnName])},
                            { "Base64FileStream", MDVUtility.ToStr(dr["Base64FileStream"])},
                            { "FileType", MDVUtility.ToStr(dr[dsBatchCharge.BatchDocuments.FileTypeColumn.ColumnName])},
                            { "txtFileName",strFileName },
                            { "lnkFileNameExt", strFileExt}
                            

                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;
                        var response = new
                        {
                            status = true,
                            Count = dsBatchCharge.Tables[dsBatchCharge.BatchDocuments.TableName].Rows.Count,
                            DocumentLoad_JSON = js.Serialize(keyValues),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = obj.Message
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

        private string DeleteBatchCharge(long BatchId)
        {

            try
            {
                if (BatchId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.DeleteBatchCharge(BatchId);
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
        #endregion

    }
}