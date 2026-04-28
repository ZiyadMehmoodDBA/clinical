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
    public class Patient_Demographic
    {


        private BLLPatient BLLPatientObj = null;
        private BLLRcopia BLLRcopiaObj = null;
        private BLLCQM BLLCQMObj = null;
        public Patient_Demographic()
        {
            BLLPatientObj = new BLLPatient();
            BLLRcopiaObj = new BLLRcopia();
            BLLCQMObj = new BLLCQM();
        }
        #region Singleton
        private static Patient_Demographic _obj = null;
        private static bool isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
        public static Patient_Demographic Instance()
        {
            if (_obj == null)
            {
                _obj = new Patient_Demographic();
                //  isDrFirstRequired = Convert.ToBoolean(WebConfigurationManager.AppSettings["isDrFirstRequired"]);
            }
            return _obj;
        }
        #endregion




        #region Private Functions
        /// <summary>
        /// Saves the demographic.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        private string SaveDemographic(string fieldsJSON, string IsDemographicQuick)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatient dsPatient = new DSPatient();
                DSPatient.PatientsRow dr = dsPatient.Patients.NewPatientsRow();

                if (IsDemographicQuick.ToLower() != "true")
                {
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPrefix"]))
                        dr.Prefix = MDVUtility.ToStr(SearchedfieldsJSON["ddlPrefix_text"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSuffix"]))
                        dr.Suffix = MDVUtility.ToStr(SearchedfieldsJSON["ddlSuffix_text"]);
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
                    //dr.AccountNumber = SearchedfieldsJSON["txtAccountNo"];
                    dr.MRNumber = SearchedfieldsJSON["txtMRN"];
                    dr.SSN = SearchedfieldsJSON["txtSSN"];
                    dr.LastName = SearchedfieldsJSON["txtLastName"];
                    dr.MotherMaidenName = SearchedfieldsJSON["txtMotherMaidenName"];

                    dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                    dr.MI = SearchedfieldsJSON["txtMiddleInitial"];
                    dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                    dr.PreviousName = SearchedfieldsJSON["txtPreviousName"];
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpDOB"])))
                        dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                    dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                    dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                    dr.City = SearchedfieldsJSON["txtCity"];
                    dr.State = SearchedfieldsJSON["txtState"];
                    dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                    dr.ZIPCodeExt = SearchedfieldsJSON["txtZipExt"];
                    dr.HomePhoneNo = SearchedfieldsJSON["txtHomeTel"];
                    dr.WorkPhoneNo = SearchedfieldsJSON["txtWorkTel"];
                    dr.WorkPhoneExt = SearchedfieldsJSON["txtExt"];
                    dr.CellNo = SearchedfieldsJSON["txtCell"];
                    dr.FaxNo = SearchedfieldsJSON["txtFax"];
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]))
                        dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPractice"]))
                        dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPractice"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfRefProvider"]))
                        dr.ReferringProviderId = SearchedfieldsJSON["hfRefProvider"];
                    else
                        dr.ReferringProviderId = null;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPCP"]))
                        dr.PCPId = SearchedfieldsJSON["hfPCP"];
                    else
                        dr.PCPId = null;
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfGuarantor"]))
                        dr.GuarantorId = SearchedfieldsJSON["hfGuarantor"];
                    else
                        dr.GuarantorId = null;
                    dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    dr.BadAddress = MDVUtility.ToStr(SearchedfieldsJSON["chkBadAddress"]) == "True" ? true : false;
                }
                else
                {
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
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]))
                        dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                        dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPractice"]))
                        dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPractice"]);
                    dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    dr.BadAddress = false;

                }
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPatientStatus"]))
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["ddlPatientStatus"]);
                // dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                if (SearchedfieldsJSON.ContainsKey("txtInactiveReason") && SearchedfieldsJSON["chkActive"] == false)
                {
                    dr.InActiveReason = SearchedfieldsJSON["txtInactiveReason"];
                }
                else
                    dr.InActiveReason = "";
                dr.Comments = SearchedfieldsJSON["txtComments"];
                dr.PatientPortalStatus = "false";
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                if (SearchedfieldsJSON.ContainsKey("chkSelfPay"))
                {
                    dr.SelfPay = MDVUtility.ToStr(SearchedfieldsJSON["chkSelfPay"]) == "True" ? true : false;
                }
                dr.PatientPortalStatus = "0";
                if (!string.IsNullOrEmpty(SearchedfieldsJSON["CauseOfDeath"]))
                    dr.CauseOfDeath = MDVUtility.ToStr(SearchedfieldsJSON["CauseOfDeath"]);


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
                    //temp
                    if (isDrFirstRequired == true)
                    {
                        #region Add Patient In DrFirst

                        string PatientID = MDVUtility.ToStr(dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PatientIdColumn.ColumnName]);
                        //string ResponseOfDrFirst = SavePatientInDrFirst(dr, PatientID);
                        ProblemListHelper objProblemlisthelper = new ProblemListHelper();
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
                    // End DrFirst changes temporary commented by babur on 1/14/2016

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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string SaveDemographic(PatientDemographicModel model)
        {
            try
            {

                DSPatient dsPatient = new DSPatient();
                DSPatient.PatientsRow dr = dsPatient.Patients.NewPatientsRow();

                if (model.IsDemographicQuick.ToLower() != "true")
                {
                    if (!string.IsNullOrEmpty(model.Prefix))
                        dr.Prefix = MDVUtility.ToStr(model.Prefix_text);
                    if (!string.IsNullOrEmpty(model.Suffix))
                        dr.Suffix = MDVUtility.ToStr(model.Suffix_text);
                    if (!string.IsNullOrEmpty(model.Sex))
                        dr.Gender = MDVUtility.ToStr(model.Sex_text);
                    if (!string.IsNullOrEmpty(model.MaritalStatus))
                        dr.MaritialStatus = MDVUtility.ToStr(model.MaritalStatus_text);
                    if (!string.IsNullOrEmpty(model.Ethnicity))
                        dr.EthnicityId = MDVUtility.ToInt(model.Ethnicity);
                    //if (!string.IsNullOrEmpty(model.Race))
                    //    dr.RaceId = MDVUtility.ToInt(model.Race);
                    if (!string.IsNullOrEmpty(model.LanguageID))
                        dr.PrefLanguageId = MDVUtility.ToInt(model.LanguageID);
                    //dr.AccountNumber = SearchedfieldsJSON["txtAccountNo"];
                    dr.MRNumber = model.MRN;
                    dr.SSN = model.SSN;
                    dr.LastName = model.LastName;
                    dr.MotherMaidenName = model.MotherMaidenName;
                    dr.FirstName = model.FirstName;
                    dr.MI = model.MiddleInitial;
                    dr.EmailAddress = model.Email;
                    dr.PreviousName = model.PreviousName;
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.DOB)))
                        dr.DOB = MDVUtility.ToDateTime(model.DOB);
                    dr.Address1 = model.Address1;
                    dr.Address2 = model.Address2;
                    dr.City = model.City;
                    dr.State = model.State;
                    dr.ZIPCode = model.Zip;
                    dr.ZIPCodeExt = model.ZipExt;
                    dr.HomePhoneNo = model.HomeTel;
                    dr.WorkPhoneNo = model.WorkTel;
                    dr.WorkPhoneExt = model.Ext;
                    dr.CellNo = model.Cell;
                    dr.FaxNo = model.Fax;
                    if (!string.IsNullOrEmpty(model.ProviderID))
                        dr.ProviderId = MDVUtility.ToInt64(model.ProviderID);
                    if (!string.IsNullOrEmpty(model.FacilityID))
                        dr.FacilityId = MDVUtility.ToInt64(model.FacilityID);
                    if (!string.IsNullOrEmpty(model.PracticeID))
                        dr.PracticeId = MDVUtility.ToInt64(model.PracticeID);
                    if (!string.IsNullOrEmpty(model.RefProviderID))
                        dr.ReferringProviderId = model.RefProviderID;
                    else
                        dr.ReferringProviderId = null;
                    if (!string.IsNullOrEmpty(model.PCPID))
                        dr.PCPId = model.PCPID;
                    else
                        dr.PCPId = null;
                    if (!string.IsNullOrEmpty(model.GuarantorID))
                        dr.GuarantorId = model.GuarantorID;
                    else
                        dr.GuarantorId = null;
                    dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);

                    if (!string.IsNullOrEmpty(model.BirthSex))
                        dr.BirthSex = model.BirthSex;
                    else
                        dr.BirthSex = null;

                    //if (!string.IsNullOrEmpty(model.imgPatient))
                    //{
                    //    if (!model.imgPatient.Contains("default"))
                    //    {
                    //        string strBase64 = model.imgPatient.Split(',')[1];
                    //        byte[] currentFileStream = Convert.FromBase64String(strBase64);
                    //        byte[] PatientImageThumbnailStream = Convert.FromBase64String(MDVUtility.ResizeImage(strBase64, 100, 100));
                    //        dr.PatientImageThumbnail = PatientImageThumbnailStream;
                    //        dr.PatientImage = currentFileStream;
                    //        dr.ImageType = model.imgPatient.Split(',')[0].Split(':')[1].Split(';')[0];
                    //    }
                    //    else
                    //    {
                    //        dr.PatientImage = null;
                    //        dr.ImageType = "";
                    //    }
                    //}

                    string filetype = string.Empty;

                    string PatientProfileImagePath = string.Empty;
                    string PatientProfileThumbnailPath = string.Empty;

                    if (!string.IsNullOrEmpty(model.imgPatient))
                    {
                        if (!model.imgPatient.Contains("default"))
                        {
                            string strBase64 = model.imgPatient.Split(',')[1];
                            byte[] currentFileStream = Convert.FromBase64String(strBase64);

                            byte[] PatientImageThumbnailStream = Convert.FromBase64String(MDVUtility.ResizeImage(strBase64, 100, 100));
                            // dr.PatientImageThumbnail = PatientImageThumbnailStream;

                            //dr.PatientImage = currentFileStream;
                            dr.ImageType = model.imgPatient.Split(',')[0].Split(':')[1].Split(';')[0];

                            filetype = model.imgPatient.Split(',')[0].Split(':')[1].Split(';')[0].Split('/')[1];

                            //PatientProfileImagePath = "new_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            PatientProfileImagePath = model.LastName + '-' + model.FirstName + '-' + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            PatientDemographicImageSaveInFolder(strBase64, PatientProfileImagePath, filetype);

                            dr.PatientProfileImagePath = PatientProfileImagePath + "." + filetype;
                            dr.PatientProfileThumbnailPath = PatientProfileImagePath + "-th." + filetype;

                        }
                        else
                        {
                            //dr.PatientImage = null;
                            dr.PatientProfileImagePath = null;

                            dr.ImageType = "";
                        }
                    }


                    dr.BadAddress = MDVUtility.ToStr(model.BadAddress) == "True" ? true : false;
                    dr.IsTCM = MDVUtility.ToStr(model.TransitionalCareManagement) == "True" ? true : false;
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.DischargeDate)))
                        dr.DischargeDate = MDVUtility.ToDateTime(model.DischargeDate);
                }
                else
                {
                    dr.LastName = model.LastName;
                    dr.MotherMaidenName = model.MotherMaidenName;
                    dr.FirstName = model.FirstName;
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.DOB)))
                        dr.DOB = MDVUtility.ToDateTime(model.DOB);
                    if (!string.IsNullOrEmpty(model.Sex))
                        dr.Gender = MDVUtility.ToStr(model.Sex_text);
                    if (!string.IsNullOrEmpty(model.MaritalStatus))
                        dr.MaritialStatus = MDVUtility.ToStr(model.MaritalStatus_text);
                    //if (!string.IsNullOrEmpty(model.Ethnicity))
                    //    dr.EthnicityId = MDVUtility.ToInt(model.Ethnicity);
                    //if (!string.IsNullOrEmpty(model.Race))
                    //    dr.RaceId = MDVUtility.ToInt(model.Race);
                    if (!string.IsNullOrEmpty(model.LanguageID))
                        dr.PrefLanguageId = MDVUtility.ToInt(model.LanguageID);

                    dr.Address1 = model.Address1;
                    dr.City = model.City;
                    dr.State = model.State;
                    dr.ZIPCode = model.Zip;
                    dr.ZIPCodeExt = model.ZipExt;

                    if (!string.IsNullOrEmpty(model.ProviderID))
                        dr.ProviderId = MDVUtility.ToInt64(model.ProviderID);
                    if (!string.IsNullOrEmpty(model.FacilityID))
                        dr.FacilityId = MDVUtility.ToInt64(model.FacilityID);
                    if (!string.IsNullOrEmpty(model.PracticeID))
                        dr.PracticeId = MDVUtility.ToInt64(model.PracticeID);
                    dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    dr.BadAddress = false;

                }
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

                if (!string.IsNullOrEmpty(model.GenderIdentityId))
                    dr.GenderIdentityId = MDVUtility.ToInt64(model.GenderIdentityId);
                else
                    dr[dsPatient.Patients.GenderIdentityIdColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.SexualOrientationId))
                    dr.SexualOrientationId = MDVUtility.ToInt64(model.SexualOrientationId);
                else
                    dr[dsPatient.Patients.SexualOrientationIdColumn] = DBNull.Value;
                dr.RaceName = model.PatientRaceIds_text;
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
                //End Change made by Khaleel Ur rehman on 6 May 2016.
                dr.IsActive = MDVUtility.ToInt32(model.Active);
                //dr.IsActive = MDVUtility.ToStr(model.Active) == "True" ? 1 : 0;
                if (dr.IsActive == 0)
                {
                    dr.InActiveReason = model.InactiveReason;
                }
                //if (dr.IsActive == false)
                //{
                //    dr.InActiveReason = model.InactiveReason;
                //}
                else
                    dr.InActiveReason = "";
                dr.Comments = model.Comments;
                dr.PatientPortalStatus = "false";
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.SelfPay = MDVUtility.ToStr(model.SelfPay) == "True" ? true : false;
                dr.PatientPortalStatus = "0";

                if (string.IsNullOrEmpty(model.CountryID))
                    dr[dsPatient.Patients.CountryIDColumn] = DBNull.Value;
                else
                    dr.CountryID = MDVUtility.ToInt64(model.CountryID);

                if (string.IsNullOrEmpty(model.PreferredAddressID))
                    dr[dsPatient.Patients.PreferredAddressIDColumn] = DBNull.Value;
                else
                    dr.PreferredAddressID = MDVUtility.ToInt64(model.PreferredAddressID);

                if (string.IsNullOrEmpty(model.PreferredPhoneID))
                    dr[dsPatient.Patients.PreferredPhoneIDColumn] = DBNull.Value;
                else
                    dr.PreferredPhoneID = MDVUtility.ToInt64(model.PreferredPhoneID);

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
                        referralModel.PatientId = string.IsNullOrEmpty(model.PatientID) == true ? MDVUtility.ToStr(Patient_Id) : model.PatientID;
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
                        string isSaved = SavePatientDocument(Patient_Id, model.imagedata, "image/jpg", model.filename, true, true);
                    }
                    else
                    {
                        if (!model.imgPatient.Contains("default"))
                        {
                            string strBase64 = model.imgPatient.Split(',')[1];
                            string fileType = model.imgPatient.Split(',')[0].Split(':')[1].Split(';')[0];
                            //string isSaved = SavePatientDocument(Patient_Id, model.imagedata, model.filetype, model.filename, model.foldername, model.AssignUserto,model.AssignedToName, model.ScannerDOS, model.ScannerComments);
                            string isSaved = SavePatientDocument(Patient_Id, strBase64, fileType, model.filename, true, true);
                        }

                    }
                    // Start DrFirst changes by babur on 1/14/2016

                    if (isDrFirstRequired == true)
                    {
                        #region Add Patient In DrFirst

                        string PatientID = MDVUtility.ToStr(dsPatient.Tables[dsPatient.Patients.TableName].Rows[0][dsPatient.Patients.PatientIdColumn.ColumnName]);
                        //string ResponseOfDrFirst = SavePatientInDrFirst(dr, PatientID);
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

                    // End DrFirst changes temporary commented by babur on 1/14/2016

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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        // Start DrFirst changes by babur on 1/14/2016
        public string SavePatientDocument(Int64 PatientID, string strBase64 = null, string fileType = null, string FileName = "", Boolean IsScan = false, bool isreviewed = true)
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
                    byte[] currentFileStream = null;
                    if (strBase64.IndexOf(",") > -1)
                    {
                        currentFileStream = Convert.FromBase64String(strBase64.Split(',')[1]);
                    }else
                    {
                        currentFileStream = Convert.FromBase64String(strBase64);
                    }
                    //byte[] currentFileStream = Convert.FromBase64String(strBase64.Split(',')[1]);
                    string dt = DateTime.Now.ToShortDateString();
                    string tm = DateTime.Now.ToShortTimeString();
                    string fileName = dt + " " + tm.Replace(" ", "");
                    fileName = fileName.Replace("/", "-").Replace(":", "");

                    var mnth = fileName.Split('-')[0];
                    var day = fileName.Split('-')[1];
                    var year = fileName.Split('-')[2];
                    mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                    day = day.Length == 1 ? "0" + day : day;
                    var mnt = year.Split(' ')[1];
                    year = year.Split(' ')[0];
                    mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                    //fileName = mnth + "-" + day + "-" + year + " "+ mnt + ".jpg";
                    fileName = mnth + "." + day + "." + year + ".jpg";

                    string FilePath = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Patient ID Card", PatientID, fileName, currentFileStream);
                    dr.Url = FilePath;

                    //dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
                    dr.FileType = fileType;
                    dr.FilePath = Path.GetFileName(FilePath);

                    //if (FileName == "")
                    //    dr.FilePath = Guid.NewGuid().ToString() + "." + fileType.Split('/')[1];//Utility.ToStr(SearchedfieldsJSON["txtDescription"]);
                    //else
                    //    dr.FilePath = FileName + "." + fileType.Split('/')[1];
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
                    BLObject<DSPatient> obj = BLLPatientObj.InsertPatientDocument(dsDocument, isreviewed);
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
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string SavePatientInDrFirst(DSPatient.PatientsRow PatientData, string PatientID)
        {
            int CountTriesANS1 = 0;
            int CountTriesANS = 0;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8080/");
            string error = string.Empty;
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));

            var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>vendorsh1100</VendorName>       <VendorPassword>b2xddjpn</VendorPassword>       </Caller>    <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

            var url = "https://engine301.drfirst.com/servlet/rcopia.servlet.EngineServlet/getURL?xml=" + inputdata;
            HttpResponseMessage response = client.GetAsync(url).Result;
            try
            {
                if (response != null)
                {
                    var getdata = response.Content.ReadAsStringAsync().Result;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(getdata);
                    XmlNodeList nodeListuploadurl = doc.GetElementsByTagName("EngineUploadURL");
                    XmlNodeList nodelistDownloadurl = doc.GetElementsByTagName("EngineDownloadURL");
                    XmlNodeList nodelistWebBrowserURL = doc.GetElementsByTagName("WebBrowserURL");
                    string UploadUrl = string.Empty;
                    string downloadUrl = string.Empty;
                    string WebBrowserURL = string.Empty;
                    foreach (XmlNode node in nodelistWebBrowserURL)
                    {
                        WebBrowserURL = node.InnerText;
                    }
                    foreach (XmlNode node in nodeListuploadurl)
                    {
                        UploadUrl = node.InnerText;
                    }
                    foreach (XmlNode node in nodelistDownloadurl)
                    {
                        downloadUrl = node.InnerText;
                    }
                    DSRcopia dsRcopia = new DSRcopia();
                    BLObject<DSRcopia> obj = BLLRcopiaObj.SelectGetUrls();
                    dsRcopia = obj.Data;
                    if (obj.Data != null)
                    {

                    }
                    string result = string.Empty;
                    var uploadPatient = MDVUtility.GetXmlForAddPatient("vendorsh1100", "b2xddjpn", "vendorsh1100", "lmsp-sh1100", PatientData, PatientID);
                    var Uploadurl = UploadUrl + "?xml=" + HttpContext.Current.Server.UrlEncode(uploadPatient);
                    MDVLogger.RcopiaLogMessage("Request: Send_Patient", PatientID, Uploadurl, "");
                    HttpResponseMessage ResponseUploadPatient = client.GetAsync(Uploadurl).Result;
                    var GetPatientData = ResponseUploadPatient.Content.ReadAsStringAsync().Result;

                    if (GetPatientData == string.Empty)
                    {
                        string errormessage = ResponseUploadPatient.StatusCode.ToString();

                        for (int i = 0; i < 3; i++)
                        {
                            var URLANS = "https://engine301.drfirst.com/servlet/rcopia.servlet.EngineServlet/getURL?xml=" + inputdata;
                            HttpResponseMessage ResponseANS = client.GetAsync(URLANS).Result;
                            if (ResponseANS != null)
                            {
                                var GetdataANS = ResponseANS.Content.ReadAsStringAsync().Result;
                                XmlDocument DocANS = new XmlDocument();
                                DocANS.LoadXml(GetdataANS);
                                XmlNodeList nodeListuploadurlANS = DocANS.GetElementsByTagName("EngineUploadURL");
                                XmlNodeList nodelistDownloadurlANS = DocANS.GetElementsByTagName("EngineDownloadURL");
                                XmlNodeList nodelistWebBrowserURLANS = DocANS.GetElementsByTagName("WebBrowserURL");
                                string UploadUrlANS = string.Empty;
                                string downloadUrlANS = string.Empty;
                                string WebBrowserURLANS = string.Empty;
                                foreach (XmlNode node in nodelistWebBrowserURLANS)
                                {
                                    WebBrowserURLANS = node.InnerText;
                                }
                                foreach (XmlNode node in nodeListuploadurlANS)
                                {
                                    UploadUrlANS = node.InnerText;
                                }
                                foreach (XmlNode node in nodelistDownloadurlANS)
                                {
                                    downloadUrlANS = node.InnerText;
                                }


                                var uploadPatientANS = MDVUtility.GetXmlForAddPatient("vendorsh1100", "b2xddjpn", "vendorsh1100", "lmsp-sh1100", PatientData, PatientID);
                                var UploadurlANS = UploadUrlANS + "?xml=" + HttpContext.Current.Server.UrlEncode(uploadPatientANS);
                                MDVLogger.RcopiaLogMessage("Request: Send_Patient", PatientID, "error", uploadPatient);
                                HttpResponseMessage ResponseUploadPatientANS = client.GetAsync(UploadUrlANS).Result;
                                var GetPatientDataANS = ResponseUploadPatientANS.Content.ReadAsStringAsync().Result;
                                if (GetPatientDataANS == string.Empty)
                                {
                                    CountTriesANS++;
                                    MDVLogger.RcopiaLogMessage("Response: Send_Patient", PatientID, "error", " ", errormessage, CountTriesANS);
                                }
                                else
                                {
                                    XmlDocument Xmldoc = new XmlDocument();
                                    Xmldoc.LoadXml(GetPatientDataANS);


                                    string status = "";
                                    XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");
                                    XmlNodeList statusNodeerror = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status/Error");

                                    foreach (XmlNode node in statusNode)
                                    {
                                        status = node.InnerText;
                                    }
                                    foreach (XmlNode noderror in statusNodeerror)
                                    {
                                        error = noderror.InnerText;
                                    }
                                    MDVLogger.RcopiaLogMessage("Response: Send_Patient", PatientID, GetPatientDataANS, "");
                                    if (status == "ok")
                                    {
                                        string RcopiaID = "";
                                        XmlNodeList RcopialNode = Xmldoc.GetElementsByTagName("RcopiaID");
                                        foreach (XmlNode node in RcopialNode)
                                        {
                                            RcopiaID = node.InnerText;
                                        }
                                        return RcopiaID;
                                    }
                                    else if (status == "error")
                                    {

                                        return "error";

                                    }
                                    break;
                                }


                            }
                        }
                        if (CountTriesANS > 2)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var URLANS = "https://ans501.stage.drfirst.com/getURL?xml=" + inputdata;
                                HttpResponseMessage ResponseANS1 = client.GetAsync(URLANS).Result;
                                if (ResponseANS1 != null)
                                {
                                    var GetdataANS1 = ResponseANS1.Content.ReadAsStringAsync().Result;
                                    XmlDocument DocANS1 = new XmlDocument();
                                    DocANS1.LoadXml(GetdataANS1);
                                    XmlNodeList nodeListuploadurlANS1 = DocANS1.GetElementsByTagName("EngineUploadURL");
                                    XmlNodeList nodelistDownloadurlANS1 = DocANS1.GetElementsByTagName("EngineDownloadURL");
                                    XmlNodeList nodelistWebBrowserURLANS1 = DocANS1.GetElementsByTagName("WebBrowserURL");
                                    string UploadUrlANS1 = string.Empty;
                                    string downloadUrlANS1 = string.Empty;
                                    string WebBrowserURLANS1 = string.Empty;
                                    foreach (XmlNode node in nodelistWebBrowserURLANS1)
                                    {
                                        WebBrowserURLANS1 = node.InnerText;
                                    }
                                    foreach (XmlNode node in nodeListuploadurlANS1)
                                    {
                                        UploadUrlANS1 = node.InnerText;
                                    }
                                    foreach (XmlNode node in nodelistDownloadurlANS1)
                                    {
                                        downloadUrlANS1 = node.InnerText;
                                    }


                                    var uploadPatientANS1 = MDVUtility.GetXmlForAddPatient("vendorsh1100", "b2xddjpn", "vendorsh1100", "lmsp-sh1100", PatientData, PatientID);
                                    var UploadurlANS1 = UploadUrlANS1 + "?xml=" + HttpContext.Current.Server.UrlEncode(uploadPatientANS1);
                                    MDVLogger.RcopiaLogMessage("Request: Send_Patient", PatientID, "error", uploadPatient);
                                    HttpResponseMessage ResponseUploadPatientANS1 = client.GetAsync(UploadUrlANS1).Result;
                                    var GetPatientDataANS1 = ResponseUploadPatientANS1.Content.ReadAsStringAsync().Result;
                                    if (GetPatientDataANS1 == string.Empty)
                                    {
                                        CountTriesANS1++;
                                        MDVLogger.RcopiaLogMessage("Response: Send_Patient", PatientID, "error", " ", errormessage);
                                    }
                                    else
                                    {
                                        XmlDocument Xmldoc = new XmlDocument();
                                        Xmldoc.LoadXml(GetPatientDataANS1);


                                        string status = "";
                                        XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");
                                        XmlNodeList statusNodeerror = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status/Error");

                                        foreach (XmlNode node in statusNode)
                                        {
                                            status = node.InnerText;
                                        }
                                        foreach (XmlNode noderror in statusNodeerror)
                                        {
                                            error = noderror.InnerText;
                                        }
                                        MDVLogger.RcopiaLogMessage("Response: Send_Patient", PatientID, GetPatientDataANS1, "");
                                        if (status == "ok")
                                        {
                                            string RcopiaID = "";
                                            XmlNodeList RcopialNode = Xmldoc.GetElementsByTagName("RcopiaID");
                                            foreach (XmlNode node in RcopialNode)
                                            {
                                                RcopiaID = node.InnerText;
                                            }
                                            return RcopiaID;
                                        }
                                        else if (status == "error")
                                        {

                                            return "error";

                                        }
                                        break;
                                    }


                                }
                            }
                            if (CountTriesANS1 > 2)
                            {
                                DSRcopia dsRcopiaUpdateUrl = new DSRcopia();
                                DSRcopia.Rcopia_GetUrlRow dr = dsRcopia.Rcopia_GetUrl.NewRcopia_GetUrlRow();
                                dr.WebBrowserURL = WebBrowserURL;
                                dr.EngineUploadURL = UploadUrl;
                                dr.EngineDownloadURL = downloadUrl;
                                //dr.IsActive = true;
                                //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                //dr.CreatedOn = DateTime.Now;
                                //dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                //dr.ModifiedOn = DateTime.Now;
                                //BLObject<DSRcopia> dsUpdateUrl =BLLRcopiaObj.UpdateRcopiaUrls(dsRcopia);
                                //dsRcopiaUpdateUrl = dsUpdateUrl.Data;
                                //if (dsUpdateUrl.Data != null)
                                //{

                                //}
                                for (int i = 0; i < 3; i++)
                                {

                                    var URLANS = "https://ans2.drfirst.com/getURL?xml=" + inputdata;
                                    HttpResponseMessage ResponseANS1 = client.GetAsync(URLANS).Result;
                                    if (ResponseANS1 != null)
                                    {
                                        var GetdataANS1 = ResponseANS1.Content.ReadAsStringAsync().Result;
                                        XmlDocument DocANS1 = new XmlDocument();
                                        DocANS1.LoadXml(GetdataANS1);
                                        XmlNodeList nodeListuploadurlANS1 = DocANS1.GetElementsByTagName("EngineUploadURL");
                                        XmlNodeList nodelistDownloadurlANS1 = DocANS1.GetElementsByTagName("EngineDownloadURL");
                                        XmlNodeList nodelistWebBrowserURLANS1 = DocANS1.GetElementsByTagName("WebBrowserURL");
                                        string UploadUrlANS1 = string.Empty;
                                        string downloadUrlANS1 = string.Empty;
                                        string WebBrowserURLANS1 = string.Empty;
                                        foreach (XmlNode node in nodelistWebBrowserURLANS1)
                                        {
                                            WebBrowserURLANS1 = node.InnerText;
                                        }
                                        foreach (XmlNode node in nodeListuploadurlANS1)
                                        {
                                            UploadUrlANS1 = node.InnerText;
                                        }
                                        foreach (XmlNode node in nodelistDownloadurlANS1)
                                        {
                                            downloadUrlANS1 = node.InnerText;
                                        }


                                        var uploadPatientANS1 = MDVUtility.GetXmlForAddPatient("vendorsh1100", "b2xddjpn", "vendorsh1100", "lmsp-sh1100", PatientData, PatientID);
                                        var UploadurlANS1 = UploadUrlANS1 + "?xml=" + HttpContext.Current.Server.UrlEncode(uploadPatientANS1);
                                        MDVLogger.RcopiaLogMessage("Request: Send_Patient", PatientID, "error", uploadPatient);
                                        HttpResponseMessage ResponseUploadPatientANS1 = client.GetAsync(UploadUrlANS1).Result;
                                        var GetPatientDataANS1 = ResponseUploadPatientANS1.Content.ReadAsStringAsync().Result;
                                        if (GetPatientDataANS1 == string.Empty)
                                        {
                                            CountTriesANS1++;
                                            MDVLogger.RcopiaLogMessage("Response: Send_Patient", PatientID, "error", " ", errormessage);
                                        }
                                        else
                                        {
                                            XmlDocument Xmldoc = new XmlDocument();
                                            Xmldoc.LoadXml(GetPatientDataANS1);


                                            string status = "";
                                            XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");
                                            XmlNodeList statusNodeerror = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status/Error");

                                            foreach (XmlNode node in statusNode)
                                            {
                                                status = node.InnerText;
                                            }
                                            foreach (XmlNode noderror in statusNodeerror)
                                            {
                                                error = noderror.InnerText;
                                            }
                                            MDVLogger.RcopiaLogMessage("Response: Send_Patient", PatientID, GetPatientDataANS1, "");
                                            if (status == "ok")
                                            {
                                                string RcopiaID = "";
                                                XmlNodeList RcopialNode = Xmldoc.GetElementsByTagName("RcopiaID");
                                                foreach (XmlNode node in RcopialNode)
                                                {
                                                    RcopiaID = node.InnerText;
                                                }
                                                return RcopiaID;
                                            }
                                            else if (status == "error")
                                            {

                                                return "error";

                                            }
                                            break;
                                        }


                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        XmlDocument Xmldoc = new XmlDocument();
                        Xmldoc.LoadXml(GetPatientData);


                        string status = "";
                        XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");
                        XmlNodeList statusNodeerror = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status/Error");

                        foreach (XmlNode node in statusNode)
                        {
                            status = node.InnerText;
                        }
                        foreach (XmlNode noderror in statusNodeerror)
                        {
                            error = noderror.InnerText;
                        }
                        MDVLogger.RcopiaLogMessage("Response: Send_Patient", PatientID, GetPatientData, "");
                        if (status == "ok")
                        {
                            string RcopiaID = "";
                            XmlNodeList RcopialNode = Xmldoc.GetElementsByTagName("RcopiaID");
                            foreach (XmlNode node in RcopialNode)
                            {
                                RcopiaID = node.InnerText;
                            }
                            return RcopiaID;
                        }
                        else if (status == "error")
                        {

                            return "error";
                            // MDVLogger.RcopiaLogMessage("Response: Send_Patient", PatientID, "error", GetPatientData,error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MDVLogger.RcopiaLogMessage("Response: Send_Patient", PatientID, "error", " ", ex.Message.ToString());

            }
            return "";

        }




        //public List<RcopiaModel> GetRcopiaInfo()
        //{
        //    List<RcopiaModel> RcopiaInfo = new List<RcopiaModel>();
        //    try
        //    {
        //        RcopiaModel model = new RcopiaModel();
        //        DSRcopia dsRcopia = new DSRcopia();
        //        BLObject<DSRcopia> obj =BLLRcopiaObj.SelectSoftwareCustomerInfo(AppConfig.CustomerRegCode);
        //        dsRcopia = obj.Data;
        //        HttpClient client = new HttpClient();
        //        model.RcopiaANSbackup = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaANSbackupColumn.ColumnName]);
        //        model.RcopiaScretkey = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][dsRcopia.SoftwareCustomersInfo.RcopiaScretkeyColumn.ColumnName]);
        //        model.RcopiaVendorUsername = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][12]);
        //        model.RcopiaVendorPassword = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][13]);
        //        model.RcopiaPortalSystemName = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][14]);
        //        model.RcopiaPracticeUserName = MDVUtility.ToStr(dsRcopia.Tables[dsRcopia.SoftwareCustomersInfo.TableName].Rows[0][15]); ;
        //        RcopiaInfo.Add(model);
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return RcopiaInfo;
        //}

        // End DrFirst changes by babur on 1/14/2016

        /// <summary>
        /// Updates the demographic.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        private string UpdateDemographic(string fieldsJSON, int PatientId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (PatientId > 0)
                {
                    DSPatient dsPatient = new DSPatient();
                    //DSPatient.PatientsRow dr = dsPatient.Patients.NewPatientsRow();
                    BLObject<DSPatient> objLoad = BLLPatientObj.FillPatient_PictureById(PatientId, "Demographics");
                    dsPatient = objLoad.Data;
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        DSPatient.PatientsRow dr = (DSPatient.PatientsRow)dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                        //  dr.PatientId = PatientId;
                        //  dr.AccountNumber = MDVUtility.ToStr(SearchedfieldsJSON["txtAccountNo"]);
                        //  dsPatient.Patients.AddPatientsRow(dr);
                        //   dsPatient.AcceptChanges();
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPrefix"]))
                            dr.Prefix = MDVUtility.ToStr(SearchedfieldsJSON["ddlPrefix_text"]);
                        else
                            dr[dsPatient.Patients.PrefixColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSuffix"]))
                            dr.Suffix = MDVUtility.ToStr(SearchedfieldsJSON["ddlSuffix_text"]);
                        else
                            dr[dsPatient.Patients.SuffixColumn] = DBNull.Value;
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
                        //dr.AccountNumber = SearchedfieldsJSON["txtAccountNo"];
                        dr.MRNumber = SearchedfieldsJSON["txtMRN"];
                        dr.SSN = SearchedfieldsJSON["txtSSN"];
                        dr.LastName = SearchedfieldsJSON["txtLastName"];
                        dr.MotherMaidenName = SearchedfieldsJSON["txtMotherMaidenName"];
                        dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                        dr.MI = SearchedfieldsJSON["txtMiddleInitial"];
                        dr.PreviousName = SearchedfieldsJSON["txtPreviousName"];
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["dtpDOB"])))
                            dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                        dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                        dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                        dr.City = SearchedfieldsJSON["txtCity"];
                        dr.State = SearchedfieldsJSON["txtState"];
                        dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                        dr.ZIPCodeExt = SearchedfieldsJSON["txtZipExt"];
                        dr.HomePhoneNo = SearchedfieldsJSON["txtHomeTel"];
                        dr.WorkPhoneNo = SearchedfieldsJSON["txtWorkTel"];
                        dr.WorkPhoneExt = SearchedfieldsJSON["txtExt"];
                        dr.CellNo = SearchedfieldsJSON["txtCell"];
                        dr.FaxNo = SearchedfieldsJSON["txtFax"];
                        //dr = SearchedfieldsJSON["txtInactiveReason"];
                        dr.EmailAddress = SearchedfieldsJSON["txtEmail"];
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfProvider"]))
                            dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["hfProvider"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]))
                            dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["hfFacility"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPractice"]))
                            dr.PracticeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPractice"]);


                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtRefProvider"]))
                            dr.ReferringProviderId = SearchedfieldsJSON["hfRefProvider"];
                        else
                            dr.ReferringProviderId = null;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtPCP"]))
                            dr.PCPId = SearchedfieldsJSON["hfPCP"];
                        else
                            dr.PCPId = null;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtGuarantor"]))
                            dr.GuarantorId = SearchedfieldsJSON["hfGuarantor"];
                        else
                            dr.GuarantorId = null;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["imgPatient"]))
                        {
                            // Checking whether default image is shown or not
                            if (!SearchedfieldsJSON["imgPatient"].Contains("default"))
                            {
                                string strBase64 = SearchedfieldsJSON["imgPatient"].Split(',')[1];
                                strBase64 = strBase64.Replace(' ', '+');
                                byte[] currentFileStream = Convert.FromBase64String(strBase64);
                                //dr.PatientImage = currentFileStream;
                                dr.PatientProfileImagePath = currentFileStream.ToString();
                                dr.ImageType = SearchedfieldsJSON["imgPatient"].Split(',')[0].Split(':')[1].Split(';')[0];
                            }
                        }
                        dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                        dr.BadAddress = MDVUtility.ToStr(SearchedfieldsJSON["chkBadAddress"]) == "True" ? true : false;
                        dr.IsActive = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPatientStatus"]);
                        //dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;


                        if (SearchedfieldsJSON["chkActive"] == false)
                            dr.InActiveReason = SearchedfieldsJSON["txtInactiveReason"];
                        else
                            dr.InActiveReason = "";
                        dr.Comments = SearchedfieldsJSON["txtComments"];
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (SearchedfieldsJSON.ContainsKey("chkSelfPay"))
                        {
                            dr.SelfPay = MDVUtility.ToStr(SearchedfieldsJSON["chkSelfPay"]) == "True" ? true : false;
                        }
                        #region Database Updation

                        // dsPatient.Patients.AcceptChanges();

                        if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                        {
                            //  dsPatient.Patients.Rows[0].SetModified();

                            // Start DrFirst changes by babur on 1/14/2016

                            #region Update Patient In DrFirst
                            dynamic ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("UpdatePatient", dr, PatientId.ToString()));

                            #endregion

                            // End DrFirst changes by babur on 1/14/2016
                            BLObject<DSPatient> obj = BLLPatientObj.UpdatePatient(dsPatient);
                            if (obj.Data != null)
                            {
                                // Start DrFirst changes by babur on 1/14/2016
                                if (ResponseOfDrFirst.Rcopia != "")
                                {
                                    // End DrFirst changes by babur on 1/14/2016
                                    var response = new
                                    {
                                        status = true,
                                        message = Common.AppPrivileges.Update_Message
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                                    // Start DrFirst changes by babur on 1/14/2016

                                }
                                else
                                {
                                    var response = new
                                    {
                                        status = false,
                                        message = "Problem in updating patient on DrFirst"
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                }

                                // DrFirst changes by babur on 1/14/2016
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
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        #endregion
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objLoad.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient not found."
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

        /*start Irfan Patient demographic API implementation*/

        public string UpdateDemographic(PatientDemographicModel model)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                if (MDVUtility.ToInt64(model.PatientID) > 0)
                {
                    DSPatient dsPatient = new DSPatient();
                    BLObject<DSPatient> objLoad = BLLPatientObj.FillPatient_PictureById(MDVUtility.ToInt64(model.PatientID), "Demographics");
                    dsPatient = objLoad.Data;
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        foreach (DSPatient.PatientsRow dr in dsPatient.Tables[dsPatient.Patients.TableName].Rows)
                        {
                            if (!string.IsNullOrEmpty(model.Prefix))
                                dr.Prefix = MDVUtility.ToStr(model.Prefix_text);
                            else
                                dr[dsPatient.Patients.PrefixColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(model.Suffix))
                                dr.Suffix = MDVUtility.ToStr(model.Suffix_text);
                            else
                                dr[dsPatient.Patients.SuffixColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(model.Sex))
                                dr.Gender = MDVUtility.ToStr(model.Sex_text);
                            if (!string.IsNullOrEmpty(model.MaritalStatus))
                                dr.MaritialStatus = MDVUtility.ToStr(model.MaritalStatus_text);

                            dr.strEthnicityIds = model.strEthnicityIds;
                            if (!string.IsNullOrEmpty(model.strEthnicityIds))
                                if (model.strEthnicityIds.Length > 0)
                                    dr.EthnicityId = MDVUtility.ToInt(model.strEthnicityIds.Split(',')[0]);

                            if (!string.IsNullOrEmpty(model.GenderIdentityId))
                                dr.GenderIdentityId = MDVUtility.ToInt64(model.GenderIdentityId);
                            else
                                dr[dsPatient.Patients.GenderIdentityIdColumn] = DBNull.Value;
                            if (!string.IsNullOrEmpty(model.SexualOrientationId))
                                dr.SexualOrientationId = MDVUtility.ToInt64(model.SexualOrientationId);
                            else
                                dr[dsPatient.Patients.SexualOrientationIdColumn] = DBNull.Value;

                            if (!string.IsNullOrEmpty(model.LanguageID))
                                dr.PrefLanguageId = MDVUtility.ToInt(model.LanguageID);
                            dr.MRNumber = model.MRN;
                            dr.SSN = model.SSN;
                            dr.LastName = model.LastName;
                            dr.MotherMaidenName = model.MotherMaidenName;
                            dr.FirstName = model.FirstName;
                            dr.MI = model.MiddleInitial;
                            dr.PreviousName = model.PreviousName;
                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.DOB)))
                                dr.DOB = MDVUtility.ToDateTime(model.DOB);

                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.DischargeDate)))
                                dr.DischargeDate = MDVUtility.ToDateTime(model.DischargeDate);
                            else
                                dr[dsPatient.Patients.DischargeDateColumn] = DBNull.Value;

                            if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.CareGiverIds)))
                                dr.CareGiverIds = model.CareGiverIds;
                            else
                                dr[dsPatient.Patients.CareGiverIdsColumn] = DBNull.Value;

                            dr.Address1 = model.Address1;
                            dr.Address2 = model.Address2;
                            dr.City = model.City;
                            dr.State = model.State;
                            dr.ZIPCode = model.Zip;
                            dr.ZIPCodeExt = model.ZipExt;
                            dr.HomePhoneNo = model.HomeTel;
                            dr.WorkPhoneNo = model.WorkTel;
                            dr.WorkPhoneExt = model.Ext;
                            dr.CellNo = model.Cell;
                            dr.FaxNo = model.Fax;
                            dr.EmailAddress = model.Email;

                            if (dr.strRaceIds != model.strRaceIds)
                            {
                                dr.RaceName = model.PatientRaceIds_text;
                            }

                            dr.strRaceIds = model.strRaceIds;//kr
                            //dr. = model.strEthnicityIds;
                            if (!string.IsNullOrEmpty(model.Provider))
                                dr.ProviderId = MDVUtility.ToInt64(model.ProviderID);
                            if (!string.IsNullOrEmpty(model.Facility))
                                dr.FacilityId = MDVUtility.ToInt64(model.FacilityID);
                            if (!string.IsNullOrEmpty(model.Practice))
                                dr.PracticeId = MDVUtility.ToInt64(model.PracticeID);
                            if (!string.IsNullOrEmpty(model.RefProvider))
                                dr.ReferringProviderId = model.RefProviderID;
                            else
                                dr.ReferringProviderId = null;

                            if (!string.IsNullOrEmpty(model.BirthSex))
                                dr.BirthSex = model.BirthSex;
                            else
                                dr.BirthSex = null;

                            if (!string.IsNullOrEmpty(model.PCP))
                                dr.PCPId = model.PCPID;
                            else
                                dr.PCPId = null;
                            if (!string.IsNullOrEmpty(model.Guarantor))
                                dr.GuarantorId = model.GuarantorID;
                            else
                                dr.GuarantorId = null;
                            if (model.IsImageUpdated == "True")
                            {
                                if (!string.IsNullOrEmpty(model.imgPatient))
                                {

                                    if (!model.imgPatient.Contains("default"))
                                    {
                                        string sessiomImagestrBase64 = string.Empty;
                                        string ServerPathForLoadFile = string.Empty;

                                        string base64String2 = string.Empty;

                                        if (MDVSession.Current.ImageID != "")
                                        {
                                            string lfileName = MDVUtility.ToStr(MDVSession.Current.ImageID.Split('|')[0]);
                                            ServerPathForLoadFile = System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesPath"];

                                            string imgPath = System.IO.Path.Combine(ServerPathForLoadFile, lfileName);

                                            if (System.IO.File.Exists(imgPath))
                                            {
                                                byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                                                sessiomImagestrBase64 = Convert.ToBase64String(imageBytes);
                                            }

                                        }

                                        string strBase64 = model.imgPatient.Split(',')[1];
                                        /*  byte[] currentFileStream = Convert.FromBase64String(strBase64)*/
                                        ;

                                        //dr.PatientImage = currentFileStream;
                                        //dr.PatientProfileImagePath = currentFileStream.ToString();

                                        //byte[] PatientImageThumbnailStream = Convert.FromBase64String(MDVUtility.ResizeImage(strBase64, 100, 100));
                                        //dr.PatientImageThumbnail = PatientImageThumbnailStream;

                                        //e.g,  model.imgPatient= data:image/jpeg;base64,/9j/4AA..........
                                        //   if (!strBase64.Equals(sessiomImagestrBase64))
                                        // {
                                        dr.ImageType = model.imgPatient.Split(',')[0].Split(':')[1].Split(';')[0];

                                        string filetype = model.imgPatient.Split(',')[0].Split(':')[1].Split(';')[0].Split('/')[1];

                                        string FileName = model.LastName + '-' + model.FirstName + '-' + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                        PatientDemographicImageSaveInFolder(strBase64, FileName, filetype);

                                        dr.PatientProfileImagePath = FileName + "." + filetype;
                                        dr.PatientProfileThumbnailPath = FileName + "-th." + filetype;

                                        //   }




                                    }
                                    else
                                    {
                                        //dr.PatientImage = null;
                                        //dr.PatientImageThumbnail = null;
                                        dr.PatientProfileImagePath = null;
                                        dr.PatientProfileThumbnailPath = null;
                                        dr.ImageType = "";
                                        dr.PatientImage = null;
                                    }
                                }
                            }

                            dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                            dr.BadAddress = MDVUtility.ToStr(model.BadAddress) == "True" ? true : false;
                            dr.IsTCM = MDVUtility.ToStr(model.TransitionalCareManagement) == "True" ? true : false;
                            dr.IsActive = MDVUtility.ToInt32(model.Active);
                            if (MDVUtility.ToInt32(model.Active) == 0)
                                dr.InActiveReason = model.InactiveReason;
                            else
                                dr.InActiveReason = "";
                            dr.Comments = model.Comments;
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;

                            dr.SelfPay = MDVUtility.ToStr(model.SelfPay) == "True" ? true : false;

                            //------------Save Value in RaceId as instructed by Babur sb-----
                            //Start Change made by Khaleel Ur rehman on 6 May 2016.
                            if (!string.IsNullOrEmpty(model.strRaceIds))
                            {
                                if (model.strRaceIds.Length > 0)
                                {
                                    dr.RaceId = MDVUtility.ToInt(model.strRaceIds.Split(',')[0]);
                                }
                            }
                            //Start 23-08-2016 Humaira Yousaf for hear from info
                            if (!string.IsNullOrEmpty(model.HearFromId))
                            {
                                dr.HearFromId = MDVUtility.ToInt32(model.HearFromId);
                                if (MDVUtility.ToInt32(model.HearFromId) == 10 && !string.IsNullOrEmpty(model.HearFromOther))
                                {
                                    dr.HearFromOther = model.HearFromOther;
                                }
                            }
                            else
                            {
                                dr.HearFromOther = null;
                                dr[dsPatient.Patients.HearFromIdColumn] = DBNull.Value;
                            }
                            //End 23-08-2016 Humaira Yousaf for hear from info
                            //End Change made by Khaleel Ur rehman on 6 May 2016.
                            if (string.IsNullOrEmpty(model.DateOfDeath))
                                dr[dsPatient.Patients.DODColumn] = DBNull.Value;
                            else
                                dr.DOD = MDVUtility.ToDateTime(model.DateOfDeath);
                            if (string.IsNullOrEmpty(model.CauseOfDeath))
                                dr[dsPatient.Patients.CauseOfDeathColumn] = DBNull.Value;
                            else
                                dr.CauseOfDeath = model.CauseOfDeath;

                            if (string.IsNullOrEmpty(model.CountryID))
                                dr[dsPatient.Patients.CountryIDColumn] = DBNull.Value;
                            else
                                dr.CountryID = MDVUtility.ToInt64(model.CountryID);

                            if (string.IsNullOrEmpty(model.PreferredAddressID))
                                dr[dsPatient.Patients.PreferredAddressIDColumn] = DBNull.Value;
                            else
                                dr.PreferredAddressID = MDVUtility.ToInt64(model.PreferredAddressID);

                            if (string.IsNullOrEmpty(model.PreferredPhoneID))
                                dr[dsPatient.Patients.PreferredPhoneIDColumn] = DBNull.Value;
                            else
                                dr.PreferredPhoneID = MDVUtility.ToInt64(model.PreferredPhoneID);
                        }
                        #region Database Updation

                        if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                        {
                            dynamic ResponseOfDrFirst = null;
                            // Start DrFirst changes by babur on 1/14/2016
                            if (isDrFirstRequired == true)
                            {
                                #region Update Patient In DrFirst

                                DSPatient.PatientsRow dr = (DSPatient.PatientsRow)dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];

                                ResponseOfDrFirst = JObject.Parse(BLLRcopiaObj.getRcopiaResponseUrl("UpdatePatient", dr, model.PatientID));


                                dsPatient.Patients.Rows[0][dsPatient.Patients.RcopiaIDColumn.ColumnName] = ResponseOfDrFirst.Rcopia;

                                #endregion
                            }

                            // End DrFirst changes by babur on 1/14/2016


                            if (!string.IsNullOrEmpty(model.imagedata))
                            {
                                if (!string.IsNullOrEmpty(model.IsImageUpdated) && MDVUtility.ToBool(model.IsImageUpdated)!=false) {
                                    Int64 patientID = Convert.ToInt64(model.PatientID);
                                    //string isSaved = SavePatientDocument(Patient_Id, model.imagedata, model.filetype, model.filename, model.foldername, model.AssignUserto,model.AssignedToName, model.ScannerDOS, model.ScannerComments);
                                    string isSaved = SavePatientDocument(patientID, model.imagedata, model.filetype, model.filename, true);
                                }
                               
                            }
                            else
                            {
                                if (model.IsImageUpdated.Equals("True"))
                                {
                                    if (!model.imgPatient.Contains("default"))
                                    {
                                        Int64 patientID = Convert.ToInt64(model.PatientID);
                                        string strBase64 = model.imgPatient.Split(',')[1];
                                        string fileType = model.imgPatient.Split(',')[0].Split(':')[1].Split(';')[0];
                                        //string isSaved = SavePatientDocument(Patient_Id, model.imagedata, model.filetype, model.filename, model.foldername, model.AssignUserto,model.AssignedToName, model.ScannerDOS, model.ScannerComments);
                                        string isSaved = SavePatientDocument(patientID, strBase64, fileType, model.filename, true);
                                    }
                                }
                            }

                            BLObject<DSPatient> obj = BLLPatientObj.UpdatePatient(dsPatient);
                            if (obj.Data != null)
                            {
                                if (model.HearFromId == "1")
                                {
                                    if (string.IsNullOrWhiteSpace(model.ReferralId))
                                    {
                                        model.ReferralId = "-1";
                                    }
                                    PatientReferralModel referralModel = new PatientReferralModel();
                                    referralModel.PatientId = model.PatientID;
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

                                if (isDrFirstRequired == false)
                                {
                                    var response = new
                                    {
                                        status = true,
                                        message = Common.AppPrivileges.Update_Message
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                }
                                else
                                {
                                    // Start DrFirst changes by babur on 1/14/2016
                                    if (ResponseOfDrFirst.Rcopia != "")
                                    {
                                        // End DrFirst changes by babur on 1/14/2016

                                        var response = new
                                        {
                                            status = true,
                                            message = Common.AppPrivileges.Update_Message
                                        };
                                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                                        // Start DrFirst changes by babur on 1/14/2016

                                    }
                                    else
                                    {
                                        var response = new
                                        {
                                            status = false,
                                            message = "Problem in updating patient on DrFirst"
                                        };
                                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                    }

                                    // End DrFirst changes by babur on 1/14/2016
                                }
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
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        #endregion
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objLoad.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient not found."
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

        public string UpdateDemographicPic(PatientDemographicModel model, string PatientDocumentImage)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                if (MDVUtility.ToInt64(model.PatientID) > 0)
                {
                    DSPatient dsPatient = new DSPatient();
                    BLObject<DSPatient> objLoad = BLLPatientObj.FillPatient_PictureById(MDVUtility.ToInt64(model.PatientID), "Demographics");
                    dsPatient = objLoad.Data;
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        foreach (DSPatient.PatientsRow dr in dsPatient.Tables[dsPatient.Patients.TableName].Rows)
                        {
                            if (!string.IsNullOrEmpty(model.imgPatient))
                            {
                                string filetype = string.Empty;

                                string PatientProfileImagePath = string.Empty;
                                string PatientProfileThumbnailPath = string.Empty;

                                if (!model.imgPatient.Contains("default"))
                                {
                                    string strBase64 = model.imgPatient.Split(',')[1];

                                    if (!string.IsNullOrWhiteSpace(strBase64))
                                    {
                                        byte[] currentFileStream = Convert.FromBase64String(strBase64);
                                        dr.PatientImage = currentFileStream;

                                        byte[] PatientImageThumbnailStream = Convert.FromBase64String(MDVUtility.ResizeImage(strBase64, 100, 100));

                                        //dr.PatientImageThumbnail = PatientImageThumbnailStream;

                                        dr.ImageType = model.imgPatient.Split(',')[0].Split(':')[1].Split(';')[0];
                                        filetype = model.imgPatient.Split(',')[0].Split(':')[1].Split(';')[0].Split('/')[1];

                                        PatientProfileImagePath = model.LastName + '-' + model.FirstName + '-' + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                        PatientDemographicImageSaveInFolder(strBase64, PatientProfileImagePath, filetype);

                                        dr.PatientProfileImagePath = PatientProfileImagePath + "." + filetype;
                                        dr.PatientProfileThumbnailPath = PatientProfileImagePath + "-th." + filetype;
                                    }


                                }
                                else
                                {
                                    //dr.PatientImage = null;
                                    //dr.PatientImageThumbnail = null;
                                    dr.PatientProfileImagePath = null;
                                    dr.PatientProfileThumbnailPath = null;
                                    dr.ImageType = "";
                                    dr.PatientImage = null;
                                }
                            }
                            dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }
                        #region Database Updation

                        if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                        {
                            byte[] DocumentFileStream = null;
                            if (!string.IsNullOrWhiteSpace(PatientDocumentImage))
                            {
                                DocumentFileStream = Convert.FromBase64String(PatientDocumentImage.Split(',')[1]);
                            }
                            string FilePath = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Patient ID Card", MDVUtility.ToInt64(model.PatientID), "Patient ID Card.jpg", DocumentFileStream);
                            BLObject<DSPatient> obj = BLLPatientObj.UpdatePatientPic(dsPatient, FilePath);
                            if (obj.Data != null)
                            {
                                var response = new
                                {
                                    status = true,
                                    message = Common.AppPrivileges.Update_Message
                                };
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
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        #endregion
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objLoad.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Patient not found."
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
        public string FillPatientDetailForCustomForms(PatientDemographicModel request_model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(request_model.PatientID)) && MDVUtility.ToLong(request_model.PatientID) > 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = "Something went wrong !!"
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    PatientDemographicModel response_model = new PatientDemographicModel();
                    response_model = BLLPatientObj.GetPatientDemographicsDetailsForCustomForms(MDVUtility.ToLong(request_model.PatientID));
                    var response = new
                    {
                        status = true,
                        DemographicFill_JSON = js.Serialize(response_model),
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

        public string FillPatient(PatientDemographicModel request_model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(request_model.PatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSPatient dsPatient = null;
                    PatientDemographicModel response_model = new PatientDemographicModel();
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    BLObject<DSPatient> obj = BLLPatientObj.FillPatient_PictureById(MDVUtility.ToInt64(request_model.PatientID), "Demographics");
                    dsPatient = obj.Data;
                    dsPatient.Merge(BLLPatientObj.GetRaceCodes(MDVUtility.ToLong(request_model.PatientID)).Data);
                    response_model.PatientPHQScore = BLLCQMObj.loadPatientPHQScore_VBP(MDVUtility.ToInt64(request_model.PatientID));

                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];

                        string ConfidentialyCode = MDVUtility.ToStr(dr[dsPatient.Patients.ConfidentialityCodeColumn]);
                        if (ConfidentialyCode == "R")
                        {

                            string IsDataPrivacy = GetIsDataPrivacy();

                            if (IsDataPrivacy == "False")
                            {
                                var ReturnResponse = new
                                {
                                    status = false,
                                    Message = MDVUtility.ToStr("You are not authorized to access the data")
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(ReturnResponse);
                            }

                        }
                        string AgeResponse = GetAge(MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]));
                        string ImagePath = "";

                        if (dr[dsPatient.Patients.ImagePathColumn.ColumnName].ToString() != "")
                            ImagePath = dr[dsPatient.Patients.ImagePathColumn.ColumnName].ToString();

                        string Format = "{0:0.00}";
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        ser.MaxJsonLength = Int32.MaxValue;
                        var SearchedfieldsJSON = ser.Deserialize<dynamic>(AgeResponse);

                        if (dsPatient.PatientRace.Rows.Count > 0)
                        {
                            foreach (DataRow dr_race in dsPatient.PatientRace.Rows)
                            {
                                PatientRacesModel obj_ = new PatientRacesModel();
                                obj_.Name = MDVUtility.ToStr(dr_race[dsPatient.PatientRace.DescriptionColumn.ColumnName]);
                                obj_.Id = MDVUtility.ToStr(dr_race[dsPatient.PatientRace.IdColumn.ColumnName]);
                                response_model.PatientRaces.Add(obj_);
                            }
                        }

                        response_model.AccountNo = MDVUtility.ToStr(dr[dsPatient.Patients.AccountNumberColumn.ColumnName]);
                        response_model.ClaimFlagDescription = MDVUtility.ToStr(dr[dsPatient.Patients.CFDescriptionColumn.ColumnName]);
                        response_model.MRN = MDVUtility.ToStr(dr[dsPatient.Patients.MRNumberColumn.ColumnName]);
                        response_model.SSN = MDVUtility.ToStr(dr[dsPatient.Patients.SSNColumn.ColumnName]);
                        response_model.Prefix = MDVUtility.ToStr(dr[dsPatient.Patients.PrefixColumn.ColumnName]);
                        response_model.Prefix_text = MDVUtility.ToStr(dr[dsPatient.Patients.PrefixColumn.ColumnName]);
                        response_model.Suffix = MDVUtility.ToStr(dr[dsPatient.Patients.SuffixColumn.ColumnName]);
                        response_model.Suffix_text = MDVUtility.ToStr(dr[dsPatient.Patients.SuffixColumn.ColumnName]);
                        response_model.LastName = MDVUtility.ToStr(dr[dsPatient.Patients.LastNameColumn.ColumnName]);
                        response_model.MotherMaidenName = MDVUtility.ToStr(dr[dsPatient.Patients.MotherMaidenNameColumn.ColumnName]);
                        response_model.FirstName = MDVUtility.ToStr(dr[dsPatient.Patients.FirstNameColumn.ColumnName]);
                        response_model.FullName = MDVUtility.ToStr(dr[dsPatient.Patients.FullNameColumn.ColumnName]);
                        response_model.MiddleInitial = MDVUtility.ToStr(dr[dsPatient.Patients.MIColumn.ColumnName]);
                        response_model.PreviousName = MDVUtility.ToStr(dr[dsPatient.Patients.PreviousNameColumn.ColumnName]);
                        response_model.DOB = string.IsNullOrEmpty(dr[dsPatient.PatientInsurance.DOBColumn.ColumnName].ToString()) ? "" : MDVUtility.GetDateMMDDYYY(dr[dsPatient.PatientInsurance.DOBColumn.ColumnName].ToString());
                        response_model.DischargeDate = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.Patients.DischargeDateColumn.ColumnName])) ? "" : MDVUtility.GetDateMMDDYYY(dr[dsPatient.Patients.DischargeDateColumn.ColumnName].ToString());
                        response_model.Age = MDVUtility.ToStr(SearchedfieldsJSON["ActualAge"]);
                        response_model.Sex = MDVUtility.ToStr(dr[dsPatient.Patients.GenderColumn.ColumnName]);
                        response_model.Sex_text = MDVUtility.ToStr(dr[dsPatient.Patients.GenderColumn.ColumnName]);
                        response_model.MaritalStatus = MDVUtility.ToStr(dr[dsPatient.Patients.MaritialStatusColumn.ColumnName]);
                        response_model.MaritalStatus_text = MDVUtility.ToStr(dr[dsPatient.Patients.MaritialStatusColumn.ColumnName]);
                        response_model.Ethnicity = MDVUtility.ToStr(dr[dsPatient.Patients.EthnicityIdColumn.ColumnName]);
                        response_model.Race = MDVUtility.ToStr(dr[dsPatient.Patients.RaceIdColumn.ColumnName]);
                        response_model.LanguageID = MDVUtility.ToStr(dr[dsPatient.Patients.PrefLanguageIdColumn.ColumnName]);
                        response_model.PrefLanguage = MDVUtility.ToStr(dr[dsPatient.Patients.LanguageNameColumn.ColumnName]);
                        response_model.Active = MDVUtility.ToStr(dr[dsPatient.Patients.IsActiveColumn.ColumnName]);
                        response_model.InactiveReason = MDVUtility.ToStr(dr[dsPatient.Patients.InActiveReasonColumn.ColumnName]);
                        response_model.Comments = MDVUtility.ToStr(dr[dsPatient.Patients.CommentsColumn.ColumnName]);
                        response_model.Address1 = MDVUtility.ToStr(dr[dsPatient.Patients.Address1Column.ColumnName]);
                        response_model.Address2 = MDVUtility.ToStr(dr[dsPatient.Patients.Address2Column.ColumnName]);
                        response_model.City = MDVUtility.ToStr(dr[dsPatient.Patients.CityColumn.ColumnName]);
                        response_model.State = MDVUtility.ToStr(dr[dsPatient.Patients.StateColumn.ColumnName]);
                        response_model.Zip = MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeColumn.ColumnName]);
                        response_model.ZipExt = MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeExtColumn.ColumnName]);
                        response_model.HomeTel = MDVUtility.ToStr(dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName]);
                        response_model.WorkTel = MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneNoColumn.ColumnName]);
                        response_model.Ext = MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneExtColumn.ColumnName]);
                        response_model.Cell = MDVUtility.ToStr(dr[dsPatient.Patients.CellNoColumn.ColumnName]);
                        response_model.Fax = MDVUtility.ToStr(dr[dsPatient.Patients.FaxNoColumn.ColumnName]);
                        response_model.Email = MDVUtility.ToStr(dr[dsPatient.Patients.EmailAddressColumn.ColumnName]);
                        response_model.BadAddress = MDVUtility.ToStr(dr[dsPatient.Patients.BadAddressColumn.ColumnName]);
                        response_model.Provider = MDVUtility.ToStr(dr[dsPatient.Patients.ProviderNameColumn.ColumnName]);
                        response_model.ProviderID = MDVUtility.ToStr(dr[dsPatient.Patients.ProviderIdColumn.ColumnName]);
                        response_model.InsurancePlanId = MDVUtility.ToStr(dr[dsPatient.Patients.InsurancePlanIdColumn.ColumnName]);
                        response_model.Facility = MDVUtility.ToStr(dr[dsPatient.Patients.FacilityNameColumn.ColumnName]);
                        response_model.FacilityID = MDVUtility.ToStr(dr[dsPatient.Patients.FacilityIdColumn.ColumnName]);
                        response_model.Practice = MDVUtility.ToStr(dr[dsPatient.Patients.PracticeNameColumn.ColumnName]);
                        response_model.PracticeID = MDVUtility.ToStr(dr[dsPatient.Patients.PracticeIdColumn.ColumnName]);
                        response_model.RefProvider = MDVUtility.ToStr(dr[dsPatient.Patients.ReferringProviderNameColumn.ColumnName]);
                        response_model.RefProviderID = MDVUtility.ToStr(dr[dsPatient.Patients.ReferringProviderIdColumn.ColumnName]);
                        response_model.PCP = MDVUtility.ToStr(dr[dsPatient.Patients.PCPNameColumn.ColumnName]);
                        response_model.PCPID = MDVUtility.ToStr(dr[dsPatient.Patients.PCPIdColumn.ColumnName]);
                        response_model.Guarantor = MDVUtility.ToStr(dr[dsPatient.Patients.GuarantorNameColumn.ColumnName]);
                        response_model.GuarantorID = MDVUtility.ToStr(dr[dsPatient.Patients.GuarantorIdColumn.ColumnName]);
                        response_model.PatientBalance = String.Format(Format, dr[dsPatient.Patients.PatientBalanceColumn.ColumnName]);
                        response_model.InsuranceBalance = String.Format(Format, dr[dsPatient.Patients.InsuranceBalanceColumn.ColumnName]);
                        response_model.AdvanceBalance = String.Format(Format, dr[dsPatient.Patients.AdvanceBalanceColumn.ColumnName]);
                        response_model.CollectionBalance = String.Format(Format, dr[dsPatient.Patients.CollectionBalanceColumn.ColumnName]);
                        response_model.PatientPortalStatus = String.Format(Format, dr[dsPatient.Patients.PatientPortalStatusColumn.ColumnName]);
                        response_model.SelfPay = MDVUtility.ToStr(dr[dsPatient.Patients.SelfPayColumn.ColumnName]);
                        response_model.TransitionalCareManagement = MDVUtility.ToStr(dr[dsPatient.Patients.IsTCMColumn.ColumnName]);
                        response_model.InsuranceName = MDVUtility.ToStr(dr[dsPatient.Patients.InsuranceNameColumn.ColumnName]);
                        response_model.PatDocId = MDVUtility.ToStr(dr[dsPatient.Patients.PatDocIdColumn.ColumnName]);
                        response_model.Image_url = ImagePath;
                        response_model.strRaceIds = MDVUtility.ToStr(dr[dsPatient.Patients.strRaceIdsColumn.ColumnName]);//kr
                        response_model.strEthnicityIds = MDVUtility.ToStr(dr[dsPatient.Patients.strEthnicityIdsColumn.ColumnName]);
                        response_model.SexualOrientationId = MDVUtility.ToStr(dr[dsPatient.Patients.SexualOrientationIdColumn.ColumnName]);
                        response_model.GenderIdentityId = MDVUtility.ToStr(dr[dsPatient.Patients.GenderIdentityIdColumn.ColumnName]);
                        response_model.Scan = MDVUtility.ToStr(dr[dsPatient.Patients.ScanColumn.ColumnName]);//kr
                        response_model.OCR = MDVUtility.ToStr(dr[dsPatient.Patients.OCRColumn.ColumnName]);//kr
                        response_model.PrefCommunicationId = MDVUtility.ToStr(dr[dsPatient.Patients.PrefCommunicationIdColumn.ColumnName]);
                        response_model.ScndPrefCommunicationId = MDVUtility.ToStr(dr[dsPatient.Patients.ScndPrefCommunicationIdColumn.ColumnName]);
                        response_model.HearFromId = MDVUtility.ToStr(dr[dsPatient.Patients.HearFromIdColumn.ColumnName]);
                        response_model.HearFromOther = MDVUtility.ToStr(dr[dsPatient.Patients.HearFromOtherColumn.ColumnName]);
                        response_model.CommunicateWithGurantor = MDVUtility.ToStr(dr[dsPatient.Patients.CommunicatewithGuarantorColumn]);
                        //-------------------
                        response_model.ProviderFName = MDVUtility.ToStr(dr[dsPatient.Patients.ProviderFirstNameColumn]);
                        response_model.ProviderLName = MDVUtility.ToStr(dr[dsPatient.Patients.ProviderLastNameColumn]);
                        response_model.FacilityAddress = MDVUtility.ToStr(dr[dsPatient.Patients.FacilityAddressColumn]);
                        response_model.FacilityCity = MDVUtility.ToStr(dr[dsPatient.Patients.FacilityCityColumn]);
                        response_model.FacilityState = MDVUtility.ToStr(dr[dsPatient.Patients.FacilityStateColumn]);
                        response_model.FacilityZip = MDVUtility.ToStr(dr[dsPatient.Patients.FacilityZipColumn]);
                        response_model.FacilityZipExt = MDVUtility.ToStr(dr[dsPatient.Patients.FacilityZipExtColumn]);
                        response_model.FacilityPhone = MDVUtility.ToStr(dr[dsPatient.Patients.FacilityPhoneColumn]);
                        response_model.PatientInsuranceId = MDVUtility.ToStr(dr[dsPatient.Patients.PatientInsuranceIdColumn]);
                        response_model.DateOfDeath = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.Patients.DODColumn.ColumnName])) ? "" : MDVUtility.GetDateMMDDYYY(dr[dsPatient.Patients.DODColumn.ColumnName].ToString());
                        response_model.CauseOfDeath = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.Patients.CauseOfDeathColumn.ColumnName])) ? "" : MDVUtility.ToStr(dr[dsPatient.Patients.CauseOfDeathColumn.ColumnName]);
                        // MU3-16 Faizan Ameen  added confidentiality code.
                        response_model.ConfidentialityCode = MDVUtility.ToStr(dr[dsPatient.Patients.ConfidentialityCodeColumn]);
                        response_model.BirthSex = MDVUtility.ToStr(dr[dsPatient.Patients.BirthSexColumn]);
                        // End Of code added by faizan Ameen..
                        //CCM BUTTON WILL ONLY BE SHOWN TO THE USERS WHO HAVE CCM PRIVILIGES
                        string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "VIEW", "Dash Board")).ToString();
                        if (string.IsNullOrEmpty(privilegasMessage))
                        {
                            response_model.CCMStatus = MDVUtility.ToStr(dr[dsPatient.Patients.CCMStatusColumn]);
                            response_model.EnrollmentInfoId = MDVUtility.ToStr(dr[dsPatient.Patients.EnrollmentInfoIdColumn]);
                        }

                        response_model.CountryID = MDVUtility.ToStr(dr[dsPatient.Patients.CountryIDColumn]);
                        response_model.Country = MDVUtility.ToStr(dr[dsPatient.Patients.CountryColumn]);
                        response_model.PreferredAddressID = MDVUtility.ToStr(dr[dsPatient.Patients.PreferredAddressIDColumn]);
                        response_model.PreferredPhoneID = MDVUtility.ToStr(dr[dsPatient.Patients.PreferredPhoneIDColumn]);
                        //--------------------
                        response_model.CityId = MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeColumn.ColumnName]);

                        //string imageBase64 = "";
                        //byte[] imageByteArr = dr[dsPatient.Patients.PatientImageColumn.ColumnName] as byte[];
                        //if (imageByteArr != null)
                        //{
                        //    imageBase64 = "data:" + dr[dsPatient.Patients.ImageTypeColumn.ColumnName] + ";base64," + Convert.ToBase64String(dr[dsPatient.Patients.PatientImageColumn.ColumnName] as byte[]);
                        //}
                        bool lByteImgPath = true;
                        string imageBase64 = "";

                        //byte[] imageByteArr = dr[dsPatient.Patients.PatientImageColumn.ColumnName] as byte[];

                        string filetype = dr[dsPatient.Patients.ImageTypeColumn.ColumnName] as string;
                        //string strBase64 = Convert.ToBase64String(dr[dsPatient.Patients.PatientImageColumn.ColumnName] as byte[]);
                        //string FileName = string.Empty;

                        response_model.PatientProfileImagePath = MDVUtility.ToStr(dr[dsPatient.Patients.PatientProfileImagePathColumn]);
                        response_model.PatientProfileThumbnailPath = MDVUtility.ToStr(dr[dsPatient.Patients.PatientProfileThumbnailPathColumn]);
                        response_model.MissingDataAlertsCount = MDVUtility.ToInt64(dr[dsPatient.Patients.MissingDataAlertsCountColumn]) != 0 ? MDVUtility.ToInt64(dr[dsPatient.Patients.MissingDataAlertsCountColumn]) : 0;
                        response_model.CareGiverIds = MDVUtility.ToStr(dr[dsPatient.Patients.CareGiverIdsColumn]);
                        string lfileName = MDVUtility.ToStr(dr[dsPatient.Patients.PatientProfileImagePathColumn]);
                        string lfileNameThumbnail = MDVUtility.ToStr(dr[dsPatient.Patients.PatientProfileThumbnailPathColumn]);


                        string ServerPathForLoadFile = System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesPath"];

                        if (System.IO.Directory.Exists(ServerPathForLoadFile))
                        {
                            if (!lfileName.Equals(""))
                            {

                                MDVSession.Current.ImageID = lfileName + "|" + lfileNameThumbnail;


                                //byte[] imageBytes = System.IO.File.ReadAllBytes(System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesDetails"] + "\\" + lfileName);


                                string imgPath = System.IO.Path.Combine(ServerPathForLoadFile, lfileName);

                                if (System.IO.File.Exists(imgPath))
                                {
                                    byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                                    //byte[] imageBytes = Convert.FromBase64String(lfileName);

                                    string base64String = Convert.ToBase64String(imageBytes);

                                    //string imgBase64String = GetBase64StringForImage(ServerPathForLoadFile);
                                    imageBase64 = "data:" + dr[dsPatient.Patients.ImageTypeColumn.ColumnName] + ";base64," + base64String;
                                    lByteImgPath = false;
                                }


                            }
                        }

                        byte[] imageByteArr = dr[dsPatient.Patients.PatientImageColumn.ColumnName] as byte[];
                        if (lByteImgPath && imageByteArr != null)
                        {
                            imageBase64 = "data:" + dr[dsPatient.Patients.ImageTypeColumn.ColumnName] + ";base64," + Convert.ToBase64String(dr[dsPatient.Patients.PatientImageColumn.ColumnName] as byte[]);
                        }

                        if (!String.IsNullOrEmpty(imageBase64))
                            response_model.imgPatient = imageBase64;
                       // PMS - 4597
                        var serializer = new JavaScriptSerializer();
                        serializer.MaxJsonLength = Int32.MaxValue;

                        var response = new
                        {
                            status = true,
                            DemographicFill_JSON = serializer.Serialize(response_model),
                            //  ReferralJSON = js.Serialize(ReferralData),
                            //Image_url = ImagePath  != "" ? string.Format("{0}{1}", HttpRuntime.AppDomainAppVirtualPath, ImagePath):ImagePath,
                            //Image_url = imageBase64,
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DemographicFill_JSON = js.Serialize(response_model),
                            Image_url = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
        /*end Irfan Patient demographic new implementation*/

        // Start DrFirst changes by babur on 1/14/2016
        private string GetIsDataPrivacy()
        {
            try
            {

                string Result = BLLPatientObj.GetIsDataPrivacy();

                return Result;

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
        private string UpdatePatientInDrFirst(DSPatient.PatientsRow PatientData)
        {
            HttpClient client = new HttpClient();
            //    client.BaseAddress = new Uri("http://localhost:8080/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));

            var inputdata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest> <Caller>             <VendorName>vendorsh1100</VendorName>       <VendorPassword>b2xddjpn</VendorPassword>       </Caller>    <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername> <Request>            <Command>get_url</Command> </Request></RCExtRequest>";

            var url = "https://engine301.drfirst.com/servlet/rcopia.servlet.EngineServlet/getURL?xml=" + inputdata;

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response != null)
            {
                var getdata = response.Content.ReadAsStringAsync().Result;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(getdata);
                //string jsonText = JsonConvert.SerializeXmlNode(doc);
                XmlNodeList nodeListuploadurl = doc.GetElementsByTagName("EngineUploadURL");
                XmlNodeList nodelistDownloadurl = doc.GetElementsByTagName("EngineDownloadURL");

                string UploadUrl = string.Empty;
                string downloadUrl = string.Empty;
                foreach (XmlNode node in nodeListuploadurl)
                {
                    UploadUrl = node.InnerText;
                }
                foreach (XmlNode node in nodelistDownloadurl)
                {
                    downloadUrl = node.InnerText;
                }

                //string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername,DSPatient.PatientsRow patient
                var updatePatientXml = MDVUtility.GetXmlForUpdatePatient("vendorsh1100", "b2xddjpn", "vendorsh1100", "lmsp-sh1100", PatientData);
                var Uploadurl = UploadUrl + "?xml=" + updatePatientXml;
                HttpResponseMessage ResponseUpdatePatient = client.GetAsync(Uploadurl).Result;
                var GetPatientUpdateData = ResponseUpdatePatient.Content.ReadAsStringAsync().Result;

                XmlDocument Xmldoc = new XmlDocument();
                Xmldoc.LoadXml(GetPatientUpdateData);

                string status = "";
                XmlNodeList statusNode = Xmldoc.DocumentElement.SelectNodes("/RCExtResponse/Response/Status");


                foreach (XmlNode node in statusNode)
                {
                    status = node.InnerText;
                }
                if (status == "ok")
                {
                    return "ok";
                }
                else if (status == "error")
                {
                    return "error";
                }
            }
            return "";

        }

        // End DrFirst changes by babur on 1/14/2016

        /// <summary>
        /// Fills the patient.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        private string FillPatient(Int64 PatientId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj = BLLPatientObj.FillPatient_PictureById(PatientId, "Demographics");
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                        string AgeResponse = GetAge(MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]));
                        string ImagePath = "";

                        if (dr[dsPatient.Patients.ImagePathColumn.ColumnName].ToString() != "")
                            ImagePath = dr[dsPatient.Patients.ImagePathColumn.ColumnName].ToString();

                        string Format = "{0:0.00}";
                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        ser.MaxJsonLength = Int32.MaxValue;
                        var SearchedfieldsJSON = ser.Deserialize<dynamic>(AgeResponse);
                        var keyValues = new Dictionary<string, string>
                        {
                            { "txtAccountNo", MDVUtility.ToStr(dr[dsPatient.Patients.AccountNumberColumn.ColumnName])},
                            { "txtMRN", MDVUtility.ToStr(dr[dsPatient.Patients.MRNumberColumn.ColumnName])},
                            { "txtSSN", MDVUtility.ToStr(dr[dsPatient.Patients.SSNColumn.ColumnName])},
                            { "ddlPrefix", MDVUtility.ToStr(dr[dsPatient.Patients.PrefixColumn.ColumnName])},
                            { "txtLastName", MDVUtility.ToStr(dr[dsPatient.Patients.LastNameColumn.ColumnName])},
                            { "txtMotherMaidenName", MDVUtility.ToStr(dr[dsPatient.Patients.MotherMaidenNameColumn.ColumnName])},
                            { "ddlSuffix", MDVUtility.ToStr(dr[dsPatient.Patients.SuffixColumn.ColumnName])},
                            { "txtFullName", MDVUtility.ToStr(dr[dsPatient.Patients.FullNameColumn.ColumnName])},
                            { "txtFirstName", MDVUtility.ToStr(dr[dsPatient.Patients.FirstNameColumn.ColumnName])},
                            { "txtMiddleInitial", MDVUtility.ToStr(dr[dsPatient.Patients.MIColumn.ColumnName])},
                            { "txtPreviousName", MDVUtility.ToStr(dr[dsPatient.Patients.PreviousNameColumn.ColumnName])},
                            { "dtpDOB",  String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.Patients.DOBColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]).ToShortDateString()},
                            { "txtAge", SearchedfieldsJSON["ActualAge"]},
                            { "ddlSex", MDVUtility.ToStr(dr[dsPatient.Patients.GenderColumn.ColumnName])},
                            { "ddlMaritalStatus", MDVUtility.ToStr(dr[dsPatient.Patients.MaritialStatusColumn.ColumnName])},
                            { "ddlEthnicity", MDVUtility.ToStr(dr[dsPatient.Patients.EthnicityIdColumn.ColumnName])},
                            { "ddlPatientRace", MDVUtility.ToStr(dr[dsPatient.Patients.strRaceIdsColumn.ColumnName])},
                            { "ddlPrefLanguage", MDVUtility.ToStr(dr[dsPatient.Patients.PrefLanguageIdColumn.ColumnName])},
                            { "ddlPatientStatus", MDVUtility.ToStr(dr[dsPatient.Patients.IsActiveColumn.ColumnName])},
                            { "txtInactiveReason", MDVUtility.ToStr(dr[dsPatient.Patients.InActiveReasonColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsPatient.Patients.CommentsColumn.ColumnName])},
                            { "txtAddress1", MDVUtility.ToStr(dr[dsPatient.Patients.Address1Column.ColumnName])},
                            { "txtAddress2", MDVUtility.ToStr(dr[dsPatient.Patients.Address2Column.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsPatient.Patients.CityColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsPatient.Patients.StateColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeColumn.ColumnName])},
                            { "txtZipExt", MDVUtility.ToStr(dr[dsPatient.Patients.ZIPCodeExtColumn.ColumnName])},
                            { "txtHomeTel", MDVUtility.ToStr(dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName])},
                            { "txtWorkTel", MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneNoColumn.ColumnName])},
                            { "txtExt", MDVUtility.ToStr(dr[dsPatient.Patients.WorkPhoneExtColumn.ColumnName])},
                            { "txtCell", MDVUtility.ToStr(dr[dsPatient.Patients.CellNoColumn.ColumnName])},
                            { "txtFax", MDVUtility.ToStr(dr[dsPatient.Patients.FaxNoColumn.ColumnName])},
                            { "txtEmail", MDVUtility.ToStr(dr[dsPatient.Patients.EmailAddressColumn.ColumnName])},
                            { "chkBadAddress", MDVUtility.ToStr(dr[dsPatient.Patients.BadAddressColumn.ColumnName])},
                            { "txtProvider", MDVUtility.ToStr(dr[dsPatient.Patients.ProviderNameColumn.ColumnName])},
                            { "hfProvider", MDVUtility.ToStr(dr[dsPatient.Patients.ProviderIdColumn.ColumnName])},
                            { "txtFacility", MDVUtility.ToStr(dr[dsPatient.Patients.FacilityNameColumn.ColumnName])},
                            { "hfFacility", MDVUtility.ToStr(dr[dsPatient.Patients.FacilityIdColumn.ColumnName])},
                            { "txtPractice", MDVUtility.ToStr(dr[dsPatient.Patients.PracticeNameColumn.ColumnName])},
                            { "hfPractice", MDVUtility.ToStr(dr[dsPatient.Patients.PracticeIdColumn.ColumnName])},
                            { "txtRefProvider", MDVUtility.ToStr(dr[dsPatient.Patients.ReferringProviderNameColumn.ColumnName])},
                            { "hfRefProvider", MDVUtility.ToStr(dr[dsPatient.Patients.ReferringProviderIdColumn.ColumnName])},
                            { "txtPCP", MDVUtility.ToStr(dr[dsPatient.Patients.PCPNameColumn.ColumnName])},
                            { "hfPCP", MDVUtility.ToStr(dr[dsPatient.Patients.PCPIdColumn.ColumnName])},
                            { "txtGuarantor", MDVUtility.ToStr(dr[dsPatient.Patients.GuarantorNameColumn.ColumnName])},
                            { "hfGuarantor", MDVUtility.ToStr(dr[dsPatient.Patients.GuarantorIdColumn.ColumnName])},
                            { "hfPatientBalance", String.Format(Format,dr[dsPatient.Patients.PatientBalanceColumn.ColumnName])},
                            { "hfInsuranceBalance", String.Format(Format,dr[dsPatient.Patients.InsuranceBalanceColumn.ColumnName])},
                            { "hfAdvanceBalance", String.Format(Format,dr[dsPatient.Patients.AdvanceBalanceColumn.ColumnName])},
                            { "hfPatientPortalStatus", String.Format(Format,dr[dsPatient.Patients.PatientPortalStatusColumn.ColumnName])},
                            { "chkSelfPay", MDVUtility.ToStr(dr[dsPatient.Patients.SelfPayColumn.ColumnName])}


                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;

                        string imageBase64 = "";

                        byte[] imageByteArr = dr[dsPatient.Patients.PatientProfileImagePathColumn.ColumnName] as byte[];
                        byte[] patImageByteArr = dr[dsPatient.Patients.PatientImageColumn.ColumnName] as byte[];
                        if (imageByteArr != null)
                        {
                            imageBase64 = "data:" + dr[dsPatient.Patients.ImageTypeColumn.ColumnName] + ";base64," + Convert.ToBase64String(dr[dsPatient.Patients.PatientProfileImagePathColumn.ColumnName] as byte[]);
                        }
                        else if (patImageByteArr != null)
                        {
                            imageBase64 = "data:" + dr[dsPatient.Patients.ImageTypeColumn.ColumnName] + ";base64," + Convert.ToBase64String(dr[dsPatient.Patients.PatientImageColumn.ColumnName] as byte[]);
                        }

                        //string imageBase64 = "";
                        //string ServerPathForLoadFile = System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesPath"];
                        //string lfileName = MDVUtility.ToStr(dr[dsPatient.Patients.PatientProfileImagePathColumn.ColumnName]);

                        //if (System.IO.Directory.Exists(ServerPathForLoadFile))
                        //{
                        //    if (!lfileName.Equals(""))
                        //    {
                        //        string imgPath = System.IO.Path.Combine(ServerPathForLoadFile, lfileName);
                        //        byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);

                        //        string base64String = Convert.ToBase64String(imageBytes);
                        //        imageBase64 = "data:" + dr[dsPatient.Patients.ImageTypeColumn.ColumnName] + ";base64," + base64String;

                        //    }
                        //}

                        if (!String.IsNullOrEmpty(imageBase64))
                            keyValues.Add("imgPatient", imageBase64);

                        var response = new
                        {

                            status = true,
                            DemographicFill_JSON = js.Serialize(keyValues),
                            //Image_url = ImagePath  != "" ? string.Format("{0}{1}", HttpRuntime.AppDomainAppVirtualPath, ImagePath):ImagePath,
                            //Image_url = imageBase64,


                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        var keyValues = new Dictionary<string, string>
                        {
                         { "txtAccountNo", ""},
                            { "txtMRN", ""},
                            { "txtSSN", ""},
                            { "ddlPrefix", ""},
                            { "txtLastName", ""},
                            { "txtMotherMaidenName", ""},
                            { "ddlSuffix", ""},
                            { "txtFullName", ""},
                            { "txtFirstName", ""},
                            { "txtMiddleInitial", ""},
                            { "txtPreviousName", ""},
                            { "dtpDOB", ""},
                            { "txtAge", ""},
                            { "ddlSex", ""},
                            { "ddlMaritalStatus", ""},
                            { "ddlEthnicity", ""},
                            { "ddlRace", ""},
                            { "ddlPrefLanguage", ""},
                            { "ddlPatientStatus", ""},
                            {"txtInactiveReason",""},
                            {"txtComments",""},
                            { "txtAddress1", ""},
                            { "txtAddress2", ""},
                            { "txtCity", ""},
                            { "txtState", ""},
                            { "txtZip", ""},
                            { "txtZipExt", ""},
                            { "txtHomeTel", ""},
                            { "txtWorkTel", ""},
                            { "txtExt", ""},
                            { "txtCell", ""},
                            { "txtFax", ""},
                            { "txtEmail", ""},
                            { "chkBadAddress", ""},
                            { "txtProvider", ""},
                            { "hfProvider", ""},
                            { "txtFacility", ""},
                            { "hfFacility", ""},
                            { "txtPractice", ""},
                            { "hfPractice", ""},
                            { "txtRefProvider", ""},
                            { "hfRefProvider", ""},
                            { "txtPCP", ""},
                            { "hfPCP", ""},
                            { "txtGuarantor", ""},
                            { "hfGuarantor", ""},
                            { "hfPatientBalance", "0.00"},
                            { "hfInsuranceBalance", "0.00"},
                            { "hfAdvanceBalance", "0.00"},
                            { "chkSelfPay", ""}

                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            DemographicFill_JSON = js.Serialize(keyValues),
                            Image_url = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        /// <summary>
        /// Deletes the demographic.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        private string DeleteDemographic(Int64 PatientId)
        {

            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLPatientObj.DeletePatient(MDVUtility.ToStr(PatientId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
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
        #endregion


        /// <summary>
        /// Gets the age.
        /// </summary>
        /// <param name="birthday">The birthday.</param>
        /// <returns></returns>
        public string GetAge(DateTime birthday)
        {
            try
            {
                //int age = ((DateTime.Now.Year - birthday.Year) * 372 + (DateTime.Now.Month - birthday.Month) * 31 + (DateTime.Now.Day - birthday.Day)) / 372;

                //int age2 = Convert.ToInt32(Math.Round(today.Subtract(birthday).TotalDays * 0.00273790926));

                DateTime today = DateTime.Now;
                //TimeSpan ts = today - birthday;
                //int Age3 = ts.Days / 365;
                // DateTime Age = DateTime.MinValue.AddDays(ts.Days);

                int days = today.Day - birthday.Day;
                if (days < 0)
                {
                    today = today.AddMonths(-1);
                    days += DateTime.DaysInMonth(today.Year, today.Month);
                }

                int months = today.Month - birthday.Month;
                if (months < 0)
                {
                    today = today.AddYears(-1);
                    months += 12;
                }

                int years = today.Year - birthday.Year;


                string ActAge = string.Format(" {0} Year(s), {1} Month(s), {2} Day(s)  ", years,
                             months,
                             days);

                var response = new
                {
                    status = true,
                    ActualAge = ActAge
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        private string GetImagePath(string PatientID, byte[] PatientImage)
        {
            string strPath = "";
            try
            {



                strPath = MDVSession.Current.ImagePath + "/" + MDVSession.Current.EntityId;
                if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(strPath)))
                {
                    foreach (var f in System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(strPath)))
                        System.IO.File.Delete(f);
                }

                //  strPath += "/" + FileName + "." + FileExtension;

                // byte[] input = new byte[FileLen];

                System.IO.FileStream _FileStream = new System.IO.FileStream(HttpContext.Current.Server.MapPath(strPath + "/" + PatientID), System.IO.FileMode.Create, System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(PatientImage, 0, PatientImage.Length);

                // close file stream
                _FileStream.Close();




                return strPath;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            return strPath;
        }
        public string GetPreferedLanguages(LanguageAndCountriesModel model)
        {
            try
            {
                DSPatientLookups dsPatientLookup = null;
                BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupLanguages(model.name);


                dsPatientLookup = objPatient.Data;
                if (objPatient.Data != null)
                {
                    if (dsPatientLookup.Tables[dsPatientLookup.Languages.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PreferedlanguagesCount = dsPatientLookup.Tables[dsPatientLookup.Languages.TableName].Rows.Count,
                            PreferedlanguagesLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPatientLookup.Tables[dsPatientLookup.Languages.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PreferedlanguagesCount = 0,
                            Message = objPatient.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objPatient.Message
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
        public string GetCountries(LanguageAndCountriesModel model)
        {
            try
            {
                DSPatientLookups dsPatientLookup = null;
                BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupCountries(model.name);


                dsPatientLookup = objPatient.Data;
                if (objPatient.Data != null)
                {
                    if (dsPatientLookup.Tables[dsPatientLookup.Countries.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CountriesCount = dsPatientLookup.Tables[dsPatientLookup.Countries.TableName].Rows.Count,
                            CountriesLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsPatientLookup.Tables[dsPatientLookup.Countries.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            CountriesCount = 0,
                            Message = objPatient.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objPatient.Message
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
        public string GetCities(LanguageAndCountriesModel model)
        {
            try
            {              
                BLObject<List<CityModel>> objCities = new BLLPatient().LookupCities(model.name);
               
                if (objCities != null && objCities.Data.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        CitiesCount = objCities.Data.Count,
                        CitiesLoad_JSON = objCities.Data
                   };
                   return (Newtonsoft.Json.JsonConvert.SerializeObject(response));                                        
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objCities.Message
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
                case "SAVE_DEMOGRAPHIC":
                    {
                        string fieldsJSON = context.Request["DemographicData"];
                        string IsDemographicQuick = context.Request["IsDemographicQuick"];
                        string strJSONData = SaveDemographic(fieldsJSON, IsDemographicQuick);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_DEMOGRAPHIC":
                    {
                        string PatientID = context.Request["PatientID"];
                        string strJSONData = FillPatient(MDVUtility.ToInt64(PatientID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_DEMOGRAPHIC":
                    {
                        string PatientID = context.Request["PatientID"];
                        string strJSONData = DeleteDemographic(MDVUtility.ToInt64(PatientID));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_DEMOGRAPHIC":
                    {
                        string fieldsJSON = context.Request["DemographicData"];
                        int PatientID = MDVUtility.ToInt(context.Request["PatientID"]);
                        string strJSONData = UpdateDemographic(fieldsJSON, PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "CALCULATE_AGE":
                    {
                        DateTime birthDate = MDVUtility.ToDateTime(context.Request["BirthDate"]);
                        string strJSONData = GetAge(birthDate);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion


        public string loadReferralData(PatientDemographicModel request_model)
        {
            try
            {
                PatientReferralModel model = new PatientReferralModel();
                model.ReferralId = request_model.ReferralId;
                model.PatientId = request_model.PatientID;
                model.Type = "Incoming";

                DSPatientReferral dsReferral = null;
                BLObject<DSPatientReferral> obj = null;
                obj = BLLPatientObj.loadReferral(MDVUtility.ToLong(model.NoteId), (model.IsActive != null ? model.IsActive : ""), MDVUtility.ToLong(model.ReferralId), MDVUtility.ToLong(model.PatientId), model.CPTCodeDescription, MDVUtility.ToLong(model.RefferalTo), MDVUtility.ToLong(model.RefferalFrom), model.DateFrom, model.DateTo, MDVUtility.ToInt32(model.Status), model.PAN, model.Type, MDVUtility.ToInt32(model.VisitType), MDVUtility.ToInt64(model.PatientInsurance), MDVUtility.ToInt64(model.FacilityFrom), MDVUtility.ToInt64(model.FacilityTo), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));

                if (obj != null)
                {
                    dsReferral = obj.Data;
                }
                if (dsReferral != null && dsReferral.Referrals.Rows.Count > 0)
                {

                    DataRow dr = dsReferral.Tables[dsReferral.Referrals.TableName].Rows[0];

                    var ReferralData = new Dictionary<string, string>
                            {
                                {"ReferralId",MDVUtility.ToStr(dr[dsReferral.Referrals.ReferralIdColumn.ColumnName])},
                                { "Date", MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateColumn.ColumnName]).ToShortDateString())},
                                { "Time", MDVUtility.ToStr(dr[dsReferral.Referrals.TimeColumn.ColumnName])},
                                { "ProviderReferral", MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderNameColumn.ColumnName])},
                                { "ProviderIdReferral", MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderIdColumn.ColumnName])},
                                { "RefProviderReferral", MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderNameColumn.ColumnName])},
                                { "RefProviderIdReferral", MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderIdColumn.ColumnName])},
                                { "Assignee", MDVUtility.ToStr(dr[dsReferral.Referrals.AssigneeNameColumn.ColumnName])},
                                { "AssigneeId", MDVUtility.ToStr(dr[dsReferral.Referrals.AssigneeIdColumn.ColumnName])},
                                { "Status", MDVUtility.ToStr(dr[dsReferral.Referrals.StatusColumn.ColumnName])},
                                { "Visits", MDVUtility.ToStr(dr[dsReferral.Referrals.VisitsColumn.ColumnName])},
                                { "Reason", MDVUtility.ToStr(dr[dsReferral.Referrals.ReasonColumn.ColumnName])},
                                { "PatientInsurance", MDVUtility.ToStr(dr[dsReferral.Referrals.PatientInsuranceColumn.ColumnName])},
                                { "CommentsReferral", MDVUtility.ToStr(dr[dsReferral.Referrals.CommentsColumn.ColumnName])},
                                { "PatientId", MDVUtility.ToStr(dr[dsReferral.Referrals.PatientIdColumn.ColumnName])},

                            };
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ReferralListLoad_JSON = js.Serialize(ReferralData),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ReferralListCount = 0,
                        ReferralListLoad_JSON = MDVUtility.JSON_DataTable(dsReferral.Tables[dsReferral.Referrals.TableName]),
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

        public bool PatientDemographicImageSaveInFolder(string ImgStr, string ImgName, string fileType)
        {
            try
            {
                string ServerPath = System.Configuration.ConfigurationManager.AppSettings["PatintDemographicImagesPath"];
                string PatientImageThumbnailStream;

                //string path = HttpContext.Current.Server.MapPath("~/PatientDemographicImages"); //Path

                //Check if directory exist
                if (!string.IsNullOrEmpty(ImgName))
                {
                    if (!System.IO.Directory.Exists(ServerPath))
                    {
                        System.IO.Directory.CreateDirectory(ServerPath); //Create directory if it doesn't exist
                    }

                    PatientImageThumbnailStream = MDVUtility.ResizeImage(ImgStr, 100, 100);

                    //Save profile img
                    string lPatientProfileImageName = ImgName + "." + fileType;
                    string imgPath = Path.Combine(ServerPath, lPatientProfileImageName);
                    byte[] imageBytes = Convert.FromBase64String(ImgStr);
                    File.WriteAllBytes(imgPath, imageBytes);


                    //Save thumbnail img
                    string lPatientImageThumbnailName = ImgName + "-th." + fileType;
                    string imgPathforthumbnail = Path.Combine(ServerPath, lPatientImageThumbnailName);
                    byte[] ThumbnailimageBytes = Convert.FromBase64String(PatientImageThumbnailStream);
                    File.WriteAllBytes(imgPathforthumbnail, ThumbnailimageBytes);
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string SendBillingInquiryEmail(BillingInquiryEmailModel model)
        {
            string mailAddress = MDVApplication.BillingInquiryEmail;

            if (mailAddress != "")
            {
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient("smtp.office365.com");

                mail.From = new MailAddress(MDVApplication.No_Reply_EmailAddress);
                mail.To.Add(mailAddress);
                mail.Subject = "Billing Inquiry from " + MDVUtility.ToStr(model.hfPatientProviderName);

                mail.Body = GetFormattedEmailMessage(model);
                mail.IsBodyHtml = true;

                client.Port = 587;
                client.Credentials = new NetworkCredential(MDVApplication.No_Reply_EmailAddress, MDVApplication.No_Reply_EmailPassword);
                client.EnableSsl = true;

                try
                {
                    client.Send(mail);

                    BLLPatientObj.InsertBillingInquiryEmail(model, Convert.ToString(MDVApplication.No_Reply_EmailAddress), MDVUtility.ToStr(MDVApplication.BillingInquiryEmail));
                    var response = new
                    {
                        status = true,
                        message = "Email sent successfully.",
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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
            else
            {
                var response = new
                {
                    status = false,
                    message = "Billing inquiry email address missing.",
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string GetFormattedEmailMessage(BillingInquiryEmailModel model)
        {

            StringBuilder str = new StringBuilder();
            const string newLine = "<br/>";

            
            str.Append(@"<html><body><div style='font-family: Calibri, Helvetica, sans-serif;'>");
            
            str.Append(newLine).Append("Dear User");
            str.Append(newLine);
            str.Append(newLine).Append("<b>Patient's Account:</b> ").Append(model.hfAccountNo);
            str.Append(newLine).Append("<b>Patient's Name:</b> ").Append(MDVUtility.ToStr(model.hfPatientFullNameOnly));
            str.Append(newLine).Append("<b>Outstanding Balance: </b>").Append(model.hfPatientBalance);
            str.Append(newLine);
            str.Append(newLine).Append("It is to notify that the patient's outstanding balance is increasing. Please follow-up on this account and get back to me.");  
            str.Append(newLine).Append(newLine).Append("Thank you,");
            str.Append(newLine).Append("Sovereign Medical Group");
            str.Append(newLine).Append(newLine).Append("<span style='font-family: Calibri, Helvetica, sans-serif; font-size: 7;'>Secure Email</span>");
            str.Append("</div></body></html>");

            return str.ToString();
        }
        
    }
}
