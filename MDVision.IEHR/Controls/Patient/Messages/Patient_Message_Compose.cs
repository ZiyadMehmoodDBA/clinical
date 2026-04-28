using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using Newtonsoft.Json;
using MDVision.IEHR.Model.Messages;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using MDVision.IEHR.EMR.HL7Folder.Summary;
using System.Web.Script.Serialization;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using Ionic.Zip;

namespace MDVision.IEHR.Controls.Patient.Messages
{
    public class Patient_Message_Compose
    {
        private BLLMessage BLLMessageObj = null;
        private BLLAdminSecurity BLLAdminSecurityObj = null;

        public Patient_Message_Compose()
        {
            BLLMessageObj = new BLLMessage();
            BLLAdminSecurityObj = new BLLAdminSecurity();
        }

        //#region Singleton
        //private Patient_Message_Compose _obj = null;
        //private BLLMessage BLLMessageObj = null;
        //public Patient_Message_Compose Instance()
        //{
        //    if (_obj == null)
        //    {
        //        _obj = new Patient_Message_Compose();
        //        BLLMessageObj = new BLLMessage();
        //    }
        //    return _obj;
        //}
        //#endregion

        #region "Functions"
        public string SaveUserMessage(UserMessagesModel model)
        {
            try
            {
                DSMessage dsMessage = new DSMessage();
                DSMessage.UserMessagesRow dr = dsMessage.UserMessages.NewUserMessagesRow();

                //if (!string.IsNullOrEmpty(model.hfMessageFrom))
                dr.AssignedFromId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);

                if (!string.IsNullOrEmpty(model.hfMessageTo))
                    dr.AssignedToId = MDVUtility.ToInt64(model.hfMessageTo);
                if (!string.IsNullOrEmpty(model.AttatchedPatientId))
                    dr.AttatchedPatientId = MDVUtility.ToInt64(model.AttatchedPatientId);
                if (!string.IsNullOrEmpty(model.Subject))
                    dr.Subject = MDVUtility.ToStr(model.Subject);
                if (!string.IsNullOrEmpty(model.MessageDtl123))
                    dr.MessageDetail = MDVUtility.ToStr(model.MessageDtl123);
                if (!string.IsNullOrEmpty(model.Priority))
                    dr.PriorityId = MDVUtility.ToInt(model.Priority);
                dr.FileStream = model.FileStream;
                if (!string.IsNullOrEmpty(model.FileType))
                    dr.FileType = model.FileType;
                if (!string.IsNullOrEmpty(model.FilePath))
                    dr.FilePath = model.FilePath;
                dr.PatientLetterId = MDVUtility.ToInt64(model.PatientLetterId);
                dr.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.IsRead = false;

                #region Database Insertion
                dsMessage.UserMessages.AddUserMessagesRow(dr);
                BLObject<DSMessage> obj = BLLMessageObj.InsertUserMessage(dsMessage);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        UserMessageId = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.UserMessagesIdColumn.ColumnName],
                        //AccountNumber = dsMessage.Tables[dsMessage.Patients.TableName].Rows[0][dsMessage.Patients.AccountNumberColumn.ColumnName]
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

