using MDVision.Datasets;
using MDVision.Business.BCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using System.Text;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Model.AuditableEvents;

namespace MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems
{
    public class ActivityLogHelper
    {
        private BLLAuditbleEventsActivityLog BLLActivityLogObj = null;
        public ActivityLogHelper()
        {
            BLLActivityLogObj = new BLLAuditbleEventsActivityLog();
        }
        private static ActivityLogHelper _instance = null;
        public static ActivityLogHelper Instance()
        {
            if (_instance == null)
                _instance = new ActivityLogHelper();
            return _instance;
        }

        /// Author: Zia Mehmood
        /// Purpose: To load User Activity Log
        /// Date : OCTOBER 27, 2017
        #region Load ROS Templates
        public string loadAcitivityLogUser(ActivityLog model)
        {
        
            List<ActivityLog> objList_ActivityLog = new List<ActivityLog>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                js.MaxJsonLength = 50000000;
                objList_ActivityLog = BLLActivityLogObj.loadAcitivityLogUser(model);
                if (objList_ActivityLog != null)
                {
                    if (objList_ActivityLog.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogUserCount = objList_ActivityLog.Count,
                            iTotalDisplayRecords = objList_ActivityLog[0].RecordCount,
                            ActivityLogUser_JSON = js.Serialize(objList_ActivityLog)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogUserCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogUser_JSON = "[]",
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
                        ActivityLogUserCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogUser_JSON = "[]",
                        Message = "",
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
        public string loadAcitivityLogComponents(ActivityLog model)
        {

            List<ActivityLog> objList_ActivityLog = new List<ActivityLog>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_ActivityLog = BLLActivityLogObj.loadAcitivityLogComponents(model);
                if (objList_ActivityLog != null)
                {
                    if (objList_ActivityLog.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogCompCount = objList_ActivityLog.Count,
                            iTotalDisplayRecords = objList_ActivityLog[0].RecordCount,
                            ActivityLogCompLoad_JSON = js.Serialize(objList_ActivityLog)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogCompCount = 0,

                            ActivityLogCompLoad_JSON = "[]",
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
                        ActivityLogCompCount = 0,
                        
                        ActivityLogCompLoad_JSON = "[]",
                        Message = "",
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
        public string loadAcitivityLogChanges(ActivityLog model)
        {

            List<ActivityLog> objList_ActivityLog = new List<ActivityLog>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_ActivityLog = BLLActivityLogObj.loadAcitivityLogChanges(model);
                if (objList_ActivityLog != null)
                {
                    if (objList_ActivityLog.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = objList_ActivityLog.Count,
                             iTotalDisplayRecords = objList_ActivityLog[0].RecordCount,
                            ActivityLogChangesLoad_JSON = js.Serialize(objList_ActivityLog)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogChangesLoad_JSON = "[]",
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
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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
        public string loadCheckInAppUser(ActivityLog model)
        {

            List<ActivityLog> objList_ActivityLog = new List<ActivityLog>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                objList_ActivityLog = BLLActivityLogObj.loadCheckInAppUser(model);
                if (objList_ActivityLog != null)
                {
                    if (objList_ActivityLog.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = objList_ActivityLog.Count,
                            iTotalDisplayRecords = objList_ActivityLog[0].RecordCount,
                            ActivityLogChangesLoad_JSON = js.Serialize(objList_ActivityLog)
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            ActivityLogChangesCount = 0,
                            iTotalDisplayRecords = 0,
                            ActivityLogChangesLoad_JSON = "[]",
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
                        ActivityLogChangesCount = 0,
                        iTotalDisplayRecords = 0,
                        ActivityLogChangesLoad_JSON = "[]",
                        Message = "",
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
    }
}