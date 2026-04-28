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

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_WaitList
    {
        BLLPatient BLLPatientObj = null;
        private BLLSchedule BLLScheduleObj = null;

        public Scheduling_WaitList()
        {
            BLLPatientObj = new BLLPatient();
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Scheduling_WaitList _obj = null;
        public static Scheduling_WaitList Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_WaitList();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string FillPatient(Int64 PatientId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.FillPatientAndInsuranceById(PatientId, 0, 0, "Appointment");
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;

                        if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtAccount", MDVUtility.ToStr(dr[dsPatient.Patients.AccountNumberColumn.ColumnName])},
                            { "hfpatientid", MDVUtility.ToStr(dr[dsPatient.Patients.PatientIdColumn.ColumnName])},
                          
                        };

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientFill_JSON = js.Serialize(keyValues),
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
        private string LoadWaitList(string fieldsJSON, Int64 WaitListId, int PageNumber, int RowsPerPage)
        {

            string facility;
            string provider;
            string resource;
            string preftime;
            string appstatus;

            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (SearchedfieldsJSON.ContainsKey("ddlFacility"))
                {
                    if (SearchedfieldsJSON["ddlFacility"] == "")
                        facility = "0";
                    else
                        facility = SearchedfieldsJSON["ddlFacility"];
                }
                else
                    facility = "0";

                if (SearchedfieldsJSON.ContainsKey("ddlProvider"))
                {
                    if (SearchedfieldsJSON["ddlProvider"] == "")
                        provider = "0";
                    else
                        provider = SearchedfieldsJSON["ddlProvider"];
                }
                else
                    provider = "0";

                if (SearchedfieldsJSON.ContainsKey("ddlResource"))
                {
                    if (SearchedfieldsJSON["ddlResource"] == "")
                        resource = "0";
                    else
                        resource = SearchedfieldsJSON["ddlResource"];
                }
                else
                    resource = "0";

                if (SearchedfieldsJSON.ContainsKey("ddlPreferredTime"))
                {
                    if (SearchedfieldsJSON["ddlPreferredTime"] == "")
                        preftime = "0";
                    else
                        preftime = SearchedfieldsJSON["ddlPreferredTime"];
                }
                else
                    preftime = "0";

                if (SearchedfieldsJSON.ContainsKey("ddlAppstatus"))
                {
                    if (SearchedfieldsJSON["ddlAppstatus"] == "")
                        appstatus = "0";
                    else
                        appstatus = SearchedfieldsJSON["ddlAppstatus"];
                }
                else
                    appstatus = "0";

                DateTime? dtpref = String.IsNullOrEmpty(SearchedfieldsJSON["dpPrefDate"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpPrefDate"]);
                DateTime? dtfrom = String.IsNullOrEmpty(SearchedfieldsJSON["dpFromDate"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpFromDate"]);
                DateTime? dtto = String.IsNullOrEmpty(SearchedfieldsJSON["dpToDate"]) ? (DateTime?)null : DateTime.Parse(SearchedfieldsJSON["dpToDate"]);

                //long WaitListId, long PatientId, long FacilityId, long ProviderId, long ResourceId, int PrfTimeId, int WtListStatusId, DateTime? FromDate = null, DateTime? PreferredDate = null

                DSAppointment dsWaitList = null;
                BLObject<DSAppointment> obj;
                if (SearchedfieldsJSON == null)
                    obj = BLLScheduleObj.LoadWaitList(WaitListId, 0, 0, 0, 0, 0, 0, null, null);
                else
                    obj = BLLScheduleObj.LoadWaitList(WaitListId, MDVUtility.ToInt64(SearchedfieldsJSON["hfpatientid"]), MDVUtility.ToInt64(facility), MDVUtility.ToInt64(provider), MDVUtility.ToInt64(resource), MDVUtility.ToInt32(preftime), MDVUtility.ToInt32(appstatus), dtfrom, dtpref, dtto, PageNumber, RowsPerPage);
                dsWaitList = obj.Data;

                if (obj.Data != null)
                {
                    if (dsWaitList.Tables[dsWaitList.WaitList.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            WaitListCount = dsWaitList.Tables[dsWaitList.WaitList.TableName].Rows.Count,
                            iTotalDisplayRecords = dsWaitList.WaitList.Rows[0][dsWaitList.WaitList.RecordCountColumn.ColumnName],
                            WaitListLoad_JSON = MDVUtility.JSON_DataTable(dsWaitList.Tables[dsWaitList.WaitList.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            WaitListCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        WaitListCount = 0,
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
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {

                case "FILL_PATIENT":
                    {
                        string PatientID = context.Request["PatientID"];

                        string strJSONData = FillPatient(MDVUtility.ToInt64(PatientID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_WAIT_LIST":
                    {
                        string fieldsJSON = context.Request["WaitListData"];
                        Int64 WaitListId = MDVUtility.ToInt64(context.Request["WaitListId"]);
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = LoadWaitList(fieldsJSON, WaitListId, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32((RowsPerPage)));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}