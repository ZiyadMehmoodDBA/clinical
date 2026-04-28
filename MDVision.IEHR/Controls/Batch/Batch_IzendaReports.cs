using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MDVision.Business.BCommon;
using MDVision.IEHR.Controls.Batch.CQM;
using MDVision.Common.Utilities;

namespace MDVision.IEHR.Controls.Batch.Izenda
{
    public class Batch_IzendaReports
    {
        
        #region Singleton

        private static Batch_IzendaReports _obj = null;

        public static Batch_IzendaReports Instance()
        {
            if (_obj == null)
                _obj = new Batch_IzendaReports();
            return _obj;
        }

        #endregion

        #region Service Command Handler

        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                    #region Catagory3

                case "SEARCH_CQM_MEASURES":
                {
                    var winScpPath = ConfigurationManager.AppSettings["IzendaURL"];

                    var fieldsJson = context.Request["BatchClinicalQualityMeasureData"];
                    var strJsonData = ";"; //LoadCqm(fieldsJson);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(strJsonData);
                }
                    break;

                    #endregion

                    #region Catagory1

                case "SEARCH_CQM_MEASURE_DETAILS":
                {
                    var fieldsJson = context.Request["ClinicalQualityMeasureDetailData"];
                    var cqmid = MDVUtility.ToStr(context.Request["CQMID"]);
                    var providerId = MDVUtility.ToStr(context.Request["providerId"]);
                    var dateFrom = MDVUtility.ToStr(context.Request["dateFrom"]);
                    var dateTo = MDVUtility.ToStr(context.Request["dateTo"]);

                    var strJsonData = ";"; //LoadCQM_Details(fieldsJson, cqmid, providerId, dateFrom, dateTo);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(strJsonData);
                }
                    break;

                    #endregion
            }
        }

        #endregion
    }
}
