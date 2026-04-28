using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDVision.Business.BCommon;
using MDVision.Datasets;
using System.Data;
using System.Drawing;
using System.IO;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;

namespace MDVision.IEHR.Controls.CommonControls
{
    public class Upload_Image
    {
        #region Singleton
        private static Upload_Image _obj = null;
        public static Upload_Image Instance()
        {
            if (_obj == null)
                _obj = new Upload_Image();
            return _obj;
        }
        #endregion

        #region Private Functions

        //private string UploadFile(string patientId, HttpPostedFile file)
        //{

        //    string FileExtension = "jpg";
        //    string strFile = GetFileServerPath(FileExtension, false, patientId);
        //    string strFullPath = HttpContext.Current.Server.MapPath(strFile);
        //    file.SaveAs(strFullPath);
        //    SavePatientImageFileToServer(patientId, strFullPath);

        //    strFile = GetServerSideFilePath(FileExtension, false, patientId);
        //    SavePatientSnapShot(patientId, strFile);

        //    return string.Format("{0}/PatientImages{1}", HttpRuntime.AppDomainAppVirtualPath, strFile);

        //    //SetImage();
        //    //this.Close();

        //    // file.SaveAs(string.Format("{0}\\abc.jpg", _default.Server_MapPath));
        //}

        /// <summary>


