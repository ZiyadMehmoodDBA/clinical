using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.IEHR.Common;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data.SqlClient;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using System.Runtime.InteropServices;
using System.Threading;
using MDVision.IEHR.Controls.Clinical;
using Newtonsoft.Json.Linq;
using MDVision.Model.Patient;
using System.Web.Configuration;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.Business.BLL;
using MDVision.IEHR.EMR.Model.Patient;
using MDVision.IEHR.EMR.Helpers.Patient;
using MDVision.Model.Native;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MDVision.Model.Native.Patient;

namespace MDVision.IEHR.Controls.Patient.Demographics
{
    public class Patient_DemographicNative
    {
        private BLLPatient BLLPatientObj = null;
        private BLLRcopia BLLRcopiaObj = null;
        private BLLCQM BLLCQMObj = null;
        private BLLMobileApp BLLMobileApp = null;
        public Patient_DemographicNative()
        {
            BLLPatientObj = new BLLPatient();
            BLLRcopiaObj = new BLLRcopia();
            BLLCQMObj = new BLLCQM();
            BLLMobileApp = new BLLMobileApp();

        }

        #region Singleton
        private static Patient_DemographicNative _obj = null;
        private static bool isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
        public static Patient_DemographicNative Instance()
        {
            if (_obj == null)
            {
                _obj = new Patient_DemographicNative();
                //  isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
            }
            return _obj;
        }
        #endregion
        public string FillPatientNative(PatientDemographicModelNative request_model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(request_model)))
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
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    Int64? PatientId = !string.IsNullOrEmpty(request_model.PatientID) ? MDVUtility.ToInt64(request_model.PatientID) :(long?) null;
                    string DimmyPatientId= !string.IsNullOrEmpty(request_model.DimmyPatientId) ? MDVUtility.ToStr(request_model.DimmyPatientId) :null;
                    PatientDemographicModelNative response_model = BLLMobileApp.FillPatient_Native(PatientId, DimmyPatientId,request_model.RequestStatus);
                    if (!string.IsNullOrEmpty(response_model.PatientProfileImagePath))
                    {
                        string FileServerPath = System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesPath"];
                        MDVSession.Current.ImageID = response_model.PatientProfileImagePath + "|" + response_model.PatientProfileThumbnailPath;
                        string imgFullPath = Path.Combine(FileServerPath, response_model.PatientProfileImagePath);
                        if (File.Exists(imgFullPath))
                           {
                            string ImgBase64String = Convert.ToBase64String(File.ReadAllBytes(imgFullPath));
                            response_model.imgPatient = "data:" + response_model.ImageType + ";base64," + ImgBase64String;
                           }
                    }
                    js.MaxJsonLength = Int32.MaxValue;
                    var response = new
                    {

                        status = true,
                        DemographicFill_JSON = js.Serialize(response_model),
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
        public string UpdatePatientDemographicsNative(PatientDemographicModelNative request_model)
        {


            try
            {
                if (!string.IsNullOrEmpty(request_model.PatientID)||!string.IsNullOrEmpty(request_model.DimmyPatientId))
                {
                    string image = request_model.imgPatient;
                    request_model.imgPatient = null;
                    if (request_model.lstChangedColumns.Count > 0)
                    {
                        //  IList<string> strings = SearchedfieldsJSON.lstChangedColumns;
                        request_model.listChangedColumns = string.Join(",", request_model.lstChangedColumns.Select(i => i.columnName));
                    }
                  request_model.PatientID=  !string.IsNullOrEmpty(request_model.PatientID) ? request_model.PatientID : null;
                  request_model.DimmyPatientId= !string.IsNullOrEmpty(request_model.DimmyPatientId) ? request_model.DimmyPatientId : null;
                    string Result = BLLMobileApp.UpdatePatientDemographicsNative(request_model);
                    long PatientId;
                    bool isNum = long.TryParse(Result, out PatientId);

                    if (isNum)
                    {
                   
                            if ( !string.IsNullOrEmpty(image) && !image.Contains("default"))
                            {
                                string strBase64 = image.Split(',')[1];
                                string fileType = image.Split(',')[0].Split(':')[1].Split(';')[0];
                                string isSaved = new Patient_Demographic().SavePatientDocument(PatientId, strBase64, fileType, request_model.PatientProfileImagePath, true);
                            }

                        
                    }

                    if (isNum)                   
                    {
                        DSPatient dsPatient = new DSPatient();
                        DataRow row = dsPatient.Tables["Patients"].NewRow();
                        row["PatientId"] = !string.IsNullOrEmpty(request_model.PatientID)?request_model.PatientID:MDVUtility.ToStr( PatientId);
                        row["AccountNumber"] = request_model.AccountNo;
                        row["FirstName"] = request_model.FirstName;
                        row["MI"] = request_model.MI;
                        row["LastName"] = request_model.LastName;

                        row["DOB"] = request_model.DOB;
                        row["Gender"] = request_model.Gender;
                        row["Address1"] = request_model.Address1;
                        row["City"] = request_model.City;
                        row["State"] = request_model.State;
                        row["ZIPCode"] = request_model.ZipCode;


                        Thread thread = new Thread(new ThreadStart(delegate ()
                        {
                            try
                            {
                                //ResponseOfDrFirst = JObject.Parse( );
                                BLLRcopiaObj.getRcopiaResponseUrl("UpdatePatient", row, !string.IsNullOrEmpty(request_model.PatientID) ? request_model.PatientID : MDVUtility.ToStr(PatientId));
                            }
                            catch (Exception ex)
                            {
                                MDVLogger.SendExcepToDB(ex, "UpdatePatientNative", null);
                            }
                        }));
                        thread.IsBackground = true;
                        thread.Start();


                        var response = new
                        {
                            status = true,
                            PatientId=PatientId,
                            Message = "Approved Successfully!..."
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Result
                        };
                        return (JsonConvert.SerializeObject(response));

                    }

                }
                else
                {
                    var response1 = new
                    {
                        status = false,
                        Message = "No Change has been Made from User."
                    };
                    return (JsonConvert.SerializeObject(response1));

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
    }
}