        public string UserMessagesCount(UserMessagesModel model)
        {
            try
            {
                DSMessage dsMessage = null;
                BLObject<DSMessage> obj;
                obj = BLLMessageObj.LoadUserMessagesCount(MDVUtility.ToInt64(MDVSession.Current.AppUserId), MDVUtility.ToInt64(model.PatientId));

                dsMessage = obj.Data;
                if (obj.Data != null)
                {


                    if (dsMessage.Tables[dsMessage.MessagesCount.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.MessagesCount.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
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
        public string GetUsers(UserMessagesModel model)
        {
            try
            {
                DSUserLookup dsuser = null;
                BLObject<DSUserLookup> obj;

                obj = BLLAdminSecurityObj.LookupFullUserName("1", model.Username);
                dsuser = obj.Data;
                if (obj.Data != null)
                {
                    if (dsuser.Tables[dsuser.UsersPractices.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            UserCount = dsuser.Tables[dsuser.UsersPractices.TableName].Rows.Count,
                            UserLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsuser.Tables[dsuser.UsersPractices.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            UserCount = 0,
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
        public string SavePracticeMessage(UserMessagesModel model)
        {
            try
            {
                DSMessage dsMessage = new DSMessage();
                DSMessage.UserMessagesRow dr = dsMessage.UserMessages.NewUserMessagesRow();

                //if (!string.IsNullOrEmpty(model.hfMessageFrom))
                dr.AssignedFromId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);

                if (!string.IsNullOrEmpty(model.hfMessageTo))
                    dr.AssignedToId = MDVUtility.ToInt64(model.hfMessageTo);
                if (!string.IsNullOrEmpty(model.AttatchedPatientId))
                    dr.AttatchedPatientId = MDVUtility.ToInt64(model.AttatchedPatientId);
                if (!string.IsNullOrEmpty(model.Subject))
                    dr.Subject = MDVUtility.ToStr(model.Subject);
                if (!string.IsNullOrEmpty(model.MessageDtl123))
                    dr.MessageDetail = MDVUtility.ToStr(model.MessageDtl123);
                if (!string.IsNullOrEmpty(model.Priority))
                    dr.PriorityId = MDVUtility.ToInt(model.Priority);
                //dr.FileStream = model.FileStream;
                //if (!string.IsNullOrEmpty(model.FileType))
                //    dr.FileType = model.FileType;
                //if (!string.IsNullOrEmpty(model.FilePath))
                //    dr.FilePath = model.FilePath;
                dr.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.IsRead = false;
                dr.Entityid = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.MessagerType = model.MessageType;
                dr.UniqueNumber = model.UniqueNumber;
                dr.UserNameWithPractice = model.MessageTo;
                dr.usernamewithpracticefrom = model.UsernameFrom;
                dr.PatientLetterId = MDVUtility.ToInt64(model.PatientLetterId);
                dr.IsMUAlertUpdated = false;
                #region Database Insertion
                dsMessage.UserMessages.AddUserMessagesRow(dr);

                if (model.MessageType == "Patient")
                {
                    dsMessage = EncryptMessageAndGenerateHash(dsMessage);
                }
                BLObject<DSMessage> obj = BLLMessageObj.InsertPracticeMessage(dsMessage);
                if (obj.Data != null)
                {
                    string UserMessageId = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.UserMessagesIdColumn.ColumnName].ToString();
                    if (UserMessageId != "")
                    {
                        SaveFiles(model, MDVUtility.ToInt64(UserMessageId));
                    }

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        UserMessageId = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.UserMessagesIdColumn.ColumnName],
                        IsMUAlertUpdated = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.IsMUAlertUpdatedColumn.ColumnName],
                        //AccountNumber = dsMessage.Tables[dsMessage.Patients.TableName].Rows[0][dsMessage.Patients.AccountNumberColumn.ColumnName]
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

        public string SaveFiles(UserMessagesModel model, Int64 MessageID)
        {
            try
            {

                DSMessage dsMessage = new DSMessage();

                //List<object> FileStreamlist = JsonConvert.DeserializeObject<List<object>>(model.FileStream);
                List<string> FileTypelist = JsonConvert.DeserializeObject<List<string>>(model.FileType);
                List<string> FilePathlist = JsonConvert.DeserializeObject<List<string>>(model.FilePath);

                //if (!string.IsNullOrEmpty(model.hfMessageFrom))
                for (int i = 0; i < FileTypelist.Count; i++)
                {
                    DSMessage.DocumentsStreamRow dr = dsMessage.DocumentsStream.NewDocumentsStreamRow();
                    // dr.DocumentsStrId = MDVUtility.ToInt32(MDVSession.Current.AppUserId);


                    dr.UserMessagesId = MessageID;
                    dr.FileStream = System.Convert.FromBase64String(model.Files[i]);
                    if (!string.IsNullOrEmpty(FileTypelist[i]))
                        dr.FileType = FileTypelist[i];
                    if (!string.IsNullOrEmpty(FilePathlist[i]))
                        dr.FilePath = FilePathlist[i];

                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    dsMessage.DocumentsStream.AddDocumentsStreamRow(dr);
                }
                #region Database Insertion

                BLObject<DSMessage> obj = BLLMessageObj.InsertUserMessageDocumentStream(dsMessage);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        // message = Common.AppPrivileges.Save_Message,
                        //  UserMessageId = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.UserMessagesIdColumn.ColumnName],
                        //AccountNumber = dsMessage.Tables[dsMessage.Patients.TableName].Rows[0][dsMessage.Patients.AccountNumberColumn.ColumnName]
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
        public string DeleteUserMessages(string userMsgIds)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(userMsgIds)))
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
                    BLObject<string> obj = BLLMessageObj.DeleteUserMessage(MDVUtility.ToStr(userMsgIds));
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

        public string FillUserMessages(Int64 UserMsgId)
        {

            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(UserMsgId)))
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
                    DSMessage dsMessage = null;
                    BLObject<DSMessage> obj = BLLMessageObj.LoadUserMessage(UserMsgId, 0, "", "", 0, MDVUtility.ToInt64(MDVSession.Current.AppUserId), 1, 15);
                    if (obj.Data != null)
                    {
                        string htmlBase64 = string.Empty;
                        string xmlBase64 = string.Empty;
                        dsMessage = obj.Data;
                        if (dsMessage.Tables[dsMessage.UserMessages.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0];

                            List<string[]> documents = new List<string[]>();
                            foreach (DataRow dtr in dsMessage.Tables[dsMessage.DocumentsStream.TableName].Rows)
                            {
                                string[] Doc = new string[3];
                                byte[] byteArr = dtr[dsMessage.DocumentsStream.FileStreamColumn].ToFormatedByteArray();
                                if (byteArr != null)
                                {
                                    string strBase64 = Convert.ToBase64String(byteArr);
                                    Doc[0] = strBase64;
                                    Doc[1] = dtr[dsMessage.DocumentsStream.FileTypeColumn].ToString();
                                    Doc[2] = dtr[dsMessage.DocumentsStream.FilePathColumn].ToString();
                                    documents.Add(Doc);

                                    if (Doc[2].ToLower().Contains("zip"))
                                    {
                                        Stream stream = new MemoryStream(byteArr);
                                        byte[] ccdaBytes = null;
                                        using (ZipFile zip = ZipFile.Read(stream))
                                        {
                                            foreach (ZipEntry entry in zip)
                                            {
                                                if (entry.FileName.ToLower().Contains("xml") && !entry.FileName.ToLower().Contains("metadata"))
                                                {
                                                    XmlDocument doc = new XmlDocument();
                                                    using (var outStream = new MemoryStream())
                                                    {
                                                        entry.Extract(outStream);
                                                        ccdaBytes = outStream.ToArray();
                                                    }

                                                    using (MemoryStream ms = new MemoryStream(ccdaBytes))
                                                    {
                                                        doc.Load(ms);
                                                    }
                                                    if (doc.GetElementsByTagName("ClinicalDocument").Count > 0) // XML is a CCDA
                                                    {
                                                        string strBase64_CCDA = Convert.ToBase64String(ccdaBytes);
                                                        string[] CCDA = new string[3];
                                                        var fileName = entry.FileName.Split('/');
                                                        CCDA[0] = strBase64_CCDA;
                                                        CCDA[1] = "application/xml";
                                                        CCDA[2] = fileName[fileName.Length - 1];
                                                        documents.Add(CCDA);
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            
                            var jsonSerialiser = new JavaScriptSerializer();
                            var docsJson = jsonSerialiser.Serialize(documents);

                            var keyValues = new Dictionary<string, string>
                        {
                            { "MessageTo", MDVUtility.ToStr(dr[dsMessage.UserMessages.AssignedFromNameColumn.ColumnName])},
                            //{ "AttatchPatient", MDVUtility.ToStr(dr[dsMessage.UserMessages.AccountNumberColumn.ColumnName] + " - " + dr[dsMessage.UserMessages.LastNameColumn.ColumnName] + ", " + dr[dsMessage.UserMessages.FirstNameColumn.ColumnName])},
                            { "AttatchedPatientId", MDVUtility.ToStr(dr[dsMessage.UserMessages.AssignedFromIdColumn.ColumnName])},
                            { "Subject", MDVUtility.ToStr(dr[dsMessage.UserMessages.SubjectColumn.ColumnName])},
                            { "Priority", MDVUtility.ToStr(dr[dsMessage.UserMessages.PriorityIdColumn.ColumnName])},
                            { "MessageDtl", MDVUtility.ToStr(dr[dsMessage.UserMessages.MessageDetailColumn.ColumnName])},
                            { "hfMessageTo", MDVUtility.ToStr(dr[dsMessage.UserMessages.AssignedFromIdColumn.ColumnName])},
                            //{ "Base64FileStream", MDVUtility.ToStr(dr["Base64FileStream"])},
                            //Start || 24 May, 2016 || ZeeshanAK || Changes for showing HTML file with XML for phiMail messages
                                //{ "Base64FileStreamHTML", htmlBase64},
                                { "phiMAilAddress", MDVUtility.ToStr(dr[dsMessage.UserMessages.AssignedFromNameColumn.ColumnName])},
                            //End   || 24 May, 2016 || ZeeshanAK || Changes for showing HTML file with XML for phiMail messages
                                {"Documents", MDVUtility.ToStr(docsJson )},

                            { "FileType", MDVUtility.ToStr(dr[dsMessage.DocumentsStream.FileTypeColumn.ColumnName])},
                            { "FilePath", MDVUtility.ToStr(dr[dsMessage.DocumentsStream.FilePathColumn.ColumnName])},
                            { "FileStreamName", MDVUtility.ToStr(dr[dsMessage.DocumentsStream.FileStreamColumn.ColumnName])},
                            { "FileTypeName", MDVUtility.ToStr(dr[dsMessage.DocumentsStream.FileTypeColumn.ColumnName])},
                            { "FilePathName", MDVUtility.ToStr(dr[dsMessage.DocumentsStream.FilePathColumn.ColumnName])},
                            { "ProviderId", MDVUtility.ToStr(dr[dsMessage.UserMessages.ProviderIdColumn.ColumnName])},
                            { "CreatedBy", MDVUtility.ToStr(dr[dsMessage.UserMessages.CreatedByColumn.ColumnName])},
                            { "IsPatientMessage", MDVUtility.ToStr(dr[dsMessage.UserMessages.IsPatientMessageColumn.ColumnName])},
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                userMessagesFill_JSON = js.Serialize(keyValues)
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
                            Message = obj.Message
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
        public string FillPracticeMessages(UserMessagesModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.UserMesgId)))
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
                    DSMessage dsMessage = null;
                    BLObject<DSMessage> obj = BLLMessageObj.FillPracticeMessage(MDVUtility.ToLong(model.UserMesgId), 0, "", "", 0, MDVUtility.ToInt64(MDVSession.Current.AppUserId), model.MessageType, 1, 15);
                    if (obj.Data != null)
                    {
                        dsMessage = obj.Data;
                        if (dsMessage.Tables[dsMessage.UserMessages.TableName].Rows.Count > 0)
                        {

                            if (model.MessageType == "Patient" && !string.IsNullOrEmpty(dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0][dsMessage.UserMessages.EncryptionKeyColumn.ColumnName].ToString()))
                            {
                                dsMessage = DecryptMessage(dsMessage, false);
                            }
                            DataRow dr = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows[0];
                            //-----------------

                            //String HtmlDocument = MDVUtility.ToStr(dr[dsMessage.UserMessages.MessageDetailColumn.ColumnName]);
                            //string[] ImageSources = HtmlDocument.Split(new string[] { "src=" }, StringSplitOptions.None);
                            //for (int i = 0; i < ImageSources.Length; i++)
                            //{
                            //    if (ImageSources[i].Contains("data:image"))
                            //    {
                            //        string ImageString = ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[0];
                            //        ImageSources[i] = "src=" + ImageString.Replace(" ", "+") + " " + ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[1];
                            //    }
                            //}

                            //HtmlDocument = String.Join("", ImageSources);

                            List<string[]> documents = new List<string[]>();
                            foreach (DataRow dtr in dsMessage.Tables[dsMessage.DocumentsStream.TableName].Rows)
                            {
                                string[] Doc = new string[3];
                                byte[] byteArr = dtr["FileStream"] as byte[];
                                if (byteArr != null)
                                {

                                    string strBase64 = Convert.ToBase64String(byteArr);
                                    Doc[0] = strBase64;
                                    Doc[1] = dtr["FileType"] as string;
                                    Doc[2] = dtr["FilePath"] as string;
                                }
                                documents.Add(Doc);
                            }
                            var jsonSerialiser = new JavaScriptSerializer();
                            jsonSerialiser.MaxJsonLength = 2147483644;
                            var docsJson = jsonSerialiser.Serialize(documents);

                            //foreach (DataRow dtr in dsMessage.Tables[dsMessage.DocumentsStream.TableName].Rows)
                            //{
                            //    byte[] byteArr = dtr["FileStream"] as byte[];
                            //    dsMessage.Tables[dsMessage.DocumentsStream.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                            //    if (byteArr != null)
                            //    {
                            //        string strBase64 = Convert.ToBase64String(byteArr);
                            //        // Add a New Column to Store the Base64 String
                            //        //if (!dtr.Table.Columns.Contains("Base64FileStream"))
                            //        //{
                            //        //    dsMessage.Tables[dsMessage.UserMessages.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                            //        //}
                            //        dtr["Base64FileStream"] = strBase64;
                            //    }
                            //}
                            ////Start || 24 May, 2016 || ZeeshanAK || Changes for showing HTML file with XML for phiMail messages

                            //dsMessage.Tables[dsMessage.UserMessages.TableName].Columns.Add("Base64FileStreamHTML", typeof(System.String));

                            ////Converting XML to HTML
                            //if (!DBNull.Value.Equals(dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.FileStreamColumn]))
                            //{
                            //    string result = System.Text.Encoding.UTF8.GetString(dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.FileStreamColumn].ToFormatedByteArray());

                            //    var folderPath = System.Web.HttpContext.Current.Server.MapPath(MDVision.IEHR.MDVSession.Current.ImagePath);
                            //    var htmlfileName = "phiMail_Attachment.html";
                            //    var xmlfileName = "phiMail_Attachment.xml";
                            //    var htmlFilePath = string.Format(@"{0}\{1}", folderPath, htmlfileName);
                            //    var xmlFilePath = string.Format(@"{0}\{1}", folderPath, xmlfileName);
                            //    var stylesheetPath = string.Empty;

                            //    //Save XML Content
                            //    File.WriteAllText(xmlFilePath, result);

                            //    // Create a resolver with default credentials.
                            //    XmlUrlResolver resolver = new XmlUrlResolver();
                            //    resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;
                            //    // transform Xml file to HTML
                            //    XslCompiledTransform transform = new XslCompiledTransform();
                            //    XsltSettings settings = new XsltSettings() { EnableDocumentFunction = true };
                            //    // load up the stylesheet xslPath
                            //    if (result.IndexOf("ContinuityOfCareRecord") > -1)
                            //    {
                            //        stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\CCR.xsl";
                            //    }
                            //    else if (result.IndexOf("ClinicalDocument") > -1)
                            //    {
                            //        stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\CDA.xsl";
                            //    }
                            //    transform.Load(stylesheetPath, settings, resolver);
                            //    // perform the transformation
                            //    transform.Transform(xmlFilePath, htmlFilePath);
                            //    string content = File.ReadAllText(htmlFilePath);
                            //    content = content.Replace("�", " ").Replace("div {  width: 80%;}", "");


                            //    // dsMessage.Tables[dsMessage.UserMessages.TableName].Columns.Add("Base64FileStreamHTML", typeof(System.String));
                            //    dsMessage.UserMessages.Rows[0]["Base64FileStreamHTML"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));

                            //}
                            ////End   || 24 May, 2016 || ZeeshanAK || Changes for showing HTML file with XML for phiMail messages

                            //-----------------
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtTo", MDVUtility.ToStr(dr[dsMessage.UserMessages.AssignedToIdColumn])},
                            //{ "AttatchPatient", MDVUtility.ToStr(dr[dsMessage.UserMessages.AccountNumberColumn.ColumnName] + " - " + dr[dsMessage.UserMessages.LastNameColumn.ColumnName] + ", " + dr[dsMessage.UserMessages.FirstNameColumn.ColumnName])},
                            { "AttatchedPatientId", MDVUtility.ToStr(dr[dsMessage.UserMessages.AttatchedPatientIdColumn.ColumnName])},
                            { "Subject", MDVUtility.ToStr(dr[dsMessage.UserMessages.SubjectColumn.ColumnName])},
                            { "Priority", MDVUtility.ToStr(dr[dsMessage.UserMessages.PriorityIdColumn.ColumnName])},
                            { "MessageDtl", MDVUtility.ToStr(dr[dsMessage.UserMessages.MessageDetailColumn.ColumnName])},
                            { "hfMessageTo", MDVUtility.ToStr(dr[dsMessage.UserMessages.AssignedFromIdColumn.ColumnName])},
                            //{ "Base64FileStream", MDVUtility.ToStr(dr["Base64FileStream"])},
                            //Start || 24 May, 2016 || ZeeshanAK || Changes for showing HTML file with XML for phiMail messages
                            //{ "Base64FileStreamHTML", MDVUtility.ToStr(dr["Base64FileStreamHTML"])},
                            { "phiMAilAddress", MDVUtility.ToStr(dr[dsMessage.UserMessages.AssignedFromNameColumn.ColumnName])},
                            //End   || 24 May, 2016 || ZeeshanAK || Changes for showing HTML file with XML for phiMail messages
                             { "hfRace", MDVUtility.ToStr(dr[dsMessage.UserMessages.RaceColumn.ColumnName])},
                            { "hfEthnicity", MDVUtility.ToStr(dr[dsMessage.UserMessages.EthnicityColumn.ColumnName])},
                            { "FileType", MDVUtility.ToStr(dr[dsMessage.UserMessages.FileTypeColumn.ColumnName])},
                            { "FilePath", MDVUtility.ToStr(dr[dsMessage.UserMessages.FilePathColumn.ColumnName])},
                            { "FileStreamName", MDVUtility.ToStr(dr[dsMessage.UserMessages.FileStreamColumn.ColumnName])},
                            { "FileTypeName", MDVUtility.ToStr(dr[dsMessage.UserMessages.FileTypeColumn.ColumnName])},
                            { "FilePathName", MDVUtility.ToStr(dr[dsMessage.UserMessages.FilePathColumn.ColumnName])},
                            { "ProviderId", MDVUtility.ToStr(dr[dsMessage.UserMessages.ProviderIdColumn.ColumnName])},
                            { "CreatedBy", MDVUtility.ToStr(dr[dsMessage.UserMessages.CreatedByColumn.ColumnName])},
                            { "IsPatientMessage", MDVUtility.ToStr(dr[dsMessage.UserMessages.IsPatientMessageColumn.ColumnName])},
                            { "txtaccountnumber", MDVUtility.ToStr(dr[dsMessage.UserMessages.AccountNumberColumn.ColumnName])},
                            { "txtnewPatientName", MDVUtility.ToStr(dr[dsMessage.UserMessages.PatientNameColumn.ColumnName])},
                            { "CreatedOn", MDVUtility.ToStr(dr[dsMessage.UserMessages.CreatedOnColumn.ColumnName])},
                            { "Priorityname", MDVUtility.ToStr(dr[dsMessage.UserMessages.PriorityColumn.ColumnName])},
                            {"Documents", MDVUtility.ToStr(docsJson )},
                            { "IsTask", MDVUtility.ToStr(dr[dsMessage.UserMessages.IsTaskColumn.ColumnName])},
                            { "UserMessageId", MDVUtility.ToStr(dr[dsMessage.UserMessages.UserMessagesIdColumn.ColumnName])},
                            { "PatientLetterId", MDVUtility.ToStr(dr[dsMessage.UserMessages.PatientLetterIdColumn.ColumnName])},
                            { "LetterTemplateName", MDVUtility.ToStr(dr[dsMessage.UserMessages.LetterTemplateNameColumn.ColumnName])},
                             { "LetterStatus", MDVUtility.ToStr(dr[dsMessage.UserMessages.StatusColumn.ColumnName])},
                             { "UniqueNumber", MDVUtility.ToStr(dr[dsMessage.UserMessages.UniqueNumberColumn.ColumnName])},
                             { "UserNameWithPractice", MDVUtility.ToStr(dr[dsMessage.UserMessages.UserNameWithPracticeColumn.ColumnName])},
                             { "UserNameWithPracticeFrom", MDVUtility.ToStr(dr[dsMessage.UserMessages.usernamewithpracticefromColumn.ColumnName])},
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            js.MaxJsonLength = 2147483644;
                            var response = new
                            {
                                status = true,
                                userMessagesFill_JSON = js.Serialize(keyValues)
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
                            Message = obj.Message
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
        public string FillChatThread(UserMessagesModel model)
        {
            try
            {
                DSMessage dsMessage = null;
                BLObject<DSMessage> obj;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.UserMesgId)))
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

                    obj = BLLMessageObj.FillChatThread(MDVUtility.ToLong(model.UserMesgId), MDVUtility.ToInt64(MDVSession.Current.AppUserId), model.MessageType);
                }
                dsMessage = obj.Data;
                dsMessage = DecryptMessage(dsMessage, true);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        // BlockHoursCount = dsMessage.Tables[dsMessage.SchBlockHours.TableName].Rows.Count,
                        // iTotalDisplayRecords = (dsSchedule.SchBlockHours.Rows.Count > 0) ? dsSchedule.SchBlockHours.Rows[0][dsSchedule.SchBlockHours.RecordCountColumn.ColumnName] : 0,
                        ChatThreadLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.UserMessageReply.TableName]),
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        BlockHoursCount = 0,
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
        public string LoadUserMessages(UserMessagesModel model)
        {
            try
            {
                DSMessage dsMessage = null;
                BLObject<DSMessage> obj;

                obj = BLLMessageObj.LoadUserMessage(0, MDVUtility.ToInt32(model.Priority), model.MessageName, model.MessageDate, MDVUtility.ToInt64(model.AttatchedPatientId), MDVUtility.ToInt64(MDVSession.Current.AppUserId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsMessage = obj.Data;
                if (obj.Data != null)
                {
                    if (dsMessage.Tables[dsMessage.UserMessages.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.RecordCountColumn.ColumnName],
                            iTotalDisplayRecords = dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.RecordCountColumn.ColumnName],
                            MessageLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.UserMessages.TableName]),
                            MessageLogCount = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows.Count,
                            MessageLogLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.UserMessages.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            MessageLogCount = 0,
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



        public string LoadUserTasks(UserMessagesModel model)
        {
            try
            {
                DSMessage dsMessage = null;
                BLObject<DSMessage> obj;

                obj = BLLMessageObj.LoadUserTask(MDVUtility.ToInt64(model.UserMesgId));
                dsMessage = obj.Data;
                if (obj.Data != null)
                {
                    if (dsMessage.Tables[dsMessage.PatMessages.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            TaskCount = dsMessage.Tables[dsMessage.PatMessages.TableName].Rows.Count,
                            TaskLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.PatMessages.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            TaskCount = 0,
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

        public string SavePatientMessage(UserMessagesModel model)
        {
            try
            {
                DSMessage dsMessage = new DSMessage();
                DSMessage.PatMessagesRow dr = dsMessage.PatMessages.NewPatMessagesRow();

                if (!string.IsNullOrEmpty(model.AttatchedPatientId))
                    dr.PatientId = MDVUtility.ToInt64(model.AttatchedPatientId);
                if (!string.IsNullOrEmpty(model.MessageDtl123))
                    dr.MsgDetail = MDVUtility.ToStr(model.MessageDtl123);
                //dr.IsDeleted = 0;
                dr.UserId = MDVUtility.ToStr(MDVSession.Current.AppUserId);
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.Subject = model.Subject;
                dr.IsRead = false;

                #region Database Insertion
                dsMessage.PatMessages.AddPatMessagesRow(dr);
                BLObject<DSMessage> obj = BLLMessageObj.InsertPatientMessage(dsMessage);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        UserMessageId = dsMessage.Tables[dsMessage.PatMessages.TableName].Rows[0][dsMessage.PatMessages.PatMsgIdColumn.ColumnName],
                        //AccountNumber = dsMessage.Tables[dsMessage.Patients.TableName].Rows[0][dsMessage.Patients.AccountNumberColumn.ColumnName]
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


        #region Direct Messaging
        public string SaveDirectMessage(UserMessagesModel model)
        {
            try
            {
                string phiMail_RemoteUser = model.msgType == "direct" ? ConfigurationManager.AppSettings["phiMail_DirectUser"] : ConfigurationManager.AppSettings["phiMail_EdgeUser"];
                dynamic send = SendEmailThroughPhiMailConnector(model, phiMail_RemoteUser);

                if (send.status)
                {
                    DSMessage dsMessage = new DSMessage();
                    DSMessage.DirectMessagesRow dr = dsMessage.DirectMessages.NewDirectMessagesRow();


                    if (!string.IsNullOrEmpty(phiMail_RemoteUser))
                        dr.EmailFrom = MDVUtility.ToStr(phiMail_RemoteUser);
                    if (!string.IsNullOrEmpty(model.EmailTo))
                        dr.EmailTo = MDVUtility.ToStr(model.EmailTo);
                    if (!string.IsNullOrEmpty(model.Subject))
                        dr.Subj = MDVUtility.ToStr(model.Subject);

                    dr.DirectMsgId = MDVUtility.ToStr(send.Message);
                    dr.DateTime = DateTime.Now;

                    if (!string.IsNullOrEmpty(model.MessageDetail))
                        dr.MessageDetail = MDVUtility.ToStr(model.MessageDetail);

                    #region Database Insertion
                    dsMessage.DirectMessages.AddDirectMessagesRow(dr);

                    BLObject<DSMessage> obj = BLLMessageObj.InsertDirectMessage(dsMessage);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
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
                        Message = send.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
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

        private object SendEmailThroughPhiMailConnector(UserMessagesModel messageModel, string phiMail_RemoteUser)
        {
            string phiMail_RemotePassword = ConfigurationManager.AppSettings["phiMail_SadboxPassword"];
            try
            {
                using (PhiMailConnector pc = new PhiMailConnector(ConfigurationManager.AppSettings["phiMail_RemoteHost"], Convert.ToInt32(ConfigurationManager.AppSettings["phiMail_RemotePort"])))
                {
                    try
                    {
                        pc.AuthenticateUser(phiMail_RemoteUser, phiMail_RemotePassword);
                        try
                        {
                            pc.AddRecipient(messageModel.EmailTo);
                        }
                        catch (Exception e)
                        {
                            var output = new
                            {
                                status = false,
                                Message = "Aborting send; could not add recipient: " + e.Message
                            };
                            return output;
                        }
                        pc.SetSubject(messageModel.Subject);

                        // Add the main body of the message.
                        if (messageModel.MessageDetail.Length > 0)
                        {
                            pc.AddText(messageModel.MessageDetail);
                        }

                        List<string> FileTypelist = JsonConvert.DeserializeObject<List<string>>(messageModel.FileType);
                        List<string> FilePathlist = JsonConvert.DeserializeObject<List<string>>(messageModel.FilePath);

                        if (FileTypelist.Count > 0)
                        {
                            if (FileTypelist.Count > 1)
                            {
                                byte[] zipStream = null;
                                using (var stream = new MemoryStream())
                                {
                                    using (ZipFile zip = new ZipFile())
                                    {
                                        //zip.Password = messageModel.Password;
                                        for (int i = 0; i < FileTypelist.Count; i++)
                                        {
                                            zip.AddEntry(FilePathlist[i], System.Convert.FromBase64String(messageModel.Files[i]));
                                        }
                                        zip.Save(stream);
                                        zipStream = stream.ToArray();
                                        pc.AddRaw(zipStream, "Files.zip");
                                    }
                                }

                            }
                            else
                            {

                                //for (int i = 0; i < FileTypelist.Count; i++)
                                //{
                                pc.AddRaw(System.Convert.FromBase64String(messageModel.Files[0]), FilePathlist[0]);
                                //}
                            }
                        }

                        // Optionally, request a final delivery notification message.
                        // Note that not all HISPs can provide this notification when requested.
                        // If the receiving HISP does not support this feature, the message will
                        // result in a failure notification after the timeout period has elapsed.
                        // Additional information on final delivery notification can be found in
                        // the API Guide.
                        // This command will override the default setting set by the server.
                        //
                        // The default setting is false.
                        pc.SetDeliveryNotification(true);

                        // Send the message. sendRes will contain one entry for each recipient.
                        // If more than one recipient was specified, then each would have an entry.
                        List<PhiMailConnector.SendResult> sendRes = pc.Send();

                        if (sendRes[0].Succeeded)
                        {
                            // The MessageID is unique for each message/recipient pair and should 
                            // be saved since any future status notifications that might be received
                            // from the phiMail server relating to this message will also reference 
                            // this MessageID.

                            var output = new
                            {
                                status = true,
                                Message = sendRes[0].MessageId
                            };
                            return output;
                        }
                        else
                        {

                            // ...handle a send failure...
                            var output = new
                            {
                                status = false,
                                Message = sendRes[0].ErrorText
                            };
                            // Clear the current outgoing message on failure.
                            // Outgoing messages are automatically cleared if all recipients
                            // were successful.
                            pc.Clear();
                            return output;
                        }
                    }
                    catch (Exception e)
                    {
                        // generic exception handling for connector errors.
                        var output = new
                        {
                            status = false,
                            Message = e.Message
                        };
                        return output;
                    }
                }
            }
            catch (Exception e)
            {
                var output = new
                {
                    status = false,
                    Message = e.Message
                };
                return output;
            }
        }

        #endregion

        #region Secure Messaging
        private DSMessage EncryptMessageAndGenerateHash(DSMessage ds)
        {
            byte[] IV;
            byte[] key;
            byte[] encrypted;
            byte[] messageHash;
            string messageString = ds.Tables[ds.UserMessages.TableName].Rows[0][ds.UserMessages.MessageDetailColumn.ColumnName].ToString();

            using (Aes aesAlg = Aes.Create())
            {
                IV = aesAlg.IV;
                key = aesAlg.Key;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(messageString);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            using (var sha512 = new SHA512Managed())
            {
                byte[] textData = Encoding.UTF8.GetBytes(messageString);
                messageHash = sha512.ComputeHash(textData);
            }

            ds.Tables[ds.UserMessages.TableName].Rows[0][ds.UserMessages.MessageDetailColumn.ColumnName] = BitConverter.ToString(encrypted).Replace("-", String.Empty).ToLower();
            ds.Tables[ds.UserMessages.TableName].Rows[0][ds.UserMessages.MessageHashColumn.ColumnName] = messageHash;
            ds.Tables[ds.UserMessages.TableName].Rows[0][ds.UserMessages.EncryptionIVColumn.ColumnName] = IV;
            ds.Tables[ds.UserMessages.TableName].Rows[0][ds.UserMessages.EncryptionKeyColumn.ColumnName] = key;

            return ds;
        }


        static DSMessage DecryptMessage(DSMessage ds, bool isMessageReply)
        {

            byte[] cipherText, IV, key;

            if (isMessageReply == true)
            {
                cipherText = StringToByteArray(ds.Tables[ds.UserMessageReply.TableName].Rows[0][ds.UserMessageReply.MessageSentColumn.ColumnName].ToString());
                IV = (byte[])ds.Tables[ds.UserMessageReply.TableName].Rows[0][ds.UserMessageReply.EncryptionIVColumn.ColumnName];
                key = (byte[])ds.Tables[ds.UserMessageReply.TableName].Rows[0][ds.UserMessageReply.EncryptionKeyColumn.ColumnName];
            }
            else
            {
                cipherText = StringToByteArray(ds.Tables[ds.UserMessages.TableName].Rows[0][ds.UserMessages.MessageDetailColumn.ColumnName].ToString());
                IV = (byte[])ds.Tables[ds.UserMessages.TableName].Rows[0][ds.UserMessages.EncryptionIVColumn.ColumnName];
                key = (byte[])ds.Tables[ds.UserMessages.TableName].Rows[0][ds.UserMessages.EncryptionKeyColumn.ColumnName];
            }

            //ds.Tables[ds.UserMessages.TableName].Rows[0][ds.UserMessages.EncryptionKeyColumn.ColumnName] = key;
            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.IV = IV;
                aesAlg.Key = key;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            if (isMessageReply == true)
            {
                ds.Tables[ds.UserMessageReply.TableName].Rows[0][ds.UserMessageReply.MessageSentColumn.ColumnName] = plaintext;
            }
            else
            {
                ds.Tables[ds.UserMessages.TableName].Rows[0][ds.UserMessages.MessageDetailColumn.ColumnName] = plaintext;
            }
            return ds;

        }

        static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }


        public string OpenEncryptedMessage(string patientId, string messageId)
        {
            DSMessage dsMsg = new DSMessage();
            BLObject<DSMessage> objMSg = BLLMessageObj.CheckSteSecretKey(messageId, Convert.ToString(MDVSession.Current.AppUserId));
            if (objMSg.Data != null && objMSg.Data.SecretKey.Rows.Count > 0)
            {
                dsMsg = objMSg.Data;

                string savedKey = dsMsg.Tables[dsMsg.SecretKey.TableName].Rows[0][dsMsg.SecretKey.SecretKeyColumn.ColumnName].ToString();

                if (savedKey != null && savedKey != "")
                {
                    DateTime savedTime = (DateTime)dsMsg.Tables[dsMsg.SecretKey.TableName].Rows[0][dsMsg.SecretKey.SecretKeyTimeColumn.ColumnName];
                    DateTime currTime = DateTime.Parse(DateTime.Now.ToString("h:mm:ss"));
                    TimeSpan diff = currTime - savedTime;

                    if (diff.TotalMinutes < 30)
                    {

                        var response = new
                        {
                            status = true,
                            message = "Secret key Exist.",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        string response = GeneratekeyAndSendEmail(patientId, messageId, true);
                        return response;
                    }
                }
                else
                {
                    string response = GeneratekeyAndSendEmail(patientId, messageId, false);
                    return response;
                }
            }
            else
            {
                string response = GeneratekeyAndSendEmail(patientId, messageId, false);
                return response;
            }


        }
        private string GeneratekeyAndSendEmail(string patientId, string messageId, bool isUpdate)
        {
            BLObject<DSUsers> obj;
            string mailAddress = "";

            string key = GetUniqueKey(10);
            obj = BLLAdminSecurityObj.LoadUserEmail(MDVSession.Current.AppUserId);
            DSUsers ds = obj.Data;

            if (ds.UserEmail.Rows.Count > 0)
                mailAddress = ds.UserEmail.Rows[0][ds.UserEmail.EmailAddressColumn].ToString();

            if (mailAddress != "")
            {
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient("smtp.office365.com");

                mail.From = new MailAddress("no_reply@sovms.com");
                mail.To.Add(mailAddress);
                mail.Subject = "Sovereign Health System Patient Portal";

                mail.Body = GetFormattedEmailMessage(key);
                mail.IsBodyHtml = true;

                client.Port = 587;
                client.Credentials = new NetworkCredential("no_reply@sovms.com", "Naka5116");
                client.EnableSsl = true;

                try
                {
                    client.SendMailAsync(mail);
                    if (isUpdate == true)
                    {
                        BLLMessageObj.UpdateSecretKey(messageId, key, DateTime.Now, Convert.ToString(MDVSession.Current.AppUserId));
                    }
                    else
                    {
                        BLLMessageObj.InsertSecretKey(messageId, key, DateTime.Now, Convert.ToString(MDVSession.Current.AppUserId));
                    }

                    var response = new
                    {
                        status = true,
                        message = "Email sent successfully.",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
            else
            {
                var response = new
                {
                    status = false,
                    message = "Please add your email address in order to receive your secret key!",
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        private string GetFormattedEmailMessage(string key)
        {

            StringBuilder str = new StringBuilder();
            const string newLine = "<br/>";

            str.Append(@"<html><body><div style='font-family: Calibri, Helvetica, sans-serif;'>");
            str.Append("Dear User,");
            str.Append(newLine);
            str.Append(newLine).Append("Your encryption key is: ").Append(key);
            str.Append(newLine);
            str.Append(newLine).Append("Please enter this key in the pop up to view/decrypt your message.");
            str.Append(newLine).Append(newLine).Append("If you have not attempted to view the message, please give us a call at 201-479-5433.");
            str.Append(newLine).Append(newLine).Append("Note: This encryption key expires in 30 minutes.");
            str.Append(newLine).Append(newLine).Append("Thank you,");
            str.Append(newLine).Append("Sovereign Medical Group");

            str.Append("</div></body></html>");

            return str.ToString();
        }


        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }


        public string ValidateSecretKey(string messageID, string secretKey)
        {
            try
            {
                BLObject<DSMessage> obj = BLLMessageObj.CheckSteSecretKey(messageID, Convert.ToString(MDVSession.Current.AppUserId));
                DSMessage ds = new DSMessage();
                if (obj.Data != null)
                {
                    ds = obj.Data;
                    string savedKey = ds.Tables[ds.SecretKey.TableName].Rows[0][ds.SecretKey.SecretKeyColumn.ColumnName].ToString();
                    DateTime savedTime = (DateTime)ds.Tables[ds.SecretKey.TableName].Rows[0][ds.SecretKey.SecretKeyTimeColumn.ColumnName];
                    DateTime currTime = DateTime.Now;

                    TimeSpan diff = currTime - savedTime;

                    if (secretKey == savedKey)
                    {
                        if (diff.TotalMinutes < 30)
                        {
                            var response = new
                            {
                                status = true,
                                message = "Secret key matched.",
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                message = "Expired",
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = "Mismatched",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Secret key not found.",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        #endregion

    }
}