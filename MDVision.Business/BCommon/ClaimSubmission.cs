using System;
using System.Collections.Generic;
using System.Linq;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Claim;
using MDVision.DataAccess.DCommon;
using MDVision.DataAccess.DAL.Visits;
using System.IO;
using System.Data;
using iTextSharp.text;
using MDVision.Business.BCommon.FTP;
using System.Collections;
using EDIParser.Professional;
using EDIParser;
using WinSCP;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.Business.BLL;
using Newtonsoft.Json.Linq;
using MDVision.Model.Billing.ERA;

namespace MDVision.Business.BCommon
{
    public class BLLClaimSubmissionModel
    {
        public bool Status = false;
        public bool isDBException = false;
        public string Filename = string.Empty;
        public string Message = string.Empty;
        public int ClaimsCount = 0;
        public Dictionary<long, string> EroredClaims = new Dictionary<long, string>();
    }

    public class ClaimSubmission
    {

        public Int64 batchId { get; set; }
        public String ControlNUmber { get; set; }
        public String edi837String { get; set; }
        public DS837Batch dsBatch { get; set; }
        public DSEDI dsEDI { get; set; }
        public string fileName_Message { get; set; }
        public string fileName { get; set; }
        public bool Isreadytosubmit { get; set; }
        public List<long> VisitIds { get; set; }
        public long ClearingHouseId { get; set; }
        public bool MarkSubmitted { get; set; }
        public string ftp { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string ftpHostKey { get; set; }
        public Int32 ftpPortNo { get; set; }
        public string FTPUploadFolder { get; set; }
        public string ProfessionalClaimExtension { get; set; }
        public string WinscpPatch { get; set; }
        public SharedVariable SharedVariable { get; set; }

        public ClaimSubmission(SharedVariable SharedVariable, string WinscpPatch)
        {
            this.SharedVariable = SharedVariable;
            this.WinscpPatch = WinscpPatch;
        }
        public ClaimSubmission(SharedVariable SharedVariable, List<long> VisitIds, long ClearingHouseId, bool MarkSubmitted, string WinscpPatch, DSEDI dsEDI)
        {
            this.batchId = 0;
            this.ControlNUmber = "";
            this.edi837String = "";
            this.dsBatch = new DS837Batch();
            this.SharedVariable = SharedVariable;
            this.fileName_Message = "";
            this.fileName = "";
            this.Isreadytosubmit = false;
            this.VisitIds = VisitIds;
            this.ClearingHouseId = ClearingHouseId;
            this.MarkSubmitted = MarkSubmitted;
            this.WinscpPatch = WinscpPatch;
            this.ftp = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTPColumn]);
            this.userName = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserNameColumn]);
            this.userPassword = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.UserPasswordColumn]);
            this.ftpPortNo = MDVUtility.ToInt32(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_PORTNOColumn]);
            this.ftpHostKey = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.FTP_HOSTKEYColumn]);
            this.FTPUploadFolder = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.IN_UPLOADEDColumn]);
            this.ProfessionalClaimExtension = MDVUtility.ToStr(dsEDI.ClearingHouse.Rows[0][dsEDI.ClearingHouse.ProfessionalClaimExtensionColumn]);
        }

        public BLLClaimSubmissionModel SubmitClaims()
        {
            BLLClaimSubmissionModel model = new BLLClaimSubmissionModel();
            model.ClaimsCount = VisitIds.Count;
            model.Status = false;

            DSHCFA ds = new DSHCFA();

            try
            {
                //Create Batch
                BLObject<DS837Batch> ds837Batch = Create837BatchClaim(VisitIds, ClearingHouseId, true, MarkSubmitted, true);
                if (ds837Batch.Data != null)
                {
                    // try catch to roll back all process if any exception occur.
                    try
                    {
                        batchId = Convert.ToInt64(ds837Batch.Data._837Batch.Rows[0][ds837Batch.Data._837Batch._837BatchIdColumn]);
                        // Get Control Number
                        ControlNUmber = Convert.ToString(ds837Batch.Data._837Batch.Rows[0][ds837Batch.Data._837Batch.BatchControlNoColumn]);
                        if (ControlNUmber != "")
                        {
                            ds = new DAL837(SharedVariable).Load837Header(SharedVariable, MDVUtility.ToInt64(ds837Batch.Data._837Batch.Rows[0][ds837Batch.Data._837Batch.ClearingHouseIdColumn]), SharedVariable.EntityId);

                            if (ds._837Header.Rows.Count > 0)
                            {
                                // Set Control Number
                                ds._837Header.Rows[0][ds._837Header.ISA13Column] = ControlNUmber;
                                ds._837Header.Rows[0][ds._837Header.GS06Column] = ControlNUmber;
                                ds._837Header.Rows[0][ds._837Header.ST02Column] = ControlNUmber;
                                ds._837Header.Rows[0][ds._837Header.BHT03Column] = ControlNUmber;
                                ds.AcceptChanges();

                                ds.Merge(new DAL837(SharedVariable).LoadClaimSubmission(SharedVariable, string.Join(",", VisitIds.Select(n => n.ToString()).ToArray()), 0));
                                ds.Merge(new DAL837(SharedVariable).LoadClaimICDS(SharedVariable, string.Join(",", VisitIds.Select(n => n.ToString()).ToArray())));

                                EDI837Parser parser = new EDI837Parser(ref ds);
                                edi837String = parser.Get837();
                                model.EroredClaims = parser.EroredClaims;

                                if (VisitIds.Count > model.EroredClaims.Count)
                                {
                                    dsBatch = ds837Batch.Data;
                                    dsBatch._837Batch.Rows[0][dsBatch._837Batch.EDI837StringColumn] = edi837String;
                                    dsBatch._837Batch.Rows[0][dsBatch._837Batch.BatchStatusColumn] = "Submitted";


                                    // Upload 837 File on FTP server.
                                    if (MarkSubmitted == false)
                                    {
                                        fileName = UploadFileToFTP(edi837String, ControlNUmber, ftp, userName, userPassword, ftpPortNo, FTPUploadFolder, this.ProfessionalClaimExtension, ftpHostKey);
                                        fileName_Message = fileName + " is uploaded successfully";
                                    }
                                    else
                                    {
                                        fileName_Message = "Selected claims are marked as Transmitted successfully.";
                                    }

                                    // Set SendStatus Submitted in current Batch
                                    dsBatch._837Batch.Rows[0][dsBatch._837Batch.SendStatusColumn] = "S";
                                    dsBatch._837Batch.Rows[0][dsBatch._837Batch.IsCompletedColumn] = true;

                                    dsBatch._837Batch.Rows[0][dsBatch._837Batch.CommentsColumn] = fileName_Message;
                                    // Updated Batch Status
                                    dsBatch = new DAL837(SharedVariable).Update837Batch(SharedVariable, dsBatch);

                                    // delete from Error Table.
                                    new DALClaim(SharedVariable).DeleteClaimSubmissionError(SharedVariable, "", string.Join(",", VisitIds.ToArray()));

                                    //update batch with Errored Claims
                                    if (model.EroredClaims.Count > 0)
                                        SaveClaimSubmissionErrors(model.EroredClaims);

                                    // Update Patient Visit and Charge Status
                                    string objUpdate = new DALVisits(SharedVariable).UpdatePatientVisitsAndChargesStatusUpdate(SharedVariable, batchId, 2, SharedVariable.EntityId, SharedVariable.UserName);
                                    if (!string.IsNullOrEmpty(objUpdate))
                                        throw (new Exception(objUpdate));

                                    // all process is completed
                                    model.Message = fileName_Message;
                                    model.Status = true;
                                    model.ClaimsCount = VisitIds.Count - model.EroredClaims.Count;
                                }
                                else
                                {
                                    if (model.EroredClaims.Count > 0)
                                        SaveClaimSubmissionErrors(model.EroredClaims);
                                }
                            }
                            else
                                throw new Exception("Please setup the EDI receiver ID and submitter ID for selected clearing house.");
                        }
                        else
                            throw new Exception("Batch Control Number is not created");
                    }
                    catch (Exception ex)
                    {
                        // roll back all process.
                        new DAL837(SharedVariable).DeleteEDI837Batch(SharedVariable, batchId);

                        if (!string.IsNullOrEmpty(fileName) && ftp != "" && userName != "" && userPassword != "" && FTPUploadFolder != "")
                            DeleteFileFromFTP(fileName, ftp, userName, userPassword, ftpPortNo, FTPUploadFolder, ftpHostKey);

                        model.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                        model.Status = false;
                        model.isDBException = true;
                        MDVLogger.BLLErrorLog("SubmitClaim", ex);
                        //foreach (var item in VisitIds)
                        //    model.EroredClaims.Add(item, ex.Message);

                        ////update batch with Errored Claims
                        //if (model.EroredClaims.Count > 0)
                        //    SaveClaimSubmissionErrors(model.EroredClaims);
                    }
                }
                else
                {
                    throw new Exception(ds837Batch.Message);
                }
            }
            catch (Exception ex)
            {
                //foreach (var item in VisitIds)
                //    model.EroredClaims.Add(item, ex.Message);

                if (ex.ToString().Contains("Selected claims are already submitted"))
                {
                    model.isDBException = false;
                }
                else
                {
                    model.isDBException = true;
                }

                model.Message = MDVCustomException.HumanReadableMessage(ex.Message);
                model.Status = false;
                MDVLogger.BLLErrorLog("SubmitClaim", ex);
            }

            return model;
        }

        public void SaveClaimSubmissionErrors(Dictionary<long, string> EroredClaims)
        {

            string Errored_VisitIDs = string.Join(",", EroredClaims.Keys.Select(n => n.ToString()).ToArray());
            new DAL837(SharedVariable).Delete837BatchClaimByVisitIds(SharedVariable, Errored_VisitIDs, batchId);

            // insert into Error Table
            DSVisits dsVisits = new DSVisits();
            foreach (var item in EroredClaims)
            {
                DSVisits.ClaimSubmissionErrorRow drError = dsVisits.ClaimSubmissionError.NewClaimSubmissionErrorRow();
                drError.VisitId = item.Key;
                drError.ClaimError = item.Value;
                drError.CreateBy = SharedVariable.UserName;
                drError.CreatedOn = DateTime.Now;
                dsVisits.ClaimSubmissionError.Rows.Add(drError);
            }

            dsVisits = new DALClaim(SharedVariable).InsertClaimSubmissionError(SharedVariable, dsVisits);

        }

        public BLObject<DS837Batch> Create837BatchClaim(List<long> VisitIds, long ClearingHouseId, bool SubmitType, bool MarkSubmitted, bool IsAlreadySubmitted)
        {
            if (IsAlreadySubmitted)
            {
                //Check is selected visits already have batch with submit status if they have do not create batch again.
                string VisitIDs = string.Join(",", VisitIds.Select(n => n.ToString()).ToArray());
                DS837Batch dsbatchExist = new DS837Batch();

                // Return Visits that already submitted with ClaimSubmitstatus 2.
                dsbatchExist = new DAL837(SharedVariable).Is837BatchExists(SharedVariable, VisitIDs);

                foreach (DS837Batch._837BatchClaimRow row in dsbatchExist._837BatchClaim.Rows)
                    VisitIds.Remove(row.VisitId);

                if (VisitIds.Count <= 0)
                    throw new Exception("Selected claims are already submitted.");
            }

            DS837Batch dsBatch = new DS837Batch();
            try
            {
                DS837Batch._837BatchRow drBatch = dsBatch._837Batch.New_837BatchRow();
                drBatch.ClearingHouseId = ClearingHouseId;
                drBatch.EntityId = MDVUtility.ToInt64(SharedVariable.EntityId);
                drBatch.IsActive = true;
                drBatch.ModifiedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                drBatch.modifiedOn = DateTime.Now;
                drBatch.CreatedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                drBatch.CreatedOn = DateTime.Now;
                drBatch.SubmitType = SubmitType;
                drBatch.SendStatus = "I";
                drBatch.IsCompleted = false;
                drBatch.MarkSubmitted = MarkSubmitted;
                drBatch.VisitIds = string.Join(",", VisitIds.Select(n => n.ToString()).ToArray());
                drBatch.ClientId = MDVUtility.ToInt64(SharedVariable.ClientId);
                dsBatch._837Batch.Add_837BatchRow(drBatch);

                dsBatch = new DAL837(SharedVariable).InsertEDI837Batch(SharedVariable, dsBatch);

                return new BLObject<DS837Batch>(dsBatch);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #region " Variables "


        private clsFTP objFTP;
        public SFTP.SFTP.clsSFTP objsftp;
        List<string> FileNamesAllsFTP = new List<string>();
        private string[] FileNamesAll;
        private string[] FileNamesToDownLoad;

        #endregion

        #region "837 Upload FTP file"

        public BCommon.FTP.clsFTP objftp;
        public string UploadFileToFTP(string FinalStr837, string BtachCtrlNo, string FTPClearingHouse, string FTPUserName, string FTPPassword, Int32 FTP_PortNo, string DestFTPFolder, string sBatchFileExtension, string ftpHostKey, bool isFromStatement = false)
        {
            if (FinalStr837.Trim() == "")
            {
                throw new Exception("Enter batch file data.");
            }


            string sFileName = null;
            string DestFile = "";

            sFileName = BtachCtrlNo;
            sFileName = sFileName + sBatchFileExtension;


            try
            {
                FileServer Obj = new FileServer();
                // for patient statement,last slash create problem while uploading
                if (!isFromStatement)
                {
                    DestFTPFolder = "\\" + DestFTPFolder + "\\";
                }
                else
                {
                    DestFTPFolder = "\\" + DestFTPFolder;
                }
                string DestFileFolder = "\\\\" + Obj.FtpHostWithAlias() + MDVApplication.CustomerRegCode + "\\EDI" + "\\" + SharedVariable.ClientId + DestFTPFolder;
                DestFile = DestFileFolder + sFileName;

                if (DestFileFolder != "" && FinalStr837 != "")
                {

                    // If HostKey Does not exist Its an FTP, Goes with FTP Upload
                    if (string.IsNullOrEmpty(ftpHostKey))
                    {

                        #region FTP

                        if (Obj.FileWrite(DestFileFolder, sFileName, FinalStr837) == false)
                        {

                            throw new Exception("file doesn't create.");
                        }

                        if (string.IsNullOrEmpty(DestFile))
                        {

                            throw new Exception("Source file not selected.");
                        }

                        if (Obj.FileExists(DestFile) == false)
                        {
                            throw new Exception("Source file '" + DestFile + "' not found");
                        }

                        try
                        {
                            bool check = false;
                            objftp = new BCommon.FTP.clsFTP(FTPClearingHouse, DestFTPFolder, FTPUserName, FTPPassword, FTP_PortNo);

                            if (objftp.Login() == true)
                            {

                                objftp.CloseConnection();

                                if (objftp.MessageString.Contains("550 CWD failed"))
                                {
                                    throw new Exception(objftp.MessageString);
                                }
                            }

                            if (objftp.MessageString == "" && !string.IsNullOrEmpty(DestFile))
                            {
                                //Dim filename As String = Me.GetFileNameByPath(DestFile)
                                check = UploadFTP(DestFileFolder, DestFTPFolder, DestFile);
                                if (check == true)
                                {
                                    objftp.CloseConnection();
                                }
                                else
                                {
                                    throw new Exception("Fail to upload file to remote server.");
                                }
                            }
                            else
                            {
                                throw new Exception("Fail to connect to remote server. " + objftp.MessageString);
                            }
                        }
                        catch (Exception ex)
                        {
                            objftp.CloseConnection();
                            throw ex;
                        }

                        #endregion
                    }
                    else
                    {
                        //string[] filePaths = Directory.GetFiles(DestFileFolder);
                        //foreach (string filePath in filePaths)
                        //    File.Delete(filePath);
                        DateTime date_ = DateTime.Now;
                        if (Obj.FileWrite(DestFileFolder, sFileName, FinalStr837) == false)
                        {
                            throw new Exception("file doesn't create.");
                        }

                        bool IsResponse = false;
                        IsResponse = DownLoadAllFiles_SFTP(FTPClearingHouse, FTPUserName, FTPPassword, FTP_PortNo, ftpHostKey, DestFileFolder, DestFTPFolder.Replace(@"\", "/"), sFileName, date_, isFromStatement);

                        if (IsResponse)
                        {
                            // delete this file from FTP server.
                            if (File.Exists(DestFileFolder + sFileName))
                                File.Delete(DestFileFolder + sFileName);
                        }

                    }

                }
                else
                {
                    throw new Exception("Path is not valid or file doesn't contain data.");
                }

                string FileName = "";
                int ind = 0;
                ind = DestFile.LastIndexOf("\\");
                ind = ind + 1;
                FileName = DestFile.Substring(ind, DestFile.Length - ind);

                return FileName;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("UploadFileToFTP", ex);
                throw ex;
            }
        }

        public string DeleteFileFromFTP(string fileName, string FTPClearingHouse, string FTPUserName, string FTPPassword, Int32 FTP_PortNo, string DestFTPFolder, string ftpHostKey)
        {

            string sFileName = null;
            string DestFile = "";
            sFileName = fileName;
            try
            {
                FileServer Obj = new FileServer();
                DestFTPFolder = "\\" + DestFTPFolder + "\\";
                string DestFileFolder = "\\\\" + Obj.FtpHostWithAlias() + MDVApplication.CustomerRegCode + "\\EDI" + "\\" + SharedVariable.ClientId + DestFTPFolder;
                DestFile = DestFileFolder + sFileName;

                if (!string.IsNullOrEmpty(DestFileFolder))
                {
                    if (string.IsNullOrEmpty(ftpHostKey))
                    {

                        #region FTP
                        try
                        {
                            bool check = false;
                            objftp = new BCommon.FTP.clsFTP(FTPClearingHouse, DestFTPFolder, FTPUserName, FTPPassword, FTP_PortNo);

                            if (objftp.Login() == true)
                            {

                                objftp.CloseConnection();

                                if (objftp.MessageString.Contains("550 CWD failed"))
                                {
                                    throw new Exception(objftp.MessageString);
                                }
                            }

                            if (objftp.MessageString == "" && !string.IsNullOrEmpty(DestFile))
                            {
                                check = DeleteFTP(DestFileFolder, DestFTPFolder, DestFile);
                                if (check == true)
                                {
                                    objftp.CloseConnection();
                                }
                                else
                                {
                                    throw new Exception("Fail to delete file to remote server.");
                                }
                            }
                            else
                            {
                                throw new Exception("Fail to connect to remote server. " + objftp.MessageString);
                            }
                        }
                        catch (Exception ex)
                        {
                            objftp.CloseConnection();
                            throw ex;
                        }

                        #endregion
                    }
                    else
                    {
                        DeleteFile_SFTP(FTPClearingHouse, FTPUserName, FTPPassword, FTP_PortNo, ftpHostKey, DestFTPFolder.Replace(@"\", "/"), sFileName);
                    }

                }
                else
                {
                    throw new Exception("Path is not valid or file doesn't contain data.");
                }

                string FileName = "";
                int ind = 0;
                ind = DestFile.LastIndexOf("\\");
                ind = ind + 1;
                FileName = DestFile.Substring(ind, DestFile.Length - ind);

                return FileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool DeleteFTP(string FolderPath, string FTPFolderPath, string fname)
        {
            try
            {
                bool check = false;


                check = objftp.Login();
                if (check == true)
                {

                    check = objftp.ChangeDirectory(FTPFolderPath);

                    if (check == true)
                    {
                        //=================================================================================
                        objftp.DeleteFile(fname);
                        //=================================================================================
                        objftp.CloseConnection();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private bool UploadFTP(string FolderPath, string FTPFolderPath, string fname)
        {
            try
            {
                bool check = false;


                check = objftp.Login();
                if (check == true)
                {

                    check = objftp.ChangeDirectory(FTPFolderPath);

                    if (check == true)
                    {
                        //=================================================================================
                        objftp.UploadFile(fname);
                        //=================================================================================
                        objftp.CloseConnection();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool DownLoadAllFiles_SFTP(string ftp, string userName, string userPassword, Int64 ftpPortNo, string ftpHostKey, string DestFileFolder, string destFTPFolder, string sfileName, DateTime date_, bool isFromStatement = false)
        {
            SessionOptions _SessionOptions = new SessionOptions();
            bool IsReposense = false;
            try
            {
                var with = _SessionOptions;
                with.Protocol = Protocol.Sftp;
                with.HostName = ftp;
                with.UserName = userName;
                with.Password = userPassword;
                try
                {
                    with.SshHostKeyFingerprint = ftpHostKey;
                }
                catch (Exception)
                {
                    throw new Exception("SSH host key \"" + ftpHostKey + "\" does not match pattern.");
                }

                with.PortNumber = Convert.ToInt32(ftpPortNo);
                var sessionOptions = _SessionOptions;
                if (sessionOptions.SshHostKeyFingerprint != null)
                {
                    using (Session session = new Session())
                    {
                        // The package includes the assembly itself (winscpnet.dll) and a required dependency, WinSCP executable winscp.exe.
                        // The binaries interact with each other and must be kept in the same folder for the assembly to work.

                        //Note that your runtime or development environment may copy the assembly into an another location. In that case you need to copy winscp.exe into that location too.
                        //E.g. If you reference WinSCP assembly from your project in Microsoft Visual Studio, it copies the assembly during build into the project Output path (e.g. <your_project_path>/obj/Debug). Similar case is when you install the assembly into Global Assembly Cache (GAC).
                        //You may want to add winscp.exe to your Visual Studio project, to have it copied to the Output path automatically (by setting file property Copy to Output Directory to Copy if newer). The Build Action should be automatically set to Content, what means that the file will be included when deploying your application (e.g. an ASP.NET web application or Azure WebJob application).

                        // Quick Reference : http://winscp.net/eng/docs/library_install

                        session.ExecutablePath = this.WinscpPatch;
                        session.Open(sessionOptions);
                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Binary;

                        TransferOperationResult transferResult;
                        try
                        {
                            FileServer Obj = new FileServer();

                            if (Obj.CreateDirectory(DestFileFolder) == false)
                                throw new Exception("File/Directory does not exist.");

                            // Check if download or upload Case
                            if (string.IsNullOrEmpty(sfileName)) // Download Case
                            {
                                int i = 0;
                                string fileName = null;
                                if (FileNamesToDownLoad != null)
                                {

                                    // Get each downloaded file from SFTP and place in Temporary Drive
                                    for (i = 0; i <= FileNamesToDownLoad.Length - 1; i++)
                                    {
                                        fileName = FileNamesToDownLoad[i];
                                        // GetFiles, Utility Method
                                        transferResult = session.GetFiles(destFTPFolder + fileName, DestFileFolder, false, transferOptions);
                                        transferResult.Check();

                                        //Delete File from SFTP
                                        session.RemoveFiles(destFTPFolder + fileName);
                                    }
                                }

                                IsReposense = true;
                            }
                            else
                            {
                                //string[] filePaths = Directory.GetFiles(DestFileFolder);
                                //foreach (string filePath in filePaths)
                                //    File.Delete(filePath);

                                if (!isFromStatement)
                                {
                                    destFTPFolder = destFTPFolder.Replace("/", "");
                                }

                                //transferOptions.FileMask = ("*>=" + date_.ToString("yyyy-MM-dd hh:mm tt") + "YYYY-MM-DD hh:mm tt");
                                transferOptions.FileMask = ("*" + sfileName + ""); // put only selected file.
                                transferResult = session.PutFiles(DestFileFolder, destFTPFolder, false, transferOptions);
                                if (session.FileExists(destFTPFolder + "\\" + sfileName))
                                {
                                    // Now you can delete this file from FTP server.
                                    IsReposense = true;
                                }
                                else
                                    throw new Exception("Unable to upload file " + sfileName + " on SPFT server.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MDVLogger.BLLErrorLog("BLLBillingClaim::GetLatestReports" + this.WinscpPatch, ex);
                            throw new Exception("Error establisihing secure connection, Contact your System Administrator.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return IsReposense;

        }

        public void DeleteFile_SFTP(string ftp, string userName, string userPassword, Int64 ftpPortNo, string ftpHostKey, string destFTPFolder, string sfileName)
        {
            SessionOptions _SessionOptions = new SessionOptions();
            try
            {
                var with = _SessionOptions;
                with.Protocol = Protocol.Sftp;
                with.HostName = ftp;
                with.UserName = userName;
                with.Password = userPassword;
                try
                {
                    with.SshHostKeyFingerprint = ftpHostKey;
                }
                catch (Exception)
                {
                    throw new Exception("SSH host key \"" + ftpHostKey + "\" does not match pattern.");
                }

                with.PortNumber = Convert.ToInt32(ftpPortNo);
                var sessionOptions = _SessionOptions;
                if (sessionOptions.SshHostKeyFingerprint != null)
                {
                    using (Session session = new Session())
                    {
                        session.ExecutablePath = this.WinscpPatch;
                        session.Open(sessionOptions);
                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Binary;
                        try
                        {
                            session.RemoveFiles(destFTPFolder + sfileName);
                        }
                        catch (Exception ex)
                        {
                            MDVLogger.BLLErrorLog("BLLBillingClaim::GetLatestReports" + this.WinscpPatch, ex);
                            throw new Exception("Error establishing secure connection, Contact your System Administrator.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Functions
        public bool LoginFTP(string userName, string userPassword, string ftp, Int32 ftpPortNo, string hostKey, string destFTPFolder, bool isFromService)
        {
            try
            {
                try
                {
                    if (ftp != "" && userName != "" && userPassword != "" && string.IsNullOrEmpty(hostKey))
                    {
                        objFTP = new clsFTP(ftp, destFTPFolder, userName, userPassword, ftpPortNo);

                        if (objFTP.Login() == true)
                        {
                            objFTP.CloseConnection();
                            return true;
                        }
                    }
                    else
                    {
                        bool status = false;
                        objsftp = new SFTP.SFTP.clsSFTP(ftp, userName, userPassword, ftpPortNo.ToString(), hostKey);
                        status = objsftp.UserLoginAuthorization_SFTP(ftp, userName, userPassword, ftpPortNo.ToString(), hostKey, isFromService);
                        if (status)
                        {
                            //objsftp.CloseConnection();
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetFilesList(string username, string password, string ftp, Int32 ftpPortNo, string ftpHostKey, string destFTPFolder, bool isFromService)
        {
            bool IsLogedIn = false;
            try
            {
                // If HostKey Does not Exist, Than to FTP
                if (string.IsNullOrEmpty(ftpHostKey))
                {

                    IsLogedIn = LoginFTP(username, password, ftp, ftpPortNo, ftpHostKey, destFTPFolder, isFromService);
                    if (IsLogedIn == false)
                    {
                        throw new Exception("Unable to connect to remote server.");
                    }
                    if (IsLogedIn == true)
                    {
                        bool check = objFTP.ChangeDirectory(destFTPFolder);
                        if (check == true)
                        {
                            FileNamesAll = objFTP.GetFileList("");
                        }
                    }
                    FilterFileList();
                    FileNamesAll = null;
                    objFTP.CloseConnection();
                }
                // If HostKey Does Exist, Than to SFTP
                else
                {
                    IsLogedIn = LoginFTP(username, password, ftp, ftpPortNo, ftpHostKey, destFTPFolder, isFromService);
                    if (IsLogedIn == false)
                    {
                        throw new Exception("Unable to connect to remote server.");
                    }
                    FileNamesAllsFTP = objsftp.GetFiles(username, password, ftp, ftpPortNo, ftpHostKey, destFTPFolder, isFromService);
                    FileNamesAll = FileNamesAllsFTP.ToArray();
                    FilterFileList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //FileNamesAll = null;
                //objFTP.CloseConnection();
            }
        }
        public void FilterFileList()
        {
            try
            {
                int i = 0;
                System.Text.StringBuilder TempStr = new System.Text.StringBuilder();
                List<string> ExList = new List<string>(new string[] { "835", "271", "277", "997", "rpt", "txt", "edi" });
                for (i = 0; i < FileNamesAll.Length; i++)
                {
                    if (ExList.FirstOrDefault(p => FileNamesAll[i].Contains(p)) != null)
                    {
                        //do nothing
                        string ext = FileNamesAll[i].Trim().Substring(FileNamesAll[i].Trim().Length - 3, 3).ToLower();
                        if (ExList.FirstOrDefault(p => p.Contains(ext)) != null)
                        {
                            TempStr.Append(FileNamesAll[i].Trim() + " ");
                        }
                    }
                }
                //download only production files

                FileNamesToDownLoad = TempStr.ToString().Trim().Split(' ');
                if (FileNamesToDownLoad.Length == 1)
                {
                    if (FileNamesToDownLoad[0].Trim().Length == 0)
                    {
                        FileNamesToDownLoad = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DownLoadAllFiles(string destFTPFolder, string DestFileFolder)
        {
            try
            {
                int i = 0;
                string FileName = null;
                if (FileNamesToDownLoad != null)
                {
                    for (i = 0; i <= FileNamesToDownLoad.Length - 1; i++)
                    {
                        FileName = FileNamesToDownLoad[i];
                        FileServer Obj = new FileServer();
                        Obj.CreateDirectory(DestFileFolder);
                        objFTP.DownloadFile(FileName, DestFileFolder + FileName, true);
                        //MK//objFTP.DeleteFile(FileName);
                        objFTP.CloseConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objFTP.CloseConnection();
            }
        }


        #endregion

        public struct StructReportId
        {
            public string ID;
            public string Status;
            public string StatusDescription;
        }
        public DSEDIReports DownloadsEDIReport(long clearingHouseId, string userName, string userPasswprd, string ftpHost, Int32 ftpPortNo, string ftpHostKey, string destFTPFolder, string Type, string Title, bool isFromService)
        {
            FileServer Obj = new FileServer();

            destFTPFolder = "\\" + destFTPFolder + "\\";
            string DestFileFolder = "\\\\" + Obj.FtpHostWithAlias() + MDVApplication.CustomerRegCode + "\\EDI" + "\\" + SharedVariable.ClientId + destFTPFolder;

            // Get All Files List from FTP or SFTP
            this.GetFilesList(userName, userPasswprd, ftpHost, ftpPortNo, ftpHostKey, destFTPFolder, isFromService);


            // Download from FTP and put on Shared Folder
            if (string.IsNullOrEmpty(ftpHostKey))
            {
                this.DownLoadAllFiles(destFTPFolder, DestFileFolder);
            }
            // Download from SFTP and put on Shared Folder
            else
            {
                this.DownLoadAllFiles_SFTP(ftpHost, userName, userPasswprd, ftpPortNo, ftpHostKey, DestFileFolder, destFTPFolder.Replace(@"\", "/"), "", DateTime.Now);
            }

            #region " Save downloaded EDI Reports "

            DSEDIReports dsEdiReport = new DSEDIReports();
            DSEDIReports dsTempEdiReport = new DSEDIReports();
            if (FileNamesToDownLoad != null)
            {
                #region " Save EDi files to MDVision Database "

                for (int j = 0; j < FileNamesToDownLoad.Length; j++)
                {
                    DSEDIReports._837ClaimReportDataTable dt_837ClaimReport = new DSEDIReports._837ClaimReportDataTable();
                    DSEDIReports.EDIReportsRow drEdiReport = dsEdiReport.EDIReports.NewEDIReportsRow();

                    string ediText = "";

                    // Reading file data for Logging Purpose
                    if (string.IsNullOrEmpty(ftpHostKey))
                    {
                        Obj.FileRead(DestFileFolder, FileNamesToDownLoad[j].ToString(), ref ediText);
                    }
                    else
                    {
                        //if (!System.IO.Directory.Exists(strTempPath))
                        //{
                        //    throw new Exception("File/Directory does not exist.");
                        //}

                        Obj.FileRead(DestFileFolder, FileNamesToDownLoad[j].ToString(), ref ediText);

                        string sFileName = FileNamesToDownLoad[j].ToString();

                        //DestFileFolder = "\\\\" + MDVApplication.DocsHostName + "\\" + MDVApplication.DocsAlias + "\\" + MDVApplication.CustomerRegCode + "\\EDI" + "\\" + SharedVariable.ClientId + destFTPFolder;                        DestFile = DestFileFolder + sFileName;
                        if (DestFileFolder != "" && ediText != "")
                        {
                            // Write Files to Shared Folder
                            if (Obj.FileWrite(DestFileFolder, sFileName, ediText) == false)
                            {
                                throw new Exception("file does'nt create.");
                            }
                        }
                        else
                        {
                            throw new Exception("Path is not vaild or file does'nt contain data.");
                        }

                    }
                    EDI835Parser parser835 = new EDI835Parser();
                    ArrayList StatusAndClaimNo = new ArrayList();
                    if (ediText.Trim().Length > 0)
                    {
                        if (EDICommon.IsEDI(ediText) == true)
                        {
                            MDVUtility.SetDelimiters(ediText);

                            parser835.GetStatusAndClaimNo(ediText, ref StatusAndClaimNo);
                        }

                        string sDate = parser835.ProductionDate(ediText);
                        DateTime ReportDate;
                        if (sDate == "")
                        {
                            ReportDate = DateTime.Now;
                        }
                        else
                        {
                            ReportDate = MDVUtility.StringToDate(sDate);
                        }
                        var fileName=getFileName(FileNamesToDownLoad[j], ref Type);
                        drEdiReport.ClearingHouseId = clearingHouseId;
                        drEdiReport.ReportDate = ReportDate;
                        drEdiReport.FileName = fileName;
                        drEdiReport.IsActive = true;
                        drEdiReport.IsParse = "";
                        drEdiReport.CreatedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                        drEdiReport.CreatedOn = DateTime.Now;
                        drEdiReport.ModifiedBy = MDVUtility.DecryptFrom64(SharedVariable.UserName);
                        drEdiReport.ModifiedOn = DateTime.Now;
                        drEdiReport.EDIText = ediText;
                        drEdiReport.ReportType = Type;
                        drEdiReport.ReportTitle = Title;
                        drEdiReport.ClientId = Convert.ToInt64(SharedVariable.ClientId);
                        drEdiReport.EntityId = Convert.ToInt64(SharedVariable.EntityId);
                        if (Type=="277")
                        {
                            DS277 ds277 = new DS277();
                            
                            BLObject<DS277> obj277 = new BLLBillingClaim().Report277CA(ediText, this.SharedVariable);
                            ds277 = obj277.Data;
                            dynamic EdiDetailObj = JObject.Parse(getEdidetail(ds277));
                            List<STC> STCList = new List<STC>();
                            STCList = EdiDetailObj.STCRows.ToObject<List<STC>>();
                            drEdiReport.TotalRejected = EdiDetailObj.TotalRejected;
                            drEdiReport.TotalAccepted = EdiDetailObj.TotalAccepted;

                            drEdiReport.xml = MDVUtility.GetXmlOfObject(typeof(List<STC>), STCList);

                        }
                        dsEdiReport.EDIReports.AddEDIReportsRow(drEdiReport);




                        dsEdiReport = new DALEDIReports(SharedVariable).InsertEDIReports(SharedVariable, dsEdiReport);
                        //dsEdiReport.Clear();
                        // :MN: Have to get provider and facility against EDI

                        long ReportId = Convert.ToInt64(dsEdiReport.Tables[dsEdiReport.EDIReports.TableName].Rows[0][dsEdiReport.EDIReports.EDIReportIdColumn.ColumnName]);

                        if (StatusAndClaimNo != null)
                        {
                            int Index = 0;
                            DSEDIReports dsTempEdiReportClaim = new DSEDIReports();
                            //DSEDIReports._837ClaimReportDataTable dt_837ClaimReport = new DSEDIReports._837ClaimReportDataTable();
                            for (Index = 0; Index <= StatusAndClaimNo.Count - 1; Index++)
                            {
                                StructReportId sIdAndStatus = new StructReportId();
                                // sIdAndStatus = (StructReportId)StatusAndClaimNo[Index];
                                sIdAndStatus.ID = ((EDIParser.Professional.EDI835Parser.StructReportId)(StatusAndClaimNo[Index])).ID;
                                sIdAndStatus.Status = ((EDIParser.Professional.EDI835Parser.StructReportId)(StatusAndClaimNo[Index])).Status;
                                sIdAndStatus.StatusDescription = ((EDIParser.Professional.EDI835Parser.StructReportId)(StatusAndClaimNo[Index])).StatusDescription;

                                if (!string.IsNullOrEmpty(sIdAndStatus.ID))
                                {
                                    dt_837ClaimReport = Insert837ClaimReportDetail(sIdAndStatus.ID, ReportId, sIdAndStatus.Status, sIdAndStatus.StatusDescription, dt_837ClaimReport);
                                }
                                dsTempEdiReportClaim.Merge(dt_837ClaimReport);
                            }
                            DSEDIReports ds_EdiReport = new DALEDIReports(SharedVariable).Insert837ClaimReports(SharedVariable, dsTempEdiReportClaim);
                        }
                        dsTempEdiReport.Merge(dsEdiReport);
                    }
                }

                #endregion

            }
            return dsTempEdiReport;
            #endregion

        }
        public string getEdidetail(DS277 ds)
        {
            List<STC> STCRows = new List<STC>();
            var TotalRejected = 0;
            var TotalAccepted = 0;
            foreach (DS277.EDI277NamesRow EDI277Names in ds.EDI277Names)
            {
                if (EDI277Names.NM101_QUL == "85")
                {
                    var ProviderName = EDI277Names.NM103;
                    var NPI = EDI277Names.NM109;

                    foreach (DS277.EDI277NamesRow Patient_item in ds.EDI277Names)
                    {

                        if (Patient_item.ParentNameId == EDI277Names.EDI277NameId)
                        {
                            //Select Patient STC rows
                            foreach (DS277.EDI277StatusRow Status_item in ds.EDI277Status)
                            {

                                if (Status_item.EDI277NameId == Patient_item.EDI277NameId)
                                {
                                    STC stc = new STC();
                                    stc.ClaimNumber = Patient_item.TRN02;
                                    stc.ChargeAmount = Status_item.STC04;
                                    stc.PaidAmount = Status_item.STC05;
                                    stc.ClaimCategoryCode = Status_item.STC01_1_QUL + ": " + Status_item.STC01_1;
                                    stc.ClaimStatusCode = Status_item.STC01_2_QUL + ": " + Status_item.STC01_2;
                                        
                                    if (Status_item.STC03.ToLower() == "accept")
                                    {
                                        stc.isAccepted = true;
                                        TotalAccepted++;
                                    }
                                    else
                                    {
                                        if (Status_item.STC01_1_QUL == "A3" || Status_item.STC01_1_QUL == "A4" || Status_item.STC01_1_QUL == "A6" || Status_item.STC01_1_QUL == "A7" || Status_item.STC01_1_QUL == "A8")
                                        {
                                            stc.rejectionReason = Status_item.STC12;
                                        }
                                        stc.isAccepted = false;
                                        TotalRejected++;
                                    }
                                    STCRows.Add(stc);
                                }

                            }


                        }

                    }
                }
            }

            var respons = new
            {
                TotalAccepted = TotalAccepted,
                TotalRejected = TotalRejected,
                STCRows = STCRows
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(respons));
        }
        private string getFileName(string FileName, ref string Type)
        {
            try
            {
                List<string> EDIFileTypes = new List<string>() { "835", "271", "277", "997", "999", "rpt", "REPORTS" };
                string type_ = EDIFileTypes.FirstOrDefault(s => FileName.Contains("." + s));

                if (!string.IsNullOrEmpty(type_))
                {
                    // FileName example file001.835.edi.cp or file001.835 required FileName is file001.835
                    FileName = FileName.Substring(0, FileName.IndexOf("." + type_));
                    FileName += "." + type_;
                    Type = type_ == "rpt" ? "REPORTS" : type_;
                }
                else
                    throw new Exception("catch");

            }
            catch (Exception)
            {
                FileName = FileName + (Type.ToLower() == "reports" ? ".rpt" : "." + Type);
            }


            return FileName;
        }

        private DSEDIReports._837ClaimReportDataTable Insert837ClaimReportDetail(string ClaimNumber, long ReportId, string Status, string StatusDescription, DSEDIReports._837ClaimReportDataTable dt_837ClaimReport)
        {
            try
            {
                DSEDIReports._837ClaimReportRow dr_837ClaimReportRow = dt_837ClaimReport.New_837ClaimReportRow();
                dr_837ClaimReportRow.Status = Status;
                dr_837ClaimReportRow.Description = StatusDescription;
                dr_837ClaimReportRow.ClaimNumber = ClaimNumber;
                dr_837ClaimReportRow.EDIReportId = ReportId;

                dt_837ClaimReport.Add_837ClaimReportRow(dr_837ClaimReportRow);

                return dt_837ClaimReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
