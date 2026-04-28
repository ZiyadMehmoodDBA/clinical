
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.Common.Utilities;
using MDVision.IEHR.Controls.CommonControls;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using Newtonsoft.Json;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_CPTCode
    {
        private BLLAdminCodes BLLAdminCodesObj = null;
        public Admin_CPTCode()
        {
            BLLAdminCodesObj = new BLLAdminCodes();
        }


        #region Singleton
        private static Admin_CPTCode _obj = null;
        public static Admin_CPTCode Instance()
        {
            if (_obj == null)
                _obj = new Admin_CPTCode();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load all the specialities for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The specialty identifier.</param>
        /// <returns></returns>
        private string LoadCPTCode(string fieldsJSON, Int64 CPTCodeID, Int64 pageNo, Int64 rpp)
        {
            string chkDiscontinued = null;

            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CPT", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSCodes dsCPTCode = null;
                    BLObject<DSCodes> obj;
                    if (SearchedfieldsJSON == null)
                        obj = BLLAdminCodesObj.LoadCPTCode(CPTCodeID, null, null, null, null, null, null, null, null, pageNo, rpp);
                    else
                    {
                        if (SearchedfieldsJSON.ContainsKey("chkDiscontinued"))
                        {
                            if (SearchedfieldsJSON["chkDiscontinued"] == true)
                                chkDiscontinued = "true";
                            else
                                chkDiscontinued = "false";
                        }
                        else
                        {
                            if (SearchedfieldsJSON.ContainsKey("lstDiscontinued"))
                                chkDiscontinued = SearchedfieldsJSON["lstDiscontinued"];
                        }
                        obj = BLLAdminCodesObj.LoadCPTCode(CPTCodeID, null, SearchedfieldsJSON["txtCPTCode"], SearchedfieldsJSON["txtDiscription"], SearchedfieldsJSON["lstTOSId"], SearchedfieldsJSON["lstSpeciality"], SearchedfieldsJSON["ddlActive"], chkDiscontinued, null, pageNo, rpp);
                        //obj =BLLAdminCodesObj.LoadCPTCode(CPTCodeID, SearchedfieldsJSON["txtShortName"], SearchedfieldsJSON["txtCPTCode"], SearchedfieldsJSON["txtDiscription"], SearchedfieldsJSON["lstTOSId"], SearchedfieldsJSON["lstSpeciality"], chkDiscontinued, SearchedfieldsJSON["ddlEntity"], pageNo, rpp);
                    }
                    dsCPTCode = obj.Data;
                    if (obj.Data != null)
                    {
                        if (dsCPTCode.Tables[dsCPTCode.CPTCode.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                CPTCount = dsCPTCode.Tables[dsCPTCode.CPTCode.TableName].Rows.Count,
                                iTotalDisplayRecords = dsCPTCode.CPTCode.Rows[0][dsCPTCode.CPTCode.RecordCountColumn.ColumnName],
                                CPTLoad_JSON = MDVUtility.JSON_DataTable(dsCPTCode.Tables[dsCPTCode.CPTCode.TableName]),
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
                            CPTCodeCount = 0,
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

        private string LoadHPCSCode(string fieldsJSON, Int64 CPTCodeID, Int64 pageNo, Int64 rpp)
        {


            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSCodes dsHPCSCode = null;
                BLObject<DSCodes> obj;

                obj = BLLAdminCodesObj.LoadHPCSCode(SearchedfieldsJSON["txtCPTCode"]);

                dsHPCSCode = obj.Data;
                if (obj.Data != null)
                {
                    if (dsHPCSCode.Tables[dsHPCSCode.HPCSCode.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CPTCount = dsHPCSCode.Tables[dsHPCSCode.HPCSCode.TableName].Rows.Count,
                            iTotalDisplayRecords = 1,
                            CPTLoad_JSON = MDVUtility.JSON_DataTable(dsHPCSCode.Tables[dsHPCSCode.HPCSCode.TableName]),
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
                        CPTCodeCount = 0,
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
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Provider Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_CPT_CODE":
                    {
                        string fieldsJSON = context.Request["CPTCodeData"];
                        Int64 CPTCodeID = MDVUtility.ToInt64(context.Request["CPTCodeID"]);
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJSONData = LoadCPTCode(fieldsJSON, CPTCodeID, pageNo, rpp);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_HPCS_CODE":
                    {
                        string fieldsJSON = context.Request["CPTCodeData"];
                        Int64 CPTCodeID = MDVUtility.ToInt64(context.Request["CPTCodeID"]);
                        Int64 rpp = MDVUtility.ToInt64(context.Request["rpp"]);
                        Int64 pageNo = MDVUtility.ToInt64(context.Request["PageNo"]);
                        string strJSONData = LoadHPCSCode(fieldsJSON, CPTCodeID, pageNo, rpp);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}