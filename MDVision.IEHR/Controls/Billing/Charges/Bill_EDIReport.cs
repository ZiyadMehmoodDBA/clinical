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
using MDVision.IEHR.Model.Billing.Charges;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;


namespace MDVision.IEHR.Controls.Billing.Charges
{
    public class Bill_EDIReport
    {
        private BLLBillingClaim BLLBillingClaimObj = null;
        public Bill_EDIReport()
        {
            BLLBillingClaimObj = new BLLBillingClaim();
        }
        #region Singleton
        private static Bill_EDIReport _obj = null;
        public static Bill_EDIReport Instance()
        {
            return _obj ?? (_obj = new Bill_EDIReport());
        }

        #endregion

        /// <summary>
        /// Load EDI Report
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="ediReportId"></param>
        /// <param name="_837BatchId"></param>
        /// <returns></returns>

        public string LoadEDIReports(EDIReportModel model)
        {
            try
            {
                DateTime? downloadDate = null;
                DSEDIReports dsEdiReports = null;
                BLObject<DSEDIReports> objEdiReports;

                downloadDate = String.IsNullOrEmpty(model.DownloadDate) ? (DateTime?)null : DateTime.Parse(model.DownloadDate);

                //searchedfieldsJson.ContainsKey("ddlSpecialityID") ? string.IsNullOrEmpty(SearchedfieldsJSON["ddlSpecialityID"]) ? 0 : MDVUtility.ToLong(SearchedfieldsJSON["ddlSpecialityID"]) : 0;

                objEdiReports = BLLBillingClaimObj.LoadEDIReports(0, MDVUtility.ToLong(model.Clearinghouse), false, model.FileName,
                    null, model.ReviewStatus, model.ReportTitle, model.EDIText, model.IsERADeleted, null, "",downloadDate, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage)
                    );

                dsEdiReports = objEdiReports.Data;
                if (objEdiReports.Data != null)
                {
                    if (dsEdiReports.Tables[dsEdiReports.EDIReports.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            EDIReportCount = dsEdiReports.Tables[dsEdiReports.EDIReports.TableName].Rows.Count,
                            iTotalDisplayRecords = dsEdiReports.EDIReports.Rows[0][dsEdiReports.EDIReports.RecordCountColumn.ColumnName],
                            EDIReportLoad_JSON = MDVUtility.JSON_DataTable(dsEdiReports.Tables[dsEdiReports.EDIReports.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            EDIReportCount = 0,
                            Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        EDIReportCount = 0,
                        Message = objEdiReports.Message
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
                return (JsonConvert.SerializeObject(response));
            }
        }
        /// <summary>
        /// Get Reports
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="clearingHouseId"></param>
        /// <returns></returns>
        public string GetLatestReports(EDIReportModel model)
        {
            try
            {

                var clearingHouseid = model.Clearinghouse;
                BLObject<DSEDIReports> objEdiReports = BLLBillingClaimObj.GetLatestReports(long.Parse(clearingHouseid));
                if (objEdiReports.Data != null)
                {
                    var dsEdiReports = objEdiReports.Data;

                    if (dsEdiReports.Tables[dsEdiReports.EDIReports.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            EDIReportCount = dsEdiReports.Tables[dsEdiReports.EDIReports.TableName].Rows.Count,
                            EDIReportLoad_JSON = MDVUtility.JSON_DataTable(dsEdiReports.Tables[dsEdiReports.EDIReports.TableName]),
                            Message = Common.AppPrivileges.Download_Success_Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            EDIReportCount = dsEdiReports.Tables[dsEdiReports.EDIReports.TableName].Rows.Count,
                            EDIReportLoad_JSON = MDVUtility.JSON_DataTable(dsEdiReports.Tables[dsEdiReports.EDIReports.TableName]),
                            Message = Common.AppPrivileges.No_New_Report_Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objEdiReports.Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string DeleteEDIReport(EDIReportModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.EDIReportID))
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
                    BLObject<string> obj = BLLBillingClaimObj.DeleteEDIReports(model.EDIReportID);
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

    }
}