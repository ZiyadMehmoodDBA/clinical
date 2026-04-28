using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.Model.Clinical.Notes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Controls.Patient.Document
{
    public class Patient_Document_Image_Annotation
    {
        private BLLPatient BLLPatientObj = null;
        private BLLClinical BLLClinicalObj = null;
        public Patient_Document_Image_Annotation()
        {
            BLLPatientObj = new BLLPatient();
            BLLClinicalObj = new BLLClinical();
        }

        #region Singleton
        private static Patient_Document_Image_Annotation _obj = null;
        public static Patient_Document_Image_Annotation Instance()
        {
            if (_obj == null)
                _obj = new Patient_Document_Image_Annotation();
            return _obj;
        }


        #endregion


        private BLObject<NoteDocumentModel> InsertNoteDocument(string documentIDs, long notesId)
        {
            NoteDocumentModel model = new NoteDocumentModel();
            model.NoteId = notesId.ToString();
            model.PatDocId = documentIDs;
            model.IsActive = "1";
            model.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            model.CreatedOn = DateTime.Now.ToString();
            model.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            model.ModifiedOn = DateTime.Now.ToString();

            return BLLPatientObj.InsertNoteDocument(model);
        }

        public string AttachPatientDocumentToNote(string documentIDs, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(documentIDs) || notesId <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.No_Record_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                   
                    BLObject<NoteDocumentModel> obj = InsertNoteDocument(documentIDs, notesId);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                            NoteDocumentId = obj.Data.NoteDocumentId
                    };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {

                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
                            NoteDocumentId = ""
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

        private string DetachPatientDocumentFromNote(long PatDocId, long notesId)
        {
            try
            {
                if (PatDocId <= 0 && notesId <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.No_Record_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {

                    BLObject<string> obj = BLLPatientObj.DeleteNoteDocument(0, notesId, PatDocId);
                    if (string.IsNullOrEmpty(obj.Data))
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {

                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
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

        private string UpdatePatientDocumentWithNote(string fieldsJSON, Int64 PatientId, string PatientDocId, long NoteId, bool IsToAddNote, string FileBase64)
        {
            try
            {
                if (PatientDocId != "")
                {
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    string FileName = string.Empty;
                    string FileNameExt = string.Empty;
                    string FolderName = string.Empty;
                    string NoteDocumentId = string.Empty;

                    if (SearchedfieldsJSON.ContainsKey("FileName") && !string.IsNullOrEmpty(SearchedfieldsJSON["FileName"]))
                        FileName = MDVUtility.ToStr(SearchedfieldsJSON["FileName"]);

                    if (SearchedfieldsJSON.ContainsKey("FileNameExt") && !string.IsNullOrEmpty(SearchedfieldsJSON["FileNameExt"]))
                        FileNameExt = MDVUtility.ToStr(SearchedfieldsJSON["FileNameExt"]);

                    if (SearchedfieldsJSON.ContainsKey("Folder_text") && !string.IsNullOrEmpty(SearchedfieldsJSON["Folder_text"]))
                        FolderName = MDVUtility.ToStr(SearchedfieldsJSON["Folder_text"]);

                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = null;

                    obj = BLLPatientObj.LoadPatientDocument(PatientDocId, PatientId, "", "", "", null, null, null, null, "", 0, 0, "", 0, "");
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;
                        if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows)
                            {
                                if (SearchedfieldsJSON.ContainsKey("Folder") && !string.IsNullOrEmpty(SearchedfieldsJSON["Folder"]))
                                    dr[dsPatient.PatientDocument.DocumentidColumn] = MDVUtility.ToInt32(SearchedfieldsJSON["Folder"]);

                                if (SearchedfieldsJSON.ContainsKey("Comments") && !string.IsNullOrEmpty(SearchedfieldsJSON["Comments"]))
                                {
                                    dr[dsPatient.PatientDocument.CommentsColumn] = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["Comments"]);
                                }
                                else if (SearchedfieldsJSON.ContainsKey("Comments") && string.IsNullOrEmpty(SearchedfieldsJSON["Comments"]))
                                {
                                    dr[dsPatient.PatientDocument.CommentsColumn] = string.Empty;
                                }
                                

                                //STEP3 UPDATE IMAGE on Destination Location
                                FileBase64 = FileBase64.Replace(" ", "+");

                                int mod4 = FileBase64.Length % 4;
                                if (mod4 > 0)
                                {
                                    FileBase64 += new string('=', 4 - mod4);
                                }
                                byte[] currentFileStream = null;
                                currentFileStream = Convert.FromBase64String(FileBase64);


                                string FilePath = CommonFunc.SaveDocumentToFolder(null, "Patient Document", FolderName, PatientId, FileName + FileNameExt, currentFileStream,true);
                                dr[dsPatient.PatientDocument.UrlColumn] = FilePath;
                                dr[dsPatient.PatientDocument.BUpdateStreamColumn] = 1;
                                
                            }

                            // STEP 1 Save Note Document Link
                            if (IsToAddNote)
                            {
                                BLObject<NoteDocumentModel> obj_ = InsertNoteDocument(PatientDocId, NoteId);
                                if (obj_.Data != null)
                                {
                                    // Saved success.
                                    NoteDocumentId = obj_.Data.NoteDocumentId;
                                }
                                else
                                {
                                    throw new Exception(obj_.Message);
                                }
                            }

                            //STEP 2 UPDATE Document Information
                            BLObject<DSPatient> objPatientDocument = null;
                            objPatientDocument = BLLPatientObj.UpdatePatientDocument(dsPatient);

                            if (objPatientDocument.Data != null)
                            {

                                var response = new
                                {
                                    status = true,
                                    NoteDocumentId= NoteDocumentId,
                                    Message = Common.AppPrivileges.Update_Message
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = objPatientDocument.Message
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }

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
                            Message = "Patient Document not found."
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient Document not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        public string Detach_DocumentsFromNote(string patientDocumentIds, long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(patientDocumentIds)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLClinicalObj.DetachDocumentsFromNote(patientDocumentIds, NotesId);
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
        #region Service Command Handler
        /// <summary>
        /// Handle the Basic Free Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            string privilegesMessage = string.Empty;
            switch (cammandAction)
            {
                case "ATTACH_PATIENT_DOCUMENT_TO_NOTE":
                    {
                        string strJSONData = "";
                        string DocumentIDs = MDVUtility.ToStr(context.Request["DocumentIDs"]);
                        long NotesId = MDVUtility.ToLong(context.Request["NotesId"]);
                        strJSONData = AttachPatientDocumentToNote(DocumentIDs, NotesId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DETACH_PATIENT_DOCUMENT_TO_NOTE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "DELETE")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            long DocumentID = MDVUtility.ToLong(context.Request["DocumentID"]);
                            long NotesId = MDVUtility.ToLong(context.Request["NotesId"]);

                            strJSONData = DetachPatientDocumentFromNote(DocumentID, NotesId);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_DOCUMENT_AND_ATTACH_NOTE":
                    {
                        string strJSONData = "";
                        string fieldsJSON = context.Request["PatientDocumentData"];
                        string ObjectData = context.Request["ObjectData"];

                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        ser.MaxJsonLength = Int32.MaxValue;
                        var SearchedfieldsJSON = ser.Deserialize<dynamic>(ObjectData);

                        string DocumentID = string.Empty;
                        if (!string.IsNullOrEmpty(Convert.ToString(SearchedfieldsJSON["DocumentID"])))
                             DocumentID = MDVUtility.ToStr(SearchedfieldsJSON["DocumentID"]);

                        long NotesId = 0;
                        if (!string.IsNullOrEmpty(Convert.ToString(SearchedfieldsJSON["NotesId"])))
                            NotesId = MDVUtility.ToLong(SearchedfieldsJSON["NotesId"]);

                        long PatientID = 0;
                        if (!string.IsNullOrEmpty(Convert.ToString(SearchedfieldsJSON["PatientID"])))
                            PatientID = MDVUtility.ToLong(SearchedfieldsJSON["PatientID"]);

                        bool IsToAddNote = false;
                        if (!string.IsNullOrEmpty(Convert.ToString(SearchedfieldsJSON["IsToAddNote"])))
                            IsToAddNote = MDVUtility.ToBool(SearchedfieldsJSON["IsToAddNote"]);

                        string FileBase64 = string.Empty;
                        if (!string.IsNullOrEmpty(Convert.ToString(SearchedfieldsJSON["FileBase64"])))
                            FileBase64 = MDVUtility.ToStr(SearchedfieldsJSON["FileBase64"]);

                        strJSONData = UpdatePatientDocumentWithNote(fieldsJSON, PatientID, DocumentID, NotesId, IsToAddNote, FileBase64);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DETACH_PATIENT_DOCUMENTS_FROM_NOTE":
                    {
                        string strJSONData = "";
                        string DocumentIds = MDVUtility.ToStr(context.Request["PatientDocumentIds"]);
                        long NotesId = MDVUtility.ToLong(context.Request["NotesId"]);

                        strJSONData = Detach_DocumentsFromNote(DocumentIds, NotesId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

            }

        }

        #endregion
    }

}