        private string UploadPatientImage(Int64 PatientID, HttpPostedFile file)
        {
            try
            {

                string strPath = "";
                if (file.ContentLength > 0)
                {

                    strPath = MDVUtility.GetImagePath(MDVSession.Current.ImagePath, MDVSession.Current.EntityId, PatientID.ToString(), "jpg");
                    file.SaveAs(HttpContext.Current.Server.MapPath(strPath));
                    System.Threading.Thread.Sleep(5000);
                    strPath = strPath.Replace("~", "");
                }




                string successMsg;

                successMsg = Common.AppPrivileges.Upload_Image;
                var response = new
                {
                    status = true,
                    Image_url = strPath != "" ? string.Format("{0}{1}", HttpRuntime.AppDomainAppVirtualPath, strPath) : strPath,
                    message = successMsg
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string UploadPatientWebCamImage(Int64 PatientID, byte[] byteArrayIn)
        {
            try
            {
                string strPath = "";
                if (byteArrayIn.Length > 0)
                {
                    strPath = MDVUtility.GetImagePath(MDVSession.Current.ImagePath, MDVSession.Current.EntityId, PatientID.ToString(), "jpg");
                    MemoryStream ms = new MemoryStream(byteArrayIn);
                    Image img = Image.FromStream(ms);
                    img.Save(HttpContext.Current.Server.MapPath(strPath));
                    System.Threading.Thread.Sleep(5000);
                    strPath = strPath.Replace("~", "");
                }

                string successMsg;
                successMsg = Common.AppPrivileges.Upload_Image;
                var response = new
                {
                    status = true,
                    Image_url = strPath != "" ? string.Format("{0}{1}", HttpRuntime.AppDomainAppVirtualPath, strPath) : strPath,
                    message = successMsg
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
       

        //private string UploadPatientImage1(Int64 PatientID, HttpPostedFile file)
        //{
        //    try
        //    {
        //        if (PatientID == 0)
        //            PatientID = -1;

        //        DSPatient dsPatient = null;
        //        BLObject<DSPatient> obj = BLLPatientObj.LoadPatient(PatientID, null, null, null, null, null, 1, 15, null);
        //        dsPatient = obj.Data;


        //        if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
        //        {
        //            DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];


        //            //System.IO.BinaryReader br = new System.IO.BinaryReader(file.InputStream);
        //            //byte[] ImageInput = br.ReadBytes(file.ContentLength);
        //            string strPath = "";
        //            if (file.ContentLength > 0)
        //            {
        //                byte[] ImageInput = new byte[file.ContentLength];
        //                //// Read the file into the byte array.
        //                file.InputStream.Read(ImageInput, 0, file.ContentLength);
        //                dr[dsPatient.Patients.PatientImageColumn.ColumnName] = ImageInput;
        //                strPath = MDVUtility.GetImagePath(Common.AppConfig.ImagePath, MDVSession.Current.EntityId, PatientID.ToString(), "jpg");
        //                file.SaveAs(HttpContext.Current.Server.MapPath(strPath));
        //                System.Threading.Thread.Sleep(2000);
        //                strPath = strPath.Replace("~", "");
        //            }




        //            BLObject<DSPatient> objPatient = BLLPatientObj.UpdatePatient(dsPatient);
        //            string successMsg;
        //            if (objPatient.Data != null)
        //            {
        //                successMsg = Common.AppPrivileges.Upload_Image;
        //                var response = new
        //                {
        //                    status = true,
        //                    Image_url = string.Format("{0}{1}", HttpRuntime.AppDomainAppVirtualPath, strPath),
        //                    message = successMsg
        //                };
        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    Message = objPatient.Message
        //                };
        //                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //            }
        //        }
        //        else
        //        {
        //            var response = new
        //            {
        //                status = false,
        //                Message = obj.Message
        //            };
        //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new
        //        {
        //            status = false,
        //            Message =MDVCustomException.HumanReadableMessage(ex.Message),
        //        };
        //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    }
        //}
        private string RemovePatientImage(Int64 PatientID)
        {
            try
            {

                MDVUtility.DeleteImageFile(MDVSession.Current.ImagePath, MDVSession.Current.EntityId, PatientID.ToString(), "jpg");




                var response = new
                {
                    status = true,
                    //Image_url = string.Format("{0}{1}", HttpRuntime.AppDomainAppVirtualPath, strPath),
                    message = ""
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }





        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the Basic Free Group Commands and call to the respective method
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_IMAGE":
                    {


                        string strJSONData = "";

                        try
                        {
                            string patientId = "";
                            if (context.Request.Files.Count > 0)
                            {
                                HttpPostedFile file = context.Request.Files[0];
                                patientId = context.Request.Form["PatientID"];

                                if (string.IsNullOrEmpty(patientId))
                                {
                                    throw new Exception("Unable to save patient image.");

                                }
                                strJSONData = UploadPatientImage(MDVUtility.ToInt64(patientId), file);

                            }
                            else
                            {
                                patientId = context.Request["PatientID"];
                                strJSONData = UploadPatientImage(MDVUtility.ToInt64(patientId), null);
                            }
                        }
                        catch (Exception ex)
                        {
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);

                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);


                    }
                    break;
                case "SAVE_WEBCAM_IMAGE":
                    {


                        string strJSONData = "";

                        try
                        {
                            string patientId = "";
                            string uploadImageType = "";
                            uploadImageType = context.Request["uploadImageType"];
                            if (MDVUtility.ToConvertInt32(uploadImageType) == 2)
                            {

                                string[] arrayBuffer = context.Request["data"].Split(new Char[] { ',' });
                                byte[] bdata=new byte[arrayBuffer.Length];
                                for (int i = 0; i < arrayBuffer.Length; i++)
                                {
                                    bdata[i] = Convert.ToByte(arrayBuffer[i]);
                                }
                                    patientId = context.Request.Form["PatientID"];
                                if (string.IsNullOrEmpty(patientId))
                                {
                                    throw new Exception("Unable to save patient webcam image.");

                                }
                                strJSONData = UploadPatientWebCamImage(MDVUtility.ToInt64(patientId), bdata);

                            }
                            else
                            {
                                patientId = context.Request["PatientID"];
                                strJSONData = UploadPatientImage(MDVUtility.ToInt64(patientId), null);
                            }
                        }
                        catch (Exception ex)
                        {
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);

                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);


                    }
                    break;
                case "REMOVE_IMAGE":
                    {

                        string patientId = context.Request["PatientID"];
                        string strJSONData = RemovePatientImage(MDVUtility.ToInt64(patientId));
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);


                    }
                    break;
            }
        }
        #endregion
    }
}