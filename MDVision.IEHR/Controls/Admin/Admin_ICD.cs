using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using Newtonsoft.Json;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_ICD
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_ICD()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }
        #region Singleton
        private static Admin_ICD _obj = null;
        public static Admin_ICD Instance()
        {
            if (_obj == null)
                _obj = new Admin_ICD();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the icd.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ICDID">The icdid.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string LoadICD(string fieldsJSON, Int64 ICDID, Int64 pageNo, Int64 rpp)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ICD", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCodes = null;
                    BLObject<DSCodes> obj;
                    if (SearchedfieldsJSON == null || ICDID != 0)
                        obj = BLLAdminCodesObj.LoadICD(ICDID, null, null, null, null, null, null, null, null, pageNo, rpp);
                    else
                        //obj =BLLAdminCodesObj.LoadICD(ICDID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["txtICD9"], SearchedfieldsJSON["ddlActive"], SearchedfieldsJSON["ddlEntity"], SearchedfieldsJSON["ddlICDType"], pageNo, rpp);
                        obj = BLLAdminCodesObj.LoadICD(ICDID, null, SearchedfieldsJSON["txtDescription"], SearchedfieldsJSON["txtICD9"], SearchedfieldsJSON["txtICD10Description"], SearchedfieldsJSON["txtICD10"], SearchedfieldsJSON["txtSNOMEDDescription"], SearchedfieldsJSON["txtSNOMED"], SearchedfieldsJSON["ddlActive"], pageNo, rpp);
                    dsCodes = obj.Data;
                    if (obj.Data != null)
                    {
                        if (dsCodes.Tables[dsCodes.ICD.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                ICDCount = dsCodes.Tables[dsCodes.ICD.TableName].Rows.Count,
                                iTotalDisplayRecords = dsCodes.ICD.Rows[0][dsCodes.ICD.RecordCountColumn.ColumnName],
                                ICDLoad_JSON = MDVUtility.JSON_DataTable(dsCodes.Tables[dsCodes.ICD.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                ICDCount = 0,
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = privilegesMessage
                    };
                    return JsonConvert.SerializeObject(response);
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
        /// Handle the ICD Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_ICD":
                    {
                        string fieldsJSON = context.Request["ICDData"];
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        Int64 ICDID = MDVUtility.ToInt64(context.Request["ICDID"]);
                        string strJSONData = LoadICD(fieldsJSON, ICDID, pageNo, rpp);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}