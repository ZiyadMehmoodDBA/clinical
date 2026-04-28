using MDVision.Business.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Model.CCM.Reports;
using MDVision.Business.BCommon;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Areas.CCM.Helpers
{
    public class CCM_ReportHelper
    {
        private BLLReports BLLReportObj = null;
        public CCM_ReportHelper()
        {
            BLLReportObj = new BLLReports();
        }
        private static CCM_ReportHelper _instance = null;
        public static CCM_ReportHelper Instance()
        {
            if (_instance == null)
                _instance = new CCM_ReportHelper();
            return _instance;
        }

        internal string Load_CCM_Report(CCM_ReportSearchModel model)
        {
            try
            {
                BLObject<List<CCM_ReportFillModel>> obj = BLLReportObj.Load_CCM_Report(model);
                List<CCM_ReportFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ccmCount = modelList.Count,
                        ccmList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        allergiesCount = 0,
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
    }
}