using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Controls.CommonControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_IMOCPT
    {
         private BLLAdminCodes BLLAdminCodesObj = null;
         public Admin_IMOCPT()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }

        #region Singleton
        private static Admin_IMOCPT _obj = null;
        public static Admin_IMOCPT Instance()
        {
            return _obj ?? (_obj = new Admin_IMOCPT());
        }

        #endregion


        #region "Private Functions"

        private string LoadSCT_CPTs(string SCTSnomed)
        {
            try
            {
                DSCodes dsCodes = null;
                BLObject<DSCodes> obj;
                obj =BLLAdminCodesObj.LoadSCTProcedures(SCTSnomed);
                dsCodes = obj.Data;
                if (obj.Data != null)
                {
                    if (dsCodes.Tables[dsCodes.SCTProcedures.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CPTCount = dsCodes.Tables[dsCodes.SCTProcedures.TableName].Rows.Count,
                            //iTotalDisplayRecords = dsCodes.ICD.Rows[0][dsCodes.SCTProcedures.RecordCountColumn.ColumnName],
                            CPTLoad_JSON = MDVUtility.JSON_DataTable(dsCodes.Tables[dsCodes.SCTProcedures.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            CPTCount = 0,
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

        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the ICD Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_CPT":
                    {
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        string entityId = context.Request["entityID"];
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string cpt = context.Request["CPT"] == "" ? "0" : context.Request["CPT"];
                        string strJsonData = IMO.GetAllIMOCPTCodes(entityId, cpt, pageNo, rpp, false);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;

                case "SEARCH_SCT_CPT":
                    {

                        string SCTSnomed = context.Request["SCTSnomed"];
                        string strJsonData = LoadSCT_CPTs(SCTSnomed);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }
        #endregion
    }
}
