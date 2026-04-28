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
using MDVision.DataAccess.DCommon;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Helpers.Patient
{
    public class PatientCustomFormHelper
    {
        private BLLPatient BLLPatObj = null;
        public PatientCustomFormHelper()
        {
            BLLPatObj = new BLLPatient();
        }
        private static PatientCustomFormHelper _instance = null;
        public static PatientCustomFormHelper Instance()
        {
            if (_instance == null)
                _instance = new PatientCustomFormHelper();
            return _instance;
        }
        //public byte[] AddWaterMarkToPDF(byte[] byteArr, string strWaterMark)
        //{
        //    //create pdfreader object to read sorce pdf
        //    PdfReader pdfReader = new PdfReader(byteArr);
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        //create pdfstamper object which is used to add addtional content to source pdf file
        //        PdfStamper pdfStamper = new PdfStamper(pdfReader, stream);
        //        //iterate through all pages in source pdf
        //        for (int pageIndex = 1; pageIndex <= pdfReader.NumberOfPages; pageIndex++)
        //        {
        //            //Rectangle class in iText represent geomatric representation... in this case, rectanle object would contain page geomatry
        //            iTextSharp.text.Rectangle pageRectangle = pdfReader.GetPageSizeWithRotation(pageIndex);
        //            //pdfcontentbyte object contains graphics and text content of page returned by pdfstamper

        //            // That line was adding water mark beneath the content
        //            //PdfContentByte pdfData = pdfStamper.GetUnderContent(pageIndex);
        //            PdfContentByte pdfData = pdfStamper.GetOverContent(pageIndex);

        //            //create fontsize for watermark

        //            pdfData.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 20);
        //            //create new graphics state and assign opacity
        //            PdfGState graphicsState = new PdfGState();
        //            graphicsState.FillOpacity = 0.4F;
        //            //set graphics state to pdfcontentbyte
        //            pdfData.SetGState(graphicsState);
        //            //set color of watermark
        //            pdfData.SetColorFill(BaseColor.BLUE);
        //            //indicates start of writing of text
        //            pdfData.BeginText();
        //            //show text as per position and rotation
        //            pdfData.ShowTextAligned(Element.ALIGN_BOTTOM, strWaterMark, pageRectangle.Width - (pageRectangle.Width - 20), pageRectangle.Height - (pageRectangle.Height - 35), 0);
        //            //call endText to invalid font set
        //            pdfData.EndText();
        //        }
        //        //close stamper and output filestream
        //        pdfStamper.Close();

        //        return stream.GetBuffer();
        //    }
        //}
        public string insertCustomForm(PatientCustomFormModel model)
        {
            CustomFormResponse responseModel = new CustomFormResponse();
            try
            {
                byte[] currentFileStream = Convert.FromBase64String(model.CustomFormBase64);
                string strPatientID = model.PatientId;
                Int64 PatientID = 0;
                if (!string.IsNullOrWhiteSpace(strPatientID))
                {
                    PatientID = Convert.ToInt64(model.PatientId);
                }

                if (model.IsSigned == "1")
                {
                    var strWaterMark = "Electronically signed by " + ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() + " on " + DateTime.Now.ToString();
                    byte[] NewbyteArr = new BLLPatient().AddWaterMarkToPDF(currentFileStream, strWaterMark);
                    model.Url = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Custom Form", PatientID, "Custom Form.pdf", NewbyteArr);
                }
                else
                {
                    model.Url = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Custom Form", PatientID, "Custom Form.pdf", currentFileStream);
                }
                BLObject<string> obj = BLLPatObj.insertPatientCustomForm(model);
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
                    responseModel.Message = obj.Message;
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

        public string GetProviderCPTs(long CustomFormId, long ProviderId )
        {
            try
            {
                BLObject<List<PatientCustomFormModel>> obj = BLLPatObj.GetProviderCPTs(CustomFormId, ProviderId);
                var ser = new JavaScriptSerializer();
                if (obj.Data[0].ProviderCPTsList.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        ProviderCPTsList_JSON = ser.Serialize(obj.Data[0].ProviderCPTsList)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
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
        
        public string GetPatientDocumentId(string CustomFormId)
        {
            CustomFormResponse responseModel = new CustomFormResponse();
            try
            {

                BLObject<string> obj = BLLPatObj.GetPatientDocumentId(CustomFormId);
                if (!(string.IsNullOrEmpty(obj.Data)) && MDVUtility.ToInt64(obj.Data)>0)
                {
                    var response = new
                    {
                        status = true,
                        patDocId = obj.Data.ToString(),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        patDocId = -1,
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
        
        internal string getReportHeaderFooter(PatientCustomFormModel model)
        {
            try
            {
                DSReportHeader dsreportHeader = null;
                BLObject<DSReportHeader> obj = new BLLAdminClinical().getReportHeaderTagsValue(MDVUtility.ToInt64(model.PatientId), 0, -1, "Custom Forms");
                dsreportHeader = obj.Data;
                if (dsreportHeader.ReportHeaderTags.Rows.Count > 0)
                {
                    DSReportHeader.ReportHeaderTagsRow dr = (DSReportHeader.ReportHeaderTagsRow)dsreportHeader.Tables[dsreportHeader.ReportHeaderTags.TableName].Rows[0];

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        HeaderLogo = dr["HeaderLogo"],
                        FooterText = dr["FooterText"],
                        PatientText = dr["PatientText"],
                        ProviderText = dr["ProviderText"],
                        PracticeText = dr["PracticeText"],
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        HeaderLogo = "",
                        FooterText = "",
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



        public string updatePatientCustomForm(PatientCustomFormModel model)
        {
            CustomFormResponse responseModel = new CustomFormResponse();
            try
            {
                byte[] currentFileStream = Convert.FromBase64String(model.CustomFormBase64);
                string strPatientID = model.PatientId;
                Int64 PatientID = 0;
                if (!string.IsNullOrWhiteSpace(strPatientID))
                {
                    PatientID = Convert.ToInt64(model.PatientId);
                }

                if (model.IsSigned == "1")
                {
                    var strWaterMark = "Electronically signed by " + ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() + " on " + DateTime.Now.ToString();
                    byte[] NewbyteArr = new BLLPatient().AddWaterMarkToPDF(currentFileStream, strWaterMark);
                    model.Url = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Custom Form", PatientID, "Custom Form.pdf", NewbyteArr);
                }
                else
                {
                    model.Url = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Custom Form", PatientID, "Custom Form.pdf", currentFileStream);
                }
                
                BLObject<string> obj = BLLPatObj.updatePatientCustomForm(model);
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
                    responseModel.Message = obj.Message;
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


        public string loadPatientCustomForm(PatientCustomFormModel model)
        {
            try
            {
                List<PatientCustomFormModel> listCustomForm = new List<PatientCustomFormModel>();
                BLObject<List<PatientCustomFormModel>> obj = BLLPatObj.loadPatientCustomForm(model.FormName, Convert.ToInt16(model.IsActive), model.PatientId, model.PageNumber, model.RowsPerPage);

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

        public string deletePatientCustomForm(string formId)
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
                    BLObject<string> obj = BLLPatObj.deletePatientCustomForm(formId);
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

        public string activeInactivePatientCustomForm(string formId, string isActive)
        {
            try
            {
                BLObject<string> obj = BLLPatObj.activeInactivePatientCustomForm(formId, isActive);
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

        public string fillPatientCustomForm(PatientCustomFormModel model)
        {
            try
            {
                List<PatientCustomFormModel> listCustomForm = new List<PatientCustomFormModel>();
                BLObject<List<PatientCustomFormModel>> obj = BLLPatObj.fillPatientCustomForm(model.PatientCustomFormId);

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


    }
}