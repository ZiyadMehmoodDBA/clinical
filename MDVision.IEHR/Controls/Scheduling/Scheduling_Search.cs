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
    public class Scheduling_Search
    {
        private BLLPatient BLLPatientObj = null;
        private BLLSchedule BLLScheduleObj = null;
        public Scheduling_Search()
        {
            BLLPatientObj = new BLLPatient();
            BLLScheduleObj = new BLLSchedule();
        }

        #region Singleton
        private static Scheduling_Search _obj = null;
        public static Scheduling_Search Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_Search();
            return _obj;
        }
        #endregion

        #region Private Functions

        private string ScheduleSearch(string fieldsJSON, int PageNumber, int RowsPerPage)
        {

            string chkSunday = "";
            string chkMonday = "";
            string chkTuesday = "";
            string chkWednesday = "";
            string chkThursday = "";
            string chkFriday = "";
            string chkSaturday = "";

            string chkDaysString = "";

            string chkBlocked = "";
            string chkBooked = "";
            string chkOverBook = "";
            string chkFull = "";
            string chkOpen = "";
            string proresbit = "";

            string chkStatusString = "";


            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj;
                if (SearchedfieldsJSON == null)
                    obj = BLLScheduleObj.SchedulingSearch(0, 0, 0, 0, 0, "", "", "", "", "", "", "", "", PageNumber, RowsPerPage);

                else
                {


                    if (SearchedfieldsJSON["rdProvider"] == true)
                        proresbit = "0";
                    if (SearchedfieldsJSON["rdResource"] == true)
                        proresbit = "1";
                    if (SearchedfieldsJSON["chkSunday"] == true)
                        chkSunday = "1";
                    else
                        chkSunday = "0";

                    if (SearchedfieldsJSON["chkMonday"] == true)
                        chkMonday = "2";
                    else
                        chkMonday = "0";

                    if (SearchedfieldsJSON["chkTuesday"] == true)
                        chkTuesday = "3";
                    else
                        chkTuesday = "0";

                    if (SearchedfieldsJSON["chkWednesday"] == true)
                        chkWednesday = "4";
                    else
                        chkWednesday = "0";

                    if (SearchedfieldsJSON["chkThursday"] == true)
                        chkThursday = "5";
                    else
                        chkThursday = "0";

                    if (SearchedfieldsJSON["chkFriday"] == true)
                        chkFriday = "6";
                    else
                        chkFriday = "0";

                    if (SearchedfieldsJSON["chkSaturday"] == true)
                        chkSaturday = "7";
                    else
                        chkSaturday = "0";

                    chkDaysString = (chkSunday + "," + chkMonday + "," + chkTuesday + "," + chkWednesday + "," + chkThursday + "," + chkFriday + "," + chkSaturday);



                    if (SearchedfieldsJSON["chkBlocked"] == true)
                        chkBlocked = "1";
                    else
                        chkBlocked = "0";

                    if (SearchedfieldsJSON["chkBooked"] == true)
                        chkBooked = "2";
                    else
                        chkBooked = "0";

                    if (SearchedfieldsJSON["chkOverBook"] == true)
                        chkOverBook = "3";
                    else
                        chkOverBook = "0";

                    if (SearchedfieldsJSON["chkFull"] == true)
                        chkFull = "4";
                    else
                        chkFull = "0";

                    if (SearchedfieldsJSON["chkOpen"] == true)
                        chkOpen = "5";
                    else
                        chkOpen = "0";

                    chkStatusString = (chkBlocked + "," + chkBooked + "," + chkOverBook + "," + chkFull + "," + chkOpen);


                    obj = BLLScheduleObj.SchedulingSearch(MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientid"]), MDVUtility.ToInt64(SearchedfieldsJSON["hfFacilityId"]), 0, MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]), MDVUtility.ToInt64(SearchedfieldsJSON["hfResource"]), SearchedfieldsJSON["dpfromDate"], SearchedfieldsJSON["dptoDate"], SearchedfieldsJSON["tpFromTime"], SearchedfieldsJSON["tpToTime"], SearchedfieldsJSON["time1"], chkStatusString, chkDaysString, proresbit, PageNumber, RowsPerPage);
                }

                dsSchedule = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSchedule.Tables[dsSchedule.TimeSlotsDetail.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ScheduleSearchCount = dsSchedule.Tables[dsSchedule.TimeSlotsDetail.TableName].Rows.Count,
                            iTotalDisplayRecords = dsSchedule.TimeSlotsDetail.Rows[0][dsSchedule.TimeSlotsDetail.RecordCountColumn.ColumnName],
                            ScheduleSearch_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.TimeSlotsDetail.TableName]),
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ScheduleSearchCount = 0,
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
                        ScheduleSearchCount = 0,
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
                            { "txtAccountNo", MDVUtility.ToStr(dr[dsPatient.Patients.AccountNumberColumn.ColumnName]) + " - " + (MDVUtility.ToStr(dr[dsPatient.Patients.FullNameColumn.ColumnName]).Trim())},
                            //{ "txtFullName", MDVUtility.ToStr(dr[dsPatient.Patients.FullNameColumn.ColumnName])},
                            ////{ "ddlReferringProvider", MDVUtility.ToStr(dr[dsPatient.Patients.ReferringProviderIdColumn.ColumnName])},
                            ////{ "ddlInsurancePlan", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName])},
                            //{ "dtpDOB", MDVUtility.ToStr(dr[dsPatient.Patients.DOBColumn.ColumnName])},
                            { "hfPatientid", MDVUtility.ToStr(dr[dsPatient.Patients.PatientIdColumn.ColumnName])},
                          
                        };

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientFill_JSON = js.Serialize(keyValues),
                                //PatientInsurance_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientInsurance.TableName]),
                                //Patient_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName])
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

        #endregion

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_BATCH_SCHEDULING":
                    {
                        string fieldsJSON = context.Request["SchedulingSearchData"];
                        string PageNumber = MDVUtility.ToStr(context.Request["PageNumber"]);
                        string RowsPerPage = MDVUtility.ToStr(context.Request["RowsPerPage"]);
                        string strJSONData = ScheduleSearch(fieldsJSON, MDVUtility.ToInt32(PageNumber), MDVUtility.ToInt32((RowsPerPage)));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT":
                    {
                        string PatientID = context.Request["PatientID"];

                        string strJSONData = FillPatient(MDVUtility.ToInt64(PatientID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                //case "CHANGE_SCH_FACILITY":
                //    {

                //        string SlotDtlIds = MDVUtility.ToStr(context.Request["SlotDtlIds"]);
                //        Int64 MoveFacilityId = MDVUtility.ToInt64(context.Request["MoveFacilityId"]);
                //        string strJSONData = ChangeSchFacility(SlotDtlIds, MoveFacilityId);

                //        context.Response.ContentType = "text/plain";
                //        context.Response.Write(strJSONData);
                //    }
                //    break;
            }
        }

        #endregion
    }
}