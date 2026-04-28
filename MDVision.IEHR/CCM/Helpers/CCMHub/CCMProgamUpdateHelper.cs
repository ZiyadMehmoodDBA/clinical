using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.Model.CCM.CCMHub;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MDVision.IEHR.Controls.CCM.Herlpers
{

    public class CCMProgramUpdateHelper
    {
        private BLLCCM BLLCCCMObj = null;
        public CCMProgramUpdateHelper()
        {
            BLLCCCMObj = new BLLCCM();
        }
        public string SaveCCMTaskTime(TaskAmalgamatedModel model)
        {
            try
            {
                double TaskHours = MDVUtility.ToInt32(model.TaskHours) > 0 ? MDVUtility.ToInt32(model.TaskHours) : 0;
                double TaskMinutes = MDVUtility.ToInt32(model.TaskMinutes) > 0 ? MDVUtility.ToInt32(model.TaskMinutes) : 0;
                double TaskSeconds = MDVUtility.ToInt32(model.TaskSeconds) > 0 ? MDVUtility.ToInt32(model.TaskSeconds) : 0;
                double duration = 0;

                duration = MDVUtility.ToDouble((TaskHours * 60) + (TaskMinutes) + (TaskSeconds / 60));
                model.TaskDuration = MDVUtility.ToStr(duration);
                string TaskTimerId = BLLCCCMObj.SaveCCMTaskTime(model);

                var response = new
                {
                    status = true,
                    TaskTimerId = TaskTimerId,
                    Message = AppPrivileges.Save_Message,
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

        public string DeleteCCMTaskTime(CCMTaskTimerModel model)
        {
            try
            {
                long taskTimerId = MDVUtility.ToInt64(model.TaskTimerId);
                if (taskTimerId > 0)
                {

                    string TaskTimerId = BLLCCCMObj.DeleteCCMTaskTime(model);

                    var response = new
                    {
                        status = true,
                        TaskTimerId = TaskTimerId,
                        Message = AppPrivileges.Delete_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Delete_Error_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
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
        public string UpdateCCMTaskTime(CCMTaskTimerModel model)
        {
            try
            {
                double TaskHours = MDVUtility.ToInt32(model.TaskHours) > 0 ? MDVUtility.ToInt32(model.TaskHours) : 0;
                double TaskMinutes = MDVUtility.ToInt32(model.TaskMinutes) > 0 ? MDVUtility.ToInt32(model.TaskMinutes) : 0;
                double TaskSeconds = MDVUtility.ToInt32(model.TaskSeconds) > 0 ? MDVUtility.ToInt32(model.TaskSeconds) : 0;
                double duration = 0;

                duration = MDVUtility.ToDouble((TaskHours * 60) + (TaskMinutes) + (TaskSeconds / 60));
                model.TaskDuration = MDVUtility.ToStr(duration);
                string TaskTimerId = BLLCCCMObj.UpdateCCMTaskTime(model);

                var response = new
                {
                    status = true,
                    TaskTimerId = TaskTimerId,
                    Message = AppPrivileges.Update_Message,
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

        public string SaveCCMTaskTimeFromDashBoard(CCMTaskTimerModel model)
        {
            try
            {
                //double TaskHours = MDVUtility.ToInt32(model.TaskHours) > 0 ? MDVUtility.ToInt32(model.TaskHours) : 0;
                //double TaskMinutes = MDVUtility.ToInt32(model.TaskMinutes) > 0 ? MDVUtility.ToInt32(model.TaskMinutes) : 0;
                //double TaskSeconds = MDVUtility.ToInt32(model.TaskSeconds) > 0 ? MDVUtility.ToInt32(model.TaskSeconds) : 0;
                //double duration = 0;

                //duration = MDVUtility.ToDouble((TaskHours * 60) + (TaskMinutes) + (TaskSeconds / 60));
                //model.TaskDuration = MDVUtility.ToStr(duration);
                string TaskTimerId = BLLCCCMObj.SaveCCMTaskTimeFromDashBoard(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
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


        public string LoadCCMTaskTimer(CCMTaskTimerModel model)
        {
            try
            {
                List<CCMTaskTimerModel> TaskTimerList = null;
                BLObject<List<CCMTaskTimerModel>> obj;

                obj = BLLCCCMObj.LoadCCMTaskTimer(MDVUtility.ToInt32(model.TaskTimerId), MDVUtility.ToInt32(model.EnrollmentInfoId), MDVUtility.ToInt32(model.PatientId),model.Action,model.SelectedMonth, model.PageNumber,model.RowsPerPage);
                TaskTimerList = obj.Data;
                if (obj.Data != null)
                {
                    if (TaskTimerList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CCMTaskTimerCount = TaskTimerList.Count,
                            iTotalDisplayRecords = TaskTimerList[0].RecordCount,
                            CCMTaskTimer_JSON = TaskTimerList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            CCMTaskTimerCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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

        public string SaveCCMCallDetails(TaskAmalgamatedModel model)
        {
            try
            {


                DateTime dt;
                bool res = DateTime.TryParse(model.CallTime, out dt);

                model.CallTime = dt.ToString("HH:mm");


                string CallDetailsId = BLLCCCMObj.SaveCCMCallDetails(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
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

        public string UpdateCCMTaskTimeDetails(TaskAmalgamatedModel model)
        {
            try
            {

                double TaskHours = MDVUtility.ToInt32(model.TaskHours) > 0 ? MDVUtility.ToInt32(model.TaskHours) : 0;
                double TaskMinutes = MDVUtility.ToInt32(model.TaskMinutes) > 0 ? MDVUtility.ToInt32(model.TaskMinutes) : 0;
                double TaskSeconds = MDVUtility.ToInt32(model.TaskSeconds) > 0 ? MDVUtility.ToInt32(model.TaskSeconds) : 0;
                double duration = 0;

                if (model.DurationUnit == "seconds")
                {
                    TaskHours = 0;
                    TaskMinutes = 0;
                    TaskSeconds = double.Parse(model.TaskDuration);
                }
                else if (model.DurationUnit == "minutes")
                {
                    TaskHours = 0;
                    TaskMinutes = double.Parse(model.TaskDuration);
                    TaskSeconds = 0;
                }
                else if (model.DurationUnit == "hours")
                {
                    TaskHours = double.Parse(model.TaskDuration);
                    TaskMinutes = 0; 
                    TaskSeconds = 0;
                }

                duration = MDVUtility.ToDouble((TaskHours * 60) + (TaskMinutes) + (TaskSeconds / 60));
                model.TaskDuration = MDVUtility.ToStr(duration);

                DateTime dt;
                bool res = DateTime.TryParse(model.CallTime, out dt);

                model.CallTime = dt.ToString("HH:mm");


                string CallDetailsId = BLLCCCMObj.UpdateCCMTaskTimeDetails(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
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

        public string DeleteCCMCallDetails(CCMCallDetailsModel model)
        {
            try
            {
                long callId = MDVUtility.ToInt64(model.CallId);

                if (callId > 0)
                {
                    string CallDetailsId = BLLCCCMObj.DeleteCCMCallDetails(callId);
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Delete_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return JsonConvert.SerializeObject(response);
                }

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

        public string LoadCCMCallDetails(CCMCallDetailsModel model)
        {
            try
            {
                List<CCMCallDetailsModel> CallDetailsList = null;
                BLObject<List<CCMCallDetailsModel>> obj;

                obj = BLLCCCMObj.LoadCCMCallDetails(MDVUtility.ToInt32(model.CallId), MDVUtility.ToInt32(model.EnrollmentInfoId), MDVUtility.ToInt32(model.PatientId), model.Action);
                CallDetailsList = obj.Data;
                if (obj.Data != null)
                {
                    if (CallDetailsList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CCMCallDetailsCount = CallDetailsList.Count,
                            iTotalDisplayRecords = CallDetailsList.Count,
                            CCMTaskTimer_JSON = CallDetailsList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            CCMTaskTimerCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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

        public string LoadCCMTaskTimerDetails(TaskAmalgamatedModel model)
        {
            try
            {
                List<TaskAmalgamatedModel> CallDetailsList = null;
                BLObject<List<TaskAmalgamatedModel>> obj;

                obj = BLLCCCMObj.LoadCCMTaskTimerDetails(MDVUtility.ToInt32(model.TaskTimerAmalgamatedId), MDVUtility.ToInt32(model.EnrollmentInfoId), MDVUtility.ToInt32(model.PatientId), model.Action, model.Month, model.Year);
                CallDetailsList = obj.Data;
                if (obj.Data != null)
                {
                    if (CallDetailsList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CCMCallDetailsCount = CallDetailsList.Count,
                            iTotalDisplayRecords = CallDetailsList.Count,
                            CCMTaskTimer_JSON = CallDetailsList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            CCMTaskTimerCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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

        public string SaveProgramUpdate(CCMProgramUpdateModel model)
        {
            try
            {

                string ProgressUpdateId = BLLCCCMObj.SaveProgramUpdate(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
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


        public string LoadProgressUpdate(CCMProgramUpdateModel model)
        {
            try
            {
                List<CCMProgramUpdateModel> ProgressUpdateList = null;
                BLObject<List<CCMProgramUpdateModel>> obj;

                obj = BLLCCCMObj.LoadProgressUpdate(MDVUtility.ToInt32(model.ProgressUpdateId), MDVUtility.ToInt32(model.ProgressCategoryId), MDVUtility.ToInt32(model.EnrollmentInfoId), MDVUtility.ToInt32(model.PatientId));
                ProgressUpdateList = obj.Data;
                if (obj.Data != null)
                {
                    if (ProgressUpdateList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CCMProgressUpdateListCount = ProgressUpdateList.Count,
                            iTotalDisplayRecords = ProgressUpdateList.Count,
                            CCMProgressUpdateList_JSON = ProgressUpdateList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            CCMProgressUpdateListCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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

        public string LoadProgressUpdateDetail(CCMProgramUpdateModel model)
        {
            try
            {
                List<CCMProgramUpdateModel> ProgressUpdateList = null;
                BLObject<List<CCMProgramUpdateModel>> obj;

                obj = BLLCCCMObj.LoadProgressUpdateDetail(MDVUtility.ToInt32(model.EnrollmentInfoId), MDVUtility.ToInt32(model.PatientId), MDVUtility.ToInt32(model.ProgressMonth), MDVUtility.ToInt32(model.ProgressYear));
                ProgressUpdateList = obj.Data;
                if (obj.Data != null)
                {
                    if (ProgressUpdateList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CCMProgressUpdateListCount = ProgressUpdateList.Count,
                            iTotalDisplayRecords = ProgressUpdateList.Count,
                            CCMProgressUpdateFill_JSON = ProgressUpdateList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            CCMProgressUpdateListCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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


    }
}