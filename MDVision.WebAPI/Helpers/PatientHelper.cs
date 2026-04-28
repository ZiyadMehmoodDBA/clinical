using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;

using MDVision.IEHR.Controls.Clinical;
using MDVision.IEHR.Controls.Patient;
using MDVision.IEHR.Controls.Patient.Document;
using MDVision.IEHR.Controls.Patient.Demographics;

using MDVision.Model.Native.Patient;
using MDVision.Model.Patient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using MDVision.WebAPI.Entities;
using MDVision.Model.Native;
using System.Linq;
using MDVision.Model.Clinical.Notes;
using MDVision.WebAPI.Controllers;
using MDVision.Business.AppointmentReminders;

namespace MDVision.WebAPI.Helpers
{
    public class PatientHelper
    {
        BLLPatient BLLPatientObj;
        BLLClinical BLLClinicalObj;
        BLLDocument BLLDocumentObj;
        Patient_EmergencyContact_Detail ObjPatientEmergencyContactDetail;
        Patient_Preferences objPatientPreferences;
        Patient_EmergencyContact ObjPatientEmergencyContact;
        BLLMobileApp BLLMobileAppObj;
        private string successMessage = "Your information has been received successfully!";
        public PatientHelper()
        {

            BLLPatientObj = new BLLPatient();
            BLLClinicalObj = new BLLClinical();
            BLLDocumentObj = new BLLDocument();
            ObjPatientEmergencyContactDetail = new Patient_EmergencyContact_Detail();
            objPatientPreferences = new Patient_Preferences();
            ObjPatientEmergencyContact = new Patient_EmergencyContact();
            BLLMobileAppObj = new BLLMobileApp();

        }
        public string LookupMostViewedPatientNative()
        {
            List<PatientLookupModel> PatientLookupList = new List<PatientLookupModel>();
            try
            {
                long EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                long UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);

                PatientLookupList = BLLPatientObj.LookupMostViewedPatientNative(EntityId, UserId);

                int totalRecords = PatientLookupList.Count;
                if (totalRecords > 0)
                {
                    var response = new
                    {
                        status = true,
                        patientsCount = totalRecords,
                        patientsList_JSON = PatientLookupList,
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "No Patient Found"
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


        public object LookupPatientVisitType()
        {
            List<PatientLookupModel> PatientLookupList = new List<PatientLookupModel>();
            try
            {
                IList<PatientVisitTypeModel> visitTypes = new List<PatientVisitTypeModel>();
                BLObject<DSScheduleLookups> objVisitType = new BLLSchedule().LookupPatientVisitType();

                if (objVisitType.Data.PatientVisitType.Rows.Count > 0)
                {
                    foreach (DataRow dr in objVisitType.Data.PatientVisitType.Rows)
                    {
                        var type = new PatientVisitTypeModel();
                        type.Duration = MDVUtility.ToStr(dr[objVisitType.Data.PatientVisitType.DurationColumn.ColumnName.ToString()]);
                        type.VisitType = MDVUtility.ToStr(dr[objVisitType.Data.PatientVisitType.VisitTypeColumn.ColumnName]);
                        type.VisitTypeID = MDVUtility.ToStr(dr[objVisitType.Data.PatientVisitType.VisitTypeIDColumn.ColumnName]);
                        type.PatientTypeID = MDVUtility.ToStr(dr[objVisitType.Data.PatientVisitType.PatientTypeIDColumn.ColumnName]);
                        visitTypes.Add(type);
                    }
                    var response = new
                    {
                        status = true,
                        PatientVisitType_JSON = visitTypes,
                    };
                    return response;
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Record Not Found"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return response;
            }
        }

        public string LookupPatientNative(string Searchstring, long EntityId, long UserId, string IsActive, int PageNumber)
        {
            List<PatientLookupModel> PatientLookupList = new List<PatientLookupModel>();
            try
            {
                PatientLookupList = BLLPatientObj.LookupPatientNative(Searchstring, EntityId, UserId, IsActive, PageNumber);

                int totalRecords = PatientLookupList.Count;
                if (totalRecords > 0)
                {
                    var response = new
                    {
                        status = true,
                        patientsCount = totalRecords,
                        patientsList_JSON = PatientLookupList,
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "No Patient Found"
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

        public Object GetPatientDemographics(int PatientId, string FormName)
        {
            //AddResult(4);
            List<PatientDemographicModel> PatientDemographicList = new List<PatientDemographicModel>();
            List<PatientLookupModel> RaceList = new List<PatientLookupModel>();
            List<PatientLookupModel> EthnicityList = new List<PatientLookupModel>();
            List<PatientLookupModel> MaritalStatusList = new List<PatientLookupModel>();
            List<PatientInsuranceModel> PatientInsuranceList = new List<PatientInsuranceModel>();
            List<PatientLookupModel> LanguagesList = new List<PatientLookupModel>();
            List<PatientLookupModel> RelationshipList = new List<PatientLookupModel>();
            List<PatientEmergencyContactModel> EmergencyContactList = new List<PatientEmergencyContactModel>();
            List<PatientPreferenceModel> PatientPreferenceList = new List<PatientPreferenceModel>();
            List<PatientLookupModel> RelationshipListForEmergencyContact = new List<PatientLookupModel>();
            DSPatient dsPatient = null;
            try
            {
                PatientDemographicList = BLLPatientObj.GetPatientDemographics(PatientId, FormName);
                MaritalStatusList = BLLPatientObj.LookupMaritalStatusNative();
                EthnicityList = BLLPatientObj.LookupEthnicityNative();
                RaceList = BLLPatientObj.LookupRaceNative();
                LanguagesList = BLLPatientObj.LookupLanguagesNative();
                PatientInsuranceList = BLLPatientObj.LoadPatientInsuranceNative(0, Convert.ToInt64(PatientId), 0);
                RelationshipList = BLLPatientObj.LookupRelationshipNative();
                var patientConsent = BLLPatientObj.LoadPatientConsentNative(PatientId);
                BLObject<DSPatient> obj = BLLPatientObj.loadPatientEmergencyContacts(PatientId);
                dsPatient = obj.Data;
                if (obj.Data != null)
                {
                    EmergencyContactList = MDVUtility.ConvertDataTable<PatientEmergencyContactModel>(dsPatient.Tables[dsPatient.EmergencyContactsSearch.TableName]);
                }
                var objPreference = BLLPatientObj.FillPatientById(PatientId, null);
                if (objPreference.Data != null)
                {
                    var dsProfile = objPreference.Data;
                    PatientPreferenceList = MDVUtility.ConvertDataTable<PatientPreferenceModel>(dsProfile.Tables[dsProfile.Patients.TableName]);


                }
                //  var PatientPreferenceResult = objPatientPreferences.FillPatientPreferences(PatientId);
                int totalRecords = PatientDemographicList.Count;
                if (totalRecords > 0)
                {
                    var response = new
                    {
                        status = true,
                        patientsCount = totalRecords,
                        patientDemographic_JSON = PatientDemographicList,
                        race_JSON = RaceList.Select(m => new { m.RaceId, m.RaceDescription }).ToList(),
                        ethnicity_JSON = EthnicityList.Select(m => new { m.EthnicityId, m.EthnicityDescription }).ToList(),
                        maritalStatus_JSON = MaritalStatusList.Select(m => new { m.MaritalStatusId, m.MaritalStatus }).ToList(),
                        patientInsurance_JSON = PatientInsuranceList,
                        languages_JSON = LanguagesList.Select(m => new { m.LanguagesId, m.LanguagesDescription }).ToList(),
                        relationship_JSON = RelationshipList.Select(m => new { m.RelationshipId, m.RelationshipDescription }).ToList(),
                        PatientConsent = patientConsent,
                        PatientEmergencyContact_JSON = EmergencyContactList,
                        PatientPreference_JSON = PatientPreferenceList


                    };

                    return response;
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "No Patient Demographics Found"
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return response;
            }
        }

        public string GetPatientDemographicslockUps()
        {
            try
            {
                List<PatientLookupModel> RaceList = new List<PatientLookupModel>();
                List<PatientLookupModel> EthnicityList = new List<PatientLookupModel>();
                List<PatientLookupModel> MaritalStatusList = new List<PatientLookupModel>();
                List<PatientLookupModel> LanguagesList = new List<PatientLookupModel>();
                List<PatientLookupModel> RelationshipList = new List<PatientLookupModel>();

                MaritalStatusList = BLLPatientObj.LookupMaritalStatusNative();
                EthnicityList = BLLPatientObj.LookupEthnicityNative();
                RaceList = BLLPatientObj.LookupRaceNative();
                LanguagesList = BLLPatientObj.LookupLanguagesNative();
                RelationshipList = BLLPatientObj.LookupRelationshipNative();

                var response = new
                {
                    status = true,
                    race_JSON = RaceList,
                    ethnicity_JSON = EthnicityList,
                    maritalStatus_JSON = MaritalStatusList,
                    languages_JSON = LanguagesList,
                    relationship_JSON = RelationshipList,
                };

                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    PatientConsentId = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        internal string LoadPatientConsent(long patientId)
        {
            var patientConsent = BLLPatientObj.LoadPatientConsentNative(patientId);
            if (patientConsent != null)
            {
                return JsonConvert.SerializeObject(new { status = true, data = patientConsent });
            }
            else
            {
                return JsonConvert.SerializeObject(new { status = false, message = "No record found." });
            }
        }

        internal string SavePatientConsent(JObject data)
        {
            try
            {

                PatientConsentVM model = JsonConvert.DeserializeObject<PatientConsentVM>(MDVUtility.ToStr(data["data"]));
                model.CreatedOn = DateTime.Now;
                model.ModifiedOn = DateTime.Now;
                model.CreatedBy = MDVUtility.DecryptFrom64(model.UserName);
                model.ModifiedBy = MDVUtility.DecryptFrom64(model.UserName);
                model.IsActive = true;

                UploadPDFVM docModel = JsonConvert.DeserializeObject<UploadPDFVM>(MDVUtility.ToStr(data["data"]));

                string DocumentFolder = "Kiosk Document";
                string fileType = "application/pdf";
                BLObject<DSDocument> objDocument = null;
                string message = "";
                objDocument = BLLDocumentObj.LoadDocument(0, DocumentFolder, 0, "1", null, null);

                if (docModel.PdfBase64String == null || docModel.PdfBase64String.Length == 0)
                {
                    message = "Document not found in request.";
                }
                else if (string.IsNullOrEmpty(model.PatientId))
                {
                    message = "PatientId not found in request.";
                }
                else if (objDocument.Data.Documents.Rows.Count == 0)
                {
                    message = DocumentFolder + " folder not found in system.";
                }


                if (message.Length > 0)
                {
                    var response = new
                    {
                        status = false,
                        message = message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    int docId = MDVUtility.ToInt(objDocument.Data.Documents.Rows[0][objDocument.Data.Documents.DocIdColumn.ColumnName]);


                    DSPatient dsDocument = new DSPatient();

                    DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                    dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                    dr.Documentid = docId;


                    byte[] currentFileStream = Convert.FromBase64String(docModel.PdfBase64String);


                    dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
                    dr.FileType = fileType;
                    if (docModel.Filename == null || docModel.Filename == "")
                        dr.FilePath = Guid.NewGuid().ToString() + "." + fileType.Split('/')[1];//Utility.ToStr(SearchedfieldsJSON["txtDescription"]);
                    else
                        dr.FilePath = docModel.Filename;
                    //MemoryStream ms = new MemoryStream(currentFileStream);

                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtComments"]))
                    //{
                    //    dr.Comments = System.Web.HttpUtility.UrlDecode(SearchedfieldsJSON["txtComments"]);
                    //}

                    if (fileType == "application/pdf")
                        dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
                    else
                        dr.Pages = 1;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(model.UserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(model.UserName);
                    dr.ModifiedOn = DateTime.Now;



                    dsDocument.PatientDocument.AddPatientDocumentRow(dr);


                    string PatientConsentId = BLLPatientObj.InsertUpdatePatientConsentNative(model, dsDocument);

                    if (MDVUtility.ToInt64(PatientConsentId) > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientConsentId = PatientConsentId,
                            message = "Successfully updated."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = "Could not saved.",
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    PatientConsentId = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string UploadPDF(JObject data)
        {
            UploadPDFVM model = JsonConvert.DeserializeObject<UploadPDFVM>(MDVUtility.ToStr(data["data"]));

            string DocumentFolder = "Kiosk Document";
            BLObject<DSDocument> objDocument = null;
            string message = "";
            objDocument = BLLDocumentObj.LoadDocument(0, DocumentFolder, 0, "1", null, null);

            if (model.PdfBase64String == null || model.PdfBase64String.Length == 0)
            {
                message = "Document not found in request.";
            }
            else if (string.IsNullOrEmpty(model.PatientId))
            {
                message = "PatientId not found in request.";
            }
            else if (objDocument.Data.Documents.Rows.Count == 0)
            {
                message = DocumentFolder + " not found in system.";
            }
            else
            {
                int docId = MDVUtility.ToInt(objDocument.Data.Documents.Rows[0][objDocument.Data.Documents.DocIdColumn.ColumnName]);
                string fieldsJSON = "{\"ddlFolder\":\"" + docId + "\",\"ddlAssignUserto\":\"\",\"ddlAssignUserto_text\":\"\",\"dtpDOS\":\"\",\"ddlClaim\":\"\",\"ddlCase\":\"\",\"txtComments\":\"\"}";

                return new Patient_Document().SavePatientDocument(fieldsJSON, MDVUtility.ToInt64(model.PatientId), null, model.PdfBase64String, "application/pdf", null, model.Filename);
            }

            var response = new
            {
                status = false,
                Message = message
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        }

        public string RecentPatient(int patientId, string userName, string entityId)
        {
            Dictionary<string, string> ResponseList = new Dictionary<string, string>();
            PatientModel model = new PatientModel { PatientID = MDVUtility.ToStr(patientId) };
            string response = null;
            response = Patient_Search.Instance().SaveRecentPatient(model);
            ResponseList.Add(MDVisionConstants.ResponseModel, response);

            return JsonConvert.SerializeObject(ResponseList);
        }

        public string LookupInsurancePlan(string ShortName, long EntityId)
        {
            List<InsuranceLookupModel> InsuranceLookupList = new List<InsuranceLookupModel>();
            try
            {
                InsuranceLookupList = BLLPatientObj.LookupInsurancePlan(ShortName, EntityId, "1");

                int totalRecords = InsuranceLookupList.Count;
                if (totalRecords > 0)
                {
                    var response = new
                    {
                        status = true,
                        insurancePlanCount = totalRecords,
                        insuranceList_JSON = InsuranceLookupList,
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "No Insurance Found",
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

        public string UpdatePatientDemographicsNative(JObject data)
        {


            try
            {
                SavePatientNative spn = JsonConvert.DeserializeObject<SavePatientNative>(MDVUtility.ToStr(data["data"]));
                // Below code will be run from Web application from moderator.
                if (!string.IsNullOrEmpty(spn.patientDemographic_JSON.PatientID) || !string.IsNullOrEmpty(spn.patientDemographic_JSON.DimmyPatientId))
                {
                    string filetype = string.Empty;
                    string PatientProfileImagePath = string.Empty;

                    DataChangeRequest img = (DataChangeRequest)spn.patientDemographic_JSON.DataChangeRequest.Where(p => p.columnName == "PatientImage").FirstOrDefault();
                    DataChangeRequest imgType = (DataChangeRequest)spn.patientDemographic_JSON.DataChangeRequest.Where(p => p.columnName == "ImageType").FirstOrDefault();
                    if (img != null)
                    {
                        string image = img.CurrentValueDisplay.ToString();
                        if (image.Length > 30)
                        {
                            byte[] currentFileStream = Convert.FromBase64String(image);
                            filetype = imgType.CurrentValueDisplay.ToString().Split('/')[1];
                            PatientProfileImagePath = spn.patientDemographic_JSON.LastName + '-' + spn.patientDemographic_JSON.FirstName + '-' + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            new Patient_Demographic().PatientDemographicImageSaveInFolder(image, PatientProfileImagePath, filetype);
                            spn.patientDemographic_JSON.DataChangeRequest.Remove(img);
                            spn.patientDemographic_JSON.DataChangeRequest.Add(new DataChangeRequest { columnName = "PatientProfileImagePath", CurrentValueDisplay = PatientProfileImagePath + "." + filetype });
                            spn.patientDemographic_JSON.DataChangeRequest.Add(new DataChangeRequest { columnName = "PatientProfileThumbnailPath", CurrentValueDisplay = PatientProfileImagePath + "-th." + filetype });
                        }
                        else
                        {
                            spn.patientDemographic_JSON.DataChangeRequest.Add(new DataChangeRequest { columnName = "PatientProfileImagePath", CurrentValueDisplay = "" });
                            spn.patientDemographic_JSON.DataChangeRequest.Add(new DataChangeRequest { columnName = "PatientProfileThumbnailPath", CurrentValueDisplay = "" });
                        }
                    }

                    if (spn.patientDemographic_JSON.DataChangeRequest.Count > 0)
                    {
                        List<DataChangeRequest> lstdatachangeRequest = new List<DataChangeRequest>();


                        foreach (var item in spn.patientDemographic_JSON.DataChangeRequest)
                        {
                            item.ColumnKeyId = spn.patientDemographic_JSON.PatientID;
                            item.ColumnKeyName = "PatientId";
                            item.CreatedBy = MDVUtility.DecryptFrom64(spn.UserName);
                            item.CreatedOn = DateTime.Now;
                            item.ModifiedBy = MDVUtility.DecryptFrom64(spn.UserName);
                            item.ModifiedOn = DateTime.Now;

                            item.PatientId = !string.IsNullOrEmpty(spn.patientDemographic_JSON.PatientID) ? Convert.ToInt64(spn.patientDemographic_JSON.PatientID) : (long?)null;
                            item.IsSynced = false;
                            item.EntityId = spn.EntityId;
                            item.DBTableName = "Patients";
                            item.DimmyPatientId = !string.IsNullOrEmpty(spn.patientDemographic_JSON.DimmyPatientId) ? MDVUtility.ToStr(spn.patientDemographic_JSON.DimmyPatientId) : null;

                        }
                        string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(spn.patientDemographic_JSON.DataChangeRequest);
                        if (returnVal == "")
                        {
                            var response = new
                            {
                                status = true,
                                Message = successMessage
                            };
                            return (JsonConvert.SerializeObject(response));

                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = returnVal
                            };
                            return (JsonConvert.SerializeObject(response));

                        }

                    }
                    else
                    {
                        var response1 = new
                        {
                            status = true,
                            Message = "No Change has been Made from User."
                        };
                        return (JsonConvert.SerializeObject(response1));

                    }


                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Both PatientId And DummyPatientId cannot be null"
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

        public string UpdatePatientInsuranceNative(JObject data)
        {
            PatientInsurancSave pis = JsonConvert.DeserializeObject<PatientInsurancSave>(MDVUtility.ToStr(data["data"]));
            try
            {
                List<DataChangeRequest> lstDataChangeRequest = new List<DataChangeRequest>();

                foreach (PatientInsuranceModel model in pis.patientInsurance_JSON)

                {
                    int ExistingMaximumPlanPriority = 0;
                    if (MDVUtility.ToInt64(model.InsuranceId) < 0)
                    {
                        string PatientId = !string.IsNullOrEmpty(model.PatientId) ? MDVUtility.ToStr(model.PatientId) : MDVUtility.ToStr(model.DimmyPatientId);
                        int InsuranceId = CheckExistingRecordForInsurnace(PatientId, MDVUtility.ToInt64(model.InsurancePlanId), "PatientInsurance");

                        if (InsuranceId == 0)
                        {




                            int ExistingMinimumId = CheckExistingRecord(PatientId, "PatientInsurance", "InsuranceId");

                            if (ExistingMinimumId != 0)
                            {
                                ExistingMaximumPlanPriority = GetMaxPlanPriorityDbAuditNative(PatientId);
                                //    ExistingMaximumPlanPriority = CheckPlanPriorityAgainstInsuranceId(Convert.ToInt64(model.PatientId), "PatientInsurance",MDVUtility.ToStr( ExistingMinimumId));
                                model.InsuranceId = Convert.ToString(Convert.ToInt32(model.InsuranceId) + (ExistingMinimumId));
                                ExistingMaximumPlanPriority++;
                                // check existing Minimum Plan Priority against this minimum Id

                            }
                            else
                            {
                                ExistingMaximumPlanPriority = GetMaxPlanPriorityDbAuditNative(PatientId);
                                ExistingMaximumPlanPriority++;

                            }
                        }
                        else
                        {
                            model.InsuranceId = MDVUtility.ToStr(InsuranceId);
                            ExistingMaximumPlanPriority = CheckPlanPriorityAgainstInsuranceId(PatientId, "PatientInsurance", MDVUtility.ToStr(InsuranceId));
                        }
                    }

                    foreach (var item in model.DataChangeRequest)
                    {
                        if (item.columnName == "PlanPriority")
                        {
                            if (ExistingMaximumPlanPriority != 0 && MDVUtility.ToInt64(model.InsuranceId) < 0)
                            {
                                item.CurrentValueDisplay = MDVUtility.ToStr(ExistingMaximumPlanPriority);
                            }
                        }

                        item.ColumnKeyId = model.InsuranceId;
                        item.ColumnKeyName = "InsuranceId";

                        item.CreatedBy = MDVUtility.DecryptFrom64(pis.UserName);
                        item.CreatedOn = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.CreatedBy = MDVUtility.DecryptFrom64(pis.UserName);

                        item.ModifiedBy = MDVUtility.DecryptFrom64(pis.UserName);
                        item.ModifiedOn = DateTime.Now;


                        item.PatientId = !string.IsNullOrEmpty(model.PatientId) ? Convert.ToInt64(model.PatientId) : (long?)null;
                        item.DimmyPatientId = !string.IsNullOrEmpty(model.DimmyPatientId) ? MDVUtility.ToStr(model.DimmyPatientId) : null;
                        item.IsSynced = false;
                        item.EntityId = Convert.ToInt64(pis.EntityId);

                        item.DBTableName = "PatientInsurance";




                        lstDataChangeRequest.Add(item);

                    }



                }

                string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(lstDataChangeRequest);




                if (returnVal == "")
                {
                    var response = new
                    {
                        status = true,
                        message = successMessage
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = returnVal
                    };
                    return JsonConvert.SerializeObject(response);
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
        public int CheckExistingRecord(string PatientId, string DbTableName, string ColumnKeyName)
        {

            try
            {
                int ReturnVal = 0;

                ReturnVal = BLLMobileAppObj.CheckExistingRecord(PatientId, DbTableName, ColumnKeyName);
                return ReturnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckExistingPatient(string InsuranceId, string ExpiringOn, string Street, string City, string State, string Zip, string FirstName, string LastName, string DOB, string Mobile, string Email, string gender)
        {

            try
            {
                int ReturnVal = 0;

                //ReturnVal = BLLMobileAppObj.CheckExistingPatients(InsuranceId, ExpiringOn, Street, City, State, Zip, FirstName, LastName, DOB, Mobile, Email, gender);
                return ReturnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Object SearchPatienMultiFilter(LoadMuiltPatientAddress model)
        {
            try
            {
                bool status = WebApiCommon.CheckNetwrokIP(model.NetWorkIP);
                if (status == false)
                {
                    var response = new
                    {
                        status = true,
                        Message = "Not a part of our system network.",


                    };
                    return response;
                }
                else
                {
                    int PatientId = -1;
                    if (model.PatientId == "")
                    {
                        PatientId = BLLMobileAppObj.CheckExistingPatients(model);
                    }
                    else
                    {
                        PatientId = int.Parse(model.PatientId);
                    }
                    if (PatientId != -1)
                    {
                        string FormName = "Demographics";
                        return new PatientHelper().GetPatientDemographics(PatientId, FormName);
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "patient not found"
                        };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return response;
            }

        }
        public object SearchPatienAdressMultiFilter(string NetWorkIP, string Ext, string Zip, string FirstName, string LastName, string DOB, string Gender, string AccountNo, string EntityId, string MobileNumber)
        {
            try
            {
                bool status = WebApiCommon.CheckNetwrokIP(NetWorkIP);
                if (status == false)
                {
                    var response = new
                    {
                        status = true,
                        Message = "Not a part of our system network.",
                        Patient_Address_list = new Array[0]

                    };
                    return response;
                }
                else
                {

                    List<LoadMuiltPatientAddress> obj = new BLLPatient().SearchPatienAdressMultiFilter(NetWorkIP, Ext, Zip, FirstName, LastName, DOB, Gender, AccountNo, EntityId, MobileNumber);
                    if (obj.Count > 0)
                    {
                        var list1 = obj.Select(m => new { m.PatientId, m.Address });

                        var response = new
                        {
                            status = true,
                            Message = "",
                            Patient_Address_list = list1

                        };
                        return response;
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = "No Patient Found",
                            Patient_Address_list = new Array[0]

                        };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return response;
            }

        }
        public object LoadPatientContact(string NetWorkIP, string PatientId)
        {
            try
            {
                bool status = WebApiCommon.CheckNetwrokIP(NetWorkIP);
                if (status == false)
                {
                    var response = new
                    {
                        status = true,
                        Message = "Not a part of our system network.",
                        Patient_Contact_list = new Array[0]

                    };
                    return response;
                }
                else
                {

                    List<LoadMuiltPatientAddress> obj = new BLLPatient().LoadPatientContact(NetWorkIP, PatientId);
                    if (obj.Count > 0)
                    {
                        var list1 = obj.Select(m => new { m.Contacts });

                        var response = new
                        {
                            status = true,
                            Message = "",
                            Patient_Contact_list = list1

                        };
                        return response;
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = "No Patient Found",
                            Patient_Contact_list = new Array[0]

                        };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return response;
            }

        }

        public object LoadPatientContactWebApi(string NetWorkIP, string PatientId)
        {
            try
            {
                bool status = WebApiCommon.CheckNetwrokIP(NetWorkIP);
                if (status == false)
                {
                    var response = new
                    {
                        status = true,
                        Message = "Not a part of our system network.",
                        Patient_Contact_list = new Array[0]

                    };
                    return response;
                }
                else
                {

                    List<LoadMuiltPatientAddress> obj = new BLLPatient().LoadPatientContact(NetWorkIP, PatientId);
                    if (obj.Count > 0)
                    {
                        var list1 = obj.Select(m => new { m.Contacts });
                        int code = 0;
                        Random rnd = new Random();
                        code = rnd.Next(1000, 9999);
                        string res = "";
                        for (int i = 0; i < obj.Count; i++)
                        {
                            AppointmentReminder objreminder = new AppointmentReminder();
                            res = objreminder.TwoStepVerfication(obj[i].Contacts, code.ToString(), DateTime.Now.ToString());
                        }
                        if (res != "")
                        {
                            bool VisitType = new BLLPatient().LoadPatientVisitType(PatientId);

                            var response = new
                            {
                                status = true,
                                Message = "",
                                Code = code,
                                PatientId = PatientId,
                                VisitType = VisitType
                            };
                            return response;
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                Message = res,
                                Code = 0
                            };
                            return response;
                        }

                        //var response = new
                        //{
                        //    status = true,
                        //    Message = "",
                        //    Patient_Contact_list = list1

                        //};
                        //return response;
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = "No Patient Found",
                            Patient_Contact_list = new Array[0]

                        };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return response;
            }

        }
        public object SavePatientSignature(LoadMuiltPatientAddress model)
        {
            try
            {
                bool status = WebApiCommon.CheckNetwrokIP(model.NetWorkIP);
                if (status == false)
                {
                    var response = new
                    {
                        status = true,
                        Message = "Not a part of our system network.",
                        Patient_list = new Object(),
                        Image = ""

                    };
                    return response;
                }
                else
                {
                    model.ModifiedOn = DateTime.Now.ToString();
                    List<LoadMuiltPatientAddress> obj = new BLLPatient().SavePatientSignature(model);

                    if (obj.Count > 0)
                    {
                        Patient_Demographic_MobileApp dpobj = new Patient_Demographic_MobileApp();
                        string images = dpobj.getPatientProfileImage(obj[0].PatientProfileImagePath);

                        var list1 = obj.Select(m => new { m.ScheduledTime, m.EstimatedTime, m.PatientAhead }).First();

                        var response = new
                        {
                            status = true,
                            Message = "",
                            Patient_list = list1,
                            Image = images

                        };
                        return response;
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = "No Patient Found",
                            Patient_list = new Object(),
                            Image = ""
                        };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return response;
            }

        }
        public object TwoSetVerfication(LoadMuiltPatientAddress model)
        {
            try
            {
                int code = 0;
                bool status = WebApiCommon.CheckNetwrokIP(model.NetWorkIP);
                if (status == false)
                {
                    var response = new
                    {
                        status = true,
                        Message = "Not a part of our system network.",
                        Code = code

                    };
                    return response;
                }
                else
                {
                    Random rnd = new Random();
                    code = rnd.Next(100000,999999);
                    AppointmentReminder obj = new AppointmentReminder();
                   string res= obj.TwoStepVerfication(model.MobileNumber,code.ToString(), DateTime.Now.ToString());
                    if (res != "")
                    {
                        

                        var response = new
                        {
                            status = true,
                            Message = "",
                            Code = code

                        };
                        return response;
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = res,
                            Code = 0
                        };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return response;
            }

        }
        public object LoadPatientRelations()
        {
            try
            {

                List<LoadMuiltPatientAddress> obj = new BLLPatient().LoadPatientRelations();
                if (obj.Count > 0)
                {
                    var list1 = obj.Select(m => new { m.RelationShipId, m.RelationShipName });

                    var response = new
                    {
                        status = true,
                        Message = "",
                        Patient_Relationship_list = list1

                    };
                    return response;
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = "No Relation Found",
                        Patient_Relationship_list = new Array[0]

                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return response;
            }

        }
        public object CheckNetwrok(string NetworkIp)
        {
            try
            {
                bool status =WebApiCommon.CheckNetwrokIP(NetworkIp);
                if (status == true)
                {

                    var response = new
                    {
                        status = true,
                        Message = "",

                    };
                    return response;
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Not a part of our system network.",

                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return response;
            }

        }
        public string LoadPatientByInsurance(string SubscriberId, string expiryDate)
        {
            try
            {
                string PatientId = "";
                PatientId = BLLMobileAppObj.CheckExistingPatientByInsurance(SubscriberId, expiryDate);
                if (!string.IsNullOrEmpty(PatientId))
                {
                    string FormName = "Demographics";
                    return "Code Commented";//new PatientHelper().GetPatientDemographics(int.Parse(PatientId), FormName);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "patient not found"
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message
                };
                return JsonConvert.SerializeObject(response);
            }

        }

        public int CheckPlanPriorityAgainstInsuranceId(string PatientId, string DbTableName, string InsuranceId)
        {

            try
            {
                int ReturnVal = 0;

                ReturnVal = BLLMobileAppObj.CheckPlanPriorityAgainstInsuranceId(PatientId, DbTableName, InsuranceId);
                return ReturnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxPlanPriorityDbAuditNative(string PatientId)
        {

            try
            {
                int ReturnVal = 0;

                ReturnVal = BLLMobileAppObj.GetMaxPlanPriorityFromDbAuditNative(PatientId);
                return ReturnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int CheckExistingRecordForInsurnace(string PatientId, long InsurancePlanId, string DbTableName)
        {

            try
            {
                int ReturnVal = 0;

                ReturnVal = BLLMobileAppObj.CheckExistingRecordForInsurance(PatientId, InsurancePlanId, DbTableName);
                return ReturnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SaveEmergencyContact(JObject data)
        {
            try
            {


                PatientEmergencyContactModel PECModel = JsonConvert.DeserializeObject<PatientEmergencyContactModel>(MDVUtility.ToStr(data["data"]));

                //   Int64 PatientID = MDVUtility.ToInt64(PECModel.PatientId);
                string PatientId = !string.IsNullOrEmpty(PECModel.PatientId) ? MDVUtility.ToStr(PECModel.PatientId) : MDVUtility.ToStr(PECModel.DimmyPatientId);
                //   string JSONFields = MDVUtility.ToStr( fieldsJSON);

                if (MDVUtility.ToInt64(PECModel.ContactId) < 0)
                {
                    int ExistingMinimumId = CheckExistingRecord(PatientId, "EmergencyContacts", "ContactId");

                    if (ExistingMinimumId != 0)
                    {
                        PECModel.ContactId = Convert.ToString(Convert.ToInt32(PECModel.ContactId) + (ExistingMinimumId));
                    }
                }

                string Result = "";
                if (!string.IsNullOrEmpty(PatientId))
                {
                    //List<DataChangeRequest> lstDataChangeRequest = new List<DataChangeRequest>();

                    //lstDataChangeRequest = fieldsJSON.dataChangeRequest;

                    foreach (var item in PECModel.DataChangeRequest)
                    {
                        item.ColumnKeyId = PECModel.ContactId;
                        item.ColumnKeyName = "ContactId";
                        item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.CreatedOn = DateTime.Now;
                        item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.ModifiedOn = DateTime.Now;
                        item.PatientId = !string.IsNullOrEmpty(PECModel.PatientId) ? Convert.ToInt64(PECModel.PatientId) : (long?)null;
                        item.DimmyPatientId = !string.IsNullOrEmpty(PECModel.DimmyPatientId) ? MDVUtility.ToStr(PECModel.DimmyPatientId) : null;
                        item.IsSynced = false;
                        item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                        item.DBTableName = "EmergencyContacts";
                        // string returnVal = new DALPatient(true).InsertupdateDemographicsInDBAuditNative(dbManager, item);

                        string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(item);



                        if (returnVal != "")
                        {
                            Result = Result + returnVal;
                        }
                        else
                        {

                        }


                    }

                    if (Result == "")
                    {

                        var response = new
                        {
                            status = true,
                            Message = successMessage
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);

                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Result
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }


                }

                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient Not Found"
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        public string UpdateEmergencyContact(JObject data)
        {
            try
            {



                var PECModel = JsonConvert.DeserializeObject<PatientEmergencyContactModel>(MDVUtility.ToStr(data["data"]));

                Int64 PatientID = MDVUtility.ToInt64(PECModel.PatientId);
                Int64 EmergencyContactID = MDVUtility.ToInt64(PECModel.ContactId);
                if (EmergencyContactID > 0)
                {
                    string Result = "";
                    if (PatientID > 0)
                    {
                        //List<DataChangeRequest> lstDataChangeRequest = new List<DataChangeRequest>();

                        //lstDataChangeRequest = fieldsJSON.dataChangeRequest;

                        foreach (var item in PECModel.DataChangeRequest)
                        {
                            item.ColumnKeyId = PECModel.ContactId;
                            item.ColumnKeyName = "ContactId";
                            item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            item.CreatedOn = DateTime.Now;
                            item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            item.ModifiedOn = DateTime.Now;
                            item.PatientId = PatientID;
                            item.IsSynced = false;
                            item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                            item.DBTableName = "EmergencyContacts";

                            // string returnVal = new DALPatient(true).InsertupdateDemographicsInDBAuditNative(dbManager, item);

                            string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(item);



                            if (returnVal != "")
                            {
                                Result = Result + returnVal;
                            }
                            else
                            {

                            }


                        }

                        if (Result == "")
                        {
                            //DSPatient dsPatientNew = null;
                            //BLObject<DSPatient> objLoadContacts = BLLPatientObj.loadPatientEmergencyContacts(PatientID);
                            //dsPatientNew = objLoadContacts.Data;
                            //if (objLoadContacts.Data != null)
                            //{
                            //    var response = new
                            //    {
                            //        status = true,
                            //        EmergencyContactsCount = dsPatientNew.Tables[dsPatientNew.EmergencyContactsSearch.TableName].Rows.Count > 0 ? dsPatientNew.Tables[dsPatientNew.EmergencyContactsSearch.TableName].Rows.Count : 0,
                            //        EmergencyContactsLoad_JSON = MDVUtility.JSON_DataTable(dsPatientNew.Tables[dsPatientNew.EmergencyContactsSearch.TableName]),
                            //        iTotalDisplayRecords = (dsPatientNew.EmergencyContactsSearch.Rows.Count > 0) ? dsPatientNew.EmergencyContactsSearch.Rows[0][dsPatientNew.EmergencyContactsSearch.RecordCountColumn.ColumnName] : 0,
                            //    };
                            //    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            //}
                            //else
                            //{
                            //    var response = new
                            //    {
                            //        status = false,
                            //        Message = objLoadContacts.Message
                            //    };
                            //    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            //}
                            var response = new
                            {
                                status = true,
                                Message = successMessage
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);

                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Result
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }













                        // BLLMobileAppObj.SavePatientEmergencyContactInDBAuditNative();

                        //DSPatient dsPatient = new DSPatient();
                        //DSPatient.EmergencyContactsRow dr = dsPatient.EmergencyContacts.NewEmergencyContactsRow();

                        //dr.PatientId = MDVUtility.ToInt64(PatientID);
                        //if (!string.IsNullOrEmpty(MDVUtility.ToStr(fieldsJSON["LastName"])))
                        //    dr.LastName = MDVUtility.ToStr(fieldsJSON["LastName"]);
                        //if (!string.IsNullOrEmpty(MDVUtility.ToStr(fieldsJSON["FirstName"])))
                        //    dr.FirstName = MDVUtility.ToStr(fieldsJSON["FirstName"]);
                        //if (!string.IsNullOrEmpty(MDVUtility.ToStr(fieldsJSON["MI"])))
                        //    dr.MI = MDVUtility.ToStr(fieldsJSON["MI"]);
                        //if (!string.IsNullOrEmpty(MDVUtility.ToStr(fieldsJSON["DOB"])))
                        //    dr.DOB = MDVUtility.ToDateTime(fieldsJSON["DOB"]);
                        //dr.Address1 = fieldsJSON["Address1"];
                        //dr.Address2 = fieldsJSON["Address2"];
                        //dr.City = fieldsJSON["City"];
                        //dr.State = fieldsJSON["State"];
                        //dr.Zipcode = fieldsJSON["Zip"];
                        //dr.Zipext = fieldsJSON["ZipExt"];
                        //dr.HomePhone = fieldsJSON["HomePhone"];
                        //dr.WorkPhone = fieldsJSON["WorkPhone"];
                        //dr.WorkPhext = fieldsJSON["WorkPhext"];
                        //dr.CellNo = fieldsJSON["CellNo"];
                        //dr.FaxNo = fieldsJSON["FaxNo"];
                        //dr.EmailAddress = fieldsJSON["EmailAddress"];
                        //dr.RelationShipId = MDVUtility.ToConvertInt32(fieldsJSON["RelationShipId"]);
                        //dr.IsPrimary = MDVUtility.ToStr(fieldsJSON["IsPrimary"]) == "True" ? true : false;
                        //dr.IsActive = true;//Utility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.CreatedOn = DateTime.Now;
                        //dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        //dr.ModifiedOn = DateTime.Now;


                        //dsPatient.EmergencyContacts.AddEmergencyContactsRow(dr);
                        //BLObject<DSPatient> obj = BLLPatientObj.InsertPatientEmergencyContact(dsPatient);
                        //if (obj.Data != null)
                        //{







                        //   }
                    }

                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Patient Not Found"
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }

                    //DSPatient dsPatient = new DSPatient();
                    ////DSPatient.EmergencyContactsRow dr = dsPatient.EmergencyContacts.NewEmergencyContactsRow();
                    //BLObject<DSPatient> objLoad = BLLPatientObj.LoadPatientEmergencyContact(PatientID, EmergencyContactID);
                    //dsPatient = objLoad.Data;
                    //foreach (DSPatient.EmergencyContactsRow dr in dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows)
                    //{
                    //    //dr.ContactId = EmergencyContactID;
                    //    dr.PatientId = MDVUtility.ToInt64(PatientID);
                    //    if (!string.IsNullOrEmpty(MDVUtility.ToStr(fieldsJSON["LastName"])))
                    //        dr.LastName = MDVUtility.ToStr(fieldsJSON["LastName"]);
                    //    if (!string.IsNullOrEmpty(MDVUtility.ToStr(fieldsJSON["FirstName"])))
                    //        dr.FirstName = MDVUtility.ToStr(fieldsJSON["FirstName"]);
                    //    if (!string.IsNullOrEmpty(MDVUtility.ToStr(fieldsJSON["MI"])))
                    //        dr.MI = MDVUtility.ToStr(fieldsJSON["MI"]);
                    //    if (!string.IsNullOrEmpty(MDVUtility.ToStr(fieldsJSON["DOB"])))
                    //        dr.DOB = MDVUtility.ToDateTime(fieldsJSON["DOB"]);
                    //    dr.Address1 = fieldsJSON["Address1"];
                    //    dr.Address2 = fieldsJSON["Address2"];
                    //    dr.City = fieldsJSON["City"];
                    //    dr.State = fieldsJSON["State"];
                    //    dr.Zipcode = fieldsJSON["Zip"];
                    //    dr.Zipext = fieldsJSON["ZipExt"];
                    //    dr.HomePhone = fieldsJSON["HomePhone"];
                    //    dr.WorkPhone = fieldsJSON["WorkPhone"];
                    //    dr.WorkPhext = fieldsJSON["WorkPhext"];
                    //    dr.CellNo = fieldsJSON["CellNo"];
                    //    dr.FaxNo = fieldsJSON["FaxNo"];
                    //    dr.EmailAddress = fieldsJSON["EmailAddress"];
                    //    dr.RelationShipId = MDVUtility.ToConvertInt32(fieldsJSON["RelationShipId"]);
                    //    if (fieldsJSON["HomePhone"] == "" && fieldsJSON["CellNo"] == "")
                    //    {
                    //        if (MDVUtility.ToStr(fieldsJSON["IsPrimary"]) == "True")
                    //        {
                    //            dr.IsPrimary = false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        dr.IsPrimary = MDVUtility.ToStr(fieldsJSON["IsPrimary"]) == "True" ? true : false;
                    //    }
                    //    //dr.IsPrimary = MDVUtility.ToStr(SearchedfieldsJSON["hfIsPrimary"]) == "True" ? true : false;
                    //    dr.IsActive = MDVUtility.ToStr(fieldsJSON["IsActive"]) == "True" ? true : false;

                    //    //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    //    //dr.CreatedOn = DateTime.Now;
                    //    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    //    dr.ModifiedOn = DateTime.Now;
                    //}


                    //#region Database Updation
                    ////dsPatient.EmergencyContacts.AddEmergencyContactsRow(dr);
                    ////dsPatient.Patients.AddPatientsRow(dr);
                    ////dsPatient.EmergencyContacts.AcceptChanges();

                    //if (dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows.Count > 0)
                    //{
                    //    //dsPatient.EmergencyContacts.Rows[0].SetModified();
                    //    BLObject<DSPatient> obj = BLLPatientObj.UpdatePatientEmergencyContact(dsPatient);
                    //    if (obj.Data != null)
                    //    {
                    //        DSPatient dsPatientNew = null;
                    //        BLObject<DSPatient> objLoadContacts = BLLPatientObj.loadPatientEmergencyContacts(PatientID);
                    //        dsPatientNew = objLoadContacts.Data;
                    //        if (objLoadContacts.Data != null)
                    //        {
                    //            var response = new
                    //            {
                    //                status = true,
                    //                EmergencyContactsCount = dsPatientNew.Tables[dsPatientNew.EmergencyContactsSearch.TableName].Rows.Count > 0 ? dsPatientNew.Tables[dsPatientNew.EmergencyContactsSearch.TableName].Rows.Count : 0,
                    //                EmergencyContactsLoad_JSON = MDVUtility.JSON_DataTable(dsPatientNew.Tables[dsPatientNew.EmergencyContactsSearch.TableName]),
                    //                iTotalDisplayRecords = (dsPatientNew.EmergencyContactsSearch.Rows.Count > 0) ? dsPatientNew.EmergencyContactsSearch.Rows[0][dsPatientNew.EmergencyContactsSearch.RecordCountColumn.ColumnName] : 0,
                    //            };
                    //            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    //        }
                    //        else
                    //        {
                    //            var response = new
                    //            {
                    //                status = false,
                    //                Message = objLoadContacts.Message
                    //            };
                    //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    //        }



                    //    }
                    //    else
                    //    {
                    //        var response = new
                    //        {
                    //            status = false,
                    //            Message = obj.Message
                    //        };
                    //        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    //    }
                    //}
                    //else
                    //{
                    //    var response = new
                    //    {
                    //        status = false,
                    //        Message = "No Record Found"
                    //    };
                    //    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    //}
                    //#endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Emergency Contact not found."
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
        public string FillEmergencyContact(Int64 ContactId, Int64 PatientId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr("Patient Not Found")
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatientEmergencyContact(PatientId, ContactId);
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;
                        if (dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.EmergencyContacts.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                        {
                            { "LastName", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.LastNameColumn.ColumnName])},
                            { "FirstName", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.FirstNameColumn.ColumnName])},
                            { "MI", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.MIColumn.ColumnName])},
                            { "DOB", MDVUtility.GetDateMMDDYYY(MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.DOBColumn.ColumnName]))},
                            { "Address1", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.Address1Column.ColumnName])},
                            { "Address2", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.Address2Column.ColumnName])},
                            { "City", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.CityColumn.ColumnName])},
                            { "State", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.StateColumn.ColumnName])},
                            { "Zip", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.ZipcodeColumn.ColumnName])},
                            { "ZipExt", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.ZipextColumn.ColumnName])},
                            { "HomePhone", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.HomePhoneColumn.ColumnName])},
                            { "WorkPhone", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.WorkPhoneColumn.ColumnName])},
                            { "WorkPhext", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.WorkPhextColumn.ColumnName])},
                            { "CellNo", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.CellNoColumn.ColumnName])},
                            { "FaxNo", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.FaxNoColumn.ColumnName])},
                            { "EmailAddress", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.EmailAddressColumn.ColumnName])},
                            { "IsPrimary", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.IsPrimaryColumn.ColumnName])},
                            { "RelationShipId", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.RelationShipIdColumn.ColumnName])},
                            { "IsActive", MDVUtility.ToStr(dr[dsPatient.EmergencyContacts.IsActiveColumn.ColumnName])}

                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                EmergencyContactsFill_JSON = js.Serialize(keyValues)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "No Record Found",
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

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
        public string FillPatientInfo(Int64 PatientId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr("Patient Not Found")
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.FillPatientById(PatientId, "EmergencyContact");
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;
                        if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>

                            {
                                { "Address1", MDVUtility.ToStr(dr[dsPatient.Patients.Address1Column.ColumnName])},
                                { "Address2", MDVUtility.ToStr(dr[dsPatient.Patients.Address2Column.ColumnName])},
                                { "City", MDVUtility.ToStr(dr[dsPatient.Patients.CityColumn.ColumnName])},
                                { "State", MDVUtility.ToStr(dr[dsPatient.Patients.StateColumn.ColumnName])},
                                { "Zip", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeColumn.ColumnName])},
                                { "ZipExt", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeExtColumn.ColumnName])},
                                { "HomePhone", MDVUtility.ToStr(dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName])},

                            };


                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientInfoFill_JSON = js.Serialize(keyValues)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "No Record Found",
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }

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
        public string DeleteEmergencyContact(Int64 EmergencyContactId)
        {

            try
            {
                string strJSONData = ObjPatientEmergencyContact.DeleteEmergencyContact(MDVUtility.ToInt64(EmergencyContactId));

                return strJSONData;
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
        public string DeletePatientPicture(Int64 PatientId)
        {

            try
            {

                string ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                DateTime ModifiedOn = DateTime.Now;
                string strJSONData = BLLPatientObj.DeletePatientPicNative(ModifiedBy, ModifiedOn, PatientId);

                if (strJSONData == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = "Picture has been Updated",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = strJSONData,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public HashSet<NameValuePair> GetCommunication(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupCommunication();
            DSPatientLookups ds = objPatient.Data;
            list.Add(new NameValuePair("- Select -", ""));
            if (ds != null)
            {
                if (ds.Tables[ds.Communication.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Communication.TableName].Select("1=1", ds.Communication.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        list.Add(new NameValuePair(dr[ds.Communication.DescriptionColumn.ColumnName].ToString(), dr[ds.Communication.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

        public string UpdatePatientPreferences(JObject data)
        {
            PatientPreferenceModel Model = JsonConvert.DeserializeObject<PatientPreferenceModel>(MDVUtility.ToStr(data["data"]));

            string PatientId = !string.IsNullOrEmpty(Model.PatientId) ? MDVUtility.ToStr(Model.PatientId) : MDVUtility.ToStr(Model.DimmyPatientId);

            // string JSONFields = MDVUtility.ToStr(fieldsJSON);
            try
            {
                if (!string.IsNullOrEmpty(PatientId))
                {



                    //drPatient.PrefCommModeId = !string.IsNullOrEmpty(Convert.ToString(fieldsJSON["PrefCommModeId"])) ? fieldsJSON["PrefCommModeId"] : null;
                    //drPatient.PreferredPrimaryContactId = !string.IsNullOrEmpty(Convert.ToString(fieldsJSON["PreferredPrimaryContactId"])) ? fieldsJSON["PreferredPrimaryContactId"] : null;



                    string Result = "";

                    foreach (var item in Model.DataChangeRequest)
                    {
                        item.ColumnKeyId = Model.PatientId;
                        item.ColumnKeyName = "PatientId";
                        item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.CreatedOn = DateTime.Now;
                        item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        item.ModifiedOn = DateTime.Now;

                        item.IsSynced = false;
                        item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                        item.DBTableName = "PatientPreferences";
                        item.PatientId = !string.IsNullOrEmpty(Model.PatientId) ? Convert.ToInt64(Model.PatientId) : (long?)null;
                        item.DimmyPatientId = !string.IsNullOrEmpty(Model.DimmyPatientId) ? MDVUtility.ToStr(Model.DimmyPatientId) : null;
                        // string returnVal = new DALPatient(true).InsertupdateDemographicsInDBAuditNative(dbManager, item);

                        string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(item);



                        if (returnVal != "")
                        {
                            Result = Result + returnVal;
                        }
                        else
                        {

                        }


                    }
                    if (Result == "")
                    {
                        var response = new
                        {
                            status = true,
                            message = successMessage
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
                        return JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient not found."
                    };
                    return JsonConvert.SerializeObject(response);
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

        public string UpdatePrimaryContact(JObject data)
        {
            try
            {



                var fieldsJSON = JsonConvert.DeserializeObject<dynamic>(MDVUtility.ToStr(data["data"]));

                Int64 PatientID = MDVUtility.ToInt64(fieldsJSON.PatientId);
                Int64 EmergencyContactID = MDVUtility.ToInt64(fieldsJSON.EmergencyContactID);
                Int64 IsPrimary = MDVUtility.ToInt64(fieldsJSON.IsPrimary);
                if (EmergencyContactID > 0)
                {

                    DSPatient dsPatient = new DSPatient();
                    //DSPatient.EmergencyContactsRow dr = dsPatient.EmergencyContacts.NewEmergencyContactsRow();


                    //    string result = ObjPatientEmergencyContact.UpdateEmergencyContactIsPrimary(PatientID, EmergencyContactID, IsPrimary);
                    DataChangeRequest item = new DataChangeRequest();


                    item.ColumnKeyId = MDVUtility.ToStr(EmergencyContactID);
                    item.ColumnKeyName = "ContactId";
                    item.DBTableName = "EmergencyContacts";
                    item.columnName = "IsPrimary";
                    item.OriginalValueDisplay = "";
                    item.CurrentValueDisplay = MDVUtility.ToStr(IsPrimary);
                    item.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.CreatedOn = DateTime.Now;
                    item.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    item.ModifiedOn = DateTime.Now;
                    item.PatientId = Convert.ToInt64(PatientID);
                    item.IsSynced = false;
                    item.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);



                    string returnVal = BLLMobileAppObj.SaveRecordInDBAuditNative(item);
                    if (returnVal == "")
                    {
                        var responseResult = new
                        {
                            status = true,
                            message = successMessage
                        };
                        return (JsonConvert.SerializeObject(responseResult));
                    }
                    else
                    {
                        var responseResult = new
                        {
                            status = false,
                            Message = returnVal
                        };
                        return JsonConvert.SerializeObject(responseResult);
                    }




                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Emergency Contact Not Found.",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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



        #region "Functions"

        #endregion

    }
}