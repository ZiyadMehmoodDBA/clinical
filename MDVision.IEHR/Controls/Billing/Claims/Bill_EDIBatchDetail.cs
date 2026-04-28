using MDVision.Business.BCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using System.IO;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using EDIParser;
using MDVision.Model.Billing.ERA;
using Newtonsoft.Json.Linq;

namespace MDVision.IEHR.Controls.Billing.Claims
{
    public class Bill_EDIBatchDetail
    {
        private BLLBillingClaim BLLBillingClaimObj = null;
        public Bill_EDIBatchDetail()
        {
            BLLBillingClaimObj = new BLLBillingClaim();
        }

        #region Singleton
        private static Bill_EDIBatchDetail _obj = null;
        public static Bill_EDIBatchDetail Instance()
        {
            if (_obj == null)
                _obj = new Bill_EDIBatchDetail();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string FillEDIBatchDetail(Int64 _837BatchId, string BatchControlNo)
        {
            try
            {
                DS837Batch dsBatches = null;
                DSEDIReports dsEDIReports = null;
                BLObject<DS837Batch> obj;
                BLObject<DSEDIReports> obj835;
                obj = BLLBillingClaimObj.Load837Batch(_837BatchId, "", "", null, 0, null,"","");
                dsBatches = obj.Data;
                //  _837BatchId = 163;
                obj835 = BLLBillingClaimObj.Load837BatchReports(BatchControlNo, 0);
                dsEDIReports = obj835.Data;
                if (dsBatches.Tables[dsBatches._837Batch.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsBatches.Tables[dsBatches._837Batch.TableName].Rows[0];

                    var keyValues = new Dictionary<string, string>
                        {
                            { "txtBatchNumber", MDVUtility.ToStr(dr[dsBatches._837Batch.BatchControlNoColumn.ColumnName])},
                            { "txtClearingHouse", MDVUtility.ToStr(dr[dsBatches._837Batch.ClearingHouseNameColumn.ColumnName])},
                            { "hfClearingHouseId", MDVUtility.ToStr(dr[dsBatches._837Batch.ClearingHouseIdColumn.ColumnName])},
                            { "txtSubmitDate", MDVUtility.ToStr(dr[dsBatches._837Batch.SubmitDateColumn.ColumnName])},
                            { "txtSubmittedBy", MDVUtility.ToStr(dr[dsBatches._837Batch.CreatedByColumn.ColumnName])},
                            { "txtSubmitType", MDVUtility.ToStr(dr[dsBatches._837Batch.SubmitTypeStatusColumn.ColumnName])},
                            { "txtTotalClaims", MDVUtility.ToStr(dr[dsBatches._837Batch.TotalClaimsColumn.ColumnName])},
                            { "txtClaimsAccepted", MDVUtility.ToStr(dr[dsBatches._837Batch.ClaimsAcceptedColumn.ColumnName])},
                            { "txtClaimsRejected", MDVUtility.ToStr(dr[dsBatches._837Batch.ClaimsRejectedColumn.ColumnName])},
                            { "hfEDI837String", MDVUtility.ToStr(dr[dsBatches._837Batch.EDI837StringColumn.ColumnName])},
                            { "chkCompleted",MDVUtility.ToStr(dr[dsBatches._837Batch.IsCompletedColumn.ColumnName])},
                            { "txtComments",MDVUtility.ToStr(dr[dsBatches._837Batch.CommentsColumn.ColumnName])}
                        };
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        EDIBatchLoad_JSON = js.Serialize(keyValues),
                        EDIReport_JSON = MDVUtility.JSON_DataTable(dsEDIReports.Tables[dsEDIReports._837BatchReport.TableName]),
                        EDIReportCount = dsEDIReports.Tables[dsEDIReports._837BatchReport.TableName].Rows.Count,
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

        private string DeleteEDIReport(string EDIReportId)
        {
            try
            {
                if (string.IsNullOrEmpty(EDIReportId))
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
                    BLObject<string> obj = BLLBillingClaimObj.DeleteEDIReports(EDIReportId);
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

        private Dictionary<string, string> GetReportInfo(string FileName)
        {
            Dictionary<string, string> Response = new Dictionary<string, string>();
            try
            {
                string ex = System.IO.Path.GetExtension(FileName);
                switch (ex)
                {
                    case ".txt":
                    case ".dat":
                    case ".rpt":
                        Response.Add("REPORTS", "REPORTS");
                        break;
                    case ".835":
                        Response.Add("835", "ERA (ELECTRONIC REMITTANCE ADVICE)");
                        break;
                    case ".271":
                        Response.Add("271", "ELIGIBILITY RESPONSE");
                        break;
                    case ".277":
                        Response.Add("277", "CLAIM STATUS RESPONSE");
                        break;
                    case ".997":
                        Response.Add("997", "ACKNOWLEDGEMENT");
                        break;
                }

                return Response;
            }
            catch (Exception)
            {
                return Response;
            }

        }

        private string SaveEDIFile(string _837BatchNumber, string fileName, string fileData, string _837BatchId, long ClearingHouseId, bool IsBatch)
        {
            try
            {

                DSEDIReports dsEDIReports = new DSEDIReports();
                DSEDIReports.EDIReportsRow dr = dsEDIReports.EDIReports.NewEDIReportsRow();
                dr.ClearingHouseId = ClearingHouseId;
                //dr.SubmittedBatchNo = SubmittedBatchNo;
                dr.ReportDate = DateTime.Now;
                dr.EDIText = fileData;
                //dr.FileName = Guid.NewGuid().ToString() + ".txt";
                dr.FileName = fileName;

                Dictionary<string, string> ReportInfo = GetReportInfo(fileName);
                var Type = "";
                if (ReportInfo.Count > 0)
                {
                    Type = ReportInfo.First().Key;
                    dr.ReportType = ReportInfo.First().Key;
                    dr.ReportTitle = ReportInfo.First().Value;
                }

                dr.IsReviewed = false;
                dr.ClientId = MDVUtility.ToInt64(MDVSession.Current.ClientId);
                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);

                dr.IsActive = true;
                dr.IsParse = "";
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                if (Type == "277")
                {
                    DS277 ds277 = new DS277();
                    BLObject<DS277> obj277 = BLLBillingClaimObj.Report277CA(fileData);
                    ds277 = obj277.Data;
                    dynamic EdiDetailObj = JObject.Parse(getEdidetail(ds277));
                    List<STC> STCList = new List<STC>();
                    STCList = EdiDetailObj.STCRows.ToObject<List<STC>>();
                    dr.TotalRejected = EdiDetailObj.TotalRejected;
                    dr.TotalAccepted = EdiDetailObj.TotalAccepted;

                    dr.xml = MDVUtility.GetXmlOfObject(typeof(List<STC>), STCList);
                    
                }
                #region Database Insertion
                dsEDIReports.EDIReports.AddEDIReportsRow(dr);

                DSEDIReports._837BatchReportRow drBatchReport = dsEDIReports._837BatchReport.New_837BatchReportRow();
                drBatchReport.BatchNumber = _837BatchNumber;
                drBatchReport.Status = "Import";
                drBatchReport.Description = "Import document";

                dsEDIReports._837BatchReport.Add_837BatchReportRow(drBatchReport);

                BLObject<DSEDIReports> obj = BLLBillingClaimObj.InsertEDIReportBatch(dsEDIReports, IsBatch);
                dsEDIReports = obj.Data;
                if (obj.Data != null)
                {

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        EDIReportId = dsEDIReports.Tables[dsEDIReports.EDIReports.TableName].Rows[0][dsEDIReports.EDIReports.EDIReportIdColumn.ColumnName],
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
        public string getEdidetail(DS277 ds)
        {
            List<STC> STCRows = new List<STC>();
            var TotalRejected = 0;
            var TotalAccepted = 0;
            foreach (DS277.EDI277NamesRow EDI277Names in ds.EDI277Names)
            {
                if (EDI277Names.NM101_QUL == "85")
                {
                    var ProviderName = EDI277Names.NM103;
                    var NPI = EDI277Names.NM109;

                    foreach (DS277.EDI277NamesRow Patient_item in ds.EDI277Names)
                    {

                        if (Patient_item.ParentNameId == EDI277Names.EDI277NameId)
                        {
                            //Select Patient STC rows
                            foreach (DS277.EDI277StatusRow Status_item in ds.EDI277Status)
                            {

                                if (Status_item.EDI277NameId == Patient_item.EDI277NameId)
                                {
                                    STC stc = new STC();
                                    stc.ClaimNumber = Patient_item.TRN02;
                                    stc.ChargeAmount = Status_item.STC04;
                                    stc.PaidAmount = Status_item.STC05;
                                    stc.ClaimCategoryCode = Status_item.STC01_1_QUL + ": " + Status_item.STC01_1;
                                    stc.ClaimStatusCode = Status_item.STC01_2_QUL + ": " + Status_item.STC01_2;
                                    if (Status_item.STC03.ToLower() == "accept")
                                    {
                                        stc.isAccepted = true;
                                        TotalAccepted++;
                                    }
                                    else
                                    {
                                        if (Status_item.STC01_1_QUL == "A3" || Status_item.STC01_1_QUL == "A4" || Status_item.STC01_1_QUL == "A6" || Status_item.STC01_1_QUL == "A7" || Status_item.STC01_1_QUL == "A8")
                                        {
                                            stc.rejectionReason = Status_item.STC12;
                                        }
                                        stc.isAccepted = false;
                                        TotalRejected++;
                                    }
                                    STCRows.Add(stc);
                                }

                            }


                        }

                    }
                }
            }

            var respons = new
            {
                TotalAccepted = TotalAccepted,
                TotalRejected = TotalRejected,
                STCRows = STCRows
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(respons));
        }

        private string UpdateEDIReport(long _837BatchId, string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (SearchedfieldsJSON != null)
                {
                    DS837Batch dsBatches = new DS837Batch();
                    BLObject<DS837Batch> obj;
                    obj = BLLBillingClaimObj.Load837Batch(_837BatchId, "", "", null, 0, "","", "");
                    if (obj.Data != null)
                    {
                        foreach (DS837Batch._837BatchRow dr in obj.Data.Tables[dsBatches._837Batch.TableName].Rows)
                        {
                            dr.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                            //dr.IsCompleted = MDVUtility.ToStr(SearchedfieldsJSON["chkCompleted"]) == "True";
                        }

                        dsBatches = obj.Data;

                        BLObject<DS837Batch> objBatches = BLLBillingClaimObj.Update837Batch(dsBatches);
                        if (objBatches.Data != null)
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
                                Message = objBatches.Message
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
                        Message = ""
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

        #endregion

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "FILL_EDI_BATCH_DETAIL":
                    {
                        string _837BatchId = MDVUtility.ToStr(context.Request["_837BatchId"]);
                        string BatchControlNo = MDVUtility.ToStr(context.Request["BatchControlNo"]);
                        string strJSONData = FillEDIBatchDetail(MDVUtility.ToInt64(_837BatchId), BatchControlNo);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_EDI_REPORT":
                    {
                        string EDIReportId = MDVUtility.ToStr(context.Request["EDIReportId"]);
                        string strJSONData = DeleteEDIReport(EDIReportId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_EDI_FILE":
                    {
                        string _837BatchNumber = MDVUtility.ToStr(context.Request["_837BatchNumber"]);
                        string fileName = MDVUtility.ToStr(context.Request["fileName"]);
                        string fileData = MDVUtility.ReplaceSpecialCharacters(MDVUtility.ToStr(context.Request["fileData"]));

                        string _837BatchId = MDVUtility.ToStr(context.Request["_837BatchId"]);
                        string ClearingHouseId = MDVUtility.ToStr(context.Request["ClearingHouseId"]);
                        bool IsBatch = Convert.ToBoolean(context.Request["IsBatch"]);

                        string strJSONData = SaveEDIFile(_837BatchNumber, fileName, fileData, _837BatchId, Convert.ToInt64(ClearingHouseId), IsBatch);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_EDI_REPORT":
                    {
                        long _837BatchId = MDVUtility.ToLong(context.Request["_837BatchId"]);
                        string fieldsJSON = context.Request["_837BatchData"];
                        string strJSONData = UpdateEDIReport(_837BatchId, fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

            }
        }


        #endregion

    }
}