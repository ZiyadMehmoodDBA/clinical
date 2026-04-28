using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;

using Newtonsoft.Json;
using MDVision.IEHR.Common;
using MDVision.Model.Patient;
using MDVision.IEHR.Controls.Clinical;
using Newtonsoft.Json.Linq;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Web.Configuration;
using System.IO;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.IEHR.EMR.Model.Patient;
using MDVision.IEHR.EMR.Helpers.Patient;
using MDVision.Model.Clinical.Medical;
using MDVision.Model.MU;

namespace MDVision.IEHR.Controls.Patient.Demographics
{
    public class Patient_Demographic_Quick
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_Demographic_Quick()
        {
            BLLPatientObj = new BLLPatient();
        }
        #region Singleton
        private static Patient_Demographic_Quick _obj = null;
        private static bool isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
        public static Patient_Demographic_Quick Instance()
        {
            if (_obj == null)
            {
                _obj = new Patient_Demographic_Quick();
                //   isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
            }
            return _obj;
        }
        #endregion

        #region Private Functions
        private string SaveDemographicQuick(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatient dsPatient = new DSPatient();
                DSPatient.PatientsRow dr = dsPatient.Patients.NewPatientsRow();

                dr.LastName = SearchedfieldsJSON["txtLastName"];
                dr.MotherMaidenName = SearchedfieldsJSON["txtMotherMaidenName"];
                dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpDOB"])))
                    dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSex"]))
                    dr.Gender = MDVUtility.ToStr(SearchedfieldsJSON["ddlSex_text"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMaritalStatus"]))
                    dr.MaritialStatus = MDVUtility.ToStr(SearchedfieldsJSON["ddlMaritalStatus_text"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlEthnicity"]))
                    dr.EthnicityId = MDVUtility.ToInt(SearchedfieldsJSON["ddlEthnicity"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlRace"]))
                    dr.RaceId = MDVUtility.ToInt(SearchedfieldsJSON["ddlRace"]);
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPrefLanguage"]))
                    dr.PrefLanguageId = MDVUtility.ToInt(SearchedfieldsJSON["ddlPrefLanguage"]);
                dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                dr.City = SearchedfieldsJSON["txtCity"];
                dr.State = SearchedfieldsJSON["txtState"];
                dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                dr.ZIPCodeExt = SearchedfieldsJSON["txtZipExt"];
                dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPractice"]);
                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? 1 : 0;
                //dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                dr.BadAddress = false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.Comments = SearchedfieldsJSON["txtComments"];
                dr.CellNo = SearchedfieldsJSON["txtCell"];
                dr.PatientPortalStatus = "0";
                #region Database Insertion
                dsPatient.Patients.AddPatientsRow(dr);
                BLObject<DSPatient> obj = BLLPatientObj.InsertPatient(dsPatient);
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        PatientId = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PatientIdColumn.ColumnName],
                        AccountNumber = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.AccountNumberColumn.ColumnName]
                    };

                    // Start DrFirst changes by babur on 1/14/2016
                    if (isDrFirstRequired == true)
                    {
                        #region Add Patient In DrFirst
                        //temp

                        string PatientID = MDVUtility.ToStr(dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PatientIdColumn.ColumnName]);
                        //string ResponseOfDrFirst = SavePatientInDrFirst(dr, PatientID);
                        BLLRcopia BLLRcopiaObj = new BLLRcopia(); 
                        dynamic ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("Patient", dr, PatientID));
                        if (ResponseOfDrFirst.Rcopia != "")
                        {
                            DSPatient DatasetPatient = new DSPatient();
                            DSPatient.PatientsRow PatientRow = DatasetPatient.Patients.NewPatientsRow();
                            PatientRow.PatientId = MDVUtility.ToLong(PatientID);
                            DatasetPatient.Patients.AddPatientsRow(PatientRow);
                            PatientRow.RcopiaID = ResponseOfDrFirst.Rcopia;
                            BLObject<DSPatient> obj1 = BLLPatientObj.InsertPatientsRcopialID(DatasetPatient);

                            RcopiaModel model = new RcopiaModel();
                            model.PatientId = PatientID;
                            model.AllergyLastUpdateDate = "";
                            model.MedicationLastUpdateDate = "";
                            model.PrescriptionLastUpdateDate = "";
                            RcopiaHelper helperRcopia = new RcopiaHelper();

                            helperRcopia.UpdatePatientLastUpdateInfo(model);
                        }
                        else
                        {
                            var response1 = new
                            {
                                status = false,
                                Message = "Problem in ADD Patient on DrFirst"
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response1);

                        }

                        #endregion
                    }

                    // End DrFirst changes by babur on 1/14/2016


                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
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
        public string SaveDemographicQuick(PatientDemographicQuickModel model)
        {
            try
            {
                DSPatient dsPatient = new DSPatient();
                DSPatient.PatientsRow dr = dsPatient.Patients.NewPatientsRow();

                dr.LastName = model.LastName;
                dr.MotherMaidenName = model.MotherMaidenName;
                dr.FirstName = model.FirstName;
                if (!string.IsNullOrEmpty(model.DOB))
                    dr.DOB = MDVUtility.ToDateTime(model.DOB);
                if (!string.IsNullOrEmpty(model.Gender))
                {
                    if (model.Gender == "0")
                        dr.Gender = "Male";
                    else if (model.Gender == "1")
                        dr.Gender = "Female";
                    else if (model.Gender == "2")
                        dr.Gender = "Unknown";
                }

                if (!string.IsNullOrEmpty(model.MaritalStatus))
                    dr.MaritialStatus = MDVUtility.ToStr(model.MaritalStatus_text);
                if (!string.IsNullOrEmpty(model.Race))
                    dr.RaceId = MDVUtility.ToInt(model.Race);
                if (!string.IsNullOrEmpty(model.PrefLanguage))
                    dr.PrefLanguageId = MDVUtility.ToInt(model.PrefLanguage);
                dr.Address1 = model.Address1;
                dr.City = model.city;
                dr.State = model.State;
                dr.ZIPCode = model.Zip;
                dr.ZIPCodeExt = model.ZipExt;

                if (!string.IsNullOrEmpty(model.frontimage))
                {
                    if (!model.frontimage.Contains("default"))
                    {
                        ////string strBase64 = model.frontimage.Split(',')[1];
                        ////byte[] currentFileStream = Convert.FromBase64String(strBase64);
                        ////dr.PatientImage = currentFileStream;
                        ////dr.ImageType = model.frontimage.Split(',')[0].Split(':')[1].Split(';')[0];

                        string strBase64 = model.frontimage.Split(',')[1];
                        string filetype = string.Empty;
                        string QuickProfileImage = string.Empty;

                        dr.ImageType = model.frontimage.Split(',')[0].Split(':')[1].Split(';')[0];
                        filetype = model.frontimage.Split(',')[0].Split(':')[1].Split(';')[0].Split('/')[1];

                        string dt = DateTime.Now.ToShortDateString();
                        string tm = DateTime.Now.ToShortTimeString();
                        string fName = dt + " " + tm.Replace(" ", "");
                        fName = fName.Replace("/", "-").Replace(":", "");

                        var mnth = fName.Split('-')[0];
                        var day = fName.Split('-')[1];
                        var year = fName.Split('-')[2];
                        mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                        day = day.Length == 1 ? "0" + day : day;
                        var mnt = year.Split(' ')[1];
                        year = year.Split(' ')[0];
                        mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                        //fName = mnth + "-" + day + "-" + year + " " + mnt ;
                        fName = mnth + "." + day + "." + year ;


                        QuickProfileImage = model.LastName + '-' + model.FirstName + '-' + fName;
                        PatientDemographicQuickImageSaveInFolder(strBase64, QuickProfileImage, filetype);
                        dr.PatientProfileImagePath = QuickProfileImage + "." + filetype;
                       
                    }
                    else
                    {
                        dr.PatientProfileImagePath = null;
                        dr.ImageType = "";
                    }
                }
                if (!string.IsNullOrEmpty(model.ProviderID))
                    dr.ProviderId = MDVUtility.ToInt64(model.ProviderID);
                if (!string.IsNullOrEmpty(model.FacilityID))
                    dr.FacilityId = MDVUtility.ToInt64(model.FacilityID);
                if (!string.IsNullOrEmpty(model.PracticeID))
                    dr.PracticeId = MDVUtility.ToInt64(model.PracticeID);
                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                //dr.IsActive = MDVUtility.ToStr(model.Active) == "True" ? true : false;
                dr.IsActive = MDVUtility.ToStr(model.Active) == "True" ? 1 : 0;
                dr.BadAddress = false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.Comments = model.Comments;
                dr.CellNo = model.Cell;
                dr.PatientPortalStatus = "0";
                dr.strRaceIds = model.strRaceIds;//kr
                //------------Save Value in RaceId as instructed by Babur sb-----
                //Start Change made by Khaleel Ur rehman on 6 May 2016.
                if (!string.IsNullOrEmpty(model.strRaceIds))
                {
                    if (model.strRaceIds.Length > 0)
                    {
                        dr.RaceId = MDVUtility.ToInt(model.strRaceIds[0]);
                    }
                }
                dr.strEthnicityIds = model.strEthnicityIds;
                if (!string.IsNullOrEmpty(model.strEthnicityIds))
                    if (model.strEthnicityIds.Length > 0)
                        dr.EthnicityId = MDVUtility.ToInt(model.strEthnicityIds.Split(',')[0]);

                //Start 23-08-2016 Humaira Yousaf for hear from info
                if (!string.IsNullOrEmpty(model.HearFromId))
                {
                    dr.HearFromId = MDVUtility.ToInt32(model.HearFromId);
                }
                if (MDVUtility.ToInt32(model.HearFromId) == 10 && !string.IsNullOrEmpty(model.HearFromOther))
                {
                    dr.HearFromOther = model.HearFromOther;
                }
                else
                {
                    dr.HearFromOther = null;
                }
                //End 23-08-2016 Humaira Yousaf for hear from info
                dr.IsCCDAAvailable = model.IsCCDAAvailable;
                #region Database Insertion
                dsPatient.Patients.AddPatientsRow(dr);
                BLObject<DSPatient> obj = BLLPatientObj.InsertPatient(dsPatient);
                if (obj.Data != null)
                {
                    Int64 Patient_Id = Convert.ToInt64(dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PatientIdColumn.ColumnName]);

                    if (Patient_Id > 0 && model.HearFromId == "1")
                    {
                        PatientReferralModel referralModel = new PatientReferralModel();
                        referralModel.PatientId = MDVUtility.ToStr(Patient_Id);
                        referralModel.ReferralId = model.ReferralId;
                        referralModel.Type = "Incoming";
                        referralModel.ProviderId = model.ProviderIdReferral;
                        referralModel.RefProviderId = model.RefProviderIdReferral;
                        referralModel.Reason = model.Reason;
                        referralModel.Visits = model.Visits;
                        referralModel.Date = model.Date;
                        referralModel.Time = model.Time;
                        referralModel.AssigneeId = model.AssigneeId;
                        referralModel.PatientInsurance = model.PatientInsurance;
                        referralModel.Status = model.Status;
                        referralModel.Comments = model.CommentsReferral;


                        PatientReferralHelper referralHelper = new PatientReferralHelper();
                        referralHelper.InsertUpdatePatientReferral(referralModel);
                    }

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        PatientId = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PatientIdColumn.ColumnName],
                        AccountNumber = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.AccountNumberColumn.ColumnName]
                    };
                    if (!string.IsNullOrEmpty(model.imagedata))
                    {
                      
                        //string isSaved = SavePatientDocument(Patient_Id, model.imagedata, model.filetype, model.filename, model.foldername, model.AssignUserto,model.AssignedToName, model.ScannerDOS, model.ScannerComments);
                        string isSaved = SavePatientDocument(Patient_Id, model.imagedata, "image/jpg", model.filename, true,true);
                    }
                    // Start DrFirst changes by babur on 1/14/2016

                    if (isDrFirstRequired == true)
                    {
                        #region Add Patient In DrFirst

                        string PatientID = MDVUtility.ToStr(dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PatientIdColumn.ColumnName]);
                        //string ResponseOfDrFirst = SavePatientInDrFirst(dr, PatientID);
                        BLLRcopia BLLRcopiaObj = new BLLRcopia();
                        dynamic ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("Patient", dr, PatientID));
                        if (ResponseOfDrFirst.Rcopia != "")
                        {
                            DSPatient DatasetPatient = new DSPatient();
                            DSPatient.PatientsRow PatientRow = DatasetPatient.Patients.NewPatientsRow();
                            PatientRow.PatientId = MDVUtility.ToLong(PatientID);
                            DatasetPatient.Patients.AddPatientsRow(PatientRow);
                            PatientRow.RcopiaID = ResponseOfDrFirst.Rcopia;
                            BLObject<DSPatient> obj1 = BLLPatientObj.InsertPatientsRcopialID(DatasetPatient);

                            RcopiaModel modelRcopia = new RcopiaModel();
                            modelRcopia.PatientId = PatientID;
                            modelRcopia.AllergyLastUpdateDate = "";
                            modelRcopia.MedicationLastUpdateDate = "";
                            modelRcopia.PrescriptionLastUpdateDate = "";
                            RcopiaHelper helperRcopia = new RcopiaHelper();

                            helperRcopia.UpdatePatientLastUpdateInfo(modelRcopia);
                        }
                        else
                        {
                            var response1 = new
                            {
                                status = false,
                                Message = "Problem in ADD Patient on DrFirst"
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response1);

                        }

