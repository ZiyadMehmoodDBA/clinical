using System;
using System.ComponentModel;
using System.IO;
using MDVision.Common.Shared;

namespace MDVision.Business.BCommon
{

    public class FileServer
    {

    
        #region Variable
        
        //private static FileServer _instance;

        //public static FileServer Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            try
        //            {
        //                System.Threading.Monitor.Enter(typeof(FileServer));
        //                _instance = new FileServer();
        //            }
        //            finally
        //            {
        //                System.Threading.Monitor.Exit(typeof(FileServer));
        //            }

        //        }
        //        return _instance;
        //    }
        //}
        #endregion
       
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminEDI"/> class.
        /// </summary>
        public FileServer()
        {
            //SharedVariable SharedObj
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this.SharedObj = SharedObj;
            //Add your own initialization code after the InitializeComponent() call

        }

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region "Functions"


        private string GetFTPHostName()
        {
            if (MDVApplication.DocsHostName != null)
            {
                if (MDVApplication.DocsHostName != "")
                    return MDVApplication.DocsHostName;
                else
                    return "127.0.0.1";
            }
            else
            {
                return "127.0.0.1";
            }
        }

        private string GetFTPAlias()
        {
            if (MDVApplication.DocsAlias != null)
            {
                if (MDVApplication.DocsAlias != null)
                    return MDVApplication.DocsAlias;
                else
                    return "ehrdocs";
            }
            else
            {
                return "ehrdocs";
            }
        }
        public string FtpHostWithAlias()
        {
            return GetFTPHostName() + "\\" + GetFTPAlias() + "\\";
        }

        public string FullFtpPath()
        {
            return "\\\\" + FtpHostWithAlias();
        }


        //public string FullFtpPathWithEntityId()
        //{
              //MDVSession.Current.EntityId  not available here
        //    return "\\\\" + FtpHostWithAlias() + MDVSession.Current.EntityId + "\\" + DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString();
        //}
        //public bool DeleteFilesFromServer(string DestinationPath)
        //{
        //    using (NetworkSecurity.Instance.Login())
        //    {
        //        Directory.Delete(DestinationPath, true);
        //        return true;
        //    }

        //}
        public bool DeleteFileFromServer(string PathFileName)
        {
            using (NetworkSecurity.Instance.Login())
            {
                if (File.Exists(PathFileName))
                {
                    File.Delete(PathFileName);
                }
              
                return true;
            }

        }
        public bool FileWrite(string path, string FileName, string sData)
        {
           //fixme NetworkSecurity.Instance.SharedObj = SharedObj;

            using (NetworkSecurity.Instance.Login())
            {
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);

                System.IO.FileStream fs = new System.IO.FileStream(path + "\\" + FileName, System.IO.FileMode.Create);
                fs.Close();
                //Append Text into File
                StreamWriter sr = File.CreateText(path + "\\" + FileName);
                sr.Write(sData);
                sr.Close();
                return true;
            }
        }
        public bool FileRead(string path, string FileName,ref string FileString)
        {
           //fixme NetworkSecurity.Instance.SharedObj = SharedObj;

            using (NetworkSecurity.Instance.Login())
            {
                if (File.Exists(path + "\\" + FileName) == false)
                    throw new Exception("File : " + FileName + ", Does Not exists");

                System.IO.StreamReader sr = new System.IO.StreamReader(path + "\\" + FileName);
                         
                 FileString = sr.ReadToEnd();
                sr.Close();

                return true;
            }
        }

        public bool CreateDirectory(string Path)
        {
          // fixme NetworkSecurity.Instance.SharedObj = SharedObj;
            using (NetworkSecurity.Instance.Login())
            {
                if (Directory.Exists(Path) == false)
                    Directory.CreateDirectory(Path);
                return true ;
            }
        }

