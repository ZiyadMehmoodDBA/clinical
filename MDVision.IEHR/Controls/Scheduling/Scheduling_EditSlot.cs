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
    public class Scheduling_EditSlot
    {
        private BLLSchedule BLLScheduleObj = null;
        public Scheduling_EditSlot()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Scheduling_EditSlot _obj = null;
        public static Scheduling_EditSlot Instance()
        {
            if (_obj == null)
                _obj = new Scheduling_EditSlot();
            return _obj;
        }
        #endregion

        #region "Private Functions"
        private string SelectSchSlotDetail(Int64 TimeslotDetailid, Int64 ProviderId, Int64 ResourceId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(TimeslotDetailid)))
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
                    DSAppointment dsSchedule = null;
                    BLObject<DSAppointment> obj = BLLScheduleObj.LoadScheduleSlot(TimeslotDetailid, ProviderId, ResourceId);
                    if (obj.Data != null)
                    {
                        dsSchedule = obj.Data;
                        if (dsSchedule.Tables[dsSchedule.TimeSlotsDetail.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsSchedule.Tables[dsSchedule.TimeSlotsDetail.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                             { "dpDate", MDVUtility.ToDateTime(dr[dsSchedule.TimeSlotsDetail.ScheduleDateColumn.ColumnName]).ToString("MM/dd/yyyy")},
                             { "txtTime", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.FromTimeSlotsColumn.ColumnName])},
                             { "txtMinutes", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.SlotMinutesColumn.ColumnName])},
                             { "txtProvider", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.ProviderNameColumn.ColumnName])},
                             { "txtResource", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.ResourceNameColumn.ColumnName])},
                             { "txtPatientAllowed", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.PatientAllowedColumn.ColumnName])},
                             { "txtFacility", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.FacilityNameColumn.ColumnName])},
                             { "chkOverbookAllowed", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.OverBookAllowedColumn.ColumnName])},
                             { "ddlReason", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.ScheduleReasonIdColumn.ColumnName])},
                             { "txtBooked", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.PatientBookedColumn.ColumnName])},
                             { "txtComments", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.CommentsColumn.ColumnName])},
                             { "hfSchReasonId", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.ScheduleReasonIdColumn.ColumnName])},
                             { "txtSchReason", MDVUtility.ToStr(dr[dsSchedule.TimeSlotsDetail.ReasonsColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                SchSlotDetailsCount = dsSchedule.Tables[dsSchedule.SchAppointment.TableName].Rows.Count,
                                SchSlotDetail_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.SchAppointment.TableName]),
                                SchSlot_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.TimeSlotsDetail.TableName]),
                                SchSlotDetailFill_JSON = js.Serialize(keyValues)
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
        private string UpdateScheduleSlot(string fieldsJSON, Int64 TimeSlotDtlId, Int64 ProviderId, Int64 ResourceId)
        {
            //try
            //{
            //    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            //    var UpdatefieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

            //    DSAppointment dsSchSlot = new DSAppointment();
            //    DSAppointment.TimeSlotsDetailRow dr = dsSchSlot.TimeSlotsDetail.NewTimeSlotsDetailRow();

            //    dr.TimeSlotDtlId = TimeSlotDtlId;
            //    dr.PatientAllowed = Convert.ToInt32(MDVUtility.ToStr(UpdatefieldsJSON["txtPatientAllowed"]));
            //    dr.OverBookAllowed = MDVUtility.ToStr(UpdatefieldsJSON["chkOverbookAllowed"]) == "True" ? true : false;
            //    if (!string.IsNullOrEmpty(UpdatefieldsJSON["ddlReason"]))
            //    {
            //        dr.BlockReasonId = Convert.ToInt32(MDVUtility.ToStr(UpdatefieldsJSON["ddlReason"]));
            //    }
            //    dr.Comments = MDVUtility.ToStr(UpdatefieldsJSON["txtComments"]);

            //    #region Database Updation
            //    dsSchSlot.TimeSlotsDetail.AddTimeSlotsDetailRow(dr);
            //    dsSchSlot.TimeSlotsDetail.AcceptChanges();

            //    DSAppointment dsSchedule = null;
            //    //BLObject<DSAppointment> obj1 =BLLScheduleObj.LoadScheduleSlot(TimeSlotDtlId);
            //    //dsSchedule = obj1.Data;
            //    //if (dsSchedule.Tables[dsSchedule.SchAppointment.TableName].Rows.Count <= dr.PatientAllowed)
            //    //{
            //        if (dsSchSlot.Tables[dsSchSlot.TimeSlotsDetail.TableName].Rows.Count > 0)
            //        {

            //            dsSchSlot.TimeSlotsDetail.Rows[0].SetModified();
            //            BLObject<DSAppointment> obj =BLLScheduleObj.UpdateSchSlot(dr.TimeSlotDtlId, dr.PatientAllowed, dr.OverBookAllowed, dr.BlockReasonId, dr.Comments);
            //            if (obj.Data != null)
            //            {
            //                var response = new
            //                {
            //                    status = true,
            //                    message = Common.AppPrivileges.Update_Message
            //                };
            //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //            }
            //            else
            //            {
            //                var response = new
            //                {
            //                    status = false,
            //                    Message = obj.Message
            //                };
            //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            //            }
            //        }

            //        else
            //        {
            //            var response = new
            //            {
            //                status = false,
            //                Message = ""
            //            };
            //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            //        }
            //   // }

            //    //else
            //    //{
            //    //    var response = new
            //    //    {
            //    //        status = false,
            //    //        Message = "Patient Allowed Must be Greater than Patient Booked"
            //    //    };
            //    //    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            //    //}

            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    var response = new
            //    {
            //        status = false,
            //        Message =MDVCustomException.HumanReadableMessage(ex.Message),
            //    };
            //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            //}

            Int32 PatientAllowed = 0;
            bool OverBookAllowed = true;
            Int32 BlockReasonId = 0;
            string Comments = "";
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var searchedfieldsJson = ser.Deserialize<dynamic>(fieldsJSON);
                DSAppointment dsSchSlot = null;
                BLObject<DSAppointment> obj = BLLScheduleObj.LoadScheduleSlot(TimeSlotDtlId, ProviderId, ResourceId);
                dsSchSlot = obj.Data;
                if (dsSchSlot.Tables[dsSchSlot.TimeSlotsDetail.TableName].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsSchSlot.Tables[dsSchSlot.TimeSlotsDetail.TableName].Rows)
                    {
                        if (searchedfieldsJson.ContainsKey("txtPatientAllowed"))
                            PatientAllowed = Convert.ToInt32(MDVUtility.ToStr(searchedfieldsJson["txtPatientAllowed"]));

                        if (searchedfieldsJson.ContainsKey("hfSchReasonId") && !string.IsNullOrEmpty(searchedfieldsJson["hfSchReasonId"]))
                            BlockReasonId = Convert.ToInt32(MDVUtility.ToStr(searchedfieldsJson["hfSchReasonId"]));
                        else
                            BlockReasonId = 0;
                        if (searchedfieldsJson.ContainsKey("txtComments"))
                            Comments = MDVUtility.ToStr(searchedfieldsJson["txtComments"]);

                        OverBookAllowed = MDVUtility.ToStr(searchedfieldsJson["chkOverbookAllowed"]) == "True" ? true : false;

                    }
                    BLObject<DSAppointment> objType = BLLScheduleObj.UpdateSchSlot(TimeSlotDtlId, PatientAllowed, OverBookAllowed, BlockReasonId, Comments);
                    dsSchSlot = objType.Data;
                    if (objType.Data != null)
                    {
                        if (dsSchSlot.Tables[dsSchSlot.TimeSlotsDetail.TableName].Rows.Count > 0 && dsSchSlot.Tables[dsSchSlot.TimeSlotsDetail.TableName].Rows[0][dsSchSlot.TimeSlotsDetail.ErrorMessageColumn.ColumnName].ToString() != "")
                        {
                            var response = new
                            {
                                status = false,
                                Message = dsSchSlot.Tables[dsSchSlot.TimeSlotsDetail.TableName].Rows[0][dsSchSlot.TimeSlotsDetail.ErrorMessageColumn.ColumnName].ToString()
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objType.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
                case "SELECT_SLOT_DETAIL":
                    {
                        string TimeslotDetailid = MDVUtility.ToStr(context.Request["TimeslotDetailid"]);
                        string ProviderId = MDVUtility.ToStr(context.Request["ProviderId"]);
                        string ResourceId = MDVUtility.ToStr(context.Request["ResourceId"]);
                        string strJSONData = SelectSchSlotDetail(MDVUtility.ToInt64(TimeslotDetailid), MDVUtility.ToInt64(ProviderId), MDVUtility.ToInt64(ResourceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_SCH_SLOT":
                    {
                        string fieldsJSON = context.Request["SlotData"];
                        Int64 TimeSlotDtlId = MDVUtility.ToInt64(context.Request["TimeSlotDtlId"]);
                        string ProviderId = MDVUtility.ToStr(context.Request["ProviderId"]);
                        string ResourceId = MDVUtility.ToStr(context.Request["ResourceId"]);

                        string strJSONData = UpdateScheduleSlot(fieldsJSON, TimeSlotDtlId, MDVUtility.ToInt64(ProviderId), MDVUtility.ToInt64(ResourceId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}