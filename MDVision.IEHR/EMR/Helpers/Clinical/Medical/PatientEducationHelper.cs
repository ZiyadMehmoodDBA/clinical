using MDVision.Datasets;
using MDVision.Business.BCommon;
using System;
using System.Collections.Generic;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Model.Clinical.Medical.PatientEducation;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using System.Web.Configuration;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Medical
{
    public class PatientEducationHelper
    {
        private BLLPatientEducation BLLPatientEducationObj = null;
        private BLLPatient BLLPatientObj = null;
        private BLLClinical BLLClinicalObj = null;
        public PatientEducationHelper()
        {
            BLLPatientEducationObj = new BLLPatientEducation();
            BLLClinicalObj = new BLLClinical();
            BLLPatientObj = new BLLPatient();
        }


        private static PatientEducationHelper _instance = null;
        public static PatientEducationHelper Instance()
        {
            if (_instance == null)
                _instance = new PatientEducationHelper();
            return _instance;
        }

        public string InsertClinical_PatientEducation(PatientEducationModel model)
        {
            try
            {
                #region Binding DataSet Information
                //@DocType = 1 (info),  @DocType = 0 (Non Info)
                DSPatientEducation dsPatEdu = new DSPatientEducation();
                DSPatientEducation.PatientEducationRow dr = dsPatEdu.PatientEducation.NewPatientEducationRow();
                dr.PatientID = MDVUtility.ToInt64(model.PatientId);
                dr.DocType = model.DocType;
                dr.PatDocId = MDVUtility.ToInt32(model.DocumentId);
                model.FileType = "application/pdf";

                if (model.FileStream != null && model.FileStream.Length > 0)
                {

                    dr.FileType = model.FileType;

                    dr.FileStream = null;
                    if (model.FileType == "application/pdf")
                        dr.Pages = MDVUtility.getPdfPagesCount(model.FileStream);
                    else
                        dr.Pages = 1;
                }
                else
                    dr.Pages = 0;


                string dt = DateTime.Now.ToShortDateString();
                string tm = DateTime.Now.ToShortTimeString();
                string fName = dt + " " + tm.Replace(" ", "");
                fName = fName.Replace("/", "-").Replace(":", "");

                var mnth = fName.Split('-')[0];
                var day = fName.Split('-')[1];
                var year = fName.Split('-')[2];
                mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                day = day.Length == 1 ? "0" + day : day;
                var mnt = year.Split(' ')[1];

                year = year.Split(' ')[0];
                mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                string fileExt = Path.GetExtension(model.DocumentName);

                if (model.IsNonInfo == "Yes")
                {
                    fName = mnth + "." + day + "." + year + "_" + model.DocumentName;
                }else
                {
                    if (fileExt != ".pdf")
                    {
                        fName = mnth + "." + day + "." + year + "_" + model.DocumentName + ".pdf";
                    }
                    
                }
                               
                dr.DocumentName = fName;
                fileExt = Path.GetExtension(model.DocumentName);
                switch (fileExt)
                {
                    case ".mp3":
                        dr.FileType = "audio/mp3";
                        break;
                    case ".pdf":
                        dr.FileType = "application/pdf";
                        break;
                    case ".jpg":
                        dr.FileType = "image/jpg";
                        break;
                    case ".jpeg":
                        dr.FileType = "image/jpeg";
                        break;
                    case ".png":
                        dr.FileType = "image/png";
                        break;
                    case ".gif":
                        dr.FileType = "image/gif";
                        break;
                    case ".bmp":
                        dr.FileType = "image/bmp";
                        break;
                    case ".xlsx":
                        dr.FileType = ".xlsx";
                        break;
                    case ".docx":
                        dr.FileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".doc":
                        dr.FileType = "application/msword";
                        break;
                    case ".html":
                        dr.FileType = "application/html";
                        break;
                }
               
                if (model.DocType == "1" && model.FileStream != null)
                    dr.URL = CommonFunc.SaveDocumentToFolder(null, "PatientEducation", "Documents", MDVUtility.ToInt64(model.PatientId), fName, model.FileStream);
                else
                {
                    byte[] file = null;
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> objp = null;
                    objp = BLLPatientObj.LoadPatientDocument(model.DocumentId, 0, "", "", "", null, null, null, null, "", 0, 0, "", 0, "", "0");
                    dsPatient = objp.Data;
                    if (objp.Data != null)
                    {
                        if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > 0)
                        {
                            string LoadPrevious = "0";
                            foreach (DataRow dtr in dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows)
                            {
                                byte[] byteArr = dtr["FileStream"] as byte[];
                                string UrlPath = dtr["Url"].ToString();
                                if (!String.IsNullOrEmpty(UrlPath))
                                {

                                    string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                                    UrlPath = FilePath + UrlPath;
                                    using (var stream = new FileStream(UrlPath, FileMode.Open, FileAccess.Read))
                                    {
                                        using (var reader = new BinaryReader(stream))
                                        {
                                            file = reader.ReadBytes((int)stream.Length);

                                        }
                                    }

                                }
                                else if (byteArr != null)
                                {
                                    //string strBase64 = Convert.ToBase64String(byteArr);

                                    //if (!dtr.Table.Columns.Contains("Base64FileStream"))
                                    //{
                                    //    dsPatient.Tables[dsPatient.PatientDocument.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                    //}
                                    //dtr["Base64FileStream"] = strBase64;
                                    //LoadPrevious = "1";
                                }

                            }
                        }
                    }
                    dr.URL = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Pat Education", MDVUtility.ToInt64(model.PatientId), fName, file);
                }
                dr.Comments = model.Comments;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);

                if (!string.IsNullOrEmpty(model.NoteId))
                {
                    dr.NoteId = MDVUtility.ToInt64(model.NoteId);
                }
                else
                    dr[dsPatEdu.PatientEducation.NoteIdColumn] = DBNull.Value;

                #endregion

                #region Database Insertion
                dsPatEdu.PatientEducation.AddPatientEducationRow(dr);
                BLObject<DSPatientEducation> obj = BLLPatientEducationObj.InsertClinical_PatientEducation(dsPatEdu);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        PatEducationId = dsPatEdu.Tables[dsPatEdu.PatientEducation.TableName].Rows[0][dsPatEdu.PatientEducation.PatEducationIdColumn.ColumnName],
                        FileStream = dsPatEdu.Tables[dsPatEdu.PatientEducation.TableName].Rows[0][dsPatEdu.PatientEducation.FileStreamColumn.ColumnName]
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

        public string DeleteClinical_PatientEducation(long PatientEducationId, int PatDocID)
        {
            try
            {
                BLObject<string> obj = BLLPatientEducationObj.DeleteClinical_PatientEducation(PatientEducationId, PatDocID);
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

        public string loadClinical_PatientEducation_Obsolete(long PatientID, int PageNumber, int RowsPerPage, string docType, long NoteId = 0)
        {
            try
            {
                DSPatientEducation dsPatEduInfo = null;
                BLObject<DSPatientEducation> objInfo = null;

                DSPatientEducation dsPatEduNonInfo = null;
                BLObject<DSPatientEducation> objNonInfo = null;
                if (docType == null)
                {
                    objNonInfo = BLLPatientEducationObj.loadClinical_PatientEducation_Obsolete(PatientID,
                        "0", PageNumber, RowsPerPage, NoteId); // For Non Info Based documents
                    objInfo = BLLPatientEducationObj.loadClinical_PatientEducation_Obsolete(PatientID, "1",
                        PageNumber, RowsPerPage, NoteId); // For Info Based documents
                }
                else
                {
                    if (docType == "0")
                    {
                        objNonInfo = BLLPatientEducationObj.loadClinical_PatientEducation_Obsolete(PatientID,
                          "0", PageNumber, RowsPerPage); // For Non Info Based documents
                    }
                    if (docType == "1")
                    {
                        objInfo = BLLPatientEducationObj.loadClinical_PatientEducation_Obsolete(PatientID,
                            "1",
                            PageNumber, RowsPerPage); // For Info Based documents
                    }
                }

                if (objInfo != null)
                {
                    dsPatEduInfo = objInfo.Data;
                }
                if (objNonInfo != null)
                {
                    dsPatEduNonInfo = objNonInfo.Data;
                }

                if (dsPatEduInfo != null && dsPatEduNonInfo != null)
                {
                    if (dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName].Rows.Count > 0 ||
                        dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count > 0)
                    {
                        if (dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName].Rows.Count >
                            dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count)
                        {
                            if (dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count == 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount =
                                        dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName].Rows.Count,
                                    iTotalDisplayRecords =
                                        dsPatEduInfo.PatientEducation.Rows[0][
                                            dsPatEduInfo.PatientEducation.RecordCountColumn.ColumnName],
                                    InfoCount =
                                        dsPatEduInfo.PatientEducation.Rows[0][
                                            dsPatEduInfo.PatientEducation.InfoDocColumn.ColumnName],
                                    NonInfoCount =
                                        dsPatEduInfo.PatientEducation.Rows[0][
                                            dsPatEduInfo.PatientEducation.NonInfoDocColumn.ColumnName],
                                    InfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName]),
                                    NonInfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else if (dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count != 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount =
                                        dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName].Rows.Count,
                                    iTotalDisplayRecords =
                                        dsPatEduInfo.PatientEducation.Rows[0][
                                            dsPatEduInfo.PatientEducation.RecordCountColumn.ColumnName],
                                    InfoCount =
                                        dsPatEduInfo.PatientEducation.Rows[0][
                                            dsPatEduInfo.PatientEducation.InfoDocColumn.ColumnName],
                                    NonInfoCount =
                                        dsPatEduNonInfo.PatientEducation.Rows[0][
                                            dsPatEduNonInfo.PatientEducation.NonInfoDocColumn.ColumnName],
                                    InfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName]),
                                    NonInfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount =
                                        dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName].Rows.Count,
                                    iTotalDisplayRecords =
                                        dsPatEduInfo.PatientEducation.Rows[0][
                                            dsPatEduInfo.PatientEducation.RecordCountColumn.ColumnName],
                                    InfoCount =
                                        dsPatEduInfo.PatientEducation.Rows[0][
                                            dsPatEduInfo.PatientEducation.InfoDocColumn.ColumnName],
                                    NonInfoCount =
                                        dsPatEduNonInfo.PatientEducation.Rows[0][
                                            dsPatEduNonInfo.PatientEducation.NonInfoDocColumn.ColumnName],
                                    InfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName]),
                                    NonInfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }

                        }
                        else if (dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName].Rows.Count <
                                 dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count)
                        {
                            if (dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName].Rows.Count == 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount =
                                        dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count,
                                    iTotalDisplayRecords =
                                        dsPatEduNonInfo.PatientEducation.Rows[0][
                                            dsPatEduNonInfo.PatientEducation.RecordCountColumn.ColumnName],
                                    InfoCount =
                                        dsPatEduNonInfo.PatientEducation.Rows[0][
                                            dsPatEduNonInfo.PatientEducation.InfoDocColumn.ColumnName],
                                    NonInfoCount =
                                        dsPatEduNonInfo.PatientEducation.Rows[0][
                                            dsPatEduNonInfo.PatientEducation.NonInfoDocColumn.ColumnName],
                                    InfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName]),
                                    NonInfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else if (dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName].Rows.Count != 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount =
                                        dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count,
                                    iTotalDisplayRecords =
                                        dsPatEduNonInfo.PatientEducation.Rows[0][
                                            dsPatEduNonInfo.PatientEducation.RecordCountColumn.ColumnName],
                                    InfoCount =
                                        dsPatEduInfo.PatientEducation.Rows[0][
                                            dsPatEduInfo.PatientEducation.InfoDocColumn.ColumnName],
                                    NonInfoCount =
                                        dsPatEduNonInfo.PatientEducation.Rows[0][
                                            dsPatEduNonInfo.PatientEducation.NonInfoDocColumn.ColumnName],
                                    InfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName]),
                                    NonInfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount =
                                        dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count,
                                    iTotalDisplayRecords =
                                        dsPatEduNonInfo.PatientEducation.Rows[0][
                                            dsPatEduNonInfo.PatientEducation.RecordCountColumn.ColumnName],
                                    InfoCount =
                                        dsPatEduInfo.PatientEducation.Rows[0][
                                            dsPatEduInfo.PatientEducation.InfoDocColumn.ColumnName],
                                    NonInfoCount =
                                        dsPatEduNonInfo.PatientEducation.Rows[0][
                                            dsPatEduNonInfo.PatientEducation.NonInfoDocColumn.ColumnName],
                                    InfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName]),
                                    NonInfoDocumentLoad_JSON =
                                        MDVUtility.JSON_DataTable(
                                            dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName]),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                DocumentCount = dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName].Rows.Count,
                                iTotalDisplayRecords =
                                    dsPatEduInfo.PatientEducation.Rows[0][
                                        dsPatEduInfo.PatientEducation.RecordCountColumn.ColumnName],
                                InfoCount =
                                    dsPatEduInfo.PatientEducation.Rows[0][
                                        dsPatEduInfo.PatientEducation.InfoDocColumn.ColumnName],
                                NonInfoCount =
                                    dsPatEduNonInfo.PatientEducation.Rows[0][
                                        dsPatEduNonInfo.PatientEducation.NonInfoDocColumn.ColumnName],
                                InfoDocumentLoad_JSON =
                                    MDVUtility.JSON_DataTable(dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName]),
                                NonInfoDocumentLoad_JSON =
                                    MDVUtility.JSON_DataTable(
                                        dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName]),
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else if (dsPatEduInfo == null)
                {
                    if (dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount =
                                dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count,
                            iTotalDisplayRecords =
                                dsPatEduNonInfo.PatientEducation.Rows[0][
                                    dsPatEduNonInfo.PatientEducation.RecordCountColumn.ColumnName],
                            InfoCount = 0,
                            NonInfoCount =
                                dsPatEduNonInfo.PatientEducation.Rows[0][
                                    dsPatEduNonInfo.PatientEducation.NonInfoDocColumn.ColumnName],
                            InfoDocumentLoad_JSON = "",
                            NonInfoDocumentLoad_JSON =
                                MDVUtility.JSON_DataTable(
                                    dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else if (dsPatEduNonInfo == null)
                {
                    if (dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName].Rows.Count,
                            iTotalDisplayRecords =
                                dsPatEduInfo.PatientEducation.Rows[0][
                                    dsPatEduInfo.PatientEducation.RecordCountColumn.ColumnName],
                            InfoCount =
                                dsPatEduInfo.PatientEducation.Rows[0][
                                    dsPatEduInfo.PatientEducation.InfoDocColumn.ColumnName],
                            NonInfoCount = 0,
                            InfoDocumentLoad_JSON =
                                MDVUtility.JSON_DataTable(dsPatEduInfo.Tables[dsPatEduInfo.PatientEducation.TableName]),
                            NonInfoDocumentLoad_JSON = "",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        DocumentCount = 0,
                        Message = "Record not found."
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

        public string loadClinical_PatientEducation(long PatientID, int PageNumber, int RowsPerPage, string docType, long NoteId = 0)
        {
            try
            {
                List<PatientEducation> listPatEduInfo = null;

                List<PatientEducation> listPatEduNonInfo = null;

                if (docType == null)
                {
                    listPatEduNonInfo = BLLPatientEducationObj.loadClinical_PatientEducation(PatientID,
                        "0", PageNumber, RowsPerPage, NoteId); // For Non Info Based documents
                    listPatEduInfo = BLLPatientEducationObj.loadClinical_PatientEducation(PatientID, "1",
                        PageNumber, RowsPerPage, NoteId); // For Info Based documents
                }
                else
                {
                    if (docType == "0")
                    {
                        listPatEduNonInfo = BLLPatientEducationObj.loadClinical_PatientEducation(PatientID,
                          "0", PageNumber, RowsPerPage); // For Non Info Based documents
                    }
                    if (docType == "1")
                    {
                        listPatEduInfo = BLLPatientEducationObj.loadClinical_PatientEducation(PatientID,
                            "1",
                            PageNumber, RowsPerPage); // For Info Based documents
                    }
                }

                if (listPatEduInfo != null && listPatEduNonInfo != null)
                {
                    if (listPatEduInfo.Count > 0 ||
                        listPatEduNonInfo.Count > 0)
                    {
                        if (listPatEduInfo.Count >
                            listPatEduNonInfo.Count)
                        {
                            if (listPatEduNonInfo.Count == 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = listPatEduInfo.Count,
                                    iTotalDisplayRecords = listPatEduInfo[0].RecordCount,
                                    InfoCount = listPatEduInfo.Count > 0 ? listPatEduInfo[0].InfoDoc : "0",
                                    NonInfoCount = 0,
                                    InfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduInfo),
                                    NonInfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduNonInfo),
                                };
                                return (JsonConvert.SerializeObject(response));
                            }
                            else if (listPatEduNonInfo.Count != 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = listPatEduInfo.Count,
                                    iTotalDisplayRecords = listPatEduInfo.Count > 0 ? listPatEduInfo[0].RecordCount : "0",
                                    InfoCount = listPatEduInfo.Count > 0 ? listPatEduInfo[0].InfoDoc : "0",
                                    NonInfoCount = listPatEduNonInfo.Count > 0 ? listPatEduNonInfo[0].NonInfoDoc : "0",
                                    InfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduInfo),
                                    NonInfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduNonInfo),
                                };
                                return (JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = listPatEduInfo.Count,
                                    iTotalDisplayRecords = listPatEduInfo.Count > 0 ? listPatEduInfo[0].RecordCount : "0",
                                    InfoCount = listPatEduInfo.Count > 0 ? listPatEduInfo[0].InfoDoc : "0",
                                    NonInfoCount = listPatEduNonInfo.Count > 0 ? listPatEduNonInfo[0].NonInfoDoc : "0",
                                    InfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduInfo),
                                    NonInfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduNonInfo),
                                };
                                return (JsonConvert.SerializeObject(response));
                            }

                        }
                        else if (listPatEduInfo.Count <
                                 listPatEduNonInfo.Count)
                        {
                            if (listPatEduInfo.Count == 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = listPatEduInfo.Count,
                                    iTotalDisplayRecords = 0,
                                    InfoCount = "0",
                                    NonInfoCount = listPatEduNonInfo.Count > 0 ? listPatEduNonInfo[0].NonInfoDoc : "0",
                                    InfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduInfo),
                                    NonInfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduNonInfo),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else if (listPatEduInfo.Count != 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = listPatEduInfo.Count,
                                    iTotalDisplayRecords = listPatEduInfo.Count > 0 ? listPatEduInfo[0].RecordCount : "0",
                                    InfoCount = listPatEduInfo.Count > 0 ? listPatEduInfo[0].InfoDoc : "0",
                                    NonInfoCount = listPatEduNonInfo.Count > 0 ? listPatEduNonInfo[0].NonInfoDoc : "0",
                                    InfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduInfo),
                                    NonInfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduNonInfo),
                                };
                                return (JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = listPatEduInfo.Count,
                                    iTotalDisplayRecords = listPatEduInfo.Count > 0 ? listPatEduInfo[0].RecordCount : "0",
                                    InfoCount = listPatEduInfo.Count > 0 ? listPatEduInfo[0].InfoDoc : "0",
                                    NonInfoCount = listPatEduNonInfo.Count > 0 ? listPatEduNonInfo[0].NonInfoDoc : "0",
                                    InfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduInfo),
                                    NonInfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduNonInfo),
                                };
                                return (JsonConvert.SerializeObject(response));
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                DocumentCount = listPatEduInfo.Count,
                                iTotalDisplayRecords = listPatEduInfo.Count > 0 ? listPatEduInfo[0].RecordCount : "0",
                                InfoCount = listPatEduInfo.Count > 0 ? listPatEduInfo[0].InfoDoc : "0",
                                NonInfoCount = listPatEduNonInfo.Count > 0 ? listPatEduNonInfo[0].NonInfoDoc : "0",
                                InfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduInfo),
                                NonInfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduNonInfo),
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else if (listPatEduInfo == null)
                {
                    if (listPatEduNonInfo.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = listPatEduInfo.Count,
                            iTotalDisplayRecords = listPatEduInfo.Count > 0 ? listPatEduInfo[0].RecordCount : "0",
                            InfoCount = "0",
                            NonInfoCount = listPatEduNonInfo[0].NonInfoDoc,
                            InfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduInfo),
                            NonInfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduNonInfo),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else if (listPatEduNonInfo == null)
                {
                    if (listPatEduInfo.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = listPatEduInfo.Count,
                            iTotalDisplayRecords = listPatEduInfo[0].RecordCount,
                            InfoCount = listPatEduInfo[0].InfoDoc,
                            NonInfoCount = "0",
                            InfoDocumentLoad_JSON = JsonConvert.SerializeObject(listPatEduInfo),
                            NonInfoDocumentLoad_JSON = "",
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        DocumentCount = 0,
                        Message = "Record not found."
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
        public string LookupClinical_PatientEducation(string DocumentName)
        {
            try
            {
                DSPatientEducation dsPatEduNonInfo = null;
                BLObject<DSPatientEducation> objNonInfo;
                objNonInfo = BLLPatientEducationObj.LookupClinical_PatientEducation(DocumentName); // For Non Info Based documents

                if (objNonInfo != null)
                {
                    dsPatEduNonInfo = objNonInfo.Data;
                }

                if (dsPatEduNonInfo != null)
                {
                    if (dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count > 0)
                    {
                        List<Tuple<string, string>> documents = new List<Tuple<string, string>>();

                        foreach (DSPatientEducation.PatientEducationRow dr in dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows)
                        {
                            documents.Add(new Tuple<string, string>(Convert.ToString(dr["PatDocId"]), Convert.ToString(dr["FilePath"])));
                        }

                        var response = new
                        {
                            status = true,
                            DocumentCount = dsPatEduNonInfo.Tables[dsPatEduNonInfo.PatientEducation.TableName].Rows.Count,
                            NonInfoDocumentLoad_JSON = Newtonsoft.Json.JsonConvert.SerializeObject(documents),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        DocumentCount = 0,
                        Message = "Record not found."
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
        public string previewInfoPdf(byte[] contentInfo, string patientId)
        {
            try
            {
                BLObject<byte[]> obj = BLLPatientEducationObj.previewInfoPdf(contentInfo, MDVUtility.ToInt64(patientId));

                if (obj.Data != null)
                {
                    return Convert.ToBase64String(obj.Data);
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public DSPatient loadDataforPdf(string patientId)
        {
            DSPatient dsPatient = null;

            try
            {
                BLObject<DSPatient> obj;

                obj = BLLPatientEducationObj.loadDataForPdf(MDVUtility.ToInt64(patientId));
                dsPatient = obj.Data;

            }
            catch (Exception ex)
            {
            }

            return dsPatient;
        }
        /// <summary>
        /// Module Name: getlatestPatientEducationByPatientId
        /// Author: Humaira Yousaf
        /// Created Date: 28-07-2016
        /// Description: Gets patient education
        /// </summary>
        /// <param name="PatientId" type="Int64">PatientId</param>
        public string getlatestPatientEducationByPatientId(Int64 PatientId)
        {
            try
            {

                DSPatientEducation dsPatientEducationSoap = null;
                BLObject<DSPatientEducation> obj;

                obj = BLLPatientEducationObj.getLatestPatientEducationByPatientId(PatientId);

                dsPatientEducationSoap = obj.Data;
                var response = new
                {
                    status = true,
                    PatientEducationSoapCount = dsPatientEducationSoap.Tables[dsPatientEducationSoap.PatientEducation.TableName].Rows.Count,
                    PatientEducationSoap_JSON = MDVUtility.JSON_DataTable(dsPatientEducationSoap.Tables[dsPatientEducationSoap.PatientEducation.TableName]),
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string getPatientEducationSOAP(Int64 PatientId, string PatEducationId, Int64 NoteId)
        {
            try
            {

                DSPatientEducation dsPatientEducationSoap = null;
                BLObject<DSPatientEducation> obj;

                obj = BLLPatientEducationObj.getPatientEducationSOAP(PatientId, PatEducationId, NoteId);

                dsPatientEducationSoap = obj.Data;
                var response = new
                {
                    status = true,
                    PatientEducationSoapCount = dsPatientEducationSoap.Tables[dsPatientEducationSoap.PatientEducation.TableName].Rows.Count,
                    PatientEducationSoap_JSON = MDVUtility.JSON_DataTable(dsPatientEducationSoap.Tables[dsPatientEducationSoap.PatientEducation.TableName]),
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
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        /// <summary>
        /// Module Name: detachPatientEducationFromNotes
        /// Author: Humaira Yousaf
        /// Created Date: 29-07-2016
        /// Description: Detach Patient Education From Notes
        /// </summary>
        /// <param name="PatientEducationId" type="string">PatientEducationId</param>
        /// <param name="NotesId" type="long">NotesId</param>
        internal string detachPatientEducationFromNotes(string PatientEducationId, long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientEducationId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLPatientEducationObj.detachPatientEducationFromNotes(PatientEducationId, NotesId);
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

        /// <summary>
        /// Module Name: attachPatientEducationWithNotes
        /// Author: Humaira Yousaf
        /// Created Date: 29-07-2016
        /// Description: Attach Patient Education From Notes
        /// </summary>
        /// <param name="PatientEducationId" type="string">PatientEducationId</param>
        /// <param name="NotesId" type="long">NotesId</param>
        internal string attachPatientEducationWithNotes(string PatientEducationId, long NotesId)
        {
            try
            {
                DSPatientEducation dsPatientEducation = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientEducationId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<DSPatientEducation> obj = BLLPatientEducationObj.attachPatientEducationWithNotes(PatientEducationId, NotesId);
                    if (obj.Data != null)
                    {
                        dsPatientEducation = obj.Data;
                        var response = new
                        {
                            status = true,
                            PatientEducationCount = dsPatientEducation.Tables[dsPatientEducation.PatientEducation.TableName].Rows.Count,
                            PatientEducationLoad_JSON = MDVUtility.JSON_DataTable(dsPatientEducation.Tables[dsPatientEducation.PatientEducation.TableName]),
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

    }
}