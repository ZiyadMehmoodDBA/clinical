
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
using MDVision.IEHR.EMR.Model.ReviewofSystems;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Model;
using MDVision.Model.Lookups;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems
{
    public class CustomFormHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public CustomFormHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static CustomFormHelper _instance = null;
        public static CustomFormHelper Instance()
        {
            if (_instance == null)
                _instance = new CustomFormHelper();
            return _instance;
        }

        //Author: ZeeshanAK
        //Purpose: Insert Custom Form
        //Date : September 20, 2016
        public string insertCustomForm(CustomFormModel model)
        {
            CustomFormResponse responseModel = new CustomFormResponse();
            try
            {
                BLObject<string> obj = BLLClinicalObj.insertCustomForm(model);
                if (obj.Data != null && obj.Data != "-1")
                {
                    responseModel.CustomFormId = obj.Data.ToString();
                    responseModel.status = true;
                    responseModel.Message = Common.AppPrivileges.Save_Message;
                }
                else
                {
                    responseModel.status = false;
                    responseModel.CustomFormId = model.CustomFormId;
                    responseModel.Message = obj.Message == "" ? "Custom form already exist" : obj.Message;
                }


            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.CustomFormId = model.CustomFormId;

            }
            if (responseModel.status)
            {
                var response = new
                {
                    status = true,
                    CustomFormId = responseModel.CustomFormId,
                    Message = Common.AppPrivileges.Save_Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = responseModel.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        //Author: ZeeshanAK
        //Purpose: Load Custom Form
        //Date : September 21, 2016
        public string loadCustomForm(CustomFormModel model)
        {
            try
            {
                List<CustomFormModel> listCustomForm = new List<CustomFormModel>();
                BLObject<List<CustomFormModel>> obj = BLLClinicalObj.loadCustomForm(model.FormName, Convert.ToInt16(model.IsActive), model.ProviderIds, model.SpecialtyIds, model.PageNumber, model.RowsPerPage, Convert.ToInt16(model.IsFromNotes));

                if (obj.Data!=null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listCustomForm = obj.Data,
                        customFormCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        listCustomForm = "[]",
                        customFormCount = 0,
                        iTotalDisplayRecords = 0
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

        //Author: ZeeshanAK
        //Purpose: Fill Custom Form
        //Date : September 23, 2016
        public string fillCustomForm(CustomFormModel model)
        {
            try
            {
                List<CustomFormModel> listCustomForm = new List<CustomFormModel>();
                BLObject<List<CustomFormModel>> obj = BLLClinicalObj.fillCustomForm(model.CustomFormId);

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listCustomForm = obj.Data,
                        customFormCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        listCustomForm = "[]",
                        customFormCount = 0,
                        iTotalDisplayRecords = 0
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


        //Author: ZeeshanAK
        //Purpose: Delete Custom Form
        //Date : September 22, 2016
        public string deleteCustomForm(string formId)
        {
            try
            {
                if (string.IsNullOrEmpty(formId))
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
                    BLObject<string> obj = BLLClinicalObj.deleteCustomForm(formId);
                    if (obj.Data!=null && obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else if (obj.Data.IndexOf("fk_customform_Patient_customform_CustomFormId")>0)
                    {
                        var response = new
                        {
                            status = false,
                            Message = "This form is currently associated with a Patient and cannot be deleted."
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

        //Author: ZeeshanAK
        //Purpose: Update Custom Form
        //Date : September 23, 2016
        public string updateCustomForm(CustomFormModel model)
        {
            try
            {
                BLObject<string> obj = BLLClinicalObj.updateCustomForm(model);
                if (obj.Data != null && obj.Data == "")
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
                        Message = obj.Message == "" ? "FormName Already Exists" : obj.Message
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

        internal string LookupMedicationsReprot()
        {
            try
            {
                List<CategoryLookupModel> modelList=null;
                BLObject<List<CategoryLookupModel>> obj = BLLClinicalObj.LookupAttachCategory();
                if (obj.Data != null)
                {
                    modelList = obj.Data;
                }
                 
                if (modelList != null && modelList.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        categoryCount = modelList.Count,
                        categoryList_JSON = js.Serialize(modelList),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        medicationCount = 0,
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

        //Author: ZeeshanAK
        //Purpose: Update Custom Form Active Inactive
        //Date : September 26, 2016
        public string activeInactiveCustomForm(string formId, string isActive)
        {
            try
            {
                BLObject<string> obj = BLLClinicalObj.activeInactiveCustomForm(formId, isActive);
                if (obj.Data!=null&& obj.Data == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Inactive_Message
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

        #region Global Question
        /// <summary>
        //Author: Azeem Raza Tayyab
        //Purpose: Insert Global Question
        //Date : September 28, 2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string insertGlobalQuestion(GlobalQuestionModel model)
        {
            GlobalQuestionResponse responseModel = new GlobalQuestionResponse();
            try
            {
                BLObject<string> obj = BLLClinicalObj.insertGlobalQuestion(model);
                if (obj.Data != null)
                {
                    if (obj.Data.ToString() == "Question with the same name already exists.")
                    {
                        responseModel.status = false;
                        responseModel.QuestionId = obj.Data.ToString();
                        responseModel.Message = obj.Data.ToString();
                    }
                    else
                    {
                        responseModel.status = true;
                        responseModel.QuestionId = obj.Data.ToString();
                        responseModel.Message = Common.AppPrivileges.Save_Message;
                    }

                }
                else
                {
                    responseModel.status = false;
                    responseModel.QuestionId = obj.Data.ToString();
                    responseModel.Message = obj.Data.ToString();
                }


            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.QuestionId = model.QuestionId;

            }
            if (responseModel.status)
            {
                var response = new
                {
                    status = true,
                    QuestionId = responseModel.QuestionId,
                    Message = Common.AppPrivileges.Save_Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = responseModel.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        /// <summary>
        //Author: Azeem Raza Tayyab
        //Purpose: Load Global Question
        //Date : September 28, 2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadGlobalQuestion(GlobalQuestionModel model)
        {
            try
            {
                BLObject<List<GlobalQuestionModel>> obj = BLLClinicalObj.loadGlobalQuestion(model.QuestionId, model.QuestionName, Convert.ToInt16(model.IsActive), model.PageNumber, model.RowsPerPage);

                if (obj.Data!=null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listQuestions = obj.Data,
                        questionsCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        listQuestions = "[]",
                        questionsCount = 0,
                        iTotalDisplayRecords = 0
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
        public string deleteGlobalQuestion(string questionId)
        {
            try
            {
                if (string.IsNullOrEmpty(questionId))
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
                    BLObject<string> obj = BLLClinicalObj.deleteGlobalQuestion(questionId);
                    if (obj.Data != null && obj.Data == "")
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
        public string updateGlobalQuestion(GlobalQuestionModel model)
        {
            GlobalQuestionResponse responseModel = new GlobalQuestionResponse();
            try
            {
                BLObject<string> obj = BLLClinicalObj.updateGlobalQuestion(model);
                if (obj.Data != null)
                {
                    responseModel.QuestionId = obj.Data.ToString();
                    responseModel.status = true;
                    responseModel.Message = Common.AppPrivileges.Save_Message;
                }
                else
                {
                    if (obj.Message == "QuestionId")
                    {
                        responseModel.status = false;
                        responseModel.QuestionId = model.QuestionId;
                        responseModel.Message = "Question with the same name already exists.";
                    }
                    else
                    {
                        responseModel.status = false;
                        responseModel.QuestionId = model.QuestionId;
                        responseModel.Message = obj.Message;
                    }
                }


            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message = ex.Message;
                responseModel.QuestionId = model.QuestionId;

            }
            if (responseModel.status)
            {
                var response = new
                {
                    status = true,
                    QuestionId = responseModel.QuestionId,
                    Message = Common.AppPrivileges.Update_Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = responseModel.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        #endregion

        #region Global Question Group
        /// <summary>
        //Author: Azeem Raza Tayyab
        //Purpose: Insert Global Question Group
        //Date : September 28, 2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string insertGlobalQuestionGroup(GlobalQuestionGroupModel model)
        {
            GlobalQuestionGroupResponse responseModel = new GlobalQuestionGroupResponse();
            try
            {
                BLObject<string> obj = BLLClinicalObj.insertGlobalQuestionGroup(model);
                if (obj.Data != null)
                {
                    if (obj.Data == "QuestionGroupName should be unique")
                    {
                        responseModel.status = false;
                        responseModel.QuestionGroupId = model.QuestionGroupId;
                        responseModel.Message = "Question Group with the same name already exists.";
                    }
                    else
                    {
                        responseModel.QuestionGroupId = obj.Data.ToString();
                        responseModel.status = true;
                        responseModel.Message = Common.AppPrivileges.Save_Message;
                    }
                }
                else
                {
                    if (obj.Message == "QuestionGroupId")
                    {
                        responseModel.status = false;
                        responseModel.QuestionGroupId = model.QuestionGroupId;
                        responseModel.Message = "Question Group with the same name already exists.";
                    }
                    else
                    {
                        responseModel.status = false;
                        responseModel.QuestionGroupId = model.QuestionGroupId;
                        responseModel.Message = obj.Message;
                    }
                }


            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.QuestionGroupId = model.QuestionGroupId;

            }
            if (responseModel.status)
            {
                var response = new
                {
                    status = true,
                    QuestionGroupId = responseModel.QuestionGroupId,
                    Message = Common.AppPrivileges.Save_Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = responseModel.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        /// <summary>
        //Author: Azeem Raza Tayyab
        //Purpose: Load Global Question Group
        //Date : September 28, 2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string loadGlobalQuestionGroup(GlobalQuestionGroupModel model)
        {
            try
            {
                BLObject<List<GlobalQuestionGroupModel>> obj = BLLClinicalObj.loadGlobalQuestionGroup(model.QuestionGroupId, model.QuestionGroupName, model.PageNumber, model.RowsPerPage, null);

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listQuestionGroup = obj.Data,
                        questionGroupCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        listQuestionGroup = "[]",
                        questionGroupCount = 0,
                        iTotalDisplayRecords = 0
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

        public string loadGlobalQuestionGroupSavedGlobally(GlobalQuestionGroupModel model)
        {
            try
            {
                BLObject<List<GlobalQuestionGroupModel>> obj = BLLClinicalObj.loadGlobalQuestionGroup(model.QuestionGroupId, model.QuestionGroupName, model.PageNumber, model.RowsPerPage, true);

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listQuestionGroup = obj.Data,
                        questionGroupCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        listQuestionGroup = "[]",
                        questionGroupCount = 0,
                        iTotalDisplayRecords = 0
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
        public string deleteGlobalQuestionGroup(string questionGroupId)
        {
            try
            {
                if (string.IsNullOrEmpty(questionGroupId))
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
                    BLObject<string> obj = BLLClinicalObj.deleteGlobalQuestionGroup(questionGroupId);
                    if (obj.Data != null && obj.Data == "")
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
        public string updateGlobalQuestionGroup(GlobalQuestionGroupModel model)
        {
            GlobalQuestionGroupResponse responseModel = new GlobalQuestionGroupResponse();
            try
            {
                BLObject<string> obj = BLLClinicalObj.updateGlobalQuestionGroup(model);
                if (obj.Data != null)
                {
                    responseModel.QuestionGroupId = obj.Data.ToString();
                    responseModel.status = true;
                    responseModel.Message = Common.AppPrivileges.Update_Message;
                }
                else
                {
                    if (obj.Message == "QuestionGroupId")
                    {
                        responseModel.status = false;
                        responseModel.QuestionGroupId = model.QuestionGroupId;
                        responseModel.Message = "Question with the same name already exists.";
                    }
                    else
                    {
                        responseModel.status = false;
                        responseModel.QuestionGroupId = model.QuestionGroupId;
                        responseModel.Message = obj.Message;
                    }
                }


            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.QuestionGroupId = model.QuestionGroupId;

            }
            if (responseModel.status)
            {
                var response = new
                {
                    status = true,
                    QuestionGroupId = responseModel.QuestionGroupId,
                    Message = Common.AppPrivileges.Update_Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = false,
                    Message = responseModel.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string fillGlobalQuestion(GlobalQuestionGroupModel model)
        {
            try
            {
                BLObject<List<GlobalQuestionGroupModel>> obj = BLLClinicalObj.fillGlobalQuestion(model.QuestionGroupId);

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listGlobalQuestion = obj.Data,
                        globalQuestionCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        listGlobalQuestion = "[]",
                        globalQuestionCount = 0,
                        iTotalDisplayRecords = 0
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion



        public string attachCustomFormsWithNotes(string CustomFormId, string NotesId)
        {
            CustomFormResponse responseModel = new CustomFormResponse();
            try
            {
                BLObject<string> obj = BLLClinicalObj.attachCustomFormsWithNotes(CustomFormId, NotesId);
                if (obj.Data != null && obj.Data != "-1")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message
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
        internal string detachCustomFormFromNotes(string CustomFormId, string NotesId, string CustomFormDocName)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(CustomFormId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachCustomFormFromNotes(CustomFormId, NotesId, CustomFormDocName);
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

        public string DeleteProblemList(string problemListIds, string noteId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(problemListIds)))
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

                    BLObject<string> obj = BLLClinicalObj.deleteCustomFormProblemLists(problemListIds, MDVUtility.ToInt64(noteId));
                    if (obj.Data == "")
                    {
                        //HttpContext.Current.Session["DeleteProblemList4DrFirst"] = model;
                      //  MDVSession.Current.DeleteProblemList4DrFirst = model; // check it
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
        public string DeleteProcedures(string procedureIds, string noteId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(procedureIds)))
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
                    BLObject<string> obj = BLLClinicalObj.deleteCustomFormProcedures(procedureIds, MDVUtility.ToInt64(noteId));
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

        public string loadFavoriteListCustomForm(CustomFormModel model)
        {
            try
            {
                List<CustomFormModel> listCustomForm = new List<CustomFormModel>();
                BLObject<List<CustomFormModel>> obj = BLLClinicalObj.loadFavoriteListCustomForm(MDVUtility.ToInt64(model.FavoriteListCustomFormId), MDVUtility.ToInt64(model.FavoriteListId), MDVUtility.ToInt64(model.ProviderId), MDVUtility.ToInt16(model.IsActive), MDVUtility.ToInt64(model.PageNumber), MDVUtility.ToInt64(model.RowsPerPage));

                if (obj.Data != null && obj.Data.Count != 0)
                {

                    var response = new
                    {
                        status = true,
                        listCustomForm = obj.Data,
                        customFormCount = obj.Data.Count,
                        iTotalDisplayRecords = obj.Data[0].RecordCount
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        listCustomForm = "[]",
                        customFormCount = 0,
                        iTotalDisplayRecords = 0
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
    }
}