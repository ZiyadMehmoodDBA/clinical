using MDVision.Business.BCommon;
using MDVision.Business.BCommon.FTP;
using MDVision.Business.BCommon.SFTP;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using WinSCP;

namespace MDVision.Business.AppointmentReminders
{
    class TeleVoxAppointmentReminder
    {
        private static List<string> FileNamesAllsFTP = new List<string>();
        private static string[] FileNamesAll;
        private static string[] FileNamesToDownLoad;

        private static string _ftp = "SSH2.MYTELEVOX.COM";
        private static string _userName = "645943";
        private static string _userPassword = "9Zz3M4Hg";
        private static string _ftpPortNo = "22";
        private static string _hostKey = "ssh-dss 1024 20:3d:53:6f:88:99:a2:bf:fd:b3:ea:84:c5:62:23:eb";

        private clsFTP objFTP;
        public SFTP.clsSFTP objsftp;
        //List<string> FileNamesAllsFTP = new List<string>();
        //private static string[] FileNamesAll;
        //private static string[] FileNamesToDownLoad;

        public static void FilterFileList()
        {
            try
            {
                int i = 0;
                System.Text.StringBuilder tempStr = new System.Text.StringBuilder();

                for (i = 0; i < FileNamesAll.Length; i++)
                {
                    if (FileNamesAll[i].Contains("pdf"))
                    {
                        if (FileNamesAll[i].Trim().Substring(FileNamesAll[i].Trim().Length - 3, 3).ToLower() == "pdf")
                        {
                            tempStr.Append(FileNamesAll[i].Trim() + " ");
                        }
                    }

                    FileNamesToDownLoad = tempStr.ToString().Trim().Split(' ');
                    if (FileNamesToDownLoad.Length == 1)
                    {
                        if (FileNamesToDownLoad[0].Trim().Length == 0)
                        {
                            FileNamesToDownLoad = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string RemoveExtraSomething(string str)
        {
            str= string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            return str.Replace("-", "").Replace("(", "").Replace(")", "");
        }
        public static string CreateMessageText(DataTable table, int index)
        {

            var televoxRow = table.Rows[index];
            var appointmentId = televoxRow["AppointmentId"].ToString();
            var patientName = televoxRow["PatientName"].ToString();
            var patientMobilePhone = RemoveExtraSomething(televoxRow["PatientPhone"].ToString());
            var appointmentDate = televoxRow["AppointmentDate"].ToString();
            var appointmentTime = televoxRow["AppointmentTime"].ToString();
            var patientAccountNumber = televoxRow["AccountNumber"].ToString();
            var providerNumber = televoxRow["ProviderNumber"].ToString();
            var locationNumber = televoxRow["LocationNumber"].ToString();
            var appointmentComments = televoxRow["Comments"].ToString();
            var providerName = televoxRow["ProviderLastName"].ToString() + ", " + televoxRow["ProviderFirstName"].ToString();
            var locationName = televoxRow["LocationName"].ToString();
            var clientMobilePhoneNumber = RemoveExtraSomething(televoxRow["ClientMobilePhoneNumber"].ToString());
            var sendSMS = televoxRow["SendSMS"].ToString();
            var callerIdNumber = RemoveExtraSomething(televoxRow["CallerIDNumber"].ToString());
            var clientEmailAddress = televoxRow["PatientEmailAddress"].ToString();
            var sendEMail = televoxRow["SendEmail"].ToString();
            var neverCall = televoxRow["RevokeReminderService"].ToString();
            var patientLanguage = televoxRow["PatientLanguage"].ToString();

            StringBuilder mStr = new StringBuilder();
            mStr.Append(AddDoubleQuotes(appointmentId));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(patientName));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(patientMobilePhone));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(appointmentDate));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(appointmentTime));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(patientAccountNumber));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(providerNumber));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(locationNumber));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(appointmentComments));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(providerName));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(locationName));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(clientMobilePhoneNumber));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(sendSMS));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(callerIdNumber));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(clientEmailAddress));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(sendEMail));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(neverCall));
            mStr.Append(",");
            mStr.Append(AddDoubleQuotes(patientLanguage));

            return mStr.ToString();


        }
        public static string AddDoubleQuotes(string value)
        {
            return "\"" + value + "\"";
        }

        public static bool UplaodFile_SFTP(SharedVariable SharedVariable, string ftp, string userName, string userPassword, Int64 ftpPortNo,
            string ftpHostKey, string destFileFolder, string destFtpFolder, string sfileName, DateTime date,
            bool isFromStatement = false)
        {
            SessionOptions _SessionOptions = new SessionOptions();
            bool isReposense = false;
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
                        session.ExecutablePath = ConfigurationManager.AppSettings["WinSCP_Path"];

                        session.Open(sessionOptions);
                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Binary;

                        TransferOperationResult transferResult;
                        try
                        {
                            if (!isFromStatement)
                                destFtpFolder = destFtpFolder.Replace("/", "");

                            transferOptions.FileMask = ("*" + sfileName + "");
                            destFileFolder = destFileFolder + "\\" + sfileName;
                            destFtpFolder = destFtpFolder + "//" + sfileName;

                            transferResult = session.PutFiles(destFileFolder, destFtpFolder, false, transferOptions);


                            if (session.FileExists(destFtpFolder))
                            {
                                // If you want to remove the file from the local directory
                                //session.RemoveFiles(destFileFolder);
                                isReposense = true;
                            }
                            else
                                throw new Exception("Unable to upload file " + sfileName + " on SPFT server.");

                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error establisihing secure connection, Contact your System Administrator.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isReposense;
        }

        public static void CreateFile_(string fileName, string messageText, string strPath)
        {
            string path = strPath + fileName;
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine(messageText);
                    tw.Close();
                }
            }
            else if (File.Exists(path))
            {
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine(messageText);
                    tw.Close();
                }
            }
        }

    }

}
