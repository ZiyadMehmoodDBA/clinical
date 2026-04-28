using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.IEHR.EMR.Helpers.Clinical.Summary;
using MDVision.Model.Clinical.Medical.Implantable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace MDVision.IEHR.EMR.HL7Folder.Summary
{
    public class ClinicalSummary_ImportCCDA
    {
        private BLLPatient _bllPatientObj = null;
        private static DSCCDA dsCCDA = null;
        private static string CreatedBy = "CCDA";
        private static string ModifiedBy = "CCDA";
        private static string ModifiedByImmunization = "CCDA, Integration";
        private static string ProviderId { get { return ConfigurationManager.AppSettings["DefaultProviderId"]; } }
        private static string FacilityId { get { return ConfigurationManager.AppSettings["DefaultFacilityId"]; } }
        private static string DocTemplateId = "2.16.840.1.113883.10.20.22.1.1";
        private static string EntityId { get { return MDVSession.Current.EntityId; } }
        private static long UserId { get { return MDVSession.Current.AppUserId; } }
        private static Int64 patientId = 0;
        private static string PracticeId { get { return ConfigurationManager.AppSettings["DefaultPracticeId"]; } }
        private static DateTime CreatedOn { get { return DateTime.Now; } }
        private static DateTime ModifiedOn { get { return DateTime.Now; } }
        private static List<string> EncounterSNOMEDIDs = null;
        private static string FollowUp = string.Empty;
        private static int careTeamCounter = -1;
        private static BLLCCDA _bllCCDAobj = null;
        public ClinicalSummary_ImportCCDA()
        {
            dsCCDA = new DSCCDA();
            _bllCCDAobj = new BLLCCDA();
            _bllPatientObj = new BLLPatient();
            EncounterSNOMEDIDs = new List<string>();
        }

        public DSPatient ReconcileCCDABase64(string xmlContent, ref DSCCDA dsCCDAReconcile, List<string> sectionList, List<string> errors, string fileTypeCDDA, string DocfileType, string DocfileName, bool IsFile)
        {
            ClinicalDocument CCDA;
            DSPatient ds = new DSPatient();
            string url = string.Empty;
            byte[] encodedString = null;
            try
            {
                if (fileTypeCDDA.ToLower() == "ccda")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        byte[] byteArray2 = Encoding.UTF8.GetBytes(xmlContent);
                        byte[] byteArray = Convert.FromBase64String(Encoding.ASCII.GetString(byteArray2));
                        url = CommonFunc.SaveDocumentToFolder(null, "Import_CCDA", "Import_CCDA", 0, "ccda.xml", byteArray);
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        string url2 = FilePath + url;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(url2);
                        string xmlcontents = doc.InnerXml;
                        encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.2"))
                    {
                        GetDocumentData(CCDA, ref dsCCDAReconcile, ref ds, ref sectionList, ref errors);
                        string ConfidentialityCode = MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.ConfidentialityCodeColumn.ColumnName]);
                        if (ConfidentialityCode == "R")
                        {
                            throw new Exception("privacy protected");
                        }
                        return ds;
                    }
                    else
                    {
                        throw new Exception("Unknown Document Type. The selected file is not a valid CCD file");
                    }
                }
                else if (fileTypeCDDA.ToLower() == "ccd v3")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        byte[] byteArray2 = Encoding.UTF8.GetBytes(xmlContent);
                        byte[] byteArray = Convert.FromBase64String(Encoding.ASCII.GetString(byteArray2));
                        url = CommonFunc.SaveDocumentToFolder(null, "Import_CCDA", "Import_CCDA", 0, "ccda.xml", byteArray);
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        string url2 = FilePath + url;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(url2);
                        string xmlcontents = doc.InnerXml;
                        encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.2"))
                    {
                        GetDocumentData(CCDA, ref dsCCDAReconcile, ref ds, ref sectionList, ref errors);
                        string ConfidentialityCode = MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.ConfidentialityCodeColumn.ColumnName]);
                        if (ConfidentialityCode == "R")
                        {
                            throw new Exception("privacy protected");
                        }
                        return ds;
                    }
                    else
                    {
                        throw new Exception("Unknown Document Type. The selected file is not a valid CCD V3 file");
                    }
                }
                else if (fileTypeCDDA.ToLower() == "referral note")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        byte[] byteArray2 = Encoding.UTF8.GetBytes(xmlContent);
                        byte[] byteArray = Convert.FromBase64String(Encoding.ASCII.GetString(byteArray2));
                        url = CommonFunc.SaveDocumentToFolder(null, "Import_CCDA", "Import_CCDA", 0, "ccda.xml", byteArray);
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        string url2 = FilePath + url;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(url2);
                        string xmlcontents = doc.InnerXml;
                        encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.14"))
                    {
                        GetDocumentData(CCDA, ref dsCCDAReconcile, ref ds, ref sectionList, ref errors);
                        string ConfidentialityCode = MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.ConfidentialityCodeColumn.ColumnName]);
                        if (ConfidentialityCode == "R")
                        {
                            throw new Exception("privacy protected");
                        }
                        return ds;
                    }
                    else
                    {
                        throw new Exception("Unknown Document Type. The selected file is not a valid Referral Note file");
                    }
                }
                else if (fileTypeCDDA.ToLower() == "care plan")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        byte[] byteArray2 = Encoding.UTF8.GetBytes(xmlContent);
                        byte[] byteArray = Convert.FromBase64String(Encoding.ASCII.GetString(byteArray2));
                        url = CommonFunc.SaveDocumentToFolder(null, "Import_CCDA", "Import_CCDA", 0, "ccda.xml", byteArray);
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        string url2 = FilePath + url;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(url2);
                        string xmlcontents = doc.InnerXml;
                        encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.15"))
                    {
                        GetDocumentDataCarePlan(CCDA, ref dsCCDAReconcile, ref ds, ref sectionList, ref errors);
                        string ConfidentialityCode = MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.ConfidentialityCodeColumn.ColumnName]);
                        if (ConfidentialityCode == "R")
                        {
                            throw new Exception("privacy protected");
                        }
                        return ds;
                    }
                    else
                    {
                        throw new Exception("Unknown Document Type. The selected file is not a valid Referral Note file");
                    }
                }
                else
                {
                    throw new Exception("The selected file is not valid");
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public DSPatient ReconcileCCDA(string xmlContent, ref DSCCDA dsCCDAReconcile, List<string> sectionList, ref List<string> errors, string fileType, bool IsFile, long PatientId = 0)
        {
            ClinicalDocument CCDA;
            DSPatient ds = new DSPatient();
            try
            {
                if (fileType.ToLower() == "ccda")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        xmlContent = FilePath + xmlContent;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlContent);
                        string xmlcontents = doc.InnerXml;
                        byte[] encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.2"))
                    {
                        GetDocumentData(CCDA, ref dsCCDAReconcile, ref ds, ref sectionList, ref errors);
                        string ConfidentialityCode = MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.ConfidentialityCodeColumn.ColumnName]);
                        if (ConfidentialityCode == "R")
                        {
                            throw new Exception("privacy protected");
                        }
                        return ds;
                    }
                    else
                    {
                        throw new Exception("Unknown Document Type. The selected file is not a valid CCD file");
                    }
                }
                else if (fileType.ToLower() == "ccd v3")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        xmlContent = FilePath + xmlContent;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlContent);
                        string xmlcontents = doc.InnerXml;
                        byte[] encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.2"))
                    {
                        GetDocumentData(CCDA, ref dsCCDAReconcile, ref ds, ref sectionList, ref errors);
                        string ConfidentialityCode = MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.ConfidentialityCodeColumn.ColumnName]);
                        if (ConfidentialityCode == "R")
                        {
                            throw new Exception("privacy protected");
                        }
                        return ds;
                    }
                    else
                    {
                        throw new Exception("Unknown Document Type. The selected file is not a valid CCD V3 file");
                    }
                }
                else if (fileType.ToLower() == "referral note")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        xmlContent = FilePath + xmlContent;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlContent);
                        string xmlcontents = doc.InnerXml;
                        byte[] encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.14"))
                    {
                        GetDocumentData(CCDA, ref dsCCDAReconcile, ref ds, ref sectionList, ref errors);
                        string ConfidentialityCode = MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.ConfidentialityCodeColumn.ColumnName]);
                        if (ConfidentialityCode == "R")
                        {
                            throw new Exception("privacy protected");
                        }
                        return ds;
                    }
                    else
                    {
                        throw new Exception("Unknown Document Type. The selected file is not a valid Referral Note file");
                    }
                }
                else if (fileType.ToLower() == "care plan")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        xmlContent = FilePath + xmlContent;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlContent);
                        string xmlcontents = doc.InnerXml;
                        byte[] encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.15"))
                    {
                        GetDocumentDataCarePlan(CCDA, ref dsCCDAReconcile, ref ds, ref sectionList, ref errors);
                        string ConfidentialityCode = MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.ConfidentialityCodeColumn.ColumnName]);
                        if (ConfidentialityCode == "R")
                        {
                            throw new Exception("privacy protected");
                        }
                        return ds;
                    }
                    else
                    {
                        throw new Exception("Unknown Document Type. The selected file is not a valid Referral Note file");
                    }
                }
                else
                {
                    throw new Exception("The selected file is not valid");
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public string ReconcileCCDAData(string xmlContent, List<string> sectionList, ref List<string> errors, string fileTypeCDDA, bool IsFile, long PatientId = 0, long ProviderId= 0, long FacilityId = 0)
        {
            ClinicalDocument CCDA;
            DSPatient ds = new DSPatient();
            string strResult = string.Empty;
            try
            {
                if (fileTypeCDDA.ToLower() == "ccda")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        xmlContent = FilePath + xmlContent;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlContent);
                        string xmlcontents = doc.InnerXml;
                        byte[] encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.2"))
                    {
                        strResult = ImportDocumentData(CCDA, ref ds, ref sectionList, ref errors, fileTypeCDDA);

                        if (strResult == "Data Imported successfully.")
                        {
                            BLObject<DSCCDA> dsCCDAReconcilation = new BLObject<DSCCDA>();
                            BLLCCDA _bllCCDAobj = new BLLCCDA();
                            dsCCDAReconcilation = _bllCCDAobj.InsertMUSetting(MDVUtility.ToInt64(patientId), ProviderId, FacilityId, 0, false, false, false, false, false, 0, 0, true );

                        }
                    }
                    else
                    {
                        throw new Exception("The selected file is not valid");
                    }
                }
                else if (fileTypeCDDA.ToLower() == "ccd v3")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        xmlContent = FilePath + xmlContent;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlContent);
                        string xmlcontents = doc.InnerXml;
                        byte[] encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.2"))
                    {
                        strResult = ImportDocumentData(CCDA, ref ds, ref sectionList, ref errors, fileTypeCDDA);
                        
                    }
                    else
                    {
                        throw new Exception("The selected file is not valid");
                    }
                }
                else if (fileTypeCDDA.ToLower() == "referral note")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        xmlContent = FilePath + xmlContent;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlContent);
                        string xmlcontents = doc.InnerXml;
                        byte[] encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.14"))
                    {
                        strResult = ImportDocumentData(CCDA, ref ds, ref sectionList, ref errors, fileTypeCDDA);
                    }
                    else
                    {
                        throw new Exception("The selected file is not valid");
                    }
                }
                else if (fileTypeCDDA.ToLower() == "care plan")
                {
                    if (IsFile)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(byteArray, ref errors);
                    }
                    else
                    {
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        xmlContent = FilePath + xmlContent;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlContent);
                        string xmlcontents = doc.InnerXml;
                        byte[] encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.15"))
                    {
                        strResult = ImportDocumentDataCarePlan(CCDA, ref ds, ref sectionList, ref errors, fileTypeCDDA);
                    }
                    else
                    {
                        throw new Exception("The selected file is not valid");
                    }
                }
                else
                {
                    throw new Exception("The selected file is not valid");
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return strResult;
        }

        public DSPatient ReconcileCCDAReconcilation(string xmlContent, ref DSCCDA dsCCDAReconcilation, List<string> sectionList, ref List<string> errors, bool IsFile, long PatientId = 0)
        {
            ClinicalDocument CCDA;
            DSPatient ds = new DSPatient();
            try
            {
                if (IsFile)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                    CCDA = GetDeserializeDocument(byteArray, ref errors);
                }
                else
                {
                    string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                    xmlContent = FilePath + xmlContent;
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlContent);
                    string xmlcontents = doc.InnerXml;
                    byte[] encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                    CCDA = GetDeserializeDocument(encodedString, ref errors);
                }
                if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.2"))
                {
                    GetDocumentDataReconcilation(ref dsCCDAReconcilation, CCDA, ref ds, ref sectionList, ref errors);
                    return ds;
                }
                else
                {
                    throw new Exception("The selected file is not valid");
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public string GetDocumentDataReconcilation(ref DSCCDA dsCCDAReconcilation, ClinicalDocument document, ref DSPatient dsPatient, ref List<string> sectionList, ref List<string> errors)
        {
            if (document == null) return string.Empty;
            if (document.Validate(DocTemplateId))
            {
                #region Patient 

                var dsPatientLookUp = new BLLClinical().PatientLookup();

                //var pr = document.GetPatientRole();
                var dtPatient = dsPatient.Tables[dsPatient.Patients.TableName];

                GetPatientData(document, ref dtPatient, ref errors, dsPatientLookUp.Data);

                string ConfidenitalityCode = document.GetConfidentialityCode();

                if (dtPatient.HasRows())
                {
                    if (!string.IsNullOrEmpty(ConfidenitalityCode))
                    {
                        dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.ConfidentialityCodeColumn.ColumnName] = ConfidenitalityCode;
                    }
                    dsPatient.Merge(dtPatient);
                }

                #endregion Patient

                if (sectionList.IsNullOrEmpty()) return string.Empty;
                foreach (string sectionId in sectionList)
                {
                    if (sectionId == DocumentSections.Allergies)
                    {
                        #region Allergies
                        DataTable dtAllergies = dsCCDAReconcilation.Tables[dsCCDAReconcilation.Allergy.TableName];
                        GetAllergiesData(document, ref dtAllergies, ref errors, patientId);
                        if (dtAllergies.Rows.Count > 0)
                        {
                            dsCCDAReconcilation.Merge(dtAllergies);
                        }
                        #endregion Allergies
                    }
                    else if (sectionId == DocumentSections.Medications)
                    {
                        #region Medications
                        DataTable dtMedication = dsCCDAReconcilation.Tables[dsCCDAReconcilation.Medication.TableName];
                        GetMedicationData(document, ref dtMedication, ref errors, patientId);
                        if (dtMedication.Rows.Count > 0)
                        {
                            dsCCDAReconcilation.Merge(dtMedication);
                        }
                        #endregion Medications

                        #region Planned Medication
                        DataTable dtPOTMedication = dsCCDA.Tables[dsCCDA.Medication.TableName];
                        GetPOTMedicationData(document, ref dtPOTMedication, ref errors, patientId);
                        if (dtPOTMedication.Rows.Count > 0)
                        {
                            dsCCDA.Merge(dtPOTMedication);
                        }
                        #endregion Planned Medication
                    }
                    else if (sectionId == DocumentSections.Problem)
                    {
                        #region Problems
                        DataTable dtProblems = dsCCDAReconcilation.Tables[dsCCDAReconcilation.ProblemList.TableName];
                        GetProblemsData(document, ref dtProblems, ref errors, patientId);
                        if (dtProblems.Rows.Count > 0)
                        {
                            dsCCDAReconcilation.Merge(dtProblems);
                        }
                        #endregion Problems
                    }
                }
            }
            else
            {
                throw new Exception("The selected file is not valid");
            }

            return string.Empty;
        }

        public string GetDocumentData(ClinicalDocument document, ref DSCCDA dsCCDAReconcile, ref DSPatient dsPatient, ref List<string> sectionList, ref List<string> errors)
        {
            if (document == null) return string.Empty;
            if (document.Validate(DocTemplateId))
            {

                #region Patient 

                var dsPatientLookUp = new BLLClinical().PatientLookup();

                //var pr = document.GetPatientRole();
                var dtPatient = dsPatient.Tables[dsPatient.Patients.TableName];

                GetPatientData(document, ref dtPatient, ref errors, dsPatientLookUp.Data);

                string ConfidenitalityCode = document.GetConfidentialityCode();

                if (dtPatient.HasRows())
                {
                    if (!string.IsNullOrEmpty(ConfidenitalityCode))
                    {
                        dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.ConfidentialityCodeColumn.ColumnName] = ConfidenitalityCode;
                    }
                    dsPatient.Merge(dtPatient);
                }

                #endregion Patient

                #region Allergies

                DataTable dtAllergies = dsCCDAReconcile.Tables[dsCCDAReconcile.Allergy.TableName];
                GetAllergiesData(document, ref dtAllergies, ref errors, patientId);
                if (dtAllergies.Rows.Count > 0)
                {
                    dsCCDAReconcile.Merge(dtAllergies);
                }

                #endregion Allergies

                #region Medications

                DataTable dtMedication = dsCCDAReconcile.Tables[dsCCDAReconcile.Medication.TableName];
                GetMedicationData(document, ref dtMedication, ref errors, patientId);
                if (dtMedication.Rows.Count > 0)
                {
                    dsCCDAReconcile.Merge(dtMedication);
                }

                #endregion Medications

                #region Problems

                DataTable dtProblems = dsCCDAReconcile.Tables[dsCCDAReconcile.ProblemList.TableName];
                GetProblemsData(document, ref dtProblems, ref errors, patientId);
                if (dtProblems.Rows.Count > 0)
                {
                    dsCCDAReconcile.Merge(dtProblems);
                }

                #endregion Problems

                #region Planned Medication
                DataTable dtPOTMedication = dsCCDAReconcile.Tables[dsCCDAReconcile.Medication.TableName];
                GetPOTMedicationData(document, ref dtPOTMedication, ref errors, patientId);
                if (dtPOTMedication.Rows.Count > 0)
                {
                    dsCCDAReconcile.Merge(dtPOTMedication);
                }
                #endregion Planned Medication

            }
            else
            {
                throw new Exception("The selected file is not valid");
            }

            return string.Empty;
        }

        public string GetDocumentDataCarePlan(ClinicalDocument document, ref DSCCDA dsCCDAReconcile, ref DSPatient dsPatient, ref List<string> sectionList, ref List<string> errors)
        {
            if (document == null) return string.Empty;
            if (document.Validate(DocTemplateId))
            {

                #region Patient 

                var dsPatientLookUp = new BLLClinical().PatientLookup();

                //var pr = document.GetPatientRole();
                var dtPatient = dsPatient.Tables[dsPatient.Patients.TableName];

                GetPatientData(document, ref dtPatient, ref errors, dsPatientLookUp.Data);

                string ConfidenitalityCode = document.GetConfidentialityCode();

                if (dtPatient.HasRows())
                {
                    if (!string.IsNullOrEmpty(ConfidenitalityCode))
                    {
                        dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.ConfidentialityCodeColumn.ColumnName] = ConfidenitalityCode;
                    }
                    dsPatient.Merge(dtPatient);
                }

                #endregion Patient

                #region Goals
                DataTable dtCarePlanGoals = dsCCDAReconcile.Tables[dsCCDAReconcile.CarePlanGoals.TableName];
                GetCarePlanGoalsData(document, ref dtCarePlanGoals, ref errors, patientId);
                if (dtCarePlanGoals.Rows.Count > 0)
                {
                    dsCCDAReconcile.Merge(dtCarePlanGoals);
                }
                #endregion Goals

                #region Health Concerns
                DataTable dtHealthConcerns = dsCCDAReconcile.Tables[dsCCDAReconcile.HealthConcerns.TableName];
                GetCarePlanHealthConcernData(document, ref dtHealthConcerns, ref errors, patientId);
                if (dtHealthConcerns.Rows.Count > 0)
                {
                    dsCCDAReconcile.Merge(dtHealthConcerns);
                }
                #endregion Health Concerns

                #region Interventions
                DataTable dtInterventions = dsCCDA.Tables[dsCCDA.Interventions.TableName];
                GetCarePlanInterventionsData(document, ref dtInterventions, ref errors, patientId);
                if (dtInterventions.Rows.Count > 0)
                {
                    dsCCDA.Merge(dtInterventions);
                }
                #endregion Interventions

                #region Health Status Evaluations/Outcomes Section
                DataTable dtHealthStatus = dsCCDAReconcile.Tables[dsCCDAReconcile.HealthStatus.TableName];
                GetCarePlanHealthStatusData(document, ref dtHealthStatus, ref errors, patientId);
                if (dtHealthStatus.Rows.Count > 0)
                {
                    dsCCDAReconcile.Merge(dtHealthStatus);
                }
                #endregion Health Status Evaluations/Outcomes Section

            }
            else
            {
                throw new Exception("The selected file is not valid");
            }

            return string.Empty;
        }

        public string ImportDocumentDataCarePlan(ClinicalDocument document, ref DSPatient dsPatient, ref List<string> sectionList, ref List<string> errors, string fileTypeCDDA)
        {
            string strResult = string.Empty;
            if (document == null) return strResult;
            if (document.Validate(DocTemplateId))
            {

                #region Patient 

                var dsPatientLookUp = new BLLClinical().PatientLookup();

                //var pr = document.GetPatientRole();
                var dtPatient = dsPatient.Tables[dsPatient.Patients.TableName];

                GetPatientData(document, ref dtPatient, ref errors, dsPatientLookUp.Data);

                string ConfidenitalityCode = document.GetConfidentialityCode();

                if (errors.IsNullOrEmpty() && dtPatient.HasRows())
                {
                    if (!string.IsNullOrEmpty(ConfidenitalityCode))
                    {
                        dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.ConfidentialityCodeColumn.ColumnName] = ConfidenitalityCode;
                    }
                    dsPatient.Merge(dtPatient);
                }
                if (dsPatient.Patients.Rows.Count > 0)
                {
                    dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.RecordCountColumn.ColumnName] = 15;

                    string lastName = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.LastNameColumn.ColumnName].ToString();
                    string firstName = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.FirstNameColumn.ColumnName].ToString();
                    DateTime dob = MDVUtility.ToDateTime(dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.DOBColumn.ColumnName]);
                    string gender = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.GenderColumn.ColumnName].ToString();

                    Int64 PatientId = new BLLClinical().CCDAPatientsCheckExist(lastName, firstName, dob, gender);

                    if (PatientId > 0)
                    {
                        patientId = PatientId;
                    }
                    else
                    {
                        BLObject<DSPatient> objj = _bllPatientObj.InsertPatient_CCDA(dsPatient);
                        if (dsPatient.Patients.Rows.Count > 0 && dsPatient.Patients[0].PatientId > 0)
                        {
                            patientId = dsPatient.Patients[0].PatientId;
                        }
                        else
                        {
                            BLObject<DSPatient> obj_ = new BLLPatient().LoadPatient(dsPatient);
                            if (obj_.Data.Patients.Rows.Count > 0)
                            {
                                patientId = obj_.Data.Patients[0].PatientId;
                            }
                        }
                    }
                }

                #endregion Patient

                if (patientId > 0)
                {
                    #region Care Team members

                    DataTable dtCareTeamMembers = dsCCDA.Tables[dsCCDA.CareTeamMembers.TableName];
                    GetCareTeamMembersData(document, ref dtCareTeamMembers, ref errors, patientId);
                    if (dtCareTeamMembers.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtCareTeamMembers);
                    }

                    //DataTable dtCareGivers = dsCCDA.Tables[dsCCDA.CareTeamMembers.TableName];
                    //GetCareGiversData(document, ref dtCareGivers, ref errors, patientId);
                    //if (dtCareGivers.Rows.Count > 0)
                    //{
                    //    dsCCDA.Merge(dtCareGivers);
                    //}

                    #endregion Care Team members

                    #region Goals
                    DataTable dtCarePlanGoals = dsCCDA.Tables[dsCCDA.CarePlanGoals.TableName];
                    GetCarePlanGoalsData(document, ref dtCarePlanGoals, ref errors, patientId);
                    if (dtCarePlanGoals.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtCarePlanGoals);
                    }
                    #endregion Goals

                    #region Health Concerns
                    DataTable dtHealthConcerns = dsCCDA.Tables[dsCCDA.HealthConcerns.TableName];
                    GetCarePlanHealthConcernData(document, ref dtHealthConcerns, ref errors, patientId);
                    if (dtHealthConcerns.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtHealthConcerns);
                    }
                    #endregion Health Concerns

                    #region Interventions
                    DataTable dtInterventions = dsCCDA.Tables[dsCCDA.Interventions.TableName];
                    GetCarePlanInterventionsData(document, ref dtInterventions, ref errors, patientId);
                    if (dtInterventions.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtInterventions);
                    }
                    #endregion Interventions

                    #region Health Status Evaluations/Outcomes Section
                    DataTable dtHealthStatus = dsCCDA.Tables[dsCCDA.HealthStatus.TableName];
                    GetCarePlanHealthStatusData(document, ref dtHealthStatus, ref errors, patientId);
                    if (dtHealthStatus.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtHealthStatus);
                    }
                    #endregion Health Status Evaluations/Outcomes Section

                    #region Database Insert

                    strResult = _bllCCDAobj.ImportCCDAData(dsCCDA);

                    #endregion Database Insert

                }
                else
                {
                    throw new Exception("Patient does not exists.");
                }
            }
            else
            {
                throw new Exception("The selected file is not valid");
            }
            if (string.IsNullOrWhiteSpace(strResult))
            {
                strResult = "Data Imported successfully.";
            }

            return strResult;

        }

        public string ImportDocumentData(ClinicalDocument document, ref DSPatient dsPatient, ref List<string> sectionList, ref List<string> errors, string fileTypeCDDA)
        {
            string strResult = string.Empty;
            FollowUp = string.Empty;
            if (document == null) return strResult;
            if (document.Validate(DocTemplateId))
            {

                #region Patient 

                var dsPatientLookUp = new BLLClinical().PatientLookup();

                //var pr = document.GetPatientRole();
                var dtPatient = dsPatient.Tables[dsPatient.Patients.TableName];

                GetPatientData(document, ref dtPatient, ref errors, dsPatientLookUp.Data);

                string ConfidenitalityCode = document.GetConfidentialityCode();

                if (errors.IsNullOrEmpty() && dtPatient.HasRows())
                {
                    if (!string.IsNullOrEmpty(ConfidenitalityCode))
                    {
                        dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.ConfidentialityCodeColumn.ColumnName] = ConfidenitalityCode;
                    }
                    dsPatient.Merge(dtPatient);
                }
                if (dsPatient.Patients.Rows.Count > 0)
                {
                    dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.RecordCountColumn.ColumnName] = 15;

                    string lastName = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.LastNameColumn.ColumnName].ToString();
                    string firstName = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.FirstNameColumn.ColumnName].ToString();
                    DateTime dob = MDVUtility.ToDateTime(dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.DOBColumn.ColumnName]);
                    string gender = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.GenderColumn.ColumnName].ToString();

                    Int64 PatientId = new BLLClinical().CCDAPatientsCheckExist(lastName, firstName, dob, gender);

                    if (PatientId > 0)
                    {
                        patientId = PatientId;
                    }
                    else
                    {
                        BLObject<DSPatient> objj = _bllPatientObj.InsertPatient_CCDA(dsPatient);
                        if (dsPatient.Patients.Rows.Count > 0 && dsPatient.Patients[0].PatientId > 0)
                        {
                            patientId = dsPatient.Patients[0].PatientId;
                        }
                        else
                        {
                            BLObject<DSPatient> obj_ = new BLLPatient().LoadPatient(dsPatient);
                            if (obj_.Data.Patients.Rows.Count > 0)
                            {
                                patientId = obj_.Data.Patients[0].PatientId;
                            }
                        }
                    }
                }

                #endregion Patient

                if (patientId > 0)
                {

                    #region Care Team members

                    DataTable dtCareTeamMembers = dsCCDA.Tables[dsCCDA.CareTeamMembers.TableName];
                    GetCareTeamMembersData(document, ref dtCareTeamMembers, ref errors, patientId);
                    if (dtCareTeamMembers.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtCareTeamMembers);
                    }

                    //DataTable dtCareGivers = dsCCDA.Tables[dsCCDA.CareTeamMembers.TableName];
                    //GetCareGiversData(document, ref dtCareGivers, ref errors, patientId);
                    //if (dtCareGivers.Rows.Count > 0)
                    //{
                    //    dsCCDA.Merge(dtCareGivers);
                    //}

                    #endregion Care Team members

                    #region Allergies

                    DataTable dtAllergies = dsCCDA.Tables[dsCCDA.Allergy.TableName];
                    GetAllergiesData(document, ref dtAllergies, ref errors, patientId);
                    if (dtAllergies.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtAllergies);
                    }
                    #endregion Allergies

                    #region Immunization

                    DataTable dtImmunization = dsCCDA.Tables[dsCCDA.Immunization.TableName];
                    GetImmunizationData(document, ref dtImmunization, ref errors, patientId);
                    if (dtImmunization.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtImmunization);
                    }
                    #endregion

                    #region Medications

                    DataTable dtMedication = dsCCDA.Tables[dsCCDA.Medication.TableName];
                    GetMedicationData(document, ref dtMedication, ref errors, patientId);
                    if (dtMedication.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtMedication);
                    }
                    #endregion Medications

                    #region Problems

                    DataTable dtProblems = dsCCDA.Tables[dsCCDA.ProblemList.TableName];
                    GetProblemsData(document, ref dtProblems, ref errors, patientId);
                    if (dtProblems.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtProblems);
                    }
                    #endregion Problems

                    #region Procedures

                    DataTable dtProcedures = dsCCDA.Tables[dsCCDA.NotesProcedures.TableName];
                    GetProceduresData(document, ref dtProcedures, ref errors, patientId);
                    if (dtProcedures.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtProcedures);
                    }
                    #endregion Procedures

                    #region Vital Signs

                    DataTable dtVitalSigns = dsCCDA.Tables[dsCCDA.VitalSigns.TableName];
                    GetVitalSignsData(document, ref dtVitalSigns, ref errors, patientId);
                    if (dtVitalSigns.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtVitalSigns);
                    }

                    #endregion Vital Signs

                    #region Results

                    DataTable dtResults = dsCCDA.Tables[dsCCDA.ResultDetail.TableName];
                    GetResultsData(document, ref dtResults, ref errors, patientId);
                    if (dtResults.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtResults);
                    }

                    #endregion Results

                    #region Social History

                    DataTable dtSocialHistory = dsCCDA.Tables[dsCCDA.SocialHistory.TableName];
                    GetSocialHistoryData(document, ref dtSocialHistory, ref errors, patientId);
                    if (dtSocialHistory.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtSocialHistory);
                    }

                    #endregion Social History

                    #region Laboratory Test

                    DataTable dtLaboratoryTest = dsCCDA.Tables[dsCCDA.LabOrderTest.TableName];
                    GetLaboratoryTestData(document, ref dtLaboratoryTest, ref errors, patientId);
                    if (dtLaboratoryTest.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtLaboratoryTest);
                    }

                    #endregion Laboratory Test

                    #region Goals

                    DataTable dtGoals = dsCCDA.Tables[dsCCDA.EnrolledGoals.TableName];
                    GetGoalsData(document, ref dtGoals, ref errors, patientId);
                    if (dtGoals.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtGoals);
                    }

                    #endregion Goals

                    #region Medical Device Equipment

                    DataTable dtMedicalDeviceEquipment = dsCCDA.Tables[dsCCDA.MedicalDeviceEquipment.TableName];
                    GetMedicalDeviceEquipmentData(document, ref dtMedicalDeviceEquipment, ref errors, patientId);
                    if (dtMedicalDeviceEquipment.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtMedicalDeviceEquipment);
                    }

                    #endregion Medical Device Equipment

                    #region Health Concerns

                    DataTable dtHealthConcern = dsCCDA.Tables[dsCCDA.HealthConcerns.TableName];
                    GetHealthConcernData(document, ref dtHealthConcern, ref errors, patientId);
                    if (dtHealthConcern.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtHealthConcern);
                    }


                    #endregion Health Concerns

                    #region Planned Medication
                    DataTable dtPOTMedication = dsCCDA.Tables[dsCCDA.Medication.TableName];
                    GetPOTMedicationData(document, ref dtPOTMedication, ref errors, patientId);
                    if (dtPOTMedication.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtPOTMedication);
                    }
                    #endregion Planned Medication

                    #region Encounter

                    DataTable dtEncounter = dsCCDA.Tables[dsCCDA.NotesEncounter.TableName];
                    GetEncounterData(document, ref dtEncounter, ref errors, patientId);
                    if (dtEncounter.Rows.Count > 0)
                    {
                        dsCCDA.Merge(dtEncounter);
                        DataTable dtPOTEncounter = dsCCDA.Tables[dsCCDA.NotesEncounter.TableName];
                        GetPOTEncounterData(document, ref dtPOTEncounter, ref errors, patientId);
                    }
                    else
                    {
                        DataTable dtPOTEncounter = dsCCDA.Tables[dsCCDA.NotesEncounter.TableName];
                        GetPOTEncounterData(document, ref dtPOTEncounter, ref errors, patientId);
                        if (dtPOTEncounter.Rows.Count > 0)
                        {
                            dsCCDA.Merge(dtPOTEncounter);
                        }
                    }

                    #endregion Encounter
                    var bResultChiefCoomplaint = false;
                    #region Assessment
                    DataTable dtAssessment = dsCCDA.Tables[dsCCDA.Assessment.TableName];
                    GetAssessmentData(document, ref dtAssessment, ref errors, patientId);
                    string Assessment = string.Empty;
                    if (dtAssessment.Rows.Count > 0)
                    {
                        if (dsCCDA.ProblemList.Rows.Count > 0)
                        {
                            if (EncounterSNOMEDIDs != null && EncounterSNOMEDIDs.Count() > 0)
                            {
                                foreach (DataRow drAssessment in dtAssessment.Rows)
                                {
                                    foreach (DataRow drProblem in dsCCDA.ProblemList.Rows)
                                    {
                                        foreach (string SNOMEDID in EncounterSNOMEDIDs)
                                        {
                                            if (SNOMEDID == drProblem["SNOMEDID"].ToString())
                                            {
                                                drProblem["Comments"] = drAssessment["AssessmentDescription"];
                                                drProblem["IsChiefComplaint"] = true;
                                                drProblem["ComplaintComments"] = FollowUp;
                                                drProblem["IsProblem"] = true;
                                                bResultChiefCoomplaint = true;
                                            }
                                        }
                                    }
                                    Assessment += drAssessment["AssessmentDescription"] + " ";
                                }
                            }
                        }
                        dsCCDA.Merge(dtAssessment);
                    }
                    if (!bResultChiefCoomplaint)
                    {
                        if (dsCCDA.ProblemList.Rows.Count > 0)
                        {
                            if (EncounterSNOMEDIDs != null && EncounterSNOMEDIDs.Count() > 0)
                            {
                                foreach (DataRow drProblem in dsCCDA.ProblemList.Rows)
                                {
                                    foreach (string SNOMEDID in EncounterSNOMEDIDs)
                                    {
                                        if (SNOMEDID == drProblem["SNOMEDID"].ToString())
                                        {
                                            drProblem["IsChiefComplaint"] = true;
                                            drProblem["ComplaintComments"] = FollowUp;
                                            drProblem["IsProblem"] = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    DataTable dtNotesEncounter = dsCCDA.Tables[dsCCDA.NotesEncounter.TableName];
                    if (dtEncounter != null && dtEncounter.Rows.Count > 0)
                    {
                        AddEncounterProblems(dtNotesEncounter, Assessment);
                    }
                    #endregion Assessment

                    if (fileTypeCDDA.ToLower() == "ccd v3" || fileTypeCDDA.ToLower() == "referral note")
                    {
                        #region Functional Status
                        DataTable dtFunctionalStatus = dsCCDA.Tables[dsCCDA.FunctionalStatus.TableName];
                        DataTable dtMentalStatus = dsCCDA.Tables[dsCCDA.FunctionalStatus.TableName];
                        GetFunctionalStatusData(document, ref dtFunctionalStatus, ref errors, patientId);
                        if (dtFunctionalStatus.Rows.Count > 0)
                        {
                            dsCCDA.Merge(dtFunctionalStatus);
                        }
                        #endregion Functional Status

                        #region Mental Status
                        GetMentalStatusData(document, ref dtMentalStatus, ref errors, patientId);
                        if (dtMentalStatus.Rows.Count > 0)
                        {
                            dsCCDA.Merge(dtMentalStatus);
                        }
                        #endregion Mental Status

                        #region Reason For Referral

                        DataTable dtReasonForReferral = dsCCDA.Tables[dsCCDA.ReasonForReferral.TableName];
                        GetReasonForReferraltData(document, ref dtReasonForReferral, ref errors, patientId);
                        if (dtReasonForReferral.Rows.Count > 0)
                        {
                            dsCCDA.Merge(dtReasonForReferral);
                        }
                        #endregion Reason For Referral
                    }

                    //#region Insurance Provider

                    //DataTable dtInsuranceProvider = dsCCDA.Tables[dsCCDA.InsuranceProvider.TableName];
                    //GetInsuranceProviderData(document, ref dtInsuranceProvider, ref errors, patientId);
                    //if (dtInsuranceProvider.Rows.Count > 0)
                    //{
                    //    dsCCDA.Merge(dtInsuranceProvider);
                    //}
                    //#endregion Insurance Provider

                    #region Database Insert

                    strResult = _bllCCDAobj.ImportCCDAData(dsCCDA);

                    #endregion Database Insert
                }
                else
                {
                    throw new Exception("Patient does not exists.");
                }
            }
            else
            {
                throw new Exception("The selected file is not valid");
            }
            if (string.IsNullOrWhiteSpace(strResult))
            {
                strResult = "Data Imported successfully.";
            }

            return strResult;
        }

        public async Task<string> ValidateCCDAData(string xmlContent, List<string> sectionList, List<string> errors, string fileTypeCDDA, string DocfileType, string DocfileName, bool IsFile)
        {
            ClinicalDocument CCDA;
            DSPatient ds = new DSPatient();
            string strResult = string.Empty;
            string url = string.Empty;
            byte[] encodedString = null;
            try
            {
                if (fileTypeCDDA.ToLower() == "ccda")
                {
                    if (IsFile)
                    {
                        encodedString = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    else
                    {
                        byte[] byteArray2 = Encoding.UTF8.GetBytes(xmlContent);
                        byte[] byteArray = Convert.FromBase64String(Encoding.ASCII.GetString(byteArray2));
                        url = CommonFunc.SaveDocumentToFolder(null, "Import_CCDA", "Import_CCDA", 0, "ccda.xml", byteArray);
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        string url2 = FilePath + url;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(url2);
                        string xmlcontents = doc.InnerXml;
                        encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.2"))
                    {

                        strResult = await ValidateXML(encodedString, DocfileType, DocfileName, url);
                    }
                    else
                    {
                        throw new Exception("The selected file is not valid");
                    }
                }
                else if (fileTypeCDDA.ToLower() == "ccd v3")
                {
                    if (IsFile)
                    {
                        encodedString = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    else
                    {
                        byte[] byteArray2 = Encoding.UTF8.GetBytes(xmlContent);
                        byte[] byteArray = Convert.FromBase64String(Encoding.ASCII.GetString(byteArray2));
                        url = CommonFunc.SaveDocumentToFolder(null, "Import_CCDA", "Import_CCDA", 0, "ccda.xml", byteArray);
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        string url2 = FilePath + url;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(url2);
                        string xmlcontents = doc.InnerXml;
                        encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.2"))
                    {
                        strResult = await ValidateXML(encodedString, DocfileType, DocfileName, url);
                    }
                    else
                    {
                        throw new Exception("The selected file is not valid");
                    }
                }
                else if (fileTypeCDDA.ToLower() == "referral note")
                {
                    if (IsFile)
                    {
                        encodedString = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    else
                    {
                        byte[] byteArray2 = Encoding.UTF8.GetBytes(xmlContent);
                        byte[] byteArray = Convert.FromBase64String(Encoding.ASCII.GetString(byteArray2));
                        url = CommonFunc.SaveDocumentToFolder(null, "Import_CCDA", "Import_CCDA", 0, "ccda.xml", byteArray);
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        string url2 = FilePath + url;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(url2);
                        string xmlcontents = doc.InnerXml;
                        encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.14"))
                    {
                        strResult = await ValidateXML(encodedString, DocfileType, DocfileName, url);
                    }
                    else
                    {
                        throw new Exception("The selected file is not valid");
                    }
                }
                else if (fileTypeCDDA.ToLower() == "care plan")
                {
                    if (IsFile)
                    {
                        encodedString = Encoding.UTF8.GetBytes(xmlContent);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    else
                    {
                        byte[] byteArray2 = Encoding.UTF8.GetBytes(xmlContent);
                        byte[] byteArray = Convert.FromBase64String(Encoding.ASCII.GetString(byteArray2));
                        url = CommonFunc.SaveDocumentToFolder(null, "Import_CCDA", "Import_CCDA", 0, "ccda.xml", byteArray);
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        string url2 = FilePath + url;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(url2);
                        string xmlcontents = doc.InnerXml;
                        encodedString = Encoding.UTF8.GetBytes(xmlcontents);
                        CCDA = GetDeserializeDocument(encodedString, ref errors);
                    }
                    if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.15"))
                    {
                        strResult = await ValidateXML(encodedString, DocfileType, DocfileName, url);
                    }
                    else
                    {
                        throw new Exception("The selected file is not valid");
                    }
                }
                else
                {
                    throw new Exception("The selected file is not valid");
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return strResult;
        }

        private string GetHtmlFromXMLContent(string xmlData)
        {
            var folderPath = HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);

            var htmlfileName = "CCDA_{Guid.NewGuid()}.html";
            var xmlfileName = "CCDA_{Guid.NewGuid()}.xml";
            //var htmlFilePath = @"{folderPath}\{htmlfileName}";
            //var xmlFilePath = @"{folderPath}\{xmlfileName}";
            var xmlFilePath = Path.GetTempFileName();
            var htmlFilePath = Path.GetTempFileName();

            XslCompiledTransform transform = new XslCompiledTransform();
            XmlUrlResolver resolver = new XmlUrlResolver { Credentials = CredentialCache.DefaultCredentials };
            XsltSettings settings = new XsltSettings() { EnableDocumentFunction = true };
            // load up the stylesheet
            var stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\CCDA.xsl";
            transform.Load(stylesheetPath, settings, resolver);
            // perform the transformation
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);
            doc.Save(xmlFilePath);

            transform.Transform(xmlFilePath, htmlFilePath);
            string content = File.ReadAllText(htmlFilePath);
            content = content.Replace("�", " ");
            //Delete Xml & Html File

            if (File.Exists(xmlFilePath))
                File.Delete(xmlFilePath);
            if (File.Exists(htmlFilePath))
                File.Delete(htmlFilePath);
            return content;
        }

        public async Task<string> ValidateXML(byte[] byteArray, string DocfileType, string DocfileName, string url)
        {
            string responseResult = string.Empty;
            try
            {
                var EmpResponse = string.Empty;
                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();


                form.Add(new ByteArrayContent(byteArray, 0, byteArray.Length), "ccdaFile", "XMLData.xml");
                HttpResponseMessage response = await httpClient.PostAsync("https://ttpds.sitenv.org:8443/referenceccdaservice/?validationObjective=" + DocfileType + "&referenceFileName=" + DocfileName, form);
                if (response.IsSuccessStatusCode)
                {
                    HttpContent stream = response.Content;
                    var data = await stream.ReadAsStringAsync();
                    string dataHtml = string.Empty;
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                        string url2 = FilePath + url;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(url2);
                        string xmlcontents = doc.InnerXml;

                        string html = GetHtmlFromXMLContent(xmlcontents);
                        var htmlfileName = string.Format("{0}.html", Guid.NewGuid());
                        var folderPath = HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);
                        var htmlFilePath = string.Format(@"{0}\{1}", folderPath, htmlfileName);
                        //Save XML Content
                        File.WriteAllText(htmlFilePath, html);

                        dataHtml = MDVSession.Current.ImagePath.Replace("~/", string.Empty) + "/" + htmlfileName;
                    }

                    var output = new
                    {
                        status = true,
                        Message = data,
                        url = url,
                        dataHtml= dataHtml
                    };
                    responseResult = JsonConvert.SerializeObject(output);
                }
                else
                {
                    HttpContent stream = response.Content;
                    var data = await stream.ReadAsStringAsync();
                    var output = new
                    {
                        status = false,
                        Message = data,
                        url = url
                    };
                    responseResult = JsonConvert.SerializeObject(output);
                }
            }
            catch (HttpRequestException ex)
            {
                var output = new
                {
                    status = false,
                    Message = "An error occurred while sending the request.",
                    url = url
                };
                responseResult = JsonConvert.SerializeObject(output);
            }
            return responseResult;
        }

        public string ReconcilePrivacyData(string xmlContent, string FileName, ref List<string> errors)
        {
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(xmlContent);
                ClinicalDocument CCDA = GetDeserializeDocument(byteArray, ref errors);
                DSCCDA dsCCDAReconcile = new DSCCDA();
                DSPatient ds = new DSPatient();
                List<string> sectionList = new List<string>();
                if (CCDA.Validate("2.16.840.1.113883.10.20.22.1.2"))
                {
                    GetDocumentData(CCDA, ref dsCCDAReconcile, ref ds, ref sectionList, ref errors);
                }
                else
                {
                    throw new Exception("The selected file is not valid");
                }
                string ConfidentialityCode = MDVUtility.ToStr(ds.Patients.Rows[0][ds.Patients.ConfidentialityCodeColumn.ColumnName]);
                string IsDataPrivacy = _bllCCDAobj.GetIsDataPrivacy();
                if (ConfidentialityCode == "N" || (ConfidentialityCode == "R" && IsDataPrivacy == "True"))
                {
                    string ServerPath = ConfigurationManager.AppSettings["PatientFilesPath"];
                    int count = 1;

                    string DocumentFolder = string.Format("CCDA_PrivacySegmentedDocument");
                    if (!Directory.Exists(ServerPath + DocumentFolder))
                        Directory.CreateDirectory(ServerPath + DocumentFolder);

                    int idx = FileName.LastIndexOf('.');
                    string fileNameOnly = FileName.Substring(0, idx);// FileName.Split('.')[0];
                    string extension = FileName.Substring(idx + 1);// FileName.Split('.')[1];

                    var newFullPath = string.Format(DocumentFolder + "\\{0}.{1}", fileNameOnly, extension);
                    while (File.Exists(ServerPath + newFullPath))
                    {
                        string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                        newFullPath = Path.Combine(DocumentFolder, tempFileName + "." + extension);
                    }
                    File.WriteAllBytes(ServerPath + newFullPath, byteArray);
                    ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.FilePathColumn] = newFullPath;
                    ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.CreatedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    ds.Tables[ds.Patients.TableName].Rows[0][ds.Patients.ModifiedByColumn] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);

                    #region Database Insertion
                    BLObject<DSPatient> obj = _bllCCDAobj.InsertPrivacySegmentedDocument(ds);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = "The file has been imported successfully.",
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "You are not authorized to upload this document as it is privacy protected."
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string LoadPrivacySegmentedDocument(long Id, long EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = _bllCCDAobj.LoadPrivacySegmentedDocument(Id, EntityId, PageNumber, RowsPerPage);
                dsPatient = obj.Data;
                if (obj != null && obj.Data != null)
                {
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PrivacySegmentedDocumentCount = dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count,
                            PrivacySegmentedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]),
                            iTotalDisplayRecords = dsPatient.Patients.Rows[0][dsPatient.Patients.RecordCountColumn.ColumnName],
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PrivacySegmentedDocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PrivacySegmentedDocumentCount = 0,
                        Message = "Record not found."
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string GetPrivacySegmentedDocumentHTML(string UrlPath, string ConfidentialityCode)
        {
            if (!String.IsNullOrEmpty(UrlPath))
            {
                string IsDataPrivacy = _bllCCDAobj.GetIsDataPrivacy();
                if (ConfidentialityCode == "N" || (ConfidentialityCode == "R" && IsDataPrivacy == "True"))
                {
                    byte[] file;
                    string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"];
                    UrlPath = FilePath + UrlPath;
                    using (var stream = new FileStream(UrlPath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            file = reader.ReadBytes((int)stream.Length);
                        }
                    }
                    if (file != null)
                    {
                        XmlDocument doc = new XmlDocument();
                        string xml = Encoding.UTF8.GetString(file);
                        doc.LoadXml(xml);

                        return xml;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        #region Patient Data

        private void GetPatientData(ClinicalDocument document, ref DataTable dtPatient, ref List<string> errors, DSClinicalSummary dsPatientLookUp)
        {
            var dsPatient = new DSPatient();
            var patientRole = document.GetPatientRole();
            if (patientRole == null) return;
            var dr = dtPatient.NewRow();

            #region Patient

            var p = patientRole.patient;
            if (p != null)
            {

                #region DOB
                try
                {
                    if (!string.IsNullOrEmpty(p.birthTime.value) && p.birthTime.value.Length >= 8)
                    {
                        if (!string.IsNullOrEmpty(p.birthTime.value))
                        {
                            var dob = p.birthTime.value.Substring(0, 8);
                            dr[dsPatient.Patients.DOBColumn.ColumnName] = dob.ToFormatedDateTimeByFormat();
                        }
                    }
                }
                catch (Exception exc)
                {
                    errors.Add("Patient DOB: " + exc.Message);
                }

                #endregion

                #region Name
                try
                {
                    if (!p.name.IsNullOrEmpty() && !p.name[0].Items.IsNullOrEmpty())
                    {
                        USRealmName name = GetUsRealmNameWithOutPreviousName(p.name[0], ref errors);

                        dr[dsPatient.Patients.LastNameColumn.ColumnName] = name.LastName;
                        dr[dsPatient.Patients.FirstNameColumn.ColumnName] = name.FirstName;
                        dr[dsPatient.Patients.MIColumn.ColumnName] = name.MiddleName;
                        //if(!string.IsNullOrWhiteSpace(name.PreviousName))
                        //{
                        //    dr[dsPatient.Patients.PreviousNameColumn.ColumnName] = name.PreviousName;
                        //}
                    }
                    if (!p.name.IsNullOrEmpty() && p.name.Count() > 1 && !p.name[1].Items.IsNullOrEmpty())
                    {
                        USRealmName name2 = GetUsRealmName(p.name[1], ref errors);
                        if (!string.IsNullOrWhiteSpace(name2.PreviousName))
                        {
                            dr[dsPatient.Patients.PreviousNameColumn.ColumnName] = name2.PreviousName;
                        }
                    }
                    if (!p.name.IsNullOrEmpty() && p.name.Count() > 2 && !p.name[2].Items.IsNullOrEmpty())
                    {
                        USRealmName name3 = GetUsRealmName(p.name[2], ref errors);
                        if (!string.IsNullOrWhiteSpace(name3.PreviousName))
                        {
                            dr[dsPatient.Patients.PreviousNameColumn.ColumnName] = name3.PreviousName;
                        }
                    }
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Name: " + exc.Message);
                }
                #endregion

                #region Race
                try
                {
                    StringBuilder strRaceIds = new StringBuilder();
                    if (p.raceCode != null && !string.IsNullOrEmpty(p.raceCode.code))
                    {
                        if (!string.IsNullOrWhiteSpace(strRaceIds.ToString()))
                            strRaceIds.Append(",");
                        dr[dsPatient.Patients.RaceIdColumn.ColumnName] = GetRaceIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Race.TableName], MDVUtility.ToStr(p.raceCode.code));
                        strRaceIds.Append(GetRaceIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Race.TableName], MDVUtility.ToStr(p.raceCode.code)));
                    }
                    if (!p.raceCode1.IsNullOrEmpty())
                    {
                        foreach (var race in p.raceCode1)
                        {
                            if (!string.IsNullOrWhiteSpace(strRaceIds.ToString()))
                                strRaceIds.Append(",");
                            strRaceIds.Append(GetRaceIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Race.TableName], MDVUtility.ToStr(race.code)));
                            if (dr[dsPatient.Patients.RaceIdColumn.ColumnName] == DBNull.Value)
                            {
                                dr[dsPatient.Patients.RaceIdColumn.ColumnName] = GetRaceIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Race.TableName], MDVUtility.ToStr(race.code));
                            }
                        }
                    }
                    else if (!p.raceCode2.IsNullOrEmpty())
                    {
                        foreach (var race in p.raceCode2)
                        {
                            if (!string.IsNullOrWhiteSpace(strRaceIds.ToString()))
                                strRaceIds.Append(",");
                            strRaceIds.Append(GetRaceIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Race.TableName], MDVUtility.ToStr(race.code)));
                            if (dr[dsPatient.Patients.RaceIdColumn.ColumnName] == DBNull.Value)
                            {
                                dr[dsPatient.Patients.RaceIdColumn.ColumnName] = GetRaceIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Race.TableName], MDVUtility.ToStr(race.code));
                            }
                        }
                    }
                    if (strRaceIds != null)
                    {
                        dr[dsPatient.Patients.strRaceIdsColumn.ColumnName] = strRaceIds.ToString();
                    }
                    if (dr[dsPatient.Patients.RaceIdColumn.ColumnName] == DBNull.Value)
                    {
                        dr[dsPatient.Patients.RaceIdColumn.ColumnName] = "5";
                    }
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Race: " + exc.Message);
                }
                #endregion

                #region Gender
                try
                {
                    if (p.administrativeGenderCode != null && !string.IsNullOrEmpty(p.administrativeGenderCode.code))
                        dr[dsPatient.Patients.GenderColumn.ColumnName] = GetGender(p.administrativeGenderCode.code);
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Gender: " + exc.Message);
                }

                #endregion

                #region Address
                try
                {
                    if (!patientRole.addr.IsNullOrEmpty() && patientRole.addr[0] != null
                        && !patientRole.addr[0].Items.IsNullOrEmpty())
                    {
                        List<ADXP> addList = patientRole.addr[0].Items;
                        foreach (ADXP adxp in addList.Where(adxp => adxp != null && !adxp.Text.IsNullOrEmpty() && !string.IsNullOrEmpty(adxp.Text[0])))
                        {
                            if (adxp is adxpstate)
                            {
                                dr[dsPatient.Patients.StateColumn.ColumnName] = adxp.Text[0].ToUpper();
                            }
                            else if (adxp is adxpcity)
                            {
                                dr[dsPatient.Patients.CityColumn.ColumnName] = adxp.Text[0].ToUpper();
                            }
                            else if (adxp is adxppostalCode)
                            {
                                dr[dsPatient.Patients.ZIPCodeColumn.ColumnName] = adxp.Text[0].ToUpper();
                            }
                            else if (adxp is adxpstreetAddressLine)
                            {
                                var streetAdd = adxp.Text.Where(str => !string.IsNullOrEmpty(str)).Aggregate("", (current, str) => current + " " + str.Trim());
                                dr[dsPatient.Patients.Address1Column.ColumnName] = streetAdd.ToUpper();
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Address: " + exc.Message);
                }

                #endregion

                #region Ethnicity
                try
                {
                    StringBuilder strEthnicityIds = new StringBuilder();
                    if (p.ethnicGroupCode != null && !string.IsNullOrEmpty(p.ethnicGroupCode.code))
                    {
                        dr[dsPatient.Patients.EthnicityIdColumn.ColumnName] = GetEthnicityIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Ethnicity.TableName], MDVUtility.ToStr(p.ethnicGroupCode.code));
                        strEthnicityIds.Append(GetEthnicityIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Ethnicity.TableName], MDVUtility.ToStr(p.ethnicGroupCode.code)));
                    }
                    else
                        dr[dsPatient.Patients.EthnicityIdColumn.ColumnName] = "4"; // Unknown
                    if (!p.ethnicGroupCode1.IsNullOrEmpty())
                    {
                        foreach (var ethnicity in p.ethnicGroupCode1)
                        {
                            strEthnicityIds.Append("," + GetEthnicityIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Ethnicity.TableName], MDVUtility.ToStr(ethnicity.code)));
                        }
                    }
                    if (strEthnicityIds != null)
                    {
                        dr[dsPatient.Patients.strEthnicityIdsColumn.ColumnName] = strEthnicityIds.ToString();
                    }
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Ethnicity: " + exc.Message);
                }
                #endregion

                #region Martial Status
                try
                {
                    if (p.maritalStatusCode != null && !string.IsNullOrEmpty(p.maritalStatusCode.code))
                    {
                        dr[dsPatient.Patients.MaritialStatusColumn.ColumnName] = GetMaritalStatusCode(p.maritalStatusCode.code);
                    }
                    else
                        dr[dsPatient.Patients.MaritialStatusColumn.ColumnName] = "Unknown";
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Marital Status: " + exc.Message);
                }

                #endregion

                #region Language Communication
                try
                {
                    if (!p.languageCommunication.IsNullOrEmpty() && !string.IsNullOrEmpty(p.languageCommunication[0].languageCode.code))
                        dr[dsPatient.Patients.PrefLanguageIdColumn.ColumnName] = GetLanguageIdFromCode(dsPatientLookUp.Tables[dsPatientLookUp.Languages.TableName], MDVUtility.ToStr(p.languageCommunication[0].languageCode.code));
                    else
                        dr[dsPatient.Patients.PrefLanguageIdColumn.ColumnName] = "146";
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Language: " + exc.Message);
                }
                #endregion

                #region TeleCommunication
                try
                {
                    if (!patientRole.telecom.IsNullOrEmpty() && !string.IsNullOrEmpty(patientRole.telecom[0].value))
                    {
                        string PhoneNo1 = string.Empty;
                        string PhoneNo2 = string.Empty;
                        foreach (TEL tel in patientRole.telecom)
                        {
                            if (!tel.use.IsNullOrEmpty() && tel.use.Count() > 0 && tel.use[0] == "HP")
                            {
                                if (!string.IsNullOrWhiteSpace(tel.value))
                                    PhoneNo1 = GetTeleCommunication(tel.value);
                            }
                            else
                            {
                                string mp = GetTeleCommunication(tel.value);
                                if (!string.IsNullOrWhiteSpace(mp))
                                {
                                    if (mp.Length <= 15)
                                    {
                                        PhoneNo2 = GetTeleCommunication(mp);
                                    }
                                    else
                                    {
                                        int idx = tel.value.LastIndexOf('-');
                                        dr[dsPatient.Patients.WorkPhoneExtColumn.ColumnName] = tel.value.Substring(idx + 1);
                                        PhoneNo2 = GetTeleCommunication(tel.value.Substring(0, idx));
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(PhoneNo1))
                        {
                            dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName] = PhoneNo1;
                            if (!string.IsNullOrWhiteSpace(PhoneNo2))
                                dr[dsPatient.Patients.WorkPhoneNoColumn.ColumnName] = PhoneNo2;

                        }
                        else if (!string.IsNullOrWhiteSpace(PhoneNo2))
                        {
                            dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName] = PhoneNo2;
                        }
                    }
                }
                catch (Exception exc)
                {
                    errors.Add("Patient Telephone No: " + exc.Message);
                }
                #endregion
            }

            var intendedRecipient = document.GetIntendedRecipient();
            if (intendedRecipient != null)
            {
                Person person = intendedRecipient.informationRecipient;
                if (!person.name.IsNullOrEmpty() && !person.name[0].Items.IsNullOrEmpty())
                {
                    USRealmName name = GetUsRealmNameWithOutPreviousName(person.name[0], ref errors);

                    dr[dsPatient.Patients.RefLastNameColumn.ColumnName] = name.LastName;
                    dr[dsPatient.Patients.RefFirstNameColumn.ColumnName] = name.FirstName;
                }
            }

            #endregion

            #region Practice/Provider/Facility

            try
            {
                dr[dsPatient.Patients.CreatedByColumn.ColumnName] = CreatedBy;
                dr[dsPatient.Patients.CreatedOnColumn.ColumnName] = CreatedOn;
                dr[dsPatient.Patients.ModifiedByColumn.ColumnName] = ModifiedBy;
                dr[dsPatient.Patients.ModifiedOnColumn.ColumnName] = ModifiedOn;
                dr[dsPatient.Patients.CommunicatewithGuarantorColumn.ColumnName] = false;
                dr[dsPatient.Patients.CommunicationOptoutColumn.ColumnName] = false;
                dr[dsPatient.Patients.ProviderIdColumn.ColumnName] = MDVUtility.ToStr(ProviderId);
                dr[dsPatient.Patients.EntityIdColumn.ColumnName] = MDVUtility.ToStr(EntityId);
                dr[dsPatient.Patients.IsActiveColumn.ColumnName] = true;
                if (!string.IsNullOrWhiteSpace(FacilityId))
                    dr[dsPatient.Patients.FacilityIdColumn.ColumnName] = MDVUtility.ToStr(FacilityId);
                if (!string.IsNullOrWhiteSpace(PracticeId))
                    dr[dsPatient.Patients.PracticeIdColumn.ColumnName] = MDVUtility.ToStr(PracticeId);
            }
            catch (Exception ex)
            {
                errors.Add("Patient Provider: " + ex.Message);
            }

            #endregion

            dtPatient.Rows.Add(dr);
        }

        private static USRealmName GetUsRealmNameWithOutPreviousName(PN personName, ref List<string> errors)
        {
            var name = new USRealmName();
            if (personName == null) return name;
            foreach (ENXP item in personName.Items.Where(item => item != null && !item.Text.IsNullOrEmpty() && !string.IsNullOrEmpty(item.Text[0])))
            {
                if (item is enfamily)
                {
                    name.LastName = item.Text[0].ToUpper();
                }
                else if (item is engiven)
                {
                    if (string.IsNullOrEmpty(name.FirstName))
                    {
                        name.FirstName = item.Text[0].ToUpper();
                    }
                    else if (string.IsNullOrEmpty(name.MiddleName))
                    {
                        name.MiddleName = item.Text[0].ToUpper();
                    }
                }
            }
            return name;
        }

        private static USRealmName GetUsRealmName(PN personName, ref List<string> errors)
        {
            var name = new USRealmName();
            if (personName == null) return name;
            foreach (ENXP item in personName.Items.Where(item => item != null && !item.Text.IsNullOrEmpty() && !string.IsNullOrEmpty(item.Text[0])))
            {
                if (item is enfamily)
                {
                    name.LastName = item.Text[0].ToUpper();
                }
                else if (item is engiven)
                {
                    if (!item.qualifier.IsNullOrEmpty() && item.qualifier.Count() > 0 && item.qualifier[0].ToLower() == "br")
                    {
                        name.PreviousName = item.Text[0].ToUpper();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(name.FirstName))
                        {
                            name.FirstName = item.Text[0].ToUpper();
                        }
                        else
                        {
                            name.MiddleName = item.Text[0].ToUpper();
                        }
                    }
                }
            }
            return name;
        }

        private static string GetRaceIdFromCode(DataTable dataTable, string raceCode)
        {
            string raceId = string.Empty;
            var ds = new DSClinicalSummary();
            foreach (var row in dataTable.Rows.Cast<DataRow>().Where(row => MDVUtility.ToStr(row[ds.Race.CodeColumn.ColumnName]) == raceCode))
            {
                raceId = MDVUtility.ToStr(row[ds.Race.IdColumn.ColumnName]);
                break;
            }
            return raceId;
        }

        private static string GetGender(string gender)
        {
            var sex = "Unknown";
            switch (gender.ToUpper())
            {
                case "M":
                    sex = "Male";
                    break;
                case "F":
                    sex = "Female";
                    break;
                case "UN":
                    sex = "Unknown";
                    break;
            }
            return sex;
        }

        private static Int64 GetEthnicityIdFromCode(DataTable dataTable, string ethnicityCode)
        {
            Int64 ethnicityId = 4;
            var ds = new DSClinicalSummary();
            foreach (var row in dataTable.Rows.Cast<DataRow>().Where(row => MDVUtility.ToStr(row[ds.Ethnicity.CodeColumn.ColumnName]) == ethnicityCode))
            {
                ethnicityId = MDVUtility.ToInt64(row[ds.Ethnicity.IdColumn.ColumnName]);
                break;
            }
            return ethnicityId;
        }

        public static string GetMaritalStatusCode(string maritalStatus)
        {
            var returnVal = string.Empty;
            switch (maritalStatus.ToUpper())
            {
                case "D":
                    returnVal = "Divorced";
                    break;
                case "M":
                    returnVal = "Married";
                    break;
                case "P":
                    returnVal = "Partner";
                    break;
                case "S":
                    returnVal = "Single";
                    break;
                case "W":
                    returnVal = "Widowed";
                    break;
                case "L":
                    returnVal = "Legally separated";
                    break;
            }
            return returnVal;
        }

        private static Int64 GetLanguageIdFromCode(DataTable dataTable, string languageCode)
        {
            Int64 languageId = 146;
            var ds = new DSClinicalSummary();
            foreach (var row in dataTable.Rows.Cast<DataRow>().Where(row => MDVUtility.ToStr(row[ds.Languages.CodeColumn.ColumnName]) == languageCode))
            {
                languageId = MDVUtility.ToInt64(row[ds.Languages.IdColumn.ColumnName]);
                break;
            }
            return languageId;
        }

        public static string GetTeleCommunication(string teleComm)
        {
            teleComm = teleComm.Trim();
            var straight = teleComm.Replace("tel:+1", "").Replace("tel: 1-", "").Replace("tel: 1", "").Replace("-", "").Replace("tel:", "");
            if (straight.Length == 10)
            {
                straight = string.Format("{0:(###) ###-####}", straight); //"{straight:0(###) ###-####}";
            }
            return straight;
        }

        public class PatientPreliminary
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PreviousName { get; set; }
            public DateTime? DOB { get; set; }
        }
        public class USRealmName : PatientPreliminary
        {
            public string MiddleName { get; set; }
            public string Prefix { get; set; }
        }

        #endregion Patient Data

        #region Care Team Members

        private static void GetCareTeamMembersData(ClinicalDocument document, ref DataTable dtCareTeamMembers, ref List<string> errors, long patientId)
        {
            if (document.documentationOf != null && document.documentationOf[0] != null)
            {
                if (document.documentationOf[0].serviceEvent is ServiceEvent)
                {
                    ServiceEvent serviceEvent = document.documentationOf[0].serviceEvent as ServiceEvent;
                    if (serviceEvent.performer != null)
                    {
                        foreach (Performer1 performer in serviceEvent.performer)
                        {
                            DataRow dr = dtCareTeamMembers.NewRow();
                            dr[dsCCDA.CareTeamMembers.PatientIdColumn.ColumnName] = patientId;
                            dr[dsCCDA.CareTeamMembers.CareTeamMemberIdColumn.ColumnName] = careTeamCounter--;
                            if (performer.functionCode != null && !string.IsNullOrWhiteSpace(performer.functionCode.code))
                            {
                                dr[dsCCDA.CareTeamMembers.CareProviderTypeCodeColumn.ColumnName] = performer.functionCode.code;
                                dr[dsCCDA.CareTeamMembers.CareProviderTypeColumn.ColumnName] = performer.functionCode.displayName;
                            }
                            if (performer.assignedEntity != null)
                            {
                                AssignedEntity assignedEntity = performer.assignedEntity;

                                if (assignedEntity.code != null && !string.IsNullOrWhiteSpace(assignedEntity.code.code))
                                {
                                    dr[dsCCDA.CareTeamMembers.MemberCodeColumn.ColumnName] = assignedEntity.code.code;
                                    dr[dsCCDA.CareTeamMembers.MemberCodeSystemColumn.ColumnName] = assignedEntity.code.codeSystemName;
                                    dr[dsCCDA.CareTeamMembers.MemberCodeNameColumn.ColumnName] = assignedEntity.code.displayName;
                                }
                                #region Name
                                try
                                {
                                    if (assignedEntity.assignedPerson != null && assignedEntity.assignedPerson.name[0] != null)
                                    {
                                        USRealmName name = GetUsRealmNameCareTeam(assignedEntity.assignedPerson.name[0], ref errors);
                                        dr[dsCCDA.CareTeamMembers.PrefixColumn.ColumnName] = name.Prefix;
                                        dr[dsCCDA.CareTeamMembers.FirstNameColumn.ColumnName] = name.FirstName;
                                        dr[dsCCDA.CareTeamMembers.MIColumn.ColumnName] = name.MiddleName;
                                        dr[dsCCDA.CareTeamMembers.LastNameColumn.ColumnName] = name.LastName;
                                    }
                                }
                                catch (Exception exc)
                                {
                                    errors.Add("Care Team Members - Assigned Person: " + exc.Message);
                                }
                                #endregion
                                #region Address
                                try
                                {
                                    if (!assignedEntity.addr.IsNullOrEmpty() && assignedEntity.addr[0] != null
                                        && !assignedEntity.addr[0].Items.IsNullOrEmpty())
                                    {
                                        List<ADXP> addList = assignedEntity.addr[0].Items;
                                        foreach (ADXP adxp in addList.Where(adxp => adxp != null && !adxp.Text.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(adxp.Text[0])))
                                        {
                                            if (adxp is adxpstate)
                                            {
                                                dr[dsCCDA.CareTeamMembers.StateColumn.ColumnName] = adxp.Text[0].ToUpper();
                                            }
                                            else if (adxp is adxpcity)
                                            {
                                                dr[dsCCDA.CareTeamMembers.CityColumn.ColumnName] = adxp.Text[0].ToUpper();
                                            }
                                            else if (adxp is adxppostalCode)
                                            {
                                                dr[dsCCDA.CareTeamMembers.ZIPCodeColumn.ColumnName] = adxp.Text[0].ToUpper();
                                            }
                                            else if (adxp is adxpstreetAddressLine)
                                            {
                                                var streetAdd = adxp.Text.Where(str => !string.IsNullOrWhiteSpace(str)).Aggregate("", (current, str) => current + " " + str.Trim());
                                                dr[dsCCDA.CareTeamMembers.Address1Column.ColumnName] = streetAdd.ToUpper();
                                            }
                                        }
                                    }
                                }
                                catch (Exception exc)
                                {
                                    errors.Add("Care Team Members - Address: " + exc.Message);
                                }

                                #endregion

                                #region Telephone
                                try
                                {
                                    if (!assignedEntity.telecom.IsNullOrEmpty() && !string.IsNullOrEmpty(assignedEntity.telecom[0].value))
                                    {
                                        dr[dsCCDA.CareTeamMembers.HomePhoneNoColumn.ColumnName] = GetTeleCommunication(assignedEntity.telecom[0].value);
                                    }
                                }
                                catch (Exception exc)
                                {
                                    errors.Add("Care Team Members - Telecom: " + exc.Message);
                                }
                                #endregion Telephone

                                #region Organization
                                if (assignedEntity.representedOrganization != null && assignedEntity.representedOrganization is Organization)
                                {
                                    Organization organization = assignedEntity.representedOrganization as Organization;
                                    if (organization.name != null)
                                    {
                                        foreach (ON orgName in organization.name)
                                        {
                                            dr[dsCCDA.CareTeamMembers.OrganizationColumn.ColumnName] = orgName.Text[0];
                                        }

                                        #region Telephone
                                        try
                                        {
                                            if (!organization.telecom.IsNullOrEmpty() && !string.IsNullOrEmpty(organization.telecom[0].value))
                                            {
                                                dr[dsCCDA.CareTeamMembers.OrganizationTelephoneColumn.ColumnName] = GetTeleCommunication(organization.telecom[0].value);
                                            }
                                        }
                                        catch (Exception exc)
                                        {
                                            errors.Add("Care Team members - Organization Telecom: " + exc.Message);
                                        }
                                        #endregion Telephone

                                        #region Address
                                        try
                                        {
                                            if (!assignedEntity.addr.IsNullOrEmpty() && assignedEntity.addr[0] != null
                                                && !assignedEntity.addr[0].Items.IsNullOrEmpty())
                                            {
                                                List<ADXP> addList = assignedEntity.addr[0].Items;
                                                foreach (ADXP adxp in addList.Where(adxp => adxp != null && !adxp.Text.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(adxp.Text[0])))
                                                {
                                                    if (adxp is adxpstate)
                                                    {
                                                        dr[dsCCDA.CareTeamMembers.OrganizationStateColumn.ColumnName] = adxp.Text[0].ToUpper();
                                                    }
                                                    else if (adxp is adxpcity)
                                                    {
                                                        dr[dsCCDA.CareTeamMembers.OrganizationCityColumn.ColumnName] = adxp.Text[0].ToUpper();
                                                    }
                                                    else if (adxp is adxppostalCode)
                                                    {
                                                        dr[dsCCDA.CareTeamMembers.OrganizationZIPCodeColumn.ColumnName] = adxp.Text[0].ToUpper();
                                                    }
                                                    else if (adxp is adxpstreetAddressLine)
                                                    {
                                                        var streetAdd = adxp.Text.Where(str => !string.IsNullOrWhiteSpace(str)).Aggregate("", (current, str) => current + " " + str.Trim());
                                                        dr[dsCCDA.CareTeamMembers.OrganizationAddress1Column.ColumnName] = streetAdd.ToUpper();
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception exc)
                                        {
                                            errors.Add("Care Team Members - Organization Address: " + exc.Message);
                                        }
                                        #endregion
                                    }
                                }
                                #endregion Organization
                            }
                            dr[dsCCDA.CareTeamMembers.CreatedByColumn.ColumnName] = CreatedBy;
                            dr[dsCCDA.CareTeamMembers.CreatedOnColumn.ColumnName] = CreatedOn;
                            dr[dsCCDA.CareTeamMembers.ModifiedByColumn.ColumnName] = ModifiedBy;
                            dr[dsCCDA.CareTeamMembers.ModifiedOnColumn.ColumnName] = ModifiedOn;
                            dr[dsCCDA.CareTeamMembers.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                            dr[dsCCDA.CareTeamMembers.EntityIdColumn.ColumnName] = Convert.ToInt64(EntityId);
                            dtCareTeamMembers.Rows.Add(dr);
                        }
                    }
                }
            }
        }

        private static USRealmName GetUsRealmNameCareTeam(PN personName, ref List<string> errors)
        {
            var name = new USRealmName();
            if (personName == null) return name;
            foreach (ENXP item in personName.Items.Where(item => item != null && !item.Text.IsNullOrEmpty() && !string.IsNullOrEmpty(item.Text[0])))
            {
                if (item is enfamily)
                {
                    name.LastName = item.Text[0].ToUpper();
                }
                else if (item is engiven)
                {
                    if (string.IsNullOrEmpty(name.FirstName))
                    {
                        name.FirstName = item.Text[0].ToUpper();
                    }
                    else
                    {
                        name.MiddleName = item.Text[0].ToUpper();
                    }
                }
                else if (item is enprefix)
                {
                    name.Prefix = item.Text[0].ToUpper();
                }
            }
            return name;
        }

        private static void GetCareGiversData(ClinicalDocument document, ref DataTable dtCareTeamMembers, ref List<string> errors, long patientId)
        {
            if (!document.participant.IsNullOrEmpty())
            {
                foreach (Participant1 participant in document.participant)
                {
                    DataRow dr = dtCareTeamMembers.NewRow();
                    dr[dsCCDA.CareTeamMembers.PatientIdColumn.ColumnName] = patientId;
                    dr[dsCCDA.CareTeamMembers.CareTeamMemberIdColumn.ColumnName] = careTeamCounter--;
                    if (participant.associatedEntity != null && !string.IsNullOrWhiteSpace(participant.associatedEntity.classCode) && participant.associatedEntity.classCode.ToLower() == "caregiver")
                    {
                        AssociatedEntity associatedEntity = participant.associatedEntity;
                        if (associatedEntity.code != null && !string.IsNullOrWhiteSpace(associatedEntity.code.code))
                        {
                            dr[dsCCDA.CareTeamMembers.MemberCodeColumn.ColumnName] = associatedEntity.code.code;
                            dr[dsCCDA.CareTeamMembers.MemberCodeSystemColumn.ColumnName] = associatedEntity.code.codeSystemName;
                            dr[dsCCDA.CareTeamMembers.MemberCodeNameColumn.ColumnName] = "CAREGIVER";
                        }
                        else
                        {
                            dr[dsCCDA.CareTeamMembers.MemberCodeNameColumn.ColumnName] = "CAREGIVER";
                        }
                        #region Name
                        try
                        {
                            if (associatedEntity.associatedPerson != null && associatedEntity.associatedPerson.name[0] != null)
                            {
                                USRealmName name = GetUsRealmNameCareTeam(associatedEntity.associatedPerson.name[0], ref errors);
                                dr[dsCCDA.CareTeamMembers.PrefixColumn.ColumnName] = name.Prefix;
                                dr[dsCCDA.CareTeamMembers.FirstNameColumn.ColumnName] = name.FirstName;
                                dr[dsCCDA.CareTeamMembers.MIColumn.ColumnName] = name.MiddleName;
                                dr[dsCCDA.CareTeamMembers.LastNameColumn.ColumnName] = name.LastName;
                            }
                        }
                        catch (Exception exc)
                        {
                            errors.Add("Care Giver - Assiciated Person: " + exc.Message);
                        }
                        #endregion
                        #region Address
                        try
                        {
                            if (!associatedEntity.addr.IsNullOrEmpty() && associatedEntity.addr[0] != null
                                && !associatedEntity.addr[0].Items.IsNullOrEmpty())
                            {
                                List<ADXP> addList = associatedEntity.addr[0].Items;
                                foreach (ADXP adxp in addList.Where(adxp => adxp != null && !adxp.Text.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(adxp.Text[0])))
                                {
                                    if (adxp is adxpstate)
                                    {
                                        dr[dsCCDA.CareTeamMembers.StateColumn.ColumnName] = adxp.Text[0].ToUpper();
                                    }
                                    else if (adxp is adxpcity)
                                    {
                                        dr[dsCCDA.CareTeamMembers.CityColumn.ColumnName] = adxp.Text[0].ToUpper();
                                    }
                                    else if (adxp is adxppostalCode)
                                    {
                                        dr[dsCCDA.CareTeamMembers.ZIPCodeColumn.ColumnName] = adxp.Text[0].ToUpper();
                                    }
                                    else if (adxp is adxpstreetAddressLine)
                                    {
                                        var streetAdd = adxp.Text.Where(str => !string.IsNullOrWhiteSpace(str)).Aggregate("", (current, str) => current + " " + str.Trim());
                                        dr[dsCCDA.CareTeamMembers.Address1Column.ColumnName] = streetAdd.ToUpper();
                                    }
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            errors.Add("Care Giver - Address: " + exc.Message);
                        }

                        #endregion
                        #region Telephone
                        try
                        {
                            if (!associatedEntity.telecom.IsNullOrEmpty() && !string.IsNullOrEmpty(associatedEntity.telecom[0].value))
                            {
                                dr[dsCCDA.CareTeamMembers.HomePhoneNoColumn.ColumnName] = GetTeleCommunication(associatedEntity.telecom[0].value);
                            }
                        }
                        catch (Exception exc)
                        {
                            errors.Add("Care Giver - Telecom: " + exc.Message);
                        }
                        #endregion Telephone
                        #region Organization
                        if (associatedEntity.scopingOrganization != null && associatedEntity.scopingOrganization is Organization)
                        {
                            Organization organization = associatedEntity.scopingOrganization as Organization;
                            if (organization.name != null)
                            {
                                foreach (ON orgName in organization.name)
                                {
                                    dr[dsCCDA.CareTeamMembers.OrganizationColumn.ColumnName] = orgName.Text[0];
                                }
                                #region Telephone
                                try
                                {
                                    if (!organization.telecom.IsNullOrEmpty() && !string.IsNullOrEmpty(organization.telecom[0].value))
                                    {
                                        dr[dsCCDA.CareTeamMembers.OrganizationTelephoneColumn.ColumnName] = GetTeleCommunication(organization.telecom[0].value);
                                    }
                                }
                                catch (Exception exc)
                                {
                                    errors.Add("Care Giver - Organization Telecom: " + exc.Message);
                                }
                                #endregion Telephone
                                #region Address
                                try
                                {
                                    if (!associatedEntity.addr.IsNullOrEmpty() && associatedEntity.addr[0] != null
                                        && !associatedEntity.addr[0].Items.IsNullOrEmpty())
                                    {
                                        List<ADXP> addList = associatedEntity.addr[0].Items;
                                        foreach (ADXP adxp in addList.Where(adxp => adxp != null && !adxp.Text.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(adxp.Text[0])))
                                        {
                                            if (adxp is adxpstate)
                                            {
                                                dr[dsCCDA.CareTeamMembers.OrganizationStateColumn.ColumnName] = adxp.Text[0].ToUpper();
                                            }
                                            else if (adxp is adxpcity)
                                            {
                                                dr[dsCCDA.CareTeamMembers.OrganizationCityColumn.ColumnName] = adxp.Text[0].ToUpper();
                                            }
                                            else if (adxp is adxppostalCode)
                                            {
                                                dr[dsCCDA.CareTeamMembers.OrganizationZIPCodeColumn.ColumnName] = adxp.Text[0].ToUpper();
                                            }
                                            else if (adxp is adxpstreetAddressLine)
                                            {
                                                var streetAdd = adxp.Text.Where(str => !string.IsNullOrWhiteSpace(str)).Aggregate("", (current, str) => current + " " + str.Trim());
                                                dr[dsCCDA.CareTeamMembers.OrganizationAddress1Column.ColumnName] = streetAdd.ToUpper();
                                            }
                                        }
                                    }
                                }
                                catch (Exception exc)
                                {
                                    errors.Add("Care Giver - Organization Address: " + exc.Message);
                                }

                                #endregion
                            }
                        }
                        #endregion Organization

                        dr[dsCCDA.CareTeamMembers.CreatedByColumn.ColumnName] = CreatedBy;
                        dr[dsCCDA.CareTeamMembers.CreatedOnColumn.ColumnName] = CreatedOn;
                        dr[dsCCDA.CareTeamMembers.ModifiedByColumn.ColumnName] = ModifiedBy;
                        dr[dsCCDA.CareTeamMembers.ModifiedOnColumn.ColumnName] = ModifiedOn;
                        dr[dsCCDA.CareTeamMembers.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                        dr[dsCCDA.CareTeamMembers.EntityIdColumn.ColumnName] = Convert.ToInt64(EntityId);
                        dtCareTeamMembers.Rows.Add(dr);
                    }
                }
            }
        }
        #endregion Care Team Members

        #region Allergies

        private static void GetAllergiesData(ClinicalDocument document, ref DataTable dtAllergies, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 AllergiesComp = new Component3();
                List<Entry> entryList = null;
                bool isComponentExist = document.GetComponentByTemplateId(ref AllergiesComp, DocumentSections.Allergies);
                if (isComponentExist && AllergiesComp.GetEntry(ref entryList))
                {
                    foreach (Entry e in entryList)
                    {
                        Observation allergyIntoleranceObservation = null;
                        if (e != null && e.Item != null && e.Item is Act)
                        {
                            DataRow dr = dtAllergies.NewRow();
                            dr[dsCCDA.Allergy.PatientIdColumn.ColumnName] = patientId;
                            dr[dsCCDA.Allergy.IsDeletedColumn.ColumnName] = false;
                            Act allergyProblemAct = e.Item as Act;
                            if (!allergyProblemAct.entryRelationship.IsNullOrEmpty() && allergyProblemAct.entryRelationship != null && allergyProblemAct.entryRelationship[0].Item != null
                                && allergyProblemAct.entryRelationship[0].Item is Observation)
                            {
                                if (allergyProblemAct.statusCode != null && !string.IsNullOrWhiteSpace(allergyProblemAct.statusCode.code) && allergyProblemAct.statusCode.code.ToLower() == "active")
                                {
                                    dr[dsCCDA.Allergy.IsActiveColumn.ColumnName] = true;
                                    dr[dsCCDA.Allergy.StatusColumn.ColumnName] = allergyProblemAct.statusCode.code;
                                }
                                else
                                {
                                    dr[dsCCDA.Allergy.IsActiveColumn.ColumnName] = true;
                                    dr[dsCCDA.Allergy.StatusColumn.ColumnName] = allergyProblemAct.statusCode.code;
                                }

                                allergyIntoleranceObservation = allergyProblemAct.entryRelationship[0].Item as Observation;
                                foreach (EntryRelationship er in allergyProblemAct.entryRelationship)
                                {
                                    if (er.Item != null && er.Item is Observation)
                                    {
                                        Observation observation = (Observation)er.Item;

                                        if (observation.effectiveTime != null && observation.effectiveTime.Items != null && observation.effectiveTime.Items.Count() > 0 && observation.effectiveTime is IVL_TS)
                                        {
                                            string lowTime, highTime;
                                            GetLowHighTime(observation.effectiveTime, out lowTime, out highTime);
                                            if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                            {
                                                dr[dsCCDA.Allergy.OnSetDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                            }
                                            if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                            {
                                                dr[dsCCDA.Allergy.LastModifiedColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                            }
                                        }
                                        else if (observation.effectiveTime != null && !string.IsNullOrWhiteSpace(observation.effectiveTime.value) &&
                                        observation.effectiveTime.value.ToLower() != "unk" && observation.effectiveTime.value.ToLower() != "ni")
                                        {
                                            dr[dsCCDA.Allergy.OnSetDateColumn.ColumnName] = observation.effectiveTime.value.ToFormatedDateTimeByFormat();
                                        }
                                        if (!observation.value.IsNullOrEmpty() && observation.value[0] is CD
                                        && !String.IsNullOrWhiteSpace((observation.value[0] as CD).code))
                                        {
                                            CD allergyType = observation.value[0] as CD;
                                            if (allergyType != null && !string.IsNullOrWhiteSpace(allergyType.code))
                                            {
                                                dr[dsCCDA.Allergy.TypeSNOMEDCodeColumn.ColumnName] = allergyType.code;
                                                dr[dsCCDA.Allergy.TypeColumn.ColumnName] = allergyType.displayName;
                                            }
                                        }
                                        if (!observation.participant.IsNullOrEmpty() && observation.participant[0] != null && observation.participant[0].participantRole != null
                                            && observation.participant[0].participantRole.Item != null && observation.participant[0].participantRole.Item is PlayingEntity)
                                        {
                                            PlayingEntity objPlayingEntity = observation.participant[0].participantRole.Item as PlayingEntity;
                                            if (objPlayingEntity.code != null && !String.IsNullOrWhiteSpace(objPlayingEntity.code.code))
                                            {
                                                dr[dsCCDA.Allergy.RxnormIDColumn.ColumnName] = objPlayingEntity.code.code;
                                                dr[dsCCDA.Allergy.AllergenColumn.ColumnName] = objPlayingEntity.code.displayName;
                                            }
                                        }
                                        else if (!observation.participant.IsNullOrEmpty() && observation.participant[0] != null && observation.participant[0].participantRole != null
                                            && observation.participant[0].participantRole.Item != null && observation.participant[0].participantRole.Item is PlayingEntity
                                            && (observation.participant[0].participantRole.Item as PlayingEntity).name != null
                                            && !string.IsNullOrWhiteSpace((observation.participant[0].participantRole.Item as PlayingEntity).name[0].Text[0]))
                                        {
                                            string substance = (observation.participant[0].participantRole.Item as PlayingEntity).name[0].Text[0];
                                            dr[dsCCDA.Allergy.RxnormIDColumn.ColumnName] = 0;
                                            dr[dsCCDA.Allergy.AllergenColumn.ColumnName] = substance;
                                        }
                                        string Reactions = null;
                                        int counter = 0;
                                        if (!allergyIntoleranceObservation.entryRelationship.IsNullOrEmpty())
                                        {
                                            foreach (EntryRelationship erIntolerance in allergyIntoleranceObservation.entryRelationship)
                                            {
                                                if (erIntolerance != null && erIntolerance.Item != null && erIntolerance.Item is Observation)
                                                {
                                                    allergyIntoleranceObservation = erIntolerance.Item as Observation;
                                                    if (!allergyIntoleranceObservation.templateId.IsNullOrEmpty() && allergyIntoleranceObservation.templateId[0] != null
                                                        && !string.IsNullOrWhiteSpace(allergyIntoleranceObservation.templateId[0].root) &&
                                                        allergyIntoleranceObservation.templateId[0].root == AllergiesCodes.ObsStatusTemplateId &&
                                                        !allergyIntoleranceObservation.value.IsNullOrEmpty() && allergyIntoleranceObservation.value[0] != null
                                                        && allergyIntoleranceObservation.value[0] is CE)
                                                    {
                                                        CE obsValue = allergyIntoleranceObservation.value[0] as CE;
                                                        if (obsValue != null && !string.IsNullOrWhiteSpace(obsValue.code))
                                                        {
                                                            if (obsValue.code == AllergiesCodes.ActiveCode) //active
                                                            {
                                                                dr[dsCCDA.Allergy.IsActiveColumn.ColumnName] = true;
                                                            }
                                                            else if (obsValue.code == AllergiesCodes.InactiveCode) //inactive
                                                            {
                                                                dr[dsCCDA.Allergy.IsActiveColumn.ColumnName] = false;
                                                            }
                                                        }
                                                    }
                                                    //Allergy Reaction Observation
                                                    else if (!allergyIntoleranceObservation.templateId.IsNullOrEmpty() && allergyIntoleranceObservation.templateId[0] != null
                                                        && !string.IsNullOrWhiteSpace(allergyIntoleranceObservation.templateId[0].root) &&
                                                        allergyIntoleranceObservation.templateId[0].root == AllergiesCodes.ObsReactionTemplateId && !allergyIntoleranceObservation.value.IsNullOrEmpty()
                                                        && allergyIntoleranceObservation.value[0] != null && allergyIntoleranceObservation.value[0] is CD)
                                                    {
                                                        CD obsValue = allergyIntoleranceObservation.value[0] as CD;
                                                        if (obsValue != null && !string.IsNullOrWhiteSpace(obsValue.code))
                                                        {
                                                            if (counter == 0)
                                                            {
                                                                Reactions = obsValue.displayName;
                                                            }
                                                            else
                                                            {
                                                                Reactions = "," + obsValue.displayName;
                                                            }
                                                            counter++;
                                                        }
                                                        if (!allergyIntoleranceObservation.entryRelationship.IsNullOrEmpty())
                                                        {
                                                            foreach (EntryRelationship innerERIntolerance in allergyIntoleranceObservation.entryRelationship)
                                                            {
                                                                Observation allergyIntoleranceObservationInner = innerERIntolerance.Item as Observation;
                                                                if (!allergyIntoleranceObservationInner.templateId.IsNullOrEmpty() && allergyIntoleranceObservationInner.templateId[0] != null
                                                                        && !string.IsNullOrWhiteSpace(allergyIntoleranceObservationInner.templateId[0].root) &&
                                                                        allergyIntoleranceObservationInner.templateId[0].root == AllergiesCodes.ObsStatusTemplateId &&
                                                                        !allergyIntoleranceObservationInner.value.IsNullOrEmpty() && allergyIntoleranceObservationInner.value[0] != null
                                                                        && allergyIntoleranceObservationInner.value[0] is CD)
                                                                {
                                                                    CD obsValueInner = allergyIntoleranceObservationInner.value[0] as CD;
                                                                    if (obsValue != null && !string.IsNullOrWhiteSpace(obsValueInner.code))
                                                                    {
                                                                        if (obsValueInner.code == AllergiesCodes.ActiveCode) //active
                                                                        {
                                                                            dr[dsCCDA.Allergy.IsActiveColumn.ColumnName] = true;
                                                                        }
                                                                        else if (obsValueInner.code == AllergiesCodes.InactiveCode) //inactive
                                                                        {
                                                                            dr[dsCCDA.Allergy.IsActiveColumn.ColumnName] = false;
                                                                        }
                                                                    }
                                                                }
                                                                else if (!allergyIntoleranceObservationInner.templateId.IsNullOrEmpty() && allergyIntoleranceObservationInner.templateId[0] != null &&
                                                                        !string.IsNullOrWhiteSpace(allergyIntoleranceObservationInner.templateId[0].root) &&
                                                                        allergyIntoleranceObservationInner.templateId[0].root == AllergiesCodes.ObsSeverityTemplateId && !allergyIntoleranceObservationInner.value.IsNullOrEmpty()
                                                                        && allergyIntoleranceObservationInner.value[0] != null && allergyIntoleranceObservationInner.value[0] is CD)
                                                                {
                                                                    CD obsValueInner = allergyIntoleranceObservationInner.value[0] as CD;
                                                                    if (obsValueInner != null && !string.IsNullOrWhiteSpace(obsValueInner.code))
                                                                    {
                                                                        dr[dsCCDA.Allergy.SeverityColumn.ColumnName] = obsValueInner.displayName;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //Allergy Severity Observation
                                                    else if (!allergyIntoleranceObservation.templateId.IsNullOrEmpty() && allergyIntoleranceObservation.templateId[0] != null &&
                                                        !string.IsNullOrWhiteSpace(allergyIntoleranceObservation.templateId[0].root) &&
                                                        allergyIntoleranceObservation.templateId[0].root == AllergiesCodes.ObsSeverityTemplateId && !allergyIntoleranceObservation.value.IsNullOrEmpty()
                                                        && allergyIntoleranceObservation.value[0] != null && allergyIntoleranceObservation.value[0] is CD)
                                                    {
                                                        CD obsValue = allergyIntoleranceObservation.value[0] as CD;
                                                        if (obsValue != null && !string.IsNullOrWhiteSpace(obsValue.code))
                                                        {
                                                            dr[dsCCDA.Allergy.SeverityColumn.ColumnName] = obsValue.displayName;
                                                        }
                                                        if (!allergyIntoleranceObservation.entryRelationship.IsNullOrEmpty())
                                                        {
                                                            foreach (EntryRelationship innerERIntolerance in allergyIntoleranceObservation.entryRelationship)
                                                            {
                                                                Observation allergyIntoleranceObservationInner = erIntolerance.Item as Observation;
                                                                if (!allergyIntoleranceObservationInner.templateId.IsNullOrEmpty() && allergyIntoleranceObservationInner.templateId[0] != null
                                                                        && !string.IsNullOrWhiteSpace(allergyIntoleranceObservationInner.templateId[0].root) &&
                                                                        allergyIntoleranceObservationInner.templateId[0].root == AllergiesCodes.ObsStatusTemplateId &&
                                                                        !allergyIntoleranceObservationInner.value.IsNullOrEmpty() && allergyIntoleranceObservationInner.value[0] != null
                                                                        && allergyIntoleranceObservationInner.value[0] is CD)
                                                                {
                                                                    CD obsValueInner = allergyIntoleranceObservationInner.value[0] as CD;
                                                                    if (obsValueInner != null && !string.IsNullOrWhiteSpace(obsValueInner.code))
                                                                    {
                                                                        if (obsValueInner.code == AllergiesCodes.ActiveCode) //active
                                                                        {
                                                                            dr[dsCCDA.Allergy.IsActiveColumn.ColumnName] = true;
                                                                        }
                                                                        else if (obsValueInner.code == AllergiesCodes.InactiveCode) //inactive
                                                                        {
                                                                            dr[dsCCDA.Allergy.IsActiveColumn.ColumnName] = false;
                                                                        }
                                                                    }
                                                                }
                                                                else if (!allergyIntoleranceObservationInner.templateId.IsNullOrEmpty() && allergyIntoleranceObservationInner.templateId[0] != null
                                                                        && !string.IsNullOrWhiteSpace(allergyIntoleranceObservationInner.templateId[0].root) &&
                                                                        allergyIntoleranceObservationInner.templateId[0].root == AllergiesCodes.ObsReactionTemplateId && !allergyIntoleranceObservationInner.value.IsNullOrEmpty()
                                                                        && allergyIntoleranceObservationInner.value[0] != null && allergyIntoleranceObservationInner.value[0] is CD)
                                                                {
                                                                    CD obsValueInner = allergyIntoleranceObservationInner.value[0] as CD;
                                                                    if (obsValueInner != null && !string.IsNullOrWhiteSpace(obsValueInner.code))
                                                                    {
                                                                        if (counter == 0)
                                                                        {
                                                                            Reactions = obsValue.displayName;
                                                                        }
                                                                        else
                                                                        {
                                                                            Reactions = "," + obsValue.displayName;
                                                                        }
                                                                        counter++;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (!string.IsNullOrWhiteSpace(Reactions))
                                            dr[dsCCDA.Allergy.ReactionColumn.ColumnName] = Reactions;
                                        if (dr[dsCCDA.Allergy.AllergenColumn.ColumnName] != DBNull.Value)
                                        {
                                            dr[dsCCDA.Allergy.CreatedByColumn.ColumnName] = CreatedBy;
                                            dr[dsCCDA.Allergy.ModifiedByColumn.ColumnName] = ModifiedBy;
                                            dtAllergies.Rows.Add(dr);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(DocumentErrors.Allergies + ": " + ex.Message);
            }
        }

        public static class AllergiesCodes
        {
            public static string DrugallergyCode = "416098002";
            public static string ActiveCode = "55561003";
            public static string InactiveCode = "73425007";
            public static string ObsStatusTemplateId = "2.16.840.1.113883.10.20.22.4.28";
            public static string ObsReactionTemplateId = "2.16.840.1.113883.10.20.22.4.9";
            public static string ObsSeverityTemplateId = "2.16.840.1.113883.10.20.22.4.8";
        }

        #endregion Allergies

        #region Immunization

        private static void GetImmunizationData(ClinicalDocument document, ref DataTable dtImmunization, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 immunizationComp = new Component3();
                List<Entry> entryList = null;
                bool isComponentExist = document.GetComponentByTemplateId(ref immunizationComp, DocumentSections.Immunization);
                if (isComponentExist && immunizationComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is SubstanceAdministration)
                        {
                            SubstanceAdministration immSubstanceAdministration = e.Item as SubstanceAdministration;
                            DataRow dr = dtImmunization.NewRow();
                            dr[dsCCDA.Immunization.PatientIdColumn.ColumnName] = patientId;
                            dr[dsCCDA.Immunization.GivenByColumn.ColumnName] = UserId;
                            if (immSubstanceAdministration.statusCode != null && !string.IsNullOrWhiteSpace(immSubstanceAdministration.statusCode.code))
                            {
                                dr[dsCCDA.Immunization.VaccineStatusColumn.ColumnName] = immSubstanceAdministration.statusCode.code;
                            }
                            dr[dsCCDA.Immunization.IsActiveColumn.ColumnName] = true;

                            if (!immSubstanceAdministration.effectiveTime.IsNullOrEmpty() && immSubstanceAdministration.effectiveTime[0] != null
                                && immSubstanceAdministration.effectiveTime[0] is IVL_TS)
                            {
                                string lowTime, highTime;
                                GetLowHighTime((IVL_TS)immSubstanceAdministration.effectiveTime[0], out lowTime, out highTime);
                                if (!string.IsNullOrWhiteSpace(lowTime) && lowTime != "UNK" && highTime != "NI")
                                {
                                    dr[dsCCDA.Immunization.AdministrationDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                }
                            }
                            else if (immSubstanceAdministration.effectiveTime != null && !string.IsNullOrWhiteSpace(immSubstanceAdministration.effectiveTime[0].value) && immSubstanceAdministration.effectiveTime[0].value != "UNK" && immSubstanceAdministration.effectiveTime[0].value != "NI")
                            {
                                dr[dsCCDA.Immunization.AdministrationDateColumn.ColumnName] = immSubstanceAdministration.effectiveTime[0].value.ToFormatedDateTimeByFormat();
                            }
                            else
                            {
                                errors.Add("Immunizatoin --> Date Administred not found");
                            }
                            if (dr[dsCCDA.Immunization.AdministrationDateColumn.ColumnName] == DBNull.Value && immSubstanceAdministration.effectiveTime != null && !string.IsNullOrWhiteSpace(immSubstanceAdministration.effectiveTime[0].value) && immSubstanceAdministration.effectiveTime[0].value != "UNK" && immSubstanceAdministration.effectiveTime[0].value != "NI")
                            {
                                dr[dsCCDA.Immunization.AdministrationDateColumn.ColumnName] = immSubstanceAdministration.effectiveTime[0].value.ToFormatedDateTimeByFormat();
                            }

                            if (immSubstanceAdministration.routeCode != null && immSubstanceAdministration.routeCode is CE)
                            {
                                CE route = immSubstanceAdministration.routeCode as CE;
                                if (!string.IsNullOrWhiteSpace(route.code))
                                    dr[dsCCDA.Immunization.RouteColumn.ColumnName] = route.code;
                                if (!string.IsNullOrWhiteSpace(route.displayName))
                                    dr[dsCCDA.Immunization.RouteDescriptionColumn.ColumnName] = route.displayName;
                            }
                            if (immSubstanceAdministration.doseQuantity != null && immSubstanceAdministration.doseQuantity is IVL_PQ)
                            {
                                IVL_PQ doseQuantity = immSubstanceAdministration.doseQuantity as IVL_PQ;
                                if (!string.IsNullOrWhiteSpace(doseQuantity.value))
                                    dr[dsCCDA.Immunization.DoseColumn.ColumnName] = doseQuantity.value;
                                if (!string.IsNullOrWhiteSpace(doseQuantity.unit))
                                    dr[dsCCDA.Immunization.AmountColumn.ColumnName] = doseQuantity.unit;
                            }
                            if (immSubstanceAdministration.consumable != null && immSubstanceAdministration.consumable is Consumable)
                            {
                                Consumable consumable = (Consumable)immSubstanceAdministration.consumable;
                                if (consumable.manufacturedProduct != null && consumable.manufacturedProduct.Item != null && consumable.manufacturedProduct.Item is Material)
                                {
                                    Material manufacturedMaterial = (Material)consumable.manufacturedProduct.Item;
                                    if (manufacturedMaterial.code != null && !string.IsNullOrWhiteSpace(manufacturedMaterial.code.code))
                                    {
                                        dr[dsCCDA.Immunization.VaccineColumn.ColumnName] = manufacturedMaterial.code.code;
                                    }
                                    if (manufacturedMaterial.code != null && !string.IsNullOrWhiteSpace(manufacturedMaterial.code.displayName))
                                    {
                                        dr[dsCCDA.Immunization.VaccineDescriptionColumn.ColumnName] = manufacturedMaterial.code.displayName;
                                    }
                                    if (manufacturedMaterial.lotNumberText != null && manufacturedMaterial.lotNumberText.Text.Count > 0 && !string.IsNullOrWhiteSpace(manufacturedMaterial.lotNumberText.Text[0]))
                                    {
                                        dr[dsCCDA.Immunization.LotNumberColumn.ColumnName] = manufacturedMaterial.lotNumberText.Text[0];
                                    }
                                }
                                if (consumable.manufacturedProduct != null && consumable.manufacturedProduct.manufacturerOrganization != null && consumable.manufacturedProduct.manufacturerOrganization is Organization)
                                {
                                    Organization manufacturedOrganization = (Organization)consumable.manufacturedProduct.manufacturerOrganization;
                                    if (manufacturedOrganization.name != null && !string.IsNullOrWhiteSpace(manufacturedOrganization.name[0].Text[0]))
                                    {
                                        dr[dsCCDA.Immunization.ManufacturerColumn.ColumnName] = manufacturedOrganization.name[0].Text[0];
                                    }
                                }
                            }
                            if (immSubstanceAdministration.negationInd == true)
                            {
                                if (!immSubstanceAdministration.entryRelationship.IsNullOrEmpty() && immSubstanceAdministration.entryRelationship != null && immSubstanceAdministration.entryRelationship[0].Item != null
                                    && immSubstanceAdministration.entryRelationship[0].Item is Observation)
                                {
                                    Observation immObservation = immSubstanceAdministration.entryRelationship[0].Item as Observation;
                                    if (immObservation.code != null)
                                    {
                                        dr[dsCCDA.Immunization.NegationIndexColumn.ColumnName] = 1;
                                        dr[dsCCDA.Immunization.NegationCodeColumn.ColumnName] = immObservation.code.code;
                                        dr[dsCCDA.Immunization.NegationReasonColumn.ColumnName] = immObservation.code.displayName;
                                    }
                                }
                                dr[dsCCDA.Immunization.TypeColumn.ColumnName] = "REFUSAL";
                            }
                            else
                            {
                                dr[dsCCDA.Immunization.TypeColumn.ColumnName] = "DOCUMENTHX";
                            }

                            if (dr[dsCCDA.Immunization.VaccineColumn.ColumnName] != DBNull.Value)
                            {
                                dr[dsCCDA.Immunization.VaccineHxIdColumn.ColumnName] = i--;
                                dr[dsCCDA.Immunization.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                                if (!string.IsNullOrWhiteSpace(FacilityId))
                                    dr[dsCCDA.Immunization.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                                dr[dsCCDA.Immunization.CreatedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.Immunization.CreatedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.Immunization.ModifiedByColumn.ColumnName] = ModifiedByImmunization;
                                dr[dsCCDA.Immunization.ModifiedOnColumn.ColumnName] = CreatedOn;
                                dtImmunization.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(DocumentErrors.Immunization + ex.Message);
            }
        }

        #endregion Immunization

        #region Medications

        private static void GetMedicationData(ClinicalDocument document, ref DataTable dtMedication, ref List<string> errors, long patientId)
        {
            Component3 medicationComp = new Component3();
            List<Entry> entryList = null;
            bool isComponentExist = document.GetComponentByTemplateId(ref medicationComp, DocumentSections.Medications);
            if (isComponentExist && medicationComp.GetEntry(ref entryList))
            {
                int i = -1;
                foreach (Entry e in entryList)
                {
                    try
                    {
                        if (e != null && e.Item != null && e.Item is SubstanceAdministration)
                        {
                            string PatientNotes = string.Empty;
                            SubstanceAdministration medSubstanceAdministration = e.Item as SubstanceAdministration;
                            DataRow dr = dtMedication.NewRow();
                            dr[dsCCDA.Medication.MedicationIdColumn.ColumnName] = i;
                            dr[dsCCDA.Medication.PatientIdColumn.ColumnName] = patientId;
                            if (medSubstanceAdministration.statusCode != null && !String.IsNullOrWhiteSpace(medSubstanceAdministration.statusCode.code))
                            {
                                dr[dsCCDA.Medication.StatusColumn.ColumnName] = medSubstanceAdministration.statusCode.code;
                            }
                            if (!medSubstanceAdministration.effectiveTime.IsNullOrEmpty() && medSubstanceAdministration.effectiveTime[0] != null
                                && medSubstanceAdministration.effectiveTime[0] is IVL_TS)
                            {
                                string lowTime, highTime;
                                GetLowHighTime((IVL_TS)medSubstanceAdministration.effectiveTime[0], out lowTime, out highTime);
                                if (!string.IsNullOrWhiteSpace(lowTime) && lowTime != "UNK" && highTime != "NI")
                                {
                                    dr[dsCCDA.Medication.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                }
                                if (!string.IsNullOrWhiteSpace(highTime) && highTime != "UNK" && highTime != "NI")
                                {
                                    dr[dsCCDA.Medication.StopDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                }
                            }
                            else if (medSubstanceAdministration.effectiveTime != null && !string.IsNullOrWhiteSpace(medSubstanceAdministration.effectiveTime[0].value)
                                    && medSubstanceAdministration.effectiveTime[0].value != "UNK" && medSubstanceAdministration.effectiveTime[0].value != "NI")
                            {
                                dr[dsCCDA.Medication.StartDateColumn.ColumnName] = medSubstanceAdministration.effectiveTime[0].value.ToFormatedDateTimeByFormat();
                            }
                            else
                            {
                                errors.Add("Medication --> Start Date not available in the file");
                            }
                            PIVL_TS effectiveTime = null;
                            EIVL_TS effectiveTimeEvent = null;
                            if (!medSubstanceAdministration.effectiveTime.IsNullOrEmpty())
                            {
                                foreach (ANY et in medSubstanceAdministration.effectiveTime)
                                {
                                    if (et is PIVL_TS)
                                        effectiveTime = et as PIVL_TS;
                                    else if (et is EIVL_TS)
                                        effectiveTimeEvent = et as EIVL_TS;
                                }
                            }
                            if (effectiveTime != null)
                            {
                                if (effectiveTime.period != null && effectiveTime.period is IVL_PQ && ((IVL_PQ)effectiveTime.period).Items != null)
                                {
                                    string val1 = string.Empty, unit1 = string.Empty, val2 = string.Empty, unit2 = string.Empty;
                                    if (((IVL_PQ)effectiveTime.period).Items[0] != null)
                                    {
                                        val1 = ((IVL_PQ)effectiveTime.period).Items[0].value;
                                        unit1 = ((IVL_PQ)effectiveTime.period).Items[0].unit;
                                    }
                                    if (((IVL_PQ)effectiveTime.period).Items[1] != null)
                                    {
                                        val2 = ((IVL_PQ)effectiveTime.period).Items[1].value;
                                        unit2 = ((IVL_PQ)effectiveTime.period).Items[1].unit;
                                    }
                                    if (effectiveTime.institutionSpecified1)
                                    {
                                        PatientNotes = NumberToWord(MDVUtility.ToInt32(val1))
                                             + " to " + NumberToWord(MDVUtility.ToInt32(val2)) + " " + MedicationUnits(unit1.ToLower());
                                    }
                                    else
                                    {
                                        PatientNotes = "every " + NumberToWord(MDVUtility.ToInt32(val1))
                                             + " to " + NumberToWord(MDVUtility.ToInt32(val2)) + " " + MedicationUnits(unit1.ToLower());
                                    }
                                }
                                else
                                {
                                    string val = effectiveTime.period.value;
                                    string unit = effectiveTime.period.unit;
                                    PatientNotes = CalculateDoseTimming(val, unit, effectiveTime.institutionSpecified1);
                                }
                            }
                            if (effectiveTimeEvent != null)
                            {
                                if (!string.IsNullOrWhiteSpace(effectiveTimeEvent.@event.code))
                                {
                                    PatientNotes += " " + CalculateEvetRelateTime(effectiveTimeEvent.@event.code);
                                }
                            }

                            if (medSubstanceAdministration.repeatNumber != null)
                            {
                                dr[dsCCDA.Medication.RepeatNumberColumn.ColumnName] = medSubstanceAdministration.repeatNumber.value;
                            }
                            if (medSubstanceAdministration.doseQuantity != null)
                            {
                                dr[dsCCDA.Medication.DoseValueColumn.ColumnName] = medSubstanceAdministration.doseQuantity.value;
                                if (!string.IsNullOrWhiteSpace(medSubstanceAdministration.doseQuantity.unit) && medSubstanceAdministration.doseQuantity.unit != "1")
                                    dr[dsCCDA.Medication.DoseUnitColumn.ColumnName] = medSubstanceAdministration.doseQuantity.unit;
                            }
                            if (medSubstanceAdministration.routeCode != null && !string.IsNullOrWhiteSpace(medSubstanceAdministration.routeCode.displayName))
                            {
                                dr[dsCCDA.Medication.RouteCodeColumn.ColumnName] = medSubstanceAdministration.routeCode.code;
                                dr[dsCCDA.Medication.RouteByColumn.ColumnName] = medSubstanceAdministration.routeCode.displayName;
                            }
                            if (medSubstanceAdministration.consumable != null && medSubstanceAdministration.consumable is Consumable)
                            {
                                Consumable consumable = (Consumable)medSubstanceAdministration.consumable;
                                if (consumable.manufacturedProduct != null && consumable.manufacturedProduct.Item != null
                                    && consumable.manufacturedProduct.Item is Material)
                                {
                                    Material manufacturedMaterial = (Material)consumable.manufacturedProduct.Item;
                                    if (manufacturedMaterial.code != null)
                                    {
                                        dr[dsCCDA.Medication.RxNormCodeColumn.ColumnName] = manufacturedMaterial.code.code;
                                        dr[dsCCDA.Medication.DrugDescriptionColumn.ColumnName] = manufacturedMaterial.code.displayName;
                                    }
                                }
                                else
                                {
                                    errors.Add("Medication --> Rx Norm Code not found in the file");
                                }
                            }
                            if (!medSubstanceAdministration.entryRelationship.IsNullOrEmpty() && medSubstanceAdministration.entryRelationship != null
                                && medSubstanceAdministration.entryRelationship[0].Item != null)
                            {
                                foreach (EntryRelationship er in medSubstanceAdministration.entryRelationship)
                                {
                                    if (er.Item != null && er.Item is Supply)
                                    {
                                        Supply supplyOrder = (Supply)er.Item;
                                        if (!supplyOrder.entryRelationship.IsNullOrEmpty())
                                        {
                                            foreach (EntryRelationship erInner in supplyOrder.entryRelationship)
                                            {
                                                if (erInner.Item != null && erInner.Item is Act)
                                                {
                                                    Act act = erInner.Item as Act;
                                                    if (act.code != null && act.code.code == "409073007")
                                                    {
                                                        dr[dsCCDA.Medication.SubstitutionColumn.ColumnName] = "y";
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            if (medSubstanceAdministration.negationInd == true)
                            {
                                dr[dsCCDA.Medication.NegationReasonColumn.ColumnName] = "";
                                dr[dsCCDA.Medication.NegationIndexColumn.ColumnName] = true;
                            }
                            dr[dsCCDA.Medication.PatientNotesColumn.ColumnName] = PatientNotes;
                            if (dr[dsCCDA.Medication.SubstitutionColumn.ColumnName] == DBNull.Value)
                            {
                                dr[dsCCDA.Medication.SubstitutionColumn.ColumnName] = "n";
                            }
                            if (dr[dsCCDA.Medication.DrugDescriptionColumn.ColumnName] != DBNull.Value)
                            {
                                dr[dsCCDA.Medication.UserIdColumn.ColumnName] = UserId;
                                dr[dsCCDA.Medication.IsActiveColumn.ColumnName] = true;
                                dr[dsCCDA.Medication.CreatedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.Medication.CreatedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.Medication.ModifiedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.Medication.ModifiedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.Medication.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                dtMedication.Rows.Add(dr);
                                i--;
                            }
                            else
                            {
                                errors.Add("Medication: Drug Description not found");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add("Medication: " + ex.Message);
                    }
                }
            }
        }

        public static string MedicationUnits(string Unit)
        {
            string strUnit = string.Empty;
            switch (Unit)
            {
                case "h": return "hours";
                case "d": return "days";

            }

            return strUnit;
        }

        public static string CalculateDoseTimming(string Value, string Unit, bool institutionSpecified)
        {
            string strResult = string.Empty;
            if (Unit == "h")
            {
                if (institutionSpecified)
                {
                    if (Value == "6")
                    {
                        strResult = "four times a day";
                    }
                    else if (Value == "8")
                    {
                        strResult = "three times a day";
                    }
                    else if (Value == "12")
                    {
                        strResult = "twice a day";
                    }
                    else if (Value == "24")
                    {
                        strResult = "once a day";
                    }
                    else
                    {
                        strResult = Value + " hours";
                    }
                }
                else
                {
                    strResult = "every ";
                    strResult += Value;
                    strResult += " hours";
                }
            }
            else if (Unit == "d")
            {
                if (institutionSpecified)
                {
                    if (Value == "7")
                    {
                        strResult = " once a week";
                    }
                    else if (Value == "14")
                    {
                        strResult = " two times a week";
                    }
                    else if (Value == "29" || Value == "30" || Value == "31")
                    {
                        strResult = " once a month";
                    }
                    else
                    {
                        strResult = Value + " day";
                    }
                }
                else
                {
                    strResult = "every ";
                    strResult += Value;
                    strResult += " day";
                }
            }
            else if (Unit == "m")
            {
                if (institutionSpecified)
                {
                    strResult = Value + " month";
                }
                else
                {
                    strResult = "every ";
                    strResult += Value;
                    strResult += " month";
                }
            }
            else
            {
                if (institutionSpecified)
                {
                    strResult = Value + " - " + Unit;
                }
                else
                {
                    strResult = "every ";
                    strResult += Value;
                    strResult += " - " + Unit;
                }
            }
            return strResult;
        }

        public static string CalculateEvetRelateTime(string code)
        {
            code = code.ToLower();
            switch (code)
            {
                case "ac":
                    return " before meal";
                case "acm":
                    return " before breakfast";
                case "pc":
                    return " after meal";
                case "pcm":
                    return " after breakfast";
                case "ic":
                    return " between meals";
                case "icm":
                    return " between breakfast and lunch";
                case "hs":
                    return " hours of sleep";
                case "acd":
                    return " before lunch";
                case "acv":
                    return " before dinner";
                case "pcd":
                    return " after lunch";
                case "pcv":
                    return " after dinner";
                case "icd":
                    return " between lunch and dinner";
                case "icv":
                    return " between dinner and go to sleep";
                default:
                    return code;
            }
        }

        private static List<string> GetMedicationsSig(object obj)
        {
            try
            {
                List<string> lstMedication = new List<string>();
                StrucDocTable HtmlTable = (StrucDocTable)obj;

                List<StrucDocTbody> Body = HtmlTable.tbody;
                if (Body != null && Body.Count > 0)
                {
                    List<StrucDocTr> listTr = Body[0].tr;
                    foreach (var tr in listTr)
                    {
                        StrucDocTd td = (StrucDocTd)tr.Items[1];
                        lstMedication.Add((string)td.Text.FirstOrDefault());
                    }
                }
                return lstMedication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion Medications

        #region Problems

        private static void GetProblemsData(ClinicalDocument document, ref DataTable dtProblems, ref List<string> errors, long patientId)
        {
            Component3 problemsComp = new Component3();
            List<Entry> entryList = null;
            bool isComponentExist = document.GetComponentByTemplateId(ref problemsComp, DocumentSections.Problem);

            if (isComponentExist && problemsComp.GetEntry(ref entryList))
            {
                int i = -1;
                foreach (Entry e in entryList)
                {
                    if (e != null && e.Item != null && e.Item is Act)
                    {
                        Act act = e.Item as Act;

                        if (!act.entryRelationship.IsNullOrEmpty())
                        {
                            foreach (EntryRelationship er in act.entryRelationship)
                            {
                                if (er.Item != null && er.Item is Observation)
                                {
                                    Observation problemObservation = er.Item as Observation;
                                    DataRow dr = dtProblems.NewRow();
                                    dr[dsCCDA.ProblemList.PatientIdColumn.ColumnName] = patientId;
                                    if (problemObservation.statusCode != null && !String.IsNullOrWhiteSpace(problemObservation.statusCode.code))
                                    {
                                        dr[dsCCDA.ProblemList.StatusColumn.ColumnName] = problemObservation.statusCode.code;
                                    }
                                    if (problemObservation.effectiveTime != null && problemObservation.effectiveTime is IVL_TS)
                                    {
                                        string lowTime, highTime;
                                        GetLowHighTime(problemObservation.effectiveTime, out lowTime, out highTime);
                                        if (!String.IsNullOrWhiteSpace(lowTime))
                                        {
                                            dr[dsCCDA.ProblemList.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                        }
                                        if (!String.IsNullOrWhiteSpace(highTime))
                                        {
                                            dr[dsCCDA.ProblemList.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                        }
                                    }
                                    else
                                    {
                                        errors.Add("Problems --> DiagnosedDate not found");
                                    }

                                    if (!problemObservation.value.IsNullOrEmpty() && problemObservation.value[0] != null && problemObservation.value[0] is CD)
                                    {
                                        CD snomedID = problemObservation.value[0] as CD;
                                        if (!String.IsNullOrWhiteSpace(snomedID.code))
                                        {
                                            dr[dsCCDA.ProblemList.SNOMEDIDColumn.ColumnName] = snomedID.code;
                                            if (!String.IsNullOrWhiteSpace(snomedID.displayName))
                                            {
                                                dr[dsCCDA.ProblemList.ProblemNameColumn.ColumnName] = snomedID.displayName;
                                                dr[dsCCDA.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName] = snomedID.displayName;
                                            }
                                            List<MDVision.Model.CCDA.ICDLookupModel> ICDLookupsList = BLLIMO.GetIMOICDbySNOMEDCode(snomedID.code);
                                            if (ICDLookupsList != null && ICDLookupsList.Count > 0)
                                            {
                                                MDVision.Model.CCDA.ICDLookupModel objICD = ICDLookupsList.FirstOrDefault(m => m.ICD10CM_CODE != null || m.ICD10CM_TITLE != null || m.ICD9CM_CODE != null || m.ICD9CM_TITLE != null);
                                                if (objICD != null)
                                                {
                                                    dr[dsCCDA.ProblemList.ICD9Column.ColumnName] = objICD.ICD9CM_CODE;
                                                    dr[dsCCDA.ProblemList.ICD10Column.ColumnName] = objICD.ICD10CM_CODE;
                                                    dr[dsCCDA.ProblemList.ICD9_DescriptionColumn.ColumnName] = snomedID.displayName;
                                                    dr[dsCCDA.ProblemList.ICD10_DescriptionColumn.ColumnName] = snomedID.displayName;
                                                }
                                            }
                                        }
                                        if (!snomedID.translation.IsNullOrEmpty() && !String.IsNullOrWhiteSpace(snomedID.translation[0].code))
                                        {
                                            dr[dsCCDA.ProblemList.ProblemNameColumn.ColumnName] = snomedID.translation[0].displayName;
                                            dr[dsCCDA.ProblemList.ICD10_DescriptionColumn.ColumnName] = snomedID.translation[0].displayName;
                                            dr[dsCCDA.ProblemList.ICD10Column.ColumnName] = snomedID.translation[0].code;
                                        }
                                        else
                                        {
                                            errors.Add("Problems --> ICD code not in the file");
                                        }
                                    }
                                    else
                                    {
                                        errors.Add("Problems --> Snomed AND ICD is not in the file");
                                    }
                                    if (!problemObservation.entryRelationship.IsNullOrEmpty())
                                    {
                                        foreach (EntryRelationship entryRelationship in problemObservation.entryRelationship)
                                        {
                                            if (entryRelationship.Item != null && entryRelationship.Item is Observation)
                                            {
                                                Observation problemStatusObs = entryRelationship.Item as Observation;
                                                if (!problemStatusObs.templateId.IsNullOrEmpty() && problemStatusObs.templateId[0].root == ProblemsCodes.ProblemStatusCode
                                                        && !problemStatusObs.value.IsNullOrEmpty() && problemStatusObs.value[0] != null && problemStatusObs.value[0] is CD
                                                        && !String.IsNullOrWhiteSpace((problemStatusObs.value[0] as CD).code))
                                                {
                                                    if ((problemStatusObs.value[0] as CD).displayName.Equals("Active"))
                                                    {
                                                        dr[dsCCDA.ProblemList.IsActiveColumn.ColumnName] = true;
                                                    }
                                                    else
                                                    {
                                                        dr[dsCCDA.ProblemList.IsActiveColumn.ColumnName] = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (!problemObservation.id.IsNullOrEmpty() && problemObservation.id[0] != null && !string.IsNullOrWhiteSpace(problemObservation.id[0].root))
                                    {
                                        dr[dsCCDA.ProblemList.RootCodeColumn.ColumnName] = problemObservation.id[0].root;
                                    }
                                    if (dr[dsCCDA.ProblemList.ProblemNameColumn.ColumnName] != DBNull.Value && (problemObservation.negationInd == false || dr[dsCCDA.ProblemList.SNOMEDIDColumn.ColumnName].ToString() != "55607006"))
                                    {
                                        dr[dsCCDA.ProblemList.IsProblemColumn.ColumnName] = true;
                                        dr[dsCCDA.ProblemList.ProblemListIdColumn.ColumnName] = i--;
                                        dr[dsCCDA.ProblemList.IsActiveColumn.ColumnName] = 1;
                                        dr[dsCCDA.ProblemList.CreatedByColumn.ColumnName] = CreatedBy;
                                        dr[dsCCDA.ProblemList.CreatedOnColumn.ColumnName] = CreatedOn;
                                        dr[dsCCDA.ProblemList.ModifiedByColumn.ColumnName] = CreatedBy;
                                        dr[dsCCDA.ProblemList.ModifiedOnColumn.ColumnName] = CreatedOn;
                                        dr[dsCCDA.ProblemList.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                        if (!string.IsNullOrWhiteSpace(FacilityId))
                                            dr[dsCCDA.ProblemList.FacilityIdColumn.ColumnName] = Convert.ToInt64(FacilityId);
                                        dr[dsCCDA.ProblemList.EntityIdColumn.ColumnName] = Convert.ToInt64(EntityId);
                                        dr[dsCCDA.ProblemList.UserIdColumn.ColumnName] = Convert.ToInt64(UserId);
                                        dtProblems.Rows.Add(dr);
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        public static class ProblemsCodes
        {
            public static string ProblemStatusCode = "2.16.840.1.113883.10.20.22.4.6";
        }

        #endregion Problems

        #region Procedures

        private static void GetProceduresData(ClinicalDocument document, ref DataTable dtProcedures, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 proceduresComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref proceduresComp, DocumentSections.Procedures);
                List<Entry> entryList = null;

                if (isComponentExist && proceduresComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e.Item is Procedure)
                        {
                            Procedure procedure = e.Item as Procedure;
                            DataRow dr = dtProcedures.NewRow();
                            dr[dsCCDA.NotesProcedures.ProcedureIdColumn.ColumnName] = i--;
                            dr[dsCCDA.NotesProcedures.PatientIdColumn.ColumnName] = patientId;
                            IVL_TS effectiveTime = procedure.effectiveTime;
                            if (procedure.effectiveTime != null && procedure.effectiveTime.Items != null && procedure.effectiveTime.Items.Count() > 0 && procedure.effectiveTime is IVL_TS)
                            {
                                string lowTime, highTime;
                                GetLowHighTime(procedure.effectiveTime, out lowTime, out highTime);

                                if (!string.IsNullOrEmpty(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                {
                                    dr[dsCCDA.NotesProcedures.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                }
                                if (!string.IsNullOrEmpty(highTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                {
                                    dr[dsCCDA.NotesProcedures.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                }
                            }
                            else if (effectiveTime != null && !string.IsNullOrWhiteSpace(effectiveTime.value) && effectiveTime.value.ToLower() != "unk" && effectiveTime.value.ToLower() != "ni")
                            {
                                dr[dsCCDA.NotesProcedures.StartDateColumn.ColumnName] = effectiveTime.value.ToFormatedDateTimeByFormat();
                            }
                            else
                            {
                                errors.Add("Procedure --> DOS not found");
                            }

                            if (procedure.negationInd == true)
                            {
                                var t = ((Summary.CCDA.Observation)procedure.entryRelationship[0].Item).value;
                                var s = t[0];
                                var NegationCode = ((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(s)).code;
                                dr[dsCCDA.NotesProcedures.NegationReasonColumn.ColumnName] = NegationCode;
                                dr[dsCCDA.NotesProcedures.NegationIndexColumn.ColumnName] = true;

                                if (GetCodeType(((MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.CD)(s)).codeSystem) == "2.16.840.1.113883.6.96")
                                {
                                    dr[dsCCDA.NotesProcedures.SNOMEDIDColumn.ColumnName] = NegationCode;
                                    dr[dsCCDA.NotesProcedures.CodeTypeColumn.ColumnName] = "SNOMED";
                                }
                                else
                                {
                                    dr[dsCCDA.NotesProcedures.CPTCodeColumn.ColumnName] = NegationCode;
                                    dr[dsCCDA.NotesProcedures.CodeTypeColumn.ColumnName] = "CPT";
                                }
                            }
                            else
                            {
                                if (procedure.code.codeSystem == "2.16.840.1.113883.6.96")
                                {
                                    dr[dsCCDA.NotesProcedures.SNOMEDIDColumn.ColumnName] = procedure.code.code;
                                    dr[dsCCDA.NotesProcedures.SNOMED_DESCRIPTIONColumn.ColumnName] = procedure.code.displayName;
                                    dr[dsCCDA.NotesProcedures.CodeTypeColumn.ColumnName] = "SNOMED";
                                }
                                else
                                {
                                    dr[dsCCDA.NotesProcedures.CPTCodeColumn.ColumnName] = procedure.code.code;
                                    dr[dsCCDA.NotesProcedures.CPT_DESCRIPTIONColumn.ColumnName] = procedure.code.displayName;
                                    dr[dsCCDA.NotesProcedures.CodeTypeColumn.ColumnName] = "CPT";
                                }
                            }
                            dr[dsCCDA.NotesProcedures.IsActiveColumn.ColumnName] = 1;
                            dr[dsCCDA.NotesProcedures.CreatedByColumn.ColumnName] = CreatedBy;
                            dr[dsCCDA.NotesProcedures.CreatedOnColumn.ColumnName] = CreatedOn;
                            dr[dsCCDA.NotesProcedures.ModifiedByColumn.ColumnName] = CreatedBy;
                            dr[dsCCDA.NotesProcedures.ModifiedOnColumn.ColumnName] = CreatedOn;
                            dr[dsCCDA.NotesProcedures.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                            if (!string.IsNullOrWhiteSpace(FacilityId))
                                dr[dsCCDA.NotesProcedures.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                            dr[dsCCDA.NotesProcedures.EntityIdColumn.ColumnName] = MDVUtility.ToInt64(EntityId);
                            dtProcedures.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Procedures: " + ex.Message);
            }
        }

        #endregion Procedures

        #region Vital Signs

        private static void GetVitalSignsData(ClinicalDocument document, ref DataTable dtVitalSigns, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 vitalSignsComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref vitalSignsComp, DocumentSections.VitalSigns);
                List<Entry> entryList = null;

                if (isComponentExist && vitalSignsComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    bool Recordexists = false;
                    foreach (Entry e in entryList)
                    {
                        if (e.Item is Organizer)
                        {
                            Organizer organizerVitalSign = e.Item as Organizer;
                            if (!organizerVitalSign.component.IsNullOrEmpty())
                            {
                                DataRow dr = dtVitalSigns.NewRow();
                                dr[dsCCDA.VitalSigns.PatientIdColumn.ColumnName] = patientId;
                                dr[dsCCDA.VitalSigns.VitalSignIdColumn.ColumnName] = i--;
                                if (organizerVitalSign.effectiveTime != null && organizerVitalSign.effectiveTime.Items != null &&
                                    organizerVitalSign.effectiveTime.Items.Count() > 0 && organizerVitalSign.effectiveTime is IVL_TS)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(organizerVitalSign.effectiveTime, out lowTime, out highTime);
                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime != "UNK")
                                    {
                                        dr[dsCCDA.VitalSigns.VitalSignDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else if (organizerVitalSign.effectiveTime != null && !string.IsNullOrWhiteSpace(organizerVitalSign.effectiveTime.value) &&
                                    organizerVitalSign.effectiveTime.value.ToLower() != "unk" && organizerVitalSign.effectiveTime.value.ToLower() != "ni")
                                {
                                    dr[dsCCDA.VitalSigns.VitalSignDateColumn.ColumnName] = organizerVitalSign.effectiveTime.value.ToFormatedDateTimeByFormat();
                                }
                                if (organizerVitalSign.statusCode != null && !string.IsNullOrWhiteSpace(organizerVitalSign.statusCode.code) &&
                                    (organizerVitalSign.statusCode.code.ToLower() == "completed" || organizerVitalSign.statusCode.code.ToLower() == "active"))
                                    dr[dsCCDA.VitalSigns.IsActiveColumn.ColumnName] = true;
                                else
                                    dr[dsCCDA.VitalSigns.IsActiveColumn.ColumnName] = false;
                                if (!organizerVitalSign.component.IsNullOrEmpty())
                                {
                                    foreach (Component4 componentVitalSign in organizerVitalSign.component)
                                    {
                                        if (componentVitalSign.Item != null)
                                        {
                                            Observation vitalSignObservation = componentVitalSign.Item as Observation;
                                            if (vitalSignObservation.code != null && !string.IsNullOrWhiteSpace(vitalSignObservation.code.code))
                                            {
                                                Recordexists = true;
                                                PQ valueVS = (PQ)vitalSignObservation.value[0];
                                                if (vitalSignObservation.code.code == "8302-2" || (!string.IsNullOrWhiteSpace(vitalSignObservation.code.displayName) && vitalSignObservation.code.displayName.ToLower() == "height"))
                                                {
                                                    double inches = 0;
                                                    if (valueVS.unit.ToLower() == "cm")
                                                    {
                                                        inches = MDVUtility.ToDouble(valueVS.value) * 0.393701;
                                                    }
                                                    dr[dsCCDA.VitalSigns.HeightColumn.ColumnName] = inches.ToString("0.00");
                                                }
                                                else if (vitalSignObservation.code.code == "29463-7" || (!string.IsNullOrWhiteSpace(vitalSignObservation.code.displayName) && vitalSignObservation.code.displayName == "patient body weight"))
                                                {
                                                    double weight = 0;
                                                    if (valueVS.unit.ToLower() == "kg")
                                                    {
                                                        weight = MDVUtility.ToDouble(valueVS.value) * 2.20462;
                                                    }
                                                    dr[dsCCDA.VitalSigns.WeightColumn.ColumnName] = weight.ToString("0.00");
                                                }
                                                else if (vitalSignObservation.code.code == "8462-4" || (!string.IsNullOrWhiteSpace(vitalSignObservation.code.displayName) && vitalSignObservation.code.displayName.ToLower() == "bp diastolic"))
                                                {
                                                    if (valueVS.unit.ToLower() == "mm[hg]")
                                                        dr[dsCCDA.VitalSigns.DiastolicColumn.ColumnName] = valueVS.value;
                                                    else
                                                        dr[dsCCDA.VitalSigns.DiastolicColumn.ColumnName] = valueVS.value;
                                                    dr[dsCCDA.VitalSigns.LOINICDiastolicColumn.ColumnName] = "8462-4";
                                                    if (vitalSignObservation.effectiveTime != null && vitalSignObservation.effectiveTime is IVL_TS)
                                                    {
                                                        dr[dsCCDA.VitalSigns.DiastolicDatetimeColumn.ColumnName] = vitalSignObservation.effectiveTime.value.ToFormatedDateTimeByFormat();
                                                    }
                                                }
                                                else if (vitalSignObservation.code.code == "8480-6" || (!string.IsNullOrWhiteSpace(vitalSignObservation.code.displayName) && vitalSignObservation.code.displayName.ToLower() == "intravascular systolic"))
                                                {
                                                    if (valueVS.unit.ToLower() == "mm[hg]")
                                                        dr[dsCCDA.VitalSigns.SystolicColumn.ColumnName] = valueVS.value;
                                                    else
                                                        dr[dsCCDA.VitalSigns.SystolicColumn.ColumnName] = valueVS.value;
                                                    dr[dsCCDA.VitalSigns.LOINICSystolicColumn.ColumnName] = "8480-6";
                                                    if (vitalSignObservation.effectiveTime != null && vitalSignObservation.effectiveTime is IVL_TS)
                                                    {
                                                        dr[dsCCDA.VitalSigns.SystolicDatetimeColumn.ColumnName] = vitalSignObservation.effectiveTime.value.ToFormatedDateTimeByFormat();
                                                    }
                                                }
                                                else if (vitalSignObservation.code.code == "8867-4" || (!string.IsNullOrWhiteSpace(vitalSignObservation.code.displayName) && vitalSignObservation.code.displayName.ToLower() == "heart rate"))
                                                {
                                                    if (valueVS.unit.ToLower().Contains("min"))
                                                        dr[dsCCDA.VitalSigns.PulseResultColumn.ColumnName] = valueVS.value;
                                                    else
                                                        dr[dsCCDA.VitalSigns.PulseResultColumn.ColumnName] = valueVS.value;
                                                    if (vitalSignObservation.effectiveTime != null && vitalSignObservation.effectiveTime is IVL_TS)
                                                    {
                                                        dr[dsCCDA.VitalSigns.PulseDatetimeColumn.ColumnName] = vitalSignObservation.effectiveTime.value.ToFormatedDateTimeByFormat();
                                                    }
                                                }
                                                else if (vitalSignObservation.code.code == "59408-5" || (!string.IsNullOrWhiteSpace(vitalSignObservation.code.displayName) && vitalSignObservation.code.displayName.ToLower() == "o2 % bldC oximetry"))
                                                {
                                                    if (valueVS.unit.ToLower().Contains("%"))
                                                        dr[dsCCDA.VitalSigns.SPO2Column.ColumnName] = valueVS.value;
                                                    else
                                                        dr[dsCCDA.VitalSigns.SPO2Column.ColumnName] = valueVS.value;
                                                }
                                                else if (vitalSignObservation.code.code == "8310-5" || (!string.IsNullOrWhiteSpace(vitalSignObservation.code.displayName) && vitalSignObservation.code.displayName.ToLower() == "body temperature"))
                                                {
                                                    double temprature = 0;
                                                    if (valueVS.unit.ToLower() == "cel")
                                                    {
                                                        temprature = (MDVUtility.ToDouble(valueVS.value) * 9 / 5) + 32;
                                                    }
                                                    else
                                                    {
                                                        temprature = MDVUtility.ToDouble(valueVS.value);
                                                    }
                                                    dr[dsCCDA.VitalSigns.TemperatureResultColumn.ColumnName] = temprature.ToString("0.00");
                                                    if (vitalSignObservation.effectiveTime != null && vitalSignObservation.effectiveTime is IVL_TS)
                                                    {
                                                        dr[dsCCDA.VitalSigns.TemperatureDatetimeColumn.ColumnName] = vitalSignObservation.effectiveTime.value.ToFormatedDateTimeByFormat();
                                                    }
                                                }
                                                else if (vitalSignObservation.code.code == "9279-1" || (!string.IsNullOrWhiteSpace(vitalSignObservation.code.displayName) && vitalSignObservation.code.displayName.ToLower() == "respiratory rate"))
                                                {
                                                    if (valueVS.unit.ToLower().Contains("min"))
                                                        dr[dsCCDA.VitalSigns.RespirationResultColumn.ColumnName] = valueVS.value;
                                                    else
                                                        dr[dsCCDA.VitalSigns.RespirationResultColumn.ColumnName] = valueVS.value;
                                                    if (vitalSignObservation.effectiveTime != null && vitalSignObservation.effectiveTime is IVL_TS)
                                                    {
                                                        dr[dsCCDA.VitalSigns.RespirationDatetimeColumn.ColumnName] = vitalSignObservation.effectiveTime.value.ToFormatedDateTimeByFormat();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {

                                }
                                if (Recordexists)
                                {
                                    dr[dsCCDA.VitalSigns.CreatedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.VitalSigns.CreatedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.VitalSigns.ModifiedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.VitalSigns.ModifiedOnColumn.ColumnName] = CreatedOn;
                                    dtVitalSigns.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Vital Signs: " + ex.Message);
            }
        }

        #endregion Vital Signs

        #region Results

        private static void GetResultsData(ClinicalDocument document, ref DataTable dtResults, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 resultsComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref resultsComp, DocumentSections.ResultDetail);
                List<Entry> entryList = null;
                if (isComponentExist && resultsComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e.Item is Organizer)
                        {
                            Organizer organizerResults = e.Item as Organizer;
                            if (!organizerResults.component.IsNullOrEmpty())
                            {
                                foreach (Component4 componentResult in organizerResults.component)
                                {
                                    if (componentResult.Item is Observation)
                                    {
                                        DataRow dr = dtResults.NewRow();
                                        Observation observationResult = componentResult.Item as Observation;
                                        dr[dsCCDA.ResultDetail.PatientIdColumn.ColumnName] = patientId;
                                        dr[dsCCDA.ResultDetail.ResultDetailIdColumn.ColumnName] = i--;
                                        if (organizerResults.code != null && !string.IsNullOrWhiteSpace(organizerResults.code.code))
                                        {
                                            dr[dsCCDA.ResultDetail.CategoryCodeColumn.ColumnName] = organizerResults.code.code;
                                            dr[dsCCDA.ResultDetail.CategoryNameColumn.ColumnName] = organizerResults.code.displayName;
                                            dr[dsCCDA.ResultDetail.CategoryCodeSystemColumn.ColumnName] = organizerResults.code.codeSystem;
                                            dr[dsCCDA.ResultDetail.CategoryCodeSystemNameColumn.ColumnName] = organizerResults.code.codeSystemName;
                                        }
                                        if (observationResult.effectiveTime != null && observationResult.effectiveTime.Items != null && observationResult.effectiveTime.Items.Count() > 0 && observationResult.effectiveTime is IVL_TS)
                                        {
                                            string lowTime, highTime;
                                            GetLowHighTime(observationResult.effectiveTime, out lowTime, out highTime);
                                            if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                            {
                                                dr[dsCCDA.ResultDetail.ObservationDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                            }
                                        }
                                        else if (observationResult.effectiveTime != null && !string.IsNullOrWhiteSpace(observationResult.effectiveTime.value) &&
                                        observationResult.effectiveTime.value.ToLower() != "unk" && observationResult.effectiveTime.value.ToLower() != "ni")
                                        {
                                            dr[dsCCDA.ResultDetail.ObservationDateColumn.ColumnName] = observationResult.effectiveTime.value.ToFormatedDateTimeByFormat();
                                        }
                                        if (observationResult.code != null && !string.IsNullOrWhiteSpace(observationResult.code.code))
                                        {
                                            dr[dsCCDA.ResultDetail.LOINCCodeColumn.ColumnName] = observationResult.code.code;
                                            dr[dsCCDA.ResultDetail.LOINCDescriptionColumn.ColumnName] = observationResult.code.displayName;
                                        }
                                        if (!observationResult.interpretationCode.IsNullOrEmpty() && observationResult.interpretationCode.Count() > 0 && !string.IsNullOrWhiteSpace(observationResult.interpretationCode[0].code))
                                        {
                                            dr[dsCCDA.ResultDetail.FlagCodeColumn.ColumnName] = observationResult.interpretationCode[0].code;
                                            dr[dsCCDA.ResultDetail.FlagColumn.ColumnName] = GetFlag(observationResult.interpretationCode[0].code);
                                        }
                                        if (observationResult.statusCode != null && !string.IsNullOrWhiteSpace(observationResult.statusCode.code))
                                            dr[dsCCDA.SocialHistory.StatusColumn.ColumnName] = observationResult.statusCode.code;
                                        dr[dsCCDA.SocialHistory.IsActiveColumn.ColumnName] = true;

                                        if (observationResult.value != null && !observationResult.value.IsNullOrEmpty() && observationResult.value[0] is ST)
                                        {
                                            ST resultValue = observationResult.value[0] as ST;
                                            dr[dsCCDA.ResultDetail.ResultValueColumn.ColumnName] = resultValue.Text[0];
                                        }
                                        else if (observationResult.value != null && !observationResult.value.IsNullOrEmpty() && observationResult.value[0] is PQ)
                                        {
                                            PQ resultValue = observationResult.value[0] as PQ;
                                            string strValue = string.Empty;
                                            if (resultValue.unit != "1")
                                            {
                                                strValue = resultValue.value + " " + resultValue.unit;
                                            }
                                            else
                                            {
                                                strValue = resultValue.value;
                                            }
                                            dr[dsCCDA.ResultDetail.ResultValueColumn.ColumnName] = strValue;
                                        }
                                        else if (observationResult.value != null && !observationResult.value.IsNullOrEmpty() && observationResult.value[0] is CO)
                                        {
                                            CO resultValue = observationResult.value[0] as CO;
                                            dr[dsCCDA.ResultDetail.ResultValueColumn.ColumnName] = resultValue.displayName;
                                        }

                                        if (!observationResult.referenceRange.IsNullOrEmpty())
                                        {
                                            foreach (ReferenceRange referenceRange in observationResult.referenceRange)
                                            {
                                                if (referenceRange.observationRange != null && referenceRange.observationRange.value != null && referenceRange.observationRange.value is CO)
                                                {
                                                    CO observationRange = referenceRange.observationRange.value as CO;
                                                    dr[dsCCDA.ResultDetail.RangeColumn.ColumnName] = observationRange.displayName;
                                                    dr[dsCCDA.ResultDetail.RangeCodeColumn.ColumnName] = observationRange.code;
                                                }
                                                else if (referenceRange.observationRange != null && referenceRange.observationRange.value != null && referenceRange.observationRange.value is ST)
                                                {
                                                    ST observationRange = referenceRange.observationRange.value as ST;
                                                    dr[dsCCDA.ResultDetail.RangeColumn.ColumnName] = observationRange.Text[0];
                                                }
                                                else if (referenceRange.observationRange != null && referenceRange.observationRange.value != null && referenceRange.observationRange.value is IVL_PQ)
                                                {
                                                    IVL_PQ observationRange = referenceRange.observationRange.value as IVL_PQ;
                                                    string strRange = string.Empty;
                                                    if (observationRange != null && observationRange.Items.Count() > 0 && observationRange.Items[0] != null && !string.IsNullOrWhiteSpace(observationRange.Items[0].value))
                                                        strRange = observationRange.Items[0].value + " - ";
                                                    if (observationRange != null && observationRange.Items.Count() > 1 && observationRange.Items[1] != null && !string.IsNullOrWhiteSpace(observationRange.Items[1].value))
                                                        strRange += observationRange.Items[1].value;
                                                    dr[dsCCDA.ResultDetail.RangeColumn.ColumnName] = strRange;
                                                }
                                                else if (referenceRange.observationRange != null && referenceRange.observationRange.text != null && referenceRange.observationRange.text is ED)
                                                {
                                                    ED observationRange = referenceRange.observationRange.text as ED;
                                                    dr[dsCCDA.ResultDetail.RangeColumn.ColumnName] = observationRange.Text[0];
                                                }

                                            }
                                        }
                                        if (dr[dsCCDA.ResultDetail.LOINCCodeColumn.ColumnName] != DBNull.Value && dr[dsCCDA.ResultDetail.LOINCDescriptionColumn.ColumnName] != DBNull.Value)
                                        {
                                            dr[dsCCDA.ResultDetail.ProviderIdColumn.ColumnName] = ProviderId;
                                            if (!string.IsNullOrWhiteSpace(FacilityId))
                                                dr[dsCCDA.ResultDetail.FacilityIdColumn.ColumnName] = FacilityId;
                                            dr[dsCCDA.ResultDetail.EntityIdColumn.ColumnName] = EntityId;
                                            dr[dsCCDA.ResultDetail.UserIdColumn.ColumnName] = UserId;
                                            dr[dsCCDA.ResultDetail.CreatedByColumn.ColumnName] = CreatedBy;
                                            dr[dsCCDA.ResultDetail.CreatedOnColumn.ColumnName] = CreatedOn;
                                            dr[dsCCDA.ResultDetail.ModifiedByColumn.ColumnName] = CreatedBy;
                                            dr[dsCCDA.ResultDetail.ModifiedOnColumn.ColumnName] = CreatedOn;
                                            dtResults.Rows.Add(dr);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Results: " + ex.Message);
            }
        }

        public static string GetFlag(string Code)
        {
            switch (Code)
            {
                case "N":
                    return "normal";
                case "MS":
                    return "moderately susceptible";
                case "LX":
                    return "below low threshold";
                case "IND":
                    return "indeterminate";
                case "I":
                    return "intermediate";
                case "HX":
                    return "above high threshold";
                case "D":
                    return "decreased";
                case "Carrier":
                    return "carrier";
                case "B":
                    return "better";
                case "A":
                    return "abnormal";
                default:
                    return Code;
            }
        }

        #endregion Results

        #region Social History

        private static void GetSocialHistoryData(ClinicalDocument document, ref DataTable dtSocialHistory, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 socialHistoryComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref socialHistoryComp, DocumentSections.SocialHistory);
                List<Entry> entryList = null;
                if (isComponentExist && socialHistoryComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation socialHistory = e.Item as Observation;
                            DataRow dr = dtSocialHistory.NewRow();
                            dr[dsCCDA.SocialHistory.PatientIdColumn.ColumnName] = patientId;
                            dr[dsCCDA.SocialHistory.TobaccoIdColumn.ColumnName] = i--;
                            if (!socialHistory.templateId.IsNullOrEmpty() && socialHistory.templateId.Count() > 0)
                            {
                                II templateId = socialHistory.templateId[0] as II;
                                if (!string.IsNullOrWhiteSpace(templateId.root) && (templateId.root == "2.16.840.1.113883.10.20.22.4.85" || templateId.root == "2.16.840.1.113883.10.20.22.4.78"))
                                {
                                    if (templateId.root == "2.16.840.1.113883.10.20.22.4.85")
                                    {
                                        dr[dsCCDA.SocialHistory.HistoryTypeColumn.ColumnName] = "Tobacco use";
                                    }
                                    else if (templateId.root == "2.16.840.1.113883.10.20.22.4.78")
                                    {
                                        dr[dsCCDA.SocialHistory.HistoryTypeColumn.ColumnName] = "Smoking Status";
                                    }
                                    if (socialHistory.code != null && !string.IsNullOrWhiteSpace(socialHistory.code.code))
                                    {
                                        dr[dsCCDA.SocialHistory.ObservationCodeColumn.ColumnName] = socialHistory.code.code;
                                        dr[dsCCDA.SocialHistory.ObservationDescriptionColumn.ColumnName] = socialHistory.code.displayName;
                                    }
                                    if (socialHistory.effectiveTime != null && socialHistory.effectiveTime.Items != null && socialHistory.effectiveTime.Items.Count() > 0 && socialHistory.effectiveTime is IVL_TS)
                                    {
                                        string lowTime, highTime;
                                        GetLowHighTime(socialHistory.effectiveTime, out lowTime, out highTime);
                                        if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                        {
                                            dr[dsCCDA.SocialHistory.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                        }
                                        if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                        {
                                            dr[dsCCDA.SocialHistory.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                        }
                                    }
                                    else if (socialHistory.effectiveTime != null && !string.IsNullOrWhiteSpace(socialHistory.effectiveTime.value) &&
                                    socialHistory.effectiveTime.value.ToLower() != "unk" && socialHistory.effectiveTime.value.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.SocialHistory.StartDateColumn.ColumnName] = socialHistory.effectiveTime.value.ToFormatedDateTimeByFormat();
                                    }

                                    if (socialHistory.statusCode != null && !string.IsNullOrWhiteSpace(socialHistory.statusCode.code))
                                        dr[dsCCDA.SocialHistory.StatusColumn.ColumnName] = socialHistory.statusCode.code;
                                    dr[dsCCDA.SocialHistory.IsActiveColumn.ColumnName] = true;

                                    if (socialHistory.value != null && !socialHistory.value.IsNullOrEmpty() && socialHistory.value[0] != null && socialHistory.value[0] is CD)
                                    {
                                        CD SocialHistoryValue = socialHistory.value[0] as CD;
                                        dr[dsCCDA.SocialHistory.SNOMEDCTCodeColumn.ColumnName] = SocialHistoryValue.code;
                                        dr[dsCCDA.SocialHistory.DescriptionColumn.ColumnName] = SocialHistoryValue.displayName;
                                    }
                                    else if (socialHistory.value != null && !socialHistory.value.IsNullOrEmpty() && socialHistory.value[0] != null && socialHistory.value[0] is ST)
                                    {
                                        ST st = socialHistory.value[0] as ST;
                                        dr[dsCCDA.SocialHistory.DescriptionColumn.ColumnName] = st.Text[0];
                                    }


                                    if (templateId.root == "2.16.840.1.113883.10.20.22.4.78")
                                    {
                                        string Observations = string.Empty;
                                        foreach (Entry eInner in entryList)
                                        {
                                            if (eInner != null && eInner.Item != null && eInner.Item is Observation)
                                            {
                                                Observation socialHistoryObservation = eInner.Item as Observation;
                                                if (!socialHistoryObservation.templateId.IsNullOrEmpty() && socialHistoryObservation.templateId.Count() > 0)
                                                {
                                                    II templateIdInner = socialHistoryObservation.templateId[0] as II;
                                                    if (templateIdInner.root == "2.16.840.1.113883.10.20.22.4.38")
                                                    {
                                                        if (socialHistoryObservation.value != null && !socialHistoryObservation.value.IsNullOrEmpty() && socialHistoryObservation.value[0] != null && socialHistoryObservation.value[0] is ST)
                                                        {
                                                            ST st = socialHistoryObservation.value[0] as ST;
                                                            Observations += st.Text[0] + ", ";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        dr[dsCCDA.SocialHistory.CommentsColumn.ColumnName] = Observations;
                                    }
                                    dr[dsCCDA.SocialHistory.SoapTextColumn.ColumnName] = GetSoapGetHistory(dr);
                                    dr[dsCCDA.SocialHistory.EntityIdColumn.ColumnName] = MDVUtility.ToInt64(EntityId);
                                    dr[dsCCDA.SocialHistory.UserIdColumn.ColumnName] = UserId;
                                    dr[dsCCDA.SocialHistory.CreatedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.SocialHistory.CreatedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.SocialHistory.ModifiedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.SocialHistory.ModifiedOnColumn.ColumnName] = CreatedOn;
                                    dtSocialHistory.Rows.Add(dr);
                                }
                                else if (!string.IsNullOrWhiteSpace(templateId.root) && templateId.root == "2.16.840.1.113883.10.20.22.4.200")
                                {
                                    if (socialHistory.value != null && !socialHistory.value.IsNullOrEmpty() && socialHistory.value[0] != null && socialHistory.value[0] is CD)
                                    {
                                        CD SocialHistoryValue = socialHistory.value[0] as CD;
                                        if (!string.IsNullOrWhiteSpace(SocialHistoryValue.displayName))
                                        {
                                            dr[dsCCDA.SocialHistory.BirthSexColumn.ColumnName] = SocialHistoryValue.displayName;
                                        }
                                    }
                                    else if (socialHistory.value != null && !socialHistory.value.IsNullOrEmpty() && socialHistory.value[0] != null && socialHistory.value[0] is ST)
                                    {
                                        ST st = socialHistory.value[0] as ST;
                                        dr[dsCCDA.SocialHistory.BirthSexColumn.ColumnName] = st.Text[0];
                                    }
                                    if (dr[dsCCDA.SocialHistory.BirthSexColumn.ColumnName] != DBNull.Value)
                                    {
                                        dr[dsCCDA.SocialHistory.EntityIdColumn.ColumnName] = MDVUtility.ToInt64(EntityId);
                                        dr[dsCCDA.SocialHistory.UserIdColumn.ColumnName] = UserId;
                                        dr[dsCCDA.SocialHistory.CreatedByColumn.ColumnName] = CreatedBy;
                                        dr[dsCCDA.SocialHistory.CreatedOnColumn.ColumnName] = CreatedOn;
                                        dr[dsCCDA.SocialHistory.ModifiedByColumn.ColumnName] = CreatedBy;
                                        dr[dsCCDA.SocialHistory.ModifiedOnColumn.ColumnName] = CreatedOn;
                                        dtSocialHistory.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Social History: " + ex.Message);
            }
        }

        public static string GetSoapGetHistory(DataRow item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='socialTobacco_' title='Tobacco'  name='Social Hx'><strong>Tobacco: </strong>");
            sb.Append(item["Description"]);
            sb.Append(", " + item["Comments"]);
            sb.Append("</div>");
            return sb.ToString();
        }

        #endregion Social History

        #region Laboratory Test

        private static void GetLaboratoryTestData(ClinicalDocument document, ref DataTable dtLaboratoryTest, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 laboratoryTestComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref laboratoryTestComp, DocumentSections.PlanOfTreatment);
                List<Entry> entryList = null;
                if (isComponentExist && laboratoryTestComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation laboratoryTest = e.Item as Observation;
                            if (laboratoryTest.moodCode != null && (laboratoryTest.moodCode.ToString().ToLower() == "rqo" || laboratoryTest.moodCode.ToString().ToLower() == "int"))
                            //if (laboratoryTest.moodCode != null && (laboratoryTest.moodCode.ToString().ToLower() == "rqo"))
                            {
                                DataRow dr = dtLaboratoryTest.NewRow();
                                dr[dsCCDA.LabOrderTest.PatientIdColumn.ColumnName] = patientId;
                                dr[dsCCDA.LabOrderTest.LabOrderTestIdColumn.ColumnName] = i;
                                if (laboratoryTest.code != null && !string.IsNullOrWhiteSpace(laboratoryTest.code.code))
                                {
                                    dr[dsCCDA.LabOrderTest.LOINCCodeColumn.ColumnName] = laboratoryTest.code.code;
                                    dr[dsCCDA.LabOrderTest.LOINCDescriptionColumn.ColumnName] = laboratoryTest.code.displayName;
                                }
                                if (laboratoryTest.effectiveTime != null && laboratoryTest.effectiveTime.Items != null &&
                                    laboratoryTest.effectiveTime.Items.Count() > 0 && laboratoryTest.effectiveTime is IVL_TS)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(laboratoryTest.effectiveTime, out lowTime, out highTime);
                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.LabOrderTest.TestDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else if (laboratoryTest.effectiveTime != null && !string.IsNullOrWhiteSpace(laboratoryTest.effectiveTime.value) &&
                                        laboratoryTest.effectiveTime.value.ToLower() != "unk" && laboratoryTest.effectiveTime.value.ToLower() != "ni")
                                {
                                    dr[dsCCDA.LabOrderTest.TestDateColumn.ColumnName] = laboratoryTest.effectiveTime.value.ToFormatedDateTimeByFormat();
                                }
                                if (laboratoryTest.statusCode != null && !string.IsNullOrWhiteSpace(laboratoryTest.statusCode.code))
                                {
                                    dr[dsCCDA.LabOrderTest.StatusColumn.ColumnName] = laboratoryTest.statusCode.code;
                                }
                                if (dr[dsCCDA.LabOrderTest.LOINCCodeColumn.ColumnName] != DBNull.Value)
                                {
                                    dr[dsCCDA.LabOrderTest.IsActiveColumn.ColumnName] = true;
                                    dr[dsCCDA.LabOrderTest.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                                    if (!string.IsNullOrWhiteSpace(FacilityId))
                                        dr[dsCCDA.LabOrderTest.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                                    dr[dsCCDA.LabOrderTest.CreatedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.LabOrderTest.CreatedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.LabOrderTest.ModifiedByColumn.ColumnName] = ModifiedBy;
                                    dr[dsCCDA.LabOrderTest.ModifiedOnColumn.ColumnName] = CreatedOn;
                                    dtLaboratoryTest.Rows.Add(dr);
                                    i--;
                                }
                                else
                                {
                                    errors.Add("Laboratory Test: LOINC does not exists.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Laboratory Test: " + ex.Message);
            }
        }

        #endregion Laboratory Test

        #region Goals

        private static void GetGoalsData(ClinicalDocument document, ref DataTable dtGoals, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 goalsComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref goalsComp, DocumentSections.Goals);
                List<Entry> entryList = null;
                if (isComponentExist && goalsComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation GoalObservation = e.Item as Observation;
                            DataRow dr = dtGoals.NewRow();
                            if (GoalObservation.code != null && !string.IsNullOrWhiteSpace(GoalObservation.code.code))
                            {
                                dr[dsCCDA.EnrolledGoals.LOINCCodeColumn.ColumnName] = GoalObservation.code.code;
                                dr[dsCCDA.EnrolledGoals.LOINCDescriptionColumn.ColumnName] = GoalObservation.code.displayName;
                                List<MDVision.Model.CCDA.CPTLookupModel> CPTLookupsList = BLLIMO.GetIMOCPTByLOINCCode(GoalObservation.code.code);
                                if (CPTLookupsList != null && CPTLookupsList.Count > 0)
                                {
                                    MDVision.Model.CCDA.CPTLookupModel objCPT = CPTLookupsList.FirstOrDefault(m => m.CPTCode != null || m.CPTCodeDescription != null);
                                    if (objCPT != null)
                                    {
                                        dr[dsCCDA.EnrolledGoals.CPTCodeColumn.ColumnName] = objCPT.CPTCode;
                                        dr[dsCCDA.EnrolledGoals.CPTDescriptionColumn.ColumnName] = objCPT.CPTCodeDescription;
                                    }
                                }
                            }
                            if (GoalObservation.statusCode != null && !string.IsNullOrWhiteSpace(GoalObservation.statusCode.code))
                            {
                                dr[dsCCDA.EnrolledGoals.StatusColumn.ColumnName] = GoalObservation.statusCode.code;
                            }
                            if (GoalObservation.effectiveTime != null && GoalObservation.effectiveTime.Items != null &&
                                    GoalObservation.effectiveTime.Items.Count() > 0 && GoalObservation.effectiveTime is IVL_TS)
                            {
                                string lowTime, highTime;
                                GetLowHighTime(GoalObservation.effectiveTime, out lowTime, out highTime);
                                if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                {
                                    dr[dsCCDA.EnrolledGoals.GoalDateTimeColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                }
                            }
                            else if (GoalObservation.effectiveTime != null && !string.IsNullOrWhiteSpace(GoalObservation.effectiveTime.value) &&
                                    GoalObservation.effectiveTime.value.ToLower() != "unk" && GoalObservation.effectiveTime.value.ToLower() != "ni")
                            {
                                dr[dsCCDA.EnrolledGoals.GoalDateTimeColumn.ColumnName] = GoalObservation.effectiveTime.value.ToFormatedDateTimeByFormat();
                            }
                            if (!GoalObservation.value.IsNullOrEmpty() && GoalObservation.value.Count() > 0)
                            {
                                string Instruction = string.Empty;
                                foreach (var instruction in GoalObservation.value)
                                {
                                    if (instruction != null && instruction is ST)
                                    {
                                        int counter = 0;
                                        foreach (string str in ((MDVision.IEHR.EMR.Helpers.Clinical.Summary.BIN)instruction).Text)
                                        {
                                            if (counter == 0)
                                            {
                                                Instruction += str;
                                            }
                                            else
                                            {
                                                Instruction += ", " + str;
                                            }
                                            counter++;
                                        }
                                    }
                                }
                                dr[dsCCDA.EnrolledGoals.InstructionColumn.ColumnName] = Instruction;
                            }
                            if (dr[dsCCDA.EnrolledGoals.LOINCCodeColumn.ColumnName] != DBNull.Value)
                            {
                                dr[dsCCDA.EnrolledGoals.PatientIdColumn.ColumnName] = patientId;
                                dr[dsCCDA.EnrolledGoals.EnrolledGoalsIdColumn.ColumnName] = i;
                                dr[dsCCDA.EnrolledGoals.IsActiveColumn.ColumnName] = true;
                                dr[dsCCDA.EnrolledGoals.CreatedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.EnrolledGoals.CreatedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.EnrolledGoals.ModifiedByColumn.ColumnName] = ModifiedBy;
                                dr[dsCCDA.EnrolledGoals.ModifiedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.EnrolledGoals.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                                if (!string.IsNullOrWhiteSpace(FacilityId))
                                    dr[dsCCDA.EnrolledGoals.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                                dtGoals.Rows.Add(dr);
                                i--;
                            }
                        }
                    }
                }
                else
                {
                    errors.Add("Goals: component not exists.");
                }
            }
            catch (Exception ex)
            {
                errors.Add("Goals: " + ex.Message);
            }
        }

        #endregion Goals

        #region Encounter

        private static void GetEncounterData2(ClinicalDocument document, ref DataTable dtEncounter, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 encounterComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref encounterComp, DocumentSections.Encounter);
                List<Entry> entryList = null;
                if (isComponentExist && encounterComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Encounter)
                        {
                            Encounter encounter = e.Item as Encounter;
                            DataRow dr = dtEncounter.NewRow();
                            dr[dsCCDA.NotesEncounter.PatientIdColumn.ColumnName] = patientId;
                            if (encounter.effectiveTime != null && encounter.effectiveTime.Items != null &&
                                    encounter.effectiveTime.Items.Count() > 0 && encounter.effectiveTime is IVL_TS)
                            {
                                string lowTime, highTime;
                                GetLowHighTime(encounter.effectiveTime, out lowTime, out highTime);
                                if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                {
                                    dr[dsCCDA.NotesEncounter.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                }
                                if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                {
                                    dr[dsCCDA.NotesEncounter.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                }
                            }
                            else if (encounter.effectiveTime != null && !string.IsNullOrWhiteSpace(encounter.effectiveTime.value) &&
                                    encounter.effectiveTime.value.ToLower() != "unk" && encounter.effectiveTime.value.ToLower() != "ni")
                            {
                                dr[dsCCDA.NotesEncounter.StartDateColumn.ColumnName] = encounter.effectiveTime.value.ToFormatedDateTimeByFormat();
                            }
                            else
                            {
                                errors.Add("Encounter: DOS not found");
                            }

                            if (encounter.code != null && !string.IsNullOrWhiteSpace(encounter.code.code))
                            {
                                dr[dsCCDA.NotesEncounter.CodeColumn.ColumnName] = encounter.code.code;
                                if (encounter.code.codeSystem == CodeTypes.SNOMED)
                                {
                                    dr[dsCCDA.NotesEncounter.CodeTypeColumn.ColumnName] = "SNOMED";
                                }
                                else if (encounter.code.codeSystem == CodeTypes.HCPCS)
                                {
                                    dr[dsCCDA.NotesEncounter.CodeTypeColumn.ColumnName] = "HCPCS";
                                }
                                else if (encounter.code.codeSystem == CodeTypes.CPT)
                                {
                                    dr[dsCCDA.NotesEncounter.CodeTypeColumn.ColumnName] = "CPT";
                                }
                            }
                            dtEncounter.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Encounter: " + ex.Message);
            }
        }

        private static void GetEncounterData(ClinicalDocument document, ref DataTable dtEncounter, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 encounterComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref encounterComp, DocumentSections.Encounter);
                List<Entry> entryList = null;
                if (isComponentExist && encounterComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Encounter)
                        {
                            Encounter encounter = e.Item as Encounter;
                            if (!encounter.templateId.IsNullOrEmpty() && encounter.templateId[0] != null
                                && encounter.templateId[0].root == PlanOfTreatmentSection.Encounter)
                            {
                                DataRow dr = dtEncounter.NewRow();
                                dr[dsCCDA.NotesEncounter.PatientIdColumn.ColumnName] = patientId;
                                dr[dsCCDA.NotesEncounter.EncounterIdColumn.ColumnName] = i;
                                if (encounter.statusCode != null && !String.IsNullOrWhiteSpace(encounter.statusCode.code))
                                {
                                    dr[dsCCDA.NotesEncounter.StatusColumn.ColumnName] = encounter.statusCode.code;
                                }
                                if (encounter.effectiveTime != null && encounter.effectiveTime.Items != null &&
                                    encounter.effectiveTime.Items.Count() > 0 && encounter.effectiveTime is IVL_TS)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(encounter.effectiveTime, out lowTime, out highTime);
                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.NotesEncounter.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                        dr[dsCCDA.NotesEncounter.VisitDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.NotesEncounter.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else if (encounter.effectiveTime != null && !string.IsNullOrWhiteSpace(encounter.effectiveTime.value) &&
                                        encounter.effectiveTime.value.ToLower() != "unk" && encounter.effectiveTime.value.ToLower() != "ni")
                                {
                                    dr[dsCCDA.NotesEncounter.StartDateColumn.ColumnName] = encounter.effectiveTime.value.ToFormatedDateTimeByFormat();
                                    dr[dsCCDA.NotesEncounter.VisitDateColumn.ColumnName] = encounter.effectiveTime.value.ToFormatedDateTimeByFormat();
                                }
                                else
                                {
                                    errors.Add("Planned Encounter: DOS not found");
                                }

                                if (encounter.code != null && !string.IsNullOrWhiteSpace(encounter.code.code))
                                {
                                    dr[dsCCDA.NotesEncounter.CodeColumn.ColumnName] = encounter.code.code;
                                    dr[dsCCDA.NotesEncounter.SNOMEDIDColumn.ColumnName] = encounter.code.code;
                                    dr[dsCCDA.NotesEncounter.SNOMED_DESCRIPTIONColumn.ColumnName] = encounter.code.displayName;
                                    if (encounter.code.codeSystem == CodeTypes.SNOMED)
                                    {
                                        dr[dsCCDA.NotesEncounter.CodeTypeColumn.ColumnName] = "SNOMED";
                                    }
                                    else if (encounter.code.codeSystem == CodeTypes.HCPCS)
                                    {
                                        dr[dsCCDA.NotesEncounter.CodeTypeColumn.ColumnName] = "HCPCS";
                                    }
                                    else if (encounter.code.codeSystem == CodeTypes.CPT)
                                    {
                                        dr[dsCCDA.NotesEncounter.CodeTypeColumn.ColumnName] = "CPT";
                                    }
                                }

                                if (!encounter.entryRelationship.IsNullOrEmpty() && encounter.entryRelationship.Count() > 0)
                                {
                                    string strReason = string.Empty;
                                    foreach (EntryRelationship er in encounter.entryRelationship)
                                    {
                                        if (er.Item != null && er.Item is Act)
                                        {
                                            Act observationAct = (Act)er.Item;
                                            if (!observationAct.entryRelationship.IsNullOrEmpty() && observationAct.entryRelationship.Count() > 0)
                                            {
                                                foreach (EntryRelationship erInner in observationAct.entryRelationship)
                                                {
                                                    if (erInner.Item != null && erInner.Item is Observation)
                                                    {
                                                        Observation ObservationInner = erInner.Item as Observation;
                                                        if (!ObservationInner.value.IsNullOrEmpty() && ObservationInner.value.Count() > 0)
                                                        {
                                                            CD cdInner = ObservationInner.value[0] as CD;
                                                            EncounterSNOMEDIDs.Add(cdInner.code);
                                                            strReason += cdInner.displayName + " ";
                                                            dr[dsCCDA.NotesEncounter.SNOMED_DESCRIPTIONColumn.ColumnName] = cdInner.displayName;
                                                            dr[dsCCDA.NotesEncounter.SNOMEDIDColumn.ColumnName] = cdInner.code;
                                                            dr[dsCCDA.NotesEncounter.IsProblemColumn.ColumnName] = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    dr[dsCCDA.NotesEncounter.EncounterColumn.ColumnName] = strReason;
                                }
                                else
                                {
                                    dr[dsCCDA.NotesEncounter.IsProblemColumn.ColumnName] = false;
                                }
                                dr[dsCCDA.NotesEncounter.UserIdColumn.ColumnName] = UserId;
                                dr[dsCCDA.NotesEncounter.IsActiveColumn.ColumnName] = true;
                                dr[dsCCDA.NotesEncounter.CreatedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.NotesEncounter.CreatedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.NotesEncounter.ModifiedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.NotesEncounter.ModifiedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.NotesEncounter.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                dr[dsCCDA.NotesEncounter.FacilityIdColumn.ColumnName] = Convert.ToInt64(FacilityId);
                                dtEncounter.Rows.Add(dr);
                                i--;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Planned Encounter: " + ex.Message);
            }
        }

        public static void AddEncounterProblems(DataTable dtEncounter, string Assessment)
        {
            bool bResult = false;
            foreach (DataRow SNOMEDID in dtEncounter.Rows)
            {
                bResult = false;
                foreach (DataRow drProblem in dsCCDA.ProblemList.Rows)
                {
                    if (SNOMEDID["SNOMEDID"].ToString() == drProblem["SNOMEDID"].ToString())
                    {
                        bResult = true;
                    }
                }
                if (!bResult)
                {
                    #region Problem add
                    DataRow dr = dsCCDA.ProblemList.NewRow();
                    dr[dsCCDA.ProblemList.PatientIdColumn.ColumnName] = patientId;
                    dr[dsCCDA.ProblemList.StatusColumn.ColumnName] = SNOMEDID["Status"];
                    if (SNOMEDID["StartDate"] != DBNull.Value || SNOMEDID["EndDate"] != DBNull.Value)
                    {
                        if (SNOMEDID["StartDate"] != DBNull.Value)
                        {
                            dr[dsCCDA.ProblemList.StartDateColumn.ColumnName] = SNOMEDID["StartDate"];
                        }
                        if (SNOMEDID["EndDate"] != DBNull.Value)
                        {
                            dr[dsCCDA.ProblemList.EndDateColumn.ColumnName] = SNOMEDID["EndDate"];
                        }
                    }
                    dr[dsCCDA.ProblemList.ComplaintCommentsColumn.ColumnName] = FollowUp;
                    if (!string.IsNullOrWhiteSpace(Assessment))
                        dr[dsCCDA.ProblemList.CommentsColumn.ColumnName] = Assessment;
                    dr[dsCCDA.ProblemList.SNOMEDIDColumn.ColumnName] = SNOMEDID["SNOMEDID"];
                    dr[dsCCDA.ProblemList.ProblemNameColumn.ColumnName] = SNOMEDID["SNOMED_DESCRIPTION"];
                    dr[dsCCDA.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName] = SNOMEDID["SNOMED_DESCRIPTION"];
                    dr[dsCCDA.ProblemList.IsProblemColumn.ColumnName] = SNOMEDID["IsProblem"];
                    List<MDVision.Model.CCDA.ICDLookupModel> ICDLookupsList = BLLIMO.GetIMOICDbySNOMEDCode(SNOMEDID["SNOMEDID"].ToString());
                    if (ICDLookupsList != null && ICDLookupsList.Count > 0)
                    {
                        MDVision.Model.CCDA.ICDLookupModel objICD = ICDLookupsList.FirstOrDefault(m => m.ICD10CM_CODE != null || m.ICD10CM_TITLE != null || m.ICD9CM_CODE != null || m.ICD9CM_TITLE != null);
                        if (objICD != null)
                        {
                            dr[dsCCDA.ProblemList.ICD9Column.ColumnName] = objICD.ICD9CM_CODE;
                            dr[dsCCDA.ProblemList.ICD10Column.ColumnName] = objICD.ICD10CM_CODE;
                            dr[dsCCDA.ProblemList.ICD9_DescriptionColumn.ColumnName] = SNOMEDID["SNOMED_DESCRIPTION"];
                            dr[dsCCDA.ProblemList.ICD10_DescriptionColumn.ColumnName] = SNOMEDID["SNOMED_DESCRIPTION"];
                        }
                    }
                    dr[dsCCDA.ProblemList.IsActiveColumn.ColumnName] = true;
                    if (dr[dsCCDA.ProblemList.ProblemNameColumn.ColumnName] != DBNull.Value)
                    {
                        dr[dsCCDA.ProblemList.IsChiefComplaintColumn.ColumnName] = true;
                        dr[dsCCDA.ProblemList.ProblemListIdColumn.ColumnName] = -(dsCCDA.ProblemList.Rows.Count + 1);
                        dr[dsCCDA.ProblemList.IsActiveColumn.ColumnName] = 1;
                        dr[dsCCDA.ProblemList.CreatedByColumn.ColumnName] = CreatedBy;
                        dr[dsCCDA.ProblemList.CreatedOnColumn.ColumnName] = CreatedOn;
                        dr[dsCCDA.ProblemList.ModifiedByColumn.ColumnName] = CreatedBy;
                        dr[dsCCDA.ProblemList.ModifiedOnColumn.ColumnName] = CreatedOn;
                        dr[dsCCDA.ProblemList.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                        if (!string.IsNullOrWhiteSpace(FacilityId))
                            dr[dsCCDA.ProblemList.FacilityIdColumn.ColumnName] = Convert.ToInt64(FacilityId);
                        dr[dsCCDA.ProblemList.EntityIdColumn.ColumnName] = Convert.ToInt64(EntityId);
                        dr[dsCCDA.ProblemList.UserIdColumn.ColumnName] = Convert.ToInt64(UserId);
                        dsCCDA.ProblemList.Rows.Add(dr);
                    }
                    #endregion Problem add
                }
            }
        }

        #endregion Encounter

        #region Medical Device Equipment

        private static void GetMedicalDeviceEquipmentData(ClinicalDocument document, ref DataTable dtMedicalDeviceEquipment, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 mdeComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref mdeComp, DocumentSections.MedicalDeviceEquipment);
                List<Entry> entryList = null;
                if (isComponentExist && mdeComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Procedure)
                        {
                            Procedure MDE = e.Item as Procedure;
                            DataRow dr = dtMedicalDeviceEquipment.NewRow();
                            dr[dsCCDA.MedicalDeviceEquipment.PatientIdColumn.ColumnName] = patientId;
                            dr[dsCCDA.MedicalDeviceEquipment.MedicalDeviceEquipmentIdColumn.ColumnName] = i--;
                            if (MDE.effectiveTime != null && MDE.effectiveTime.Items != null &&
                                    MDE.effectiveTime.Items.Count() > 0 && MDE.effectiveTime is IVL_TS)
                            {
                                string lowTime, highTime;
                                GetLowHighTime(MDE.effectiveTime, out lowTime, out highTime);
                                if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                {
                                    dr[dsCCDA.MedicalDeviceEquipment.PlacedDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                }
                            }
                            else if (MDE.effectiveTime != null && !string.IsNullOrWhiteSpace(MDE.effectiveTime.value) &&
                                    MDE.effectiveTime.value.ToLower() != "unk" && MDE.effectiveTime.value.ToLower() != "ni")
                            {
                                dr[dsCCDA.MedicalDeviceEquipment.PlacedDateColumn.ColumnName] = MDE.effectiveTime.value.ToFormatedDateTimeByFormat();
                            }
                            else
                            {
                                errors.Add("Medical Device Equipment : Placed Date not found");
                            }
                            if (MDE.code != null && !string.IsNullOrWhiteSpace(MDE.code.code))
                            {
                                if (MDE.code.codeSystem == "2.16.840.1.113883.6.96")
                                {
                                    dr[dsCCDA.MedicalDeviceEquipment.SNOMEDIDColumn.ColumnName] = MDE.code.code;
                                    dr[dsCCDA.MedicalDeviceEquipment.SNOMED_DESCRIPTIONColumn.ColumnName] = MDE.code.displayName;
                                }
                                else
                                {
                                    dr[dsCCDA.MedicalDeviceEquipment.CPTCodeColumn.ColumnName] = MDE.code.code;
                                    dr[dsCCDA.MedicalDeviceEquipment.CPT_DESCRIPTIONColumn.ColumnName] = MDE.code.displayName;
                                }
                            }
                            //if (MDE.statusCode != null && !string.IsNullOrWhiteSpace(MDE.statusCode.code))
                            //{
                            //    dr[dsCCDA.MedicalDeviceEquipment.StatusColumn.ColumnName] = MDE.statusCode.code;
                            //}
                            if (!MDE.targetSiteCode.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(MDE.targetSiteCode[0].code))
                            {
                                dr[dsCCDA.MedicalDeviceEquipment.TargetSiteCodeColumn.ColumnName] = MDE.targetSiteCode[0].code;
                                dr[dsCCDA.MedicalDeviceEquipment.TargetSiteColumn.ColumnName] = MDE.targetSiteCode[0].displayName;
                            }
                            if (!MDE.participant.IsNullOrEmpty() && MDE.participant[0] != null)
                            {
                                Participant2 participant = MDE.participant[0] as Participant2;
                                if (participant.participantRole != null)
                                {
                                    if (participant.participantRole.id != null && participant.participantRole.id[0] != null && !string.IsNullOrWhiteSpace(participant.participantRole.id[0].extension))
                                    {
                                        dr[dsCCDA.MedicalDeviceEquipment.UDIColumn.ColumnName] = participant.participantRole.id[0].extension;
                                    }
                                    if ((MDVision.IEHR.EMR.Helpers.Clinical.Summary.Device)participant.participantRole.Item != null)
                                    {
                                        Device device = participant.participantRole.Item as Device;
                                        if (device != null && device.code != null && !string.IsNullOrWhiteSpace(device.code.code))
                                        {
                                            dr[dsCCDA.MedicalDeviceEquipment.DeviceIdentifierColumn.ColumnName] = device.code.code;
                                            dr[dsCCDA.MedicalDeviceEquipment.DeviceDescriptionColumn.ColumnName] = device.code.displayName;
                                        }
                                        else if (device != null && device.code != null && device.code.originalText != null)
                                        {

                                        }
                                    }
                                }
                            }
                            if (dr[dsCCDA.MedicalDeviceEquipment.UDIColumn.ColumnName] != DBNull.Value)
                            {
                                ImplantableDevices model = new ImplantableDevices();
                                model.UDI = dr[dsCCDA.MedicalDeviceEquipment.UDIColumn.ColumnName].ToString();
                                model.DI = dr[dsCCDA.MedicalDeviceEquipment.DeviceIdentifierColumn.ColumnName].ToString();
                                model = new ImplantableHelper().GetUDIData(model);
                                if (model.Status == "UDI Known")
                                {
                                    dr[dsCCDA.MedicalDeviceEquipment.StatusColumn.ColumnName] = "Known";
                                    dr[dsCCDA.MedicalDeviceEquipment.IssuingAgencyColumn.ColumnName] = model.Issuing_agency;
                                    if (!string.IsNullOrWhiteSpace(model.Manufacturing_Date))
                                        dr[dsCCDA.MedicalDeviceEquipment.ManufacturingDateColumn.ColumnName] = MDVUtility.ToDateTime(model.Manufacturing_Date).ToString("MM/dd/yyyy");
                                    dr[dsCCDA.MedicalDeviceEquipment.SerialNumberColumn.ColumnName] = model.Serial_Number;
                                    if (!string.IsNullOrWhiteSpace(model.Expiration_Date))
                                        dr[dsCCDA.MedicalDeviceEquipment.ExpirationDateColumn.ColumnName] = MDVUtility.ToDateTime(model.Expiration_Date).ToString("MM/dd/yyyy");
                                    dr[dsCCDA.MedicalDeviceEquipment.LotNumberColumn.ColumnName] = model.Lot_Number;
                                    dr[dsCCDA.MedicalDeviceEquipment.BrandNameColumn.ColumnName] = model.BrandName;
                                    dr[dsCCDA.MedicalDeviceEquipment.VersionModelNumberColumn.ColumnName] = model.VersionModelNumber;
                                    dr[dsCCDA.MedicalDeviceEquipment.CompanyNameColumn.ColumnName] = model.CompanyName;
                                    dr[dsCCDA.MedicalDeviceEquipment.MRISafetyStatusColumn.ColumnName] = model.MRISafetyStatus;
                                    dr[dsCCDA.MedicalDeviceEquipment.LabeledContainsNRLColumn.ColumnName] = model.LabeledContainsNRL;
                                    dr[dsCCDA.MedicalDeviceEquipment.GMDNPNameColumn.ColumnName] = model.GMDNPName;
                                }
                                else
                                {
                                    dr[dsCCDA.MedicalDeviceEquipment.StatusColumn.ColumnName] = "Unknown";
                                }
                                dr[dsCCDA.MedicalDeviceEquipment.IsActiveColumn.ColumnName] = true;
                                dr[dsCCDA.MedicalDeviceEquipment.UserIdColumn.ColumnName] = MDVUtility.ToInt64(UserId);
                                dr[dsCCDA.MedicalDeviceEquipment.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                                if (!string.IsNullOrWhiteSpace(FacilityId))
                                    dr[dsCCDA.MedicalDeviceEquipment.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                                dr[dsCCDA.MedicalDeviceEquipment.CreatedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.MedicalDeviceEquipment.CreatedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.MedicalDeviceEquipment.ModifiedByColumn.ColumnName] = ModifiedBy;
                                dr[dsCCDA.MedicalDeviceEquipment.ModifiedOnColumn.ColumnName] = CreatedOn;
                                dtMedicalDeviceEquipment.Rows.Add(dr);
                            }
                            else
                            {
                                errors.Add("Medical Device Equipment : Playing device not found");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Medical Device Equipment: " + ex.Message);
            }
        }

        #endregion Medical Device Equipment

        #region Health Concerns

        private static void GetHealthConcernData(ClinicalDocument document, ref DataTable dtHealthConcern, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 mdeComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref mdeComp, DocumentSections.HealthConcerns);
                List<Entry> entryList = null;
                if (isComponentExist && mdeComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation observation = e.Item as Observation;
                            DataRow dr = dtHealthConcern.NewRow();
                            dr[dsCCDA.HealthConcerns.PatientIdColumn.ColumnName] = patientId;
                            dr[dsCCDA.HealthConcerns.HealthConcernIdColumn.ColumnName] = i--;
                            if (!observation.value.IsNullOrEmpty() && observation.value[0] is CD
                                       && !String.IsNullOrWhiteSpace((observation.value[0] as CD).code))
                            {
                                CD snomedID = observation.value[0] as CD;
                                if (snomedID != null && !string.IsNullOrWhiteSpace(snomedID.code))
                                {
                                    dr[dsCCDA.HealthConcerns.ObservationSNOMEDIDColumn.ColumnName] = snomedID.code;
                                    dr[dsCCDA.HealthConcerns.ObservationSNOMEDDescColumn.ColumnName] = snomedID.displayName;
                                    List<MDVision.Model.CCDA.ICDLookupModel> ICDLookupsList = BLLIMO.GetIMOICDbySNOMEDCode(snomedID.code);
                                    if (ICDLookupsList != null && ICDLookupsList.Count() > 0)
                                    {
                                        MDVision.Model.CCDA.ICDLookupModel objICD = ICDLookupsList.FirstOrDefault(m => m.ICD10CM_CODE != null || m.ICD10CM_TITLE != null || m.ICD9CM_CODE != null || m.ICD9CM_TITLE != null);
                                        if (objICD != null)
                                        {
                                            dr[dsCCDA.HealthConcerns.ObservationICD9CodeColumn.ColumnName] = objICD.ICD9CM_CODE;
                                            dr[dsCCDA.HealthConcerns.ObservationICD10CodeColumn.ColumnName] = objICD.ICD10CM_CODE;
                                            dr[dsCCDA.HealthConcerns.ObservationICD9DescriptionColumn.ColumnName] = snomedID.displayName;
                                            dr[dsCCDA.HealthConcerns.ObservationICD10DescriptionColumn.ColumnName] = snomedID.displayName;
                                        }
                                    }
                                    if (document.effectiveTime != null && document.effectiveTime is TS && document.effectiveTime.value != null &&
                                    document.effectiveTime.value.ToLower() != "unk" && document.effectiveTime.value.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.HealthConcerns.ObservationDateColumn.ColumnName] = document.effectiveTime.value.ToFormatedDateTimeByFormat();
                                    }
                                    dr[dsCCDA.HealthConcerns.IsActiveColumn.ColumnName] = true;
                                    if (observation.statusCode != null && !string.IsNullOrWhiteSpace(observation.statusCode.code))
                                        dr[dsCCDA.HealthConcerns.ObservationStatusColumn.ColumnName] = observation.statusCode.code;

                                    dr[dsCCDA.HealthConcerns.UserIdColumn.ColumnName] = UserId;
                                    dr[dsCCDA.HealthConcerns.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                                    if (!string.IsNullOrWhiteSpace(FacilityId))
                                        dr[dsCCDA.HealthConcerns.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                                    dr[dsCCDA.HealthConcerns.CreatedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.HealthConcerns.CreatedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.HealthConcerns.ModifiedByColumn.ColumnName] = ModifiedBy;
                                    dr[dsCCDA.HealthConcerns.ModifiedOnColumn.ColumnName] = CreatedOn;
                                    dtHealthConcern.Rows.Add(dr);
                                }
                            }

                        }
                        else if (e != null && e.Item != null && e.Item is Act)
                        {
                            Act concernAct = e.Item as Act;
                            if (concernAct.code != null && !string.IsNullOrWhiteSpace(concernAct.code.code) && concernAct.code.code == "75310-3")
                            {
                                if (!concernAct.entryRelationship.IsNullOrEmpty() && concernAct.entryRelationship[0].Item != null
                                && concernAct.entryRelationship[0].Item is Act)
                                {
                                    foreach (EntryRelationship er in concernAct.entryRelationship)
                                    {
                                        if (er.Item != null && er.Item is Act)
                                        {
                                            Act concernActInner = (Act)er.Item;
                                            DataRow dr = dtHealthConcern.NewRow();
                                            dr[dsCCDA.HealthConcerns.PatientIdColumn.ColumnName] = patientId;
                                            dr[dsCCDA.HealthConcerns.HealthConcernIdColumn.ColumnName] = i--;
                                            if (dsCCDA.ProblemList != null && dsCCDA.ProblemList.Rows.Count > 0)
                                            {
                                                if (!concernActInner.id.IsNullOrEmpty() && concernActInner.id[0] != null && !string.IsNullOrWhiteSpace(concernActInner.id[0].root))
                                                {
                                                    foreach (DataRow drProblem in dsCCDA.ProblemList.Rows)
                                                    {
                                                        string strProblemCode = drProblem["RootCode"].ToString();
                                                        if (concernActInner.id[0].root == strProblemCode)
                                                        {
                                                            dr[dsCCDA.HealthConcerns.ConcernsICD9CodeColumn.ColumnName] = drProblem["ICD9"];
                                                            dr[dsCCDA.HealthConcerns.ConcernsICD10CodeColumn.ColumnName] = drProblem["ICD10"];
                                                            dr[dsCCDA.HealthConcerns.ConcernsICD9DescriptionColumn.ColumnName] = drProblem["ICD9_Description"];
                                                            dr[dsCCDA.HealthConcerns.ConcernsICD10DescriptionColumn.ColumnName] = drProblem["ICD10_Description"];
                                                            dr[dsCCDA.HealthConcerns.ConcernsSNOMEDIDColumn.ColumnName] = drProblem["SNOMEDID"];
                                                            dr[dsCCDA.HealthConcerns.ConcernsSNOMEDDescColumn.ColumnName] = drProblem["SNOMED_DESCRIPTION"];
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            string code = concernActInner.id[0].root.Split('-').Last();
                                                            string InnerstrProblemCode = strProblemCode.Split('-').Last();
                                                            if (!string.IsNullOrWhiteSpace(code))
                                                            {
                                                                if (InnerstrProblemCode == code)
                                                                {
                                                                    dr[dsCCDA.HealthConcerns.ConcernsICD9CodeColumn.ColumnName] = drProblem["ICD9"];
                                                                    dr[dsCCDA.HealthConcerns.ConcernsICD10CodeColumn.ColumnName] = drProblem["ICD10"];
                                                                    dr[dsCCDA.HealthConcerns.ConcernsICD9DescriptionColumn.ColumnName] = drProblem["ICD9_Description"];
                                                                    dr[dsCCDA.HealthConcerns.ConcernsICD10DescriptionColumn.ColumnName] = drProblem["ICD10_Description"];
                                                                    dr[dsCCDA.HealthConcerns.ConcernsSNOMEDIDColumn.ColumnName] = drProblem["SNOMEDID"];
                                                                    dr[dsCCDA.HealthConcerns.ConcernsSNOMEDDescColumn.ColumnName] = drProblem["SNOMED_DESCRIPTION"];
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    string InnerstrProblemCodeInner = InnerstrProblemCode.Remove(InnerstrProblemCode.Length - 1);
                                                                    string codeInner = code.Remove(code.Length - 1);
                                                                    if (InnerstrProblemCodeInner == codeInner)
                                                                    {
                                                                        bool bConcernExists = false;
                                                                        foreach (DataRow healthConcern in dtHealthConcern.Rows)
                                                                        {
                                                                            if (healthConcern["ConcernsICD10Code"].ToString() == drProblem["ICD10"].ToString() || healthConcern["ConcernsICD9Code"].ToString() == drProblem["ICD9"].ToString())
                                                                            {
                                                                                bConcernExists = true;
                                                                            }
                                                                        }
                                                                        if (!bConcernExists)
                                                                        {
                                                                            dr[dsCCDA.HealthConcerns.ConcernsICD9CodeColumn.ColumnName] = drProblem["ICD9"];
                                                                            dr[dsCCDA.HealthConcerns.ConcernsICD10CodeColumn.ColumnName] = drProblem["ICD10"];
                                                                            dr[dsCCDA.HealthConcerns.ConcernsICD9DescriptionColumn.ColumnName] = drProblem["ICD9_Description"];
                                                                            dr[dsCCDA.HealthConcerns.ConcernsICD10DescriptionColumn.ColumnName] = drProblem["ICD10_Description"];
                                                                            dr[dsCCDA.HealthConcerns.ConcernsSNOMEDIDColumn.ColumnName] = drProblem["SNOMEDID"];
                                                                            dr[dsCCDA.HealthConcerns.ConcernsSNOMEDDescColumn.ColumnName] = drProblem["SNOMED_DESCRIPTION"];
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (document.effectiveTime != null && document.effectiveTime is TS && document.effectiveTime.value != null &&
                                            document.effectiveTime.value.ToLower() != "unk" && document.effectiveTime.value.ToLower() != "ni")
                                            {
                                                dr[dsCCDA.HealthConcerns.ConcernsDateColumn.ColumnName] = document.effectiveTime.value.ToFormatedDateTimeByFormat();
                                            }
                                            dr[dsCCDA.HealthConcerns.IsActiveColumn.ColumnName] = true;
                                            if (concernAct.statusCode != null && !string.IsNullOrWhiteSpace(concernAct.statusCode.code))
                                                dr[dsCCDA.HealthConcerns.ConcernsStatusColumn.ColumnName] = concernAct.statusCode.code;
                                            dr[dsCCDA.HealthConcerns.UserIdColumn.ColumnName] = UserId;
                                            dr[dsCCDA.HealthConcerns.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                                            if (!string.IsNullOrWhiteSpace(FacilityId))
                                                dr[dsCCDA.HealthConcerns.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                                            dr[dsCCDA.HealthConcerns.CreatedByColumn.ColumnName] = CreatedBy;
                                            dr[dsCCDA.HealthConcerns.CreatedOnColumn.ColumnName] = CreatedOn;
                                            dr[dsCCDA.HealthConcerns.ModifiedByColumn.ColumnName] = ModifiedBy;
                                            dr[dsCCDA.HealthConcerns.ModifiedOnColumn.ColumnName] = CreatedOn;
                                            dtHealthConcern.Rows.Add(dr);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Health Concerns: " + ex.Message);
            }
        }

        #endregion Health Concerns

        #region Plan Of Treatment

        private static void GetPOTMedicationData(ClinicalDocument document, ref DataTable dtMedication, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 laboratoryTestComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref laboratoryTestComp, DocumentSections.PlanOfTreatment);
                List<Entry> entryList = null;
                if (isComponentExist && laboratoryTestComp.GetEntry(ref entryList))
                {
                    int i = -500;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is SubstanceAdministration)
                        {
                            string PatientNotes = string.Empty;
                            SubstanceAdministration medSubstanceAdministration = e.Item as SubstanceAdministration;
                            if (!medSubstanceAdministration.templateId.IsNullOrEmpty() && medSubstanceAdministration.templateId[0] != null
                                && medSubstanceAdministration.templateId[0].root == PlanOfTreatmentSection.PlannedMedicationActivity)
                            {
                                DataRow dr = dtMedication.NewRow();
                                dr[dsCCDA.Medication.MedicationIdColumn.ColumnName] = i;
                                dr[dsCCDA.Medication.PatientIdColumn.ColumnName] = patientId;
                                if (medSubstanceAdministration.statusCode != null && !String.IsNullOrWhiteSpace(medSubstanceAdministration.statusCode.code))
                                {
                                    dr[dsCCDA.Medication.StatusColumn.ColumnName] = medSubstanceAdministration.statusCode.code;
                                }

                                if (!medSubstanceAdministration.effectiveTime.IsNullOrEmpty() && medSubstanceAdministration.effectiveTime[0] != null
                                && medSubstanceAdministration.effectiveTime[0] is IVL_TS)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime((IVL_TS)medSubstanceAdministration.effectiveTime[0], out lowTime, out highTime);
                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime != "UNK" && highTime != "NI")
                                    {
                                        dr[dsCCDA.Medication.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrWhiteSpace(highTime) && highTime != "UNK" && highTime != "NI")
                                    {
                                        dr[dsCCDA.Medication.StopDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else if (medSubstanceAdministration.effectiveTime != null && !string.IsNullOrWhiteSpace(medSubstanceAdministration.effectiveTime[0].value)
                                    && medSubstanceAdministration.effectiveTime[0].value != "UNK" && medSubstanceAdministration.effectiveTime[0].value != "NI")
                                {
                                    dr[dsCCDA.Medication.StartDateColumn.ColumnName] = medSubstanceAdministration.effectiveTime[0].value.ToFormatedDateTimeByFormat();
                                }
                                else
                                {
                                    errors.Add("Plan Of Treatment - Planned Medication Activity --> Start Date not available in the file");
                                }
                                if (medSubstanceAdministration.text != null && medSubstanceAdministration.text is ED)
                                {
                                    ED textED = medSubstanceAdministration.text as ED;
                                    if (!textED.Text.IsNullOrEmpty())
                                    {
                                        int counterMedAct = 0;
                                        foreach (string text in textED.Text)
                                        {
                                            if (counterMedAct == 0)
                                            {
                                                PatientNotes = text;
                                            }
                                            else
                                            {
                                                PatientNotes += " " + text;
                                            }
                                            counterMedAct++;
                                        }
                                    }
                                }
                                if (string.IsNullOrWhiteSpace(PatientNotes))
                                {
                                    PIVL_TS effectiveTime = null;
                                    EIVL_TS effectiveTimeEvent = null;
                                    if (!medSubstanceAdministration.effectiveTime.IsNullOrEmpty())
                                    {
                                        foreach (ANY et in medSubstanceAdministration.effectiveTime)
                                        {
                                            if (et is PIVL_TS)
                                                effectiveTime = et as PIVL_TS;
                                            else if (et is EIVL_TS)
                                                effectiveTimeEvent = et as EIVL_TS;
                                        }
                                    }
                                    if (effectiveTime != null)
                                    {
                                        if (effectiveTime.period != null && effectiveTime.period is IVL_PQ && ((IVL_PQ)effectiveTime.period).Items != null)
                                        {
                                            string val1 = string.Empty, unit1 = string.Empty, val2 = string.Empty, unit2 = string.Empty;
                                            if (((IVL_PQ)effectiveTime.period).Items[0] != null)
                                            {
                                                val1 = ((IVL_PQ)effectiveTime.period).Items[0].value;
                                                unit1 = ((IVL_PQ)effectiveTime.period).Items[0].unit;
                                            }
                                            if (((IVL_PQ)effectiveTime.period).Items[1] != null)
                                            {
                                                val2 = ((IVL_PQ)effectiveTime.period).Items[1].value;
                                                unit2 = ((IVL_PQ)effectiveTime.period).Items[1].unit;
                                            }
                                            if (effectiveTime.institutionSpecified1)
                                            {
                                                PatientNotes = NumberToWord(MDVUtility.ToInt32(val1))
                                                     + " to " + NumberToWord(MDVUtility.ToInt32(val2)) + " " + MedicationUnits(unit1.ToLower());
                                            }
                                            else
                                            {
                                                PatientNotes = "every " + NumberToWord(MDVUtility.ToInt32(val1))
                                                     + " to " + NumberToWord(MDVUtility.ToInt32(val2)) + " " + MedicationUnits(unit1.ToLower());
                                            }
                                        }
                                        else
                                        {
                                            string val = effectiveTime.period.value;
                                            string unit = effectiveTime.period.unit;
                                            PatientNotes = CalculateDoseTimming(val, unit, effectiveTime.institutionSpecified1);
                                        }
                                    }
                                    if (effectiveTimeEvent != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(effectiveTimeEvent.@event.code))
                                        {
                                            PatientNotes += " " + CalculateEvetRelateTime(effectiveTimeEvent.@event.code);
                                        }
                                    }
                                }

                                if (medSubstanceAdministration.repeatNumber != null)
                                {
                                    dr[dsCCDA.Medication.RepeatNumberColumn.ColumnName] = medSubstanceAdministration.repeatNumber.value;
                                }
                                if (medSubstanceAdministration.repeatNumber != null)
                                {
                                    dr[dsCCDA.Medication.RepeatNumberColumn.ColumnName] = medSubstanceAdministration.repeatNumber.value;
                                }
                                if (medSubstanceAdministration.doseQuantity != null)
                                {
                                    dr[dsCCDA.Medication.DoseValueColumn.ColumnName] = medSubstanceAdministration.doseQuantity.value;
                                    if (!string.IsNullOrWhiteSpace(medSubstanceAdministration.doseQuantity.unit) && medSubstanceAdministration.doseQuantity.unit != "1")
                                        dr[dsCCDA.Medication.DoseUnitColumn.ColumnName] = medSubstanceAdministration.doseQuantity.unit;
                                }
                                if (medSubstanceAdministration.routeCode != null && !string.IsNullOrWhiteSpace(medSubstanceAdministration.routeCode.displayName))
                                {
                                    dr[dsCCDA.Medication.RouteCodeColumn.ColumnName] = medSubstanceAdministration.routeCode.code;
                                    dr[dsCCDA.Medication.RouteByColumn.ColumnName] = medSubstanceAdministration.routeCode.displayName;
                                }
                                if (medSubstanceAdministration.consumable != null && medSubstanceAdministration.consumable is Consumable)
                                {
                                    Consumable consumable = (Consumable)medSubstanceAdministration.consumable;
                                    if (consumable.manufacturedProduct != null && consumable.manufacturedProduct.Item != null
                                        && consumable.manufacturedProduct.Item is Material)
                                    {
                                        Material manufacturedMaterial = (Material)consumable.manufacturedProduct.Item;
                                        if (manufacturedMaterial.code != null)
                                        {
                                            dr[dsCCDA.Medication.RxNormCodeColumn.ColumnName] = manufacturedMaterial.code.code;
                                            dr[dsCCDA.Medication.DrugDescriptionColumn.ColumnName] = manufacturedMaterial.code.displayName;
                                        }
                                    }
                                    else
                                    {
                                        errors.Add("Medication --> Rx Norm Code not found in the file");
                                    }
                                }
                                if (!medSubstanceAdministration.entryRelationship.IsNullOrEmpty() && medSubstanceAdministration.entryRelationship.Count() > 0)
                                {
                                    foreach (EntryRelationship er in medSubstanceAdministration.entryRelationship)
                                    {
                                        if (er.Item != null && er.Item is Supply)
                                        {
                                            Supply supplyOrder = (Supply)er.Item;
                                            if (!supplyOrder.entryRelationship.IsNullOrEmpty())
                                            {
                                                foreach (EntryRelationship erInner in supplyOrder.entryRelationship)
                                                {
                                                    if (erInner.Item != null && erInner.Item is Act)
                                                    {
                                                        Act act = erInner.Item as Act;
                                                        if (act.code != null && act.code.code == "409073007")
                                                        {
                                                            dr[dsCCDA.Medication.SubstitutionColumn.ColumnName] = "y";
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }

                                if (medSubstanceAdministration.negationInd == true)
                                {
                                    dr[dsCCDA.Medication.NegationReasonColumn.ColumnName] = "";
                                    dr[dsCCDA.Medication.NegationIndexColumn.ColumnName] = true;
                                }
                                dr[dsCCDA.Medication.PatientNotesColumn.ColumnName] = PatientNotes;
                                if (dr[dsCCDA.Medication.SubstitutionColumn.ColumnName] == DBNull.Value)
                                {
                                    dr[dsCCDA.Medication.SubstitutionColumn.ColumnName] = "n";
                                }
                                if (dr[dsCCDA.Medication.DrugDescriptionColumn.ColumnName] != DBNull.Value)
                                {
                                    dr[dsCCDA.Medication.UserIdColumn.ColumnName] = UserId;
                                    dr[dsCCDA.Medication.IntendedUseColumn.ColumnName] = "Yes";
                                    dr[dsCCDA.Medication.IsActiveColumn.ColumnName] = true;
                                    dr[dsCCDA.Medication.CreatedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.Medication.CreatedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.Medication.ModifiedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.Medication.ModifiedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.Medication.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                    dtMedication.Rows.Add(dr);
                                    i--;
                                }
                                else
                                {
                                    errors.Add("Plan Of Treatment - Planned Medication Activity: Drug Description not found");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Plan Of Treatment - Planned Medication Activity: " + ex.Message);
            }
        }

        private static void GetPOTEncounterData(ClinicalDocument document, ref DataTable dtEncounter, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 encounterComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref encounterComp, DocumentSections.PlanOfTreatment);
                List<Entry> entryList = null;
                if (isComponentExist && encounterComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Encounter)
                        {
                            Encounter encounter = e.Item as Encounter;
                            if (!encounter.templateId.IsNullOrEmpty() && encounter.templateId[0] != null
                                && encounter.templateId[0].root == PlanOfTreatmentSection.PlannedEncounter)
                            {
                                DataRow dr = dtEncounter.NewRow();
                                dr[dsCCDA.NotesEncounter.PatientIdColumn.ColumnName] = patientId;
                                dr[dsCCDA.NotesEncounter.EncounterIdColumn.ColumnName] = i;
                                if (encounter.statusCode != null && !String.IsNullOrWhiteSpace(encounter.statusCode.code))
                                {
                                    dr[dsCCDA.NotesEncounter.StatusColumn.ColumnName] = encounter.statusCode.code;
                                }
                                if (encounter.effectiveTime != null && encounter.effectiveTime.Items != null &&
                                    encounter.effectiveTime.Items.Count() > 0 && encounter.effectiveTime is IVL_TS)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(encounter.effectiveTime, out lowTime, out highTime);
                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.NotesEncounter.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                        dr[dsCCDA.NotesEncounter.VisitDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.NotesEncounter.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else if (encounter.effectiveTime != null && !string.IsNullOrWhiteSpace(encounter.effectiveTime.value) &&
                                        encounter.effectiveTime.value.ToLower() != "unk" && encounter.effectiveTime.value.ToLower() != "ni")
                                {
                                    dr[dsCCDA.NotesEncounter.StartDateColumn.ColumnName] = encounter.effectiveTime.value.ToFormatedDateTimeByFormat();
                                    dr[dsCCDA.NotesEncounter.VisitDateColumn.ColumnName] = encounter.effectiveTime.value.ToFormatedDateTimeByFormat();
                                }
                                else
                                {
                                    errors.Add("Planned Encounter: DOS not found");
                                }

                                if (encounter.code != null && !string.IsNullOrWhiteSpace(encounter.code.code))
                                {
                                    dr[dsCCDA.NotesEncounter.CodeColumn.ColumnName] = encounter.code.code;
                                    if (encounter.code.codeSystem == CodeTypes.SNOMED)
                                    {
                                        dr[dsCCDA.NotesEncounter.CodeTypeColumn.ColumnName] = "SNOMED";
                                    }
                                    else if (encounter.code.codeSystem == CodeTypes.HCPCS)
                                    {
                                        dr[dsCCDA.NotesEncounter.CodeTypeColumn.ColumnName] = "HCPCS";
                                    }
                                    else if (encounter.code.codeSystem == CodeTypes.CPT)
                                    {
                                        dr[dsCCDA.NotesEncounter.CodeTypeColumn.ColumnName] = "CPT";
                                    }
                                }

                                if (!encounter.entryRelationship.IsNullOrEmpty() && encounter.entryRelationship.Count() > 0)
                                {
                                    string strReason = string.Empty;
                                    foreach (EntryRelationship er in encounter.entryRelationship)
                                    {
                                        if (er.Item != null && er.Item is Observation)
                                        {
                                            Observation observation = (Observation)er.Item;
                                            if (!observation.value.IsNullOrEmpty() && observation.value[0] != null)
                                            {
                                                CD cd = observation.value[0] as CD;
                                                if (!string.IsNullOrWhiteSpace(cd.code))
                                                {
                                                    EncounterSNOMEDIDs.Add(cd.code);
                                                    strReason = cd.displayName + " ";
                                                    dr[dsCCDA.NotesEncounter.SNOMED_DESCRIPTIONColumn.ColumnName] = cd.displayName;
                                                    dr[dsCCDA.NotesEncounter.SNOMEDIDColumn.ColumnName] = cd.code;
                                                    dr[dsCCDA.NotesEncounter.IsProblemColumn.ColumnName] = true;
                                                }
                                                else
                                                {
                                                    dr[dsCCDA.NotesEncounter.IsProblemColumn.ColumnName] = false;
                                                }
                                            }
                                            else
                                            {
                                                dr[dsCCDA.NotesEncounter.IsProblemColumn.ColumnName] = false;
                                            }
                                            if (observation.text != null)
                                            {
                                                ED comment = observation.text;
                                                FollowUp = comment.Text[0];
                                            }
                                        }
                                    }
                                    dr[dsCCDA.NotesEncounter.EncounterColumn.ColumnName] = strReason;
                                }
                                else
                                {
                                    dr[dsCCDA.NotesEncounter.IsProblemColumn.ColumnName] = false;
                                }
                                dr[dsCCDA.NotesEncounter.UserIdColumn.ColumnName] = UserId;
                                dr[dsCCDA.NotesEncounter.IsActiveColumn.ColumnName] = true;
                                dr[dsCCDA.NotesEncounter.CreatedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.NotesEncounter.CreatedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.NotesEncounter.ModifiedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.NotesEncounter.ModifiedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.NotesEncounter.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                dr[dsCCDA.NotesEncounter.FacilityIdColumn.ColumnName] = Convert.ToInt64(FacilityId);
                                dtEncounter.Rows.Add(dr);
                                i--;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Planned Encounter: " + ex.Message);
            }
        }

        public static class PlanOfTreatmentSection
        {
            public static string PlannedMedicationActivity = "2.16.840.1.113883.10.20.22.4.42";
            public static string PlannedEncounter = "2.16.840.1.113883.10.20.22.4.40";
            public static string Encounter = "2.16.840.1.113883.10.20.22.4.49";
        }

        public static class CodeTypes
        {
            public static string SNOMED = "2.16.840.1.113883.6.96";
            public static string HCPCS = "2.16.840.1.113883.6.285";
            public static string CPT = "2.16.840.1.113883.6.12";
        }

        #endregion Plan Of Treatment

        #region Assessment

        private static void GetAssessmentData(ClinicalDocument document, ref DataTable dtAssessment, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 assessmentComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref assessmentComp, DocumentSections.Assessment);
                List<Entry> entryList = null;
                if (isComponentExist)
                {
                    if (assessmentComp.GetEntry(ref entryList))
                    {
                        if (!entryList.IsNullOrEmpty() && entryList.Count() > 0) { }
                    }
                    else
                    {
                        if (assessmentComp.section.text != null && assessmentComp.section.text is StrucDocText)
                        {
                            StrucDocText strucDocText = assessmentComp.section.text as StrucDocText;
                            if (!strucDocText.Text.IsNullOrEmpty() && strucDocText.Text.Count() > 0)
                            {
                                int i = -1;
                                foreach (string strAssessment in strucDocText.Text)
                                {
                                    DataRow dr = dtAssessment.NewRow();
                                    dr[dsCCDA.Assessment.PatientIdColumn.ColumnName] = patientId;
                                    dr[dsCCDA.Assessment.AssessmentIdColumn.ColumnName] = i;
                                    dr[dsCCDA.Assessment.AssessmentDescriptionColumn.ColumnName] = strAssessment;
                                    dr[dsCCDA.Assessment.UserIdColumn.ColumnName] = UserId;
                                    dr[dsCCDA.Assessment.IsActiveColumn.ColumnName] = true;
                                    dr[dsCCDA.Assessment.CreatedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.Assessment.CreatedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.Assessment.ModifiedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.Assessment.ModifiedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.Assessment.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                    dr[dsCCDA.Assessment.FacilityIdColumn.ColumnName] = Convert.ToInt64(FacilityId);
                                    dtAssessment.Rows.Add(dr);
                                    i--;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errors.Add("Health Concerns: " + ex.Message);
            }
        }


        #endregion Assessment

        #region Functional Status

        private static void GetFunctionalStatusData(ClinicalDocument document, ref DataTable dtFunctionalStatus, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 functionalStatusComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref functionalStatusComp, DocumentSections.FunctionalStatus);
                List<Entry> entryList = null;
                if (isComponentExist && functionalStatusComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation functionalStatus = e.Item as Observation;
                            if (functionalStatus.code != null && !string.IsNullOrWhiteSpace(functionalStatus.code.code))
                            {
                                DataRow dr = dtFunctionalStatus.NewRow();
                                dr[dsCCDA.FunctionalStatus.PatientIdColumn.ColumnName] = patientId;
                                dr[dsCCDA.FunctionalStatus.FunctionalStatusIdColumn.ColumnName] = i--;
                                if (!functionalStatus.templateId.IsNullOrEmpty() && functionalStatus.templateId.Count() > 0)
                                {
                                    II templateID = functionalStatus.templateId[0] as II;
                                    if (!string.IsNullOrWhiteSpace(templateID.root) && (templateID.root == "2.16.840.1.113883.10.20.22.4.68" || templateID.root == "2.16.840.1.113883.10.20.22.4.67"))
                                    {
                                        dr[dsCCDA.FunctionalStatus.TypeColumn.ColumnName] = "Functional Status";
                                    }
                                    else if (!string.IsNullOrWhiteSpace(templateID.root) && (templateID.root == "2.16.840.1.113883.10.20.22.4.73" || templateID.root == "2.16.840.1.113883.10.20.22.4.74"))
                                    {
                                        dr[dsCCDA.FunctionalStatus.TypeColumn.ColumnName] = "Cognitive Status";
                                    }
                                }
                                if (functionalStatus.effectiveTime != null && functionalStatus.effectiveTime.Items != null &&
                                        functionalStatus.effectiveTime.Items.Count() > 0 && functionalStatus.effectiveTime is IVL_TS)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(functionalStatus.effectiveTime, out lowTime, out highTime);
                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.FunctionalStatus.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.FunctionalStatus.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else if (functionalStatus.effectiveTime != null && !string.IsNullOrWhiteSpace(functionalStatus.effectiveTime.value) &&
                                        functionalStatus.effectiveTime.value.ToLower() != "unk" && functionalStatus.effectiveTime.value.ToLower() != "ni")
                                {
                                    dr[dsCCDA.FunctionalStatus.StartDateColumn.ColumnName] = functionalStatus.effectiveTime.value.ToFormatedDateTimeByFormat();
                                }
                                else
                                {
                                    errors.Add("Functional Status: Date not found");
                                }

                                if (functionalStatus.code != null && !string.IsNullOrWhiteSpace(functionalStatus.code.code))
                                {
                                    dr[dsCCDA.FunctionalStatus.CodeColumn.ColumnName] = functionalStatus.code.code;
                                    dr[dsCCDA.FunctionalStatus.CodeDescriptionColumn.ColumnName] = functionalStatus.code.displayName;
                                    if (functionalStatus.code.codeSystem == CodeTypes.SNOMED)
                                    {
                                        dr[dsCCDA.FunctionalStatus.CodeTypeColumn.ColumnName] = "SNOMED";
                                    }
                                    else if (functionalStatus.code.codeSystem == CodeTypes.HCPCS)
                                    {
                                        dr[dsCCDA.FunctionalStatus.CodeTypeColumn.ColumnName] = "HCPCS";
                                    }
                                    else if (functionalStatus.code.codeSystem == CodeTypes.CPT)
                                    {
                                        dr[dsCCDA.FunctionalStatus.CodeTypeColumn.ColumnName] = "CPT";
                                    }
                                }
                                if (!functionalStatus.value.IsNullOrEmpty())
                                {
                                    foreach (CD value in functionalStatus.value)
                                    {
                                        if (!string.IsNullOrWhiteSpace(value.code))
                                        {
                                            dr[dsCCDA.FunctionalStatus.SNOMEDIDColumn.ColumnName] = value.code;
                                            dr[dsCCDA.FunctionalStatus.SNOMEDDescriptionColumn.ColumnName] = value.displayName;
                                            List<MDVision.Model.CCDA.ICDLookupModel> ICDLookupsList = BLLIMO.GetIMOICDbySNOMEDCode(value.code);
                                            if (ICDLookupsList != null && ICDLookupsList.Count > 0)
                                            {
                                                MDVision.Model.CCDA.ICDLookupModel objICD = ICDLookupsList.FirstOrDefault(m => m.ICD10CM_CODE != null || m.ICD10CM_TITLE != null || m.ICD9CM_CODE != null || m.ICD9CM_TITLE != null);
                                                if (objICD != null)
                                                {
                                                    dr[dsCCDA.FunctionalStatus.ICD9CodeColumn.ColumnName] = objICD.ICD9CM_CODE;
                                                    dr[dsCCDA.FunctionalStatus.ICD10CodeColumn.ColumnName] = objICD.ICD10CM_CODE;
                                                    dr[dsCCDA.FunctionalStatus.ICD9_DescriptionColumn.ColumnName] = value.displayName;
                                                    dr[dsCCDA.FunctionalStatus.ICD10_DescriptionColumn.ColumnName] = value.displayName;
                                                }
                                            }
                                        }
                                    }
                                }
                                dr[dsCCDA.FunctionalStatus.UserIdColumn.ColumnName] = UserId;
                                dr[dsCCDA.FunctionalStatus.IsActiveColumn.ColumnName] = true;
                                dr[dsCCDA.FunctionalStatus.CreatedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.FunctionalStatus.CreatedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.FunctionalStatus.ModifiedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.FunctionalStatus.ModifiedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.FunctionalStatus.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                dtFunctionalStatus.Rows.Add(dr);
                            }
                        }
                        else if (e != null && e.Item != null && e.Item is Organizer)
                        {
                            Organizer functionalStatusOrganizer = e.Item as Organizer;
                            if (!functionalStatusOrganizer.component.IsNullOrEmpty())
                            {
                                foreach (Component4 componentInner in functionalStatusOrganizer.component)
                                {
                                    if (componentInner != null && componentInner.Item != null && componentInner.Item is Observation)
                                    {
                                        Observation functionalStatus = componentInner.Item as Observation;
                                        if (functionalStatus.code != null && !string.IsNullOrWhiteSpace(functionalStatus.code.code))
                                        {
                                            DataRow dr = dtFunctionalStatus.NewRow();
                                            dr[dsCCDA.FunctionalStatus.PatientIdColumn.ColumnName] = patientId;
                                            dr[dsCCDA.FunctionalStatus.FunctionalStatusIdColumn.ColumnName] = i--;
                                            if (!functionalStatus.templateId.IsNullOrEmpty() && functionalStatus.templateId.Count() > 0)
                                            {
                                                II templateID = functionalStatus.templateId[0] as II;
                                                if (!string.IsNullOrWhiteSpace(templateID.root) && (templateID.root == "2.16.840.1.113883.10.20.22.4.68" || templateID.root == "2.16.840.1.113883.10.20.22.4.67"))
                                                {
                                                    dr[dsCCDA.FunctionalStatus.TypeColumn.ColumnName] = "Functional Status";
                                                }
                                                else if (!string.IsNullOrWhiteSpace(templateID.root) && (templateID.root == "2.16.840.1.113883.10.20.22.4.73" || templateID.root == "2.16.840.1.113883.10.20.22.4.74"))
                                                {
                                                    dr[dsCCDA.FunctionalStatus.TypeColumn.ColumnName] = "Cognitive Status";
                                                }
                                            }
                                            if (functionalStatus.effectiveTime != null && functionalStatus.effectiveTime.Items != null &&
                                                    functionalStatus.effectiveTime.Items.Count() > 0 && functionalStatus.effectiveTime is IVL_TS)
                                            {
                                                string lowTime, highTime;
                                                GetLowHighTime(functionalStatus.effectiveTime, out lowTime, out highTime);
                                                if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                                {
                                                    dr[dsCCDA.FunctionalStatus.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                                }
                                                if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                                {
                                                    dr[dsCCDA.FunctionalStatus.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                                }
                                            }
                                            else if (functionalStatus.effectiveTime != null && !string.IsNullOrWhiteSpace(functionalStatus.effectiveTime.value) &&
                                                    functionalStatus.effectiveTime.value.ToLower() != "unk" && functionalStatus.effectiveTime.value.ToLower() != "ni")
                                            {
                                                dr[dsCCDA.FunctionalStatus.StartDateColumn.ColumnName] = functionalStatus.effectiveTime.value.ToFormatedDateTimeByFormat();
                                            }
                                            else
                                            {
                                                errors.Add("Functional Status: Date not found");
                                            }

                                            if (functionalStatus.code != null && !string.IsNullOrWhiteSpace(functionalStatus.code.code))
                                            {
                                                dr[dsCCDA.FunctionalStatus.CodeColumn.ColumnName] = functionalStatus.code.code;
                                                dr[dsCCDA.FunctionalStatus.CodeDescriptionColumn.ColumnName] = functionalStatus.code.displayName;
                                                if (functionalStatus.code.codeSystem == CodeTypes.SNOMED)
                                                {
                                                    dr[dsCCDA.FunctionalStatus.CodeTypeColumn.ColumnName] = "SNOMED";
                                                }
                                                else if (functionalStatus.code.codeSystem == CodeTypes.HCPCS)
                                                {
                                                    dr[dsCCDA.FunctionalStatus.CodeTypeColumn.ColumnName] = "HCPCS";
                                                }
                                                else if (functionalStatus.code.codeSystem == CodeTypes.CPT)
                                                {
                                                    dr[dsCCDA.FunctionalStatus.CodeTypeColumn.ColumnName] = "CPT";
                                                }
                                            }
                                            if (!functionalStatus.value.IsNullOrEmpty())
                                            {
                                                foreach (CD value in functionalStatus.value)
                                                {
                                                    if (!string.IsNullOrWhiteSpace(value.code))
                                                    {
                                                        dr[dsCCDA.FunctionalStatus.SNOMEDIDColumn.ColumnName] = value.code;
                                                        dr[dsCCDA.FunctionalStatus.SNOMEDDescriptionColumn.ColumnName] = value.displayName;
                                                        List<MDVision.Model.CCDA.ICDLookupModel> ICDLookupsList = BLLIMO.GetIMOICDbySNOMEDCode(value.code);
                                                        if (ICDLookupsList != null && ICDLookupsList.Count > 0)
                                                        {
                                                            MDVision.Model.CCDA.ICDLookupModel objICD = ICDLookupsList.FirstOrDefault(m => m.ICD10CM_CODE != null || m.ICD10CM_TITLE != null || m.ICD9CM_CODE != null || m.ICD9CM_TITLE != null);
                                                            if (objICD != null)
                                                            {
                                                                dr[dsCCDA.FunctionalStatus.ICD9CodeColumn.ColumnName] = objICD.ICD9CM_CODE;
                                                                dr[dsCCDA.FunctionalStatus.ICD10CodeColumn.ColumnName] = objICD.ICD10CM_CODE;
                                                                dr[dsCCDA.FunctionalStatus.ICD9_DescriptionColumn.ColumnName] = value.displayName;
                                                                dr[dsCCDA.FunctionalStatus.ICD10_DescriptionColumn.ColumnName] = value.displayName;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            dr[dsCCDA.FunctionalStatus.UserIdColumn.ColumnName] = UserId;
                                            dr[dsCCDA.FunctionalStatus.IsActiveColumn.ColumnName] = true;
                                            dr[dsCCDA.FunctionalStatus.CreatedByColumn.ColumnName] = CreatedBy;
                                            dr[dsCCDA.FunctionalStatus.CreatedOnColumn.ColumnName] = CreatedOn;
                                            dr[dsCCDA.FunctionalStatus.ModifiedByColumn.ColumnName] = CreatedBy;
                                            dr[dsCCDA.FunctionalStatus.ModifiedOnColumn.ColumnName] = CreatedOn;
                                            dr[dsCCDA.FunctionalStatus.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                            dtFunctionalStatus.Rows.Add(dr);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Functional Status: " + ex.Message);
            }
        }

        #endregion Functional Status

        #region Mental Status

        private static void GetMentalStatusData(ClinicalDocument document, ref DataTable dtFunctionalStatus, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 functionalStatusComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref functionalStatusComp, DocumentSections.MentalStatus);
                List<Entry> entryList = null;
                if (isComponentExist && functionalStatusComp.GetEntry(ref entryList))
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Observation)
                        {
                            Observation functionalStatus = e.Item as Observation;
                            if (functionalStatus.code != null && !string.IsNullOrWhiteSpace(functionalStatus.code.code))
                            {
                                DataRow dr = dtFunctionalStatus.NewRow();
                                dr[dsCCDA.FunctionalStatus.PatientIdColumn.ColumnName] = patientId;
                                dr[dsCCDA.FunctionalStatus.FunctionalStatusIdColumn.ColumnName] = i--;
                                if (!functionalStatus.templateId.IsNullOrEmpty() && functionalStatus.templateId.Count() > 0)
                                {
                                    II templateID = functionalStatus.templateId[0] as II;
                                    if (!string.IsNullOrWhiteSpace(templateID.root) && (templateID.root == "2.16.840.1.113883.10.20.22.4.73" || templateID.root == "2.16.840.1.113883.10.20.22.4.74"))
                                    {
                                        dr[dsCCDA.FunctionalStatus.TypeColumn.ColumnName] = "Mental Status";
                                    }
                                }
                                if (functionalStatus.effectiveTime != null && functionalStatus.effectiveTime.Items != null &&
                                        functionalStatus.effectiveTime.Items.Count() > 0 && functionalStatus.effectiveTime is IVL_TS)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(functionalStatus.effectiveTime, out lowTime, out highTime);
                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.FunctionalStatus.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.FunctionalStatus.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else if (functionalStatus.effectiveTime != null && !string.IsNullOrWhiteSpace(functionalStatus.effectiveTime.value) &&
                                        functionalStatus.effectiveTime.value.ToLower() != "unk" && functionalStatus.effectiveTime.value.ToLower() != "ni")
                                {
                                    dr[dsCCDA.FunctionalStatus.StartDateColumn.ColumnName] = functionalStatus.effectiveTime.value.ToFormatedDateTimeByFormat();
                                }
                                else
                                {
                                    errors.Add("Functional Status: Date not found");
                                }

                                if (functionalStatus.code != null && !string.IsNullOrWhiteSpace(functionalStatus.code.code))
                                {
                                    dr[dsCCDA.FunctionalStatus.CodeColumn.ColumnName] = functionalStatus.code.code;
                                    dr[dsCCDA.FunctionalStatus.CodeDescriptionColumn.ColumnName] = functionalStatus.code.displayName;
                                    if (functionalStatus.code.codeSystem == CodeTypes.SNOMED)
                                    {
                                        dr[dsCCDA.FunctionalStatus.CodeTypeColumn.ColumnName] = "SNOMED";
                                    }
                                    else if (functionalStatus.code.codeSystem == CodeTypes.HCPCS)
                                    {
                                        dr[dsCCDA.FunctionalStatus.CodeTypeColumn.ColumnName] = "HCPCS";
                                    }
                                    else if (functionalStatus.code.codeSystem == CodeTypes.CPT)
                                    {
                                        dr[dsCCDA.FunctionalStatus.CodeTypeColumn.ColumnName] = "CPT";
                                    }
                                }
                                if (!functionalStatus.value.IsNullOrEmpty())
                                {
                                    foreach (CD value in functionalStatus.value)
                                    {
                                        if (!string.IsNullOrWhiteSpace(value.code))
                                        {
                                            dr[dsCCDA.FunctionalStatus.SNOMEDIDColumn.ColumnName] = value.code;
                                            dr[dsCCDA.FunctionalStatus.SNOMEDDescriptionColumn.ColumnName] = value.displayName;
                                            List<MDVision.Model.CCDA.ICDLookupModel> ICDLookupsList = BLLIMO.GetIMOICDbySNOMEDCode(value.code);
                                            if (ICDLookupsList != null && ICDLookupsList.Count > 0)
                                            {
                                                MDVision.Model.CCDA.ICDLookupModel objICD = ICDLookupsList.FirstOrDefault(m => m.ICD10CM_CODE != null || m.ICD10CM_TITLE != null || m.ICD9CM_CODE != null || m.ICD9CM_TITLE != null);
                                                if (objICD != null)
                                                {
                                                    dr[dsCCDA.FunctionalStatus.ICD9CodeColumn.ColumnName] = objICD.ICD9CM_CODE;
                                                    dr[dsCCDA.FunctionalStatus.ICD10CodeColumn.ColumnName] = objICD.ICD10CM_CODE;
                                                    dr[dsCCDA.FunctionalStatus.ICD9_DescriptionColumn.ColumnName] = value.displayName;
                                                    dr[dsCCDA.FunctionalStatus.ICD10_DescriptionColumn.ColumnName] = value.displayName;
                                                }
                                            }
                                        }
                                    }
                                }
                                dr[dsCCDA.FunctionalStatus.UserIdColumn.ColumnName] = UserId;
                                dr[dsCCDA.FunctionalStatus.IsActiveColumn.ColumnName] = true;
                                dr[dsCCDA.FunctionalStatus.CreatedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.FunctionalStatus.CreatedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.FunctionalStatus.ModifiedByColumn.ColumnName] = CreatedBy;
                                dr[dsCCDA.FunctionalStatus.ModifiedOnColumn.ColumnName] = CreatedOn;
                                dr[dsCCDA.FunctionalStatus.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                dtFunctionalStatus.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Mental Status: " + ex.Message);
            }
        }

        #endregion Mental Status

        #region Reason For Referral

        private static void GetReasonForReferraltData(ClinicalDocument document, ref DataTable dtReasonForReferral, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 reasonForReferralComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref reasonForReferralComp, DocumentSections.ReasonForReferral);
                List<Entry> entryList = null;
                if (isComponentExist)
                {
                    if (reasonForReferralComp.GetEntry(ref entryList))
                    {
                        if (!entryList.IsNullOrEmpty() && entryList.Count() > 0) { }
                    }
                    else
                    {
                        if (reasonForReferralComp.section.text != null && reasonForReferralComp.section.text is StrucDocText)
                        {
                            StrucDocText strucDocText = reasonForReferralComp.section.text as StrucDocText;
                            if ((!strucDocText.Text.IsNullOrEmpty() && strucDocText.Text.Count() > 0))
                            {
                                int i = -1;
                                foreach (string strReferral in strucDocText.Text)
                                {
                                    DataRow dr = dtReasonForReferral.NewRow();
                                    dr[dsCCDA.ReasonForReferral.PatientIdColumn.ColumnName] = patientId;
                                    dr[dsCCDA.ReasonForReferral.ReferralIdColumn.ColumnName] = i;
                                    dr[dsCCDA.ReasonForReferral.ReferralNoteColumn.ColumnName] = strReferral;
                                    dr[dsCCDA.ReasonForReferral.UserIdColumn.ColumnName] = UserId;
                                    dr[dsCCDA.ReasonForReferral.IsActiveColumn.ColumnName] = true;
                                    dr[dsCCDA.ReasonForReferral.CreatedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.ReasonForReferral.CreatedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.ReasonForReferral.ModifiedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.ReasonForReferral.ModifiedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.ReasonForReferral.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                    dr[dsCCDA.ReasonForReferral.FacilityIdColumn.ColumnName] = Convert.ToInt64(FacilityId);
                                    dtReasonForReferral.Rows.Add(dr);
                                    i--;
                                }
                            }
                            else if (strucDocText != null && !strucDocText.Items.IsNullOrEmpty() && strucDocText.Items.Count() > 0)
                            {
                                int i = -1;
                                foreach (StrucDocParagraph strReferral in strucDocText.Items)
                                {
                                    DataRow dr = dtReasonForReferral.NewRow();
                                    dr[dsCCDA.ReasonForReferral.PatientIdColumn.ColumnName] = patientId;
                                    dr[dsCCDA.ReasonForReferral.ReferralIdColumn.ColumnName] = i;
                                    dr[dsCCDA.ReasonForReferral.ReferralNoteColumn.ColumnName] = strReferral.Text[0];
                                    dr[dsCCDA.ReasonForReferral.UserIdColumn.ColumnName] = UserId;
                                    dr[dsCCDA.ReasonForReferral.IsActiveColumn.ColumnName] = true;
                                    dr[dsCCDA.ReasonForReferral.CreatedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.ReasonForReferral.CreatedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.ReasonForReferral.ModifiedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.ReasonForReferral.ModifiedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.ReasonForReferral.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                    dr[dsCCDA.ReasonForReferral.FacilityIdColumn.ColumnName] = Convert.ToInt64(FacilityId);
                                    dtReasonForReferral.Rows.Add(dr);
                                    i--;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errors.Add("Reason For Referral: " + ex.Message);
            }
        }


        #endregion Reason For Referral

        #region Insurance Provider

        private static void GetInsuranceProviderData(ClinicalDocument document, ref DataTable dtInsuranceProvider, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 reasonForReferralComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref reasonForReferralComp, DocumentSections.InsuranceProvider);
                List<Entry> entryList = null;
                if (isComponentExist)
                {
                    int i = -1;
                    foreach (Entry e in entryList)
                    {
                        if (e != null && e.Item != null && e.Item is Act)
                        {
                            Act InsuranceProviderAct = e.Item as Act;
                            if (InsuranceProviderAct != null && InsuranceProviderAct.entryRelationship.IsNullOrEmpty())
                            {
                                foreach (EntryRelationship entryRelationship in InsuranceProviderAct.entryRelationship)
                                {
                                    if (entryRelationship != null && entryRelationship.Item != null && entryRelationship.Item is Act)
                                    {
                                        Act insuranceProvider = entryRelationship.Item as Act;
                                        DataRow dr = dtInsuranceProvider.NewRow();
                                        dr[dsCCDA.InsuranceProvider.PatientIdColumn.ColumnName] = patientId;
                                        dr[dsCCDA.InsuranceProvider.InsuranceProviderIdColumn.ColumnName] = i;
                                        if (insuranceProvider.statusCode != null && !string.IsNullOrWhiteSpace(insuranceProvider.statusCode.code))
                                        {
                                            dr[dsCCDA.InsuranceProvider.StatusColumn.ColumnName] = insuranceProvider.statusCode.code;
                                        }
                                        if (!insuranceProvider.id.IsNullOrEmpty() && insuranceProvider.id.Count() > 0)
                                        {
                                            II groupIdentifier = insuranceProvider.id[0] as II;
                                            if (groupIdentifier != null && !string.IsNullOrWhiteSpace(groupIdentifier.extension))
                                            {
                                                dr[dsCCDA.InsuranceProvider.InsuranceGroupIdColumn.ColumnName] = groupIdentifier.extension;
                                            }
                                        }
                                        if (insuranceProvider.priorityCode != null && !string.IsNullOrWhiteSpace(insuranceProvider.priorityCode.code))
                                        {
                                            CE priorityCode = insuranceProvider.priorityCode as CE;
                                            dr[dsCCDA.InsuranceProvider.PriorityColumn.ColumnName] = priorityCode.code;
                                        }
                                        if (!insuranceProvider.performer.IsNullOrEmpty())
                                        {
                                            foreach (Performer2 performer in insuranceProvider.performer)
                                            {
                                                if (performer.assignedEntity != null)
                                                {
                                                    AssignedEntity assignedEntity = performer.assignedEntity as AssignedEntity;
                                                    if (!assignedEntity.addr.IsNullOrEmpty() && assignedEntity.addr[0] != null && !assignedEntity.addr[0].Items.IsNullOrEmpty())
                                                    {
                                                        List<ADXP> addList = assignedEntity.addr[0].Items;
                                                        foreach (ADXP adxp in addList.Where(adxp => adxp != null && !adxp.Text.IsNullOrEmpty() && !string.IsNullOrEmpty(adxp.Text[0])))
                                                        {
                                                            if (adxp is adxpstate)
                                                            {
                                                                dr[dsCCDA.InsuranceProvider.InsuranceProviderStateColumn.ColumnName] = adxp.Text[0].ToUpper();
                                                            }
                                                            else if (adxp is adxpcity)
                                                            {
                                                                dr[dsCCDA.InsuranceProvider.InsuranceProviderCityColumn.ColumnName] = adxp.Text[0].ToUpper();
                                                            }
                                                            else if (adxp is adxppostalCode)
                                                            {
                                                                dr[dsCCDA.InsuranceProvider.InsuranceProviderPostalCodeColumn.ColumnName] = adxp.Text[0].ToUpper();
                                                            }
                                                            else if (adxp is adxpstreetAddressLine)
                                                            {
                                                                dr[dsCCDA.InsuranceProvider.InsuranceProviderStreetColumn.ColumnName] = adxp.Text[0].ToUpper();
                                                            }
                                                            else if (adxp is adxpstreetAddressLine)
                                                            {
                                                                var streetAdd = adxp.Text.Where(str => !string.IsNullOrEmpty(str)).Aggregate("", (current, str) => current + " " + str.Trim());
                                                                dr[dsCCDA.InsuranceProvider.InsuranceProviderAddress1Column.ColumnName] = streetAdd.ToUpper();
                                                            }
                                                        }
                                                    }
                                                    if (!assignedEntity.telecom.IsNullOrEmpty() && !string.IsNullOrEmpty(assignedEntity.telecom[0].value))
                                                    {
                                                        string PhoneNo1 = string.Empty;
                                                        foreach (TEL tel in assignedEntity.telecom)
                                                        {
                                                            if (tel.use != null && tel.use[0] == "HP")
                                                            {
                                                                PhoneNo1 = GetTeleCommunication(tel.value);
                                                            }
                                                            else
                                                            {
                                                                PhoneNo1 = GetTeleCommunication(tel.value);
                                                            }
                                                        }
                                                        if (!string.IsNullOrWhiteSpace(PhoneNo1))
                                                        {
                                                            dr[dsCCDA.InsuranceProvider.InsuranceProviderPhoneColumn.ColumnName] = PhoneNo1;
                                                        }
                                                    }
                                                    if (assignedEntity.assignedPerson != null)
                                                    {
                                                        Person person = assignedEntity.assignedPerson as Person;
                                                        if (!person.name.IsNullOrEmpty() && !person.name[0].Items.IsNullOrEmpty())
                                                        {
                                                            USRealmName name = GetUsRealmName(person.name[0], ref errors);

                                                            dr[dsCCDA.InsuranceProvider.SuscriberLastNameColumn.ColumnName] = name.LastName;
                                                            dr[dsCCDA.InsuranceProvider.SuscriberFirstNameColumn.ColumnName] = name.FirstName;
                                                            dr[dsCCDA.InsuranceProvider.SuscriberMiddleNameColumn.ColumnName] = name.MiddleName;
                                                        }
                                                        if (!string.IsNullOrEmpty(person.birthTime.value) && person.birthTime.value.Length >= 8)
                                                        {
                                                            if (!string.IsNullOrEmpty(person.birthTime.value))
                                                            {
                                                                var dob = person.birthTime.value.Substring(0, 8);
                                                                dr[dsCCDA.InsuranceProvider.SuscriberDOBColumn.ColumnName] = dob.ToFormatedDateTimeByFormat();
                                                            }
                                                        }
                                                    }
                                                    if (assignedEntity.representedOrganization != null)
                                                    {
                                                        Organization organization = assignedEntity.representedOrganization as Organization;
                                                        if (!organization.name.IsNullOrEmpty() && !organization.name[0].Items.IsNullOrEmpty())
                                                        {

                                                        }
                                                        else
                                                        {
                                                            dr[dsCCDA.InsuranceProvider.InsuranceProviderNameColumn.ColumnName] = organization.name[0].Text[0];
                                                        }
                                                        if (dr[dsCCDA.InsuranceProvider.InsuranceProviderPhoneColumn.ColumnName] == DBNull.Value)
                                                        {
                                                            if (!organization.telecom.IsNullOrEmpty() && !string.IsNullOrEmpty(organization.telecom[0].value))
                                                            {
                                                                string PhoneNo1 = string.Empty;
                                                                foreach (TEL tel in organization.telecom)
                                                                {
                                                                    if (tel.use != null && tel.use[0] == "HP")
                                                                    {
                                                                        PhoneNo1 = GetTeleCommunication(tel.value);
                                                                    }
                                                                    else
                                                                    {
                                                                        PhoneNo1 = GetTeleCommunication(tel.value);
                                                                    }
                                                                }
                                                                if (!string.IsNullOrWhiteSpace(PhoneNo1))
                                                                {
                                                                    dr[dsCCDA.InsuranceProvider.InsuranceProviderPhoneColumn.ColumnName] = PhoneNo1;
                                                                }
                                                            }
                                                        }
                                                        if (!organization.addr.IsNullOrEmpty() && organization.addr[0] != null && !organization.addr[0].Items.IsNullOrEmpty())
                                                        {
                                                            List<ADXP> addList = organization.addr[0].Items;
                                                            foreach (ADXP adxp in addList.Where(adxp => adxp != null && !adxp.Text.IsNullOrEmpty() && !string.IsNullOrEmpty(adxp.Text[0])))
                                                            {
                                                                if (adxp is adxpstate)
                                                                {
                                                                    if (dr[dsCCDA.InsuranceProvider.InsuranceProviderStateColumn.ColumnName] == DBNull.Value)
                                                                        dr[dsCCDA.InsuranceProvider.InsuranceProviderStateColumn.ColumnName] = adxp.Text[0].ToUpper();
                                                                }
                                                                else if (adxp is adxpcity)
                                                                {
                                                                    if (dr[dsCCDA.InsuranceProvider.InsuranceProviderCityColumn.ColumnName] == DBNull.Value)
                                                                        dr[dsCCDA.InsuranceProvider.InsuranceProviderCityColumn.ColumnName] = adxp.Text[0].ToUpper();
                                                                }
                                                                else if (adxp is adxppostalCode)
                                                                {
                                                                    if (dr[dsCCDA.InsuranceProvider.InsuranceProviderPostalCodeColumn.ColumnName] == DBNull.Value)
                                                                        dr[dsCCDA.InsuranceProvider.InsuranceProviderPostalCodeColumn.ColumnName] = adxp.Text[0].ToUpper();
                                                                }
                                                                else if (adxp is adxpstreetAddressLine)
                                                                {
                                                                    if (dr[dsCCDA.InsuranceProvider.InsuranceProviderStreetColumn.ColumnName] == DBNull.Value)
                                                                        dr[dsCCDA.InsuranceProvider.InsuranceProviderStreetColumn.ColumnName] = adxp.Text[0].ToUpper();
                                                                }
                                                                else if (adxp is adxpstreetAddressLine)
                                                                {
                                                                    if (dr[dsCCDA.InsuranceProvider.InsuranceProviderAddress1Column.ColumnName] == DBNull.Value)
                                                                    {
                                                                        var streetAdd = adxp.Text.Where(str => !string.IsNullOrEmpty(str)).Aggregate("", (current, str) => current + " " + str.Trim());
                                                                        dr[dsCCDA.InsuranceProvider.InsuranceProviderAddress1Column.ColumnName] = streetAdd.ToUpper();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (!insuranceProvider.participant.IsNullOrEmpty())
                                        {
                                            Participant2 insuranceParticipant = insuranceProvider.participant[0] as Participant2;
                                            if (insuranceParticipant != null && insuranceParticipant.participantRole != null && insuranceParticipant.participantRole is ParticipantRole)
                                            {
                                                ParticipantRole participantRole = insuranceParticipant.participantRole as ParticipantRole;
                                                if (!participantRole.id.IsNullOrEmpty() && participantRole.id.Count() > 0)
                                                {
                                                    II insuranceNumber = participantRole.id[0] as II;
                                                    if (insuranceNumber != null && !string.IsNullOrWhiteSpace(insuranceNumber.extension))
                                                    {
                                                        dr[dsCCDA.InsuranceProvider.InsuranceNumberColumn.ColumnName] = insuranceNumber.extension;
                                                    }
                                                }
                                                if (participantRole.code != null && !string.IsNullOrWhiteSpace(participantRole.code.code))
                                                {
                                                    dr[dsCCDA.InsuranceProvider.SubscriberRelationshipCodeColumn.ColumnName] = participantRole.code.code;
                                                    dr[dsCCDA.InsuranceProvider.SubscriberRelationshipDisplayNameColumn.ColumnName] = participantRole.code.displayName;
                                                }
                                            }
                                        }

                                        dr[dsCCDA.InsuranceProvider.UserIdColumn.ColumnName] = UserId;
                                        dr[dsCCDA.InsuranceProvider.IsActiveColumn.ColumnName] = true;
                                        dr[dsCCDA.InsuranceProvider.CreatedByColumn.ColumnName] = CreatedBy;
                                        dr[dsCCDA.InsuranceProvider.CreatedOnColumn.ColumnName] = CreatedOn;
                                        dr[dsCCDA.InsuranceProvider.ModifiedByColumn.ColumnName] = CreatedBy;
                                        dr[dsCCDA.InsuranceProvider.ModifiedOnColumn.ColumnName] = CreatedOn;
                                        dr[dsCCDA.InsuranceProvider.ProviderIdColumn.ColumnName] = Convert.ToInt64(ProviderId);
                                        dr[dsCCDA.InsuranceProvider.FacilityIdColumn.ColumnName] = Convert.ToInt64(FacilityId);
                                        dtInsuranceProvider.Rows.Add(dr);
                                        i = i - 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Insurance Provider: " + ex.Message);
            }

        }
        #endregion Insurance Provider

        #region Care Plan

        #region Goals
        private static void GetCarePlanGoalsData(ClinicalDocument document, ref DataTable dtGoals, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 goalsComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref goalsComp, DocumentSections.Goals);
                List<Entry> entryList = null;
                if (isComponentExist)
                {
                    if (goalsComp.GetEntry(ref entryList))
                    {
                        int i = -1;
                        foreach (Entry e in entryList)
                        {
                            if (e != null && e.Item != null && e.Item is Observation)
                            {
                                Observation observation = e.Item as Observation;
                                DataRow dr = dtGoals.NewRow();
                                dr[dsCCDA.CarePlanGoals.PatientIdColumn.ColumnName] = patientId;
                                dr[dsCCDA.CarePlanGoals.GoalIdColumn.ColumnName] = i--;
                                II IdRoot = observation.id[0] as II;
                                if (IdRoot != null && !string.IsNullOrWhiteSpace(IdRoot.root))
                                {
                                    dr[dsCCDA.CarePlanGoals.RootIdColumn.ColumnName] = IdRoot.root;
                                }
                                if (observation.code != null && !string.IsNullOrWhiteSpace(observation.code.code))
                                {
                                    dr[dsCCDA.CarePlanGoals.LOINCCodeColumn.ColumnName] = observation.code.code;
                                    dr[dsCCDA.CarePlanGoals.LOINCDescriptionColumn.ColumnName] = observation.code.displayName;
                                    List<MDVision.Model.CCDA.CPTLookupModel> CPTLookupsList = BLLIMO.GetIMOCPTByLOINCCode(observation.code.code);
                                    if (CPTLookupsList != null && CPTLookupsList.Count > 0)
                                    {
                                        MDVision.Model.CCDA.CPTLookupModel objCPT = CPTLookupsList.FirstOrDefault(m => m.CPTCode != null || m.CPTCodeDescription != null);
                                        if (objCPT != null)
                                        {
                                            dr[dsCCDA.CarePlanGoals.CPTCodeColumn.ColumnName] = objCPT.CPTCode;
                                            dr[dsCCDA.CarePlanGoals.CPTDescriptionColumn.ColumnName] = objCPT.CPTCodeDescription;
                                            dr[dsCCDA.CarePlanGoals.CPTSNOMEDIDColumn.ColumnName] = objCPT.SNOMEDID;
                                            dr[dsCCDA.CarePlanGoals.CPTSNOMEDDescriptionColumn.ColumnName] = objCPT.SNOMEDDescription;
                                        }
                                    }
                                }
                                if (observation.statusCode != null && !string.IsNullOrWhiteSpace(observation.statusCode.code))
                                {
                                    dr[dsCCDA.CarePlanGoals.StatusColumn.ColumnName] = observation.statusCode.code;
                                }
                                if (observation.effectiveTime != null && observation.effectiveTime.Items != null && observation.effectiveTime.Items.Count() > 0 && observation.effectiveTime is IVL_TS)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(observation.effectiveTime, out lowTime, out highTime);
                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.CarePlanGoals.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.CarePlanGoals.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else if (observation.effectiveTime != null && !string.IsNullOrWhiteSpace(observation.effectiveTime.value) &&
                                observation.effectiveTime.value.ToLower() != "unk" && observation.effectiveTime.value.ToLower() != "ni")
                                {
                                    dr[dsCCDA.CarePlanGoals.StartDateColumn.ColumnName] = observation.effectiveTime.value.ToFormatedDateTimeByFormat();
                                }
                                if (!observation.value.IsNullOrEmpty() && observation.value.Count() > 0 && observation.value[0] is IVL_PQ)
                                {
                                    IVL_PQ value = observation.value[0] as IVL_PQ;
                                    if (!value.Items.IsNullOrEmpty())
                                    {
                                        foreach (var valueInner in value.Items)
                                        {
                                            dr[dsCCDA.CarePlanGoals.ValueColumn.ColumnName] = valueInner.value;
                                            dr[dsCCDA.CarePlanGoals.UnitColumn.ColumnName] = valueInner.unit;
                                        }
                                    }
                                }
                                if (!observation.entryRelationship.IsNullOrEmpty() && observation.entryRelationship.Count() > 0)
                                {
                                    PatientRole pr = document.GetPatientRole();
                                    string patientRootId = string.Empty;
                                    string providerRootId = string.Empty;
                                    if (pr != null && !pr.id.IsNullOrEmpty() && pr.id.Count() > 0)
                                    {
                                        II prId = pr.id[0] as II;
                                        patientRootId = prId.root;
                                    }
                                    AssignedAuthor assignedAuthor = document.GetAuthor();
                                    if (assignedAuthor != null && !assignedAuthor.id.IsNullOrEmpty() && assignedAuthor.id.Count() > 0)
                                    {
                                        II assignedAuthorId = assignedAuthor.id[0] as II;
                                        providerRootId = assignedAuthorId.root;
                                    }
                                    foreach (EntryRelationship entryRelationshipInner in observation.entryRelationship)
                                    {
                                        if (entryRelationshipInner != null && entryRelationshipInner.Item != null &&
                                            entryRelationshipInner.typeCode == x_ActRelationshipEntryRelationship.RSON && entryRelationshipInner.Item is Observation)
                                        {
                                            Observation observationInner = entryRelationshipInner.Item as Observation;
                                            II templateIdInner = observationInner.templateId[0] as II;
                                            if (templateIdInner != null && !string.IsNullOrWhiteSpace(templateIdInner.root)
                                                && templateIdInner.root == "2.16.840.1.113883.10.20.22.4.143")
                                            {
                                                if (!observationInner.author.IsNullOrEmpty() && observationInner.author.Count() > 0 && observationInner.author[0].assignedAuthor != null)
                                                {
                                                    AssignedAuthor AssignedAuthorInner = observationInner.author[0].assignedAuthor as AssignedAuthor;
                                                    II templateIdInnerAssignedAuthor = AssignedAuthorInner.id[0] as II;
                                                    if (templateIdInnerAssignedAuthor != null && !string.IsNullOrWhiteSpace(templateIdInnerAssignedAuthor.root)
                                                                && templateIdInnerAssignedAuthor.root == patientRootId)
                                                    {
                                                        if (!observationInner.value.IsNullOrEmpty() && observationInner.value.Count() > 0 && observationInner.value[0] is CD)
                                                        {
                                                            CD value = observationInner.value[0] as CD;
                                                            if (!string.IsNullOrWhiteSpace(value.code))
                                                            {
                                                                dr[dsCCDA.CarePlanGoals.PatientPriorityCodeColumn.ColumnName] = value.code;
                                                                dr[dsCCDA.CarePlanGoals.PatientPriorityColumn.ColumnName] = value.displayName;
                                                            }
                                                        }
                                                    }
                                                    else if (templateIdInnerAssignedAuthor != null && !string.IsNullOrWhiteSpace(templateIdInnerAssignedAuthor.root)
                                                                && templateIdInnerAssignedAuthor.root == providerRootId)
                                                    {
                                                        if (!observationInner.value.IsNullOrEmpty() && observationInner.value.Count() > 0 && observationInner.value[0] is CD)
                                                        {
                                                            CD value = observationInner.value[0] as CD;
                                                            if (!string.IsNullOrWhiteSpace(value.code))
                                                            {
                                                                dr[dsCCDA.CarePlanGoals.ProviderPriorityCodeColumn.ColumnName] = value.code;
                                                                dr[dsCCDA.CarePlanGoals.ProviderPriorityColumn.ColumnName] = value.displayName;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (dr[dsCCDA.CarePlanGoals.LOINCCodeColumn.ColumnName] != DBNull.Value)
                                {
                                    dr[dsCCDA.CarePlanGoals.PatientIdColumn.ColumnName] = patientId;
                                    dr[dsCCDA.CarePlanGoals.GoalIdColumn.ColumnName] = i--;
                                    dr[dsCCDA.CarePlanGoals.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                                    if (!string.IsNullOrWhiteSpace(FacilityId))
                                        dr[dsCCDA.CarePlanGoals.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                                    dr[dsCCDA.CarePlanGoals.CreatedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.CarePlanGoals.CreatedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.CarePlanGoals.IsActiveColumn.ColumnName] = true;
                                    dr[dsCCDA.CarePlanGoals.ModifiedByColumn.ColumnName] = ModifiedBy;
                                    dr[dsCCDA.CarePlanGoals.ModifiedOnColumn.ColumnName] = CreatedOn;
                                    dtGoals.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Care Plan - Goals: " + ex.Message);
            }
        }
        #endregion Goals

        #region Health Concern
        private static void GetCarePlanHealthConcernData(ClinicalDocument document, ref DataTable dtHealthConcerns, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 healthConcernsComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref healthConcernsComp, DocumentSections.HealthConcerns);
                List<Entry> entryList = null;
                if (isComponentExist)
                {
                    if (healthConcernsComp.GetEntry(ref entryList))
                    {
                        int i = -1;
                        foreach (Entry e in entryList)
                        {
                            if (e != null && e.Item != null && e.Item is Act)
                            {
                                Act InterventionAct = e.Item as Act;
                                II templateId = InterventionAct.templateId[0] as II;

                                #region Health Concern Act
                                if (!string.IsNullOrWhiteSpace(templateId.root) && templateId.root == "2.16.840.1.113883.10.20.22.4.132")
                                {
                                    if (!InterventionAct.entryRelationship.IsNullOrEmpty() && InterventionAct.entryRelationship.Count() > 0)
                                    {
                                        foreach (EntryRelationship entryRelationship in InterventionAct.entryRelationship)
                                        {
                                            if (entryRelationship != null && entryRelationship.Item != null &&
                                                (entryRelationship.typeCode == x_ActRelationshipEntryRelationship.REFR || entryRelationship.typeCode == x_ActRelationshipEntryRelationship.SPRT) && entryRelationship.Item is Observation)
                                            {
                                                DataRow dr = dtHealthConcerns.NewRow();
                                                Observation observation = entryRelationship.Item as Observation;
                                                if (observation.effectiveTime != null && observation.effectiveTime.Items != null
                                                    && observation.effectiveTime.Items.Count() > 0 && observation.effectiveTime is IVL_TS)
                                                {
                                                    string lowTime, highTime;
                                                    GetLowHighTime(observation.effectiveTime, out lowTime, out highTime);
                                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                                    {
                                                        dr[dsCCDA.HealthConcerns.ObservationDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                                    }
                                                }
                                                else if (observation.effectiveTime != null && !string.IsNullOrWhiteSpace(observation.effectiveTime.value) &&
                                                observation.effectiveTime.value.ToLower() != "unk" && observation.effectiveTime.value.ToLower() != "ni")
                                                {
                                                    dr[dsCCDA.HealthConcerns.ObservationDateColumn.ColumnName] = observation.effectiveTime.value.ToFormatedDateTimeByFormat();
                                                }
                                                if (observation.statusCode != null && !string.IsNullOrWhiteSpace(observation.statusCode.code) && observation.statusCode.code == "active")
                                                {
                                                    dr[dsCCDA.HealthConcerns.ObservationStatusColumn.ColumnName] = "Active";
                                                }
                                                else if (observation.statusCode != null && !string.IsNullOrWhiteSpace(observation.statusCode.code) && observation.statusCode.code == "completed")
                                                {
                                                    dr[dsCCDA.HealthConcerns.ObservationStatusColumn.ColumnName] = "Completed";
                                                }
                                                if (!observation.value.IsNullOrEmpty() && observation.value.Count() > 0 && observation.value[0] is CD)
                                                {
                                                    CD value = observation.value[0] as CD;
                                                    if (!string.IsNullOrWhiteSpace(value.code))
                                                    {
                                                        dr[dsCCDA.HealthConcerns.ObservationSNOMEDIDColumn.ColumnName] = value.code;
                                                        dr[dsCCDA.HealthConcerns.ObservationSNOMEDDescColumn.ColumnName] = value.displayName;
                                                        List<MDVision.Model.CCDA.ICDLookupModel> ICDLookupsList = BLLIMO.GetIMOICDbySNOMEDCode(value.code);
                                                        if (ICDLookupsList != null && ICDLookupsList.Count > 0)
                                                        {
                                                            MDVision.Model.CCDA.ICDLookupModel objICD = ICDLookupsList.FirstOrDefault(m => m.ICD10CM_CODE != null || m.ICD10CM_TITLE != null || m.ICD9CM_CODE != null || m.ICD9CM_TITLE != null);
                                                            if (objICD != null)
                                                            {
                                                                dr[dsCCDA.HealthConcerns.ObservationICD9CodeColumn.ColumnName] = objICD.ICD9CM_CODE;
                                                                dr[dsCCDA.HealthConcerns.ObservationICD10CodeColumn.ColumnName] = objICD.ICD10CM_CODE;
                                                                dr[dsCCDA.HealthConcerns.ObservationICD9DescriptionColumn.ColumnName] = value.displayName;
                                                                dr[dsCCDA.HealthConcerns.ObservationICD10DescriptionColumn.ColumnName] = value.displayName;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (entryRelationship != null && entryRelationship.Item != null &&
                                                (entryRelationship.typeCode == x_ActRelationshipEntryRelationship.REFR || entryRelationship.typeCode == x_ActRelationshipEntryRelationship.SPRT) && entryRelationship.Item is Observation)
                                                {
                                                    PatientRole pr = document.GetPatientRole();
                                                    string patientRootId = string.Empty;
                                                    string providerRootId = string.Empty;
                                                    if (pr != null && !pr.id.IsNullOrEmpty() && pr.id.Count() > 0)
                                                    {
                                                        II prId = pr.id[0] as II;
                                                        patientRootId = prId.root;
                                                    }
                                                    AssignedAuthor assignedAuthor = document.GetAuthor();
                                                    if (assignedAuthor != null && !assignedAuthor.id.IsNullOrEmpty() && assignedAuthor.id.Count() > 0)
                                                    {
                                                        II assignedAuthorId = assignedAuthor.id[0] as II;
                                                        providerRootId = assignedAuthorId.root;
                                                    }
                                                    foreach (EntryRelationship entryRelationshipInner in InterventionAct.entryRelationship)
                                                    {
                                                        if (entryRelationshipInner != null && entryRelationshipInner.Item != null &&
                                                            entryRelationshipInner.typeCode == x_ActRelationshipEntryRelationship.RSON && entryRelationshipInner.Item is Observation)
                                                        {
                                                            Observation observationInner = entryRelationshipInner.Item as Observation;
                                                            II templateIdInner = observationInner.templateId[0] as II;
                                                            if (templateIdInner != null && !string.IsNullOrWhiteSpace(templateIdInner.root)
                                                                && templateIdInner.root == "2.16.840.1.113883.10.20.22.4.143")
                                                            {
                                                                if (!observationInner.author.IsNullOrEmpty() && observationInner.author.Count() > 0 && observationInner.author[0].assignedAuthor != null)
                                                                {
                                                                    AssignedAuthor AssignedAuthorInner = observationInner.author[0].assignedAuthor as AssignedAuthor;
                                                                    II templateIdInnerAssignedAuthor = AssignedAuthorInner.id[0] as II;
                                                                    if (templateIdInnerAssignedAuthor != null && !string.IsNullOrWhiteSpace(templateIdInnerAssignedAuthor.root)
                                                                                && templateIdInnerAssignedAuthor.root == patientRootId)
                                                                    {
                                                                        if (!observationInner.value.IsNullOrEmpty() && observationInner.value.Count() > 0 && observationInner.value[0] is CD)
                                                                        {
                                                                            CD value = observationInner.value[0] as CD;
                                                                            if (!string.IsNullOrWhiteSpace(value.code))
                                                                            {
                                                                                dr[dsCCDA.HealthConcerns.ObsPatientPriorityCodeColumn.ColumnName] = value.code;
                                                                                dr[dsCCDA.HealthConcerns.ObsPatientPriorityColumn.ColumnName] = value.displayName;
                                                                            }
                                                                        }
                                                                    }
                                                                    else if (templateIdInnerAssignedAuthor != null && !string.IsNullOrWhiteSpace(templateIdInnerAssignedAuthor.root)
                                                                                && templateIdInnerAssignedAuthor.root == providerRootId)
                                                                    {
                                                                        if (!observationInner.value.IsNullOrEmpty() && observationInner.value.Count() > 0 && observationInner.value[0] is CD)
                                                                        {
                                                                            CD value = observationInner.value[0] as CD;
                                                                            if (!string.IsNullOrWhiteSpace(value.code))
                                                                            {
                                                                                dr[dsCCDA.HealthConcerns.ObsProviderPriorityCodeColumn.ColumnName] = value.code;
                                                                                dr[dsCCDA.HealthConcerns.ObsProviderPriorityColumn.ColumnName] = value.displayName;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (dr[dsCCDA.HealthConcerns.ObservationSNOMEDIDColumn.ColumnName] != DBNull.Value)
                                                {
                                                    dr[dsCCDA.HealthConcerns.PatientIdColumn.ColumnName] = patientId;
                                                    dr[dsCCDA.HealthConcerns.HealthConcernIdColumn.ColumnName] = i--;
                                                    dr[dsCCDA.HealthConcerns.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                                                    if (!string.IsNullOrWhiteSpace(FacilityId))
                                                        dr[dsCCDA.HealthConcerns.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                                                    dr[dsCCDA.HealthConcerns.CreatedByColumn.ColumnName] = CreatedBy;
                                                    dr[dsCCDA.HealthConcerns.CreatedOnColumn.ColumnName] = CreatedOn;
                                                    dr[dsCCDA.HealthConcerns.IsActiveColumn.ColumnName] = true;
                                                    dr[dsCCDA.HealthConcerns.ModifiedByColumn.ColumnName] = ModifiedBy;
                                                    dr[dsCCDA.HealthConcerns.ModifiedOnColumn.ColumnName] = CreatedOn;
                                                    dtHealthConcerns.Rows.Add(dr);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion Health Concern Act

                                #region Risk Concern Act

                                else if (!string.IsNullOrWhiteSpace(templateId.root) && templateId.root == "2.16.840.1.113883.10.20.22.4.136")
                                {
                                    if (!InterventionAct.entryRelationship.IsNullOrEmpty() && InterventionAct.entryRelationship.Count() > 0)
                                    {
                                        foreach (EntryRelationship entryRelationship in InterventionAct.entryRelationship)
                                        {
                                            if (entryRelationship != null && entryRelationship.Item != null &&
                                                entryRelationship.typeCode == x_ActRelationshipEntryRelationship.REFR && entryRelationship.Item is Observation)
                                            {
                                                DataRow dr = dtHealthConcerns.NewRow();

                                                Observation observation = entryRelationship.Item as Observation;
                                                if (observation.effectiveTime != null && observation.effectiveTime.Items != null
                                                    && observation.effectiveTime.Items.Count() > 0 && observation.effectiveTime is IVL_TS)
                                                {
                                                    string lowTime, highTime;
                                                    GetLowHighTime(observation.effectiveTime, out lowTime, out highTime);
                                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                                    {
                                                        dr[dsCCDA.HealthConcerns.RiskDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                                    }
                                                }
                                                else if (observation.effectiveTime != null && !string.IsNullOrWhiteSpace(observation.effectiveTime.value) &&
                                                observation.effectiveTime.value.ToLower() != "unk" && observation.effectiveTime.value.ToLower() != "ni")
                                                {
                                                    dr[dsCCDA.HealthConcerns.RiskDateColumn.ColumnName] = observation.effectiveTime.value.ToFormatedDateTimeByFormat();
                                                }
                                                if (observation.statusCode != null && !string.IsNullOrWhiteSpace(observation.statusCode.code) && observation.statusCode.code == "active")
                                                {
                                                    dr[dsCCDA.HealthConcerns.RiskStatusColumn.ColumnName] = "Active";
                                                }
                                                else if (observation.statusCode != null && !string.IsNullOrWhiteSpace(observation.statusCode.code) && observation.statusCode.code == "completed")
                                                {
                                                    dr[dsCCDA.HealthConcerns.RiskStatusColumn.ColumnName] = "Completed";
                                                }
                                                if (!observation.value.IsNullOrEmpty() && observation.value.Count() > 0 && observation.value[0] is CD)
                                                {
                                                    CD value = observation.value[0] as CD;
                                                    if (!string.IsNullOrWhiteSpace(value.code))
                                                    {
                                                        dr[dsCCDA.HealthConcerns.RiskSNOMEDIDColumn.ColumnName] = value.code;
                                                        dr[dsCCDA.HealthConcerns.RiskSNOMEDDescColumn.ColumnName] = value.displayName;
                                                        List<MDVision.Model.CCDA.ICDLookupModel> ICDLookupsList = BLLIMO.GetIMOICDbySNOMEDCode(value.code);
                                                        if (ICDLookupsList != null && ICDLookupsList.Count > 0)
                                                        {
                                                            MDVision.Model.CCDA.ICDLookupModel objICD = ICDLookupsList.FirstOrDefault(m => m.ICD10CM_CODE != null || m.ICD10CM_TITLE != null || m.ICD9CM_CODE != null || m.ICD9CM_TITLE != null);
                                                            if (objICD != null)
                                                            {
                                                                dr[dsCCDA.HealthConcerns.RiskICD9CodeColumn.ColumnName] = objICD.ICD9CM_CODE;
                                                                dr[dsCCDA.HealthConcerns.RiskICD10CodeColumn.ColumnName] = objICD.ICD10CM_CODE;
                                                                dr[dsCCDA.HealthConcerns.RiskICD9DescriptionColumn.ColumnName] = value.displayName;
                                                                dr[dsCCDA.HealthConcerns.RiskICD10DescriptionColumn.ColumnName] = value.displayName;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (entryRelationship != null && entryRelationship.Item != null &&
                                                entryRelationship.typeCode == x_ActRelationshipEntryRelationship.REFR && entryRelationship.Item is Observation)
                                                {
                                                    PatientRole pr = document.GetPatientRole();
                                                    string patientRootId = string.Empty;
                                                    string providerRootId = string.Empty;
                                                    if (pr != null && !pr.id.IsNullOrEmpty() && pr.id.Count() > 0)
                                                    {
                                                        II prId = pr.id[0] as II;
                                                        patientRootId = prId.root;
                                                    }
                                                    AssignedAuthor assignedAuthor = document.GetAuthor();
                                                    if (assignedAuthor != null && !assignedAuthor.id.IsNullOrEmpty() && assignedAuthor.id.Count() > 0)
                                                    {
                                                        II assignedAuthorId = assignedAuthor.id[0] as II;
                                                        providerRootId = assignedAuthorId.root;
                                                    }
                                                    foreach (EntryRelationship entryRelationshipInner in InterventionAct.entryRelationship)
                                                    {
                                                        if (entryRelationshipInner != null && entryRelationshipInner.Item != null &&
                                                            entryRelationshipInner.typeCode == x_ActRelationshipEntryRelationship.RSON && entryRelationshipInner.Item is Observation)
                                                        {
                                                            Observation observationInner = entryRelationshipInner.Item as Observation;
                                                            II templateIdInner = observationInner.templateId[0] as II;
                                                            if (templateIdInner != null && !string.IsNullOrWhiteSpace(templateIdInner.root)
                                                                && templateIdInner.root == "2.16.840.1.113883.10.20.22.4.143")
                                                            {
                                                                if (!observationInner.author.IsNullOrEmpty() && observationInner.author.Count() > 0 && observationInner.author[0].assignedAuthor != null)
                                                                {
                                                                    AssignedAuthor AssignedAuthorInner = observationInner.author[0].assignedAuthor as AssignedAuthor;
                                                                    II templateIdInnerAssignedAuthor = AssignedAuthorInner.id[0] as II;
                                                                    if (templateIdInnerAssignedAuthor != null && !string.IsNullOrWhiteSpace(templateIdInnerAssignedAuthor.root)
                                                                                && templateIdInnerAssignedAuthor.root == patientRootId)
                                                                    {
                                                                        if (!observationInner.value.IsNullOrEmpty() && observationInner.value.Count() > 0 && observationInner.value[0] is CD)
                                                                        {
                                                                            CD value = observationInner.value[0] as CD;
                                                                            if (!string.IsNullOrWhiteSpace(value.code))
                                                                            {
                                                                                dr[dsCCDA.HealthConcerns.RiskPatientPriorityCodeColumn.ColumnName] = value.code;
                                                                                dr[dsCCDA.HealthConcerns.RiskPatientPriorityColumn.ColumnName] = value.displayName;
                                                                            }
                                                                        }
                                                                    }
                                                                    else if (templateIdInnerAssignedAuthor != null && !string.IsNullOrWhiteSpace(templateIdInnerAssignedAuthor.root)
                                                                                && templateIdInnerAssignedAuthor.root == providerRootId)
                                                                    {
                                                                        if (!observationInner.value.IsNullOrEmpty() && observationInner.value.Count() > 0 && observationInner.value[0] is CD)
                                                                        {
                                                                            CD value = observationInner.value[0] as CD;
                                                                            if (!string.IsNullOrWhiteSpace(value.code))
                                                                            {
                                                                                dr[dsCCDA.HealthConcerns.RiskProviderPriorityCodeColumn.ColumnName] = value.code;
                                                                                dr[dsCCDA.HealthConcerns.RiskProviderPriorityColumn.ColumnName] = value.displayName;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (dr[dsCCDA.HealthConcerns.RiskSNOMEDIDColumn.ColumnName] != DBNull.Value)
                                                {
                                                    dr[dsCCDA.HealthConcerns.PatientIdColumn.ColumnName] = patientId;
                                                    dr[dsCCDA.HealthConcerns.HealthConcernIdColumn.ColumnName] = i--;
                                                    dr[dsCCDA.HealthConcerns.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                                                    if (!string.IsNullOrWhiteSpace(FacilityId))
                                                        dr[dsCCDA.HealthConcerns.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                                                    dr[dsCCDA.HealthConcerns.CreatedByColumn.ColumnName] = CreatedBy;
                                                    dr[dsCCDA.HealthConcerns.CreatedOnColumn.ColumnName] = CreatedOn;
                                                    dr[dsCCDA.HealthConcerns.IsActiveColumn.ColumnName] = true;
                                                    dr[dsCCDA.HealthConcerns.ModifiedByColumn.ColumnName] = ModifiedBy;
                                                    dr[dsCCDA.HealthConcerns.ModifiedOnColumn.ColumnName] = CreatedOn;
                                                    dtHealthConcerns.Rows.Add(dr);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion Risk Concern Act

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Care Plan - Health Concerns: " + ex.Message);
            }
        }
        #endregion Health Concern

        #region Interventions 
        private static void GetCarePlanInterventionsData(ClinicalDocument document, ref DataTable dtInterventions, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 interventionsComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref interventionsComp, DocumentSections.Intervention);
                List<Entry> entryList = null;
                if (isComponentExist)
                {
                    if (interventionsComp.GetEntry(ref entryList))
                    {
                        int i = -1;
                        foreach (Entry e in entryList)
                        {
                            if (e != null && e.Item != null && e.Item is Act)
                            {
                                Act InterventionAct = e.Item as Act;
                                II templateId = InterventionAct.templateId[0] as II;
                                string status = string.Empty;
                                if (!string.IsNullOrWhiteSpace(templateId.root) && templateId.root == "2.16.840.1.113883.10.20.22.4.146")
                                {
                                    status = "Active";
                                }
                                else if (!string.IsNullOrWhiteSpace(templateId.root) && templateId.root == "2.16.840.1.113883.10.20.22.4.131")
                                {
                                    status = "Completed";
                                }
                                if (!InterventionAct.entryRelationship.IsNullOrEmpty() && InterventionAct.entryRelationship.Count() > 0)
                                {
                                    foreach (EntryRelationship entryRelationship in InterventionAct.entryRelationship)
                                    {
                                        DataRow dr = dtInterventions.NewRow();

                                        if (entryRelationship != null && entryRelationship.Item != null &&
                                            entryRelationship.typeCode == x_ActRelationshipEntryRelationship.REFR && entryRelationship.Item is Act)
                                        {
                                            Act InterventionActInner = entryRelationship.Item as Act;
                                            if (!InterventionActInner.templateId.IsNullOrEmpty() && InterventionActInner.templateId.Count() > 0 &&
                                                (InterventionActInner.templateId[0].root == "2.16.840.1.113883.10.20.22.4.39" || InterventionActInner.templateId[0].root == "2.16.840.1.113883.10.20.22.4.12"))
                                            {
                                                if (InterventionActInner.code != null && !string.IsNullOrWhiteSpace(InterventionActInner.code.code))
                                                {
                                                    dr[dsCCDA.Interventions.SNOMEDIDColumn.ColumnName] = InterventionActInner.code.code;
                                                    dr[dsCCDA.Interventions.SNOMEDDescriptionColumn.ColumnName] = InterventionActInner.code.displayName;
                                                    dr[dsCCDA.Interventions.CodeColumn.ColumnName] = InterventionActInner.code.code;
                                                    dr[dsCCDA.Interventions.CodeDescriptionColumn.ColumnName] = InterventionActInner.code.displayName;
                                                    dr[dsCCDA.Interventions.CodeSystemNameColumn.ColumnName] = InterventionActInner.code.codeSystemName;
                                                    dr[dsCCDA.Interventions.CodeSystemColumn.ColumnName] = InterventionActInner.code.codeSystem;
                                                    List<MDVision.Model.CCDA.ICDLookupModel> ICDLookupsList = BLLIMO.GetIMOICDbySNOMEDCode(InterventionActInner.code.code);
                                                    if (ICDLookupsList != null && ICDLookupsList.Count > 0)
                                                    {
                                                        MDVision.Model.CCDA.ICDLookupModel objICD = ICDLookupsList.FirstOrDefault(m => m.ICD10CM_CODE != null || m.ICD10CM_TITLE != null || m.ICD9CM_CODE != null || m.ICD9CM_TITLE != null);
                                                        if (objICD != null)
                                                        {
                                                            dr[dsCCDA.Interventions.ICD9CodeColumn.ColumnName] = objICD.ICD9CM_CODE;
                                                            dr[dsCCDA.Interventions.ICD10CodeColumn.ColumnName] = objICD.ICD10CM_CODE;
                                                            dr[dsCCDA.Interventions.ICD9_DescriptionColumn.ColumnName] = objICD.ICD9CM_TITLE;
                                                            dr[dsCCDA.Interventions.ICD10_DescriptionColumn.ColumnName] = objICD.ICD10CM_TITLE;
                                                        }
                                                    }
                                                }
                                                if (InterventionActInner.effectiveTime != null && InterventionActInner.effectiveTime.Items != null
                                                    && InterventionActInner.effectiveTime.Items.Count() > 0 && InterventionActInner.effectiveTime is IVL_TS)
                                                {
                                                    string lowTime, highTime;
                                                    GetLowHighTime(InterventionActInner.effectiveTime, out lowTime, out highTime);
                                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                                    {
                                                        dr[dsCCDA.Interventions.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                                    }
                                                    if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                                    {
                                                        dr[dsCCDA.Interventions.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                                    }
                                                }
                                                else if (InterventionActInner.effectiveTime != null && !string.IsNullOrWhiteSpace(InterventionActInner.effectiveTime.value) &&
                                                InterventionActInner.effectiveTime.value.ToLower() != "unk" && InterventionActInner.effectiveTime.value.ToLower() != "ni")
                                                {
                                                    dr[dsCCDA.Interventions.StartDateColumn.ColumnName] = InterventionActInner.effectiveTime.value.ToFormatedDateTimeByFormat();
                                                }
                                                //if (!InterventionActInner.entryRelationship.IsNullOrEmpty() && InterventionActInner.entryRelationship.Count() > 0)
                                                //{
                                                //    EntryRelationship entryRelationshipInner = InterventionActInner.entryRelationship[0] as EntryRelationship;
                                                //}
                                            }
                                        }
                                        else if (entryRelationship != null && entryRelationship.Item != null &&
                                            entryRelationship.typeCode == x_ActRelationshipEntryRelationship.REFR && entryRelationship.Item is Procedure)
                                        {
                                            Procedure InterventionProcedureInner = entryRelationship.Item as Procedure;
                                            if (!InterventionProcedureInner.templateId.IsNullOrEmpty() && InterventionProcedureInner.templateId.Count() > 0 &&
                                                InterventionProcedureInner.templateId[0].root == "2.16.840.1.113883.10.20.22.4.41")
                                            {
                                                if (InterventionProcedureInner.code != null && !string.IsNullOrWhiteSpace(InterventionProcedureInner.code.code))
                                                {
                                                    dr[dsCCDA.Interventions.SNOMEDIDColumn.ColumnName] = InterventionProcedureInner.code.code;
                                                    dr[dsCCDA.Interventions.SNOMEDDescriptionColumn.ColumnName] = InterventionProcedureInner.code.displayName;
                                                    dr[dsCCDA.Interventions.CodeColumn.ColumnName] = InterventionProcedureInner.code.code;
                                                    dr[dsCCDA.Interventions.CodeDescriptionColumn.ColumnName] = InterventionProcedureInner.code.displayName;
                                                    dr[dsCCDA.Interventions.CodeSystemNameColumn.ColumnName] = InterventionProcedureInner.code.codeSystemName;
                                                    dr[dsCCDA.Interventions.CodeSystemColumn.ColumnName] = InterventionProcedureInner.code.codeSystem;
                                                    List<MDVision.Model.CCDA.ICDLookupModel> ICDLookupsList = BLLIMO.GetIMOICDbySNOMEDCode(InterventionProcedureInner.code.code);
                                                    if (ICDLookupsList != null && ICDLookupsList.Count > 0)
                                                    {
                                                        MDVision.Model.CCDA.ICDLookupModel objICD = ICDLookupsList.FirstOrDefault(m => m.ICD10CM_CODE != null || m.ICD10CM_TITLE != null || m.ICD9CM_CODE != null || m.ICD9CM_TITLE != null);
                                                        if (objICD != null)
                                                        {
                                                            dr[dsCCDA.Interventions.ICD9CodeColumn.ColumnName] = objICD.ICD9CM_CODE;
                                                            dr[dsCCDA.Interventions.ICD10CodeColumn.ColumnName] = objICD.ICD10CM_CODE;
                                                            dr[dsCCDA.Interventions.ICD9_DescriptionColumn.ColumnName] = objICD.ICD9CM_TITLE;
                                                            dr[dsCCDA.Interventions.ICD10_DescriptionColumn.ColumnName] = objICD.ICD10CM_TITLE;
                                                        }
                                                    }
                                                }
                                                if (InterventionProcedureInner.effectiveTime != null && InterventionProcedureInner.effectiveTime.Items != null
                                                    && InterventionProcedureInner.effectiveTime.Items.Count() > 0 && InterventionProcedureInner.effectiveTime is IVL_TS)
                                                {
                                                    string lowTime, highTime;
                                                    GetLowHighTime(InterventionProcedureInner.effectiveTime, out lowTime, out highTime);
                                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                                    {
                                                        dr[dsCCDA.Interventions.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                                    }
                                                    if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                                    {
                                                        dr[dsCCDA.Interventions.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                                    }
                                                }
                                                else if (InterventionProcedureInner.effectiveTime != null && !string.IsNullOrWhiteSpace(InterventionProcedureInner.effectiveTime.value) &&
                                                InterventionProcedureInner.effectiveTime.value.ToLower() != "unk" && InterventionProcedureInner.effectiveTime.value.ToLower() != "ni")
                                                {
                                                    dr[dsCCDA.Interventions.StartDateColumn.ColumnName] = InterventionProcedureInner.effectiveTime.value.ToFormatedDateTimeByFormat();
                                                }
                                                //if (!InterventionProcedureInner.entryRelationship.IsNullOrEmpty() && InterventionProcedureInner.entryRelationship.Count() > 0)
                                                //{
                                                //    EntryRelationship entryRelationshipInner = InterventionProcedureInner.entryRelationship[0] as EntryRelationship;
                                                //}
                                            }
                                        }
                                        else if (entryRelationship != null && entryRelationship.Item != null &&
                                            entryRelationship.typeCode == x_ActRelationshipEntryRelationship.REFR && entryRelationship.Item is Observation)
                                        {
                                            Observation InterventionObservationInner = entryRelationship.Item as Observation;
                                            if (!InterventionObservationInner.templateId.IsNullOrEmpty() && InterventionObservationInner.templateId.Count() > 0 &&
                                                InterventionObservationInner.templateId[0].root == "2.16.840.1.113883.10.20.22.4.44")
                                            {
                                                if (InterventionObservationInner.code != null && !string.IsNullOrWhiteSpace(InterventionObservationInner.code.code))
                                                {
                                                    dr[dsCCDA.Interventions.LOINCCodeColumn.ColumnName] = InterventionObservationInner.code.code;
                                                    dr[dsCCDA.Interventions.LOINCDescriptionColumn.ColumnName] = InterventionObservationInner.code.displayName;
                                                    dr[dsCCDA.Interventions.CodeColumn.ColumnName] = InterventionObservationInner.code.code;
                                                    dr[dsCCDA.Interventions.CodeDescriptionColumn.ColumnName] = InterventionObservationInner.code.displayName;
                                                    dr[dsCCDA.Interventions.CodeSystemNameColumn.ColumnName] = InterventionObservationInner.code.codeSystemName;
                                                    dr[dsCCDA.Interventions.CodeSystemColumn.ColumnName] = InterventionObservationInner.code.codeSystem;
                                                    List<MDVision.Model.CCDA.CPTLookupModel> CPTLookupsList = BLLIMO.GetIMOCPTByLOINCCode(InterventionObservationInner.code.code);
                                                    if (CPTLookupsList != null && CPTLookupsList.Count > 0)
                                                    {
                                                        MDVision.Model.CCDA.CPTLookupModel objCPT = CPTLookupsList.FirstOrDefault(m => m.CPTCode != null || m.CPTCodeDescription != null);
                                                        if (objCPT != null)
                                                        {
                                                            dr[dsCCDA.Interventions.CPTCodeColumn.ColumnName] = objCPT.CPTCode;
                                                            dr[dsCCDA.Interventions.CPTDescriptionColumn.ColumnName] = objCPT.CPTCodeDescription;
                                                            dr[dsCCDA.Interventions.SNOMEDIDColumn.ColumnName] = objCPT.SNOMEDID;
                                                            dr[dsCCDA.Interventions.SNOMEDDescriptionColumn.ColumnName] = objCPT.SNOMEDDescription;
                                                        }
                                                    }
                                                }
                                                if (InterventionObservationInner.effectiveTime != null && InterventionObservationInner.effectiveTime.Items != null
                                                    && InterventionObservationInner.effectiveTime.Items.Count() > 0 && InterventionObservationInner.effectiveTime is IVL_TS)
                                                {
                                                    string lowTime, highTime;
                                                    GetLowHighTime(InterventionObservationInner.effectiveTime, out lowTime, out highTime);
                                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                                    {
                                                        dr[dsCCDA.Interventions.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                                    }
                                                    if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                                    {
                                                        dr[dsCCDA.Interventions.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                                    }
                                                }
                                                else if (InterventionObservationInner.effectiveTime != null && !string.IsNullOrWhiteSpace(InterventionObservationInner.effectiveTime.value) &&
                                                InterventionObservationInner.effectiveTime.value.ToLower() != "unk" && InterventionObservationInner.effectiveTime.value.ToLower() != "ni")
                                                {
                                                    dr[dsCCDA.Interventions.StartDateColumn.ColumnName] = InterventionObservationInner.effectiveTime.value.ToFormatedDateTimeByFormat();
                                                }
                                                if (!InterventionObservationInner.entryRelationship.IsNullOrEmpty() && InterventionObservationInner.entryRelationship.Count() > 0)
                                                {
                                                    EntryRelationship entryRelationshipInner = InterventionObservationInner.entryRelationship[0] as EntryRelationship;
                                                }
                                                //if (!InterventionObservationInner.entryRelationship.IsNullOrEmpty() && InterventionObservationInner.entryRelationship.Count() > 0)
                                                //{
                                                //}
                                            }
                                        }
                                        //else if (entryRelationship != null && entryRelationship.Item != null &&
                                        //    entryRelationship.typeCode == x_ActRelationshipEntryRelationship.RSON && entryRelationship.Item is Act)
                                        //{
                                        //    Act InterventionActInnerGoal = entryRelationship.Item as Act;
                                        //    II templateIdGoal = InterventionActInnerGoal.templateId[0] as II;
                                        //    if (templateIdGoal != null && !string.IsNullOrWhiteSpace(templateIdGoal.root) && templateIdGoal.root == "2.16.840.1.113883.10.20.22.4.122")
                                        //    {
                                        //        II IdRoot = InterventionActInnerGoal.id[0] as II;
                                        //        if (IdRoot != null && !string.IsNullOrWhiteSpace(IdRoot.root))
                                        //        {
                                        //            dr[dsCCDA.Interventions.GoalRootIdColumn.ColumnName] = IdRoot.root;
                                        //        }
                                        //    }
                                        //}
                                        if (dr[dsCCDA.Interventions.CodeColumn.ColumnName] != DBNull.Value)
                                        {
                                            dr[dsCCDA.Interventions.PatientIdColumn.ColumnName] = patientId;
                                            dr[dsCCDA.Interventions.InterventionIdColumn.ColumnName] = i--;
                                            dr[dsCCDA.Interventions.StatusColumn.ColumnName] = status;
                                            dr[dsCCDA.Interventions.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                                            if (!string.IsNullOrWhiteSpace(FacilityId))
                                                dr[dsCCDA.Interventions.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                                            dr[dsCCDA.Interventions.IsActiveColumn.ColumnName] = true;
                                            dr[dsCCDA.Interventions.CreatedByColumn.ColumnName] = CreatedBy;
                                            dr[dsCCDA.Interventions.CreatedOnColumn.ColumnName] = CreatedOn;
                                            dr[dsCCDA.Interventions.ModifiedByColumn.ColumnName] = ModifiedBy;
                                            dr[dsCCDA.Interventions.ModifiedOnColumn.ColumnName] = CreatedOn;
                                            dtInterventions.Rows.Add(dr);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Care Plan - Interventions: " + ex.Message);
            }
        }
        #endregion Interventions

        #region Health Status Evaluations/Outcomes Section
        private static void GetCarePlanHealthStatusData(ClinicalDocument document, ref DataTable dtHealthStatus, ref List<string> errors, long patientId)
        {
            try
            {
                Component3 healthStatusComp = new Component3();
                bool isComponentExist = document.GetComponentByTemplateId(ref healthStatusComp, DocumentSections.HealthStatus);
                List<Entry> entryList = null;
                if (isComponentExist)
                {
                    if (healthStatusComp.GetEntry(ref entryList))
                    {
                        int i = -1;
                        foreach (Entry e in entryList)
                        {
                            if (e != null && e.Item != null && e.Item is Observation)
                            {
                                Observation observation = e.Item as Observation;
                                DataRow dr = dtHealthStatus.NewRow();
                                if (observation.code != null && !string.IsNullOrWhiteSpace(observation.code.code))
                                {
                                    dr[dsCCDA.HealthStatus.LOINCCodeColumn.ColumnName] = observation.code.code;
                                    dr[dsCCDA.HealthStatus.LOINCDescriptionColumn.ColumnName] = observation.code.displayName;
                                    List<MDVision.Model.CCDA.CPTLookupModel> CPTLookupsList = BLLIMO.GetIMOCPTByLOINCCode(observation.code.code);
                                    if (CPTLookupsList != null && CPTLookupsList.Count > 0)
                                    {
                                        MDVision.Model.CCDA.CPTLookupModel objCPT = CPTLookupsList.FirstOrDefault(m => m.CPTCode != null || m.CPTCodeDescription != null || m.SNOMEDID != null || m.SNOMEDDescription != null);
                                        if (objCPT != null)
                                        {
                                            dr[dsCCDA.HealthStatus.CPTCodeColumn.ColumnName] = objCPT.CPTCode;
                                            dr[dsCCDA.HealthStatus.CPTDescriptionColumn.ColumnName] = objCPT.CPTCodeDescription;
                                            dr[dsCCDA.HealthStatus.SNOMEDIDColumn.ColumnName] = objCPT.SNOMEDID;
                                            dr[dsCCDA.HealthStatus.SNOMEDDescriptionColumn.ColumnName] = objCPT.SNOMEDDescription;
                                        }
                                    }
                                }
                                if (observation.effectiveTime != null && observation.effectiveTime.Items != null
                                                    && observation.effectiveTime.Items.Count() > 0 && observation.effectiveTime is IVL_TS)
                                {
                                    string lowTime, highTime;
                                    GetLowHighTime(observation.effectiveTime, out lowTime, out highTime);
                                    if (!string.IsNullOrWhiteSpace(lowTime) && lowTime.ToLower() != "unk" && lowTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.HealthStatus.StartDateColumn.ColumnName] = lowTime.ToFormatedDateTimeByFormat();
                                    }
                                    if (!string.IsNullOrWhiteSpace(highTime) && highTime.ToLower() != "unk" && highTime.ToLower() != "ni")
                                    {
                                        dr[dsCCDA.HealthStatus.EndDateColumn.ColumnName] = highTime.ToFormatedDateTimeByFormat();
                                    }
                                }
                                else if (observation.effectiveTime != null && !string.IsNullOrWhiteSpace(observation.effectiveTime.value) &&
                                observation.effectiveTime.value.ToLower() != "unk" && observation.effectiveTime.value.ToLower() != "ni")
                                {
                                    dr[dsCCDA.HealthStatus.StartDateColumn.ColumnName] = observation.effectiveTime.value.ToFormatedDateTimeByFormat();
                                }
                                if (!observation.value.IsNullOrEmpty() && observation.value.Count() > 0 && observation.value[0] is PQ)
                                {
                                    PQ value = observation.value[0] as PQ;
                                    if (!string.IsNullOrWhiteSpace(value.value))
                                    {
                                        dr[dsCCDA.HealthStatus.ValueColumn.ColumnName] = value.value;
                                        dr[dsCCDA.HealthStatus.UnitColumn.ColumnName] = value.unit;
                                    }
                                }
                                if (!observation.entryRelationship.IsNullOrEmpty() && observation.entryRelationship.Count() > 0)
                                {
                                    foreach (EntryRelationship entryRelationship in observation.entryRelationship)
                                    {
                                        if (entryRelationship != null && entryRelationship.Item != null && entryRelationship.Item is Act)
                                        {
                                            Act entryRelationshipAct = entryRelationship.Item as Act;
                                            II templateId = entryRelationshipAct.templateId[0] as II;
                                            if (templateId != null && !string.IsNullOrWhiteSpace(templateId.root) && templateId.root == "2.16.840.1.113883.10.20.22.4.122")
                                            {
                                                II IdRoot = entryRelationshipAct.id[0] as II;
                                                if (IdRoot != null && !string.IsNullOrWhiteSpace(IdRoot.root) && dr[dsCCDA.HealthStatus.RootId1Column.ColumnName] == DBNull.Value)
                                                {
                                                    dr[dsCCDA.HealthStatus.RootId1Column.ColumnName] = IdRoot.root;
                                                }
                                                else
                                                {
                                                    dr[dsCCDA.HealthStatus.RootId2Column.ColumnName] = IdRoot.root;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (dr[dsCCDA.HealthStatus.LOINCCodeColumn.ColumnName] != DBNull.Value)
                                {
                                    dr[dsCCDA.HealthStatus.PatientIdColumn.ColumnName] = patientId;
                                    dr[dsCCDA.HealthStatus.HealthStatusIdColumn.ColumnName] = i--;
                                    dr[dsCCDA.HealthStatus.ProviderIdColumn.ColumnName] = MDVUtility.ToInt64(ProviderId);
                                    if (!string.IsNullOrWhiteSpace(FacilityId))
                                        dr[dsCCDA.HealthStatus.FacilityIdColumn.ColumnName] = MDVUtility.ToInt64(FacilityId);
                                    dr[dsCCDA.HealthStatus.CreatedByColumn.ColumnName] = CreatedBy;
                                    dr[dsCCDA.HealthStatus.CreatedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.HealthStatus.IsActiveColumn.ColumnName] = true;
                                    dr[dsCCDA.HealthStatus.ModifiedByColumn.ColumnName] = ModifiedBy;
                                    dr[dsCCDA.HealthStatus.ModifiedOnColumn.ColumnName] = CreatedOn;
                                    dr[dsCCDA.HealthStatus.UserIdColumn.ColumnName] = UserId;
                                    dtHealthStatus.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Care Plan - Health Status Evaluations/Outcomes Section: " + ex.Message);
            }
        }
        #endregion Health Status Evaluations/Outcomes Section

        #endregion Care Plan

        #region Document related
        public static class DocumentSections
        {
            public static string Allergies = "2.16.840.1.113883.10.20.22.2.6.1";
            public static string Medications = "2.16.840.1.113883.10.20.22.2.1.1";
            public static string Problem = "2.16.840.1.113883.10.20.22.2.5.1";
            public static string PatientData = "2.16.840.1.113883.10.20.17.2.4";
            public static string Procedures = "2.16.840.1.113883.10.20.22.2.7.1";
            public static string Immunization = "2.16.840.1.113883.10.20.22.2.2.1";
            public static string VitalSigns = "2.16.840.1.113883.10.20.22.2.4.1";
            public static string ResultDetail = "2.16.840.1.113883.10.20.22.2.3.1";
            public static string SocialHistory = "2.16.840.1.113883.10.20.22.2.17";
            public static string PlanOfTreatment = "2.16.840.1.113883.10.20.22.2.10";
            public static string Goals = "2.16.840.1.113883.10.20.22.2.60";
            public static string Encounter = "2.16.840.1.113883.10.20.22.2.22.1";
            public static string MedicalDeviceEquipment = "2.16.840.1.113883.10.20.22.2.23";
            public static string HealthConcerns = "2.16.840.1.113883.10.20.22.2.58";
            public static string Assessment = "2.16.840.1.113883.10.20.22.2.8";
            public static string FunctionalStatus = "2.16.840.1.113883.10.20.22.2.14";
            public static string MentalStatus = "2.16.840.1.113883.10.20.22.2.56";
            public static string ReasonForReferral = "1.3.6.1.4.1.19376.1.5.3.1.3.1";
            public static string InsuranceProvider = "2.16.840.1.113883.10.20.1.9";
            public static string Intervention = "2.16.840.1.113883.10.20.21.2.3";
            public static string HealthStatus = "2.16.840.1.113883.10.20.22.2.61";
        }

        public static class DocumentErrors
        {
            public static string Allergies = "Allergies";
            public static string Medications = "Medications";
            public static string Problem = "Problem";
            public static string PatientData = "PatientData";
            public static string Procedures = "Procedures";
            public static string Immunization = "Immunization: ";
        }

        public static class DocumentRoots
        {

        }

        #endregion Document related

        #region Helper Methods

        private static string NumberToWord(int num)
        {
            if (num == 0)
                return "Zero";

            if (num < 0)
                return "Not supported";

            var words = "";
            string[] strones = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] strtens = { "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };


            int crore = 0, lakhs = 0, thousands = 0, hundreds = 0, tens = 0, single = 0;


            crore = num / 10000000; num = num - crore * 10000000;
            lakhs = num / 100000; num = num - lakhs * 100000;
            thousands = num / 1000; num = num - thousands * 1000;
            hundreds = num / 100; num = num - hundreds * 100;
            if (num > 19)
            {
                tens = num / 10; num = num - tens * 10;
            }
            single = num;


            if (crore > 0)
            {
                if (crore > 19)
                    words += NumberToWord(crore) + "Crore ";
                else
                    words += strones[crore - 1] + " Crore ";
            }

            if (lakhs > 0)
            {
                if (lakhs > 19)
                    words += NumberToWord(lakhs) + "Lakh ";
                else
                    words += strones[lakhs - 1] + " Lakh ";
            }

            if (thousands > 0)
            {
                if (thousands > 19)
                    words += NumberToWord(thousands) + "Thousand ";
                else
                    words += strones[thousands - 1] + " Thousand ";
            }

            if (hundreds > 0)
                words += strones[hundreds - 1] + " Hundred ";

            if (tens > 0)
                words += strtens[tens - 2] + " ";

            if (single > 0)
                words += strones[single - 1] + " ";

            return words.ToLower();
        }

        private static void GetLowHighTime(IVL_TS effectiveTime, out string lowTime, out string highTime)
        {
            lowTime = highTime = "";

            if (effectiveTime.ItemsElementName != null && effectiveTime.Items != null)
            {
                ItemsChoiceType2[] timeName = effectiveTime.ItemsElementName;
                QTY[] timeValues = effectiveTime.Items;

                for (int i = 0; i < 2; i++)
                {
                    if (timeName.Length > i && timeValues.Length > i && timeValues[i] != null && timeValues[i] is IVXB_TS &&
                        String.IsNullOrWhiteSpace((timeValues[i] as IVXB_TS).nullFlavor))
                    {
                        IVXB_TS timeVal = timeValues[i] as IVXB_TS;

                        if (timeName[i] == ItemsChoiceType2.low)
                        {
                            lowTime = timeVal.value != "" ? timeVal.value : "";
                        }

                        if (timeName[i] == ItemsChoiceType2.high)
                        {
                            highTime = timeVal.value != "" ? timeVal.value : "";
                        }
                    }
                }
            }
        }

        private static string GetCodeType(string CodeSystem)
        {
            switch (CodeSystem)
            {
                case "2.16.840.1.113883.6.1":
                    return "LOINC";

                case "2.16.840.1.113883.6.12":
                    return "CPT";

                case "2.16.840.1.113883.6.96":
                    return "SNOMED-CT";

                case "2.16.840.1.113883.6.103":
                    return "ICD-9-CM";

                case "2.16.840.1.113883.6.90":
                    return "ICD-10-CM";

                case "2.16.840.1.113883.6.285":
                    return "HCPCS";

                default:
                    return "";
            }
        }

        private ClinicalDocument GetDeserializeDocument(byte[] byteArray, ref List<string> errors)
        {
            ClinicalDocument document = null;
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(ClinicalDocument));
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(byteArray, 0, byteArray.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                document = (ClinicalDocument)mySerializer.Deserialize(memStream);
                memStream.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return document;
        }

        #endregion Helper Methods

    }
}