using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using System.Collections.Generic;
using MDVision.Model.CCM.CCMHub;
using Newtonsoft.Json;
using System;
using System.Web.Script.Serialization;
using MDVision.Common.Shared;

namespace MDVision.IEHR.CCM.Helpers.CCMHub
{
    public class PatientHubHelper
    {
        private BLLCCM BLLCCCMObj = null;
        private BLLClinical BLLClinical = null;
        public PatientHubHelper()
        {
            BLLCCCMObj = new BLLCCM();
            BLLClinical = new BLLClinical();
        }

        public string SaveCCMEnrolledGoals(EnrolledGoals model)
        {
            try
            {
                string demographicsData = BLLCCCMObj.SaveCCMEnrolledGoals(model);

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

        public string SaveCCMEnrolledGoalsCPT(EnrolledGoalsCPT model)
        {
            try
            {
                string demographicsData = BLLCCCMObj.SaveCCMEnrolledGoalsCPT(model);

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

        public string SaveCCMEnrolledGoals_CCMEnrolledGoalsCPT(EnrolledGoals_EnrolledGoalsCPT model, string CPTCode, string CPTDescription, string SNOMEDCode, string SNOMEDDescription)
        {
            try
            {
                Datasets.DSProcedures dsprocedures = new Datasets.DSProcedures();
                Datasets.DSProcedures.CPTLookupRow dr = dsprocedures.CPTLookup.NewCPTLookupRow();

                dr.CPTCode = CPTCode;
                dr.CPT_Description = CPTDescription;
                dr.SNOMEDId = SNOMEDCode;
                dr.SNOMED_Description = SNOMEDDescription;
                
                dsprocedures.CPTLookup.AddCPTLookupRow(dr);
                BLObject<Datasets.DSProcedures> obj = BLLClinical.insertCPTLookup(dsprocedures);
                dsprocedures = obj.Data;
                if (obj.Data != null)
                {
                    var CPTId = MDVUtility.ToLong(dsprocedures.Tables[dsprocedures.CPTLookup.TableName].Rows[0][dsprocedures.CPTLookup.CPTLookupIdColumn.ColumnName].ToString());
                    model.CPTCodeId = MDVUtility.ToLong(CPTId);

                    try
                    {

                        string demographicsData = BLLCCCMObj.SaveCCMEnrolledGoals_EnrolledGoalsCPT(model);

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
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = AppPrivileges.Save_Error_Message,
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

        public string LoadCCMPatientHubStatic(Int64 PatientId, long EnrollmentInfoId)
        {
            try
            {
                BLObject<List<PatientHubStatic>> obj = BLLCCCMObj.LoadCCMPatientHUBStatic(PatientId, EnrollmentInfoId, 0, 150);
                List<PatientHubStatic> modelList = obj.Data;

                //string imageBase64 = "";
                //byte[] imageByteArr = modelList[0].PatientImage as byte[];
                //if (imageByteArr != null)
                //{
                //    imageBase64 = "data:" + modelList[0].ImageType + ";base64," + Convert.ToBase64String(modelList[0].PatientImage as byte[]);
                //}
                //if (!String.IsNullOrEmpty(imageBase64))
                //    modelList[0].PatientImage_ = imageBase64;

                if (modelList != null && modelList.Count > 0)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PHCount = modelList.Count,
                        iTotalDisplayRecords = modelList.Count,
                        PHList_JSON = js.Serialize(modelList),
                    };
                    return (JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        PHCount = 0,
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
        public string LoadCCMPatientHubProblems(Int64 PatientId)
        {
            try
            {
                BLObject<List<PatientHubProblems>> obj = BLLCCCMObj.LoadCCMPatientHUBProblems(PatientId, 0, 15);
                List<PatientHubProblems> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PHCount = modelList.Count,
                        iTotalDisplayRecords = modelList.Count,
                        PHList_JSON = js.Serialize(modelList),
                    };
                    return (JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        PHCount = 0,
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
        //public string LoadCCMPatientHubRiskAssessmentScore(Int64 EnrollmentInfoId)
        //{
        //    try
        //    {
        //        BLObject<List<EnrolledRiskAssessmentTemp>> obj = BLLCCCMObj.LoadCCMPatientHUBRiskAssessmentScore(EnrollmentInfoId);
        //        List<EnrolledRiskAssessmentTemp> modelList = obj.Data;
        //        if (modelList != null && modelList.Count > 0)
        //        {
        //            JavaScriptSerializer js = new JavaScriptSerializer();
        //            var response = new
        //            {
        //                status = true,
        //                PHCount = modelList.Count,
        //                iTotalDisplayRecords = modelList.Count,
        //                PHList_JSON = js.Serialize(modelList),
        //            };
        //            return (JsonConvert.SerializeObject(response));
        //        }

        //        else
        //        {
        //            var response = new
        //            {
        //                status = true,
        //                PHCount = 0,
        //                iTotalDisplayRecords = 0,
        //                Message = obj.Message
        //            };
        //            return (JsonConvert.SerializeObject(response));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message = MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (JsonConvert.SerializeObject(response));
        //    }
        //}
        public string LoadCCMPatientHubRiskAssessmentScore(Int64 EnrollmentInfoId)
        {
            try
            {
                BLObject<List<EnrolledRiskAssessment>> obj = BLLCCCMObj.LoadCCMPatientHUBRiskAssessmentScore(EnrollmentInfoId);
                List<EnrolledRiskAssessment> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PHCount = modelList.Count,
                        iTotalDisplayRecords = modelList.Count,
                        PHList_JSON = js.Serialize(modelList),
                    };
                    return (JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        PHCount = 0,
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

        public string LoadCCMPatientHubCareTeam(Int64 providerId, long EnrollmentInfoId, long CareTeamId)
        {
            try
            {
                BLObject<List<ProviderCareTeam>> obj = BLLCCCMObj.LoadCCMPatientHUBCareTeam(providerId, EnrollmentInfoId, CareTeamId);
                List<ProviderCareTeam> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PHCount = modelList.Count,
                        iTotalDisplayRecords = modelList.Count,
                        PHList_JSON = js.Serialize(modelList),
                        CareTeamId = modelList[0].CareTeamId,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PHCount = 0,
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

        public string LoadCCMPatientHUBGoals(long EnrollmentInfoId)
        {
            try
            {
                BLObject<List<PatientHubEnrolledGoalsCPT>> obj = BLLCCCMObj.LoadCCMPatientHUBGoals(EnrollmentInfoId);
                List<PatientHubEnrolledGoalsCPT> modelList = obj.Data;
                if (modelList != null && modelList.Count > 0)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        PHCount = modelList.Count,
                        iTotalDisplayRecords = modelList.Count,
                        PHList_JSON = js.Serialize(modelList),
                        EnrolledGoalsICDId = modelList[0].EnrolledGoalsICDId,
                        EnrolledGoalsId = modelList[0].EnrolledGoalsId,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PHCount = 0,
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

        public string InsertCCMPatientHUBEnrolledCareTeam(EnrolledCareTeam model)
        {
            try
            {
                long EnrolledCareTeamId = BLLCCCMObj.InsertCCMPatientHUBEnrolledCareTeam(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
                    RiskAssessmentId = EnrolledCareTeamId

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

        public string InsertCCMPatientHUBRiskAssessmentTemplate(RiskAssessment model)
        {
            try
            {
                long RiskAssessmentId = BLLCCCMObj.InsertCCMPatientHUBRiskAssessmentTemplate(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
                    RiskAssessmentId = RiskAssessmentId

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
        public string InsertUpdateCCMPatientHubRiskAssessmentScore(RiskAssessment model)
        {
            try
            {
                long RiskAssessmentId = BLLCCCMObj.InsertUpdateCCMPatientHUBRiskAssessmentScore(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Save_Message,
                    RiskAssessmentId = RiskAssessmentId

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

        public string DeleteRiskAssessmentScore(long EnrollmentInfoId, long RiskAssessTemptId)
        {
            try
            {
                if (EnrollmentInfoId <= 0 && RiskAssessTemptId <= 0)
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
                    BLObject<string> obj = BLLCCCMObj.DeleteRiskAssessmentScoreTemplate(EnrollmentInfoId, RiskAssessTemptId);
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

        public string DeleteChronic(long ChronicProblemId, long PatientId)
        {
            try
            {
                if (ChronicProblemId <= 0 && PatientId <= 0)
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
                    BLObject<string> obj = BLLCCCMObj.DeleteChronicProb(ChronicProblemId, PatientId);
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

        public string DeleteRiskAssessmentScoreTemplate(long RiskAssessmentId)
        {
            try
            {
                if (RiskAssessmentId <= 0 )
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
                    BLObject<string> obj = BLLCCCMObj.DeleteRiskAssessmentScoreTemplate(RiskAssessmentId);
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

        public string DeleteCareTeamProviderTemplate(long ProviderId, long EnrollmentInfoId)
        {
            try
            {
                if (ProviderId <= 0 && EnrollmentInfoId <= 0)
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
                    BLObject<string> obj = BLLCCCMObj.DeleteCareTeamProviderTemplate(ProviderId, EnrollmentInfoId);
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
        public string DeletePatientHubEnrolledGoals(long EnrolledGoalsId,long EnrolledGoalsICDId)
        {
            try
            {
                if (EnrolledGoalsId <= 0 && EnrolledGoalsICDId <= 0)
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
                    BLObject<string> obj = BLLCCCMObj.DeletePatientHubEnrolledGoals(EnrolledGoalsId, EnrolledGoalsICDId);
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
    }
}