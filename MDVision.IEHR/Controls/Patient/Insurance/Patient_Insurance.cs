using System;
using System.Collections.Generic;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;
using System.Data;
using System.IO;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MDVision.IEHR.Common;
using MDVision.Model.Schedule;

namespace MDVision.IEHR.Controls.Patient.Insurance
{
    public class Patient_Insurance
    {
        private BLLPatient BLLPatientObj = null;
        BLLAdminInsurance BLLAdminInsuranceObj = null;
        public Patient_Insurance()
        {
            BLLPatientObj = new BLLPatient();
            BLLAdminInsuranceObj = new BLLAdminInsurance();
        }
        #region Singleton
        private static Patient_Insurance _obj = null;
        public static Patient_Insurance Instance()
        {
            if (_obj == null)
                _obj = new Patient_Insurance();
            return _obj;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Saves the Insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <returns></returns>
        public string SavePatientInsurance(HttpFileCollection files, string fieldsJSON, Int64 PatientId, Int32 BStream, string scanImage, bool IsFromCopayUpdateAlert)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                if (PatientId > 0)
                {
                    DSPatient dsPatient = new DSPatient();
                    DSPatient.PatientInsuranceRow dr = dsPatient.PatientInsurance.NewPatientInsuranceRow();

                    dr.PatientId = MDVUtility.ToInt64(PatientId);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlRelation"]))
                        dr.RelationShipId = MDVUtility.ToInt(SearchedfieldsJSON["ddlRelation"]);
                    //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlInsuranceType"]))
                    //    dr.PlanType = MDVUtility.ToStr(SearchedfieldsJSON["ddlInsuranceType"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfInsurancePlan"]) && SearchedfieldsJSON["hfInsurancePlan"] != "-1")
                        dr.InsurancePlanId = MDVUtility.ToInt64(SearchedfieldsJSON["hfInsurancePlan"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanAddress"]))
                        dr.InsurancePlanAddressId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlanAddress"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfLawyer"]) && SearchedfieldsJSON["hfLawyer"] != "-1")
                        dr.LawyerId = MDVUtility.ToInt64(SearchedfieldsJSON["hfLawyer"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfEmployer"]) && SearchedfieldsJSON["hfEmployer"] != "-1")
                        dr.EmployerId = MDVUtility.ToInt64(SearchedfieldsJSON["hfEmployer"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSex"]))
                        dr.Gender = MDVUtility.ToStr(SearchedfieldsJSON["ddlSex_text"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMSPType"]))
                        dr.MSPTypeId = MDVUtility.ToInt(SearchedfieldsJSON["ddlMSPType"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOB"]))
                        dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpEligibilityDate"]))
                        dr.EligibilityDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpEligibilityDate"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDateSigned"]))
                        dr.DateSigned = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDateSigned"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpCoverageDateFrom"]))
                        dr.CoverageFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCoverageDateFrom"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpCoverageDateTo"]))
                        dr.CoverageTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCoverageDateTo"]);
                    dr.EligibilityStatus = MDVUtility.ToStr(SearchedfieldsJSON["ddlEligibilityStatus"]);
                    if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlParticipentStatus"]))
                        dr.PARStatusId = MDVUtility.ToInt(SearchedfieldsJSON["ddlParticipentStatus"]);
                    dr.LastName = SearchedfieldsJSON["txtLastName"];
                    dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                    dr.MI = SearchedfieldsJSON["txtMiddleInitial"];
                    //dr.m = SearchedfieldsJSON["txtFirstName"];
                    dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                    dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                    dr.City = SearchedfieldsJSON["txtCity"];
                    dr.State = SearchedfieldsJSON["txtState"];
                    dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                    dr.ZIPCodeExt = SearchedfieldsJSON["txtZipExt"];
                    dr.HomePhoneNo = SearchedfieldsJSON["txtHomeTel"];
                    dr.Comments = SearchedfieldsJSON["txtComments"];
                    dr.SSN = SearchedfieldsJSON["txtSSNIns"];
                    dr.PlanPriority = MDVUtility.ToInt(SearchedfieldsJSON["hfInsurancePlanPriority"]);
                    dr.SubscriberId = MDVUtility.ToStr(SearchedfieldsJSON["txtSubscriberID"]);
                    dr.GroupId = MDVUtility.ToStr(SearchedfieldsJSON["txtSubscriberGroupID"]);
                    dr.AmtCopay = MDVUtility.ToDouble(SearchedfieldsJSON["txtVisitCopayment"]);
                    dr.SpecialistCopay = MDVUtility.ToDouble(SearchedfieldsJSON["txtSpecialistCopay"]);
                    dr.AssignBenefits = MDVUtility.ToStr(SearchedfieldsJSON["chkAssgBenefits"]) == "True" ? true : false;
                    dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                    dr.UnAssigned = MDVUtility.ToStr(SearchedfieldsJSON["chkUnassigned"]) == "True" ? true : false;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dr.IsFileStream = false;
                    if (BStream == 1)
                    {
                        if (!string.IsNullOrEmpty(scanImage))
                        {
                            String base64String = scanImage;

                            byte[] bytes = Convert.FromBase64String(base64String);


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
                            //fName = mnth + "-" + day + "-" + year + " " + mnt +".jpg";
                            fName = mnth + "." + day + "." + year + ".jpg";

                            string FilePath = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Insurance Card", PatientId, fName, bytes);
                            dr.Url = FilePath;
                            String FrontImageStream = string.Empty;
                            String BackImageStream = string.Empty;
                            base64String = base64String.Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "").Replace(' ', '+').Replace(@"\/", "/");

                            Image image;
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                image = Image.FromStream(ms);
                            }