        public bool FileExists(string FilePath)
        {
          //  NetworkSecurity.Instance.SharedObj = SharedObj;
            using (NetworkSecurity.Instance.Login())
            {
                return File.Exists(FilePath);
            }
        }
        //public bool DeleteFilesFromServer(string[] DestinationPaths)
        //{
        //    using (NetworkSecurity.Instance.Login())
        //    {
        //        foreach (string str in DestinationPaths)
        //        {
        //            if (File.Exists(str))
        //            {
        //                File.Delete(str);
        //            }
        //        }
        //        return true;
        //    }
            
        //}

        //public bool WriteFileToServer(string FilePath, string FileName, byte[] FileBytes)
        //{

        //    return FileBytesWrite(FileBytes, FilePath, FileName);
        //}

        //private bool FileBytesWrite(byte[] source, string DestinationPath, string FileName)
        //{
        //    using (NetworkSecurity.Instance.Login())
        //    {
        //        Directory.CreateDirectory(DestinationPath);
        //        File.WriteAllBytes(DestinationPath + "\\" + FileName, source);
        //        return true;
        //    }
           
        //}

        //private bool FileRead(ref byte[] source, string DestinationPath, string FileName)
        //{
        //    using (NetworkSecurity.Instance.Login())
        //    {
        //        FileInfo fInfo = new FileInfo(DestinationPath + "\\" + FileName);
        //        if (fInfo.Exists)
        //        {
        //            source = System.IO.File.ReadAllBytes(DestinationPath + "\\" + FileName);
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

      


        //public bool WriteFileFromServerToServer(string RelativeSourceFilePath, string RelativeDestinationFilePath)
        //{
        //    string strCompletePath = GetFTPHostName() + "\\" + GetFTPAlias() + "\\" + RelativeSourceFilePath;
        //    strCompletePath = "\\\\" + strCompletePath.Replace("\\\\", "\\");
        //    string fileName = Path.GetFileName(strCompletePath);
        //    string filePath = Path.GetDirectoryName(strCompletePath);
        //    byte[] FileBytes = null;
        //    bool rtn = FileRead(ref FileBytes, filePath, fileName);
        //    if (rtn)
        //    {
        //        string strCompleteDestinationPath = GetFTPHostName() + "\\" + GetFTPAlias() + "\\" + RelativeDestinationFilePath;
        //        strCompleteDestinationPath = "\\\\" + strCompleteDestinationPath.Replace("\\\\", "\\");
        //        string strDestinationfileName = Path.GetFileName(strCompleteDestinationPath);
        //        string strDestinationfilePath = Path.GetDirectoryName(strCompleteDestinationPath);
        //        return WriteFileToServer(strDestinationfilePath, strDestinationfileName, FileBytes);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public bool WriteFileFromServerToServer(string[] RelativeSourceFilePath, string RelativeDestinationFolderPath)
        //{
        //    using (NetworkSecurity.Instance.Login())
        //    {
        //        string strServerPath = GetFTPHostName() + "\\" + GetFTPAlias();
        //        foreach (string RelativefilePath in RelativeSourceFilePath)
        //        {
        //            string strCompletePath = strServerPath + "\\" + RelativefilePath;
        //            strCompletePath = "\\\\" + strCompletePath.Replace("\\\\", "\\");
        //            if (File.Exists(strCompletePath) == false)
        //            {
        //                string relativePath = strCompletePath.Replace("\\\\" + strServerPath, "");
        //                throw new Exception("File " + relativePath + " does not exits");
        //            }
        //        }
        //        foreach (string RelativefilePathToCopy in RelativeSourceFilePath)
        //        {
        //            string strCompletePath = strServerPath + "\\" + RelativefilePathToCopy;
        //            strCompletePath = "\\\\" + strCompletePath.Replace("\\\\", "\\");
        //            string fileName = Path.GetFileName(strCompletePath);
        //            string strDestinationPath = strServerPath + "\\" + RelativeDestinationFolderPath + "\\" + fileName;
        //            strDestinationPath = "\\\\" + strDestinationPath.Replace("\\\\", "\\");
        //            File.Copy(strCompletePath, strDestinationPath, true);
        //        }

