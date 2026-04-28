using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Xsl;
using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.Patient.Demographics;
using MDVision.IEHR.Controls.Patient.Insurance;
using MDVision.IEHR.EMR.HL7Folder.Summary;
using MDVision.IEHR.EMR.HL7Folder.Summary.CCDA;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Model.Patient;
using Newtonsoft.Json;
using MDVision.Model.Clinical.Medical.ProblemLists;

namespace MDVision.IEHR.Controls.Batch
{
    public class Batch_ImportCCDA
    {
        private BLLPatient _bllPatientObj = null;
        private BLLClinical _bllClinicalObj = null;
        private BLLCCDA _bllCCDA = null;
        public Batch_ImportCCDA()
        {
            _bllClinicalObj = new BLLClinical();
            _bllPatientObj = new BLLPatient();
            _bllCCDA = new BLLCCDA();
        }

        #region Singleton
        private static Batch_ImportCCDA _obj = null;
        public static Batch_ImportCCDA Instance()
        {
            if (_obj == null)
                _obj = new Batch_ImportCCDA();
            return _obj;
        }

        #endregion

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            string privilegesMessage = string.Empty;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            switch (cammandAction)
            {
                #region Catagory3

                case "ImportCCDA":
                    {
                        //var fieldsJson = context.Request["BatchClinicalQualityMeasureData"];
                        //var strJsonData = LoadCqm(fieldsJson);
                        //context.Response.ContentType = "text/plain";
                        //context.Response.Write(strJsonData);
                    }
                    break;


                #endregion

                #region Old Cases
                case "LOAD_PATIENT_COMPONENT":
                    var patient_Id = context.Request["PatientId"];
                    // if (patient_Id == null) throw new ArgumentNullException(nameof(patient_Id));
                    string strJsonData = LoadPatientComponents(MDVUtility.ToLong(patient_Id));
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(strJsonData);
                    break;
                case "CHECK_PATIENT":
                    var patientModel = ser.Deserialize<PatientModel>(MDVUtility.ToStr(context.Request["PatientInfo"]));
                    string strJson = CcdaPatientsCheckExist(patientModel.LastName, patientModel.FirstName, MDVUtility.ToDateTime(patientModel.DOB), patientModel.Sex);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(strJson);
                    break;
                case "INSERT_COMPONENTS":
                    var patientData = ser.Deserialize<Dictionary<string, string>>(MDVUtility.ToStr(context.Request["PatientData"]));
                    if (patientData != null)
                    {
                        var patientId = MDVUtility.ToInt64(patientData["PatientId"]);
                        var providerId = MDVUtility.ToInt64(patientData["ProviderId"]);
                        var facilityId = MDVUtility.ToInt64(patientData["FacilityId"]);
                        var fileName = MDVUtility.ToStr(patientData["FileName"]);

                        var lstMedication = ser.Deserialize<List<MedicationModel>>(MDVUtility.ToStr(patientData["lstMedication"]));
                        var lstProblems = ser.Deserialize<List<ProblemListModel>>(MDVUtility.ToStr(patientData["lstProblems"]));
                        var lstAllergies = ser.Deserialize<List<AllergyModel>>(MDVUtility.ToStr(patientData["lstAllergies"]));

                        CcdaInsertPatientsData(patientId, lstMedication, lstProblems, lstAllergies);
                        var Result = new { status = true };
                        if (Result.status == true)
                        {
                            bool IsMedication = false;
                            bool IsAllergy = false;
                            bool IsProblemList = false;
                            IsMedication = lstMedication.Count > 0 ? true : false;
                            IsAllergy = lstAllergies.Count > 0 ? true : false;
                            IsProblemList = lstProblems.Count > 0 ? true : false;

                            insertMUSetting(patientId, providerId, facilityId,0, IsMedication, IsProblemList, IsAllergy,false);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(JsonConvert.SerializeObject(Result));
                    }
                    break;
                case "CREATE_DBAUDITNEWPATIENT":
                    var data = ser.Deserialize<Dictionary<string, string>>(MDVUtility.ToStr(context.Request["PatientIdAndFileName"]));
                    string result = InsertDbAuditNewPatient(MDVUtility.ToInt64(data["PatientId"]), data["FileName"]);
                    context.Response.ContentType = "text/plain";
                    var Output = new
                    {
                        status = true
                    };
                    context.Response.Write(JsonConvert.SerializeObject(Output));
                    break;
                case "CREATE_PATIENT":
                    PatientDemographicQuickModel modelDemographicQuick = ser.Deserialize<PatientDemographicQuickModel>(MDVUtility.ToStr(context.Request["PatientInfo"]));
                    string response = null;
                    response = SaveDemographicQuick(modelDemographicQuick);
                    // ResponseList.Add(MDVisionConstants.ResponseModel, response);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(JsonConvert.SerializeObject(response));
                    break;

                case "XML_TO_HTML":
                    var folderPath = HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);
                    var xmlData = MDVUtility.ToStr(context.Request["XMLContent"]);
                    var fileType = MDVUtility.ToStr(context.Request["FileType"]);
                    bool IsFile = MDVUtility.ToBool(context.Request["IsFile"]);
                    List<string> error = new List<string>();
                    DSCCDA dsCCDAReconcile = new DSCCDA();
                    DSPatient ds = null;
                    if (fileType != "C32" && fileType != "CCR")
                    {
                         ds = new ClinicalSummary_ImportCCDA().ReconcileCCDA(xmlData, ref dsCCDAReconcile, new List<string>(), ref error, fileType, IsFile);// 
                    }
                    string html = GetHtmlFromXMLContent(xmlData, fileType);
                    var htmlfileName = string.Format("{0}.html", Guid.NewGuid());
                    var htmlFilePath = string.Format(@"{0}\{1}", folderPath, htmlfileName);

                    //Save XML Content
                    File.WriteAllText(htmlFilePath, html);

                    var message = string.Empty;
                    if (error.Count > 0)
                    {
                        message = error.Aggregate(message, (current, e) => string.Concat(current, e, "<br/>"));
                    }
                    if (MDVSession.Current.ImagePath != null)
                    {
                        var output = new
                        {
                            status = true,
                            data = MDVSession.Current.ImagePath.Replace("~/", string.Empty) + "/" + htmlfileName,
                            PatientInfo = ds != null ? MDVUtility.JSON_DataTable(ds.Tables[ds.Patients.TableName]) : null,
                            AllergiesInfo = dsCCDAReconcile != null ? MDVUtility.JSON_DataTable(dsCCDAReconcile.Tables[dsCCDAReconcile.Allergy.TableName]) : null,
                            ProblemsInfo = dsCCDAReconcile != null ? MDVUtility.JSON_DataTable(dsCCDAReconcile.Tables[dsCCDAReconcile.ProblemList.TableName]) : null,
                            MedicationsInfo = dsCCDAReconcile != null ? MDVUtility.JSON_DataTable(dsCCDAReconcile.Tables[dsCCDAReconcile.Medication.TableName]) : null,
                            error = message
                        };

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(JsonConvert.SerializeObject(output));
                    }
                    break;
                case "PARSE_XML_COMPONENT":
                    var xmlDataContent = MDVUtility.ToStr(context.Request["XMLContent"]);
                    var patientID = MDVUtility.ToInt64(context.Request["PatientId"]);
                    bool IsFileComponent = MDVUtility.ToBool(context.Request["IsFile"]);
                    var includedComponents = ser.Deserialize<Dictionary<string, string>>(MDVUtility.ToStr(context.Request["IncludedComponents"]));
                    List<string> components = new List<string>();
                    if (Convert.ToBoolean(includedComponents["Medication"]))
                    {
                        components.Add(CCDAReconcile.DocumentSections.Medications);
                    }
                    if (Convert.ToBoolean(includedComponents["Allergies"]))
                    {
                        components.Add(CCDAReconcile.DocumentSections.Allergies);
                    }
                    if (Convert.ToBoolean(includedComponents["Problems"]))
                    {
                        components.Add(CCDAReconcile.DocumentSections.Problem);
                    }
                    List<string> errorInComponents = new List<string>();

                    DSCCDA dsCCDA = new DSCCDA();
                    DSPatient dsPatient = new ClinicalSummary_ImportCCDA().ReconcileCCDAReconcilation(xmlDataContent, ref dsCCDA, components, ref errorInComponents, IsFileComponent, patientID);
                    DSAllergies dsAllergies = new DSAllergies();
                    var allergy = "[]";
                    if (dsCCDA.Tables[dsAllergies.Allergy.TableName] != null && dsCCDA.Tables[dsAllergies.Allergy.TableName].Rows.Count > 0)
                    {
                        allergy = MDVUtility.JSON_DataTable(dsCCDA.Tables[dsAllergies.Allergy.TableName]);
                    }

                    DSProblemLists dsProblemLists = new DSProblemLists();
                    var problems = "[]";
                    if (dsCCDA.Tables[dsProblemLists.ProblemList.TableName] != null && dsCCDA.Tables[dsProblemLists.ProblemList.TableName].Rows.Count > 0)
                    {
                        problems = MDVUtility.JSON_DataTable(dsCCDA.Tables[dsProblemLists.ProblemList.TableName]);
                    }

                    DSClinicalMedication dsMedication = new DSClinicalMedication();
                    var medication = "[]";
                    if (dsCCDA.Tables[dsMedication.Medication.TableName] != null && dsCCDA.Tables[dsMedication.Medication.TableName].Rows.Count > 0)
                    {
                        medication = MDVUtility.JSON_DataTable(dsCCDA.Tables[dsMedication.Medication.TableName]);
                    }
                    var messageComponents = string.Empty;
                    if (errorInComponents.Count > 0)
                    {
                        messageComponents = errorInComponents.Aggregate(messageComponents, (current, e) => string.Concat(current, e, "<br/>"));
                    }
                    var outputPatient = new
                    {
                        status = true,
                        PatientInfo = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]),
                        Allergy = allergy,
                        Problems = problems,
                        Medication = medication,
                        error = messageComponents
                    };

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(JsonConvert.SerializeObject(outputPatient));
                    break;
                case "IMPORT_PRIVACY_FILE":
                    privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ImportCCDA", "ADD")).ToString();
                    if (string.IsNullOrEmpty(privilegesMessage))
                    {
                        var privacyXmlData = MDVUtility.ToStr(context.Request["XMLContent"]);
                        var privacyXmFileName = MDVUtility.ToStr(context.Request["FileName"]);
                        List<string> privacyErrors = new List<string>();
                        strJsonData = new ClinicalSummary_ImportCCDA().ReconcilePrivacyData(privacyXmlData, privacyXmFileName, ref privacyErrors);
                    }
                    else
                    {
                        var responseObj = new
                        {
                            status = false,
                            message = privilegesMessage
                        };
                        strJsonData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                    }
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(strJsonData);
                    break;
                case "SEARCH_PRIVACY_SEGMENTED_DOCUMENT":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ImportCCDA", "VIEW")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            long Id = MDVUtility.ToInt32(context.Request["Id"]);
                            int PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                            int RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                            strJSONData = new ClinicalSummary_ImportCCDA().LoadPrivacySegmentedDocument(Id, MDVUtility.ToInt64(MDVSession.Current.EntityId), PageNumber, RowsPerPage);
                        }
                        else
                        {
                            var responseObj = new
                            {
                                status = false,
                                Message = privilegesMessage
                            };
                            strJSONData = Newtonsoft.Json.JsonConvert.SerializeObject(responseObj);
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "PRIVACY_SEGMENTED_DOCUMENT_HTML":
                    folderPath = HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);
                    var ConfidentialityCode = MDVUtility.ToStr(context.Request["ConfidentialityCode"]);
                    var Path = MDVUtility.ToStr(context.Request["Path"]);
                    string PrivacySegmentedDocumentXML = new ClinicalSummary_ImportCCDA().GetPrivacySegmentedDocumentHTML(Path, ConfidentialityCode);
                    if (PrivacySegmentedDocumentXML != "")
                    {
                        html = GetHtmlFromXMLContent(PrivacySegmentedDocumentXML);
                        htmlfileName = string.Format("{0}.html", Guid.NewGuid());
                        htmlFilePath = string.Format(@"{0}\{1}", folderPath, htmlfileName);

                        //Save XML Content
                        File.WriteAllText(htmlFilePath, html);

                        if (MDVSession.Current.ImagePath != null)
                        {
                            var output = new
                            {
                                status = true,
                                data = MDVSession.Current.ImagePath.Replace("~/", string.Empty) + "/" + htmlfileName,
                            };
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(JsonConvert.SerializeObject(output));
                        }
                    }
                    else
                    {
                        var output = new
                        {
                            status = false,
                            data = "",
                            message = "You are not authorized to view this file."
                        };
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(JsonConvert.SerializeObject(output));
                    }
                    break;

                #endregion

                #region Cypress 2017

                // MK
                case "CYPRESS_XML_TO_HTML":
                    var serverfolderPath = HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(context.Request.Params[2]);   ////////its reading single file but throughs exception with more than one file

                    string XMLs = context.Request.Params[2];
                    var arrXML = System.Text.RegularExpressions.Regex.Split(XMLs, "\n,");
                    List<string> myCollection = new List<string>();

                    if (arrXML.Length > 0)
                    {
                        foreach (var item in arrXML)
                        {
                            myCollection.Add(item);
                        }
                    }

                    var xmlContentToDisplay = MDVUtility.ToStr(arrXML[0]);
                    var xmlContent = myCollection; // MDVUtility.ToStr(context.Request["XMLContent"]);
                    List<string> err = new List<string>();
                    ImportCCDA ImportCCDA = new ImportCCDA();
                    DSPatient dsPatients = ImportCCDA.ReconcileCcda(xmlContent, new List<string>(), err);

                    #region PatientDemoInsert

                    //dsPatients.Tables[dsPatients.Patients.TableName].Rows[0][dsPatients.Patients.RecordCountColumn.ColumnName] = 15;
                    //BLObject<DSPatient> obj = _bllCCDA.LoadPatient(dsPatients);
                    //if (obj.Data.Patients.Rows.Count <= 0)
                    //{
                    //    dsPatients.Tables[dsPatients.Patients.TableName].Rows[0][dsPatients.Patients.CreatedByColumn.ColumnName] = "Cypress";
                    //    dsPatients.Tables[dsPatients.Patients.TableName].Rows[0][dsPatients.Patients.CreatedOnColumn.ColumnName] = DateTime.Now;
                    //    dsPatients.Tables[dsPatients.Patients.TableName].Rows[0][dsPatients.Patients.ModifiedByColumn.ColumnName] = "Cypress";
                    //    dsPatients.Tables[dsPatients.Patients.TableName].Rows[0][dsPatients.Patients.ModifiedOnColumn.ColumnName] = DateTime.Now;
                    //    dsPatients.Tables[dsPatients.Patients.TableName].Rows[0][dsPatients.Patients.CommunicatewithGuarantorColumn.ColumnName] = false;
                    //    dsPatients.Tables[dsPatients.Patients.TableName].Rows[0][dsPatients.Patients.CommunicationOptoutColumn.ColumnName] = false;
                    //    BLObject<DSPatient> obj1 = _bllPatientObj.InsertPatient(dsPatients);
                    //}
                    //else
                    //{
                    //    obj.Data.Tables[dsPatients.Patients.TableName].Rows[0][dsPatients.Patients.ModifiedByColumn.ColumnName] = "Cypress";
                    //    obj.Data.Tables[dsPatients.Patients.TableName].Rows[0][dsPatients.Patients.ModifiedOnColumn.ColumnName] = DateTime.Now;
                    //    _bllPatientObj.UpdatePatient(obj.Data);

                    //}

                    #endregion

                    #region Response

                    //var fileName = MDVUtility.ToStr(context.Request["FileName"]);
                    string htmlContent = GetHtmlFromXMLContent(xmlContentToDisplay);
                    var htmlFileName = string.Format("{0}.html", Guid.NewGuid());//var htmlFileName = Guid.NewGuid().ToString() + ".html"; //var htmlFileName = "{Guid.NewGuid()}.html";
                    var folderPath1 = HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);
                    var htmlFilePth = string.Format(@"{0}\{1}", folderPath1, htmlFileName);//var htmlFilePth = @"{serverfolderPath}\{htmlFileName}";
                    File.WriteAllText(htmlFilePth, htmlContent);

                    var msg = string.Empty;
                    if (err.Count > 0)
                    {
                        msg = err.Aggregate(msg, (current, e) => string.Concat(current, e, "<br/>"));
                    }

                    if (MDVSession.Current.ImagePath != null)
                    {
                        var otpt = new
                        {
                            status = true,
                            data = MDVSession.Current.ImagePath.Replace("~/", string.Empty) + "/" + htmlFileName,
                            PatientInfo = MDVUtility.JSON_DataTable(dsPatients.Tables[dsPatients.Patients.TableName]),
                            error = msg,
                            //FileName = fileName
                        };
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(JsonConvert.SerializeObject(otpt));
                    }

                    #endregion

                    break;

                case "XML_TO_IMPORT":
                    try
                    {
                        var folderPathImport = HttpContext.Current.Server.MapPath(MDVSession.Current.ImagePath);
                        var xmlDataImport = MDVUtility.ToStr(context.Request["XMLContent"]);
                        var fileTypeCDDA = MDVUtility.ToStr(context.Request["FileType"]);
                        bool IsFileImport = MDVUtility.ToBool(context.Request["IsFile"]);
                        long ProviderId = MDVUtility.ToInt64(context.Request["ProviderId"]);
                        long FacilityId = MDVUtility.ToInt64(context.Request["FacilityId"]);
                        
                        List<string> errorImport = new List<string>();
                        string strResult = new ClinicalSummary_ImportCCDA().ReconcileCCDAData(xmlDataImport, new List<string>(), ref errorImport, fileTypeCDDA, IsFileImport,0, ProviderId, FacilityId);
                        if (strResult == "Data Imported successfully.")
                        {
                            var otpt = new
                            {
                                status = true,
                                Message = strResult,
                            };
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(JsonConvert.SerializeObject(otpt));
                        }
                        else
                        {
                            var otpt = new
                            {
                                status = false,
                                Message = strResult,
                            };
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(JsonConvert.SerializeObject(otpt));
                        }
                    }
                    catch (Exception ex)
                    {
                        var otpt = new
                        {
                            status = false,
                            Message = ex.Message,
                        };
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(JsonConvert.SerializeObject(otpt));
                    }
                    break;

                    #endregion
            }
        }

        private string GetHtmlFromXMLContent(string xmlData, string fileType = "CCDA")
        {
            xmlData = xmlData.Replace("0x3c", "&lt;").Replace("0x3e","&gt;");
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
            string stylesheetPath = "";
            if (fileType == "C32")
            {
                stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\C32.xsl";
            }
            else if (fileType == "CCR")
            {
                stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\CCR.xsl";
            }
            else
            {
                stylesheetPath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\CCDA.xsl";
            }


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

        private void CcdaInsertPatientsData(long patientId, List<MedicationModel> lstMedication, List<ProblemListModel> lstProblems, List<AllergyModel> lstAllergies)
        {
            try
            {
                Int64 ProviderId = MDVUtility.ToInt64(ConfigurationManager.AppSettings["DefaultProviderId"]);
                DSCCDA dsCCDAReconcilation = new DSCCDA();

                #region Medication CCDA
                //var dsmedication = new DSClinicalMedication();
                //int id = 0;
                foreach (var medication in lstMedication)
                {
                    try
                    {
                        var dr = dsCCDAReconcilation.Medication.NewMedicationRow();
                        if (medication.MedicationID < 1)
                        {
                            dr.MedicationId = medication.MedicationID;
                            dr.PatientId = patientId;
                            dr.RxNormCode = medication.RxNormId;
                            dr.ProviderId = ProviderId;
                            dr.Action = medication.Action;
                            dr.DoseValue = medication.DoseValue;
                            dr.DoseUnit = medication.DoseUnit;
                            dr.RouteBy = medication.Routeby;
                            dr.RouteCode = medication.RouteCode;
                            dr.DoseTiming = medication.DoseTiming;
                            dr.Quantity = medication.Quantity;
                            dr.QuantityUnit = medication.QuantityUnit;
                            dr.Refill = medication.Refill;
                            dr.Substitution = medication.Substitution;
                            dr.OtherNote = medication.OtherNote;
                            dr.PatientNotes = medication.PatientNotes;
                            dr.RepeatNumber = medication.RepeatNumber;
                            dr.NegationReason = medication.NegationReason;
                            dr.NegationIndex = medication.NegationIndex;
                            if (medication.StartDate.Year > 0001)
                            {
                                dr.StartDate = medication.StartDate;
                            }
                            if (medication.StopDate.Year > 0001)
                            {
                                dr.StopDate = medication.StopDate;
                            }
                            dr.Status = medication.Status;
                            dr.IsActive = medication.IsActive;
                            dr.CreatedBy = medication.CreatedBy;
                            dr.CreatedOn = medication.CreatedOn;
                            dr.ModifiedBy = medication.ModifiedBy;
                            dr.ModifiedOn = medication.ModifiedOn;
                            dr.DrugDescription = medication.DrugDescription;
                            dr.UserId = medication.UserId;
                        }
                        else
                        {
                            dr.MedicationsId = medication.MedicationID;
                            dr.MedicationId = medication.MedicationID;
                            dr.PatientId = patientId;
                            dr.IsActive = medication.IsActive;
                            if (medication.StopDate.Year > 0001)
                            {
                                dr.StopDate = medication.StopDate;
                            }
                            dr.CreatedOn = DateTime.Now;
                            dr.ModifiedOn = DateTime.Now;
                        }
                        dsCCDAReconcilation.Medication.AddMedicationRow(dr);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        // ignored
                    }
                }
                //if (lstMedication.Count > 0)
                //{
                //    //BLObject<DSClinicalMedication> objMedication = _bllClinicalObj.insertMedicationCCDA(dsmedication);
                //    //if (objMedication.Data.Tables[dsmedication.Medication.TableName].Rows.Count > 0)
                //    //{
                //    //    _bllClinicalObj.InsertImportCDS(patientId, string.Empty, 2);
                //    //}
                //}

                #endregion

                #region Allergies CCDA
                //DSAllergies dsAllergy = new DSAllergies();
                foreach (var allergy in lstAllergies)
                {
                    try
                    {
                        var dr = dsCCDAReconcilation.Allergy.NewAllergyRow();
                        if (MDVUtility.ToInt64(allergy.AllergyId) < 1)
                        {
                            dr.AllergyId = MDVUtility.ToInt64(allergy.AllergyId);
                            dr.PatientId = patientId;
                            dr.Allergen = allergy.Allergen;
                            dr.Type = allergy.Type;
                            dr.Reaction = allergy.Reaction;
                            dr.Severity = allergy.Severity;
                            dr.CreatedBy = allergy.CreatedBy;
                            if (!string.IsNullOrEmpty(allergy.OnSetDate))
                                dr.OnSetDate = MDVUtility.ToDateTime(allergy.OnSetDate);
                            else
                                dr[dsCCDAReconcilation.Allergy.OnSetDateColumn] = DBNull.Value;

                            //dr.Comments = allergy.Comments;

                            dr.IsActive = MDVUtility.ToBool(allergy.IsActive);
                            dr.ModifiedBy = allergy.ModifiedBy;
                            dr.RxnormID = allergy.RxnormID;
                            dr.RxnormIDType = allergy.RxnormIDType;
                            dr.Status = allergy.Status;
                            dr.TypeSNOMEDCode = allergy.TypeSNOMEDCode;
                        }
                        else
                        {
                            dr.AllergiesId = MDVUtility.ToInt64(allergy.AllergyId);
                            dr.AllergyId = MDVUtility.ToInt64(allergy.AllergyId);
                            dr.PatientId = patientId;
                            if (!string.IsNullOrEmpty(allergy.LastModifiedDate))
                                dr.OnSetDate = MDVUtility.ToDateTime(allergy.LastModifiedDate);
                            else
                                dr[dsCCDAReconcilation.Allergy.OnSetDateColumn] = DBNull.Value;
                            dr.IsActive = MDVUtility.ToBool(allergy.IsActive);
                        }
                        dsCCDAReconcilation.Allergy.AddAllergyRow(dr);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                //if (lstAllergies.Count > 0)
                //{
                //    BLObject<DSAllergies> objAllergies = _bllClinicalObj.insertAllergiesCCDA(dsAllergy);
                //    if (objAllergies.Data.Tables[dsAllergy.Allergy.TableName].Rows.Count > 0)
                //    {
                //        _bllClinicalObj.InsertImportCDS(patientId, string.Empty, 7);
                //    }
                //}

                #endregion

                #region ProblemList CCDA

                foreach (var problem in lstProblems)
                {
                    var drProblemList = dsCCDAReconcilation.ProblemList.NewProblemListRow();
                    if (MDVUtility.ToInt64(problem.ProblemListId) < 1)
                    {
                        drProblemList.ProblemListId = MDVUtility.ToInt64(problem.ProblemListId);
                        drProblemList.PatientId = patientId;
                        drProblemList.ChronicityLevel = problem.ChronicityLevel;
                        drProblemList.Code = problem.Code;
                        drProblemList.CodeType = problem.CodeType;
                        drProblemList.CreatedBy = problem.CreatedBy;
                        drProblemList.CreatedOn = problem.CreatedOn;
                        drProblemList.ModifiedBy = problem.ModifiedBy;
                        drProblemList.ModifiedOn = problem.ModifiedOn;
                        drProblemList.Description = problem.Description;
                        if (!string.IsNullOrEmpty(problem.StartDate))
                            drProblemList.StartDate = MDVUtility.ToDateTime(problem.StartDate);
                        else
                            drProblemList[dsCCDAReconcilation.ProblemList.StartDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(problem.EndDate))
                            drProblemList.EndDate = MDVUtility.ToDateTime(problem.EndDate);
                        else
                            drProblemList[dsCCDAReconcilation.ProblemList.EndDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(problem.ICD9))
                            drProblemList.ICD9 = problem.ICD9;
                        else
                            drProblemList[dsCCDAReconcilation.ProblemList.ICD9Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(problem.ICD10))
                            drProblemList.ICD10 = problem.ICD10;
                        else
                            drProblemList[dsCCDAReconcilation.ProblemList.ICD10Column] = DBNull.Value;
                        if (!string.IsNullOrEmpty(problem.ICD9_Description))
                            drProblemList.ICD9_Description = problem.ICD9_Description;
                        else
                            drProblemList[dsCCDAReconcilation.ProblemList.ICD9_DescriptionColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(problem.ICD10_Description))
                            drProblemList.ICD10_Description = problem.ICD10_Description;
                        else
                            drProblemList[dsCCDAReconcilation.ProblemList.ICD10_DescriptionColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(problem.SNOMEDID))
                            drProblemList.SNOMEDID = problem.SNOMEDID;
                        else
                            drProblemList[dsCCDAReconcilation.ProblemList.SNOMEDIDColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(problem.SNOMED_DESCRIPTION))
                            drProblemList.SNOMED_DESCRIPTION = problem.SNOMED_DESCRIPTION;
                        else
                            drProblemList[dsCCDAReconcilation.ProblemList.SNOMED_DESCRIPTIONColumn] = DBNull.Value;

                        drProblemList.FacilityId = problem.FacilityId;
                        drProblemList.NegationIndex = MDVUtility.ToBool(problem.NegationIndex);
                        drProblemList.NegationReason = problem.NegationReason;
                        drProblemList.ProblemName = problem.ProblemName;
                        drProblemList.ProviderId = MDVUtility.ToInt64(problem.ProviderId);
                        drProblemList.Severity = problem.Severity;
                        drProblemList.Status = problem.Status;
                        drProblemList.IsActive = MDVUtility.ToBool(problem.IsActive);
                        drProblemList.UserId = MDVUtility.ToInt64(problem.UserId);
                    }
                    else
                    {
                        drProblemList.ProblemId = MDVUtility.ToInt64(problem.ProblemListId);
                        drProblemList.ProblemListId = MDVUtility.ToInt64(problem.ProblemListId);
                        drProblemList.PatientId = patientId;
                        if (!string.IsNullOrEmpty(problem.EndDate))
                            drProblemList.EndDate = MDVUtility.ToDateTime(problem.EndDate);
                        else
                            drProblemList[dsCCDAReconcilation.ProblemList.EndDateColumn] = DBNull.Value;
                        drProblemList.IsActive = MDVUtility.ToBool(problem.IsActive);
                        drProblemList.CreatedOn = DateTime.Now;
                        drProblemList.ModifiedOn = DateTime.Now;
                    }
                    dsCCDAReconcilation.ProblemList.AddProblemListRow(drProblemList);
                }
                //if (lstProblems.Count > 0)
                //{
                //    BLObject<DSProblemLists> objProblem = _bllClinicalObj.insertProblemListsCCDA(dsProblemList);
                //    if (objProblem.Data.Tables[dsProblemList.ProblemList.TableName].Rows.Count > 0)
                //    {
                //        _bllClinicalObj.InsertImportCDS(patientId, string.Empty, 6);
                //    }
                //}

                #endregion

                #region Database Insert
                BLLCCDA _bllCCDAobj = new BLLCCDA();
                string strResult = _bllCCDAobj.ImportCCDAData(dsCCDAReconcilation);

                #endregion Database Insert
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void insertMUSetting(long patientId, long providerId, long facilityId, long noteId, bool lstMedication, bool lstProblems, bool lstAllergies, bool IsPatientEducation)
        {
            try
            {
                
                BLObject<DSCCDA> dsCCDAReconcilation = new BLObject<DSCCDA>();
              
                #region Database Insert
                BLLCCDA _bllCCDAobj = new BLLCCDA();
                dsCCDAReconcilation = _bllCCDAobj.InsertMUSetting(patientId, providerId, facilityId, noteId, lstMedication, lstProblems, lstAllergies, IsPatientEducation, false, 0, 0, false);

                #endregion Database Insert
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SaveDemographicQuick(PatientDemographicQuickModel patientInfo)
        {
            try
            {
                DSPatient dsPatient = new DSPatient();
                DSPatient.PatientsRow dr = dsPatient.Patients.NewPatientsRow();

                dr.LastName = patientInfo.LastName;
                dr.MotherMaidenName = patientInfo.MotherMaidenName;
                dr.FirstName = patientInfo.FirstName;
                if (!string.IsNullOrEmpty(patientInfo.MI))
                    dr.MI = patientInfo.MI;
                if (!string.IsNullOrEmpty(patientInfo.DOB))
                    dr.DOB = MDVUtility.ToDateTime(patientInfo.DOB);
                if (!string.IsNullOrEmpty(patientInfo.Gender))
                {
                    dr.Gender = patientInfo.Gender;
                }
                if (!string.IsNullOrEmpty(patientInfo.HomePhoneNo))
                {
                    dr.HomePhoneNo = patientInfo.HomePhoneNo;
                }
                if (!string.IsNullOrEmpty(patientInfo.WorkPhoneNo))
                {
                    dr.WorkPhoneNo = patientInfo.WorkPhoneNo;
                }
                if (!string.IsNullOrEmpty(patientInfo.MaritalStatus))
                    dr.MaritialStatus = MDVUtility.ToStr(patientInfo.MaritalStatus);
                if (!string.IsNullOrEmpty(patientInfo.Ethnicity))
                    dr.EthnicityId = MDVUtility.ToInt(patientInfo.Ethnicity);
                if (!string.IsNullOrEmpty(patientInfo.strEthnicityIds))
                    dr.strEthnicityIds = patientInfo.strEthnicityIds;
                if (!string.IsNullOrEmpty(patientInfo.Race))
                    dr.RaceId = MDVUtility.ToInt(patientInfo.Race);
                if (!string.IsNullOrEmpty(patientInfo.strRaceIds))
                    dr.strRaceIds = patientInfo.strRaceIds;
                if (!string.IsNullOrEmpty(patientInfo.PrefLanguage))
                    dr.PrefLanguageId = MDVUtility.ToInt(patientInfo.PrefLanguage);
                if (!string.IsNullOrEmpty(patientInfo.ConfidentialityCode))
                    dr.ConfidentialityCode = patientInfo.ConfidentialityCode;
                dr.Address1 = patientInfo.Address1;
                dr.City = patientInfo.city;
                dr.State = patientInfo.State;
                dr.ZIPCode = patientInfo.Zip;
                dr.ZIPCodeExt = patientInfo.ZipExt;

                //Start 19-10-2016 Humaira Yousaf for null values
                if (!string.IsNullOrEmpty(patientInfo.ProviderID) && !string.IsNullOrEmpty(patientInfo.Provider))
                    dr.ProviderId = MDVUtility.ToInt64(patientInfo.ProviderID);
                else
                    dr.ProviderId = MDVUtility.ToInt64(ConfigurationManager.AppSettings["DefaultProviderId"]);
                if (!string.IsNullOrEmpty(patientInfo.FacilityID) && !string.IsNullOrEmpty(patientInfo.Facility))
                    dr.FacilityId = MDVUtility.ToInt64(patientInfo.FacilityID);
                else
                    dr.FacilityId = MDVUtility.ToInt64(ConfigurationManager.AppSettings["DefaultFacilityId"]);
                if (!string.IsNullOrEmpty(patientInfo.PracticeID) && !string.IsNullOrEmpty(patientInfo.Practice))
                    dr.PracticeId = MDVUtility.ToInt64(patientInfo.PracticeID);
                else
                    dr.PracticeId = MDVUtility.ToInt64(ConfigurationManager.AppSettings["DefaultPracticeId"]);
                //End 19-10-2016 Humaira Yousaf for null values

                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                //dr.IsActive = true;
                dr.IsActive = 1;
                dr.BadAddress = false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.Comments = patientInfo.Comments;
                dr.PatientPortalStatus = "0";
                #region Database Insertion
                dsPatient.Patients.AddPatientsRow(dr);
                //BLObject<DSPatient> obj = _bllClinicalObj.InsertPatient(dsPatient);
                BLObject<DSPatient> obj = _bllPatientObj.InsertPatient(dsPatient);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = AppPrivileges.Save_Message,
                        PatientId = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PatientIdColumn.ColumnName],
                        AccountNumber = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.AccountNumberColumn.ColumnName]
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
                    return JsonConvert.SerializeObject(response);
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        private string InsertDbAuditNewPatient(long patientId, string fileName)
        {
            DSClinicalSummary ds = new DSClinicalSummary();
            try
            {
                #region Database Insertion
                BLObject<DSClinicalSummary> obj = _bllClinicalObj.InsertImportCDS(patientId, fileName, 1);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = AppPrivileges.Save_Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var responseRcopiaerror = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(responseRcopiaerror));
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
                return (JsonConvert.SerializeObject(response));
            }
        }

        private string LoadPatientComponents(long patientId)
        {
            DSAllergies dsAllergies = new DSAllergies();
            BLObject<DSAllergies> objAllergies = _bllClinicalObj.loadAllergies_Obsolete(0, MDVUtility.ToLong(patientId), 0, "0", "1");
            if (objAllergies.Data != null)
            {
                dsAllergies.Merge(objAllergies.Data);
            }
            DSProblemLists dsProblem = new DSProblemLists();
            BLObject<DSProblemLists> objProblem = _bllClinicalObj.LoadProblemLists(0, MDVUtility.ToLong(patientId), 0, "0", "1");
            if (objProblem.Data != null)
            {
                dsProblem.Merge(objProblem.Data);
            }
            DSClinicalMedication dsMedication = new DSClinicalMedication();
            BLObject<DSClinicalMedication> objMedication = _bllClinicalObj.loadMedications(0, MDVUtility.ToLong(patientId), 0, true, 1, 2000);
            if (objMedication.Data != null)
            {
                dsMedication.Merge(objMedication.Data);
            }
            var response = new
            {
                status = true,
                AllergiesLoad_JSON = MDVUtility.JSON_DataTable(dsAllergies.Tables[dsAllergies.Allergy.TableName]),
                ProblemLoad_JSON = MDVUtility.JSON_DataTable(dsProblem.Tables[dsProblem.ProblemList.TableName]),
                MedicationLoad_JSON = MDVUtility.JSON_DataTable(dsMedication.Tables[dsMedication.Medication.TableName])

            };
            return (JsonConvert.SerializeObject(response));
        }

        private string CcdaPatientsCheckExist(string lastName, string firstName, DateTime dob, string gender)
        {
            Int64 patientId = _bllClinicalObj.CCDAPatientsCheckExist(lastName, firstName, dob, gender);
            var response = new
            {
                status = true,
                PatientId = patientId
            };
            return (JsonConvert.SerializeObject(response));
        }

        #endregion
    }
}