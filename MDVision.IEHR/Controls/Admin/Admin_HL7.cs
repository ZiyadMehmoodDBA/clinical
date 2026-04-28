using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.IEHR.Common;
using Newtonsoft.Json;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_HL7
    {
        private BLLAdminHL7 BLLAdminHL7Obj = null;
        public Admin_HL7()
        {
            BLLAdminHL7Obj = new BLLAdminHL7();
        }
        #region Singleton
        private static Admin_HL7 _obj = null;
        public static Admin_HL7 Instance()
        {
            if (_obj == null)
                _obj = new Admin_HL7();
            return _obj;
        }
        #endregion
        #region Private Functions

        /// <summary>
        /// LoadHL7SIU
        /// </summary>
        /// <param name="fieldsJson"></param>
        /// <param name="MirthLogID"></param>
        /// <param name="rpp"></param>
        /// <param name="PageNumber"></param>
        /// <returns></returns>
        private string LoadHL7SIU(string fieldsJson, Int64 MirthLogID, int rpp, int PageNumber)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                DSHL7 dsHL7SIU = null;
                BLObject<DSHL7> obj;

                var txtMRNumber = searchedfieldsJson["txtMRNumber"] == "" ? null : searchedfieldsJson["txtMRNumber"];
                var txtFacility = searchedfieldsJson["txtFacility"] == "" ? null : searchedfieldsJson["txtFacility"];
                var txtProvider = searchedfieldsJson["txtProvider"] == "" ? null : searchedfieldsJson["txtProvider"];
                var ddlStatus = searchedfieldsJson["ddlStatus"] == "" ? null : searchedfieldsJson["ddlStatus"];
                var txtErrorMessage = searchedfieldsJson["txtErrorMessage"] == "" ? null : searchedfieldsJson["txtErrorMessage"];
                var txtFileName = searchedfieldsJson["txtFileName"] == "" ? null : searchedfieldsJson["txtFileName"];
                var txtPatientName = searchedfieldsJson["txtPatientName"] == "" ? null : searchedfieldsJson["txtPatientName"];
                var txtAppStatus = searchedfieldsJson["txtAppStatus"] == "" ? null : searchedfieldsJson["txtAppStatus"];
                var CreatedOn = searchedfieldsJson["CreatedOn"] == "" ? null : searchedfieldsJson["CreatedOn"];
                //MDVSession.Current.EntityId;

                obj = BLLAdminHL7Obj.LoadHL7SIULog(0, txtMRNumber, txtPatientName, txtFacility, txtProvider, ddlStatus, txtErrorMessage, txtFileName, CreatedOn, txtAppStatus, rpp, PageNumber);

                dsHL7SIU = obj.Data;
                if (obj.Data != null)
                {
                    if (dsHL7SIU.Tables[dsHL7SIU.HL7SIU.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            HL7SIUCount = dsHL7SIU.Tables[dsHL7SIU.HL7SIU.TableName].Rows.Count,
                            iTotalDisplayRecords = dsHL7SIU.HL7SIU.Rows[0][dsHL7SIU.HL7SIU.RecordCountColumn],
                            HL7SIULoad_JSON = MDVUtility.JSON_DataTable(dsHL7SIU.Tables[dsHL7SIU.HL7SIU.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            iTotalDisplayRecords = 0,
                            Message = "No SIU Log Found."
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

        private string LoadHL7ADT(string fieldsJson, Int64 MirthLogID, int rpp, int PageNumber)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ADT", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    DSHL7 dsHL7ADT = null;
                    BLObject<DSHL7> obj;

                    var txtMRNumber = searchedfieldsJson["txtMRNumber"] == "" ? null : searchedfieldsJson["txtMRNumber"];
                    var txtPatientName = searchedfieldsJson["txtPatientName"] == "" ? null : searchedfieldsJson["txtPatientName"];
                    var txtFacility = searchedfieldsJson["txtFacility"] == "" ? null : searchedfieldsJson["txtFacility"];
                    var txtProvider = searchedfieldsJson["txtProvider"] == "" ? null : searchedfieldsJson["txtProvider"];
                    var ddlStatus = searchedfieldsJson["ddlStatus"] == "" ? null : searchedfieldsJson["ddlStatus"];
                    var txtErrorMessage = searchedfieldsJson["txtErrorMessage"] == "" ? null : searchedfieldsJson["txtErrorMessage"];
                    var txtFileName = searchedfieldsJson["txtFileName"] == "" ? null : searchedfieldsJson["txtFileName"];
                    var CreatedOn = searchedfieldsJson["CreatedOn"] == "" ? null : searchedfieldsJson["CreatedOn"];
                    var txtInsCompanyName = searchedfieldsJson["txtInsCompanyName"] == "" ? null : searchedfieldsJson["txtInsCompanyName"];

                    obj = BLLAdminHL7Obj.LoadHL7ADTLog(0, txtMRNumber, txtPatientName, txtFacility, txtProvider, ddlStatus, txtErrorMessage, txtFileName, CreatedOn, txtInsCompanyName, rpp, PageNumber);

                    dsHL7ADT = obj.Data;
                    if (obj.Data != null)
                    {
                        if (dsHL7ADT.Tables[dsHL7ADT.HL7ADT.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                HL7ADTCount = dsHL7ADT.Tables[dsHL7ADT.HL7ADT.TableName].Rows.Count,
                                iTotalDisplayRecords = dsHL7ADT.HL7ADT.Rows[0][dsHL7ADT.HL7ADT.RecordCountColumn],
                                HL7ADTLoad_JSON = MDVUtility.JSON_DataTable(dsHL7ADT.Tables[dsHL7ADT.HL7ADT.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                MessageCount = 0,
                                iTotalDisplayRecords = 0,
                                Message = "No ADT Log Found."
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

        private string LoadHL7DFT(string fieldsJson, Int64 MirthLogID, int rpp, int PageNumber)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("DFT", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJson);
                    DSHL7 dsHL7DFT = null;
                    BLObject<DSHL7> obj;

                    var txtMRNumber = searchedfieldsJson["txtMRNumber"] == "" ? null : searchedfieldsJson["txtMRNumber"];
                    var txtFacility = searchedfieldsJson["txtFacility"] == "" ? null : searchedfieldsJson["txtFacility"];
                    var txtProvider = searchedfieldsJson["txtProvider"] == "" ? null : searchedfieldsJson["txtProvider"];
                    var ddlStatus = searchedfieldsJson["ddlStatus"] == "" ? null : searchedfieldsJson["ddlStatus"];
                    var txtErrorMessage = searchedfieldsJson["txtErrorMessage"] == "" ? null : searchedfieldsJson["txtErrorMessage"];
                    var txtFileName = searchedfieldsJson["txtFileName"] == "" ? null : searchedfieldsJson["txtFileName"];
                    var txtPatientName = searchedfieldsJson["txtPatientName"] == "" ? null : searchedfieldsJson["txtPatientName"];
                    var txtAppStatus = searchedfieldsJson["txtAppStatus"] == "" ? null : searchedfieldsJson["txtAppStatus"];
                    var CreatedOn = searchedfieldsJson["CreatedOn"] == "" ? null : searchedfieldsJson["CreatedOn"];
                    //MDVSession.Current.EntityId;

                    obj = BLLAdminHL7Obj.LoadHL7DFTLog(0, txtMRNumber, txtPatientName, txtFacility, txtProvider, ddlStatus, txtErrorMessage, txtFileName, CreatedOn, txtAppStatus, rpp, PageNumber);

                    dsHL7DFT = obj.Data;
                    if (obj.Data != null)
                    {
                        if (dsHL7DFT.Tables[dsHL7DFT.HL7DFT.TableName].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                HL7DFTCount = dsHL7DFT.Tables[dsHL7DFT.HL7DFT.TableName].Rows.Count,
                                iTotalDisplayRecords = dsHL7DFT.HL7DFT.Rows[0][dsHL7DFT.HL7DFT.RecordCountColumn],
                                HL7DFTLoad_JSON = MDVUtility.JSON_DataTable(dsHL7DFT.Tables[dsHL7DFT.HL7DFT.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                MessageCount = 0,
                                iTotalDisplayRecords = 0,
                                Message = "No DFT Log Found."
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
        /// CommandHandler
        /// </summary>
        /// <param name="context"></param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_HL7_SIU":
                    {
                        string fieldsJSON = context.Request["HL7SIUData"];
                        Int64 MirthLogID = MDVUtility.ToInt64(context.Request["MirthLogID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadHL7SIU(fieldsJSON, MirthLogID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SEARCH_HL7_ADT":
                    {
                        string fieldsJSON = context.Request["HL7ADTData"];
                        Int64 MirthLogID = MDVUtility.ToInt64(context.Request["MirthLogID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadHL7ADT(fieldsJSON, MirthLogID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SEARCH_HL7_DFT":
                    {
                        string fieldsJSON = context.Request["HL7DFTData"];
                        Int64 MirthLogID = MDVUtility.ToInt64(context.Request["MirthLogID"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadHL7DFT(fieldsJSON, MirthLogID, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32(RowsPerPage));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


            }
        }

        #endregion
    }
}