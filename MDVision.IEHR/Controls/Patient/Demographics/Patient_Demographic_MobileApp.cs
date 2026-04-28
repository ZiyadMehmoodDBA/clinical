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
using System.Text;
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
using MDVision.Model.Patient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MDVision.Model.Clinical.Medical;
using MDVision.Model.Lookups;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controls.Patient.Demographics
{
    public class Patient_Demographic_MobileApp
    {
        public string getPatientProfileImage(string lfileName)
        {

            string base64String="";
            
            
            string ServerPathForLoadFile = System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesPath"];

            if (System.IO.Directory.Exists(ServerPathForLoadFile))
            {
                if (!lfileName.Equals(""))
                {

                    //MDVSession.Current.ImageID = lfileName + "|" + lfileNameThumbnail;


                    //byte[] imageBytes = System.IO.File.ReadAllBytes(System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesDetails"] + "\\" + lfileName);


                    string imgPath = System.IO.Path.Combine(ServerPathForLoadFile, lfileName);

                    if (System.IO.File.Exists(imgPath))
                    {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                        //byte[] imageBytes = Convert.FromBase64String(lfileName);

                         base64String = Convert.ToBase64String(imageBytes);

                        //string imgBase64String = GetBase64StringForImage(ServerPathForLoadFile);
                        // imageBase64 = "data:" + dr[dsPatient.Patients.ImageTypeColumn.ColumnName] + ";base64," + base64String;

                    }
                }
            }
            return base64String;
        }

    }
}