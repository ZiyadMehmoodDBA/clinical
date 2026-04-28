using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MDVision.Common.Utilities
{
    public class CommonFunc
    {

        public static string SaveDocumentToFolderCompressed(HttpPostedFile file = null, string MainHierarchy = null, string Folder = null, long PatientID = 0, string FileName = null, byte[] currentFileStream = null)
        {
            string newFullPath = string.Empty;
            int count = 1;

            string ServerPath = System.Configuration.ConfigurationManager.AppSettings["PatientFilesPath"];

            string MainFolder = string.Format("Uploads");
            if (!System.IO.Directory.Exists(ServerPath + MainFolder))
                System.IO.Directory.CreateDirectory(ServerPath + MainFolder);

            MainHierarchy = string.Format(MainFolder + "\\" + MainHierarchy);
            if (!System.IO.Directory.Exists(ServerPath + MainHierarchy))
                System.IO.Directory.CreateDirectory(ServerPath + MainHierarchy);

            string PatientFolder = string.Empty;
            if (PatientID > 0)
            {
                PatientFolder = MainHierarchy + "\\Patient_" + PatientID;
            }
            else
            {
                PatientFolder = MainHierarchy + "\\Date_" + DateTime.Now.Date.ToString("dd-MMM-yyy");
            }
            if (!System.IO.Directory.Exists(ServerPath + PatientFolder))
                System.IO.Directory.CreateDirectory(ServerPath + PatientFolder);

            string DocumentFolder = PatientFolder + "\\" + Folder;
            if (!System.IO.Directory.Exists(ServerPath + DocumentFolder))
                System.IO.Directory.CreateDirectory(ServerPath + DocumentFolder);

            DocumentFolder = DocumentFolder + "\\" + DateTime.Now.Date.ToString("dd-MMM-yyy");
            if (!System.IO.Directory.Exists(ServerPath + DocumentFolder))
                System.IO.Directory.CreateDirectory(ServerPath + DocumentFolder);
            if (string.IsNullOrWhiteSpace(FileName))
            {
                FileName = Path.GetFileName(file.FileName);
            }
            string fileNameOnly = FileName.Split('.')[0];
            string extension = FileName.Split('.')[1];

            newFullPath = string.Format(DocumentFolder + "\\{0}.zip", fileNameOnly);
            while (File.Exists(ServerPath + newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(DocumentFolder, tempFileName + ".zip");
            }
            if (file != null)
            {
                file.SaveAs(ServerPath + newFullPath);
            }
            else
            {
                File.WriteAllBytes(ServerPath + newFullPath, currentFileStream);
            }

            return newFullPath;
        }


        public static string SaveDocumentToFolder(HttpPostedFile file = null, string MainHierarchy = null, string Folder = null, long OperationID = 0, string FileName = null, byte[] currentFileStream = null, bool IsDeleteFirst = false,string OldPath="")
        {
            string newFullPath = string.Empty;
            int count = 1;

            string ServerPath = System.Configuration.ConfigurationManager.AppSettings["PatientFilesPath"];

            string MainFolder = string.Format("Uploads");
            if (!System.IO.Directory.Exists(ServerPath + MainFolder))
                System.IO.Directory.CreateDirectory(ServerPath + MainFolder);

            MainHierarchy = string.Format(MainFolder + "\\" + MainHierarchy);
            if (!System.IO.Directory.Exists(ServerPath + MainHierarchy))
                System.IO.Directory.CreateDirectory(ServerPath + MainHierarchy);

            string PatientFolder = string.Empty;
            if (OperationID > 0)
            {
                if (MainHierarchy == "Charge Batch Documents")
                {
                    PatientFolder = MainHierarchy + "\\Charge_Batch_" + OperationID;
                }
                else if (MainHierarchy == "Payment Batch Documents")
                {
                    PatientFolder = MainHierarchy + "\\Payment_Batch_" + OperationID;
                }
                else if (MainHierarchy == "Patient Document")
                {
                    PatientFolder = MainHierarchy + "\\Patient_" + OperationID;
                }
                else
                {
                    PatientFolder = MainHierarchy + "\\Patient_" + OperationID;
                }
            }
            else
            {
                PatientFolder = MainHierarchy + "\\Date_" + DateTime.Now.Date.ToString("dd-MMM-yyy");
            }
            if (!System.IO.Directory.Exists(ServerPath + PatientFolder))
                System.IO.Directory.CreateDirectory(ServerPath + PatientFolder);

            string DocumentFolder = PatientFolder + "\\" + Folder;
            if (!System.IO.Directory.Exists(ServerPath + DocumentFolder))
                System.IO.Directory.CreateDirectory(ServerPath + DocumentFolder);

            DocumentFolder = DocumentFolder + "\\" + DateTime.Now.Date.ToString("dd-MMM-yyy");
            if (!System.IO.Directory.Exists(ServerPath + DocumentFolder))
                System.IO.Directory.CreateDirectory(ServerPath + DocumentFolder);
            if (string.IsNullOrWhiteSpace(FileName))
            {
                if (file != null)
                {
                    FileName = Path.GetFileName(file.FileName);
                }
            }
            int idx = FileName.LastIndexOf('.');
            string fileNameOnly = FileName.Substring(0, idx);// FileName.Split('.')[0];
            string extension = FileName.Substring(idx + 1);// FileName.Split('.')[1];

            newFullPath = string.Format(DocumentFolder + "\\{0}.{1}", fileNameOnly, extension);
            newFullPath = !string.IsNullOrEmpty(OldPath) ? OldPath : newFullPath;
            while (File.Exists(ServerPath + newFullPath))
            {
                if (IsDeleteFirst == true)
                {
                    File.Delete(ServerPath + newFullPath);
                }
                else
                {
                    string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                    newFullPath = Path.Combine(DocumentFolder, tempFileName + "." + extension);
                }

                
            }
            if (file != null)
            {
                file.SaveAs(ServerPath + newFullPath);
            }
            else
            {
                File.WriteAllBytes(ServerPath + newFullPath, currentFileStream);
            }

            return newFullPath;
        }

        public static string DocumentToFolderSave(HttpPostedFile file = null, string MainHierarchy = null, string Folder = null, long OperationID = 0, string FileName = null,string FileTypeExt = null, byte[] currentFileStream = null, bool IsDeleteFirst = false)
        {
            string newFullPath = string.Empty;
            int count = 1;

            string ServerPath = System.Configuration.ConfigurationManager.AppSettings["PatientFilesPath"];

            string MainFolder = string.Format("Uploads");
            if (!System.IO.Directory.Exists(ServerPath + MainFolder))
                System.IO.Directory.CreateDirectory(ServerPath + MainFolder);

            MainHierarchy = string.Format(MainFolder + "\\" + MainHierarchy);
            if (!System.IO.Directory.Exists(ServerPath + MainHierarchy))
                System.IO.Directory.CreateDirectory(ServerPath + MainHierarchy);

            string PatientFolder = string.Empty;
            if (OperationID > 0)
            {
                if (MainHierarchy == "Charge Batch Documents")
                {
                    PatientFolder = MainHierarchy + "\\Charge_Batch_" + OperationID;
                }
                else if (MainHierarchy == "Payment Batch Documents")
                {
                    PatientFolder = MainHierarchy + "\\Payment_Batch_" + OperationID;
                }
                else if (MainHierarchy == "Patient Document")
                {
                    PatientFolder = MainHierarchy + "\\Patient_" + OperationID;
                }
                else
                {
                    PatientFolder = MainHierarchy + "\\Patient_" + OperationID;
                }
            }
            else
            {
                PatientFolder = MainHierarchy + "\\Date_" + DateTime.Now.Date.ToString("dd-MMM-yyy");
            }
            if (!System.IO.Directory.Exists(ServerPath + PatientFolder))
                System.IO.Directory.CreateDirectory(ServerPath + PatientFolder);

            string DocumentFolder = PatientFolder + "\\" + Folder;
            if (!System.IO.Directory.Exists(ServerPath + DocumentFolder))
                System.IO.Directory.CreateDirectory(ServerPath + DocumentFolder);

            DocumentFolder = DocumentFolder + "\\" + DateTime.Now.Date.ToString("dd-MMM-yyy");
            if (!System.IO.Directory.Exists(ServerPath + DocumentFolder))
                System.IO.Directory.CreateDirectory(ServerPath + DocumentFolder);
            if (string.IsNullOrWhiteSpace(FileName))
            {
                if (file != null)
                {
                    FileName = Path.GetFileName(file.FileName);
                }
            }
            string fileNameOnly = FileName.Replace(':', '-');
            string extension = FileTypeExt;

            newFullPath = string.Format(DocumentFolder + "\\{0}.{1}", fileNameOnly, extension);
            while (File.Exists(ServerPath + newFullPath))
            {
                    string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                    newFullPath = Path.Combine(DocumentFolder, tempFileName + "." + extension);
            }
            if (file != null)
            {
                file.SaveAs(Path.Combine(ServerPath, newFullPath));
            }
            else
            {
                File.WriteAllBytes(Path.Combine(ServerPath, newFullPath), currentFileStream);
            }

            return newFullPath;
        }


    }
}