                            FrontImageStream = MDVUtility.CropImage(image, 0, 0, image.Width, image.Height / 2, ImageFormat.Png);
                            BackImageStream = MDVUtility.CropImage(image, 0, image.Height / 2, image.Width, image.Height, ImageFormat.Png);

                            //FrontImageStream = base64String.Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "").Replace(' ', '+').Replace(@"\/", "/");
                            //BackImageStream = base64String.Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "").Replace(' ', '+').Replace(@"\/", "/");
                            string FilePathFrontImage = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Insurance Card", PatientId, "Insurance Card Front Image.jpg", Convert.FromBase64String(FrontImageStream));
                            string FilePathBackImage = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Insurance Card", PatientId, "Insurance Card Back Image.jpg", Convert.FromBase64String(BackImageStream));

                            //try
                            //{
                            //    //dr.FileStream = Convert.FromBase64String(base64String);
                                //dr.FrontImageStream = Convert.FromBase64String(FrontImageStream);
                                //dr.BackImageStream = Convert.FromBase64String(BackImageStream);
                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    dr.InsuranceCardImagePath = MDVUtility.ToStr(FilePath);
                                    dr.IsFileStream = true;
                                }
                                else
                                {
                                    dr[dsPatient.PatientInsurance.InsuranceCardImagePathColumn] = DBNull.Value;
                                    dr.IsFileStream = false;
                                }
                                if (!string.IsNullOrEmpty(FilePathFrontImage))
                                {
                                    dr.FrontSideImagePath = MDVUtility.ToStr(FilePathFrontImage);
                                }
                                else
                                {
                                    dr[dsPatient.PatientInsurance.FrontSideImagePathColumn] = DBNull.Value;
                                }

