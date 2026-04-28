using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.IEHR.Common;

namespace MDVision.IEHR.Controls.Admin
{
    public class Admin_ResourceSchedule_Detail
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_ResourceSchedule_Detail()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Admin_ResourceSchedule_Detail _obj = null;
        public static Admin_ResourceSchedule_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_ResourceSchedule_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions
        private string SaveResourceSchedule(string fieldsJSON, string fieldsJSON1)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Resource Schedule", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    var SearchedfieldsJSON1 = ser.Deserialize<dynamic>(fieldsJSON1);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    DSScheduleSetup.ResourceScheduleRow dr = dsSchedule.ResourceSchedule.NewResourceScheduleRow();

                    dr.ResourceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlresource"]);
                    dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlfacility"]);
                    dr.FromDate = MDVUtility.ToDateTime(SearchedfieldsJSON["fromResourceDate"]);
                    dr.ToDate = MDVUtility.ToDateTime(SearchedfieldsJSON["todate"]);
                    dr.FromTime = MDVUtility.ToStr(SearchedfieldsJSON["frmtime"]);
                    dr.ToTime = MDVUtility.ToStr(SearchedfieldsJSON["totime"]);
                    //   dr.SlotMinutes = MDVUtility.ToStr(SearchedfieldsJSON["slotminutes"]);
                    //  dr.PatinetAllowed = MDVUtility.ToInt(SearchedfieldsJSON["PatientAllowed"]);


                    //   dr.OverBookedAllowed = MDVUtility.ToStr(SearchedfieldsJSON["Overbookallow"]) == "True" ? true : false;
                    //dr.ScheduleReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlschreason"]);
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSchReasonId"]))
                    //    dr.ScheduleReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["hfSchReasonId"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["frmblckhrstime"]))
                        dr.BlockTimeFrom = MDVUtility.ToStr(SearchedfieldsJSON["frmblckhrstime"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["toblckhrstime"]))
                        dr.BlockTimeTo = MDVUtility.ToStr(SearchedfieldsJSON["toblckhrstime"]);


