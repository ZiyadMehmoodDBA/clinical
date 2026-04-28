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
    public class Admin_ProviderSchedule_Detail
    {
        private BLLSchedule BLLScheduleObj = null;
        public Admin_ProviderSchedule_Detail()
        {
            BLLScheduleObj = new BLLSchedule();
        }
        #region Singleton
        private static Admin_ProviderSchedule_Detail _obj = null;
        public static Admin_ProviderSchedule_Detail Instance()
        {
            if (_obj == null)
                _obj = new Admin_ProviderSchedule_Detail();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Saves the provider schedule.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="fieldsJSON1">The fields jso n1.</param>
        /// <returns>System.String.</returns>
        private string SaveProviderSchedule(string fieldsJSON, string fieldsJSON1)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider Schedule", "ADD")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                    var SearchedfieldsJSON1 = ser.Deserialize<dynamic>(fieldsJSON1);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    DSScheduleSetup.ProviderScheduleRow dr = dsSchedule.ProviderSchedule.NewProviderScheduleRow();

                    dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlprovider"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlfacility"]))
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlfacility"]);

                    dr.FromDate = MDVUtility.ToDateTime(SearchedfieldsJSON["fromProviderDate"]);
                    dr.ToDate = MDVUtility.ToDateTime(SearchedfieldsJSON["todate"]);
                    dr.FromTime = MDVUtility.ToStr(SearchedfieldsJSON["frmtime"]);
                    dr.ToTime = MDVUtility.ToStr(SearchedfieldsJSON["totime"]);
                    // dr.SlotMinutes = MDVUtility.ToStr(SearchedfieldsJSON["slotminutes"]);
                    //  dr.PatinetAllowed = MDVUtility.ToInt(SearchedfieldsJSON["PatientAllowed"]);


                    //    dr.OverBookedAllowed = MDVUtility.ToStr(SearchedfieldsJSON["Overbookallow"]) == "True" ? true : false; 

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["frmblckhrstime"]))
                        dr.BlockTimeFrom = MDVUtility.ToStr(SearchedfieldsJSON["frmblckhrstime"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["toblckhrstime"]))
                        dr.BlockTimeTo = MDVUtility.ToStr(SearchedfieldsJSON["toblckhrstime"]);


                    //hfBlckReasonId dr.BlockReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["blockreason"]);
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
                    //dr.ScheduleReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlschreason"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfSchReasonId"]))
                        dr.ScheduleReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["hfSchReasonId"]);

                    #region Database Insertion
                    dsSchedule.ProviderSchedule.AddProviderScheduleRow(dr);
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.InsertProviderSchedule(dsSchedule);
                    dsSchedule = obj.Data;
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            ScheduleId = dsSchedule.Tables[dsSchedule.ProviderSchedule.TableName].Rows[0][dsSchedule.ProviderSchedule.ScheduleIdColumn.ColumnName]
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

        /// <summary>
        /// Updates the provider schedule.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ScheduleId">The schedule identifier.</param>
        /// <returns>System.String.</returns>
        private string UpdateProviderSchedule(string fieldsJSON, Int64 ScheduleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider Schedule", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    DSScheduleSetup dsSchedule = new DSScheduleSetup();
                    DSScheduleSetup.ProviderScheduleRow dr = dsSchedule.ProviderSchedule.NewProviderScheduleRow();

                    // dr.ScheduleId = Convert.ToInt32(ScheduleId);
                    dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlprovider"]);

                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);

                    dr.FromDate = MDVUtility.ToDateTime(SearchedfieldsJSON["fromProviderDate"]);
                    dr.ToDate = MDVUtility.ToDateTime(SearchedfieldsJSON["todate"]);
                    dr.FromTime = MDVUtility.ToStr(SearchedfieldsJSON["frmtime"]);
                    dr.ToTime = MDVUtility.ToStr(SearchedfieldsJSON["totime"]);
                    //   dr.SlotMinutes = MDVUtility.ToStr(SearchedfieldsJSON["slotminutes"]);
                    //   dr.PatinetAllowed = MDVUtility.ToInt(SearchedfieldsJSON["PatientAllowed"]);
                    dr.BlockTimeFrom = MDVUtility.ToStr(SearchedfieldsJSON["frmblckhrstime"]);
                    dr.BlockTimeTo = MDVUtility.ToStr(SearchedfieldsJSON["toblckhrstime"]);
                    dr.BlockReasonId = MDVUtility.ToInt64(SearchedfieldsJSON["blockreason"]);
                    //   dr.OverBookedAllowed = MDVUtility.ToStr(SearchedfieldsJSON["Overbookallow"]) == "True" ? true : false;

                    dr.isActive = true;

                    //dr.CreatedBy = "";
                    //dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Updation
                    dsSchedule.ProviderSchedule.AddProviderScheduleRow(dr);
                    dsSchedule.ProviderSchedule.AcceptChanges();

                    if (dsSchedule.Tables[dsSchedule.ProviderSchedule.TableName].Rows.Count > 0)
                    {
                        dsSchedule.ProviderSchedule.Rows[0].SetModified();
                        BLObject<DSScheduleSetup> obj = BLLScheduleObj.UpdateProviderSchedule(dsSchedule);
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

        /// <summary>
        /// Fills the provider schedule.
        /// </summary>
        /// <param name="ScheduleId">The schedule identifier.</param>
        /// <returns>System.String.</returns>
        private string FillProviderSchedule(Int64 ScheduleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider Schedule", "VIEW")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ScheduleId)))
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
                        BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadProviderSchedule(ScheduleId, null, null);
                        if (obj.Data != null)
                        {
                            dsSchedule = obj.Data;
                            if (dsSchedule.Tables[dsSchedule.ProviderSchedule.TableName].Rows.Count > 0)
                            {
                                DataRow dr = dsSchedule.Tables[dsSchedule.ProviderSchedule.TableName].Rows[0];
                                var keyValues = new Dictionary<string, string>
                        {
                            { "ddlprovider", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.ProviderSchedule.ProviderIdColumn.ColumnName]))},
                            { "ddlfacility", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.FacilityIdColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.FacilityIdColumn.ColumnName])},
                            { "fromProviderDate", MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsSchedule.ProviderSchedule.FromDateColumn.ColumnName]).ToShortDateString())},
                            { "todate", MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsSchedule.ProviderSchedule.ToDateColumn.ColumnName]).ToShortDateString())},
                            { "frmtime", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.FromTimeColumn.ColumnName])},
                            { "totime", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.ToTimeColumn.ColumnName])},
                            { "frmblckhrstime", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.BlockTimeFromColumn.ColumnName])},
                            { "toblckhrstime", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.BlockTimeToColumn.ColumnName])},
                            { "slotminutes", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.SlotMinutesColumn.ColumnName])},
                            { "PatientAllowed", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.ProviderSchedule.PatinetAllowedColumn.ColumnName]))},
                            { "blockreason", MDVUtility.ToStr(MDVUtility.ToInt64(dr[dsSchedule.ProviderSchedule.BlockReasonIdColumn.ColumnName]))},
                            { "Overbookallow", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.OverBookedAllowedColumn.ColumnName])},

                            { "schfor", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.SchForColumn.ColumnName])},
                            { "patternevery", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.PatternEveryColumn.ColumnName])},
                            { "value", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.ValueColumn.ColumnName])},
                            { "patterdays", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.PatternDaysColumn.ColumnName])},
                            { "patterweeks", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.PatternWeeksColumn.ColumnName])},
                            { "pattermonths", MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.PatternMonthsColumn.ColumnName])},
                            { "txtSchReason", MDVUtility.ToStr(MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.ScheduleReasonColumn.ColumnName]))},
                            { "txtBlckReason", MDVUtility.ToStr(MDVUtility.ToStr(dr[dsSchedule.ProviderSchedule.BlockReasonColumn.ColumnName]))},


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

        /// <summary>
        /// Deletes the provider schedule.
        /// </summary>
        /// <param name="ScheduleId">The schedule identifier.</param>
        /// <returns>System.String.</returns>
        private string DeleteProviderSchedule(Int64 ScheduleId)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider Schedule", "DELETE")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    if (string.IsNullOrEmpty(MDVUtility.ToStr(ScheduleId)))
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
                        BLObject<string> obj = BLLScheduleObj.DeleteProviderSchedule(MDVUtility.ToStr(ScheduleId));
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

        /// <summary>
        /// Updates the provider schedule is active.
        /// </summary>
        /// <param name="ScheduleId">The schedule identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns>System.String.</returns>
        private string UpdateProviderScheduleIsActive(Int64 ScheduleId, Int64 IsActive)
        {
            try
            {
                string privilegesMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Provider Schedule", "EDIT")).ToString();
                if (string.IsNullOrEmpty(privilegesMessage))
                {
                    DSScheduleSetup dsSchedule = null;
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadProviderSchedule(ScheduleId, null, null);
                    dsSchedule = obj.Data;
                    if (dsSchedule.Tables[dsSchedule.ProviderSchedule.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsSchedule.Tables[dsSchedule.ProviderSchedule.TableName].Rows[0];
                        dr[dsSchedule.ProviderSchedule.isActiveColumn.ColumnName] = IsActive;

                        BLObject<DSScheduleSetup> objSpecialty = BLLScheduleObj.UpdateProviderSchedule(dsSchedule);
                        string successMsg;
                        if (objSpecialty.Data != null)
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
                                Message = objSpecialty.Message
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

        private string FillScheduleReasonDuration(Int64 ScheduleReasonId)
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
                    BLObject<DSScheduleSetup> obj = BLLScheduleObj.LoadScheduleReasonDuration(ScheduleReasonId);
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

        //#region Service Command Handler
        // <summary>
        // Handle the Specialty Detail Commands and call to the respective methods.
        // </summary>
        // <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_PROVIDERSCHEDULE":
                    {
                        string fieldsJSON = context.Request["ProviderScheduleData"];
                        string fieldsJSON1 = context.Request["ProviderSchedulePatternData"];
                        string strJSONData = SaveProviderSchedule(fieldsJSON, fieldsJSON1);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PROVIDERSCHEDULE":
                    {
                        string strScheduleId = context.Request["ScheduleID"];
                        string strJSONData = FillProviderSchedule(MDVUtility.ToInt64(strScheduleId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PROVIDERSCHEDULE":
                    {
                        string strScheduleId = context.Request["ScheduleID"];
                        string strJSONData = DeleteProviderSchedule(MDVUtility.ToInt64(strScheduleId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PROVIDERSCHEDULE":
                    {
                        string fieldsJSON = context.Request["ProviderScheduleData"];
                        Int64 ScheduleID = MDVUtility.ToInt64(context.Request["ScheduleID"]);
                        string strJSONData = UpdateProviderSchedule(fieldsJSON, ScheduleID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PROVIDERSCHEDULE_ACTIVE_INACTIVE":
                    {
                        Int64 ScheduleID = MDVUtility.ToInt64(context.Request["ScheduleID"]);
                        Int64 IsActive = MDVUtility.ToInt64(context.Request["IsActive"]);
                        string strJSONData = UpdateProviderScheduleIsActive(ScheduleID, IsActive);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "FILL_SCHEDULEREASON_DURATION":
                    {
                        Int64 strScheduleReasonId = MDVUtility.ToInt64(context.Request["ScheduleReasonID"]);
                        string strJSONData = FillScheduleReasonDuration(MDVUtility.ToInt64(strScheduleReasonId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }

    }
}