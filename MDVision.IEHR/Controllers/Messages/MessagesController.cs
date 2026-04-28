using MDVision.Business.BCommon;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.CommonControls;
using MDVision.IEHR.Controls.Patient;
using MDVision.IEHR.Controls.Patient.Messages;
using MDVision.IEHR.Model.Messages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controllers.Messages
{
    public class MessagesController : ApiController
    {
        private Patient_Message_Compose _patientMessageCompose;
        public MessagesController()
        {
            _patientMessageCompose = new Patient_Message_Compose();
        }

        [HttpPost]
        public string Messages(JObject objData)
        {
            string privilegesMessage = string.Empty;
            PreRequestModel RequestModel = new PreRequestModel();
            RequestModel = new PreRequests().ApplicationServerContent();

            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            ResponseList.Add(MDVisionConstants.RequestModel, Newtonsoft.Json.JsonConvert.SerializeObject(RequestModel));

            if (RequestModel.IsLogIn)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ser.MaxJsonLength = 2147483644;
                UserMessagesModel messagesModel = ser.Deserialize<UserMessagesModel>(MDVUtility.ToStr(objData["data"]));

                if (messagesModel.CommandType.ToLower() == "save_message")
                {
                    string response = null;
                    if (messagesModel.Files.Length > 0)
                        messagesModel.FileStream = System.Convert.FromBase64String(messagesModel.Files[1]);

                    //string s3 = Convert.ToBase64String(messagesModel.FileStream);

                    if (messagesModel.IsPatientMessage == "0")
                    {
                        response = _patientMessageCompose.SaveUserMessage(messagesModel);
                    }
                    else
                    {
                        response = _patientMessageCompose.SavePatientMessage(messagesModel);
                    }

                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (messagesModel.CommandType.ToLower() == "save_practice_message")
                {
                    string response = null;
                    //if (messagesModel.Files.Length > 0)
                    //    messagesModel.FileStream = System.Convert.FromBase64String(messagesModel.Files[1]);
                    privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Message Reply", "ADD")).ToString();
                    if (string.IsNullOrEmpty(privilegesMessage))
                    {
                        response = _patientMessageCompose.SavePracticeMessage(messagesModel);
                        ResponseList.Add(MDVisionConstants.ResponseModel, response);
                    }
                    else
                    {
                        var responseObj = new
                        {
                            status = false,
                            Message = privilegesMessage
                        };
                        response = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        ResponseList.Add(MDVisionConstants.ResponseModel, response);
                    }
                }
                if (messagesModel.CommandType.ToLower() == "open_encrypted_message")
                {
                    string response = null;
                    response = _patientMessageCompose.OpenEncryptedMessage(messagesModel.PatientId, messagesModel.UserMesgId);

                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (messagesModel.CommandType.ToLower() == "validate_secret_key")
                {
                    string response = null;
                    response = _patientMessageCompose.ValidateSecretKey(messagesModel.UserMesgId, messagesModel.SecretKey);

                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (messagesModel.CommandType.ToLower() == "delete_message")
                {
                    string response = null;
                    response = _patientMessageCompose.DeleteUserMessages(messagesModel.UserMessagesIds);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (messagesModel.CommandType.ToLower() == "get_users")
                {
                    string response = null;
                    response = _patientMessageCompose.GetUsers(messagesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (messagesModel.CommandType.ToLower() == "fill_message")
                {
                    string response = null;
                    response = _patientMessageCompose.FillUserMessages(MDVUtility.ToInt64(messagesModel.UserMesgId));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (messagesModel.CommandType.ToLower() == "fill_direct_message")
                {
                    string response = null;
                    response = _patientMessageCompose.FillUserMessages(MDVUtility.ToInt64(messagesModel.UserMesgId));
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }

                if (messagesModel.CommandType.ToLower() == "fill_practice_message")
                {
                    string response = null;
                    response = _patientMessageCompose.FillPracticeMessages(messagesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (messagesModel.CommandType.ToLower() == "fill_chat_thread")
                {
                    string response = null;
                    response = _patientMessageCompose.FillChatThread(messagesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (messagesModel.CommandType.ToLower() == "load_message")
                {
                    string response = null;
                    response = _patientMessageCompose.LoadUserMessages(messagesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (messagesModel.CommandType.ToLower() == "load_tasks")
                {
                    string response = null;
                    response = _patientMessageCompose.LoadUserTasks(messagesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (messagesModel.CommandType.ToLower() == "get_messages_count")
                {
                    string response = null;
                    response = _patientMessageCompose.UserMessagesCount(messagesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
                if (messagesModel.CommandType.ToLower() == "save_direct_message")
                {
                    string response = null;
                    response = _patientMessageCompose.SaveDirectMessage(messagesModel);
                    ResponseList.Add(MDVisionConstants.ResponseModel, response);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(ResponseList);
        }

        [HttpPost]
        public string TasksCount (JObject objData)
        {
            string ReturnedResponse;
            string msgtype = "Task";
            long msgstatusId = 2;
            long entityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
            string userId = MDVUtility.ToStr(MDVSession.Current.AppUserId);

            Controls.DashBoard.DashBoardDB bb = new Controls.DashBoard.DashBoardDB();

            User_Task ts = new User_Task();
            ReturnedResponse = bb.LoadTasksCount(entityId, userId, MDVUtility.ToStr(msgstatusId), userId, msgtype);

            return ReturnedResponse;       
        }
    }
}