        //        return true;
        //    }
        //}

        //public bool ReadFileFromServer(string FilePath, string FileName, ref byte[] FileBytes)
        //{
        //    return FileRead(ref FileBytes, FilePath, FileName);
        //}


        //public string UpLoadLargFileToServer(byte[] fileBytes)
        //{
            
        //    string path = (Path.GetTempPath() + "\\tempExportedFile.zip");
        //    if (fileBytes !=null)
        //    {
        //        File.WriteAllBytes(path, fileBytes);
        //    }
        //    return path;
        //}

        //public string SaveLargFileToServer(string direcotryPath, string FileName, byte[] fileBytes, bool deleteExisting)
        //{
        //    using (NetworkSecurity.Instance.Login())
        //    {
        //        Directory.CreateDirectory(direcotryPath);
        //        if (deleteExisting && File.Exists(direcotryPath + "\\" + FileName))
        //        {
        //            File.Delete(direcotryPath + "\\" + FileName);
        //        }
        //        File.WriteAllBytes(direcotryPath + "\\" + FileName, fileBytes);
        //        return Boolean.TrueString;
        //    }
        //}

        //public object[] ReadLargeFileFromServer(string FilePath, string FileName, ref byte[] FileBytes, ref bool ismore)
        //{
        //    object[] objAry = new object[3];
        //    ismore = false;
        //    int startIndex = 0;
        //    using (NetworkSecurity.Instance.Login())
        //    {
        //        FileInfo fInfo = new FileInfo(FilePath + "\\" + FileName);
        //        if (!fInfo.Exists  && FileName.EndsWith(".zip"))
        //        {
        //            FileName = FileName.Replace(".zip", "xmls.zip");
        //            fInfo = new FileInfo(FilePath + "\\" + FileName);
        //        }
        //        if (fInfo.Exists)
        //        {
        //            long numBytes = fInfo.Length;
        //            FileStream fStream = new FileStream(FilePath + "\\" + FileName, FileMode.Open, FileAccess.Read);
        //            BinaryReader br = new BinaryReader(fStream);
        //            if ((numBytes > 20000000))
        //            {
        //                FileBytes = br.ReadBytes(20000000);
        //                ismore = true;
        //                startIndex = (startIndex + 20000000);
        //            }
        //            else
        //            {
        //                FileBytes = br.ReadBytes (Convert.ToInt32( numBytes));
        //                ismore = false;
        //            }
        //            br.Close();
        //            fStream.Close();
        //            objAry[0] = FileBytes;
        //            objAry[1] = ismore;
        //            objAry[2] = startIndex;
        //            return objAry;
        //        }
        //    }
        //    return null;
        //}

