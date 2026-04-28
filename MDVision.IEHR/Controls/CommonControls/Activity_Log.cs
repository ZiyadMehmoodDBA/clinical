using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using MDVision.IEHR.Common;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.CommonControls
{
    public class Activity_Log
    {
        private BLLPatient BLLPatientObj = null;
        private BLLBilling BLLBillingObj = null;
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        public Activity_Log()
        {
            BLLPatientObj = new BLLPatient();
            BLLBillingObj = new BLLBilling();
            BLLAdminSecurityObj = new BLLAdminSecurity();
        }
        #region Singleton
        private static Activity_Log _obj = null;
        public static Activity_Log Instance()
        {
            if (_obj == null)
                _obj = new Activity_Log();
            return _obj;
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Load all the LoadPatient for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The BasicFeeGroup identifier.</param>
        /// <returns>Json string containing Datatable or Exception message</returns>
        private string SearchPatientActivityLogs(String profilerName, string ColumnKeyId, string ProfileName, string DBAuditID, string PatientID, string VisitID, string Date, string ActivityType, string DBTableName, string ParentKeyColumnId = "")
        {
            try
            {
                DataSet dsPatientActivityLog = null;
                // string UserName = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                DateTime? createdDate = null;
                if (Date != "")
                    createdDate = MDVUtility.ToDateTime(Date);

                BLObject<DataSet> obj = BLLPatientObj.LoadPatientActivityLogs(profilerName, "", ColumnKeyId, ProfileName, DBAuditID, PatientID, VisitID, createdDate, ActivityType, DBTableName, ParentKeyColumnId);

                dsPatientActivityLog = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatientActivityLog.Tables.Contains("RequireSubmit") && dsPatientActivityLog.Tables["RequireSubmit"].Rows.Count > 0 && dsPatientActivityLog.Tables.Contains("NewEntry") && dsPatientActivityLog.Tables["NewEntry"].Rows.Count > 0)
                    {

                        if (dsPatientActivityLog.Tables["User"].Rows.Count > 0 && ActivityType.IndexOf("User") > -1)
                        {
                            var response = new
                            {
                                status = true,
                                RequireSubmitCount = dsPatientActivityLog.Tables["RequireSubmit"].Rows.Count,
                                RequireSubmit_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["RequireSubmit"]),
                                NewEntryCount = dsPatientActivityLog.Tables["NewEntry"].Rows.Count,
                                NewEntry_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["NewEntry"]),
                                UserCount = dsPatientActivityLog.Tables["User"].Rows.Count,
                                User_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["User"])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                RequireSubmitCount = dsPatientActivityLog.Tables["RequireSubmit"].Rows.Count,
                                RequireSubmit_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["RequireSubmit"]),
                                NewEntryCount = dsPatientActivityLog.Tables["NewEntry"].Rows.Count,
                                NewEntry_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["NewEntry"])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                    else if (ActivityType.IndexOf("NewEntry") > -1 && ActivityType.IndexOf("User") > -1)
                    {
                        var response = new
                        {
                            status = true,
                            NewEntryCount = dsPatientActivityLog.Tables["NewEntry"].Rows.Count,
                            NewEntry_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["NewEntry"]),
                            UserCount = dsPatientActivityLog.Tables["User"].Rows.Count,
                            User_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["User"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                        if (dsPatientActivityLog.Tables.Contains("NewEntry") && dsPatientActivityLog.Tables["NewEntry"].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                NewEntryCount = dsPatientActivityLog.Tables["NewEntry"].Rows.Count,
                                NewEntry_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["NewEntry"])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else if (dsPatientActivityLog.Tables.Contains("User") && dsPatientActivityLog.Tables["User"].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                UserCount = dsPatientActivityLog.Tables["User"].Rows.Count,
                                User_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["User"])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else if (dsPatientActivityLog.Tables.Contains("Field") && dsPatientActivityLog.Tables["Field"].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                FieldCount = dsPatientActivityLog.Tables["Field"].Rows.Count,
                                Field_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["Field"])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    if (dsPatientActivityLog.Tables.Contains("DeletedCharges") && dsPatientActivityLog.Tables["DeletedCharges"].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            DeletedChargesCount = dsPatientActivityLog.Tables["DeletedCharges"].Rows.Count,
                            DeletedCharges_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["DeletedCharges"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    if (dsPatientActivityLog.Tables.Contains("RequireSubmit") && dsPatientActivityLog.Tables["RequireSubmit"].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            RequireSubmitCount = dsPatientActivityLog.Tables["RequireSubmit"].Rows.Count,
                            RequireSubmit_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["RequireSubmit"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            NewEntryCount = 0,
                            NewEntry_JSON = "",
                            UserCount = 0,
                            User_JSON = "",
                            DeletedChargesCount = 0,
                            DeletedCharges_JSON = "",
                            FieldCount = 0,
                            Field_JSON = "",
                            RequireSubmitCount = 0,
                            RequireSubmit_JSON = "",
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        /// <summary>
        /// Search User Activity Logs
        /// </summary>
        /// <param name="profilerName"></param>
        /// <param name="ColumnKeyId"></param>
        /// <param name="ProfileName"></param>
        /// <param name="DBAuditID"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitID"></param>
        /// <param name="Date"></param>
        /// <param name="ActivityType"></param>
        /// <param name="DBTableName"></param>
        /// <param name="ParentKeyColumnId"></param>
        /// <returns></returns>
        private string SearchUserActivityLogs(String profilerName, string ColumnKeyId, string ProfileName, string DBAuditID, string PatientID, string VisitID, string Date, string ActivityType, string DBTableName, string ParentKeyColumnId = "")
        {
            try
            {
                DataSet dsUserActivityLog = null;
                // string UserName = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                DateTime? createdDate = null;
                if (Date != "")
                    createdDate = MDVUtility.ToDateTime(Date);

                BLObject<DataSet> obj = BLLAdminSecurityObj.LoadUserActivityLogs(profilerName, "", ColumnKeyId, ProfileName, DBAuditID, PatientID, VisitID, createdDate, ActivityType, DBTableName, ParentKeyColumnId);

                dsUserActivityLog = obj.Data;
                if (obj.Data != null)
                {
                    if (dsUserActivityLog.Tables.Contains("RequireSubmit") && dsUserActivityLog.Tables["RequireSubmit"].Rows.Count > 0 && dsUserActivityLog.Tables.Contains("NewEntry") && dsUserActivityLog.Tables["NewEntry"].Rows.Count > 0)
                    {

                        if (dsUserActivityLog.Tables["User"].Rows.Count > 0 && ActivityType.IndexOf("User") > -1)
                        {
                            var response = new
                            {
                                status = true,
                                RequireSubmitCount = dsUserActivityLog.Tables["RequireSubmit"].Rows.Count,
                                RequireSubmit_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["RequireSubmit"]),
                                NewEntryCount = dsUserActivityLog.Tables["NewEntry"].Rows.Count,
                                NewEntry_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["NewEntry"]),
                                UserCount = dsUserActivityLog.Tables["User"].Rows.Count,
                                User_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["User"])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                RequireSubmitCount = dsUserActivityLog.Tables["RequireSubmit"].Rows.Count,
                                RequireSubmit_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["RequireSubmit"]),
                                NewEntryCount = dsUserActivityLog.Tables["NewEntry"].Rows.Count,
                                NewEntry_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["NewEntry"])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                    else if (ActivityType.IndexOf("NewEntry") > -1 && ActivityType.IndexOf("User") > -1)
                    {
                        var response = new
                        {
                            status = true,
                            NewEntryCount = dsUserActivityLog.Tables["NewEntry"].Rows.Count,
                            NewEntry_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["NewEntry"]),
                            UserCount = dsUserActivityLog.Tables["User"].Rows.Count,
                            User_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["User"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                        if (dsUserActivityLog.Tables.Contains("NewEntry") && dsUserActivityLog.Tables["NewEntry"].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            NewEntryCount = dsUserActivityLog.Tables["NewEntry"].Rows.Count,
                            NewEntry_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["NewEntry"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else if (dsUserActivityLog.Tables.Contains("User") && dsUserActivityLog.Tables["User"].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            UserCount = dsUserActivityLog.Tables["User"].Rows.Count,
                            User_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["User"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else if (dsUserActivityLog.Tables.Contains("Field") && dsUserActivityLog.Tables["Field"].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            FieldCount = dsUserActivityLog.Tables["Field"].Rows.Count,
                            Field_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["Field"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    if (dsUserActivityLog.Tables.Contains("DeletedCharges") && dsUserActivityLog.Tables["DeletedCharges"].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            DeletedChargesCount = dsUserActivityLog.Tables["DeletedCharges"].Rows.Count,
                            DeletedCharges_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["DeletedCharges"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    if (dsUserActivityLog.Tables.Contains("RequireSubmit") && dsUserActivityLog.Tables["RequireSubmit"].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            RequireSubmitCount = dsUserActivityLog.Tables["RequireSubmit"].Rows.Count,
                            RequireSubmit_JSON = MDVUtility.JSON_DataTable(dsUserActivityLog.Tables["RequireSubmit"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            NewEntryCount = 0,
                            NewEntry_JSON = "",
                            UserCount = 0,
                            User_JSON = "",
                            DeletedChargesCount = 0,
                            DeletedCharges_JSON = "",
                            FieldCount = 0,
                            Field_JSON = "",
                            RequireSubmitCount = 0,
                            RequireSubmit_JSON = "",
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SearchPatientActivityDetailLogs(String profilerName, string ColumnKeyId, string ProfileName, string DBAuditID, string PatientID, string VisitID, string Date, string ActivityType, string DBTableName, string ParentKeyColumnId = "")
        {
            try
            {
                DataSet dsPatientActivityLog = null;
                // string UserName = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                DateTime? createdDate = null;
                if (Date != "")
                    createdDate = MDVUtility.ToDateTime(Date);

                BLObject<DataSet> obj = BLLPatientObj.LoadPatientActivityDetailLogs(profilerName, "", ColumnKeyId, ProfileName, DBAuditID, PatientID, VisitID, createdDate, ActivityType, DBTableName, ParentKeyColumnId);

                dsPatientActivityLog = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatientActivityLog.Tables.Contains("RequireSubmit") && dsPatientActivityLog.Tables["RequireSubmit"].Rows.Count > 0 && dsPatientActivityLog.Tables.Contains("NewEntry") && dsPatientActivityLog.Tables["NewEntry"].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            RequireSubmitCount = dsPatientActivityLog.Tables["RequireSubmit"].Rows.Count,
                            RequireSubmit_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["RequireSubmit"]),
                            NewEntryCount = dsPatientActivityLog.Tables["NewEntry"].Rows.Count,
                            NewEntry_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["NewEntry"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else if (ActivityType.IndexOf("NewEntry") > -1 && ActivityType.IndexOf("User") > -1)
                    {
                        var response = new
                        {
                            status = true,
                            NewEntryCount = dsPatientActivityLog.Tables["NewEntry"].Rows.Count,
                            NewEntry_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["NewEntry"]),
                            UserCount = dsPatientActivityLog.Tables["User"].Rows.Count,
                            User_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["User"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                        if (dsPatientActivityLog.Tables.Contains("NewEntry") && dsPatientActivityLog.Tables["NewEntry"].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                NewEntryCount = dsPatientActivityLog.Tables["NewEntry"].Rows.Count,
                                NewEntry_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["NewEntry"])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else if (dsPatientActivityLog.Tables.Contains("User") && dsPatientActivityLog.Tables["User"].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                UserCount = dsPatientActivityLog.Tables["User"].Rows.Count,
                                User_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["User"])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else if (dsPatientActivityLog.Tables.Contains("Field") && dsPatientActivityLog.Tables["Field"].Rows.Count > 0)
                        {
                            var response = new
                            {
                                status = true,
                                FieldCount = dsPatientActivityLog.Tables["Field"].Rows.Count,
                                Field_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["Field"])
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    if (dsPatientActivityLog.Tables.Contains("DeletedCharges") && dsPatientActivityLog.Tables["DeletedCharges"].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            DeletedChargesCount = dsPatientActivityLog.Tables["DeletedCharges"].Rows.Count,
                            DeletedCharges_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["DeletedCharges"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    if (dsPatientActivityLog.Tables.Contains("RequireSubmit") && dsPatientActivityLog.Tables["RequireSubmit"].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            RequireSubmitCount = dsPatientActivityLog.Tables["RequireSubmit"].Rows.Count,
                            RequireSubmit_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables["RequireSubmit"])
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            NewEntryCount = 0,
                            NewEntry_JSON = "",
                            UserCount = 0,
                            User_JSON = "",
                            DeletedChargesCount = 0,
                            DeletedCharges_JSON = "",
                            FieldCount = 0,
                            Field_JSON = "",
                            RequireSubmitCount = 0,
                            RequireSubmit_JSON = "",
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SearchNoteCommentsLogs(string PatientID, string VisitId, string DBTableName)
        {
            try
            {
                DSDBAudit dsPatientActivityLog = null;
                BLObject<DSDBAudit> obj = BLLBillingObj.LoadDBAudit_NoteComments(PatientID, VisitId, DBTableName);
                dsPatientActivityLog = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        LogCount = dsPatientActivityLog.Tables[dsPatientActivityLog.DBAudit.TableName].Rows.Count,
                        LogLoad_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables[dsPatientActivityLog.DBAudit.TableName]),
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

        //private string SearchPatientVisitsActivityLogs(string ColumnKeyId, string ProfileName, string DBAuditID, string PatientID, string VisitID, string Date)
        //{
        //    try
        //    {
        //        DataSet dsPatientVisitsActivityLog = null;
        //        // string UserName = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //        DateTime? createdDate = null;
        //        if (Date != "")
        //            createdDate = MDVUtility.ToDateTime(Date);

        //        BLObject<DataSet> obj = BLLPatientObj.LoadPatientActivityLogs(null, "", ColumnKeyId, ProfileName, DBAuditID, PatientID, VisitID, createdDate);

        //        dsPatientVisitsActivityLog = obj.Data;
        //        if (obj.Data != null)
        //        {
        //            if (dsPatientVisitsActivityLog.Tables["NewEntry"].Rows.Count > 0 || dsPatientVisitsActivityLog.Tables["User"].Rows.Count > 0 || dsPatientVisitsActivityLog.Tables["Field"].Rows.Count > 0)
        //            {
        //                var response = new
        //                {
        //                    status = true,
        //                    NewEntryCount = dsPatientVisitsActivityLog.Tables["NewEntry"].Rows.Count,
        //                    NewEntry_JSON = MDVUtility.JSON_DataTable(dsPatientVisitsActivityLog.Tables["NewEntry"]),
        //                    UserCount = dsPatientVisitsActivityLog.Tables["User"].Rows.Count,
        //                    User_JSON = MDVUtility.JSON_DataTable(dsPatientVisitsActivityLog.Tables["User"]),
        //                    FieldCount = dsPatientVisitsActivityLog.Tables["Field"].Rows.Count,
        //                    Field_JSON = MDVUtility.JSON_DataTable(dsPatientVisitsActivityLog.Tables["Field"]),
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = true,
        //                    NewEntryCount = 0,
        //                    NewEntry_JSON = "",
        //                    UserCount = 0,
        //                    User_JSON = "",
        //                    FieldCount = 0,
        //                    Field_JSON = "",
        //                    Message = obj.Message
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = obj.Message
        //            };
        //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}

        private string LoadPatientVisitsPrintInfo(string PatientID, string VisitId)
        {
            try
            {
                DSDBAudit dsPatientActivityLog = null;
                BLObject<DSDBAudit> obj = BLLBillingObj.LoadPatientVisitsPrintInfo(PatientID, VisitId);
                dsPatientActivityLog = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        VisisPrintCount = dsPatientActivityLog.Tables[dsPatientActivityLog.DT_PatientVisit_Print_Info.TableName].Rows.Count,
                        VisitNotePrintLoad_JSON = MDVUtility.JSON_DataTable(dsPatientActivityLog.Tables[dsPatientActivityLog.DT_PatientVisit_Print_Info.TableName], "", false),
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
        /// Handle the Basic Free Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "LOAD_PATIENT_ACTIVITY_LOG":
                    {
                        string DBAuditID = MDVUtility.ToStr(context.Request["DBAuditID"]);
                        string PatientID = MDVUtility.ToStr(context.Request["PatientID"]);
                        string ColumnKeyId = MDVUtility.ToStr(context.Request["ColumnKeyId"]);
                        string ProfileName = MDVUtility.ToStr(context.Request["ProfileName"]);
                        string createdDate = MDVUtility.ToStr(context.Request["EntryDate"]);
                        string ActivityType = MDVUtility.ToStr(context.Request["ActivityType"]);
                        string DBTableName = MDVUtility.ToStr(context.Request["DBTableName"]);
                        string strJSONData = SearchPatientActivityLogs("Patient", ColumnKeyId, ProfileName, DBAuditID, PatientID, "", createdDate, ActivityType, DBTableName);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


                case "LOAD_PATIENTVISITS_ACTIVITY_LOG":
                    {
                        string DBAuditID = MDVUtility.ToStr(context.Request["DBAuditID"]);
                        string PatientID = MDVUtility.ToStr(context.Request["PatientID"]);
                        string ColumnKeyId = MDVUtility.ToStr(context.Request["ColumnKeyId"]);
                        string ProfileName = MDVUtility.ToStr(context.Request["ProfileName"]);
                        string createdDate = MDVUtility.ToStr(context.Request["EntryDate"]);
                        string ActivityType = MDVUtility.ToStr(context.Request["ActivityType"]);
                        string ProfilerName = null;
                        string DBTableName = MDVUtility.ToStr(context.Request["DBTableName"]);
                        string ParentKeyColumnId = MDVUtility.ToStr(context.Request["ParentKeyColumnId"]);
                        string visitid = "";
                        if (!string.IsNullOrEmpty(context.Request["VisitId"]))
                            visitid = MDVUtility.ToStr(context.Request["VisitId"]);
                        //if (ProfileName.Equals("PatientCharges")&& ActivityType.Equals("NewEntry"))
                        //{
                        //    ProfilerName = "PatientCharges";
                        //}
                        string strJSONData = SearchPatientActivityLogs(ProfilerName, ColumnKeyId, ProfileName, DBAuditID, PatientID, visitid, createdDate, ActivityType, DBTableName, ParentKeyColumnId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_NOTE_COMMENTS_AUDIT":
                    {
                        string PatientID = MDVUtility.ToStr(context.Request["PatientID"]);
                        string VisitId = MDVUtility.ToStr(context.Request["VisitID"]);
                        string DBTableName = MDVUtility.ToStr(context.Request["DBTableName"]);
                        string strJSONData = SearchNoteCommentsLogs(PatientID, VisitId, DBTableName);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_DRUGCODECOST_ACTIVITY_LOG":
                    {
                       
                        string DBAuditID = MDVUtility.ToStr(context.Request["DBAuditID"]);
                        string CPTCodeCostID = MDVUtility.ToStr(context.Request["CPTCodeCostID"]) == "undefined" ? "" : MDVUtility.ToStr(context.Request["CPTCodeCostID"]);
                        string ColumnKeyId = MDVUtility.ToStr(context.Request["ColumnKeyId"]);
                        string ProfileName = MDVUtility.ToStr(context.Request["ProfileName"]);
                        string createdDate = MDVUtility.ToStr(context.Request["EntryDate"]);
                        string ActivityType = MDVUtility.ToStr(context.Request["ActivityType"]);
                        string ProfilerName = null;
                        string DBTableName = MDVUtility.ToStr(context.Request["DBTableName"]);
                        string ParentKeyColumnId = MDVUtility.ToStr(context.Request["ParentKeyColumnId"]);
                        string visitid = "";
                        if (!string.IsNullOrEmpty(context.Request["VisitId"]))
                            visitid = MDVUtility.ToStr(context.Request["VisitId"]);
                        //if (ProfileName.Equals("PatientCharges")&& ActivityType.Equals("NewEntry"))
                        //{
                        //    ProfilerName = "PatientCharges";
                        //}
                        string strJSONData = SearchPatientActivityLogs(ProfilerName, ColumnKeyId, ProfileName, DBAuditID, CPTCodeCostID, visitid, createdDate, ActivityType, DBTableName, ParentKeyColumnId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData); 
                    }
                    break;
                case "LOAD_USER_ACTIVITY_LOG":
                    {

                        string DBAuditID = MDVUtility.ToStr(context.Request["DBAuditID"]);
                        string CPTCodeCostID = MDVUtility.ToStr(context.Request["CPTCodeCostID"]) == "undefined" ? "" : MDVUtility.ToStr(context.Request["CPTCodeCostID"]);
                        string ColumnKeyId = MDVUtility.ToStr(context.Request["ColumnKeyId"]);
                        string ProfileName = MDVUtility.ToStr(context.Request["ProfileName"]);
                        string createdDate = MDVUtility.ToStr(context.Request["EntryDate"]);
                        string ActivityType = MDVUtility.ToStr(context.Request["ActivityType"]);
                        string ProfilerName = null;
                        string DBTableName = MDVUtility.ToStr(context.Request["DBTableName"]);
                        string ParentKeyColumnId = MDVUtility.ToStr(context.Request["ParentKeyColumnId"]);
                        string visitid = "";
                        if (!string.IsNullOrEmpty(context.Request["VisitId"]))
                            visitid = MDVUtility.ToStr(context.Request["VisitId"]);
                       
                        string strJSONData = SearchUserActivityLogs(ProfilerName, ColumnKeyId, ProfileName, DBAuditID, CPTCodeCostID, visitid, createdDate, ActivityType, DBTableName, ParentKeyColumnId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_PATIENTVISITS_PRINT_INFO":
                    {
                        string PatientID = MDVUtility.ToStr(context.Request["PatientId"]);
                        string VisitId = MDVUtility.ToStr(context.Request["VisitId"]);
                        string strJSONData = LoadPatientVisitsPrintInfo(PatientID, VisitId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}