                    //dr.BlockReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["blockreason"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfBlckReasonId"]))
                        dr.BlockReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["hfBlckReasonId"]);

                    if (SearchedfieldsJSON1["pattern"] == "Daily")
                    {

                        if (SearchedfieldsJSON1["Every"] == true && SearchedfieldsJSON1["chkweekendsonly"] == false)
                        {
                            dr.SchFor = "Daily";
                            dr.PatternEvery = true;
                            dr.Value = MDVUtility.ToStr(SearchedfieldsJSON1["txtevrydays"]);
                            dr.PatternDays = "0";
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = "0";
                        }
                        if (SearchedfieldsJSON1["Every"] == true && SearchedfieldsJSON1["chkweekendsonly"] == true)
                        {
                            dr.SchFor = "Daily";
                            dr.PatternEvery = true;
                            dr.Value = MDVUtility.ToStr(SearchedfieldsJSON1["txtevrydays"]);
                            dr.PatternDays = "11111";
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = "0";
                        }
                        if (SearchedfieldsJSON1["rdevryweekend"] == true)
                        {
                            dr.SchFor = "Daily";
                            dr.PatternEvery = false;
                            dr.Value = "0";
                            dr.PatternDays = "0";
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = "0";
                        }

                    }
                    if (SearchedfieldsJSON1["pattern"] == "Weekly")
                    {
                        if (SearchedfieldsJSON1["rdEveryWeekdayOn"] == true)
                        {
                            dr.SchFor = "Weekly";
                            dr.PatternEvery = false;
                            dr.Value = "0";
                            dr.PatternDays = (SearchedfieldsJSON1["chkwekSunday"] + "," + SearchedfieldsJSON1["chkwekMonday"] + "," + SearchedfieldsJSON1["chkwekTuesday"] + "," + SearchedfieldsJSON1["chkwekWednesday"] + "," + SearchedfieldsJSON1["chkwekThursday"] + "," + SearchedfieldsJSON1["chkwekFriday"] + "," + SearchedfieldsJSON1["chkwekSaturday"]).Replace("True", "1").Replace("False", "0");
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = "0";
                        }
                        if (SearchedfieldsJSON1["rdEveryweekely"] == true)
                        {
                            dr.SchFor = "Weekly";
                            dr.PatternEvery = true;
                            dr.Value = MDVUtility.ToStr(SearchedfieldsJSON1["txtactiveweek"]);
                            dr.PatternDays = (SearchedfieldsJSON1["chkwekSunday"] + "," + SearchedfieldsJSON1["chkwekMonday"] + "," + SearchedfieldsJSON1["chkwekTuesday"] + "," + SearchedfieldsJSON1["chkwekWednesday"] + "," + SearchedfieldsJSON1["chkwekThursday"] + "," + SearchedfieldsJSON1["chkwekFriday"] + "," + SearchedfieldsJSON1["chkwekSaturday"]).Replace("True", "1").Replace("False", "0");
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = "0";
                        }
                    }
                    if (SearchedfieldsJSON1["pattern"] == "Monthly")
                    {
                        if (SearchedfieldsJSON1["rdmntdaay"] == true)
                        {
                            dr.SchFor = "Monthly";
                            dr.PatternEvery = false;
                            dr.Value = MDVUtility.ToStr(SearchedfieldsJSON1["txtmntActive"]);
                            dr.PatternDays = "0";
                            dr.PatternWeeks = "0";
                            dr.PatternMonths = MDVUtility.ToStr(SearchedfieldsJSON1["txtmntofevry"]);
                        }
                        if (SearchedfieldsJSON1["rdmntthe"] == true)
                        {
                            dr.SchFor = "Monthly";
                            dr.PatternEvery = true;
                            dr.Value = "0";
                            dr.PatternDays = (SearchedfieldsJSON1["chkmntSunday"] + "," + SearchedfieldsJSON1["chkmntMonday"] + "," + SearchedfieldsJSON1["chkmntTuesday"] + "," + SearchedfieldsJSON1["chkmntWednesday"] + "," + SearchedfieldsJSON1["chkmntThursday"] + "," + SearchedfieldsJSON1["chkmntFriday"] + "," + SearchedfieldsJSON1["chkmntSaturday"]).Replace("True", "1").Replace("False", "0");
                            dr.PatternWeeks = (SearchedfieldsJSON1["chkmntFirst"] + "," + SearchedfieldsJSON1["chkmntSecond"] + "," + SearchedfieldsJSON1["chkmntThird"] + "," + SearchedfieldsJSON1["chkmntFourth"] + "," + SearchedfieldsJSON1["chkmntLast"]).Replace("True", "1").Replace("False", "0");
                            dr.PatternMonths = MDVUtility.ToStr(SearchedfieldsJSON1["txtmnttheofevery"]);
                        }
                    }
                    if (SearchedfieldsJSON1["pattern"] == "Yearly")
                    {
                        if (SearchedfieldsJSON1["rdyerEvery"] == true)
                        {
                            dr.SchFor = "Yearly";
                            dr.PatternEvery = false;
                            dr.Value = MDVUtility.ToStr(SearchedfieldsJSON1["monthDays"]);
                            dr.PatternDays = "0";
                            dr.PatternWeeks = "0";
                            if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "January")
                                dr.PatternMonths = "1";
                            else if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "February")
                                dr.PatternMonths = "2";
                            else if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "March")
                                dr.PatternMonths = "3";
                            else if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "April")
                                dr.PatternMonths = "4";
                            else if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "May")
                                dr.PatternMonths = "5";
                            else if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "June")
                                dr.PatternMonths = "6";
                            else if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "July")
                                dr.PatternMonths = "7";
                            else if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "August")
                                dr.PatternMonths = "8";
                            else if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "September")
                                dr.PatternMonths = "9";
                            else if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "October")
                                dr.PatternMonths = "10";
                            else if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "November")
                                dr.PatternMonths = "11";
                            else if (MDVUtility.ToStr(SearchedfieldsJSON1["yearMonth"]) == "Decmber")
                                dr.PatternMonths = "12";
                        }
                        if (SearchedfieldsJSON1["rdyerthe"] == true)
                        {
                            dr.SchFor = "Yearly";
                            dr.PatternEvery = true;
                            dr.Value = "0";
                            dr.PatternDays = (SearchedfieldsJSON1["chkyerSunday"] + "," + SearchedfieldsJSON1["chkyerMonday"] + "," + SearchedfieldsJSON1["chkyerTuesday"] + "," + SearchedfieldsJSON1["chkyerWednesday"] + "," + SearchedfieldsJSON1["chkyerThursday"] + "," + SearchedfieldsJSON1["chkyerFriday"] + "," + SearchedfieldsJSON1["chkyerSaturday"]).Replace("True", "1").Replace("False", "0");
                            dr.PatternWeeks = (SearchedfieldsJSON1["chkyerFirst"] + "," + SearchedfieldsJSON1["chkyerSecond"] + "," + SearchedfieldsJSON1["chkyerThird"] + "," + SearchedfieldsJSON1["chkyerFourth"] + "," + SearchedfieldsJSON1["chkyerLast"]).Replace("True", "1").Replace("False", "0");
                            dr.PatternMonths = (SearchedfieldsJSON1["chkyerJanuary"] + "," + SearchedfieldsJSON1["chkyerFebruary"] + "," + SearchedfieldsJSON1["chkyerMarch"] + "," + SearchedfieldsJSON1["chkyerApril"] + "," + SearchedfieldsJSON1["chkyerMay"] + "," + SearchedfieldsJSON1["chkyerJune"] + "," + SearchedfieldsJSON1["chkyerJuly"] + "," + SearchedfieldsJSON1["chkyerAugust"] + "," + SearchedfieldsJSON1["chkyerSeptember"] + "," + SearchedfieldsJSON1["chkyerOctober"] + "," + SearchedfieldsJSON1["chkyerNovember"] + "," + SearchedfieldsJSON1["chkyerDecember"]).Replace("True", "1").Replace("False", "0");
                        }
                    }

                    dr.isActive = true;

                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;



                    #region Database Insertion
                    dsSchedule.ResourceSchedule.AddResourceScheduleRow(dr);
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.InsertResourceSchedule(dsSchedule);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ScheduleId = dsSchedule.Tables[dsSchedule.ResourceSchedule.TableName].Rows[0][dsSchedule.ResourceSchedule.ResScheduleIdColumn.ColumnName]
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

        private string FillResourceSchedule(Int64 ResScheduleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Resource Schedule", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ResScheduleId)))
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
                        DSScheduleSetup dsSchedule = null;
                        //dsProfile = DALSpecialty.Instance.LoadSpecialty(SpecialtyId, null, null);
                        BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadResourceSchedule(ResScheduleId, null, null);
                        if (obj.Data != null)
                        {
                            dsSchedule = obj.Data;
                            if (dsSchedule.Tables[dsSchedule.ResourceSchedule.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsSchedule.Tables[dsSchedule.ResourceSchedule.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "ddlresource", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.ResourceSchedule.ResourceIdColumn.ColumnName]))},
                            { "ddlfacility", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.ResourceSchedule.FacilityIdColumn.ColumnName]))},
                            { "fromResourceDate", MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsSchedule.ResourceSchedule.FromDateColumn.ColumnName]).ToShortDateString())},
                            { "todate", MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsSchedule.ResourceSchedule.ToDateColumn.ColumnName]).ToShortDateString())},
                            { "frmtime", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.FromTimeColumn.ColumnName])},
                            { "totime", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.ToTimeColumn.ColumnName])},
                            { "frmblckhrstime", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.BlockTimeFromColumn.ColumnName])},
                            { "toblckhrstime", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.BlockTimeToColumn.ColumnName])},
                            { "slotminutes", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.SlotMinutesColumn.ColumnName])},
                            { "PatientAllowed", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.ResourceSchedule.PatinetAllowedColumn.ColumnName]))},
                            { "blockreason", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.ResourceSchedule.BlockReasonIdColumn.ColumnName]))},
                            { "Overbookallow", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.OverBookedAllowedColumn.ColumnName])},

                            { "schfor", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.SchForColumn.ColumnName])},
                            { "patternevery", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.PatternEveryColumn.ColumnName])},
                            { "value", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.ValueColumn.ColumnName])},
                            { "patterdays", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.PatternDaysColumn.ColumnName])},
                            { "patterweeks", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.PatternWeeksColumn.ColumnName])},
                            { "pattermonths", MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.PatternMonthsColumn.ColumnName])},
                            //{ "ddlschreason", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.ResourceSchedule.ScheduleReasonIdColumn.ColumnName]))},
                            { "txtSchReason", MDVUtility.ToStr(MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.ScheduleReasonColumn.ColumnName]))},
                            { "txtBlckReason", MDVUtility.ToStr(MDVUtility.ToStr(dr[dsSchedule.ResourceSchedule.BlockReasonColumn.ColumnName]))},


                        };
                                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                var response = new
                                {
                                    status = true,
                                    ResourceScheduleFill_JSON = js.Serialize(keyValues)
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

        private string DeleteResourceSchedule(Int64 ResScheduleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Resource Schedule", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ResScheduleId)))
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
                        BLObject<string> obj = BLLScheduleObj.DeleteResourceSchedule(MDVUtility.ToStr(ResScheduleId));
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

        private string UpdateResourceScheduleIsActive(Int64 ResScheduleId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Resource Schedule", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadResourceSchedule(ResScheduleId, null, null);
                    dsSchedule = obj.Data;
                    if (dsSchedule.Tables[dsSchedule.ResourceSchedule.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsSchedule.Tables[dsSchedule.ResourceSchedule.TableName].Rows[0];
                        dr[dsSchedule.ResourceSchedule.isActiveColumn.ColumnName] = IsActive;

                        BLObject<DSScheduleSetup> objResource = BLLScheduleObj.UpdateResourceSchedule(dsSchedule);
                        string successMsg;
                        if (objResource.Data != null)
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
                                Message = objResource.Message
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

        private string FillScheduleSchReasonDuration(Int64 ScheduleReasonId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ScheduleReasonId)))
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
                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadScheduleSchReasonDuration(ScheduleReasonId);
                    if (obj.Data != null)
                    {
                        dsSchedule = obj.Data;
                        if (dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsSchedule.Tables[dsSchedule.ScheduleReasons.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                           {"slotminutes", MDVUtility.ToStr(dr[dsSchedule.ScheduleReasons.DurationColumn.ColumnName])},

                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                ProviderScheduleFill_JSON = js.Serialize(keyValues)
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
                case "SAVE_RESOURCESCHEDULE":
                    {
                        string fieldsJSON = context.Request["ResourceScheduleData"];
                        string fieldsJSON1 = context.Request["ResourceSchedulePatternData"];
                        string strJSONData = SaveResourceSchedule(fieldsJSON, fieldsJSON1);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_RESOURCESCHEDULE":
                    {
                        string strResScheduleId = context.Request["ResScheduleID"];
                        string strJSONData = FillResourceSchedule(MDVUtility.ToInt64(strResScheduleId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_RESOURCESCHEDULE":
                    {
                        string strResScheduleId = context.Request["ResScheduleID"];
                        string strJSONData = DeleteResourceSchedule(MDVUtility.ToInt64(strResScheduleId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                //case "UPDATE_PROVIDERSCHEDULE":
                //    {
                //        string fieldsJSON = context.Request["ResourceScheduleData"];
                //        Int64 ScheduleID = MDVUtility.ToInt64(context.Request["ScheduleID"]);
                //        string strJSONData = UpdateResourceSchedule(fieldsJSON, ScheduleID);

                //        context.Response.ContentType = "text/plain";
                //        context.Response.Write(strJSONData);
                //    }
                //    break;
                case "UPDATE_RESOURCESCHEDULE_ACTIVE_INACTIVE":
                    {
                        Int64 ResScheduleID = MDVUtility.ToInt64(context.Request["ResScheduleID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateResourceScheduleIsActive(ResScheduleID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_SCHEDULEREASON_DURATION":
                    {
                        Int64 strScheduleReasonId = MDVUtility.ToInt64(context.Request["ScheduleReasonID"]);
                        string strJSONData = FillScheduleSchReasonDuration(MDVUtility.ToInt64(strScheduleReasonId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

        #endregion
    }
}