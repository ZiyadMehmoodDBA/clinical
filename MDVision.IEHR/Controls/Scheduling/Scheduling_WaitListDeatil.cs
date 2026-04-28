using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Scheduling
{
    public class Scheduling_WaitListDeatil
    {
        private BLLPatient BLLPatientObj = null;
        private BLLSchedule BLLScheduleObj = null;
        public Scheduling_WaitListDeatil()
        {
            BLLPatientObj = new BLLPatient();
            BLLScheduleObj = new BLLSchedule();
        }


        #region Singleton
        private static Scheduling_WaitListDeatil _obj = null;


        public static Scheduling_WaitListDeatil Instance()
        {
            if (_obj == null)
            {
                _obj = new Scheduling_WaitListDeatil();

            }
            return _obj;
        }
        #endregion





        #region Private Functions
        public string SearchPatients(string AccountNo)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj;
                obj = BLLPatientObj.LookupPatient(0, AccountNo, "1");
                dsPatient = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count,
                            PatientLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = 0,
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
                            //{ "txtAccount", MDVUtility.ToStr(dr[dsPatient.Patients.AccountNumberColumn.ColumnName])},
                            { "txtAccount", MDVUtility.ToStr(dr[dsPatient.Patients.AccountNumberColumn.ColumnName]) + " - " + (MDVUtility.ToStr(dr[dsPatient.Patients.FullNameColumn.ColumnName]))},
                            { "hfPatientid", MDVUtility.ToStr(dr[dsPatient.Patients.PatientIdColumn.ColumnName])},
                          
                        };

                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientFill_JSON = js.Serialize(keyValues),
                                //PatientInsurance_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientInsurance.TableName]),
                                Patient_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName])
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string SaveWaitList(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSAppointment dsAppointment = new DSAppointment();
                DSAppointment.WaitListRow dr = dsAppointment.WaitList.NewWaitListRow();

                dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientid"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]))
                {
                    dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfResource"]))
                {
                    dr.ResourceId = MDVUtility.ToInt64(SearchedfieldsJSON["hfResource"]);
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                {
                    dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfRefProvider"]))
                {
                    dr.RefProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfRefProvider"]);
                }

                //if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSchReasonId"]))
                //{
                //    dr.ScheduleReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["hfSchReasonId"]);
                //}
                dr[dsAppointment.WaitList.ScheduleReasonIdColumn] = DBNull.Value;
                dr.ReasonName = MDVUtility.ToStr(SearchedfieldsJSON["txtSchReason"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlStatus"]))
                {
                    dr.WtStatusId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlStatus"]);
                }
                dr.FromDate = MDVUtility.ToDateTime(SearchedfieldsJSON["fromDate"]);
                dr.ToDate = MDVUtility.ToDateTime(SearchedfieldsJSON["toDate"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["preferredDate"]))
                {
                    dr.PreferredDate = MDVUtility.ToDateTime(SearchedfieldsJSON["preferredDate"]);
                }
                dr.PAN = MDVUtility.ToStr(SearchedfieldsJSON["txtPAN"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPreferredTime"]))
                {
                    dr.PrfTimeId = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPreferredTime"]);
                }
                dr.Comments = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                dr.IsActive = 1;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;


                if (SearchedfieldsJSON["rdWeekDay"] == true)
                {
                    dr.IsPreferredDay = MDVUtility.ToStr('1');

                    dr.PreferredDay = (SearchedfieldsJSON["chkSaturday"] + "," + SearchedfieldsJSON["chkSunday"] + "," + SearchedfieldsJSON["chkMonday"] + "," + SearchedfieldsJSON["chkTuesday"] + "," + SearchedfieldsJSON["chkWednesday"] + "," + SearchedfieldsJSON["chkThursday"] + "," + SearchedfieldsJSON["chkFriday"]).Replace("True", "1").Replace("False", "0");

                }
                else if (SearchedfieldsJSON["rdAnyDay"] == true)
                {
                    dr.IsPreferredDay = MDVUtility.ToStr('2');
                    dr.PreferredDay = MDVUtility.ToStr("0,0,0,0,0,0,0");

                }

                else if (SearchedfieldsJSON["rdCustom"] == true)
                {
                    dr.IsPreferredDay = MDVUtility.ToStr('3');
                    dr.PreferredDay = (SearchedfieldsJSON["chkSaturday"] + "," + SearchedfieldsJSON["chkSunday"] + "," + SearchedfieldsJSON["chkMonday"] + "," + SearchedfieldsJSON["chkTuesday"] + "," + SearchedfieldsJSON["chkWednesday"] + "," + SearchedfieldsJSON["chkThursday"] + "," + SearchedfieldsJSON["chkFriday"]).Replace("True", "1").Replace("False", "0");
                }

                #region Database Insertion
                dsAppointment.WaitList.AddWaitListRow(dr);
                BLObject<DSAppointment> obj = BLLScheduleObj.InsertWaitList(dsAppointment);
                dsAppointment = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        WaitListId = dsAppointment.Tables[dsAppointment.WaitList.TableName].Rows[0][dsAppointment.WaitList.WaitListIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                #endregion
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
        private string FillWaitList(Int64 WaitListId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(WaitListId)))
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
                    DSAppointment dsAppointment = null;
                    BLObject<DSAppointment> obj = BLLScheduleObj.LoadWaitList(WaitListId, 0, 0, 0, 0, 0, 0, null, null);
                    if (obj.Data != null)
                    {
                        dsAppointment = obj.Data;
                        string prefDate = "";
                        if (dsAppointment.Tables[dsAppointment.WaitList.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsAppointment.Tables[dsAppointment.WaitList.TableName].Rows[0];
                            if (dr[dsAppointment.WaitList.PreferredDateColumn.ColumnName].ToString() != "")
                            {
                                prefDate = MDVUtility.ToDateTime(dr[dsAppointment.WaitList.PreferredDateColumn.ColumnName]).ToString("MM/dd/yyyy");
                            }
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtAccount", MDVUtility.ToStr(dr[dsAppointment.WaitList.AccountNumberColumn.ColumnName]) +" - "+ MDVUtility.ToStr(dr[dsAppointment.WaitList.PatientNameColumn.ColumnName])},
                            { "hfPatientid", MDVUtility.ToStr(dr[dsAppointment.WaitList.PatientIdColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsAppointment.WaitList.FacilityNameColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsAppointment.WaitList.FacilityIdColumn.ColumnName])},
                            { "txtProvider", MDVUtility.ToStr(dr[dsAppointment.WaitList.ProviderNameColumn.ColumnName])},
                            { "hfProvider", MDVUtility.ToStr(dr[dsAppointment.WaitList.ProviderIdColumn.ColumnName])},

                            { "txtResource", MDVUtility.ToStr(dr[dsAppointment.WaitList.ResourceNameColumn.ColumnName])},
                            { "hfResource", MDVUtility.ToStr(dr[dsAppointment.WaitList.ResourceIdColumn.ColumnName])},
                            { "ddlReason", MDVUtility.ToStr(dr[dsAppointment.WaitList.ScheduleReasonIdColumn.ColumnName])},
                            { "fromDate", MDVUtility.ToDateTime(dr[dsAppointment.WaitList.FromDateColumn.ColumnName]).ToString("MM/dd/yyyy")},
                            { "toDate", MDVUtility.ToDateTime(dr[dsAppointment.WaitList.ToDateColumn.ColumnName]).ToString("MM/dd/yyyy")},

                            { "preferredDate", prefDate},

                            { "txtRefProvider", MDVUtility.ToStr(dr[dsAppointment.WaitList.RefProvNameColumn.ColumnName])},
                            { "hfRefProvider", MDVUtility.ToStr(dr[dsAppointment.WaitList.RefProviderIdColumn.ColumnName])},
                            { "txtPAN", MDVUtility.ToStr(dr[dsAppointment.WaitList.PANColumn.ColumnName])},
                            { "ddlStatus", MDVUtility.ToStr(dr[dsAppointment.WaitList.WtStatusIdColumn.ColumnName])},
                            { "ddlPreferredTime", MDVUtility.ToStr(dr[dsAppointment.WaitList.PrfTimeIdColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsAppointment.WaitList.CommentsColumn.ColumnName])},
                            { "txtSchReason", MDVUtility.ToStr(dr[dsAppointment.WaitList.ReasonNameColumn.ColumnName])},
                            { "hfSchReasonId", MDVUtility.ToStr(dr[dsAppointment.WaitList.ScheduleReasonIdColumn.ColumnName])}
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                WaitListFill_JSON = js.Serialize(keyValues),
                                WaitListDetail_JSON = MDVUtility.JSON_DataTable(dsAppointment.Tables[dsAppointment.WaitList.TableName])
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
                            Message = obj.Message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string UpdateWaitList(string fieldsJSON, Int64 WaitListId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSAppointment dsWaitList = null;
                BLObject<DSAppointment> objWaitList = BLLScheduleObj.LoadWaitList(WaitListId, 0, 0, 0, 0, 0, 0, null, null);
                dsWaitList = objWaitList.Data;
                foreach (DSAppointment.WaitListRow dr in dsWaitList.Tables[dsWaitList.WaitList.TableName].Rows)
                {
                    dr[dsWaitList.WaitList.PatientIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientid"]);

                    if (SearchedfieldsJSON.ContainsKey("hfProvider"))
                    {
                        dr[dsWaitList.WaitList.ProviderIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("hfResource"))
                    {
                        dr[dsWaitList.WaitList.ResourceIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["hfResource"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("hfFacility"))
                    {
                        dr[dsWaitList.WaitList.FacilityIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("hfRefProvider"))
                    {
                        dr[dsWaitList.WaitList.RefProviderIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["hfRefProvider"]);
                    }

                    //if (SearchedfieldsJSON.ContainsKey("hfSchReasonId"))
                    //{
                    //    dr[dsWaitList.WaitList.ScheduleReasonIdColumn] = MDVUtility.ToInt64(SearchedfieldsJSON["hfSchReasonId"]);
                    //}
                    dr[dsWaitList.WaitList.ScheduleReasonIdColumn] = DBNull.Value;
                    dr.ReasonName = MDVUtility.ToStr(SearchedfieldsJSON["txtSchReason"]);
                    if (SearchedfieldsJSON.ContainsKey("ddlStatus"))
                    {
                        dr[dsWaitList.WaitList.WtStatusIdColumn] = MDVUtility.ToInt32(SearchedfieldsJSON["ddlStatus"]);
                    }
                    dr[dsWaitList.WaitList.FromDateColumn] = MDVUtility.ToDateTime(SearchedfieldsJSON["fromDate"]);
                    dr[dsWaitList.WaitList.ToDateColumn] = MDVUtility.ToDateTime(SearchedfieldsJSON["toDate"]);
                    if (SearchedfieldsJSON.ContainsKey("preferredDate"))
                    {
                        if (SearchedfieldsJSON["preferredDate"] == "")
                        {
                            dr[dsWaitList.WaitList.PreferredDateColumn] = DBNull.Value;
                        }
                        else
                        {
                            dr[dsWaitList.WaitList.PreferredDateColumn] = MDVUtility.ToDateTime(SearchedfieldsJSON["preferredDate"]);
                        }
                    }
                    dr[dsWaitList.WaitList.PANColumn] = MDVUtility.ToStr(SearchedfieldsJSON["txtPAN"]);
                    if (SearchedfieldsJSON.ContainsKey("ddlPreferredTime"))
                    {
                        dr[dsWaitList.WaitList.PrfTimeIdColumn] = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPreferredTime"]);
                    }
                    dr[dsWaitList.WaitList.CommentsColumn] = MDVUtility.ToStr(SearchedfieldsJSON["txtComments"]);
                    dr[dsWaitList.WaitList.IsActiveColumn] = 1;
                    //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    //dr.CreatedOn = DateTime.Now;
                    dr[dsWaitList.WaitList.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr[dsWaitList.WaitList.ModifiedOnColumn] = DateTime.Now;


                    if (SearchedfieldsJSON["rdWeekDay"] == true)
                    {
                        dr[dsWaitList.WaitList.IsPreferredDayColumn] = MDVUtility.ToStr('1');

                        dr[dsWaitList.WaitList.PreferredDayColumn] = (SearchedfieldsJSON["chkSaturday"] + "," + SearchedfieldsJSON["chkSunday"] + "," + SearchedfieldsJSON["chkMonday"] + "," + SearchedfieldsJSON["chkTuesday"] + "," + SearchedfieldsJSON["chkWednesday"] + "," + SearchedfieldsJSON["chkThursday"] + "," + SearchedfieldsJSON["chkFriday"]).Replace("True", "1").Replace("False", "0");

                    }
                    else if (SearchedfieldsJSON["rdAnyDay"] == true)
                    {
                        dr[dsWaitList.WaitList.IsPreferredDayColumn] = MDVUtility.ToStr('2');
                        dr[dsWaitList.WaitList.PreferredDayColumn] = MDVUtility.ToStr("0,0,0,0,0,0,0");

                    }

                    else if (SearchedfieldsJSON["rdCustom"] == true)
                    {
                        dr[dsWaitList.WaitList.IsPreferredDayColumn] = MDVUtility.ToStr('3');
                        dr[dsWaitList.WaitList.PreferredDayColumn] = (SearchedfieldsJSON["chkSaturday"] + "," + SearchedfieldsJSON["chkSunday"] + "," + SearchedfieldsJSON["chkMonday"] + "," + SearchedfieldsJSON["chkTuesday"] + "," + SearchedfieldsJSON["chkWednesday"] + "," + SearchedfieldsJSON["chkThursday"] + "," + SearchedfieldsJSON["chkFriday"]).Replace("True", "1").Replace("False", "0");
                    }
                }

                #region Database Updation

                if (dsWaitList.Tables[dsWaitList.WaitList.TableName].Rows.Count > 0)
                {
                    //dsWaitList.WaitList.Rows[0].SetModified();
                    BLObject<DSAppointment> obj = BLLScheduleObj.UpdateWaitList(dsWaitList);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ""
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
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
        private string DeleteWaitList(Int64 WaitListId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(WaitListId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLScheduleObj.DeleteWaitList(MDVUtility.ToStr(WaitListId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string WaitListIsActive(Int64 WaitListId, Int64 IsActive)
        {
            try
            {
                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj = BLLScheduleObj.LoadWaitList(WaitListId, 0, 0, 0, 0, 0, 0, null, null);
                dsSchedule = obj.Data;
                if (dsSchedule.Tables[dsSchedule.WaitList.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsSchedule.Tables[dsSchedule.WaitList.TableName].Rows[0];
                    dr[dsSchedule.WaitList.IsActiveColumn.ColumnName] = IsActive;

                    BLObject<DSAppointment> objWaitList = BLLScheduleObj.UpdateWaitList(dsSchedule);
                    string successMsg;
                    if (objWaitList.Data != null)
                    {
                        if (IsActive == 0)
                            successMsg = Common.AppPrivileges.Inactive_Message;
                        else
                            successMsg = Common.AppPrivileges.Active_Message;
                        var response = new
                        {
                            status = true,
                            message = successMsg
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objWaitList.Message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

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

            string proresbit = "";
            string timestring = "";

            string status = "";
            string fromdate = "";
            string todate = "";

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

                    if (SearchedfieldsJSON["rdWeekDay"] == true)
                    {
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
                    }
                    else if (SearchedfieldsJSON["rdAnyDay"] == true)
                    {
                        chkDaysString = "0,0,0,0,0,0,0";
                    }

                    else if (SearchedfieldsJSON["rdCustom"] == true)
                    {
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
                    }


                    if (SearchedfieldsJSON["ddlPreferredTime_text"] == "AfterNoon")
                    {
                        timestring = "PM";
                    }
                    else if (SearchedfieldsJSON["ddlPreferredTime_text"] == "- SELECT -")
                    {
                        timestring = "";
                    }

                    else if (SearchedfieldsJSON["ddlPreferredTime_text"] == "Any Time")
                    {
                        timestring = "";
                    }
                    else if (SearchedfieldsJSON["ddlPreferredTime_text"] == "Evening")
                    {
                        timestring = "PM";
                    }
                    else if (SearchedfieldsJSON["ddlPreferredTime_text"] == "Morning")
                    {
                        timestring = "AM";
                    }

                    status = "0,0,0,0,0";

                    if (SearchedfieldsJSON["preferredDate"] == "")
                    {
                        fromdate = SearchedfieldsJSON["fromDate"];
                        todate = SearchedfieldsJSON["toDate"];

                    }
                    else if (SearchedfieldsJSON["preferredDate"] != "")
                    {
                        fromdate = SearchedfieldsJSON["preferredDate"];
                        todate = SearchedfieldsJSON["preferredDate"];
                    }

                    if (SearchedfieldsJSON["rdPrefDate"] == true)
                    {
                        fromdate = SearchedfieldsJSON["preferredDate"];
                        todate = SearchedfieldsJSON["preferredDate"];
                    }
                    else if (SearchedfieldsJSON["rdFrmToDate"] == true)
                    {
                        fromdate = SearchedfieldsJSON["fromDate"];
                        todate = SearchedfieldsJSON["toDate"];

                    }
                    obj = BLLScheduleObj.SchedulingSearch(0, MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]), 0, MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]), MDVUtility.ToInt64(SearchedfieldsJSON["hfResource"]), fromdate, todate, "", "", timestring, status, chkDaysString, proresbit, PageNumber, RowsPerPage);
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
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

                case "SEARCH_PATIENT":
                    {
                        string AccountNo = context.Request["AccountNo"];

                        string strJSONData = SearchPatients(AccountNo);

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
                case "SAVE_WAITLIST":
                    {
                        string fieldsJSON = context.Request["WaitListData"];
                        string strJSONData = SaveWaitList(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_WAITLIST":
                    {
                        string strWaitListId = context.Request["WaitListId"];
                        string strJSONData = FillWaitList(MDVUtility.ToInt64(strWaitListId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_WAITLIST":
                    {
                        string fieldsJSON = context.Request["WaitListData"];
                        Int64 WaitListId = MDVUtility.ToInt64(context.Request["WaitListId"]);
                        string strJSONData = UpdateWaitList(fieldsJSON, WaitListId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_WAITLIST":
                    {
                        string strWaitListId = context.Request["WaitListId"];
                        string strJSONData = DeleteWaitList(MDVUtility.ToInt64(strWaitListId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_WAITLIST_ACTIVE_INACTIVE":
                    {
                        Int64 WaitListId = MDVUtility.ToInt64(context.Request["WaitListId"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = WaitListIsActive(WaitListId, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
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
            }
        }

        #endregion
    }
}