        //public object[] GetMoreBytes(ref byte[] fileBytes, ref bool ismore, string FilePath, int startIndex)
        //{
        //    using (NetworkSecurity.Instance.Login())
        //    {
        //        object[] objAry = new object[3];
        //        FileInfo fInfo = new FileInfo(FilePath);
        //        if (fInfo.Exists)
        //        {
        //            long numBytes = fInfo.Length;
        //            FileStream fStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        //            BinaryReader br = new BinaryReader(fStream);
        //            long remainingcount = (numBytes - startIndex);
        //            if (remainingcount > 20000000)
        //            {
        //                br.BaseStream.Seek(startIndex, SeekOrigin.Begin);
        //                fileBytes = br.ReadBytes(20000000);
        //                br.Close();
        //                fStream.Close();
        //                ismore = true;
        //                startIndex = startIndex + 20000000;
        //            }
        //            else
        //            {
        //                br.BaseStream.Seek(startIndex, SeekOrigin.Begin);
        //                fileBytes = br.ReadBytes(Convert.ToInt32(remainingcount));
        //                br.Close();
        //                fStream.Close();
        //                ismore = false;
        //                startIndex = 0;
        //            }
        //            objAry[0] = fileBytes;
        //            objAry[1] = ismore;
        //            objAry[2] = startIndex;
        //            return objAry;
        //        }
        //    }
        //    return null;
        //}
        //public bool FilePrint(PrintDocument pd)
        //{
        //    NetworkSecurity.Instance.SharedObj = SharedObj;
        //    using (NetworkSecurity.Instance.Login())
        //    {
        //        pd.Print();
        //        //FileInfo fInfo = new FileInfo(DestinationPath + "\\" + FileName);
        //        //if (fInfo.Exists)
        //        //{
        //        //    source = System.IO.File.ReadAllBytes(DestinationPath + "\\" + FileName);
        //        //}
        //        //else
        //        //{
        //        //    return false;
        //        //}
        //    }
        //    return true;
        //}
        //public bool ReadSignatureFile(string SigProvSeqNum, ref byte[] FileBytes, string DocServerPath)
        //{
        //    if (SigProvSeqNum !=null && SigProvSeqNum != "")
        //    {               
                    
        //        CommonDataAccess commonDA = new CommonDataAccess(SharedObj);
        //        string strQuery = ("SELECT SIGNATURE_PATH, SIGNATURE_FILE_NAME FROM V_SEC_PROVIDER_SPECIALITY_EMR WHERE SEQ_NUM = " + SigProvSeqNum);
        //        DataSet dsLinkProv = DataAccess.OracleHelper.ExecuteDataset(commonDA.getConnection, CommandType.Text, strQuery, false);
        //        // Dim da As New OracleDataAdapter(strQuery, commonDA.getConnection)
        //        // da.Fill(dsLinkProv)
        //        // Dim dsLinkProv As DataSet = commonDA.doSelect("EMR_SECURITY_PROVIDER", New Object() {SigProvSeqNum})
        //        if (dsLinkProv !=null && dsLinkProv.Tables[0].Rows.Count > 0);
        //            if (dsLinkProv.Tables[0].Rows[0].Item["SIGNATURE_PATH"])
        //            {
        //                IsNot;
        //                DBNull.Value;
        //                string FilePath = (DocServerPath + ("\\" + dsLinkProv.Tables[0].Rows[0]["SIGNATURE_PATH"].ToString));
        //                return FileRead(FileBytes, FilePath, dsLinkProv.Tables[0].Rows[0]["SIGNATURE_FILE_NAME"].ToString);
        //            }
        //        }
        //    }
        //}
     

        //public byte[] GetPatientImage(Decimal PatientSeqNum)
        //{
        //    CommonDataAccess commonDA = new CommonDataAccess(SharedObj);
        //    DataSet dsPatientImage = commonDA.doSelect("PATIENT_IMAGE", new object[] {
        //             PatientSeqNum});
        //    if (((dsPatientImage == null)
        //                || ((dsPatientImage.Tables[0].Rows.Count == 0)
        //                || (dsPatientImage.Tables[0].Rows[0].Item["IMAGE_PATH"].ToString == ""))))
        //    {
        //        return null;
        //    }
        //    string strRelativePath = dsPatientImage.Tables[0].Rows[0].Item["IMAGE_PATH"].ToString;
        //    string strCompletePath = (GetEDMHost() + ("\\"
        //                + (GetEDMAlias() + ("\\" + strRelativePath))));
        //    strCompletePath = ("\\\\" + strCompletePath.Replace("\\\\", "\\"));
        //    string fileName = Path.GetFileName(strCompletePath);
        //    string filePath = Path.GetDirectoryName(strCompletePath);
        //    byte[] FileBytes = null;
        //    if ((FileRead(FileBytes, filePath, fileName) == true))
        //    {
        //        return FileBytes;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion
    }
}
