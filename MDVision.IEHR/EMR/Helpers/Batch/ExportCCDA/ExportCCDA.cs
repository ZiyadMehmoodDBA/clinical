using MDVision.Datasets;
using MDVision.Business.BCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.Model.AuditableEvents;
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems;
using MDVision.Model.Batch.ExportCCDA;
using System.Xml.Serialization;
using System.IO;

namespace MDVision.IEHR.EMR.Helpers.Batch.ExportCCDA
{
    public class HelperExportCCDA
    {
        private BLLExportCCDA BLLExportCCDAObj = null;
        public HelperExportCCDA()
        {
            BLLExportCCDAObj = new BLLExportCCDA();
        }
        private static BLLExportCCDA _instance = null;
        public static BLLExportCCDA Instance()
        {
            if (_instance == null)
                _instance = new BLLExportCCDA();
            return _instance;
        }

        /// Author: Zia Mehmood
        /// Purpose: To load User Activity Log
        /// Date : OCTOBER 27, 2017
        #region Load ROS Templates
        public string Fill_Paitent_Lookpup(ExportCCDAModel model)
        {
        
            List<ExportCCDAModel> objList_ExportCCDA = new List<ExportCCDAModel>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
               
                objList_ExportCCDA = BLLExportCCDAObj.Fill_Paitent_Lookpup(model);
                if (objList_ExportCCDA != null)
                {
                    if (objList_ExportCCDA.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ExportCCDACount = objList_ExportCCDA.Count,
                            iTotalDisplayRecords = objList_ExportCCDA[0].RecordCount,
                            ExportCCDA_JSON = js.Serialize(objList_ExportCCDA)
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
        public string Get_Scheduled_PatientVisits(ExportCCDAModel model)
        {

            List<ExportCCDAModel> objList_ExportCCDA = new List<ExportCCDAModel>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                objList_ExportCCDA = BLLExportCCDAObj.Get_Scheduled_PatientVisits(model);
                if (objList_ExportCCDA != null)
                {
                    if (objList_ExportCCDA.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ExportCCDACount = objList_ExportCCDA.Count,
                            iTotalDisplayRecords = objList_ExportCCDA[0].RecordCount,
                            ExportCCDA_JSON = js.Serialize(objList_ExportCCDA)
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
        public string Fill_NoteComponent_Lookpup(ExportCCDAModel model)
        {

            List<ExportCCDAModel> objList_ExportCCDA = new List<ExportCCDAModel>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                objList_ExportCCDA = BLLExportCCDAObj.Fill_NoteComponent_Lookpup(model);
                if (objList_ExportCCDA != null)
                {
                    if (objList_ExportCCDA.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ExportCCDACount = objList_ExportCCDA.Count,
                            iTotalDisplayRecords = objList_ExportCCDA[0].RecordCount,
                            ExportCCDA_JSON = js.Serialize(objList_ExportCCDA)
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
        public string Insert_CCDA_Schedule(ExportCCDAModel model)
        {
            try
            {
                model.CreatedBy = MDVUtility.ToStr(MDVSession.Current.AppUserFullName);
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedBy = MDVUtility.ToStr(MDVSession.Current.AppUserFullName);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);

                ExportCCDAModel obj = BLLExportCCDAObj.Insert_CCDA_Schedule(model);
                if (obj.SchedulerId != "")
                {
                    //string Schedule = obj.Data.ToString();
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        ScheduleId = obj.SchedulerId,


                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Schedule already exists.",
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
        public string Load_CCDA_Schedule(ExportCCDAModel model)
        {

            List<ExportCCDAModel> objList_ExportCCDA = new List<ExportCCDAModel>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                objList_ExportCCDA = BLLExportCCDAObj.Load_CCDA_Schedule(model);
                if (objList_ExportCCDA != null)
                {
                    if (objList_ExportCCDA.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ExportCCDACount = objList_ExportCCDA.Count,
                            iTotalDisplayRecords = objList_ExportCCDA[0].RecordCount,
                            ExportCCDA_JSON = js.Serialize(objList_ExportCCDA)
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
        public string Select_CCDA_Schedule(ExportCCDAModel model)
        {

            List<ExportCCDAModel> objList_ExportCCDA = new List<ExportCCDAModel>();
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                objList_ExportCCDA = BLLExportCCDAObj.Select_CCDA_Schedule(model);
                if (objList_ExportCCDA != null)
                {
                    if (objList_ExportCCDA.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ExportCCDACount = objList_ExportCCDA.Count,
                            iTotalDisplayRecords = objList_ExportCCDA[0].RecordCount,
                            ExportCCDA_JSON = js.Serialize(objList_ExportCCDA)
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
        public string Delete_CCDA_Schedule(string SchedulerId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(SchedulerId)))
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
                    BLObject<string> obj = BLLExportCCDAObj.Delete_CCDA_Schedule(SchedulerId);
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }
        public string ActiveInactive_CCDA_Schedule(string SchedulerId, string IsActive)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(SchedulerId)))
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
                    BLObject<string> obj = BLLExportCCDAObj.ActiveInactive_CCDA_Schedule(SchedulerId, IsActive, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Update_Message
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        #endregion
    }
}