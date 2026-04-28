using EDIParser;
using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.IEHR.Model.Billing.Claims;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Billing.Claims
{
    public class Bill_EDIReviewReport
    {
        private BLLBillingClaim BLLBillingClaimObj = null;
        public Bill_EDIReviewReport()
        {
             BLLBillingClaimObj = new BLLBillingClaim();
        }
        #region Singleton
        private static Bill_EDIReviewReport _obj = null;
        public static Bill_EDIReviewReport Instance()
        {
            if (_obj == null)
                _obj = new Bill_EDIReviewReport();
            return _obj;
        }
        #endregion

        #region Private Functions

        public string FillEDIReport(EDIReviewReportModel model)
        {
            try
            {

                DSEDIReports dsEDIReports = null;
                BLObject<DSEDIReports> obj;
                obj = BLLBillingClaimObj.LoadEDIReports(MDVUtility.ToInt64(model.EDIReportID), 0, true, null, null, null, null, null, null, null, MDVUtility.ToStr(model.CheckNumber));

                if (obj.Data != null)
                {
                    dsEDIReports = obj.Data;

                    if (dsEDIReports.Tables[dsEDIReports.EDIReports.TableName].Rows.Count > 0)
                    {

                        DataRow dr = dsEDIReports.Tables[dsEDIReports.EDIReports.TableName].Rows[0];
                        string str = "";
                        if (dr[dsEDIReports.EDIReports.ReportTypeColumn.ColumnName].ToString() == "835")
                            str = dr[dsEDIReports.EDIReports.EDIFormatColumn.ColumnName].ToString();
                        else
                            str = dr[dsEDIReports.EDIReports.EDITextColumn.ColumnName].ToString();

                        DS277 ds277 = new DS277();
                        if (MDVUtility.ToStr(dr[dsEDIReports.EDIReports.ReportTypeColumn.ColumnName]) == "277")
                        {
                            BLObject<DS277> obj277 = BLLBillingClaimObj.Report277CA(MDVUtility.ToStr(dr[dsEDIReports.EDIReports.EDITextColumn.ColumnName]));
                            ds277 = obj277.Data;
                        }

                        model.Comments = MDVUtility.ToStr(dr[dsEDIReports.EDIReports.CommentsColumn.ColumnName]);
                        model.Active = MDVUtility.ToStr(dr[dsEDIReports.EDIReports.IsReviewedColumn.ColumnName]);
                        model.ClearingHouse = MDVUtility.ToStr(dr[dsEDIReports.EDIReports.ClearingHouseIdColumn.ColumnName]);
                        model.HtmlView = str;
                        model.TextView = MDVUtility.ToStr(dr[dsEDIReports.EDIReports.EDITextColumn.ColumnName]);
                        model.ReportType = MDVUtility.ToStr(dr[dsEDIReports.EDIReports.ReportTypeColumn.ColumnName]); //need to discuss
                        //var keyValues = new Dictionary<object, object>
                        //{
                        //    //{"txtBatchNo",Utility.ToStr(dr[dsEDIReports.EDIReports.BatchNumberColumn.ColumnName])},
                        //    {"txtComments",Utility.ToStr(dr[dsEDIReports.EDIReports.CommentsColumn.ColumnName])},
                        //    {"chkReviewed",Utility.ToStr(dr[dsEDIReports.EDIReports.IsReviewedColumn.ColumnName])},
                        //    {"ddlClearinghouse",Utility.ToStr(dr[dsEDIReports.EDIReports.ClearingHouseIdColumn.ColumnName])},
                        //    {"ReportType", MDVUtility.ToStr(dr[dsEDIReports.EDIReports.ReportTypeColumn.ColumnName])},
                        //    { "txtHTMLView", str},
                        //    { "txtTextView", MDVUtility.ToStr(dr[dsEDIReports.EDIReports.EDITextColumn.ColumnName])}
                        //};

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            EDI_FileDataJSON = js.Serialize(model),
                            Report277CA_JSON = ds277
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        throw new Exception("no record found.");
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
                    //The value for column 'FIELD_BATCH_BATCH_ID_PK' in table 'TableBatch' is DBNull.
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
        public string FillBatchEDIReport(EDIReviewReportModel model)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                DSEDIReports dsEDIReports = null;
                BLObject<DSEDIReports> obj;
                obj = BLLBillingClaimObj.Load837BatchReports(model.EDIBatchNumber, MDVUtility.ToInt64(model.EDIReportID), true);
                if (obj.Data != null)
                {
                    dsEDIReports = obj.Data;

                    if (dsEDIReports.Tables[dsEDIReports._837BatchReport.TableName].Rows.Count > 0)
                    {


                        DataRow dr = dsEDIReports.Tables[dsEDIReports._837BatchReport.TableName].Rows[0];
                        string str = "";
                        if (dr[dsEDIReports._837BatchReport.ReportTypeColumn.ColumnName].ToString() == "835")
                            str = dr[dsEDIReports._837BatchReport.EDIFormatColumn.ColumnName].ToString();
                        else
                            str = dr[dsEDIReports._837BatchReport.EDITextColumn.ColumnName].ToString();


                        model.Comments = MDVUtility.ToStr(dr[dsEDIReports._837BatchReport.BatchNumberColumn.ColumnName]);
                        model.Active = MDVUtility.ToStr(dr[dsEDIReports._837BatchReport.IsReviewedColumn.ColumnName]);
                        model.ClearingHouse = MDVUtility.ToStr(dr[dsEDIReports._837BatchReport.ClearingHouseIdColumn.ColumnName]);
                        model.HtmlView = str;
                        model.TextView = MDVUtility.ToStr(dr[dsEDIReports.EDIReports.EDITextColumn.ColumnName]);
                        model.BatchNumber = MDVUtility.ToStr(dr[dsEDIReports._837BatchReport.EDITextColumn.ColumnName]);
                        //var keyValues = new Dictionary<string, string>
                        //{
                        //    {"txtBatchNo",Utility.ToStr(dr[dsEDIReports._837BatchReport.BatchNumberColumn.ColumnName])},
                        //    {"txtComments",Utility.ToStr(dr[dsEDIReports._837BatchReport.CommentsColumn.ColumnName])},
                        //    {"chkReviewed",Utility.ToStr(dr[dsEDIReports._837BatchReport.IsReviewedColumn.ColumnName])},
                        //    {"ddlClearinghouse",Utility.ToStr(dr[dsEDIReports._837BatchReport.ClearingHouseIdColumn.ColumnName])},

                        //    { "txtHTMLView", str},

                        //    { "txtTextView", MDVUtility.ToStr(dr[dsEDIReports._837BatchReport.EDITextColumn.ColumnName])}

                        //};

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            EDI_FileDataJSON = js.Serialize(model)
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
        public string UpdateReport(EDIReviewReportModel model)
        {
            try
            {

                DSEDIReports dsEDIReports = new DSEDIReports();
                BLObject<DSEDIReports> obj;
                obj = BLLBillingClaimObj.LoadEDIReports(Convert.ToInt64(model.EDIReportID));
                dsEDIReports = obj.Data;

                if (dsEDIReports.Tables[dsEDIReports.EDIReports.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsEDIReports.Tables[dsEDIReports.EDIReports.TableName].Rows[0];
                    dr[dsEDIReports.EDIReports.CommentsColumn.ColumnName] = model.Comments;
                    dr[dsEDIReports.EDIReports.IsReviewedColumn.ColumnName] = MDVUtility.ToStr(model.Active) == "True" ? true : false;
                    dr[dsEDIReports.EDIReports.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr[dsEDIReports.EDIReports.ModifiedOnColumn.ColumnName] = DateTime.Now;
                    BLObject<DSEDIReports> objEDIReports = BLLBillingClaimObj.UpdateEDIReports(dsEDIReports);
                    if (objEDIReports.Data != null)
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
                            Message = objEDIReports.Message
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
        public string GetBatchDetail(EDIReviewReportModel model)
        {

            DS837Batch dsBatches = null;
            BLObject<DS837Batch> obj;
            obj = BLLBillingClaimObj.Load837Batch(0, model.BatchNumber, "", null, 0, null, "", "");
            dsBatches = obj.Data;
            if (dsBatches.Tables[dsBatches._837Batch.TableName].Rows.Count > 0)
            {
               
                var response = new
                {
                    status = true,
                    EDIReport_JSON = MDVUtility.JSON_DataTable(dsBatches.Tables[dsBatches._837Batch.TableName]),
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
        
        #endregion

    }
}