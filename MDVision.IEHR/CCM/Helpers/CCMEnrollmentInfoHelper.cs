using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.Model.CCM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Text;

namespace MDVision.IEHR.Controls.CCM.Herlpers
{

    public class CCMEnrollmentInfoHelper
    {
        public BLLCCM BLLCCCMObj = null;
        public CCMEnrollmentInfoHelper()
        {
            BLLCCCMObj = new BLLCCM();
        }


        public string SaveCCMEnrollmentInfo(CCMEnrollmentInfoModel model)
        {
            try
            {
                string CCMStream = model.ConsentFileStream;
                byte[] Filestream = null;

                if (!String.IsNullOrEmpty(CCMStream))
                {
                    string FileName = string.Empty;
                    if (model.ISVerbal == "1" && string.IsNullOrWhiteSpace(model.ConsentPath))
                    {
                        FileName = "Verbal Consent.pdf";
                    }
                    else if ((model.ISVerbal == "0" || string.IsNullOrWhiteSpace(model.ISVerbal)) && string.IsNullOrWhiteSpace(model.ConsentPath))
                    {
                        FileName = "No Consent.pdf";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(model.ConsentFileName))
                        {
                            //FileName = model.ConsentFileName + ".pdf";
                            if (!model.ConsentFileName.Contains(".pdf"))
                                FileName = model.ConsentFileName + ".pdf";
                            else
                                FileName = model.ConsentFileName;
                        }
                    }
                    CCMStream = CCMStream.Replace(" ", "+");
                    int mod4 = CCMStream.Length % 4;
                    if (mod4 > 0)
                    {
                        CCMStream += new string('=', 4 - mod4);
                    }

                    Filestream = Convert.FromBase64String(CCMStream);
                    //model.BinaryData = Filestream;
                    Int64 PatientID = 0;
                    if(!string.IsNullOrWhiteSpace(model.PatientId) && model.PatientId != "-1")
                    {
                        PatientID = Convert.ToInt64(model.PatientId);
                    }
                    model.Url = CommonFunc.SaveDocumentToFolder(null, "CCM", "CCM", PatientID, FileName, Filestream);
                }
                //else
                //{
                //    model.BinaryData = Encoding.ASCII.GetBytes("");
                //}
                
                string demographicsData = BLLCCCMObj.SaveCCMEnrollmentInfo(model);

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

       



        public string LoadCCMEnrollmentInfoDetail(Int32 EnrollmentInfoId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(EnrollmentInfoId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    CCMEnrollmentInfoModel CCMEnrollmentModel = null;
                    BLObject<CCMEnrollmentInfoModel> obj = BLLCCCMObj.LoadCCMEnrollmentInfoDetail(EnrollmentInfoId);
                    if (obj.Data != null)
                    {
                        CCMEnrollmentModel = obj.Data;




                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            CCMEnrollmentInfoFill_JSON = CCMEnrollmentModel
                        };
                        return (JsonConvert.SerializeObject(response));
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

        public string UpdateCCMEnrollmentInfo(CCMEnrollmentInfoModel model)
        {
            try
            {
                string CCMStream = model.ConsentFileStream;

                if (!String.IsNullOrEmpty(CCMStream))
                {
                    string FileName = string.Empty;
                    if (model.ISVerbal == "1" && string.IsNullOrWhiteSpace(model.ConsentPath))
                    {
                        FileName = "Verbal Consent.pdf";
                    }
                    else if ((model.ISVerbal == "0" || string.IsNullOrWhiteSpace(model.ISVerbal)) && string.IsNullOrWhiteSpace(model.ConsentPath))
                    {
                        FileName = "No Consent.pdf";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(model.ConsentFileName))
                        {
                            FileName = model.ConsentFileName;
                        }
                    }
                    CCMStream = CCMStream.Replace(" ", "+");
                    int mod4 = CCMStream.Length % 4;
                    if (mod4 > 0)
                    {
                        CCMStream += new string('=', 4 - mod4);
                    }
                    byte[] Filestream = null;
                    Filestream = Convert.FromBase64String(CCMStream);
                    //model.BinaryData = Filestream;
                    Int64 PatientID = 0;
                    if (!string.IsNullOrWhiteSpace(model.PatientId) && model.PatientId != "-1")
                    {
                        PatientID = Convert.ToInt64(model.PatientId);
                    }
                    model.Url = CommonFunc.SaveDocumentToFolder(null, "CCM Documents", "CCM", PatientID, FileName, Filestream);
                }
                else
                {
                    model.BinaryData = Encoding.ASCII.GetBytes("");
                }
                    
                string demographicsData = BLLCCCMObj.UpdateCCMEnrollmentInfo(model);

                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Update_Message,
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string ResumeCCMEnrollmentInfo(CCMEnrollmentInfoModel model)
        {
            try
            {
                string EnrollmentInfoId = BLLCCCMObj.ResumeCCMEnrollmentInfo(model);

                if (MDVUtility.ToInt32(EnrollmentInfoId) > 0)
                {

                    var response = new
                    {
                        status = true,
                        Message = "Successfully Resumed",
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Failed to Resume",
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string TerminationCCMEnrollmentInfo(MDVision.Model.CCM.CCMHub.CCMTermination model)
        {
            try
            {
                string EnrollmentInfoId = BLLCCCMObj.TerminationCCMEnrollmentInfo(model);

                if (MDVUtility.ToInt32(EnrollmentInfoId) > 0)
                {

                    var response = new
                    {
                        CCMStatus = model.Status,
                        status = true,
                        Message = "Successfully " + model.Status + ".",
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        CCMStatus = model.Status,
                        status = false,
                        Message = "Failed to " + model.Status + ".",
                    };
                    return (JsonConvert.SerializeObject(response));

                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    CCMStatus = model.Status,
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #region DECLINE

        public string SaveCCMEnrollmentDecline(CCMEnrollmentInfoModel model)
        {
            try
            {
                string EnrollmentInfoId = BLLCCCMObj.SaveCCMEnrollmentDecline(model);

                if (MDVUtility.ToInt32(EnrollmentInfoId) > 0)
                {

                    var response = new
                    {
                        status = true,
                        Message = "Successfully Declined",
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Failed to Decline",
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


        #endregion


    }
}