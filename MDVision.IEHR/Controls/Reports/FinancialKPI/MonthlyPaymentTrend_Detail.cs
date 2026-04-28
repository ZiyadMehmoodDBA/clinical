using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Model.Report;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Reports.FinancialKPI
{
    public class MonthlyPaymentTrend_Detail
    {
        private BLLReports BLLReportObj = null;
        public MonthlyPaymentTrend_Detail()
        {
            BLLReportObj = new BLLReports();
        }
        #region Singleton
        private static MonthlyPaymentTrend_Detail _obj = null;
        public static MonthlyPaymentTrend_Detail Instance()
        {
            if (_obj == null)
                _obj = new MonthlyPaymentTrend_Detail();
            return _obj;
        }
        #endregion
        #region Private Functions
     /// <summary>
     /// Monthly Payment Trend Detail
     /// </summary>
     /// <param name="ProviderId"></param>
     /// <param name="ProviderName"></param>
     /// <param name="ClaimDateFrom"></param>
     /// <param name="ClaimDateTo"></param>
     /// <param name="EntityId"></param>
     /// <returns></returns>
        private string LoadMonthlyPaymentTrendDetail(string ProviderId, string ProviderName, string ClaimDate)
        {
            try
            {
                DateTime firstDayOfMonth = DateTime.Now , lastDayOfMonth=DateTime.Now;
                ClaimDate = ClaimDate.Trim('\'');
                if (!string.IsNullOrEmpty(ClaimDate))
                {
                    string[] ClaimDateArray = ClaimDate.Split('-');
                    string MonthName = ClaimDateArray[0].Trim('\'');
                    string year = ClaimDateArray[1].Trim('\'');
                    int months = DateTime.ParseExact(MonthName, "MMM", CultureInfo.CurrentCulture).Month;
                    firstDayOfMonth = new DateTime(MDVUtility.ToInt32(year), months, 1);
                    lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                }
                string EntityId = MDVSession.Current.EntityId;
                List<ProviderMonthlyPayment> MonthlyPaymentTrendList = null;
                BLObject<List<ProviderMonthlyPayment>> objMonthlyPaymentTrend;
                objMonthlyPaymentTrend = BLLReportObj.LoadMonthlyPaymentTrendDetail(ProviderId, ProviderName,   firstDayOfMonth.ToShortDateString(), lastDayOfMonth.ToShortDateString(), EntityId);
                MonthlyPaymentTrendList = objMonthlyPaymentTrend.Data;
                if (objMonthlyPaymentTrend.Data != null)
                {
                    if (MonthlyPaymentTrendList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MonthlyPaymentTrendListCount = MonthlyPaymentTrendList.Count,

                            MonthlyPaymentTrendListInfo_JSON = MonthlyPaymentTrendList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            UnAllocatedCopayListRecordCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objMonthlyPaymentTrend.Message
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
        /// Handle the Basic Free Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_MONTHTLY_PAYMENT_TREND":
                    {
                        string ProviderId = context.Request["ProviderId"];
                        string ProviderName = context.Request["ProviderName"];
                        string ClaimDate = context.Request["ClaimDate"];
                      
                   
                        string strJSONData = LoadMonthlyPaymentTrendDetail(ProviderId, ProviderName, ClaimDate);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}