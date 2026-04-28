using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Model.CCM.CCMHub;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.CCM.Helpers.CCMHub
{
    public class HRAssessmentHelper
    {
        private BLLCCM BLLCCMObj = null;
        public HRAssessmentHelper()
        {
            BLLCCMObj = new BLLCCM();
        }
        private static HRAssessmentHelper _instance = null;

        public object BLLCCCMObj { get; private set; }

        public static HRAssessmentHelper Instance()
        {
            if (_instance == null)
                _instance = new HRAssessmentHelper();
            return _instance;
        }
        #region Methods
        internal string loadHRAssessmentList(HRAssessmentSearchModel model)
        {
            try
            {
                BLObject<List<HRAssessmentFillModel>> obj = BLLCCMObj.loadHRAssessmentList(model);
                List<HRAssessmentFillModel> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        AssessmentCount = modelList.Count,
                        iTotalDisplayRecords = modelList[0].RecordCount,
                        AssessmentList_JSON = js.Serialize(modelList),
                    };
                    return (JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        AssessmentCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = obj.Message
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        internal string deleteHRAssessmentList(long hRAssessmentId)
        {
            try
            {
                if (hRAssessmentId <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLCCMObj.deleteHRAssessment(hRAssessmentId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        internal string fillHRAssessmentList(long hRAssessmentId)
        {
            try
            {
                List<HRAssessmentModel> modelList = BLLCCMObj.fillHRAssessmentList(hRAssessmentId);
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        CPCount = modelList.Count,
                        CPList_JSON = js.Serialize(modelList),
                    };
                    return (JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        CPCount = 0,
                        Message = Common.AppPrivileges.No_Record_Message
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        internal string saveHRAssessmentList(HRAssessmentModel model)
        {
            try
            {

                model.HRAssessmentId = BLLCCMObj.saveHRAssessmentList(model);

                var response = new
                {
                    status = true,
                    HRAssessmentId = model.HRAssessmentId,
                    Message = Common.AppPrivileges.Save_Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        internal string updateHRAssessmentList(HRAssessmentModel model)
        {
            try
            {

                model.HRAssessmentId = BLLCCMObj.updateHRAssessmentList(model);

                var response = new
                {
                    status = true,
                    HRAssessmentId = model.HRAssessmentId,
                    Message = Common.AppPrivileges.Update_Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        internal string updateStatusHRAssessmentList(long hRAssessmentId, string IsActive)
        {
            try
            {
                if (hRAssessmentId <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = IsActive.Equals("1") ? MDVUtility.ToStr(Common.AppPrivileges.Inactive_Error_Message) : MDVUtility.ToStr(Common.AppPrivileges.Active_Error_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLCCMObj.updateStatusHRAssessmentList(hRAssessmentId, IsActive);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = IsActive.Equals("1") ? MDVUtility.ToStr(Common.AppPrivileges.Active_Message) : MDVUtility.ToStr(Common.AppPrivileges.Inactive_Message)
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (JsonConvert.SerializeObject(response));
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
                return (JsonConvert.SerializeObject(response));
            }
        }
        #endregion
    }
}