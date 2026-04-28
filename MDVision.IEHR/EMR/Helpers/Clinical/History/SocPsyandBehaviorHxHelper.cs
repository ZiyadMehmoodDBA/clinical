using MDVision.Model.Clinical.History;
using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.IEHR.EMR.Model.Clinical.Immunization;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Model.Clinical.Orderset;
using MDVision.Model.Clinical.Templates.OrderSets;
using MDVision.Model.Lookups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MDVision.Model.Clinical.History.HistorySummary;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MDVision.IEHR.EMR.Helpers.Clinical.History
{
    public class SocPsyandBehaviorHxHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public SocPsyandBehaviorHxHelper()
        {
            BLLClinicalObj = new BLLClinical();
            // BLLRcopiaObj = new BLLRcopia();
        }
        private static SocPsyandBehaviorHxHelper _instance = null;
        //private static bool isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
        public static SocPsyandBehaviorHxHelper Instance()
        {
            if (_instance == null)
            {
                _instance = new SocPsyandBehaviorHxHelper();
                //   isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
            }
            return _instance;
        }
        public string GetQuestions(SocPsyandBehaviorHxModel model)
        {
            try
            {
                List<SocPsyandBehaviorHxModel> QuestionList = null;
                BLObject<List<SocPsyandBehaviorHxModel>> obj;

                obj = BLLClinicalObj.GetQuestions();
                
                if (obj.Data != null)
                {
                    QuestionList = obj.Data;
                    if (QuestionList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            iTotalDisplayRecords = QuestionList[0].RecordCount,
                            QuestionCount = QuestionList.Count,
                            Question_JSON = QuestionList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            QuestionCount = 0,
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

        public string GetTotalAndPHQScore(SocPsyandBehaviorHxModel model)
        {
            try
            {
                List<SocPsyandBehaviorHxModel> ScoreList = null;
                BLObject<List<SocPsyandBehaviorHxModel>> obj;

                obj = BLLClinicalObj.GetTotalAndPHQScore(model.AllAnswerIds, model.PHQAnswerIds,model.AlcoholAnswerIds,model.SocConnAndIsolAnswerIds,model.ExposToViolIds);
                if (obj.Data != null)
                {
                    ScoreList = obj.Data;
                    if (ScoreList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ScoreCount = ScoreList.Count,
                            Score_JSON = ScoreList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ScoreCount = 0,
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
        public string LoadSocPsyandBehaviorHx(SocPsyandBehaviorHxModel model)
        {
            try
            {
                List<SocPsyandBehaviorHxModel> SocPsyandBehaviorHx = null;
                List<SocPsyQuestionAnswerModel> SocPsyQuestionAnswer = null;
                BLObject<List<SocPsyandBehaviorHxModel>> obj;
                BLObject<List<SocPsyQuestionAnswerModel>> objQuestionAnswer;
                obj = BLLClinicalObj.LoadSocPsyandBehaviorHx(model.SocialandBehaviorHxId, model.PatientId, model.Current);
                objQuestionAnswer = BLLClinicalObj.LoadSocialandBehaviorQA(model.SocialandBehaviorHxId, model.PatientId, model.Current);
               
                if (obj.Data != null && objQuestionAnswer.Data != null)
                {
                    SocPsyandBehaviorHx = obj.Data;
                    SocPsyQuestionAnswer = objQuestionAnswer.Data;

                    if (SocPsyandBehaviorHx.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SocPsyandBehaviorHxCount = SocPsyandBehaviorHx.Count,
                            SocPsyandBehaviorHx_JSON = SocPsyandBehaviorHx[0],
                            SocPsyQuestionAnswerCount = SocPsyQuestionAnswer.Count,
                            SocPsyQuestionAnswer_JSON = SocPsyQuestionAnswer
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SocPsyandBehaviorHxCount = 0,
                            SocPsyQuestionAnswerCount = 0,
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



        public string SearchSocPsyandBehaviorHx(SocPsyandBehaviorHxModel model)
        {
            try
            {
                List<SocPsyandBehaviorHxModel> SocPsyandBehaviorHx = null;
                BLObject<List<SocPsyandBehaviorHxModel>> obj;
                obj = BLLClinicalObj.SearchSocPsyandBehaviorHx(model.SocialandBehaviorHxId, model.PatientId, model.Current, model.PageNumber, model.RowspPage);
               
                if (obj.Data != null)
                {
                    SocPsyandBehaviorHx = obj.Data;
                    if (SocPsyandBehaviorHx.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SocPsyandBehaviorHxCount = SocPsyandBehaviorHx.Count,
                            SocPsyandBehaviorHx_JSON = SocPsyandBehaviorHx,
                            iTotalDisplayRecords = SocPsyandBehaviorHx.Count
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SocPsyandBehaviorHxCount = 0,
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
        public string SocPsyandBehaviorHxSave(SocPsyandBehaviorHxModel model)
        {
            dynamic response;
            try
            {
                model.IsActive = "1";
                model.IsDeleted = "1";
                model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.CreatedOn = MDVUtility.ToStr(DateTime.Now);
                model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                model.ModifiedOn = MDVUtility.ToStr(DateTime.Now);
                //string ProblemXMLString = string.Empty;
                //using (TextWriter writer = new StringWriter())
                //{
                //    GetOrderSetProblemListModelXMLTable(model.QuestionAnswerArray).WriteXml(writer);
                //    ProblemXMLString = writer.ToString();
                //}
                model.XMLQuestionAnswer = MDVUtility.GetXmlOfObject(typeof(List<SocPsyQuestionAnswerModel>), model.QuestionAnswerArray);
                string SocPsyandBehaviorHxId = BLLClinicalObj.SocPsyandBehaviorHxSaveUpdate(model);
                //Load Data
                model.SocialandBehaviorHxId = SocPsyandBehaviorHxId;
                dynamic LoadSocPsyandBehavior = JObject.Parse(LoadSocPsyandBehaviorHx(model));
                if (LoadSocPsyandBehavior.status == true)
                {
                    response = new
                    {
                        status = true,
                        SocPsyandBehaviorHxId = SocPsyandBehaviorHxId,
                        SocPsyandBehaviorHxCount = LoadSocPsyandBehavior.SocPsyandBehaviorHxCount,
                        SocPsyandBehaviorHx_JSON = LoadSocPsyandBehavior.SocPsyandBehaviorHx_JSON,
                        SocPsyQuestionAnswerCount = LoadSocPsyandBehavior.SocPsyQuestionAnswerCount,
                        SocPsyQuestionAnswer_JSON = LoadSocPsyandBehavior.SocPsyQuestionAnswer_JSON,
                        Message = AppPrivileges.Save_Message,
                    };
                }
                else
                {
                    response = new
                    {
                        SocPsyandBehaviorHxId = SocPsyandBehaviorHxId,
                        status = true,
                        Message = LoadSocPsyandBehavior.Message,
                    };
                }
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

       

        internal string detachSocPsyandBehaviorHxFromNotes(long SocialandBehaviorHxId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(SocialandBehaviorHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachSocPsyandBehaviorHxFromNotes(SocialandBehaviorHxId, notesId);
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
    }
}