                        #endregion
                    }

                    // Commented as requirement may be need in future, so do not remove code. PRD-795
                    //if (MDVUtility.StringToBoolean(MDVSession.Current.isDemographics))
                    //{
                    //    BLLMUAlerts BLLMUAlertsObj = new BLLMUAlerts();
                    //    MUModel model_ = new MUModel();
                    //    MUAlertsModel ob1 = new MUAlertsModel();
                    //    ob1.ProfileName = "Healthcare Surveys";
                    //    ob1.Fields = "Country,Preferred Address,Preferred Phone";
                    //    ob1.PatientId = Patient_Id;
                    //    ob1.IsShowAlert = true;
                    //    ob1.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    //    ob1.ModifiedBy = ob1.CreatedBy;
                    //    ob1.UserId = MDVSession.Current.AppUserId;
                    //    ob1.Type = "MU3";
                    //    model_.MUAlerts.Add(ob1);

                    //    MUAlertsModel ob2 = new MUAlertsModel();
                    //    ob2.ProfileName = "Demographics";
                    //    ob2.Fields = "Sexual Orientation,Gender Identity,Birth Sex";
                    //    ob2.PatientId = Patient_Id;
                    //    ob2.IsShowAlert = true;
                    //    ob2.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    //    ob2.ModifiedBy = ob2.CreatedBy;
                    //    ob2.UserId = MDVSession.Current.AppUserId;
                    //    ob2.Type = "MU3";
                    //    model_.MUAlerts.Add(ob2);
                    //    BLObject<string> obj_ = BLLMUAlertsObj.InsertMUAlerts(model_.MUAlerts);
                    //}

                    // End DrFirst changes by babur on 1/14/2016

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
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
        // public string SavePatientDocument(Int64 PatientID, string strBase64 = null, string fileType = null, string FileName = "", string FolderId = "", string AssignedToId = "", string AssignedToName = "", string Date = "", string comments = "")
        public string SavePatientDocument(Int64 PatientID, string strBase64 = null, string fileType = null, string FileName = "", Boolean IsScan = false,bool IsfromQuickAdd = false)
        {
            try
            {


                if (PatientID > -1) //Changed by Arslan for Patient Education Flow
                {
                    DSPatient dsDocument = new DSPatient();
                    DSMessage dsMessage = new DSMessage();

                    DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                    dr.PatientId = PatientID;
                    dr.IsScan = IsScan;
                    //if (!string.IsNullOrEmpty(AssignedToId))
                    //{
                    //    dr.AssignedToId = MDVUtility.ToInt64(AssignedToId);
                    //    dr.ViewBy = MDVUtility.ToStr(AssignedToName);
                    //}

                    //if (!string.IsNullOrEmpty(Date))
                    //{
                    //    string dtpDOS = System.Web.HttpUtility.UrlDecode(Date);

                    //    if (!string.IsNullOrEmpty(dtpDOS))
                    //        dr.DOS = MDVUtility.ToDateTime(dtpDOS);
                    //}
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlClaim"]))
                    //    dr.ClaimId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlClaim"]);
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlCase"]))
                    //    dr.CaseId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlCase"]);
                    byte[] currentFileStream = Convert.FromBase64String(strBase64.Split(',')[1]);


                    dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
                    dr.FileType = fileType;                            
                    if (FileName == "")
                        dr.FilePath = "Patient ID Card" + "." + fileType.Split('/')[1];//Utility.ToStr(SearchedfieldsJSON["txtDescription"]);
                    else
                        dr.FilePath = FileName + "." + fileType.Split('/')[1];
                    MemoryStream ms = new MemoryStream(currentFileStream);

                    //if (!string.IsNullOrEmpty(comments))
                    //{
                    //    dr.Comments = System.Web.HttpUtility.UrlDecode(comments);
                    //}

                    if (fileType == "application/pdf")
                        
dr.Pages = MDVUtility.getPdfPagesCount(currentFileStream);
                    else
                        dr.Pages = 1;
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    //if (advancePaymentId != null)
                    //    dr.AdvPaymentId = MDVUtility.ToInt64(advancePaymentId);

                    //if (!string.IsNullOrEmpty(refModuleName))
                    //{
                    //    dr.RefModuleName = refModuleName;
                    //}
                    //else
                    //{
                    //    dr[dsDocument.PatientDocument.RefModuleNameColumn] = DBNull.Value;
                    //}

                    //if (TransitionId > 0)
                    //{
                    //    dr.TransitionId = TransitionId;
                    //}
                    //else
                    //{
                    //    dr[dsDocument.PatientDocument.TransitionIdColumn] = DBNull.Value;
                    //}

                    dsDocument.PatientDocument.AddPatientDocumentRow(dr);


                    #region Database Insertion
                    BLObject<DSPatient> obj = BLLPatientObj.InsertPatientDocument(dsDocument, IsfromQuickAdd);
                    if (obj.Data != null)
                    {
                        //var response = new
                        //{
                        //    status = true,
                        //    message = Common.AppPrivileges.Save_Message,
                        //    MedicalDocumentId = dsDocument.Tables[dsDocument.PatientDocument.TableName].Rows[0][dsDocument.PatientDocument.MedicalDocIdColumn.ColumnName]
                        //};
                        //return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                        return "Saved";
                    }
                    else
                    {
                        //var response = new
                        //{
                        //    status = false,
                        //    Message = obj.Message
                        //};
                        //return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        return "not saved";
                    }
                    #endregion
                }
                else
                {
                    throw new Exception("Patient not found.");
                }

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

        public bool PatientDemographicQuickImageSaveInFolder(string ImgStr, string ImgName, string fileType)
        {

            try
            {
                string ServerPath = System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesPath"];
                if (!string.IsNullOrEmpty(ImgName))
                {
                    if (!System.IO.Directory.Exists(ServerPath))
                    {
                        System.IO.Directory.CreateDirectory(ServerPath); //Create directory if it doesn't exist
                    }
                    //Save profile img
                    string lPatientProfileImageName = ImgName + "." + fileType;
                    string imgPath = Path.Combine(ServerPath, lPatientProfileImageName);
                    byte[] imageBytes = Convert.FromBase64String(ImgStr);
                    File.WriteAllBytes(imgPath, imageBytes);

                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_DEMOGRAPHIC_QUICK":
                    {
                        string fieldsJSON = context.Request["DemographicData"];
                        string strJSONData = SaveDemographicQuick(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}
