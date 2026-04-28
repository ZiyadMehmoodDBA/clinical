/* Author:  Khaleel Ur Rehman
 * Created Date: 02/03/2016
 * OverView: Created to handel Letter Template
 */

using MDVision.Datasets;
using MDVision.Business.BCommon;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using MDVision.IEHR.EMR.Model.Templates;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using iTextSharp.text;
using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf;
using System.IO;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.IEHR.EMR.Services;
using HtmlAgilityPack;
using iTextSharp.text.html.simpleparser;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates
{
    public class PatientTemplateHelper
    {

        BLLPatient BLLPatientObj = null;
        public PatientTemplateHelper()
        {

            BLLPatientObj = new BLLPatient();


        }


        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : To load Patient Letter.
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="isActive"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="categoryId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>
        public string loadPatientTemplateLetter(Int64 Patient_LetterId, Int64 patientId, bool isActive, string name, string description, int categoryId, Int32 pageNumber, Int32 rowsPerPage, string visitDateFrom, string visitDateTo, long NotesId)
        {
            try
            {
                DSLetter dsPatientTemplateLetter = null;
                BLObject<DSLetter> obj;
                obj = BLLPatientObj.loadPatientTemplateLetter(Patient_LetterId, patientId, isActive, name, description, categoryId, pageNumber, rowsPerPage, visitDateFrom, visitDateTo, NotesId);
                dsPatientTemplateLetter = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatientTemplateLetter.Tables[dsPatientTemplateLetter.PatientTemplateLetter.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientTemplateLetterCount = dsPatientTemplateLetter.Tables[dsPatientTemplateLetter.PatientTemplateLetter.TableName].Rows.Count,
                            iTotalDisplayRecords = dsPatientTemplateLetter.PatientTemplateLetter.Rows[0][dsPatientTemplateLetter.PatientTemplateLetter.RecordCountColumn.ColumnName],
                            PatientTemplateLetterLoad_JSON = MDVUtility.JSON_DataTable(dsPatientTemplateLetter.Tables[dsPatientTemplateLetter.PatientTemplateLetter.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientTemplateLetterCount = 0,
                            Message = Common.AppPrivileges.No_Record_Message
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

        public string MergeTwoBase64Content(string LabLetterBase64, string LabResultBase64)
        {
            try
            {
                if (!string.IsNullOrEmpty(LabLetterBase64) || !string.IsNullOrEmpty(LabResultBase64))
                {
                    List<byte[]> docsToPrintArr = new List<byte[]>();

                    if (!string.IsNullOrEmpty(LabLetterBase64))
                    {
                        LabLetterBase64 = LabLetterBase64.Replace(" ", "+");
                        byte[] attachedLetterByteArr = Convert.FromBase64String(LabLetterBase64);
                        docsToPrintArr.Add(attachedLetterByteArr);
                    }
                    if (!string.IsNullOrEmpty(LabResultBase64))
                    {
                        LabResultBase64 = LabResultBase64.Replace(" ", "+");
                        byte[] attachedResultByteArr = Convert.FromBase64String(LabResultBase64);
                        docsToPrintArr.Add(attachedResultByteArr);
                    }

                    byte[] MergedByteArr = MDVUtility.CombineMultipleByteArrays(docsToPrintArr);

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    js.MaxJsonLength = Int32.MaxValue;
                    var response = new
                    {
                        status = true,
                        MergedContent = Convert.ToBase64String(MergedByteArr.ToArray())
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Record not found."
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
        public string SignPatLetter(long NotesId, string signText, long PatientId)
        {
            dynamic response = null;
            try
            {
                DSLetter dsLetter = null;
                BLObject<DSLetter> obj = null;
                obj = BLLPatientObj.loadPatientTemplateLetter(0, PatientId, true, "", "", 0, 1, 2000, null, null, NotesId);
                dsLetter = obj.Data;
                if (obj.Data != null)
                {
                    List<DataRow> rowsToDelete = new List<DataRow>();
                    string htmlContent = "";
                    int counter = 0;
                    int total = dsLetter.Tables[dsLetter.PatientTemplateLetter.TableName].Rows.Count;
                    foreach (DataRow item in dsLetter.Tables[dsLetter.PatientTemplateLetter.TableName].Rows)
                    {
                        try
                        {
                            DataRow dr = item;
                            if (dr != null && NotesId == MDVUtility.ToInt64(dr[dsLetter.PatientTemplateLetter.NotesIdColumn.ColumnName]) && MDVUtility.ToStr(dr[dsLetter.PatientTemplateLetter.StatusColumn.ColumnName]).ToLower() == "draft")
                            {
                                dr[dsLetter.PatientTemplateLetter.StatusColumn.ColumnName] = "Signed";
                                dr[dsLetter.PatientTemplateLetter.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                dr[dsLetter.PatientTemplateLetter.ModifiedOnColumn.ColumnName] = DateTime.Now;
                                htmlContent = MDVUtility.ToStr(dr[dsLetter.PatientTemplateLetter.PatientLetterContentColumn.ColumnName]);
                                HtmlDocument document = new HtmlDocument();
                                document.LoadHtml(htmlContent);
                                if (document != null)
                                {
                                    HtmlNode innerBodyContent = document.DocumentNode.SelectSingleNode("html/body");
                                    if (innerBodyContent != null)
                                    {
                                        htmlContent = htmlContent.Replace("</body>", signText + "<br /></body>").Replace("<br>", "<br />"); ;
                                        byte[] array = ConvertHtmlToPdf(htmlContent, "");
                                        string Base64String = Convert.ToBase64String(array);
                                        if (!string.IsNullOrEmpty(Base64String))
                                        {
                                            Base64String = Base64String.Replace(" ", "+");
                                            int mod4 = Base64String.Length % 4;
                                            if (mod4 > 0)
                                                Base64String += new string('=', 4 - mod4);
                                            byte[] bytes = Convert.FromBase64String(Base64String);
                                            string dt = DateTime.Now.ToShortDateString();
                                            dt.Replace("/", "-");
                                            string tm = DateTime.Now.ToShortTimeString();
                                            string letterName = dt + " " + tm.Replace(" ", "");
                                            letterName = letterName.Replace("/", "-").Replace(":", "");
                                            var mnth = letterName.Split('-')[0];
                                            var day = letterName.Split('-')[1];
                                            var year = letterName.Split('-')[2];
                                            mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                                            day = day.Length == 1 ? "0" + day : day;
                                            var mnt = year.Split(' ')[1];
                                            year = year.Split(' ')[0];
                                            mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                                            letterName = mnth + "." + day + "." + year + "_" + MDVUtility.ToStr(dr[dsLetter.PatientTemplateLetter.NameColumn.ColumnName]) + ".pdf";
                                            string FilePath = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Patient Letters", PatientId, letterName, bytes);
                                            dr[dsLetter.PatientTemplateLetter.UrlColumn.ColumnName] = FilePath;
                                            dr[dsLetter.PatientTemplateLetter.FilePathColumn.ColumnName] = Path.GetFileName(FilePath);
                                        }
                                        dr[dsLetter.PatientTemplateLetter.PatientLetterContentColumn.ColumnName] = htmlContent;
                                    }
                                    else
                                        rowsToDelete.Add(item);
                                }
                                else
                                    rowsToDelete.Add(item);
                            }
                            else
                                rowsToDelete.Add(item);
                            counter++;
                        }
                        catch (Exception ex)
                        {
                            rowsToDelete.Add(item);
                            continue;
                        }
                    }
                    foreach (DataRow item in rowsToDelete)
                        dsLetter.PatientTemplateLetter.Rows.Remove(item);

                    obj = BLLPatientObj.UpdatePatientLetter(dsLetter);
                    if (obj.Data != null)
                    {
                        if (total == counter)
                        {
                            response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Save_Message
                            };
                        }
                        else
                        {
                            response = new
                            {
                                status = true,
                                Message = counter + " out of " + total + " Letter(s) Signed Successfully!"
                            };
                        }
                    }
                    else
                    {
                        response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                    }
                }
                else
                {
                    response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                }
            }
            catch (Exception ex)
            {
                response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
            }
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }
        public string GetPatientLetterContent(Int64 patientId, Int64 TemplateLetterId, string Mode, Int64 PatientLetterId, long ProviderId, long LabOrderResultId = 0, string LOINC = null)
        {

            try
            {
                DSLetter dsLetter = null;
                BLObject<DSLetter> obj;
                obj = BLLPatientObj.GetPatientLetterContent(patientId, TemplateLetterId, Mode, PatientLetterId, ProviderId, LabOrderResultId, LOINC);
                dsLetter = obj.Data;
                if (obj.Data != null)
                {
                    if (dsLetter.Tables[dsLetter.PatientTemplateLetter.TableName].Rows.Count > 0)
                    {

                        DataRow dr = dsLetter.Tables[dsLetter.PatientTemplateLetter.TableName].Rows[0];
                        String HtmlDocument = MDVUtility.ToStr(dr[dsLetter.PatientTemplateLetter.PatientLetterContentColumn.ColumnName]);
                        byte[] byteArr = dr[dsLetter.PatientTemplateLetter.eSignatureColumn.ColumnName] as byte[];


                        string strSignatureBase64 = byteArr != null ? Convert.ToBase64String(byteArr) : string.Empty;

                        byte[] byteArreSignatureFax = dr[dsLetter.PatientTemplateLetter.eSignatureFaxProviderColumn.ColumnName] as byte[];
                        string strSignatureFaxBase64 = byteArreSignatureFax != null ? Convert.ToBase64String(byteArreSignatureFax) : string.Empty;



                        // strSignatureBase64 = MDVision.IEHR.Common._Utility.AddWaterMarkOnImage(strSignatureBase64, "©Sovereign Health System");

                        //HtmlDocument = HtmlDocument.Replace("<input id=\"184\" class=\"TagInserted\" style=\"min-width: 10px; border: none; width: 240px;\" readonly=\"readonly\" type=\"text\" value=\"{{ Primary Care Provider eSignature }}\" />", "<img id=\"imgeSignature\" src=\"data:System.Byte[];base64," + strSignatureBase64 + "\" />");



                        string[] ImageSources = HtmlDocument.Split(new string[] { "src=" }, StringSplitOptions.None);
                        for (int i = 0; i < ImageSources.Length; i++)
                        {
                            if (ImageSources[i].Contains("data:image"))
                            {
                                string ImageString = ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[0];
                                ImageSources[i] = "src=" + ImageString.Replace(" ", "+") + " " + ImageSources[i].Split(new string[] { " alt=" }, StringSplitOptions.None)[1];
                            }
                        }

                        HtmlDocument = String.Join("", ImageSources);

                        insertInsuranceCardImagesInHTML(ref HtmlDocument, dsLetter, dr);

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            PatientLetterContentCount = dsLetter.Tables[dsLetter.PatientTemplateLetter.TableName].Rows.Count,
                            PatientLetterContent = HtmlDocument,
                            eSignatureBase64 = strSignatureBase64,
                            eSignatureFaxBase64 = strSignatureFaxBase64
                            //PrimaryCardFrontBase64 = PrimaryCardFrontBase64,
                            //PrimaryCardBackBase64 = PrimaryCardBackBase64,
                            //SecondaryCardFrontBase64 = SecondaryCardFrontBase64,
                            //SecondaryCardBackBase64 = SecondaryCardBackBase64,
                            //TertiarCardFrontBase64 = TertiarCardFrontBase64,
                            //TertiaryCardBackBase64 = TertiaryCardBackBase64
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientLetterContentCount = 0,
                            Message = Common.AppPrivileges.No_Record_Message
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
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : To delete Patient Letter.
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="patientletterId"></param>
        /// <returns></returns>
        public string deletePatientLetter(long patientletterId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(patientletterId)))
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
                    BLObject<string> obj = BLLPatientObj.deletePatientLetter(MDVUtility.ToStr(patientletterId));
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
        /// Author : Khaleel Ur Rehman
        /// Purpose : To update Patient Letter.
        /// Dat : 09-March-2016
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ActiveInactivePatientLetter(LetterTemplateModel model)
        {
            try
            {
                DSLetter dsLetter = null;
                BLObject<DSLetter> obj = null;
                obj = BLLPatientObj.loadPatientTemplateLetter(model.Patient_Letter_Id, model.PatientId, (model.IsActive == false ? true : false), model.Name, model.Description, MDVUtility.ToInt32(""), model.PageNumber, model.RowsPerPage, null, null);
                dsLetter = obj.Data;
                if (dsLetter.Tables[dsLetter.PatientTemplateLetter.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsLetter.Tables[dsLetter.PatientTemplateLetter.TableName].Rows[0];
                    dr[dsLetter.PatientTemplateLetter.IsactiveColumn.ColumnName] = model.IsActive;
                }
                obj = BLLPatientObj.UpdatePatientLetter(dsLetter);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message,
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

        // Created By:  Muhammad Ahmad Imran
        // Created Date: 10/03/2016
        //OverView: Methods "UpdatePatientLetter" for Call BLL for Update Patient letter Data
        public string UpdatePatientLetter(LetterTemplateModel model)
        {
            try
            {
                DSLetter dsLetter = null;
                BLObject<DSLetter> obj = null;
                obj = BLLPatientObj.loadPatientTemplateLetter(model.Patient_Letter_Id, model.PatientId, true, "", "", MDVUtility.ToInt32(""), model.PageNumber, model.RowsPerPage, null, null);
                dsLetter = obj.Data;
                if (dsLetter.Tables[dsLetter.PatientTemplateLetter.TableName].Rows.Count > 0)
                {
                    DataRow dr = dsLetter.Tables[dsLetter.PatientTemplateLetter.TableName].Rows[0];
                    dr[dsLetter.PatientTemplateLetter.StatusColumn.ColumnName] = model.Status;
                    dr[dsLetter.PatientTemplateLetter.PatientLetterContentColumn.ColumnName] = model.PatientLetterContent;
                    dr[dsLetter.PatientTemplateLetter.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr[dsLetter.PatientTemplateLetter.ModifiedOnColumn.ColumnName] = DateTime.Now;


                    if (model.Status.ToLower() == "signed" && !string.IsNullOrEmpty(model.Base64String))
                    {
                        string[] arr = null;
                        arr = model.Base64String.Split(',');
                        model.Base64String = arr[1];
                        model.Base64String = model.Base64String.Replace(" ", "+");
                        int mod4 = model.Base64String.Length % 4;
                        if (mod4 > 0)
                            model.Base64String += new string('=', 4 - mod4);
                        model.Base64Content = Convert.FromBase64String(model.Base64String);
                        dr[dsLetter.PatientTemplateLetter.Base64ContentColumn.ColumnName] = DBNull.Value;
                        byte[] bytes = Convert.FromBase64String(model.Base64String);
                        string dt = DateTime.Now.ToShortDateString();
                        dt.Replace("/", "-");
                        string tm = DateTime.Now.ToShortTimeString();
                        string letterName = dt + " " + tm.Replace(" ", "") + "_" + model.Name + ".pdf";
                        letterName = letterName.Replace("/", "-").Replace(":", "");
                        var mnth = letterName.Split('-')[0];
                        var day = letterName.Split('-')[1];
                        var year = letterName.Split('-')[2];
                        mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                        day = day.Length == 1 ? "0" + day : day;
                        var mnt = year.Split(' ')[1].Split('_')[0];
                        year = year.Split(' ')[0];
                        mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                        letterName = mnth + "." + day + "." + year + "_" + model.Name + ".pdf";
                        string FilePath = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Patient Letters", model.PatientId, letterName, bytes);
                        dr[dsLetter.PatientTemplateLetter.UrlColumn.ColumnName] = FilePath;
                        dr[dsLetter.PatientTemplateLetter.FilePathColumn.ColumnName] = Path.GetFileName(FilePath);
                    }
                }

                #region Database Insertion
                obj = BLLPatientObj.UpdatePatientLetter(dsLetter);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        LetterId = model.Patient_Letter_Id,
                        Message = Common.AppPrivileges.Update_Message,
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
        // Created By:  Muhammad Ahmad Imran
        // Created Date: 09/03/2016
        //OverView: Methods "SavePatientLetter" for Call BLL for Save Patient letter Data
        public string SavePatientLetter(LetterTemplateModel model)
        {
            try
            {
                DSLetter dsLetter = new DSLetter();
                DSLetter.PatientTemplateLetterRow dr = dsLetter.PatientTemplateLetter.NewPatientTemplateLetterRow();
                dr.Patient_Letter_Id = -1;
                dr.Template_Letter_Id = model.TemplateLetterId;
                dr.PatientId = model.PatientId;
                dr.Isactive = true;
                dr.Status = model.Status;

                dr.PatientLetterContent = model.PatientLetterContent;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                if (model.VisitDate != null)
                {
                    dr.VisitDate = MDVUtility.ToDateTime(model.VisitDate);
                }
                else
                {
                    dr[dsLetter.PatientTemplateLetter.VisitDateColumn.ColumnName] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.VisitTime))
                {
                    dr.VisitTime = model.VisitTime;
                }
                else
                {
                    dr[dsLetter.PatientTemplateLetter.VisitTimeColumn.ColumnName] = DBNull.Value;
                }

                if (model.NotesId > 0)
                {
                    dr.NotesId = model.NotesId;
                }
                else
                {
                    dr[dsLetter.PatientTemplateLetter.NotesIdColumn.ColumnName] = DBNull.Value;
                }


                if (model.Status.ToLower() == "signed" && !string.IsNullOrEmpty(model.Base64String))
                {
                    string[] arr = null;
                    arr = model.Base64String.Split(',');
                    model.Base64String = arr[1];
                    model.Base64String = model.Base64String.Replace(" ", "+");
                    int mod4 = model.Base64String.Length % 4;
                    if (mod4 > 0)
                        model.Base64String += new string('=', 4 - mod4);
                    model.Base64Content = Convert.FromBase64String(model.Base64String);
                    dr.Base64Content = null;
                    byte[] bytes = Convert.FromBase64String(model.Base64String);
                    string dt = DateTime.Now.ToShortDateString();
                    dt.Replace("/", "-");
                    string tm = DateTime.Now.ToShortTimeString();
                    string letterName = dt + " " + tm.Replace(" ", "");
                    letterName = letterName.Replace("/", "-").Replace(":", "");
                    var mnth = letterName.Split('-')[0];
                    var day = letterName.Split('-')[1];
                    var year = letterName.Split('-')[2];
                    mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                    day = day.Length == 1 ? "0" + day : day;
                    var mnt = year.Split(' ')[1];
                    year = year.Split(' ')[0];
                    mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                    letterName = mnth + "." + day + "." + year + "_" + model.Name + ".pdf";
                    string FilePath = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Patient Letters", model.PatientId, letterName, bytes);
                    dr.Url = FilePath;
                    dr.FilePath = Path.GetFileName(FilePath);
                }
                dsLetter.PatientTemplateLetter.AddPatientTemplateLetterRow(dr);

                #region Database Insertion
                BLObject<DSLetter> obj = BLLPatientObj.InsertPatientLetter(dsLetter);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        LetterId = dsLetter.Tables[dsLetter.PatientTemplateLetter.TableName].Rows.Count > 0 ? dsLetter.Tables[dsLetter.PatientTemplateLetter.TableName].Rows[0][dsLetter.PatientTemplateLetter.Patient_Letter_IdColumn.ColumnName] : "",
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
        private byte[] convertHTMLtoPdf(string covData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var fields = ser.Deserialize<dynamic>(covData);

            var fromName = Convert.ToString(fields["From"]);
            var fromFax = Convert.ToString(fields["FaxNumber"]);
            var subject = Convert.ToString(fields["Subject"]);
            var faxNotes = Convert.ToString(fields["FaxNotes"]);
            faxNotes = faxNotes.Replace(" ", "+");
            int mod4 = faxNotes.Length % 4;
            if (mod4 > 0)
            {
                faxNotes += new string('=', 4 - mod4);
            }
            byte[] data = Convert.FromBase64String(faxNotes);
            string FaxNotes = Encoding.UTF8.GetString(data);

            var CName = Convert.ToString(fields["CompanyName"]);
            var CompanyName = "";
            if (CName != "undefined")
            {
                CompanyName = "Company Name: " + CName + "\n";
            }
            var PNo = Convert.ToString(fields["PhoneNo"]);
            var PhoneNo = "";
            if (PNo != "undefined" || PNo != "")
            {
                PhoneNo = "Phone No: " + PNo + "\n";
            }

            var TimeZone = Convert.ToString(fields["TimeZone"]);





            iTextSharp.text.Font patientNameFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font bodyFontParent = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);



            MemoryStream stream_ = new MemoryStream();


            Document pdfDocument = new Document(PageSize.LETTER, 30, 30, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(pdfDocument, stream_);
            pdfDocument.Open();

            Paragraph Heading = new Paragraph("Fax Transmission: ", patientNameFont);
            pdfDocument.Add(Heading);

            LineSeparator line3 = new LineSeparator(1f, 100f, BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -10);
            pdfDocument.Add(line3);


            PdfPTable detailsTable = new PdfPTable(1);
            float[] notifierHeaderWidths = new float[] { 20f };
            detailsTable.SetWidths(notifierHeaderWidths);
            detailsTable.TotalWidth = 520f;
            detailsTable.LockedWidth = true;
            detailsTable.HorizontalAlignment = Element.ALIGN_CENTER;
            detailsTable.DefaultCell.Border = Rectangle.NO_BORDER;
            detailsTable.SpacingBefore = 15f;
            detailsTable.SpacingAfter = 15f;

            Paragraph cellHeadingOne = new Paragraph("Attention To: ", patientNameFont);
            Paragraph cellHeadingTwo = new Paragraph("From: ", patientNameFont);


            Paragraph fromDetails = new Paragraph("Name: " + fromName + "\n" + "Fax Number: " + fromFax + "\n" + CompanyName + PhoneNo + "TimeZone: " + TimeZone + "\n" + "Date & Time: " + DateTime.Now.ToString(), bodyFont);


            PdfPCell cellOne = new PdfPCell();
            cellOne.HorizontalAlignment = Element.ALIGN_CENTER;
            cellOne.AddElement(cellHeadingOne);
            //cellOne.AddElement(attentionToDetails);
            cellOne.Border = Rectangle.NO_BORDER;


            PdfPCell cellTwo = new PdfPCell();
            cellTwo.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTwo.AddElement(cellHeadingTwo);
            cellTwo.AddElement(fromDetails);
            cellTwo.Border = Rectangle.NO_BORDER;


            //  detailsTable.AddCell(cellOne);
            detailsTable.AddCell(cellTwo);

            pdfDocument.Add(detailsTable);

            pdfDocument.Add(new Paragraph("Subject: " + subject, patientNameFont));
            pdfDocument.Add(line3);


            Paragraph commentHeader = new Paragraph("Comments and Notes:", bodyFontParent);
            commentHeader.SpacingBefore = 10f;
            pdfDocument.Add(commentHeader);

            PdfPTable notesTable = new PdfPTable(1);
            float[] notifierWidths = new float[] { 20f };
            notesTable.SetWidths(notifierHeaderWidths);
            notesTable.TotalWidth = 520f;
            notesTable.LockedWidth = true;
            notesTable.HorizontalAlignment = Element.ALIGN_CENTER;
            notesTable.DefaultCell.Border = Rectangle.NO_BORDER;
            notesTable.SpacingBefore = 15f;
            notesTable.SpacingAfter = 15f;

            PdfPCell cellnotes = new PdfPCell();
            cellnotes.Border = Rectangle.NO_BORDER;
            using (StringReader sr = new StringReader(FaxNotes))
            {
                List<IElement> elements = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(sr, null);
                foreach (IElement e in elements)
                {
                    //Add those elements to the paragraph
                    cellnotes.AddElement(e);
                }
            }

            notesTable.AddCell(cellnotes);
            pdfDocument.Add(notesTable);



            pdfDocument.Close();
            writer.Close();




            return stream_.ToArray();
        }


        private void insertInsuranceCardImagesInHTML(ref string HtmlDocument, DSLetter dsLetter, DataRow dr)
        {

            byte[] PrimaryInsuranceCardFrontArr = dr[dsLetter.PatientTemplateLetter.PrimaryInsuranceCardFrontColumn.ColumnName] as byte[];
            byte[] PrimaryInsuranceCardBackArr = dr[dsLetter.PatientTemplateLetter.PrimaryInsuranceCardBackColumn.ColumnName] as byte[];
            byte[] SecondaryInsuranceCardFrontArr = dr[dsLetter.PatientTemplateLetter.SecondaryInsuranceCardFrontColumn.ColumnName] as byte[];
            byte[] SecondaryInsuranceCardBackArr = dr[dsLetter.PatientTemplateLetter.SecondaryInsuranceCardBackColumn.ColumnName] as byte[];
            byte[] TertiaryInsuranceCardFrontArr = dr[dsLetter.PatientTemplateLetter.TertiaryInsuranceCardFrontColumn.ColumnName] as byte[];
            byte[] TertiaryInsuranceCardBackArr = dr[dsLetter.PatientTemplateLetter.TertiaryInsuranceCardBackColumn.ColumnName] as byte[];



            HtmlDocument = HtmlDocument.Replace("{{ Primary Insurance Card Front }}", PrimaryInsuranceCardFrontArr != null ? "<img src='" + "data:image/jpg;base64," + Convert.ToBase64String(PrimaryInsuranceCardFrontArr) + "'/>" : string.Empty);
            HtmlDocument = HtmlDocument.Replace("{{ Primary Insurance Card Back }}", PrimaryInsuranceCardBackArr != null ? "<img src='" + "data:image/jpg;base64," + Convert.ToBase64String(PrimaryInsuranceCardBackArr) + "'/>" : string.Empty);

            HtmlDocument = HtmlDocument.Replace("{{ Secondary Insurance Card Front }}", SecondaryInsuranceCardFrontArr != null ? "<img src='" + "data:image/jpg;base64," + Convert.ToBase64String(SecondaryInsuranceCardFrontArr) + "'/>" : string.Empty);
            HtmlDocument = HtmlDocument.Replace("{{ Secondary Insurance Card Back }}", SecondaryInsuranceCardBackArr != null ? "<img src='" + "data:image/jpg;base64," + Convert.ToBase64String(SecondaryInsuranceCardBackArr) + "'/>" : string.Empty);

            HtmlDocument = HtmlDocument.Replace("{{ Tertiary Insurance Card Front }}", TertiaryInsuranceCardFrontArr != null ? "<img src='" + "data:image/jpg;base64," + Convert.ToBase64String(TertiaryInsuranceCardFrontArr) + "'/>" : string.Empty);
            HtmlDocument = HtmlDocument.Replace("{{ Tertiary Insurance Card Back }}", TertiaryInsuranceCardBackArr != null ? "<img src='" + "data:image/jpg;base64," + Convert.ToBase64String(TertiaryInsuranceCardBackArr) + "'/>" : string.Empty);

            //  return "data:image/jpg;base64," + resultedHTML;


        }
        public byte[] ConvertHtmlToPdf(string xHtml, string css)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                using (var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 30f, 35f))
                {
                    var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, stream);

                    document.Open();

                    var tagProcessorFactory = (iTextSharp.tool.xml.html.DefaultTagProcessorFactory)iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory();
                    tagProcessorFactory.RemoveProcessor(iTextSharp.tool.xml.html.HTML.Tag.IMG);
                    tagProcessorFactory.AddProcessor(iTextSharp.tool.xml.html.HTML.Tag.IMG, new CustomImageTagProcessor());

                    var htmlPipelineContext = new iTextSharp.tool.xml.pipeline.html.HtmlPipelineContext(null);
                    htmlPipelineContext.SetTagFactory(tagProcessorFactory);

                    var pdfWriterPipeline = new iTextSharp.tool.xml.pipeline.end.PdfWriterPipeline(document, writer);
                    var htmlPipeline = new iTextSharp.tool.xml.pipeline.html.HtmlPipeline(htmlPipelineContext, pdfWriterPipeline);

                    var charset = System.Text.Encoding.UTF8;
                    // get an ICssResolver and add the custom CSS
                    var cssResolver = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                    cssResolver.AddCssFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/Default/bootstrap.css"), true);
                    cssResolver.AddCssFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/Blue/theme-custom.css"), true);
                    cssResolver.AddCss(css, "utf-8", true);
                    var cssResolverPipeline = new iTextSharp.tool.xml.pipeline.css.CssResolverPipeline(
                        cssResolver, htmlPipeline
                    );

                    var worker = new iTextSharp.tool.xml.XMLWorker(cssResolverPipeline, true);
                    var parser = new iTextSharp.tool.xml.parser.XMLParser(worker);
                    using (var stringReader = new System.IO.StringReader(xHtml))
                    {
                        parser.Parse(stringReader);
                    }

                    document.Close();
                    MemoryStream stream2 = new MemoryStream(stream.ToArray());
                    PdfReader npdf = new PdfReader(stream2);
                    MemoryStream outstream = new MemoryStream();
                    iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE);
                    PdfPTable table2 = new PdfPTable(2);
                    using (PdfStamper stamper = new PdfStamper(npdf, outstream, '\0', true))
                    {
                        stamper.Writer.CloseStream = false;
                        int PageCount = npdf.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                            Paragraph para = new Paragraph(String.Format("Page {0} of {1}", i, PageCount), bodyFont);

                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, para, 555, 17, 0);
                        }
                    }
                    return outstream.GetBuffer();
                }
            }
        }

    }
}