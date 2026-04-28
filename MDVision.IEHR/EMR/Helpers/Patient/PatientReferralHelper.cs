using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Business.MedTextReferrals;
using MDVision.Business.MedTextReferrals.RequestModels;
using MDVision.Business.MedTextReferrals.ResponseModels;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.Patient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Helpers.Patient
{
    public class PatientReferralHelper
    {
        private BLLPatient BLLPatientObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;
        private BLLClinical BLLClinicalObj = null;

        public PatientReferralHelper()
        {

            BLLPatientObj = new BLLPatient();
            BLLAdminProfileObj = new BLLAdminProfile();
            BLLClinicalObj = new BLLClinical();
        }
        /// <summary>
        /// Module Name: InsertUpdatePatientReferral
        /// Author: M Ahmad Imran
        /// Created Date: 12-03=5-2016
        /// Description: Inserts/Updates Patient Referral
        /// </summary>
        /// <param name="model" type="ConsultationOrderModel">ConsultationOrderModel containing data</param>
        public string InsertUpdatePatientReferral(PatientReferralModel model)
        {
            try
            {
                DSPatientReferral dsReferral = new DSPatientReferral();
                //Start 21-03-2016 Humaira Yousaf for search criteria
                BLObject<DSPatientReferral> obj = BLLPatientObj.loadReferral(0, "", MDVUtility.ToLong(model.ReferralId), MDVUtility.ToLong(model.PatientId), "", 0, 0, "", "", 0, "", model.Type, 0, 0, 0, 0);
                //End 21-03-2016 Humaira Yousaf for search criteria

                dsReferral = obj.Data;
                if (obj.Data != null)
                {
                    bool IsUpdate = false;
                    DSPatientReferral.ReferralsRow ReferralRow = null;
                    DSPatientReferral.ReferralsRow[] ReferralRows = (DSPatientReferral.ReferralsRow[])dsReferral.Referrals.Select(dsReferral.Referrals.ReferralIdColumn + "=" + model.ReferralId);
                    if (ReferralRows.Length > 0)
                    {
                        ReferralRow = ReferralRows[0];
                        IsUpdate = true;
                    }
                    else
                    {
                        ReferralRow = dsReferral.Referrals.NewReferralsRow();
                    }
                    if (ReferralRow != null)
                    {
                        ReferralRow.Type = model.Type;
                        ReferralRow.PatientId = MDVUtility.ToInt64(model.PatientId);
                        ReferralRow.ProviderId = MDVUtility.ToInt64(model.ProviderId);
                        ReferralRow.RefProviderId = MDVUtility.ToInt64(model.RefProviderId);
                        if (!string.IsNullOrEmpty(model.PAN))
                        {
                            ReferralRow.PAN = model.PAN;
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.PANColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.Reason))
                        {
                            ReferralRow.Reason = model.Reason;
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.ReasonColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.Visits))
                        {
                            ReferralRow.Visits = MDVUtility.ToInt32(model.Visits);
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.VisitsColumn] = DBNull.Value;
                        }


                        //  ReferralRow.Date = DateTime.Now;

                        if (!string.IsNullOrEmpty(model.Date))
                            ReferralRow.Date = MDVUtility.ToDateTime(model.Date);
                        else
                            ReferralRow[dsReferral.Referrals.DateColumn] = DBNull.Value;

                        if (!string.IsNullOrEmpty(model.AssigneeId))
                        {
                            ReferralRow.AssigneeId = MDVUtility.ToInt64(model.AssigneeId);
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.AssigneeIdColumn] = DBNull.Value;
                        }
                        if (!string.IsNullOrEmpty(model.DateFrom))
                        {
                            ReferralRow.DateFrom = MDVUtility.ToDateTime(model.DateFrom);
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.DateFromColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.DateTo))
                        {
                            ReferralRow.DateTo = MDVUtility.ToDateTime(model.DateTo);
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.DateToColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.PatientInsurance))
                        {
                            ReferralRow.PatientInsurance = MDVUtility.ToLong(model.PatientInsurance);
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.PatientInsuranceColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.FacilityFrom))
                        {
                            ReferralRow.FacilityFrom = MDVUtility.ToLong(model.FacilityFrom);
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.FacilityFromColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.FacilityTo))
                        {
                            ReferralRow.FacilityTo = MDVUtility.ToLong(model.FacilityTo);
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.FacilityToColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.VisitsUsed))
                        {
                            ReferralRow.VisitsUsed = MDVUtility.ToInt(model.VisitsUsed);
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.VisitsUsedColumn] = DBNull.Value;
                        }

                        ReferralRow.IsActive = true;
                        ReferralRow.Time = model.Time;
                        if (!string.IsNullOrEmpty(model.IsActive) && model.IsActive == "0")
                        {
                            ReferralRow.IsActive = false;
                        }

                        if (ReferralRows.Length == 0)
                        {
                            ReferralRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            ReferralRow.CreatedOn = DateTime.Now;
                        }
                        ReferralRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        ReferralRow.ModifiedOn = DateTime.Now;
                        ReferralRow.SoapText = model.SoapText;
                        //Start 22-03-2016 Humaira Yousaf for status
                        ReferralRow.Status = model.Status == "" ? null : model.Status;

                        //End 22-03-2016 Humaira Yousaf for status
                        ReferralRow.Comments = model.Comments;
                        ReferralRow.StatusReasonsIds = model.StatusReasonsIds;
                        if (!string.IsNullOrEmpty(model.NoteId))
                        {
                            ReferralRow.NotesId = MDVUtility.ToLong(model.NoteId);
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.NotesIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.IsDraft))
                        {
                            ReferralRow.IsDraft = MDVUtility.StringToBoolean(model.IsDraft);
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.IsDraftColumn] = DBNull.Value;
                        }
                        


                        if (string.Equals(model.Type, "Outgoing"))
                        {
                            ReferralRow.ToSpecialtyId = MDVUtility.ToLong(model.SpecialtyFrom);
                        }
                        else
                        {
                            ReferralRow[dsReferral.Referrals.ToSpecialtyIdColumn] = DBNull.Value;
                        }

                        if (ReferralRows.Length < 1)
                        {
                            dsReferral.Referrals.AddReferralsRow(ReferralRow);
                        }

                    }

                    #region Database Insertion/Updation
                    BLObject<DSPatientReferral> objConsultation = BLLPatientObj.InsertUpdateReferral(dsReferral);
                    if (objConsultation.Data != null)
                    {
                        dsReferral = objConsultation.Data;
                        long insertedReferralId = MDVUtility.ToInt64(dsReferral.Tables[dsReferral.Referrals.TableName].Rows[0][dsReferral.Referrals.ReferralIdColumn.ColumnName]);

                        if (model.Type == "Outgoing")
                        {
                            model.ReferralProblemList.ForEach(Rpo => Rpo.ReferralId = MDVUtility.ToStr(insertedReferralId));
                            BLLPatientObj.DeleteReferralProblems(MDVUtility.ToStr(insertedReferralId));
                            var id = saveReferralProblem(model.ReferralProblemList);

                            insertUpdateReferralProcedure(insertedReferralId, model.ReferralProcedure);
                        }
                        var ShowMessage = Common.AppPrivileges.Save_Message;
                        if (IsUpdate)
                        {
                            ShowMessage = Common.AppPrivileges.Update_Message;
                        }

                        string MedTextUrl = string.Empty;
                        //If this is an Add case and user have access of MedText Send this Refral to MedText
                        if (MDVUtility.StringToBoolean(model.IsDraft) == false && model.SendVia == "MED" )
                        {
                            MedTextUrl = SaveReferralToMedText(model);
                        }

                        var response = new
                        {
                            status = true,
                            message = ShowMessage,
                            MedTextUrl = MedTextUrl,
                            ReferralId = insertedReferralId,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objConsultation.Message
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

        private string SaveReferralToMedText(PatientReferralModel model)
        {
            string url_ = string.Empty;
            try
            {
                BLObject<DSPatient> obj_ = BLLPatientObj.FillPatient_PictureById(MDVUtility.ToInt64(model.PatientId), "Demographics");
                if (obj_.Data != null)
                {
                    BLLPatient BLLPatientObj = new BLLPatient();
                    DSPatient dsPatient = null;
                    Medtext_Referrals obj = new Medtext_Referrals();

                    dsPatient = obj_.Data;
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        List<problems> problem_list = new List<problems>();
                        List<procedures> procedure_list = new List<procedures>();
                        foreach (var item in model.ReferralProcedure)
                        { procedure_list.Add(new procedures() { code = item.CPTCode, desc = item.CPTCodeDescription, urgency = item.Urgency_text }); }
                        foreach (var item in model.ReferralProblemList)
                        { problem_list.Add(new problems() { code = item.ICD10, desc = item.ProblemName }); }

                        DSPatient.PatientsRow row = (DSPatient.PatientsRow)dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];
                        string token = obj.getMedtextToken();
                        model.Visits_text = model.Visits_text.ToLower() == "select" ? "" : model.Visits_text;
                        checkInResponse response_ = obj.checkInPatient(row, model.ProviderNPI, model.RefProviderNPI, model.Comments, model.Visits_text, model.Reason, problem_list, procedure_list, token);
                       // checkInResponse response_ = obj.checkInPatient(row, model.ProviderNPI,model.RefProviderNPI, problem_list, procedure_list, token);
                        if (response_.IsCheckedIn)
                            url_ = response_.CheckOutURL;
                        else
                            throw new Exception(response_.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return url_;
        }

        // Author: M Ahmad Imran
        // Created Date: 13/05/2016
        //OverView: This function will save Referral Problem
        public string saveReferralProblem(List<PatientReferralProblemListModel> model)
        {
            try
            {
                DSPatientReferral dsReferral = new DSPatientReferral();

                foreach (var m in model)
                {
                    DSPatientReferral.ReferralProblemsRow dr = dsReferral.ReferralProblems.NewReferralProblemsRow();
                    dr.ReferralId = MDVUtility.ToInt64(m.ReferralId);
                    dr.ProblemId = MDVUtility.ToInt32(m.ProblemId);
                    dr.IsActive = true;
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                    dsReferral.ReferralProblems.AddReferralProblemsRow(dr);
                }
                #region Database Insertion
                BLObject<DSPatientReferral> obj = BLLPatientObj.InsertReferralProblems(dsReferral);
                dsReferral = obj.Data;

                if (obj.Data != null && dsReferral.Tables[dsReferral.ReferralProblems.TableName].Rows.Count > 0)
                {

                    Int64 ReferralProblemId = MDVUtility.ToInt64(dsReferral.Tables[dsReferral.ReferralProblems.TableName].Rows[0][dsReferral.ReferralProblems.ReferralProblemIdColumn.ColumnName]);

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        ReferralProblemId = ReferralProblemId,
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

        #region"Referral Procedure"
        public string insertUpdateReferralProcedure(Int64 ReferralId, List<PatientReferralProcedureModel> ProcedureList)
        {
            try
            {
                #region Referral Procedure

                DSPatientReferral dsReferral = new DSPatientReferral();

                BLObject<DSPatientReferral> objReferralProcedure = BLLPatientObj.LoadReferralProcedure(MDVUtility.ToInt64(ReferralId), 0, 0, 0, "1", "2000");
                dsReferral = objReferralProcedure.Data;
                // List<PatientReferralProcedureModel> lstReferralProcedure = lstObjects.OfType<PatientReferralProcedureModel>().ToList();
                int id = -1;
                foreach (PatientReferralProcedureModel CurrentModel in ProcedureList)
                {
                    Int32 currentReferralProcedureId = MDVUtility.ToInt32(CurrentModel.ReferralProcedureId);
                    currentReferralProcedureId = currentReferralProcedureId == 0 ? id-- : currentReferralProcedureId;
                    DSPatientReferral.ReferralProcedureRow RowReferralProcedure = null;
                    if (CurrentModel.ReferralProcedureId != null)
                    {
                        DSPatientReferral.ReferralProcedureRow[] arrReferralProcedureRows = (DSPatientReferral.ReferralProcedureRow[])dsReferral.ReferralProcedure.Select(dsReferral.ReferralProcedure.ReferralProcedureIdColumn.ColumnName + "=" + CurrentModel.ReferralProcedureId);

                        if (arrReferralProcedureRows.Length > 0)
                        {
                            RowReferralProcedure = arrReferralProcedureRows[0];
                        }
                        else
                        {
                            RowReferralProcedure = dsReferral.ReferralProcedure.NewReferralProcedureRow();
                            RowReferralProcedure.ReferralProcedureId = currentReferralProcedureId;
                        }
                        if (RowReferralProcedure != null)
                        {
                            RowReferralProcedure.ReferralId = ReferralId;

                            if (!string.IsNullOrEmpty(CurrentModel.CPTCode))
                            {
                                RowReferralProcedure[dsReferral.ReferralProcedure.CPTCodeColumn] = CurrentModel.CPTCode;
                            }
                            else
                            {
                                RowReferralProcedure[dsReferral.ReferralProcedure.CPTCodeColumn] = DBNull.Value;
                            }
                            if (!string.IsNullOrEmpty(CurrentModel.CPTCodeDescription))
                            {
                                RowReferralProcedure[dsReferral.ReferralProcedure.CPTCodeDescriptionColumn] = CurrentModel.CPTCodeDescription;
                            }
                            else
                            {
                                RowReferralProcedure[dsReferral.ReferralProcedure.CPTCodeDescriptionColumn] = DBNull.Value;
                            }

                            //if (!string.IsNullOrEmpty(CurrentModel.Procedure))
                            //{
                            //    var CptInfo = CurrentModel.Procedure.Split('-');
                            //    var CptCode = "";
                            //    var CptDesc = "";
                            //    if (CptInfo.Length > 1)
                            //    {
                            //        CptCode = CptInfo[0];
                            //        for (var i = 1; i < CptInfo.Length; i++)
                            //        {
                            //            CptDesc = CptDesc + CptInfo[i];
                            //        }
                            //    }
                            //    else if (CptInfo.Length == 1)
                            //    {
                            //        CptDesc = CptInfo[0];
                            //    }
                            //    if (CptCode != "")
                            //    {
                            //        RowReferralProcedure[dsReferral.ReferralProcedure.CPTCodeColumn] = CptCode;
                            //    }
                            //    else
                            //    {
                            //        RowReferralProcedure[dsReferral.ReferralProcedure.CPTCodeColumn] = DBNull.Value;
                            //    }
                            //    if (CptDesc != "")
                            //    {
                            //        RowReferralProcedure[dsReferral.ReferralProcedure.CPTCodeDescriptionColumn] = CptDesc;
                            //    }
                            //    else
                            //    {
                            //        RowReferralProcedure[dsReferral.ReferralProcedure.CPTCodeDescriptionColumn] = DBNull.Value;
                            //    }
                            //}
                            //else
                            //{
                            //    RowReferralProcedure[dsReferral.ReferralProcedure.CPTCodeDescriptionColumn] = DBNull.Value;
                            //}

                            if (!string.IsNullOrEmpty(CurrentModel.Urgency))
                            {
                                RowReferralProcedure[dsReferral.ReferralProcedure.UrgencyIdColumn] = MDVUtility.ToInt32(CurrentModel.Urgency);

                            }
                            else
                            {
                                RowReferralProcedure[dsReferral.ReferralProcedure.UrgencyIdColumn] = DBNull.Value;
                            }
                            RowReferralProcedure.IsActive = true;
                            RowReferralProcedure.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowReferralProcedure.CreatedOn = DateTime.Now;
                            RowReferralProcedure.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            RowReferralProcedure.ModifiedOn = DateTime.Now;

                            if (arrReferralProcedureRows.Length < 1)
                            {
                                dsReferral.ReferralProcedure.AddReferralProcedureRow(RowReferralProcedure);
                            }
                        }
                    }
                }

                #region Database Insertion/Updation

                BLObject<DSPatientReferral> objInsertedReferralProcedure = BLLPatientObj.insertUpdateReferralProcedure(dsReferral);
                if (objInsertedReferralProcedure.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objInsertedReferralProcedure.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }


                #endregion

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
        #endregion

        /*Author: Muhammad Ahmad Imran
    Purpose: To Show Grid Data
    Created on May 14, 2016*/
        public string loadReferralData(PatientReferralModel model)
        {
            try
            {
                DSPatientReferral dsReferral = null;
                BLObject<DSPatientReferral> obj = null;

                if (string.Equals(model.Type, "Outgoing"))
                {
                    obj = BLLPatientObj.loadReferral(MDVUtility.ToLong(model.NoteId), (model.IsActive != null ? model.IsActive : ""), MDVUtility.ToLong(model.ReferralId), MDVUtility.ToLong(model.PatientId), model.CPTCodeDescription, MDVUtility.ToLong(model.RefferalFrom), MDVUtility.ToLong(model.RefferalTo), model.DateFrom, model.DateTo, MDVUtility.ToInt32(model.Status), model.PAN, model.Type, MDVUtility.ToInt32(model.VisitType), MDVUtility.ToInt64(model.PatientInsurance), MDVUtility.ToInt64(model.FacilityFrom), MDVUtility.ToInt64(model.FacilityTo), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "", "", model.IsDraft, model.Source, MDVUtility.ToInt64(model.AssigneeId), model.StatusReasonsIds, model.Date, model.Time, MDVUtility.ToInt64(model.ToSpecialtyId));

                }
                else
                {
                    obj = BLLPatientObj.loadReferral(MDVUtility.ToLong(model.NoteId), (model.IsActive != null ? model.IsActive : ""), MDVUtility.ToLong(model.ReferralId), MDVUtility.ToLong(model.PatientId), model.CPTCodeDescription, MDVUtility.ToLong(model.RefferalTo), MDVUtility.ToLong(model.RefferalFrom), model.DateFrom, model.DateTo, MDVUtility.ToInt32(model.Status), model.PAN, model.Type, MDVUtility.ToInt32(model.VisitType), MDVUtility.ToInt64(model.PatientInsurance), MDVUtility.ToInt64(model.FacilityFrom), MDVUtility.ToInt64(model.FacilityTo), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "", "", model.IsDraft, model.Source, MDVUtility.ToInt64(model.AssigneeId), model.StatusReasonsIds, model.Date, model.Time, MDVUtility.ToInt64(model.ToSpecialtyId));
                }

                if (obj != null)
                {
                    dsReferral = obj.Data;
                }
                if (dsReferral != null && dsReferral.Referrals.Rows.Count > 0)
                {


                    string MedTextURL = new Medtext_Referrals().getMedtextCheckOutURL();
                    DataRow dr = dsReferral.Tables[dsReferral.Referrals.TableName].Rows[0];

                    var ReferralData = new Dictionary<string, string>
                            {
                                {"ReferralId",MDVUtility.ToStr(dr[dsReferral.Referrals.ReferralIdColumn.ColumnName])},
                                { "Date", MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateColumn.ColumnName]).ToShortDateString())},
                                { "Time", MDVUtility.ToStr(dr[dsReferral.Referrals.TimeColumn.ColumnName])},
                                { "Provider", MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderNameColumn.ColumnName])},
                                { "ProviderId", MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderIdColumn.ColumnName])},
                                { "RefProvider", string.Equals(model.Type, "Outgoing") ? string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderNameColumn.ColumnName])) ? MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderNameColumn.ColumnName]) : MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderNameColumn.ColumnName]) + " (" + MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderEntityNameColumn.ColumnName]) + ")" : MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderNameColumn.ColumnName])},
                                { "RefProviderId", MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderIdColumn.ColumnName])},
                                { "Assignee", MDVUtility.ToStr(dr[dsReferral.Referrals.AssigneeNameColumn.ColumnName])},
                                { "AssigneeId", MDVUtility.ToStr(dr[dsReferral.Referrals.AssigneeIdColumn.ColumnName])},
                                { "Status", MDVUtility.ToStr(dr[dsReferral.Referrals.StatusColumn.ColumnName])},
                                { "PAN", MDVUtility.ToStr(dr[dsReferral.Referrals.PANColumn.ColumnName])},
                                { "Visits", MDVUtility.ToStr(dr[dsReferral.Referrals.VisitsColumn.ColumnName])},
                                { "Reason", MDVUtility.ToStr(dr[dsReferral.Referrals.ReasonColumn.ColumnName])},
                                { "DateFrom", MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateFromColumn.ColumnName]).ToShortDateString())},
                                { "DateTo", MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateToColumn.ColumnName]).ToShortDateString())},
                                { "PatientInsurance", MDVUtility.ToStr(dr[dsReferral.Referrals.PatientInsuranceColumn.ColumnName])},
                                { "FacilityFrom", MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityFromColumn.ColumnName])},
                                { "FacilityTo", MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityToColumn.ColumnName])},
                                { "VisitsUsed", MDVUtility.ToStr(dr[dsReferral.Referrals.VisitsUsedColumn.ColumnName])},
                                { "InsurancePlan", MDVUtility.ToStr(dr[dsReferral.Referrals.PatientInsuranceNameColumn.ColumnName])},
                                { "Comments", MDVUtility.ToStr(dr[dsReferral.Referrals.CommentsColumn.ColumnName])},
                                { "FacilityToName", MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityToNameColumn.ColumnName])},
                                { "FacilityFromName", MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityFromNameColumn.ColumnName])},
                                { "PatientId", MDVUtility.ToStr(dr[dsReferral.Referrals.PatientIdColumn.ColumnName])},
                                { "PatientInsuranceName", MDVUtility.ToStr(dr[dsReferral.Referrals.PatientInsuranceNameColumn.ColumnName])},
                                { "VisitTypeDescription", MDVUtility.ToStr(dr[dsReferral.Referrals.VisitTypeDescriptionColumn.ColumnName])},
                                { "PatientName", MDVUtility.ToStr(dr[dsReferral.Referrals.PatientNameColumn.ColumnName])},
                                { "AccountNumber", MDVUtility.ToStr(dr[dsReferral.Referrals.AccountNumberColumn.ColumnName])},
                                { "SpecialtyFromName", MDVUtility.ToStr(dr[dsReferral.Referrals.ToSpecialtyNameColumn.ColumnName])},
                                { "SpecialtyFrom", MDVUtility.ToStr(dr[dsReferral.Referrals.ToSpecialtyIdColumn.ColumnName])},
                                { "MedTextAppointmentId", MDVUtility.ToStr(dr[dsReferral.Referrals.MedTextAppointmentIdColumn.ColumnName])},
                                { "IsDraft", MDVUtility.ToStr(dr[dsReferral.Referrals.IsDraftColumn.ColumnName])},
                                { "StatusReasonIds", MDVUtility.ToStr(dr[dsReferral.Referrals.StatusReasonsIdsColumn.ColumnName])}
                            };
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    if (model.LoadFor == "Edit")
                    {
                        DSPatientReferral dsReferralProcedure = new DSPatientReferral();
                        BLObject<DSPatientReferral> objReferralProcedure = BLLPatientObj.LoadReferralProcedure(MDVUtility.ToInt64(model.ReferralId), 0, 0, 0, "1", "2000");
                        dsReferralProcedure = objReferralProcedure.Data;
                        if (dsReferralProcedure != null)
                        {
                            DSPatientReferral dsReferralProblem = new DSPatientReferral();
                            BLObject<DSPatientReferral> objReferralProblem = BLLPatientObj.LoadReferralProblems(MDVUtility.ToInt32(model.ReferralId), MDVUtility.ToInt64(model.PatientId), 1, 2000);
                            dsReferralProblem = objReferralProblem.Data;

                            if (dsReferralProblem != null)
                            {
                                var response = new
                                {
                                    status = true,
                                    ReferralListCount = dsReferral.Tables[dsReferral.Referrals.TableName].Rows.Count,
                                    ProcedureListCount = dsReferralProcedure.Tables[dsReferralProcedure.ReferralProcedure.TableName].Rows.Count,
                                    ProblemListCount = dsReferralProblem.Tables[dsReferralProblem.ReferralProblems.TableName].Rows.Count,
                                    ReferralProblemListLoad_JSON = MDVUtility.JSON_DataTable(dsReferralProblem.Tables[dsReferralProblem.ReferralProblems.TableName]),
                                    ReferralProcedureListLoad_JSON = MDVUtility.JSON_DataTable(dsReferralProcedure.Tables[dsReferralProcedure.ReferralProcedure.TableName]),
                                    MedTextURL = new Medtext_Referrals().getMedtextCheckOutURL(),
                                    ReferralListLoad_JSON = js.Serialize(ReferralData),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    ProblemListCount = 0,
                                    ProcedureListCount = dsReferralProcedure.Tables[dsReferralProcedure.ReferralProcedure.TableName].Rows.Count,
                                    ReferralProcedureListLoad_JSON = MDVUtility.JSON_DataTable(dsReferralProcedure.Tables[dsReferralProcedure.ReferralProcedure.TableName]),
                                    ReferralListCount = dsReferral.Tables[dsReferral.Referrals.TableName].Rows.Count,
                                    MedTextURL = MedTextURL,
                                    ReferralListLoad_JSON = js.Serialize(ReferralData),
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                        }
                        else
                        {

                            var response = new
                            {
                                status = true,
                                ProcedureListCount = 0,
                                ProblemListCount = 0,
                                MedTextURL = MedTextURL,
                                ReferralListCount = dsReferral.Tables[dsReferral.Referrals.TableName].Rows.Count,
                                ReferralListLoad_JSON = js.Serialize(ReferralData),

                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }


                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ReferralListCount = dsReferral.Tables[dsReferral.Referrals.TableName].Rows.Count,
                            ReferralListLoad_JSON = MDVUtility.JSON_DataTable(dsReferral.Tables[dsReferral.Referrals.TableName]),
                            MedTextURL = MedTextURL,
                            iTotalDisplayRecords = (dsReferral.Referrals.Rows.Count > 0) ? dsReferral.Referrals.Rows[0][dsReferral.Referrals.RecordCountColumn.ColumnName] : 0,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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


        public string SearchFacilityByName(string SearchString)
        {
            try
            {
                DSProfileLookup dsFacility = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.LookupFacilityByName(SearchString);
                dsFacility = obj.Data;
                if (obj.Data != null)
                {
                    if (dsFacility.Tables[dsFacility.Facility.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = dsFacility.Tables[dsFacility.Facility.TableName].Rows.Count,
                            PatientLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsFacility.Tables[dsFacility.Facility.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = 0,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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

        public string SearchProviderByName(string SearchString)
        {
            try
            {
                DSProfileLookup dsProvider = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.LookupProviderByName(SearchString);
                dsProvider = obj.Data;
                if (obj.Data != null)
                {
                    if (dsProvider.Tables[dsProvider.Provider.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = dsProvider.Tables[dsProvider.Provider.TableName].Rows.Count,
                            PatientLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsProvider.Tables[dsProvider.Provider.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = 0,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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

        public string SearchSpecialtyByName(string SearchString)
        {
            try
            {
                DSProfileLookup dsSpecialty = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.LookupSpecialtyByName(SearchString);
                dsSpecialty = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSpecialty.Tables[dsSpecialty.Specialty.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = dsSpecialty.Tables[dsSpecialty.Specialty.TableName].Rows.Count,
                            PatientLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsSpecialty.Tables[dsSpecialty.Specialty.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = 0,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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

        public string SearchRefProviderByName(string SearchString)
        {
            try
            {
                DSProfileLookup dsRefProvider = null;
                BLObject<DSProfileLookup> obj;
                obj = BLLAdminProfileObj.LookupRefProviderByName(SearchString);
                dsRefProvider = obj.Data;
                if (obj.Data != null)
                {
                    if (dsRefProvider.Tables[dsRefProvider.ReferringProvider.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = dsRefProvider.Tables[dsRefProvider.ReferringProvider.TableName].Rows.Count,
                            PatientLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsRefProvider.Tables[dsRefProvider.ReferringProvider.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientCount = 0,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
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
        public string ActiveInActiveReferral(PatientReferralModel model)
        {
            try
            {
                if (MDVUtility.ToInt64(model.ReferralId) > 0)
                {


                    DSPatientReferral dsReferral = new DSPatientReferral();
                    //Start 21-03-2016 Humaira Yousaf for search criteria
                    BLObject<DSPatientReferral> obj = BLLPatientObj.loadReferral(0, "", MDVUtility.ToLong(model.ReferralId), MDVUtility.ToLong(model.PatientId), "", 0, 0, "", "", 0, "", model.Type, 0, 0, 0, 0);


                    dsReferral = obj.Data;
                    foreach (DSPatientReferral.ReferralsRow dr in dsReferral.Tables[dsReferral.Referrals.TableName].Rows)
                    {

                        dr.IsActive = MDVUtility.ToStr(model.IsActiveRecord) == "1" ? true : false;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }

                    #region Database Updation
                    if (dsReferral.Tables[dsReferral.Referrals.TableName].Rows.Count > 0)
                    {
                        BLObject<DSPatientReferral> objUpdate = BLLPatientObj.InsertUpdateReferral(dsReferral);
                        if (objUpdate.Data != null)
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
                                message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            message = ""
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
                        message = "Referral not found."
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

        public string deleteReferralProcedure(string ReferralProcedureId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLPatientObj.deleteReferralProcedure(ReferralProcedureId);
                result = obj.Data;

                if (result == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,

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
            catch (Exception)
            {

                throw;
            }
        }


        public string LoadProblemLists(PatientReferralModel model)
        {
            try
            {

                DSProblemLists dsProblemList = null;
                BLObject<DSProblemLists> obj;

                obj = BLLPatientObj.LoadProblemLists(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));

                dsProblemList = obj.Data;
                if (obj.Data != null)
                {
                    int ProblemListTotalCount = 0;
                    if (dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count == 0)
                    {
                        BLObject<string> obj1 = BLLClinicalObj.CheckProblemListExists(MDVUtility.ToLong(model.PatientId));
                        if (obj1.Data == "1")
                        {
                            ProblemListTotalCount = 1;
                        }
                        else
                        {
                            ProblemListTotalCount = 0;
                        }
                    }
                    else
                    {
                        ProblemListTotalCount = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count;
                    }
                    var response = new
                    {
                        status = true,
                        ProblemListTotalCount = ProblemListTotalCount,
                        ProblemListCount = dsProblemList.Tables[dsProblemList.ProblemList.TableName].Rows.Count,
                        ProblemListLoad_JSON = MDVUtility.JSON_DataTable(dsProblemList.Tables[dsProblemList.ProblemList.TableName]),
                        ProblemListHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsProblemList.Tables[dsProblemList.ProblemHistory.TableName]),
                        iTotalDisplayRecords = (dsProblemList.ProblemList.Rows.Count > 0) ? dsProblemList.ProblemList.Rows[0][dsProblemList.ProblemList.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ProblemListCount = 0,
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

        public string deleteReferral(string ReferralIds)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLPatientObj.deleteReferral(ReferralIds);
                result = obj.Data;

                if (result == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = result
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        ///start Notes Work
        /// /// <summary>
        /// This Function will detach Referral from notes
        /// </summary>
        /// <param name="ReferralId"></param>
        /// <param name="PatientID"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string detach_Referral_From_Notes(string ReferralId, long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ReferralId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLPatientObj.detachReferralFromNotes(ReferralId, NotesId);
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

        /// <summary>
        /// This Function will attach Referral to notes
        /// </summary>
        /// <param name="ReferralId"></param>
        /// <param name="PatientID"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string attach_Referral_With_Notes(string ReferralId, long NotesId)
        {
            try
            {
                DSPatientReferral dsReferral = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(ReferralId)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<DSPatientReferral> obj = BLLPatientObj.attachReferralListWithNotes(ReferralId, NotesId);
                    if (obj.Data != null)
                    {
                        dsReferral = obj.Data;
                        var response = new
                        {
                            status = true,
                            ReferralTotalCount = dsReferral.Tables[dsReferral.Referrals.TableName].Rows.Count,
                            ReferralCount = dsReferral.Tables[dsReferral.Referrals.TableName].Rows.Count,
                            ReferralLoad_JSON = MDVUtility.JSON_DataTable(dsReferral.Tables[dsReferral.Referrals.TableName]),
                            Message = Common.AppPrivileges.Update_Message
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

        /// <summary>
        /// this function will get latest allergy for notes attachment
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public string getLatestReferralByPatientId(Int64 PatientId, Int64 ProviderId)
        {
            try
            {

                DSPatientReferral dsReferral = null;
                BLObject<DSPatientReferral> obj;
                obj = BLLPatientObj.getLatestReferralByPatientId(PatientId, ProviderId);
                dsReferral = obj.Data;
                List<PatientReferralModel> ReferralModel = new List<PatientReferralModel>();
                if (dsReferral != null && dsReferral.Referrals.Rows.Count > 0)
                {
                    for (int i = 0; i < dsReferral.Referrals.Rows.Count; i++)
                    {
                        PatientReferralModel CurrentReferralModel = new PatientReferralModel();
                        DataRow dr = dsReferral.Tables[dsReferral.Referrals.TableName].Rows[i];

                        CurrentReferralModel.ReferralId = MDVUtility.ToStr(dr[dsReferral.Referrals.ReferralIdColumn.ColumnName]);
                        CurrentReferralModel.Date = MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateColumn.ColumnName]).ToShortDateString();
                        CurrentReferralModel.Time = MDVUtility.ToStr(dr[dsReferral.Referrals.TimeColumn.ColumnName]);
                        CurrentReferralModel.Provider = MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderNameColumn.ColumnName]);
                        CurrentReferralModel.ProviderId = MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderIdColumn.ColumnName]);
                        CurrentReferralModel.RefProvider = MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderNameColumn.ColumnName]);
                        CurrentReferralModel.RefProviderId = MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderIdColumn.ColumnName]);
                        CurrentReferralModel.Assignee = MDVUtility.ToStr(dr[dsReferral.Referrals.AssigneeNameColumn.ColumnName]);
                        CurrentReferralModel.AssigneeId = MDVUtility.ToStr(dr[dsReferral.Referrals.AssigneeIdColumn.ColumnName]);
                        CurrentReferralModel.Status = MDVUtility.ToStr(dr[dsReferral.Referrals.StatusColumn.ColumnName]);
                        CurrentReferralModel.PAN = MDVUtility.ToStr(dr[dsReferral.Referrals.PANColumn.ColumnName]);
                        CurrentReferralModel.Visits = MDVUtility.ToStr(dr[dsReferral.Referrals.VisitsColumn.ColumnName]);
                        CurrentReferralModel.Reason = MDVUtility.ToStr(dr[dsReferral.Referrals.ReasonColumn.ColumnName]);
                        CurrentReferralModel.Type = MDVUtility.ToStr(dr[dsReferral.Referrals.TypeColumn.ColumnName]);
                        CurrentReferralModel.DateFrom = MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateFromColumn.ColumnName]).ToShortDateString());
                        CurrentReferralModel.DateTo = MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateToColumn.ColumnName]).ToShortDateString());
                        CurrentReferralModel.PatientInsuranceName = MDVUtility.ToStr(dr[dsReferral.Referrals.PatientInsuranceNameColumn.ColumnName]);
                        CurrentReferralModel.FacilityFromName = MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityFromNameColumn.ColumnName]);
                        CurrentReferralModel.FacilityToName = MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityToNameColumn.ColumnName]);
                        CurrentReferralModel.VisitsUsed = MDVUtility.ToStr(dr[dsReferral.Referrals.VisitsUsedColumn.ColumnName]);
                        CurrentReferralModel.Comments = MDVUtility.ToStr(dr[dsReferral.Referrals.CommentsColumn.ColumnName]);
                        CurrentReferralModel.SpecialityToName = MDVUtility.ToStr(dr[dsReferral.Referrals.SpecialityToNameColumn.ColumnName]);
                        CurrentReferralModel.FacilityFrom = MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityFromColumn.ColumnName]);
                        CurrentReferralModel.FacilityTo = MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityToColumn.ColumnName]);

                        DSPatientReferral dsReferralProcedure = new DSPatientReferral();
                        BLObject<DSPatientReferral> objReferralProcedure = BLLPatientObj.LoadReferralProcedure(MDVUtility.ToInt64(CurrentReferralModel.ReferralId), 0, 0, ProviderId, "1", "2000");
                        dsReferralProcedure = objReferralProcedure.Data;
                        List<PatientReferralProcedureModel> ReferralProedures = new List<PatientReferralProcedureModel>();
                        if (dsReferralProcedure != null)
                        {

                            for (int j = 0; j < dsReferralProcedure.ReferralProcedure.Rows.Count; j++)
                            {
                                PatientReferralProcedureModel CurrentReferralProcedureModel = new PatientReferralProcedureModel();
                                DataRow drProcedure = dsReferralProcedure.Tables[dsReferralProcedure.ReferralProcedure.TableName].Rows[j];

                                CurrentReferralProcedureModel.ReferralProcedureId = MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.ReferralProcedureIdColumn.ColumnName]);
                                CurrentReferralProcedureModel.Procedure = MDVUtility.ToStr((drProcedure[dsReferralProcedure.ReferralProcedure.CPTCodeColumn.ColumnName]) != "" ? (MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.CPTCodeColumn.ColumnName]) + " ") : "") + MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.CPTCodeDescriptionColumn.ColumnName]);
                                CurrentReferralProcedureModel.CPTCode = MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.CPTCodeColumn.ColumnName]);
                                CurrentReferralProcedureModel.CPTCodeDescription = MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.CPTCodeDescriptionColumn.ColumnName]);
                                CurrentReferralProcedureModel.Urgency = MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.UrgencyColumn.ColumnName]);
                                CurrentReferralProcedureModel.ShowCPTCode = MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.ShowCPTCodeColumn.ColumnName]);
                                ReferralProedures.Add(CurrentReferralProcedureModel);
                            }
                            CurrentReferralModel.ReferralProcedure = ReferralProedures;
                        }
                        DSPatientReferral dsReferralProblem = new DSPatientReferral();
                        BLObject<DSPatientReferral> objReferralProblem = BLLPatientObj.LoadReferralProblems(MDVUtility.ToInt32(CurrentReferralModel.ReferralId), 0, 1, 2000);
                        dsReferralProblem = objReferralProblem.Data;
                        List<PatientReferralProblemListModel> ReferralProblems = new List<PatientReferralProblemListModel>();
                        if (dsReferralProblem != null)
                        {
                            for (int j = 0; j < dsReferralProblem.ReferralProblems.Rows.Count; j++)
                            {
                                PatientReferralProblemListModel CurrentReferralProblemModel = new PatientReferralProblemListModel();
                                DataRow drProblem = dsReferralProblem.Tables[dsReferralProblem.ReferralProblems.TableName].Rows[j];
                                CurrentReferralProblemModel.ReferralProblemId = MDVUtility.ToStr(drProblem[dsReferralProblem.ReferralProblems.ReferralProblemIdColumn.ColumnName]);
                                CurrentReferralProblemModel.ProblemName = MDVUtility.ToStr(drProblem[dsReferralProblem.ReferralProblems.ProblemNameColumn.ColumnName]);
                                ReferralProblems.Add(CurrentReferralProblemModel);
                            }
                            CurrentReferralModel.ReferralProblemList = ReferralProblems;
                        }
                        ReferralModel.Add(CurrentReferralModel);

                    };

                    //var response = new
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ReferralListCount = dsReferral.Tables[dsReferral.Referrals.TableName].Rows.Count,
                        ReferralData_JSON = js.Serialize(ReferralModel),
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

        public string getReferralForSoap(string ReferralId, long PatientId, long notesId, long ProviderId)
        {
            try
            {

                DSPatientReferral dsReferral = null;
                DSProblemLists dsProblemSoap = null;
                BLObject<DSPatientReferral> obj;
                obj = BLLPatientObj.loadReferralForSoap(ReferralId, PatientId, ProviderId);
                BLObject<DSProblemLists> objPrb = BLLClinicalObj.attachReferralProblemsWithNoteForSoap(ReferralId, notesId);

                dsReferral = obj.Data;
                dsProblemSoap = objPrb.Data;
                List<PatientReferralModel> ReferralModel = new List<PatientReferralModel>();
                if (dsReferral != null && dsReferral.Referrals.Rows.Count > 0)
                {
                    for (int i = 0; i < dsReferral.Referrals.Rows.Count; i++)
                    {
                        PatientReferralModel CurrentReferralModel = new PatientReferralModel();
                        DataRow dr = dsReferral.Tables[dsReferral.Referrals.TableName].Rows[i];

                        CurrentReferralModel.ReferralId = MDVUtility.ToStr(dr[dsReferral.Referrals.ReferralIdColumn.ColumnName]);
                        CurrentReferralModel.Date = MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateColumn.ColumnName]).ToShortDateString();
                        CurrentReferralModel.Time = MDVUtility.ToStr(dr[dsReferral.Referrals.TimeColumn.ColumnName]);
                        CurrentReferralModel.Provider = MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderNameColumn.ColumnName]);
                        CurrentReferralModel.ProviderId = MDVUtility.ToStr(dr[dsReferral.Referrals.ProviderIdColumn.ColumnName]);
                        CurrentReferralModel.RefProvider = MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderNameColumn.ColumnName]);
                        CurrentReferralModel.RefProviderId = MDVUtility.ToStr(dr[dsReferral.Referrals.RefProviderIdColumn.ColumnName]);
                        CurrentReferralModel.Assignee = MDVUtility.ToStr(dr[dsReferral.Referrals.AssigneeNameColumn.ColumnName]);
                        CurrentReferralModel.AssigneeId = MDVUtility.ToStr(dr[dsReferral.Referrals.AssigneeIdColumn.ColumnName]);
                        CurrentReferralModel.Status = MDVUtility.ToStr(dr[dsReferral.Referrals.StatusColumn.ColumnName]);
                        CurrentReferralModel.PAN = MDVUtility.ToStr(dr[dsReferral.Referrals.PANColumn.ColumnName]);
                        CurrentReferralModel.Visits = MDVUtility.ToStr(dr[dsReferral.Referrals.VisitsColumn.ColumnName]);
                        CurrentReferralModel.Reason = MDVUtility.ToStr(dr[dsReferral.Referrals.ReasonColumn.ColumnName]);
                        CurrentReferralModel.Type = MDVUtility.ToStr(dr[dsReferral.Referrals.TypeColumn.ColumnName]);
                        CurrentReferralModel.DateFrom = MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateFromColumn.ColumnName]).ToShortDateString());
                        CurrentReferralModel.DateTo = MDVUtility.ToStr(MDVUtility.ToDateTime(dr[dsReferral.Referrals.DateToColumn.ColumnName]).ToShortDateString());
                        CurrentReferralModel.PatientInsuranceName = MDVUtility.ToStr(dr[dsReferral.Referrals.PatientInsuranceNameColumn.ColumnName]);
                        CurrentReferralModel.FacilityFromName = MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityFromNameColumn.ColumnName]);
                        CurrentReferralModel.FacilityToName = MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityToNameColumn.ColumnName]);
                        CurrentReferralModel.VisitsUsed = MDVUtility.ToStr(dr[dsReferral.Referrals.VisitsUsedColumn.ColumnName]);
                        CurrentReferralModel.Comments = MDVUtility.ToStr(dr[dsReferral.Referrals.CommentsColumn.ColumnName]);
                        CurrentReferralModel.SpecialityToName = MDVUtility.ToStr(dr[dsReferral.Referrals.SpecialityToNameColumn.ColumnName]);
                        CurrentReferralModel.FacilityFrom = MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityFromColumn.ColumnName]);
                        CurrentReferralModel.FacilityTo = MDVUtility.ToStr(dr[dsReferral.Referrals.FacilityToColumn.ColumnName]);


                        DSPatientReferral dsReferralProcedure = new DSPatientReferral();
                        BLObject<DSPatientReferral> objReferralProcedure = BLLPatientObj.LoadReferralProcedure(MDVUtility.ToInt64(CurrentReferralModel.ReferralId), 0, 0, ProviderId, "1", "2000");
                        dsReferralProcedure = objReferralProcedure.Data;
                        List<PatientReferralProcedureModel> ReferralProedures = new List<PatientReferralProcedureModel>();
                        if (dsReferralProcedure != null)
                        {

                            for (int j = 0; j < dsReferralProcedure.ReferralProcedure.Rows.Count; j++)
                            {
                                PatientReferralProcedureModel CurrentReferralProcedureModel = new PatientReferralProcedureModel();
                                DataRow drProcedure = dsReferralProcedure.Tables[dsReferralProcedure.ReferralProcedure.TableName].Rows[j];

                                CurrentReferralProcedureModel.ReferralProcedureId = MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.ReferralProcedureIdColumn.ColumnName]);
                                CurrentReferralProcedureModel.Procedure = MDVUtility.ToStr((drProcedure[dsReferralProcedure.ReferralProcedure.CPTCodeColumn.ColumnName]) != "" ? (MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.CPTCodeColumn.ColumnName]) + " ") : "") + MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.CPTCodeDescriptionColumn.ColumnName]);
                                CurrentReferralProcedureModel.CPTCode = MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.CPTCodeColumn.ColumnName]);
                                CurrentReferralProcedureModel.CPTCodeDescription = MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.CPTCodeDescriptionColumn.ColumnName]);
                                CurrentReferralProcedureModel.Urgency = MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.UrgencyColumn.ColumnName]);
                                CurrentReferralProcedureModel.ShowCPTCode = MDVUtility.ToStr(drProcedure[dsReferralProcedure.ReferralProcedure.ShowCPTCodeColumn.ColumnName]);
                                ReferralProedures.Add(CurrentReferralProcedureModel);
                            }
                            CurrentReferralModel.ReferralProcedure = ReferralProedures;
                        }
                        DSPatientReferral dsReferralProblem = new DSPatientReferral();
                        BLObject<DSPatientReferral> objReferralProblem = BLLPatientObj.LoadReferralProblems(MDVUtility.ToInt32(CurrentReferralModel.ReferralId), 0, 1, 2000);
                        dsReferralProblem = objReferralProblem.Data;
                        List<PatientReferralProblemListModel> ReferralProblems = new List<PatientReferralProblemListModel>();
                        if (dsReferralProblem != null)
                        {
                            for (int j = 0; j < dsReferralProblem.ReferralProblems.Rows.Count; j++)
                            {
                                PatientReferralProblemListModel CurrentReferralProblemModel = new PatientReferralProblemListModel();
                                DataRow drProblem = dsReferralProblem.Tables[dsReferralProblem.ReferralProblems.TableName].Rows[j];
                                CurrentReferralProblemModel.ReferralProblemId = MDVUtility.ToStr(drProblem[dsReferralProblem.ReferralProblems.ReferralProblemIdColumn.ColumnName]);
                                CurrentReferralProblemModel.ProblemName = MDVUtility.ToStr(drProblem[dsReferralProblem.ReferralProblems.ProblemNameColumn.ColumnName]);
                                ReferralProblems.Add(CurrentReferralProblemModel);
                            }
                            CurrentReferralModel.ReferralProblemList = ReferralProblems;
                        }
                        ReferralModel.Add(CurrentReferralModel);

                    };

                    //var response = new
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        ReferralListCount = dsReferral.Tables[dsReferral.Referrals.TableName].Rows.Count,
                        ReferralData_JSON = js.Serialize(ReferralModel),
                        ProblemListSoapCount = objPrb.Data != null && dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count > 0 ? dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count : 0,
                        ProblemListSoap_JSON = objPrb.Data != null && dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count > 0 ? MDVUtility.JSON_DataTable(dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName]) : "[]",
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
                        ProblemListSoapCount = objPrb.Data != null && dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count > 0 ? dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count : 0,
                        ProblemListSoap_JSON = objPrb.Data != null && dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName].Rows.Count > 0 ? MDVUtility.JSON_DataTable(dsProblemSoap.Tables[dsProblemSoap.ProblemListSoap.TableName]) : "[]",
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
        ///end Notes Work
        ////// Method Name: previewReferral
        /// Author : M Ahmad Imran
        /// Created Date: 17-05-2016
        /// Description: Creates PDF to view Referral
        /// </summary>
        /// <param name="model" type="ConsultationOrderModel">model</param>
        public string previewReferral(PatientReferralModel model)
        {
            try
            {
                BLObject<byte[]> obj = BLLPatientObj.previewReferral(MDVUtility.ToInt64(model.ReferralId), MDVUtility.ToInt64(model.PatientId));

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ReferralHTML = Convert.ToBase64String(obj.Data),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        FaceSheetCount = 0,
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
        ////// Method Name: loadReferralProvider
        /// Author : Humaira Yousaf
        /// Created Date: 15-08-2016
        /// Description: Loads referral providers
        /// </summary>
        /// <param name="model" type="PatientReferralModel">model</param>
        public string loadReferralProvider(PatientReferralModel model)
        {

            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> listFrom = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();
            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> listTo = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();

            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> incomingReferralFrom = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();
            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> incomingReferralTo = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();

            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> outgoingReferralFrom = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();
            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> outgoingReferralTo = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();


            DSPatientReferral dsPatientReferral = null;
            BLObject<DSProfileLookup> objProvider = BLLAdminProfileObj.LookupAllProviders("true");
            DSProfileLookup dsProvider = objProvider.Data;

            BLObject<DSProfileLookup> objRefProvider = BLLAdminProfileObj.LookupReferringProviderOutgoing("true");
            DSProfileLookup dsRefProvider = objRefProvider.Data;


            BLObject<DSPatientReferral> obj;
            obj = BLLPatientObj.loadReferral(0, "1", 0, MDVUtility.ToLong(model.PatientId), "", 0, 0, "", "", 0, "", "", 0, 0, 0, 0);
            dsPatientReferral = obj.Data;
            listFrom.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));
            listTo.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));

            incomingReferralFrom.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));
            incomingReferralTo.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));

            outgoingReferralFrom.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));
            outgoingReferralTo.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));

            if (dsPatientReferral != null)
            {
                if (dsProvider.Tables[dsProvider.Provider.TableName] != null)
                {
                    DataView view = new DataView(dsPatientReferral.Tables[dsPatientReferral.Referrals.TableName]);

                    if (view.Count > 0)
                    {
                        DataTable distinctValues = view.ToTable(true, dsPatientReferral.Referrals.ProviderIdColumn.ColumnName, dsPatientReferral.Referrals.TypeColumn.ColumnName);
                        foreach (DataRow drProv in dsProvider.Tables[dsProvider.Provider.TableName].Rows)
                        {
                            for (int i = 0; i < distinctValues.Rows.Count; i++)
                            {

                                if (MDVUtility.ToInt64(drProv[dsProvider.Provider.ProviderIdColumn]) == MDVUtility.ToInt64(distinctValues.Rows[i][0].ToString()) && MDVUtility.ToStr(distinctValues.Rows[i][1]) == "Incoming")
                                {
                                    incomingReferralTo.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair(drProv[dsProvider.Provider.ShortNameColumn.ColumnName].ToString(), drProv[dsProvider.Provider.ProviderIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.EntityIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                                }

                                if (MDVUtility.ToInt64(drProv[dsProvider.Provider.ProviderIdColumn]) == MDVUtility.ToInt64(distinctValues.Rows[i][0].ToString()) && MDVUtility.ToStr(distinctValues.Rows[i][1]) == "Outgoing")
                                {
                                    outgoingReferralFrom.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair(drProv[dsProvider.Provider.ShortNameColumn.ColumnName].ToString(), drProv[dsProvider.Provider.ProviderIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.EntityIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                                }
                            }
                        }

                        //DataTable distinctValues = view.ToTable(true, dsPatientReferral.Referrals.ProviderIdColumn.ColumnName, dsPatientReferral.Referrals.TypeColumn.ColumnName);
                        //foreach (DataRow drProv in dsProvider.Tables[dsProvider.Provider.TableName].Rows)
                        //{
                        //    for (int i = 0; i < distinctValues.Rows.Count; i++)
                        //    {

                        //        if (MDVUtility.ToInt64(drProv[dsProvider.Provider.ProviderIdColumn]) == MDVUtility.ToInt64(distinctValues.Rows[i][0].ToString()) && MDVUtility.ToStr(distinctValues.Rows[i][1]) == "Incoming")
                        //        {
                        //            incomingReferralTo.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair(drProv[dsProvider.Provider.ShortNameColumn.ColumnName].ToString(), drProv[dsProvider.Provider.ProviderIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.EntityIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                        //        }

                        //        if (MDVUtility.ToInt64(drProv[dsProvider.Provider.ProviderIdColumn]) == MDVUtility.ToInt64(distinctValues.Rows[i][0].ToString()) && MDVUtility.ToStr(distinctValues.Rows[i][1]) == "Outgoing")
                        //        {
                        //            outgoingReferralFrom.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair(drProv[dsProvider.Provider.ShortNameColumn.ColumnName].ToString(), drProv[dsProvider.Provider.ProviderIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.EntityIdColumn.ColumnName].ToString(), drProv[dsProvider.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                        //        }
                        //    }
                        //}

                        DataTable distinctValuesRef = view.ToTable(true, dsPatientReferral.Referrals.RefProviderIdColumn.ColumnName, dsPatientReferral.Referrals.TypeColumn.ColumnName);

                        foreach (DataRow drRefProv in dsRefProvider.Tables[dsRefProvider.ReferringProvider.TableName].Rows)
                        {
                            for (int i = 0; i < distinctValuesRef.Rows.Count; i++)
                            {
                                if (MDVUtility.ToInt64(drRefProv[dsRefProvider.ReferringProvider.ReferringProviderIdColumn]) == MDVUtility.ToInt64(distinctValuesRef.Rows[i][0].ToString()) && MDVUtility.ToStr(distinctValuesRef.Rows[i][1]) == "Incoming")
                                {
                                    incomingReferralFrom.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair(drRefProv[dsRefProvider.ReferringProvider.FirstNameColumn.ColumnName].ToString(), drRefProv[dsRefProvider.ReferringProvider.ReferringProviderIdColumn.ColumnName].ToString(), "", ""));
                                }
                                if (MDVUtility.ToInt64(drRefProv[dsRefProvider.ReferringProvider.ReferringProviderIdColumn]) == MDVUtility.ToInt64(distinctValuesRef.Rows[i][0].ToString()) && MDVUtility.ToStr(distinctValuesRef.Rows[i][1]) == "Outgoing")
                                {
                                    outgoingReferralTo.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair(drRefProv[dsRefProvider.ReferringProvider.FirstNameColumn.ColumnName].ToString() + " (" + drRefProv[dsRefProvider.ReferringProvider.EntityNameColumn.ColumnName].ToString() + ")", drRefProv[dsRefProvider.ReferringProvider.ReferringProviderIdColumn.ColumnName].ToString(), "", ""));
                                }
                            }
                        }
                    }
                }
            }

            HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair> facilityList = new HashSet<MDVision.IEHR.Common.MDVisionLookups.NameValuePair>();

            BLObject<DSProfileLookup> objFacility = BLLAdminProfileObj.LookupFacility("true");
            DSProfileLookup dsFaciltiy = objFacility.Data;

            facilityList.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair("- Select -", ""));

            if (dsFaciltiy != null)
            {
                foreach (DataRow drProv in dsFaciltiy.Tables[dsFaciltiy.Facility.TableName].Rows)
                {
                    facilityList.Add(new MDVision.IEHR.Common.MDVisionLookups.NameValuePair(drProv[dsFaciltiy.Facility.ShortNameColumn.ColumnName].ToString(), drProv[dsFaciltiy.Facility.FacilityIdColumn.ColumnName].ToString(), drProv[dsFaciltiy.Facility.EntityIdColumn.ColumnName].ToString()));
                }
            }

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            var response = new
            {
                status = true,

                IncomingReferralFrom = js.Serialize(incomingReferralFrom),
                IncomingReferralTo = js.Serialize(incomingReferralTo),
                OutgoingReferralFrom = js.Serialize(outgoingReferralFrom),
                OutgoingReferralTo = js.Serialize(outgoingReferralTo),
                FacilityList = js.Serialize(facilityList)
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        }

        public string GetStatusReasons(long StatusId)
        {
            try
            {
                DSPatientLookups dsPatientLookup = null;
                BLObject<DSPatientLookups> objListLookup = new BLLPatient().GetStatusReasons(MDVUtility.ToInt64(StatusId));
                dsPatientLookup = objListLookup.Data;
                if (dsPatientLookup != null)
                {
                    List<Dictionary<string, string>> lstStatusReasons = new List<Dictionary<string, string>>();
                    if (dsPatientLookup.Tables[dsPatientLookup.ReferralStatusReason.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPatientLookup.Tables[dsPatientLookup.ReferralStatusReason.TableName].Rows)
                        {
                            var StatusReasonsKeyValues = new Dictionary<string, string>
                            {
                                { "Id", MDVUtility.ToStr(dr[dsPatientLookup.ReferralStatusReason.IdColumn.ColumnName])},
                                { "Description", MDVUtility.ToStr(dr[dsPatientLookup.ReferralStatusReason.DescriptionColumn.ColumnName])}
                            };
                            lstStatusReasons.Add(StatusReasonsKeyValues);
                        }
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        StatusReasonsCount = dsPatientLookup.Tables[dsPatientLookup.ReferralStatusReason.TableName].Rows.Count,
                        StatusReasons_JSON = js.Serialize(lstStatusReasons)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        StatusReasonsCount = 0,
                        StatusReasons_JSON = "[]",
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

        public string loadIncomingReferraltAttachment(Int64 PatientId, Int64 TransitionId, string RefModuleName, long OrderSetReferralId = 0)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> objDocument = null;
                objDocument = BLLPatientObj.LoadPatientDocument("", PatientId, "", "", "", null, null, null, null, "", 0, 0, "", 0, "", "0", 0, 1, 1000, 0, TransitionId, RefModuleName, OrderSetReferralId);
                dsPatient = objDocument.Data;
                if (dsPatient != null)
                {
                    List<Dictionary<string, string>> lstAttachment = new List<Dictionary<string, string>>();
                    if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows)
                        {
                            var AttachmentkeyValues = new Dictionary<string, string>
                            {
                                { "PatDocId", MDVUtility.ToStr(dr[dsPatient.PatientDocument.PatDocIdColumn.ColumnName])},
                                { "DocumentName", MDVUtility.ToStr(dr[dsPatient.PatientDocument.DocumentNameColumn.ColumnName])},
                                { "TransitionId", MDVUtility.ToStr(dr[dsPatient.PatientDocument.TransitionIdColumn.ColumnName])},
                                { "OrderSetReferralId", MDVUtility.ToStr(dr[dsPatient.PatientDocument.OrderSetReferralIdColumn.ColumnName])},
                                { "ModifiedOn", String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatient.PatientDocument.ModifiedOnColumn.ColumnName]))?"": MDVUtility.ToDateTime(dr[dsPatient.PatientDocument.ModifiedOnColumn.ColumnName]).ToString()},
                                { "ModifiedBy", MDVUtility.ToStr(dr[dsPatient.PatientDocument.ModifiedByColumn.ColumnName])}
                            };
                            lstAttachment.Add(AttachmentkeyValues);
                        }
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        AttachmentCount = dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count,
                        AttachmentLoad_JSON = js.Serialize(lstAttachment),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = true,
                        AttachmentCount = 0,
                        AttachmentLoad_JSON = "[]",
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
    }
}