                                if (!string.IsNullOrEmpty(FilePathBackImage))
                                {
                                    dr.BackSideImagePath = MDVUtility.ToStr(FilePathBackImage);
                                }
                                else { dr[dsPatient.PatientInsurance.BackSideImagePathColumn] = DBNull.Value; }
                            }
                            //catch (Exception)
                            //{
                                //dr[dsPatient.PatientInsurance.FileStreamColumn] = DBNull.Value;
                                //dr[dsPatient.PatientInsurance.FrontImageStreamColumn] = DBNull.Value;
                                //dr[dsPatient.PatientInsurance.BackImageStreamColumn] = DBNull.Value;                         
                                //dr.IsFileStream = false;
                                //throw;
                            //}
                            //dr["FileStream"] = Convert.FromBase64String(base64String);
                            //dr.IsFileStream = true;
                        //}
                    }

                    #region Database Insertion
                    dsPatient.PatientInsurance.AddPatientInsuranceRow(dr);


                    BLObject<DSPatient> obj = BLLPatientObj.InsertPatientInsurance(dsPatient);



                    if (obj.Data != null)
                    {
                        var TotalCount = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count;
                        BLLSchedule bllSchedule = new BLLSchedule();
                        BLObject<List<PatientFutureAppointment>> objPatientFutureAppointment = null;
                        BLObject<List<PatientFutureAppointment>> objPatientFutureAppointmentForCopay = null;                       
                        if (IsFromCopayUpdateAlert == true)
                        {
                            objPatientFutureAppointmentForCopay = bllSchedule.GetPatientFutureAppointmentForCopay(Convert.ToInt64(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.PatientIdColumn.ColumnName]), Convert.ToInt64(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName]));
                        }
                        objPatientFutureAppointment = bllSchedule.GetPatientFutureAppointment(Convert.ToInt64(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.PatientIdColumn.ColumnName]));

                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                            PatientInsuranceId = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName],
                            ClaimFlagDescription = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.CFDescriptionColumn.ColumnName],
                            IsTCM = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.IsTCMColumn.ColumnName],
                            Priority = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.PriorityColumn.ColumnName],
                            PatientFutureAppointment = objPatientFutureAppointment != null ? objPatientFutureAppointment.Data : null,
                            PatientFutureAppointmentForCopay = objPatientFutureAppointmentForCopay != null ? objPatientFutureAppointmentForCopay.Data : null
                        };
                        // if (!string.IsNullOrEmpty(SearchedfieldsJSON["scanImage"]))
                        // {
                        //string isSaved = SavePatientDocument(Patient_Id, model.imagedata, model.filetype, model.filename, model.foldername, model.AssignUserto,model.AssignedToName, model.ScannerDOS, model.ScannerComments);
                        //   string isSaved = SavePatientDocument(PatientId, SearchedfieldsJSON["imagedata"], SearchedfieldsJSON["filetype"], SearchedfieldsJSON["filename"], SearchedfieldsJSON["foldername"]);
                        // }
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
        public string SavePatientDocument(Int64 PatientID, string strBase64 = null, string fileType = null, string FileName = "", string FolderId = "")
        {
            try
            {


                if (PatientID > -1) //Changed by Arslan for Patient Education Flow
                {
                    DSPatient dsDocument = new DSPatient();
                    DSMessage dsMessage = new DSMessage();

                    DSPatient.PatientDocumentRow dr = dsDocument.PatientDocument.NewPatientDocumentRow();
                    dr.PatientId = PatientID;
                    dr.Documentid = MDVUtility.ToInt32(FolderId);
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
                    if (!string.IsNullOrEmpty(strBase64))
                    {
                        String base64String = strBase64;
                        base64String = base64String.Replace("data:image/png;base64,", "");
                        base64String = base64String.Replace("data:image/jpg;base64,", "");
                        base64String = base64String.Replace(' ', '+').Replace(@"\/", "/");
                        try
                        {
                            currentFileStream = Convert.FromBase64String(base64String);
                            dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
                            // dr.IsFileStream = true;
                        }
                        catch (Exception)
                        {
                            dr[dsDocument.PatientDocument.FileStreamColumn] = DBNull.Value;
                            // dr.IsFileStream = false;
                            //throw;
                        }
                        //dr["FileStream"] = Convert.FromBase64String(base64String);
                        //dr.IsFileStream = true;
                    }
                    //  byte[] currentFileStream = Convert.FromBase64String(strBase64);


                    //    dr.FileStream = currentFileStream.Length > 0 ? currentFileStream : null;
                    dr.FileType = fileType;
                    if (FileName == "")
                        dr.FilePath = Guid.NewGuid().ToString() + "." + fileType.Split('/')[1];//Utility.ToStr(SearchedfieldsJSON["txtDescription"]);
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
                    BLObject<DSPatient> obj = BLLPatientObj.InsertPatientDocument(dsDocument);
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
        /// <summary>
        /// Updates the Insurance.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        /// 
        public string UpdatePatientInsurance(HttpFileCollection files, string fieldsJSON, Int64 PatientId, Int64 PatientInsuranceID, Int32 BStream, bool InsuranceCardChanged, string scanImage, bool IsFromCopayUpdateAlert)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = Int32.MaxValue;

                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                if (PatientInsuranceID > 0)
                {

                    DSPatient dsPatient = null;
                    BLObject<DSPatient> obj;
                    obj = BLLPatientObj.LoadPatientInsurance(PatientInsuranceID, PatientId);

                    dsPatient = obj.Data;
                    foreach (DSPatient.PatientInsuranceRow dr in dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlRelation"]))
                            dr.RelationShipId = MDVUtility.ToInt(SearchedfieldsJSON["ddlRelation"]);
                        //if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlInsuranceType"]))
                        //    dr.PlanType = MDVUtility.ToStr(SearchedfieldsJSON["ddlInsuranceType"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfInsurancePlan"]) && SearchedfieldsJSON["hfInsurancePlan"] != "-1")
                            dr.InsurancePlanId = MDVUtility.ToInt64(SearchedfieldsJSON["hfInsurancePlan"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanAddress"]))
                            dr.InsurancePlanAddressId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPlanAddress"]);
                        else
                            dr[dsPatient.PatientInsurance.InsurancePlanAddressIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfLawyer"]) && SearchedfieldsJSON["hfLawyer"] != "-1")
                            dr.LawyerId = MDVUtility.ToInt64(SearchedfieldsJSON["hfLawyer"]);
                        else
                            dr[dsPatient.PatientInsurance.LawyerIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfEmployer"]) && SearchedfieldsJSON["hfEmployer"] != "-1")
                            dr.EmployerId = MDVUtility.ToInt64(SearchedfieldsJSON["hfEmployer"]);
                        else
                            dr[dsPatient.PatientInsurance.EmployerIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSex"]))
                            dr.Gender = MDVUtility.ToStr(SearchedfieldsJSON["ddlSex_text"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlMSPType"]))
                            dr.MSPTypeId = MDVUtility.ToInt(SearchedfieldsJSON["ddlMSPType"]);
                        else
                            dr[dsPatient.PatientInsurance.MSPTypeIdColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOB"]))
                            dr.DOB = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOB"]);
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpEligibilityDate"]))
                            dr.EligibilityDate = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpEligibilityDate"]);
                        else
                            dr[dsPatient.PatientInsurance.EligibilityDateColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpDateSigned"]))
                            dr.DateSigned = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDateSigned"]);
                        else
                            dr[dsPatient.PatientInsurance.DateSignedColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpCoverageDateFrom"]))
                            dr.CoverageFrom = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCoverageDateFrom"]);
                        else
                            dr[dsPatient.PatientInsurance.CoverageFromColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["dtpCoverageDateTo"]))
                            dr.CoverageTo = MDVUtility.ToDateTime(SearchedfieldsJSON["dtpCoverageDateTo"]);
                        else
                            dr[dsPatient.PatientInsurance.CoverageToColumn] = DBNull.Value;
                        if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlParticipentStatus"]))
                            dr.PARStatusId = MDVUtility.ToInt(SearchedfieldsJSON["ddlParticipentStatus"]);
                        dr.EligibilityStatus = MDVUtility.ToStr(SearchedfieldsJSON["ddlEligibilityStatus"]);
                        dr.LastName = SearchedfieldsJSON["txtLastName"];
                        dr.FirstName = SearchedfieldsJSON["txtFirstName"];
                        dr.MI = SearchedfieldsJSON["txtMiddleInitial"];
                        dr.Address1 = SearchedfieldsJSON["txtAddress1"];
                        dr.Address2 = SearchedfieldsJSON["txtAddress2"];
                        dr.City = SearchedfieldsJSON["txtCity"];
                        dr.State = SearchedfieldsJSON["txtState"];
                        dr.ZIPCode = SearchedfieldsJSON["txtZip"];
                        dr.ZIPCodeExt = SearchedfieldsJSON["txtZipExt"];
                        dr.HomePhoneNo = SearchedfieldsJSON["txtHomeTel"];
                        dr.Comments = SearchedfieldsJSON["txtComments"];
                        dr.SSN = SearchedfieldsJSON["txtSSNIns"];
                        if (MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) != "True")
                        {
                            dr.PlanPriority = MDVUtility.ToInt(SearchedfieldsJSON["hfInsurancePlanPriority"]);

                        }
                        dr.SubscriberId = MDVUtility.ToStr(SearchedfieldsJSON["txtSubscriberID"]);
                        dr.GroupId = MDVUtility.ToStr(SearchedfieldsJSON["txtSubscriberGroupID"]);
                        dr.AmtCopay = MDVUtility.ToDouble(SearchedfieldsJSON["txtVisitCopayment"]);
                        dr.SpecialistCopay = MDVUtility.ToDouble(SearchedfieldsJSON["txtSpecialistCopay"]);
                        dr.AssignBenefits = MDVUtility.ToStr(SearchedfieldsJSON["chkAssgBenefits"]) == "True" ? true : false;
                        dr.IsActive = MDVUtility.ToStr(SearchedfieldsJSON["chkActive"]) == "True" ? true : false;
                        dr.UnAssigned = MDVUtility.ToStr(SearchedfieldsJSON["chkUnassigned"]) == "True" ? true : false;
                        //dr.CreatedBy = "";
                        //dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                        dr.IsFileStream = false;                   
                        if (BStream == 1)
                        {

                            if (!string.IsNullOrEmpty(scanImage))
                            {
                                String base64String = scanImage;
                                byte[] bytes = Convert.FromBase64String(base64String);

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
                                //fName = mnth + "-" + day + "-" + year + " " + mnt + ".jpg";
                                fName = mnth + "." + day + "." + year + ".jpg";

                                string FilePath = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Insurance Card", PatientId, fName, bytes);
                                dr.Url = FilePath;

                                String FrontImageStream = string.Empty;
                                String BackImageStream = string.Empty;
                                base64String = base64String.Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "").Replace(' ', '+').Replace(@"\/", "/");

                                Image image;
                                using (MemoryStream ms = new MemoryStream(bytes))
                                {
                                    image = Image.FromStream(ms);
                                }

                                FrontImageStream = MDVUtility.CropImage(image, 0, 0, image.Width, image.Height / 2, ImageFormat.Png);
                                BackImageStream = MDVUtility.CropImage(image, 0, image.Height / 2, image.Width, image.Height, ImageFormat.Png);

                                //FrontImageStream = FrontImageStream.Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "").Replace(' ', '+').Replace(@"\/", "/");
                                //BackImageStream = BackImageStream.Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "").Replace(' ', '+').Replace(@"\/", "/");
                                string FilePathFrontImage = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Insurance Card", PatientId, "Insurance Card Front Image.jpg", Convert.FromBase64String(FrontImageStream));
                                string FilePathBackImage = CommonFunc.SaveDocumentToFolder(null, "Patient Document", "Insurance Card", PatientId, "Insurance Card Back Image.jpg", Convert.FromBase64String(BackImageStream));

                                //try
                                //{
                                //dr.FileStream = Convert.FromBase64String(base64String);
                                //dr.FrontImageStream = Convert.FromBase64String(FrontImageStream);
                                //dr.BackImageStream = Convert.FromBase64String(BackImageStream);

                                if (!string.IsNullOrEmpty(FilePath))
                                {
                                    dr.InsuranceCardImagePath = MDVUtility.ToStr(FilePath);
                                    dr.IsFileStream = true;
                                }
                                else
                                {
                                    dr[dsPatient.PatientInsurance.InsuranceCardImagePathColumn] = DBNull.Value;
                                    dr.IsFileStream = false;
                                }
                                if (!string.IsNullOrEmpty(FilePathFrontImage))
                                {
                                    dr.FrontSideImagePath = MDVUtility.ToStr(FilePathFrontImage);
                                }
                                else
                                {
                                    dr[dsPatient.PatientInsurance.FrontSideImagePathColumn] = DBNull.Value;
                                }

                                if (!string.IsNullOrEmpty(FilePathBackImage))
                                {
                                    dr.BackSideImagePath = MDVUtility.ToStr(FilePathBackImage);
                                }
                                else { dr[dsPatient.PatientInsurance.BackSideImagePathColumn] = DBNull.Value; }

                                //}
                                //catch (Exception ex)
                                //{
                                //    dr[dsPatient.PatientInsurance.FileStreamColumn] = DBNull.Value;
                                //    dr[dsPatient.PatientInsurance.FrontImageStreamColumn] = DBNull.Value;
                                //    dr[dsPatient.PatientInsurance.BackImageStreamColumn] = DBNull.Value;
                                //    dr.IsFileStream = false;
                                //    throw;
                                //}
                                //dr.FileStream = Convert.FromBase64String(base64String);
                                //dr.IsFileStream = true;
                                if (!dr.IsNull(dsPatient.PatientDocument.PatDocIdColumn.ColumnName))
                                {
                                    BLObject<DSPatient> objPatDocument;
                                    objPatDocument = BLLPatientObj.LoadPatientDocument(MDVUtility.ToStr(dr.PatDocId), dr.PatientId, "", "", "", null, null, null, null, "", 0, 0, "", 0, "", "1");
                                    dsPatient.Tables[dsPatient.PatientDocument.TableName].Merge(objPatDocument.Data.Tables[objPatDocument.Data.PatientDocument.TableName]);
                                }
                            }
                        }
                    }

                    #region Database Updation


                    if (dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count > 0)
                    {
                        BLObject<DSPatient> objPatientInsurance = BLLPatientObj.UpdatePatientInsurance(dsPatient, InsuranceCardChanged);

                       
                        //var TotalCount = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count;
                        //long newPatientInsuranceID = Convert.ToInt64(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName]);
                        //BLObject<List<PatientFutureAppointment>> objPatientFutureAppointment = null;
                        if (objPatientInsurance.Data != null)
                        {
                          
                            var TotalCount = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count;
                            BLLSchedule bllSchedule = new BLLSchedule();
                            BLObject<List<PatientFutureAppointment>> objPatientFutureAppointment = null;
                            BLObject<List<PatientFutureAppointment>> objPatientFutureAppointmentForCopay = null;
                            if (IsFromCopayUpdateAlert == true)
                            {
                                objPatientFutureAppointmentForCopay = bllSchedule.GetPatientFutureAppointmentForCopay(Convert.ToInt64(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.PatientIdColumn.ColumnName]), Convert.ToInt64(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName]));
                            }
                            objPatientFutureAppointment = bllSchedule.GetPatientFutureAppointment(Convert.ToInt64(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.PatientIdColumn.ColumnName]));

                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message,
                                PatientInsuranceId = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName],
                                ClaimFlagDescription = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.CFDescriptionColumn.ColumnName],
                                IsTCM = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.IsTCMColumn.ColumnName],
                                Priority = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.PriorityColumn.ColumnName],
                                PatientFutureAppointment = objPatientFutureAppointment != null ? objPatientFutureAppointment.Data : null,
                                PatientFutureAppointmentForCopay = objPatientFutureAppointmentForCopay != null ? objPatientFutureAppointmentForCopay.Data : null
                            };
                            
                            //   if (!string.IsNullOrEmpty(SearchedfieldsJSON["scanImage"]))
                            // {
                            //string isSaved = SavePatientDocument(Patient_Id, model.imagedata, model.filetype, model.filename, model.foldername, model.AssignUserto,model.AssignedToName, model.ScannerDOS, model.ScannerComments);
                            //     string isSaved = SavePatientDocument(PatientId, SearchedfieldsJSON["imagedata"], SearchedfieldsJSON["filetype"], SearchedfieldsJSON["filename"], SearchedfieldsJSON["foldername"]);
                            //  }
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objPatientInsurance.Message
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
                        Message = "Insurance not found."
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

        /// <summary>
        /// Loads the patient insurances.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <returns></returns>
        private string LoadPatientInsurances(Int64 PatientId, Int64 PatientInsuranceId)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj;
                string insuranceFill = null;
                obj = BLLPatientObj.LoadPatientInsurance(0, PatientId);
                if (obj.Data != null)
                {
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count > 0)
                    {
                        long insuranceId = 0;
                        if (PatientInsuranceId > 0)
                        {
                            insuranceId = PatientInsuranceId;//Utility.ToLong(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Select([dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName]==PatientInsuranceId));
                        }
                        else
                        {
                            insuranceId = MDVUtility.ToLong(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[0][dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName]);
                        }

                        insuranceFill = FillPatientInsurances(insuranceId);
                    }
                    var response = new
                    {
                        status = true,
                        PatientInsuranceCount = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count,
                        PatientInsuranceLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientInsurance.TableName]),
                        PatientInsuranceFill_JSON = insuranceFill
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientInsuranceCount = 0,
                        Message = obj.Message
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

        private string LoadPatientInsurancesFromLab(Int64 PatientId, Int64 PatientInsuranceId)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj;
                //string insuranceFill = null;
                obj = BLLPatientObj.LoadPatientInsurance(0, PatientId);
                if (obj.Data != null)
                {
                    dsPatient = obj.Data;
                    var response = new
                    {
                        status = true,
                        PatientInsuranceCount = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count,
                        PatientInsuranceLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientInsurance.TableName]),
                        //PatientInsuranceFill_JSON = insuranceFill
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientInsuranceCount = 0,
                        Message = obj.Message
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


        private string GetPatientRelationshipInfo(Int64 PatientId)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj;
                obj = BLLPatientObj.GetPatientRelationshipInfo(PatientId);
                if (obj.Data != null)
                {
                    dsPatient = obj.Data;
                    var response = new
                    {
                        status = true,
                        PatientRelationshipInfoLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.Patients.TableName]),
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = obj.Message
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

        /// <summary>
        /// Fills the patient insurances.
        /// </summary>
        /// <param name="PatientInsuranceId">The patient insurance identifier.</param>
        /// <returns></returns>
        private string FillPatientInsurances(Int64 PatientInsuranceId)
        {
            string Format = "{0:0.00}";
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientInsuranceId)))
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
                    string strBase64 = "";
                    string fileName = string.Empty;
                    DSPatient dsPatient = null;

                    BLObject<DSPatient> obj = BLLPatientObj.LoadPatientInsurance(PatientInsuranceId, 0, 1);
                    if (obj.Data != null)
                    {
                        dsPatient = obj.Data;
                        if (dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[0];
                            fileName = MDVUtility.ToStr(dr[dsPatient.PatientInsurance.InsuranceCardImagePathColumn.ColumnName]);

                            string ServerPathForLoadFile = System.Configuration.ConfigurationManager.AppSettings["PatientFilesPath"];

                            if (System.IO.Directory.Exists(ServerPathForLoadFile))
                            {

                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    string imgPath = System.IO.Path.Combine(ServerPathForLoadFile, fileName);
                                    if (System.IO.File.Exists(imgPath))
                                    {
                                        byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                                        strBase64 = Convert.ToBase64String(imageBytes);          
                                    }
                                }else
                                {
                                    byte[] byteArr = dr["FileStream"] as byte[];
                                    if (byteArr != null)
                                    {
                                        strBase64 = Convert.ToBase64String(byteArr);
                                    }
                                }
                            }
                            var keyValues = new Dictionary<string, string>
                        {
                            { "txtInsurancePlan", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.InsurancePlanNameColumn.ColumnName])},
                            { "hfInsurancePlan", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.InsurancePlanIdColumn.ColumnName])},
                            { "ddlPlanAddress", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.InsurancePlanAddressIdColumn.ColumnName])},
                            { "txtSubscriberID", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.SubscriberIdColumn.ColumnName])},
                            //{ "txtFEE", String.Format(Format,dr[dsCharge.PatientCharges.FeeColumn.ColumnName])},
                            { "txtVisitCopayment", String.Format(Format,dr[dsPatient.PatientInsurance.AmtCopayColumn.ColumnName])},
                            { "txtSpecialistCopay", String.Format(Format,dr[dsPatient.PatientInsurance.SpecialistCopayColumn.ColumnName])},
                            { "txtAddress1", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.Address1Column.ColumnName])},
                            { "txtCity", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.CityColumn.ColumnName])},
                            { "txtState", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.StateColumn.ColumnName])},
                            { "txtZip", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.ZIPCodeColumn.ColumnName])},
                            { "txtZipExt", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.ZIPCodeExtColumn.ColumnName])},
                            { "txtSubscriberGroupID", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.GroupIdColumn.ColumnName])},
                            { "dtpCoverageDateFrom", string.IsNullOrEmpty(dr[dsPatient.PatientInsurance.CoverageFromColumn.ColumnName].ToString())?"":MDVUtility.GetDateMMDDYYY(dr[dsPatient.PatientInsurance.CoverageFromColumn.ColumnName].ToString())},//Utility.GetDateDDMMYYY
                            { "dtpCoverageDateTo", string.IsNullOrEmpty(dr[dsPatient.PatientInsurance.CoverageToColumn.ColumnName].ToString())?"":MDVUtility.GetDateMMDDYYY(dr[dsPatient.PatientInsurance.CoverageToColumn.ColumnName].ToString())},//Utility.ToStr(dr[dsPatient.PatientInsurance.CoverageToColumn.ColumnName])},
                            { "dtpDateSigned",string.IsNullOrEmpty(dr[dsPatient.PatientInsurance.DateSignedColumn.ColumnName].ToString())?"":MDVUtility.GetDateMMDDYYY(dr[dsPatient.PatientInsurance.DateSignedColumn.ColumnName].ToString())},// MDVUtility.ToStr(dr[dsPatient.PatientInsurance.DateSignedColumn.ColumnName])},
                            { "txtLawyer", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.LawyerNameColumn.ColumnName])},
                            { "hfLawyer", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.LawyerIdColumn.ColumnName])},
                            { "txtEmployer", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.EmployerNameColumn.ColumnName])},
                            { "hfEmployer", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.EmployerIdColumn.ColumnName])},
                            { "ddlMSPType", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.MSPTypeIdColumn.ColumnName])},
                            { "chkAssgBenefits", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.AssignBenefitsColumn.ColumnName])},
                            { "ddlBox24IJShaded", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.Box24IJShadedColumn.ColumnName])},
                            { "ddlBox24BShaded", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.Box24BShadedColumn.ColumnName])},
                            { "dtpEligibilityDate",string.IsNullOrEmpty(dr[dsPatient.PatientInsurance.EligibilityDateColumn.ColumnName].ToString())?"":MDVUtility.GetDateMMDDYYY(dr[dsPatient.PatientInsurance.EligibilityDateColumn.ColumnName].ToString())},// MDVUtility.ToStr(dr[dsPatient.PatientInsurance.EligibilityDateColumn.ColumnName])},
                            { "ddlEligibilityStatus", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.EligibilityStatusColumn.ColumnName])},
                            { "chkActive", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.IsActiveColumn.ColumnName])},
                            { "chkUnassigned", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.UnAssignedColumn.ColumnName])},
                            { "txtComments", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.CommentsColumn.ColumnName])},
                            { "ddlInsuranceType", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.InsuranceTypeColumn.ColumnName])},// MDVUtility.ToStr(dr[dsPatient.PatientInsurance.PlanTypeColumn.ColumnName])},
                            { "ddlRelation", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.RelationShipIdColumn.ColumnName])},
                            { "txtLastName", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.LastNameColumn.ColumnName])},
                            { "txtFirstName", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.FirstNameColumn.ColumnName])},
                            { "txtMiddleInitial", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.MIColumn.ColumnName])},
                            { "dtpDOB", string.IsNullOrEmpty(dr[dsPatient.PatientInsurance.DOBColumn.ColumnName].ToString())?"":MDVUtility.GetDateMMDDYYY(dr[dsPatient.PatientInsurance.DOBColumn.ColumnName].ToString())},//Utility.ToStr(dr[dsPatient.PatientInsurance.DOBColumn.ColumnName])},
                            { "ddlSex", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.GenderColumn.ColumnName])},
                            { "txtSSNIns", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.SSNColumn.ColumnName])},
                            { "txtAddress2", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.Address2Column.ColumnName])},
                            { "txtHomeTel", MDVUtility.ToStr(dr[dsPatient.PatientInsurance.HomePhoneNoColumn.ColumnName])},
                           { "FileStream", strBase64},
                           {"hfPatDocId",MDVUtility.ToStr(dr[dsPatient.PatientInsurance.PatDocIdColumn.ColumnName])},
                           {"Scan",MDVUtility.ToStr(dr[dsPatient.PatientInsurance.ScanColumn.ColumnName])},
                           {"OCR",MDVUtility.ToStr(dr[dsPatient.PatientInsurance.OCRColumn.ColumnName])},
                           {"ddlParticipentStatus",MDVUtility.ToStr(dr[dsPatient.PatientInsurance.StatusColumn.ColumnName]) },
                           {"Priority",MDVUtility.ToStr(dr[dsPatient.PatientInsurance.PlanPriorityColumn.ColumnName]) },
                           {"ClaimFlagDescription",MDVUtility.ToStr(dr[dsPatient.PatientInsurance.CFDescriptionColumn.ColumnName]) },
                           {"IPDescription",MDVUtility.ToStr(dr[dsPatient.PatientInsurance.IPDescriptionColumn.ColumnName]) },
                           {"IsReferral",MDVUtility.ToStr(dr[dsPatient.PatientInsurance.isreferralColumn.ColumnName]) },
                               
                        };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            js.MaxJsonLength = Int32.MaxValue;
                            var response = new
                            {
                                status = true,
                                PatientInsuranceId = MDVUtility.ToStr(dr[dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName]), // dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[0][dsPatient.PatientInsurance.InsuranceIdColumn.ColumnName]

                                InsuranceFill_JSON = js.Serialize(keyValues)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = Common.AppPrivileges.No_Record_Message,
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

        /// <summary>
        /// Updates the patient insurances priorities.
        /// </summary>
        /// <param name="PatientId">The patient identifier.</param>
        /// <param name="InsuranceIds">The insurance ids.</param>
        /// <returns></returns>
        private string UpdatePatientInsurancesPriorities(Int64 PatientId, string InsuranceIds)
        {
            try
            {
                string result = string.Empty;
                result = BLLPatientObj.UpdatePatientInsurancesPriorities(PatientId, InsuranceIds);
                BLLSchedule bllSchedule = new BLLSchedule();
                BLObject<List<PatientFutureAppointment>> objPatientFutureAppointment = null;
                objPatientFutureAppointment = bllSchedule.GetPatientFutureAppointment(PatientId);

                var response = new
                {
                    status = true,
                    message = result,
                    PatientFutureAppointment = objPatientFutureAppointment != null ? objPatientFutureAppointment.Data : null
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
        private string ValidateInsuranceFormat(string subscriberID, string format)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(subscriberID)))
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

                    result = BLLAdminInsuranceObj.ValidateSubscriberIDFormat(subscriberID, format);
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    js.MaxJsonLength = Int32.MaxValue;
                    var response = new
                    {
                        status = true,
                        isValid = result,
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

        private string DeleteInsuranceDocument(Int64 PatientInsuranceId, Int64 PatientID)
        {
            try
            {
                if (PatientInsuranceId <= 0)
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
                    BLObject<string> obj = BLLPatientObj.DeleteInsuranceDocument(PatientInsuranceId, PatientID);
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
        private string UpdateInsuranceInscheduler(Int64 PatientInsuranceId, string PatientAppointmentID)
        {
            try
            {
                //string[] arrAppointmentId;

                //arrAppointmentId= PatientAppointmentID.Split(',');

              
                BLLSchedule bllSchedule = new BLLSchedule();
                DSAppointment ds = new DSAppointment();
                BLObject<DSAppointment> objType = null;
                //  objPatientFutureAppointment = bllSchedule.UpdateInsuranceInFutureAppointment(Convert.ToInt64(dsPatient.Tables[dsPatient.PatientInsurance.TableName].Rows[TotalCount - 1][dsPatient.PatientInsurance.PatientIdColumn.ColumnName]));
                string result = string.Empty;
                result = bllSchedule.UpdateInsuranceInFutureAppointment(PatientInsuranceId,PatientAppointmentID );
                var response = new
                {
                    status = true,
                    message = result
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

        private string UpdateAppointmentCopay(string PatientAppointmentData)
        {
            try
            {
                BLLSchedule bllSchedule = new BLLSchedule();
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(PatientAppointmentData);
                string result = string.Empty;

                result = bllSchedule.UpdateAppointmentCopay(SearchedfieldsJSON["AppointmentIds"], MDVUtility.ToInt64(SearchedfieldsJSON["InsuranceId"]));
                var response = new
                {
                    status = true,
                    Message = result
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
        
        private string patientReffralALert(Int64 PatientInsuranceId, Int64 PatientId)
        {
            try
            {                
                string result = string.Empty;               
                result = BLLPatientObj.patientReffralALert(PatientInsuranceId, PatientId);
                var response = new
                {
                    status = true,
                    IsPatientReffralALert = result
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
        #endregion

        #region Service Command Handler
        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            string privilegesMessage = string.Empty;
            switch (cammandAction)
            {
                case "SAVE_PATIENT_INSURANCE":
                    {
                        string fieldsJSON = context.Request["PatientInsuranceData"];
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int32 BStream = MDVUtility.ToInt32(context.Request["BStream"]);
                        string scanImage = MDVUtility.ToStr(context.Request["scanImage"]);
                        bool IsFromCopayUpdateAlert = MDVUtility.ToBool(context.Request["IsFromCopayUpdateAlert"]);
                        string strJSONData = SavePatientInsurance(context.Request.Files, fieldsJSON, PatientID, BStream, scanImage, IsFromCopayUpdateAlert);
                        string FrontImageStream = MDVUtility.ToStr(context.Request["FrontImageStream"]);
                        string BackImageStream = MDVUtility.ToStr(context.Request["BackImageStream"]);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_PATIENT_INSURANCE":
                    {
                        string fieldsJSON = context.Request["PatientInsuranceData"];
                        bool insuranceCardChanged = MDVUtility.ToBool(context.Request["InsuranceCardChanged"]);
                        Int64 PatientInsuranceID = MDVUtility.ToInt64(context.Request["PatientInsuranceID"]);
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int32 BStream = MDVUtility.ToInt32(context.Request["BStream"]);
                        string scanImage = MDVUtility.ToStr(context.Request["scanImage"]);
                        bool IsFromCopayUpdateAlert = MDVUtility.ToBool(context.Request["IsFromCopayUpdateAlert"]);
                        string strJSONData = UpdatePatientInsurance(context.Request.Files, fieldsJSON, PatientID, PatientInsuranceID, BStream, insuranceCardChanged, scanImage, IsFromCopayUpdateAlert);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_PATIENT_INSURANCE":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 PatientInsuranceId = MDVUtility.ToInt64(context.Request["PatientInsuranceId"]);
                        string strJSONData = LoadPatientInsurances(PatientID, PatientInsuranceId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_INSURANCE":
                    {
                        string strJSONData = string.Empty;
                        privilegesMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Insurance", "VIEW")).ToString();
                        if (string.IsNullOrEmpty(privilegesMessage))
                        {
                            Int64 PatientInsuranceID = MDVUtility.ToInt64(context.Request["PatientInsuranceID"]);
                            strJSONData = FillPatientInsurances(PatientInsuranceID);
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
                case "UPDATE_PATIENT_INSURANCE_PRIORITY":
                    {
                        string InsuranceIDs = MDVUtility.ToStr(context.Request["InsurancePriority"]);
                        Int64 PatientId = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = UpdatePatientInsurancesPriorities(PatientId, InsuranceIDs);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "VALIDATE_INSURANCE_FORMAT":
                    {
                        string subscriberID = MDVUtility.ToStr(context.Request["subscriberID"]);
                        string format = MDVUtility.ToStr(context.Request["format"]);
                        string strJSONData = ValidateInsuranceFormat(subscriberID, format);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PATIENT_INSURANCE_DOCUMENT":
                    {
                        Int64 PatientInsuranceId = MDVUtility.ToInt64(context.Request["PatientInsuranceId"]);
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = DeleteInsuranceDocument(PatientInsuranceId, PatientID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_INSURANCE_IN_SCHEDULAR":
                    {
                        Int64 PatientInsuranceId = MDVUtility.ToInt64(context.Request["PatientInsuranceID"]);
                        string PatientschedulerId = MDVUtility.ToStr(context.Request["PatientAppointmentData"]);
                        string strJSONData = UpdateInsuranceInscheduler(PatientInsuranceId, PatientschedulerId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_PATIENT_INSURANCE_FROMLAB":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        Int64 PatientInsuranceId = MDVUtility.ToInt64(context.Request["PatientInsuranceId"]);
                        string strJSONData = LoadPatientInsurancesFromLab(PatientID, PatientInsuranceId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SEARCH_PATIENT_RELATIONSHIP_INFO":
                    {
                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = GetPatientRelationshipInfo(PatientID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "PATIENT_REFERRAL_ALERT":
                    {
                        Int64 PatientInsuranceId = MDVUtility.ToInt64(context.Request["PatientInsuranceID"]);
                        Int64 PatientId = MDVUtility.ToInt64(context.Request["PatientId"]);
                        string strJSONData = patientReffralALert(PatientInsuranceId, PatientId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_APPOINTMENT_COPAY":
                    {
                        string PatientAppointmentData = MDVUtility.ToStr(context.Request["PatientAppointmentData"]);
                        string strJSONData = UpdateAppointmentCopay(PatientAppointmentData);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}