using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.Model.Clinical.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates.OrderSet
{
    public class OS_PatientEducation
    {
        private BLLOrderSet BLLOrderSetObj = null;
        private BLLPatientEducation BLLPatientEducationObj = null;
        public OS_PatientEducation()
        {
            BLLOrderSetObj = new BLLOrderSet();
            BLLPatientEducationObj = new BLLPatientEducation();
        }
        private static OS_PatientEducation _instance = null;
        public static OS_PatientEducation Instance()
        {
            if (_instance == null)
                _instance = new OS_PatientEducation();
            return _instance;
        }

        #region " Order Set Patient Education "
        public string insertOrderSetPatientEducation(OrderSetPatientEducationModel model)
        {
            OrderSetPatientEducationResponse responseModel = new OrderSetPatientEducationResponse();
            try
            {
                BLObject<string> obj = BLLOrderSetObj.insertOrderSetPatientEducation(model);
                if (obj.Data != null && obj.Data != "-1")
                {
                    responseModel.OrderSetPatEducationId = obj.Data.ToString();
                    responseModel.status = true;
                    responseModel.Message = Common.AppPrivileges.Save_Message;
                }
                else
                {
                    responseModel.status = false;
                    responseModel.OrderSetPatEducationId = model.OrderSetPatEducationId;
                    responseModel.Message = obj.Message == "" ? "Order Set Patient Education already exist" : obj.Message;
                }


            }
            catch (Exception ex)
            {

                responseModel.status = false;
                responseModel.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                responseModel.OrderSetPatEducationId = model.OrderSetPatEducationId;

            }
            if (responseModel.status)
            {
                var response = new
                {
                    status = true,
                    OrderSetPatEducationId = responseModel.OrderSetPatEducationId,
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

        public string loadOrderSetPatientEducation(OrderSetPatientEducationModel model)
        {
            try
            {
                BLObject<List<OrderSetPatientEducationModel>> objEduInfo = null;
                BLObject<List<OrderSetPatientEducationModel>> objEduNonInfo = null;
                if (string.IsNullOrEmpty(model.DocType))
                {
                    objEduInfo = BLLOrderSetObj.loadOrderSetPatientEducation(MDVUtility.ToInt64(model.OrderSetPatEducationId), MDVUtility.ToInt64(model.OrderSetId), "1", model.PageNumber, model.RowsPerPage);
                    objEduNonInfo = BLLOrderSetObj.loadOrderSetPatientEducation(MDVUtility.ToInt64(model.OrderSetPatEducationId), MDVUtility.ToInt64(model.OrderSetId), "0", model.PageNumber, model.RowsPerPage);
                }
                else
                {
                    if (model.DocType == "0")
                    {
                        objEduNonInfo = BLLOrderSetObj.loadOrderSetPatientEducation(MDVUtility.ToInt64(model.OrderSetPatEducationId), MDVUtility.ToInt64(model.OrderSetId), "0", model.PageNumber, model.RowsPerPage); // For Non Info Based documents
                    }
                    if (model.DocType == "1")
                    {
                        objEduInfo = BLLOrderSetObj.loadOrderSetPatientEducation(MDVUtility.ToInt64(model.OrderSetPatEducationId), MDVUtility.ToInt64(model.OrderSetId), "1", model.PageNumber, model.RowsPerPage);// For Info Based documents
                    }
                }

                if ((objEduInfo.Data != null && objEduInfo.Data.Count > 0) && (objEduNonInfo.Data != null && objEduNonInfo.Data.Count > 0))
                {

                    var response = new
                    {
                        status = true,
                        NonInfoDocumentLoad_JSON = objEduNonInfo.Data,
                        InfoDocumentLoad_JSON = objEduInfo.Data,
                        NonInfoPatEducationCount = objEduNonInfo.Data.Count,
                        InfoPatEducationCount = objEduInfo.Data.Count,
                        iTotalDisplayRecords = objEduInfo.Data.Count > 0 ? objEduInfo.Data.Count : 0,
                        InfoCount = objEduInfo.Data.Count > 0 ? MDVUtility.ToInt32(objEduInfo.Data[0].InfoDoc) : 0,
                        NonInfoCount = objEduNonInfo.Data.Count > 0 ? MDVUtility.ToInt32(objEduNonInfo.Data[0].NonInfoDoc) : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else if ((objEduNonInfo.Data != null && objEduNonInfo.Data.Count > 0))
                {

                    var response = new
                    {
                        status = true,
                        NonInfoDocumentLoad_JSON = objEduNonInfo.Data,
                        InfoDocumentLoad_JSON = "[]",
                        NonInfoPatEducationCount = 0,
                        InfoPatEducationCount = objEduInfo.Data.Count,
                        iTotalDisplayRecords = objEduNonInfo.Data.Count > 0 ? objEduNonInfo.Data.Count : 0,
                        InfoCount = 0,
                        NonInfoCount = objEduNonInfo.Data.Count > 0 ? MDVUtility.ToInt32(objEduNonInfo.Data[0].NonInfoDoc) : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else if ((objEduInfo.Data != null && objEduInfo.Data.Count > 0))
                {

                    var response = new
                    {
                        status = true,
                        NonInfoDocumentLoad_JSON = "[]",
                        InfoDocumentLoad_JSON = objEduInfo.Data,
                        NonInfoPatEducationCount = 0,
                        InfoPatEducationCount = objEduInfo.Data.Count,
                        iTotalDisplayRecords = objEduInfo.Data.Count > 0 ? objEduInfo.Data.Count : 0,
                        InfoCount = objEduInfo.Data.Count > 0 ? MDVUtility.ToInt32(objEduInfo.Data[0].InfoDoc) : 0,
                        NonInfoCount = 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        NonInfoDocumentLoad_JSON = "[]",
                        InfoDocumentLoad_JSON = "[]",
                        NonInfoPatEducationCount = 0,
                        InfoPatEducationCount = 0,
                        iTotalDisplayRecords = 0,
                        InfoCount = 0,
                        NonInfoCount = 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    NonInfoDocumentLoad_JSON = "[]",
                    InfoDocumentLoad_JSON = "[]",
                    NonInfoPatEducationCount = 0,
                    InfoPatEducationCount = 0,
                    iTotalDisplayRecords = 0,
                    InfoCount = 0,
                    NonInfoCount = 0,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string deleteOrderSetPatientEducation(string OrderSetPatEducationId, string DocId)
        {
            try
            {
                if (string.IsNullOrEmpty(OrderSetPatEducationId) || string.IsNullOrEmpty(DocId))
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
                    BLObject<string> obj = BLLOrderSetObj.deleteOrderSetPatientEducation(OrderSetPatEducationId, DocId);
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
        public string InsertAdmin_OSPatientEducation(OrderSetPatientEducationModel model)
        {
            try
            {
                #region Binding DataSet Information
                DSOrderSet dsPatEdu = new DSOrderSet();
                DSOrderSet.PatientEducationRow dr = dsPatEdu.PatientEducation.NewPatientEducationRow();

                dr.OrderSetId = MDVUtility.ToInt64(model.OrderSetId);
                dr.DocType = model.DocType;
                dr.DocumentName = model.DocumentName;
                dr.DocId = MDVUtility.ToInt32(model.DocId);
                if (!string.IsNullOrEmpty(model.FileStream))
                {
                    dr.FileType = model.FileType;
                    byte[] currentFileStream = Convert.FromBase64String(model.FileStream);
                    dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
                    if (model.FileType == "application/pdf")
                        dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
                    else
                        dr.Pages = 1;
                }
                else
                    dr.Pages = 0;

                dr.Comments = model.Comments;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);

                #endregion

                #region Database Insertion
                dsPatEdu.PatientEducation.AddPatientEducationRow(dr);
                BLObject<DSOrderSet> obj = BLLOrderSetObj.InsertAdmin_OSPatientEducation(dsPatEdu);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        OrderSetPatEducationId = dsPatEdu.Tables[dsPatEdu.PatientEducation.TableName].Rows[0][dsPatEdu.PatientEducation.OrderSetPatEducationIdColumn.ColumnName],
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

        #endregion
    }
}