using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.Model.Clinical.Notes;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Model.Patient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MDVision.IEHR.EMR.Model.Clinical;
using MDVision.Model.Clinical.BillingInformation;
using Newtonsoft.Json;
using MDVision.IEHR.Common.ProviderNoteAccess;
using System.Xml.Serialization;
using System.IO;
using MDVision.Model.Clinical.LegacyNotes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MDVision.Model.Clinical.Notes.Notes;
using MDVision.Model.Common;
using MDVision.Model.Clinical.Templates;
using System.Web.Configuration;
using MDVision.Common.Logging;
using MDVision.IEHR.Controls.Patient.Document;
using MDVision.IEHR.EMR.Model.ReportHeader;
using MDVision.IEHR.EMR.Helpers.Clinical.ReportHeader;
using MDVision.Model.Clinical.OrdersAndResults;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MDVision.Model.Clinical.Orderset;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using Newtonsoft.Json.Linq;

namespace MDVision.IEHR.EMR.Helpers.Clinical.ClinicalNotes
{
    public class ClinicalNotesHelper
    {
        private BLLPatient BLLPatientObj = null;
        private BLLClinical BLLClinicalObj = null;
        private BLLCQM BLLCQMObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;
        public ClinicalNotesHelper()
        {
            BLLPatientObj = new BLLPatient();
            BLLClinicalObj = new BLLClinical();
            BLLCQMObj = new BLLCQM();
            BLLAdminProfileObj = new BLLAdminProfile();
        }
        private static ClinicalNotesHelper _instance = null;
        public static ClinicalNotesHelper Instance()
        {
            if (_instance == null)
                _instance = new ClinicalNotesHelper();
            return _instance;
        }
        #region Public Functions

        /// <summary>
        /// Saves the Clinical Notes.
        /// </summary>
        /// <param name="model">DTO Object Having Notes Information</param>
        /// <param name="PatientID"></param>
        /// <returns>Successful/UnsucessFul Message</returns>
        public string saveClinical_Notes(ClinicalNotesFillModel model, Int64 PatientID)
        {
            try
            {
                if (model.IsToCheckForTodaysNote)
                {
                    if (BLLClinicalObj.IsTodaysNoteCreated(PatientID, model.VisitDate))
                    {
                        var response = new
                        {
                            status = false,
                            IsTodaysNoteCreated = true,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }

                DataTable dtCopytComponents = new DataTable();
                DataColumn KeyName = new DataColumn();
                KeyName.ColumnName = "KeyName";
                KeyName.DataType = typeof(string);
                dtCopytComponents.Columns.Add(KeyName);

                DataColumn Value = new DataColumn();
                Value.ColumnName = "Value";
                Value.DataType = typeof(bool);
                dtCopytComponents.Columns.Add(Value);

                if (PatientID <= 0 || string.IsNullOrEmpty(model.NoteType) || string.IsNullOrEmpty(model.FacilityId) || string.IsNullOrEmpty(model.ProviderId) ||
                    string.IsNullOrEmpty(model.VisitDate) || string.IsNullOrEmpty(model.VisitTime))
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Save_Error_Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #region Binding DataSet Information
                DSNotes dsNotes = new DSNotes();
                DSNotes.NotesRow dr = dsNotes.Notes.NewNotesRow();
                string ProviderId = model.ProviderId;
                string OrderSetId = model.OrderSetId;

                dr.NotesId = -1;
                dr.PatientId = PatientID;

                if (!string.IsNullOrEmpty(model.VisitId) && model.VisitId != "-1")
                {
                    dr.VisitId = MDVUtility.ToInt64(model.VisitId);
                }
                else if (model.VisitId == "-1" || string.IsNullOrEmpty(model.VisitId))
                {
                    dr[dsNotes.Notes.VisitIdColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(model.AppointmentID))
                {
                    dr.AppointmentId = MDVUtility.ToInt64(model.AppointmentID);
                }
                // it's also Note Date

                dr.VisitDate = MDVUtility.ToDateTime(model.VisitDate);
                // it's also Note Time
                dr.VisitTime = MDVUtility.ToStr(model.VisitTime);

                dr.ProviderName = model.Provider;

                dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);

                dr.FacilityName = model.Facility;

                dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);

                dr.RefProviderName = model.RefProvider;
                if (!string.IsNullOrEmpty(model.RefProviderId))
                {
                    dr.RefProviderId = MDVUtility.ToInt64(model.RefProviderId);
                }
                if (!string.IsNullOrEmpty(model.ResourceId))
                {
                    dr.ResourceId = MDVUtility.ToInt64(model.ResourceId);
                }
                if (!string.IsNullOrEmpty(model.Resource))
                {
                    dr.ResourceName = model.Resource;
                }

                if (!string.IsNullOrEmpty(model.ResourceProviderId))
                {
                    dr.ResourceProviderId = MDVUtility.ToInt64(model.ResourceProviderId);
                }
                if (!string.IsNullOrEmpty(model.ResourceProvider))
                {
                    dr.ResourceProviderName = model.ResourceProvider;
                }
                if (!string.IsNullOrEmpty(model.PatientVisitType))
                {
                    dr.VisitTypeId = MDVUtility.ToInt64(model.PatientVisitType);
                }
                dr.LinkedAppointment = model.LinkedAppointment;

                dr.IsHPIComplaint = model.IsHPIComplaint == "True" ? true : false;

                //Copy HTML to CopayPreviousNote
                dr.PrevNoteDescription = model.CopayPreviousNote;

                if (!string.IsNullOrEmpty(model.PrevNotesId))
                {
                    dr.PrevNotesId = MDVUtility.ToInt64(model.PrevNotesId);
                }
                dr.NoteText = "";//model.NoteText;

                dr[dsNotes.Notes.VisitReasonIdColumn] = DBNull.Value;
                dr.VisitReasonComments = model.VisitReason;

                dr.TemplateTypeId = MDVUtility.ToInt64(model.NoteType);

                if (!string.IsNullOrEmpty(model.NoteTypeText) && System.Text.RegularExpressions.Regex.Replace(model.NoteTypeText.ToLower(), @"\s+", "") == "phoneencounter")
                {
                    dr.IsPhoneEncounter = 1;
                }
                else
                {
                    dr.IsPhoneEncounter = 0;
                }

                if (!string.IsNullOrEmpty(model.UserId))
                {
                    dr.UserId = MDVUtility.ToInt64(model.UserId);
                }

                if (!string.IsNullOrEmpty(model.EncounterType))
                {
                    dr.EncounterType = MDVUtility.ToInt32(model.EncounterType);
                }

                if (!string.IsNullOrEmpty(model.Caller))
                {
                    dr.Caller = model.Caller;
                }

                if (!string.IsNullOrEmpty(model.Receiver))
                {
                    dr.Receiver = model.Receiver;
                }
                if (model.IsNonBilable)
                {
                    dr.IsNonBilable = true;
                }
                else
                {
                    dr.IsNonBilable = false;
                }

                if (!string.IsNullOrEmpty(model.DurationText))
                {
                    dr.Duration = model.DurationText;
                }

                if (!string.IsNullOrEmpty(model.Duration))
                {
                    dr.CPTCode = MDVUtility.ToInt32(model.Duration);
                }


                if (!string.IsNullOrEmpty(model.NoteTemplate))
                {
                    dr.TemplateId = MDVUtility.ToInt64(model.NoteTemplate);
                }

                if (!string.IsNullOrEmpty(model.RoomNo))
                {
                    dr.RoomsId = MDVUtility.ToInt64(model.RoomNo);
                }

                dr.bMedReconciled = !string.IsNullOrEmpty(model.bMedReconciled) && model.bMedReconciled == "1" ? true : false;
                if (dr.bMedReconciled == true)
                {
                    dr.MedReconciledId = model.MedReconciledId;
                }
                else
                {
                    dr[dsNotes.Notes.MedReconciledIdColumn] = DBNull.Value;
                }

                dr.IsActive = true;

                dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                if (model.NoteComponentList != null && model.NoteComponentList.Count > 0)
                {
                    foreach (NoteComponentModel a in model.NoteComponentList)
                    {
                        DataRow Dr = dtCopytComponents.NewRow();
                        Dr[0] = a.ComponentName;
                        Dr[1] = a.isCopy == "1" ? true : false;
                        dtCopytComponents.Rows.Add(Dr);
                    }
                }
                else
                {

                    DataRow Dr = dtCopytComponents.NewRow();
                    Dr[0] = "ALL";
                    Dr[1] = true;
                    dtCopytComponents.Rows.Add(Dr);
                }

                #endregion
                #region Database Insertion
                dsNotes.Notes.AddNotesRow(dr);
                BLObject<DSNotes> obj = BLLClinicalObj.insertClinical_Notes(dsNotes, dtCopytComponents);

                string NotesId = dsNotes.Tables[dsNotes.Notes.TableName].Rows[0][dsNotes.Notes.NotesIdColumn.ColumnName].ToString();
                long UserId = MDVSession.Current.AppUserId;

                BLLOrderSet BLLOrderSetObj = new BLLOrderSet();
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(OrderSetId)))
                {
                    BLObject<DSOrderSet> objOrderSet = BLLOrderSetObj.attachOrderSetWithNote(MDVUtility.ToLong(NotesId), MDVUtility.ToLong(PatientID), MDVUtility.ToStr(OrderSetId));

                }

                var obj2 = BLLClinicalObj.insert_OrderSet(UserId, OrderSetId, PatientID, NotesId);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        NotesId = dsNotes.Tables[dsNotes.Notes.TableName].Rows[0][dsNotes.Notes.NotesIdColumn.ColumnName],
                        MUAlertsCount = dsNotes.Tables[dsNotes.Notes.TableName].Rows[0][dsNotes.Notes.MUAlertsCountColumn.ColumnName],
                        NewInsertTables = MDVUtility.JSON_DataTable(dsNotes.Tables[dsNotes.NewInsertionTables.TableName]),
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
        /// <summary>
        /// Updates the Clinical Notes.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="PatientId">The Patient identifier.</param>
        /// <param name="NotesID">The Notes identifier.</param>
        /// <param name="AppointmentID"></param>
        /// <returns>Successful Message</returns>
        public string updateClinical_Notes(ClinicalNotesFillModel model, Int64 NotesID, Int64 PatientID, Int64 AppointmentID)
        {
            try
            {
                if (NotesID < 0 || PatientID <= 0 || string.IsNullOrEmpty(model.VisitDate) || string.IsNullOrEmpty(model.VisitTime))
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Save_Error_Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (NotesID > 0)
                {
                    #region Binding DataSet Information
                    DSNotes dsNotes = new DSNotes();
                    BLObject<DSNotes> objLoad = BLLClinicalObj.loadClinical_Notes(0, NotesID, 0, 1, 1);
                    dsNotes = objLoad.Data;
                    foreach (DSNotes.NotesRow dr in dsNotes.Tables[dsNotes.Notes.TableName].Rows)
                    {
                        if (!string.IsNullOrEmpty(model.PatientId))
                            dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                        dr.PatientId = PatientID;

                        if (!string.IsNullOrEmpty(model.VisitId))
                        {
                            dr.VisitId = MDVUtility.ToInt64(model.VisitId);
                        }
                        if (!string.IsNullOrEmpty(model.AppointmentID))
                        {
                            dr.AppointmentId = MDVUtility.ToInt64(model.AppointmentID);
                        }

                        dr.VisitDate = MDVUtility.ToDateTime(model.VisitDate);// it's also Note Date

                        dr.VisitTime = MDVUtility.ToStr(model.VisitTime);

                        dr.ProviderName = model.Provider;
                        if (!string.IsNullOrEmpty(model.ProviderId))
                            dr.ProviderId = MDVUtility.ToInt64(model.ProviderId);

                        dr.FacilityName = model.FacilityName;

                        if (!string.IsNullOrEmpty(model.FacilityId))
                            dr.FacilityId = MDVUtility.ToInt64(model.FacilityId);

                        dr.RefProviderName = model.RefProvider;
                        if (!string.IsNullOrEmpty(model.RefProviderId))
                        {
                            dr.RefProviderId = MDVUtility.ToInt64(model.RefProviderId);
                        }
                        if (!string.IsNullOrEmpty(model.ResourceId))
                        {
                            dr.ResourceId = MDVUtility.ToInt64(model.ResourceId);
                        }
                        if (!string.IsNullOrEmpty(model.Resource))
                        {
                            dr.ResourceName = model.Resource;
                        }
                        if (!string.IsNullOrEmpty(model.ResourceProviderId))
                        {
                            dr.ResourceProviderId = MDVUtility.ToInt64(model.ResourceProviderId);
                        }
                        if (!string.IsNullOrEmpty(model.ResourceProvider))
                        {
                            dr.ResourceProviderName = model.ResourceProvider;
                        }

                        dr.LinkedAppointment = model.LinkedAppointment;

                        dr[dsNotes.Notes.VisitReasonIdColumn] = DBNull.Value;
                        dr.VisitReasonComments = model.VisitReason;

                        if (!string.IsNullOrEmpty(model.NoteType))
                            dr.TemplateTypeId = MDVUtility.ToInt64(model.NoteType);

                        if (!string.IsNullOrEmpty(model.NoteTemplate))
                        {
                            dr.TemplateId = MDVUtility.ToInt64(model.NoteTemplate);
                        }
                        else
                        {
                            dr[dsNotes.Notes.TemplateIdColumn] = DBNull.Value;
                        }
                        dr.PrevNoteDescription = model.CopayPreviousNote;
                        if (!string.IsNullOrEmpty(model.PrevNotesId))
                        {
                            dr.PrevNotesId = MDVUtility.ToInt64(model.PrevNotesId);
                        }
                        if (!string.IsNullOrEmpty(model.RoomNo))
                        {
                            dr.RoomsId = MDVUtility.ToInt64(model.RoomNo);
                        }
                        if (!string.IsNullOrEmpty(model.NoteStatus) && model.NoteStatus.Equals("Signed"))
                        {
                            dr.NoteStatus = model.NoteStatus;
                            //Copy HTML to CopayPreviousNote
                            dr.NoteText = ""; //model.NoteText /*+ " <ul id='signedByProvider' class='list-unstyled'><li id='signedBy'>Signed by: " + MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) + " on " + DateTime.Now.ToLocalTime() + "</li></ul>"*/;
                        }
                        else
                        {
                            //Copy HTML to CopayPreviousNote
                            dr.NoteText = "";//model.NoteText;
                        }

                        if (!string.IsNullOrEmpty(model.NoteStatus) && model.NoteStatus.Equals("Draft"))
                        {
                            dr.NoteStatus = model.NoteStatus;
                        }
                        if (dr.IsPhoneEncounter == 1)
                        {
                            if (!string.IsNullOrEmpty(model.DurationText))
                            {
                                dr.Duration = model.DurationText;
                            }
                            if (!string.IsNullOrEmpty(model.EncounterType))
                            {
                                dr.EncounterType = MDVUtility.ToInt32(model.EncounterType);
                            }
                            if (!string.IsNullOrEmpty(model.UserId))
                            {
                                dr.UserId = MDVUtility.ToInt64(model.UserId);
                            }

                            dr.Caller = MDVUtility.ToStr(model.Caller);


                            dr.Receiver = MDVUtility.ToStr(model.Receiver);

                        }
                        if (model.ComeFromCopyNote == "1")
                        {
                            dr.ComeFromCopyNote = true;
                        }
                        else
                        {
                            dr.ComeFromCopyNote = false;
                        }

                        if (model.IsNonBilable)
                        {
                            dr.IsNonBilable = true;
                        }
                        else
                        {
                            dr.IsNonBilable = false;
                        }
                        dr.bMedReconciled = !string.IsNullOrEmpty(model.bMedReconciled) && model.bMedReconciled == "1" ? true : false;
                        if (dr.bMedReconciled == true)
                        {
                            dr.MedReconciledId = model.MedReconciledId;
                        }
                        else
                        {
                            dr[dsNotes.Notes.MedReconciledIdColumn] = DBNull.Value;
                        }

                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                        if (!string.IsNullOrEmpty(model.PatientVisitType))
                        {
                            dr.VisitTypeId = MDVUtility.ToInt64(model.PatientVisitType);
                        }
                    }
                    #endregion
                    #region Database Updation

                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        BLObject<DSNotes> obj = BLLClinicalObj.updateClinical_Notes(dsNotes);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
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
                            Message = "Provider Note not found to update."
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
                        Message = "Please select a Provider Note to update."
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
        public string updateVisitType(Int64 NotesID, Int64 VisitTypeId, Int64 AppointmentID = 0)
        {
            try
            {
                if (NotesID < 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Save_Error_Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (NotesID > 0)
                {

                    #region Database Updation


                    string Result = BLLClinicalObj.updateVisitType(NotesID, VisitTypeId, AppointmentID);
                    if (Result == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Please select a Provider Note to update."
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
        public string unsignClinical_Notes(Int64 NotesID)
        {
            try
            {
                if (NotesID <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Save_Error_Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                if (NotesID > 0)
                {
                    #region Binding DataSet Information
                    DSNotes dsNotes = new DSNotes();
                    BLObject<DSNotes> objLoad = BLLClinicalObj.loadClinical_Notes(0, NotesID, 0, 1, 1);
                    dsNotes = objLoad.Data;
                    foreach (DSNotes.NotesRow dr in dsNotes.Tables[dsNotes.Notes.TableName].Rows)
                    {

                        dr.UnSignedStatus = true;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;
                    }
                    #endregion
                    #region Database Updation

                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        BLObject<DSNotes> obj = BLLClinicalObj.unsignClinical_Notes(dsNotes);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
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
                            Message = "Provider Note not found to update."
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
                        Message = "Please select a Provider Note to update."
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
        /// Updates the Clinical Notes, Progress Note HTML
        /// </summary>
        /// <param name="model"></param>
        /// <param name="PatientId">The Patient identifier.</param>
        /// <param name="NotesID">The Notes identifier.</param>
        /// <param name="AppointmentID"></param>
        /// <returns>Successful Message</returns>
        public string updateClinical_Notes_progressNoteHTML(ClinicalNotesFillModel model, Int64 NotesID, Int64 PatientID, Int64 AppointmentID)
        {
            try
            {

                if (NotesID > 0)
                {
                    #region Binding DataSet Information
                    DSNotes dsNotes = new DSNotes();
                    BLObject<DSNotes> objLoad = BLLClinicalObj.loadClinical_Notes(0, NotesID, 0, 1, 1);
                    dsNotes = objLoad.Data;
                    foreach (DSNotes.NotesRow dr in dsNotes.Tables[dsNotes.Notes.TableName].Rows)
                    {
                        //Copy HTML to CopayPreviousNote
                        dr.NoteText = model.NoteText;
                        dr.bMedReconciled = !string.IsNullOrEmpty(model.bMedReconciled) && model.bMedReconciled == "1" ? true : false;
                        if (dr.bMedReconciled == true)
                        {
                            dr.MedReconciledId = model.MedReconciledId;
                        }
                        else
                        {
                            dr[dsNotes.Notes.MedReconciledIdColumn] = DBNull.Value;
                        }

                        dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.ModifiedOn = DateTime.Now;

                    }
                    #endregion
                    #region Database Updation
                    //dsNotes.Notes.AddNotesRow(dr);
                    //dsNotes.Notes.AcceptChanges();

                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        //dsNotes.Notes.Rows[0].SetModified();
                        BLObject<DSNotes> obj = BLLClinicalObj.updateClinical_Notes(dsNotes);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
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
                        Message = "Notes not found."
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
        /// Deletes the Clinical Notes.
        /// </summary>
        /// <param name="NotesID">The Notes identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string deleteClinical_Notes(long NotesID)
        {
            try
            {
                if (NotesID <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.deleteClinical_Notes(MDVUtility.ToStr(NotesID));
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

        public string IsOrderSetAssociatedWithNote(string noteid)
        {
            try
            {
                bool IsAssociated = false;
                if (!string.IsNullOrEmpty(noteid))
                {
                    BLObject<string> obj = BLLClinicalObj.IsOrderSetAssociatedWithNote(MDVUtility.ToInt64(noteid));
                    if (obj.Data != null && obj.Data != "")
                        IsAssociated = MDVUtility.StringToBoolean(obj.Data);
                }
                var response = new
                {
                    status = true,
                    Result = IsAssociated,
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
        public string loadClinical_NoteInfo(long NoteID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(NoteID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = "Notes not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<List<ClinicalNotesInfo>> obj = BLLClinicalObj.loadClinical_NoteInfo(NoteID);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            NoteInfo_JSON = obj.Data
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

        public string getClinical_NoteComponentAccess(long NoteID, string ComponentName, string NoteAccessTime)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(NoteID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = "Notes not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    ProviderNoteAccessCache cashe = new ProviderNoteAccessCache();
                    RevokeAccessResponseModel res_model = cashe.IsNoteComponentAvaliable(MDVSession.Current.AppUserId, NoteID, ComponentName, NoteAccessTime);
                    if (!res_model.IsComponentAvaliable)
                    {
                        // Note Component is not avaliable check for permissions and alert accourdingly.

                        // rework: Dataset replcaed with Model

                        //DSUsers.UsersPrivilegesDataTable dtformPrivileges = MDVSession.Current.dtUserPrivileges;
                        //bool HavePrivilege = false;
                        //if (dtformPrivileges != null)
                        //{
                        //    DataRow[] dr = dtformPrivileges.Select(dtformPrivileges.FormNameColumn.ColumnName + "=" + MDVUtility.ToLINQFormatString("Notes_Revoke Access"));
                        //    if (dr.Length > 0)
                        //        HavePrivilege = true;
                        //}
                        var listformPrivileges = MDVSession.Current.ListUserPrivileges;
                        var havePrivilege = false;
                        if (listformPrivileges != null)
                        {
                            var formExist = listformPrivileges.Select(a => a.FormName + "=" + MDVUtility.ToLINQFormatString("Notes_Revoke Access"));
                            if (formExist.Any())
                                havePrivilege = true;
                        }

                        if (havePrivilege)
                        {
                            // User have privilige to Revoke Note Component Access.
                            var response = new
                            {
                                status = true,
                                IsNoteComponentAvaliable = true,
                                IsUserHaveRevokeAccess = true,
                                IsComponentUpdated = res_model.IsComponentUpdated,
                                PriorUserName = res_model.PriorUserName,
                                PriorUserId = res_model.PriorUserId,
                                Message = MDVUtility.ToTitleCase(res_model.PriorUserName) + " is already accessing  this component. Do you want to revoke their access?"
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                        }
                        else
                        {

                            // User have't privilige to Revoke Note Component Access.
                            var response = new
                            {
                                status = true,
                                IsNoteComponentAvaliable = false,
                                IsUserHaveRevokeAccess = false,
                                IsComponentUpdated = res_model.IsComponentUpdated,
                                PriorUserName = res_model.PriorUserName,
                                PriorUserId = res_model.PriorUserId,
                                Message = MDVUtility.ToTitleCase(res_model.PriorUserName) + " is already accessing  this component."
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                        }
                    }
                    else
                    {
                        // Provide access Note Component is avaliable.
                        var response = new
                        {
                            status = true,
                            IsNoteComponentAvaliable = true,
                            IsComponentUpdated = res_model.IsComponentUpdated,
                            Message = "Note Component Avaliable."
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

        public string Clinical_NoteComponentAccessAction(long NoteID, string ComponentName, string Action, long PriorUserId, string PriorUserName)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(NoteID))
                    && string.IsNullOrEmpty(MDVUtility.ToStr(ComponentName))
                    && string.IsNullOrEmpty(MDVUtility.ToStr(Action)))
                {
                    var response = new
                    {
                        status = false,
                        Message = "Notes not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    if (Action.ToLower() == "revokeaccess")
                    {
                        //1- remove prorior user access from cache
                        //2- register own access
                        //3- revoke access from that component.
                        ProviderNoteAccessCache cashe = new ProviderNoteAccessCache();

                        RevokeAccessModel model = new RevokeAccessModel();
                        model.UserId = MDVSession.Current.AppUserId;
                        model.UserName = (MDVSession.Current.AppUserFirstName + " " + MDVSession.Current.AppUserLastName).ToLower();
                        model.NoteId = NoteID;
                        model.CurrentComponent = ComponentName;

                        if (cashe.removeComponentFromCache(PriorUserId, ComponentName) && cashe.insertIntoCache(model))
                        {

                            var response = new
                            {
                                status = true,
                                UserName = (MDVSession.Current.AppUserFirstName + " " + MDVSession.Current.AppUserLastName).ToLower(),
                                PriorUserName = PriorUserName,
                                PriorUserId = PriorUserId,
                                ComponentName = ComponentName,
                                Message = "Access revoked successfully."
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));


                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = "Error occured during access revoke."
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }

                    }
                    else if (Action.ToLower() == "registeraccess")
                    {

                        RevokeAccessModel model = new RevokeAccessModel();
                        model.UserId = MDVSession.Current.AppUserId;
                        model.UserName = (MDVSession.Current.AppUserFirstName + " " + MDVSession.Current.AppUserLastName).ToLower();
                        model.NoteId = NoteID;
                        model.CurrentComponent = ComponentName;

                        ProviderNoteAccessCache cashe = new ProviderNoteAccessCache();
                        if (cashe.insertIntoCache(model))
                        {
                            var response = new
                            {
                                status = true,
                                Message = "Access registered successfully."
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {

                            var response = new
                            {
                                status = false,
                                Message = "Error occured during access registration."
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }


                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Inappropriate action performed."
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

        public string Clinical_RemoveUserNoteAccess(bool IsComponentOnly, string ComponentName)
        {

            try
            {
                bool IsDone = false;
                ProviderNoteAccessCache cashe = new ProviderNoteAccessCache();

                if (IsComponentOnly)
                {
                    IsDone = cashe.removeComponentFromCache(MDVSession.Current.AppUserId, ComponentName);
                }
                else
                {
                    IsDone = cashe.removeUserNoteAccessFromCache(MDVSession.Current.AppUserId);
                }

                if (IsDone)
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
                        Message = "Error occured while removing access from cache."
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
        public string RemoveUserNoteAccessByUserId(long AppUserId)
        {

            try
            {
                bool IsDone = false;
                ProviderNoteAccessCache cashe = new ProviderNoteAccessCache();
                IsDone = cashe.removeUserNoteAccessFromCache(MDVSession.Current.AppUserId);
                if (IsDone)
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
                        Message = "Error occured while removing access from cache."
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

        public string Clinical_NoteAccessByOthers(string noteid)
        {
            try
            {
                bool IsAccessed = false;
                string UserNames = "";
                if (!string.IsNullOrEmpty(noteid))
                {
                    var cachedObjcts = (from System.Collections.DictionaryEntry dict in HttpContext.Current.Cache
                                        let key = dict.Key.ToString()
                                        let val = dict.Value as RevokeAccessModel
                                        where key.StartsWith(ProviderNoteAccessCache.KeyPreFix) &&
                                        val.UserId != MDVSession.Current.AppUserId &&
                                        val.NoteId == MDVUtility.ToInt64(noteid) &&
                                        val.NoteComponents.Any(x => x.ComponentName == "SignNote")
                                        //from obj in val.NoteComponents
                                        //where obj.ComponentName == "SignNote"
                                        select dict.Value).ToList();
                    if (cachedObjcts.Count > 0)
                    {
                        IsAccessed = true;
                        UserNames = string.Join(", ", cachedObjcts.OfType<RevokeAccessModel>().ToArray().Select(x => x.UserName).ToList());
                    }
                }
                var response = new
                {
                    status = true,
                    Result = IsAccessed,
                    UserName = UserNames
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
        public string RemoveUsersAgainstNote(string noteid)
        {
            try
            {
                if (!string.IsNullOrEmpty(noteid))
                {
                    var cachedObjcts = (from System.Collections.DictionaryEntry dict in HttpContext.Current.Cache
                                        let key = dict.Key.ToString()
                                        let val = dict.Value as RevokeAccessModel
                                        where key.StartsWith(ProviderNoteAccessCache.KeyPreFix) &&
                                        val.NoteId == MDVUtility.ToInt64(noteid) &&
                                        val.NoteComponents.Any(x => x.ComponentName == "SignNote")
                                        select dict).ToList();
                    foreach (var item in cachedObjcts)
                        HttpContext.Current.Cache.Remove(item.Key.ToString());
                }
                var response = new
                {
                    status = true
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

        public string removeComponents(ClinicalNotesFillModel model, long NoteID)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(NoteID)))
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
                    BLObject<string> obj = BLLClinicalObj.removeComponents(model.ComponentsIdsString, MDVUtility.ToInt64(NoteID));
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

        public string GetNotesDates(ClinicalNotesFillModel model)
        {
            try
            {
                List<NotesVisitDate> NotesDetail = null;
                BLObject<List<NotesVisitDate>> obj;

                obj = BLLClinicalObj.GetNotesDates(MDVUtility.ToInt64(model.PatientId));
                NotesDetail = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ClinicalNotesCount = NotesDetail.Count,
                        NotesLoad_JSON = NotesDetail,
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

        /// <summary>
        /// Load all the Clinical Notess for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The Notes identifier.</param>
        /// <returns>Json string containing Datatable or Exception Notes</returns>
        public string loadClinical_Notes_Obsolete(Int64 PatientID, Int64 NotesID, Int64 AppointmentID, Int32 PageNumber, Int32 RowsPerPage, Int32 isPhoneEncounter = 0, string NoteStatus = null)
        {
            try
            {
                DSNotes dsNotes = null;
                BLObject<DSNotes> obj = null;

                obj = BLLClinicalObj.load_Clinical_Notes_Obsolete(PatientID, NotesID, AppointmentID, PageNumber, RowsPerPage, "", "", isPhoneEncounter, NoteStatus);
                if (obj != null)
                {
                    dsNotes = obj.Data;
                }
                if (dsNotes != null)
                {
                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        // Check if a user can delete notes
                        var privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "DELETE")).ToString();
                        privilegasMessage = privilegasMessage == "" ? "Yes" : "No";

                        var response = new
                        {
                            status = true,
                            ClinicalNotesCount = dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count,
                            iTotalDraftDisplayRecords = dsNotes.Notes.Rows[0][dsNotes.Notes.DraftNotesCountColumn.ColumnName],
                            iTotalSignedDisplayRecords = dsNotes.Notes.Rows[0][dsNotes.Notes.SignedNotesCountColumn.ColumnName],
                            NotesLoad_JSON = MDVUtility.JSON_DataTable(dsNotes.Tables[dsNotes.Notes.TableName]),
                            HasDeleteRights = privilegasMessage,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalNotesCount = 0,
                            iTotalDraftDisplayRecords = 0,
                            iTotalSignedDisplayRecords = 0,
                            Message = Common.AppPrivileges.No_Record_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ClinicalNotesCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = Common.AppPrivileges.No_Record_Message
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
        /// Load all the Clinical Notess for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The Notes identifier.</param>
        /// <returns>Json string containing Datatable or Exception Notes</returns>
        public string loadClinical_Notes(Int64 PatientID, Int64 NotesID, Int64 AppointmentID, Int32 PageNumber, Int32 RowsPerPage, Int32 isPhoneEncounter = 0, string NoteStatus = null)
        {
            try
            {

                List<Notes> notesList = BLLClinicalObj.load_Clinical_Notes(PatientID, NotesID, AppointmentID, PageNumber, RowsPerPage, "", "", isPhoneEncounter, NoteStatus);

                if (notesList != null)
                {
                    if (notesList.Count > 0)
                    {
                        // Check if a user can delete notes
                        var privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "DELETE")).ToString();
                        privilegasMessage = privilegasMessage == "" ? "Yes" : "No";

                        var response = new
                        {
                            status = true,
                            ClinicalNotesCount = notesList.Count,
                            iTotalDraftDisplayRecords = notesList[0].DraftNotesCount,
                            iTotalSignedDisplayRecords = notesList[0].SignedNotesCount,
                            NotesLoad_JSON = JsonConvert.SerializeObject(notesList),
                            HasDeleteRights = privilegasMessage,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalNotesCount = 0,
                            iTotalDraftDisplayRecords = 0,
                            iTotalSignedDisplayRecords = 0,
                            Message = Common.AppPrivileges.No_Record_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        ClinicalNotesCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = Common.AppPrivileges.No_Record_Message
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

        public string GetModifiedNoteCount(long UserId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(UserId)))
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

                    BLObject<string> obj = BLLClinicalObj.GetModifiedNoteCount(MDVUtility.ToLong(UserId));
                    if (obj.Data != "")
                    {
                        var response = new
                        {
                            status = true,
                            ModifiedNoteCount = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Exception in GetModifiedNoteCount"
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
        public string GetAmendmentData(long NotesId)
        {
            try
            {
                List<AmendmentNoteModel> AmendmentNoteModelList = null;
                BLObject<List<AmendmentNoteModel>> obj;

                obj = BLLClinicalObj.GetAmendmentData(NotesId);
                AmendmentNoteModelList = obj.Data;
                if (obj.Data != null)
                {
                    if (AmendmentNoteModelList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            AmendmentNoteCount = AmendmentNoteModelList.Count,
                            AmendmentNote_JSON = AmendmentNoteModelList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            AmendmentNoteCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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

        public string GetAmendmentDataReport(long NotesId)
        {
            try
            {
                List<AmendmentNoteReportModel> AmendmentNoteModelList = null;
                BLObject<List<AmendmentNoteReportModel>> obj;

                obj = BLLClinicalObj.GetAmendmentDataReport(NotesId);
                AmendmentNoteModelList = obj.Data;
                if (obj.Data != null)
                {
                    if (AmendmentNoteModelList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            AmendmentNoteCount = AmendmentNoteModelList.Count,
                            AmendmentNote_JSON = AmendmentNoteModelList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            AmendmentNoteCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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




        /// <summary>
        /// Load all the Clinical Notess for Grid binding.
        /// </summary>
        /// <param name="fieldsJSON">The fields json.</param>
        /// <param name="ProviderID">The Notes identifier.</param>
        /// <returns>Json string containing Datatable or Exception Notes</returns>
        public string searchClinical_Notes(Int64 PatientID, Int64 NotesID, Int64 AppointmentID, Int32 PageNumber, Int32 RowsPerPage, Int32 isPhoneEncounter = 0)
        {
            try
            {

                DSNotes dsNotes = null;
                BLObject<DSNotes> obj;
                //obj = BLLClinicalObj.loadClinical_Notes(PatientID, NotesID, AppointmentID, PageNumber, RowsPerPage);
                obj = BLLClinicalObj.load_Clinical_Notes_Obsolete(PatientID, NotesID, AppointmentID, PageNumber, RowsPerPage, "", "", isPhoneEncounter);

                dsNotes = obj.Data;
                if (obj.Data != null)
                {
                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalNotesCount = dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count,
                            iTotalDisplayRecords = dsNotes.Notes.Rows[0][dsNotes.Notes.RecordCountColumn.ColumnName],
                            NotesLoad_JSON = MDVUtility.JSON_DataTable(dsNotes.Tables[dsNotes.Notes.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalNotesCount = 0,
                            NotesCount = 0,
                            Message = Common.AppPrivileges.No_Record_Message
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
        public string searchClinical_NotesCaseReport(Int64 PatientID, Int64 NotesID, Int64 AppointmentID, Int32 PageNumber, Int32 RowsPerPage, Int32 isPhoneEncounter = 0)
        {
            try
            {

                DSNotes dsNotes = null;
                BLObject<DSNotes> obj;
                //obj = BLLClinicalObj.loadClinical_Notes(PatientID, NotesID, AppointmentID, PageNumber, RowsPerPage);
                obj = BLLClinicalObj.load_Clinical_Notes_Obsolete_casereporting(PatientID, NotesID, AppointmentID, PageNumber, RowsPerPage, "", "", isPhoneEncounter);


                if (obj.Data != null)
                {
                    dsNotes = obj.Data;
                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalNotesCount = dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count,
                            iTotalDisplayRecords = dsNotes.Notes.Rows[0][dsNotes.Notes.RecordCountColumn.ColumnName],
                            NotesLoad_JSON = MDVUtility.JSON_DataTable(dsNotes.Tables[dsNotes.Notes.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalNotesCount = 0,
                            NotesCount = 0,
                            Message = Common.AppPrivileges.No_Record_Message
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

        public string loadNoteWithNewTemplate(Int64 NotesID, Int64 PatientID, long TemplateId, long UserId, long ProviderId, string componenetsIds)
        {
            try
            {
                BLObject<ClinicalNotesWaper> modelNote = BLLClinicalObj.loadNoteWithNewTemplate(NotesID, PatientID, TemplateId, UserId, ProviderId, componenetsIds);

                if (modelNote.Data == null)
                {
                    var response = new
                    {
                        status = false,
                        Message = modelNote.Message
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    string HPIJson = "[]";
                    if (modelNote.Data.ClinicalNoteComponents.Complaints.HPINotesFindings.Count > 0)
                    {
                        var HPITemplateList = (from a in modelNote.Data.ClinicalNoteComponents.Complaints.HPINotesFindings
                                               select new
                                               {
                                                   HPITemplateId = a.HPITemplateId,
                                                   TemplateName = a.TemplateName,
                                                   Comments = a.Comments,
                                               }).Distinct().ToList();
                        HPIJson = js.Serialize(HPITemplateList);
                    }

                    var response = new
                    {
                        status = true,
                        NotesFill_JSON = js.Serialize(modelNote.Data.ClinicalNotesModel),

                        NotesComplaints_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Complaints),
                        NotesHPIFindings_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Complaints.HPINotesFindings),
                        NotesHPITemplate_JSON = HPIJson,

                        NotesAllergies_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Allergies),
                        NotesSocialHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.SocialHistory),
                        NotesMedicalHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.MedicalHistory),
                        NotesBirthHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.BirthHistory),
                        NotesFamilyHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.FamilyHistory),
                        NotesSurgicalHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.SurgicalHistory),
                        NotesHospitalizationHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.HospitalizationHistory),

                        NotesPrescription_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Prescription),
                        NotesReviewofSystem_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.ReviewofSystem),


                        NotesVitals_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Vitals.VitalsModel),
                        Notes_Attached_VitalSigns_JSON = modelNote.Data.ClinicalNoteComponents.Vitals.Vitals_JSON,

                        Notes_ProblemList_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Problems.ProblemListModel),
                        Notes_Attached_ProblemList_JSON = modelNote.Data.ClinicalNoteComponents.Problems.Attatched_ProblemList_JSON,

                        Notes_Procedures_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Procedures.Procedures),
                        Notes_Attached_Procedures_JSON = modelNote.Data.ClinicalNoteComponents.Procedures.Attatched_Procedures_JSON,

                        Notes_Immunization_Vaccines_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Immunization.Vaccines),
                        Notes_Immunization_Injections_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Immunization.Injections),
                        Notes_Attached_Immunization_Vaccines_JSON = modelNote.Data.ClinicalNoteComponents.Immunization.Attached_Vaccine_JSON,
                        Notes_Attached_Immunization_Injections_JSON = modelNote.Data.ClinicalNoteComponents.Immunization.Attached_TherInjection_JSON,

                        Notes_Medications_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Medications.Medications),
                        Notes_Attached_Medications_JSON = modelNote.Data.ClinicalNoteComponents.Medications.Attached_Medication_JSON,

                        PETemplate_JSON = modelNote.Data.ClinicalNoteComponents.PhysicalExam.Notes_PETemplate_JSON,
                        PETemplateCount = modelNote.Data.ClinicalNoteComponents.PhysicalExam.Notes_PETemplateCount,
                        PETemplateSystems_JSON = modelNote.Data.ClinicalNoteComponents.PhysicalExam.Notes_PETemplateSystems_JSON,
                        PETemplateSystemsCount = modelNote.Data.ClinicalNoteComponents.PhysicalExam.Notes_PETemplateSystemsCount,
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

                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string loadTemplate_HTML(Int64 PatientID, Int64 NotesID, long TemplateId)
        {
            try
            {


                BLObject<string> obj = BLLClinicalObj.loadTemplate_HTML(PatientID, NotesID, TemplateId);
                if (obj.Data == "")
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.Delete_Message,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        NoteHTML = obj.Data
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
        public string fillClinical_Notes(Int64 NotesID, Int64 PatientId)
        {
            try
            {
                if (NotesID < 1 && PatientId < 1)
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
                    #region Binding DataSet Information
                    DSNotes dsNotes = null;
                    BLObject<DSNotes> obj = BLLClinicalObj.loadClinical_Notes(PatientId, NotesID, 0, 1, 1, "1", "0");
                    dsNotes = obj.Data;
                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsNotes.Tables[dsNotes.Notes.TableName].Rows[0];
                        ClinicalNotesModel model = new ClinicalNotesModel();

                        model.NotesId = MDVUtility.ToStr(dr[dsNotes.Notes.NotesIdColumn.ColumnName]);
                        model.PatientId = MDVUtility.ToStr(dr[dsNotes.Notes.PatientIdColumn.ColumnName]);
                        model.VisitId = MDVUtility.ToStr(dr[dsNotes.Notes.VisitIdColumn.ColumnName]);
                        model.VisitDate = MDVUtility.ToStr(dr[dsNotes.Notes.VisitDateColumn.ColumnName]) != "" ? MDVUtility.ToDateTime(dr[dsNotes.Notes.VisitDateColumn.ColumnName]).ToShortDateString() : "";
                        model.VisitTime = MDVUtility.ToStr(dr[dsNotes.Notes.VisitTimeColumn.ColumnName]);
                        model.VisitReason = MDVUtility.ToStr(dr[dsNotes.Notes.VisitReasonCommentsColumn.ColumnName]);
                        model.NoteType = MDVUtility.ToStr(dr[dsNotes.Notes.TemplateTypeIdColumn.ColumnName]);
                        model.Duration = MDVUtility.ToStr(dr[dsNotes.Notes.DurationColumn.ColumnName]);
                        model.Provider = MDVUtility.ToStr(dr[dsNotes.Notes.ProviderNameColumn.ColumnName]);
                        model.ProviderId = MDVUtility.ToStr(dr[dsNotes.Notes.ProviderIdColumn.ColumnName]);
                        model.FacilityName = model.Facility = MDVUtility.ToStr(dr[dsNotes.Notes.FacilityNameColumn.ColumnName]);
                        model.FacilityId = MDVUtility.ToStr(dr[dsNotes.Notes.FacilityIdColumn.ColumnName]);
                        model.RoomNo = MDVUtility.ToStr(dr[dsNotes.Notes.RoomsIdColumn.ColumnName]);
                        model.NoteTemplate = MDVUtility.ToStr(dr[dsNotes.Notes.TemplateIdColumn.ColumnName]);
                        model.RefProvider = MDVUtility.ToStr(dr[dsNotes.Notes.RefProviderNameColumn.ColumnName]);
                        model.RefProviderId = MDVUtility.ToStr(dr[dsNotes.Notes.RefProviderIdColumn.ColumnName]);
                        //model.AppointmentID = MDVUtility.ToStr(dr[dsNotes.Notes.AppointmentIdColumn.ColumnName]);
                        model.AppointmentID = MDVUtility.ToStr(dr["LinkedAppointmentId"]);
                        model.LinkedAppointment = MDVUtility.ToStr(dr[dsNotes.Notes.LinkedAppointmentColumn.ColumnName]);
                        model.IsLinkedAppointment = string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsNotes.Notes.LinkedAppointmentColumn.ColumnName])) ? false : true;
                        model.IsCopayPreviousNote = string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsNotes.Notes.PrevNoteDescriptionColumn.ColumnName])) ? false : true;
                        model.CopayPreviousNote = MDVUtility.ToStr(dr[dsNotes.Notes.PrevNoteDescriptionColumn.ColumnName]);
                        model.NoteText = MDVUtility.ToStr(dr[dsNotes.Notes.NoteTextColumn.ColumnName]);
                        model.NoteStatus = MDVUtility.ToStr(dr[dsNotes.Notes.NoteStatusColumn.ColumnName]);
                        model.BillingInfoId = MDVUtility.ToStr(dr[dsNotes.Notes.BillingInfoIdColumn.ColumnName]);
                        model.PatientTypeId = MDVUtility.ToInt32(dr[dsNotes.Notes.PatientTypeIdColumn.ColumnName]);
                        model.TemplateName = MDVUtility.ToStr(dr[dsNotes.Notes.TemplateNameColumn.ColumnName]);
                        model.TemplateTypeName = MDVUtility.ToStr(dr[dsNotes.Notes.TemplateTypeNameColumn.ColumnName]);
                        //model.VisitReasonId = MDVUtility.ToStr(dr[dsNotes.Notes.SchReasonIdColumn.ColumnName]);
                        model.bMedReconciled = !string.IsNullOrEmpty(MDVUtility.ToStr(dr[dsNotes.Notes.bMedReconciledColumn.ColumnName])) && MDVUtility.ToStr(dr[dsNotes.Notes.bMedReconciledColumn.ColumnName]).ToLower() == "true" ? "1" : "0";
                        model.MedReconciledId = MDVUtility.ToStr(dr[dsNotes.Notes.MedReconciledIdColumn.ColumnName]);
                        model.HxtabOrder = MDVUtility.ToStr(dr[dsNotes.Notes.HxtabOrderColumn.ColumnName]);
                        model.IsNonBilable = MDVUtility.ToBool(dr[dsNotes.Notes.IsNonBilableColumn.ColumnName]);
                        model.ROSDataTemptId = MDVUtility.ToStr(dr[dsNotes.Notes.ROSDataTemptIdColumn.ColumnName]);
                        model.PEDataTemptId = MDVUtility.ToStr(dr[dsNotes.Notes.PEDataTemptIdColumn.ColumnName]);
                        model.PETemplateId = MDVUtility.ToStr(dr[dsNotes.Notes.PETemplateIdColumn.ColumnName]);
                        model.ROSTemplateId = MDVUtility.ToStr(dr[dsNotes.Notes.ROSTemplateIdColumn.ColumnName]);
                        model.EncounterType = MDVUtility.ToStr(dr[dsNotes.Notes.EncounterTypeColumn.ColumnName]);
                        model.User = MDVUtility.ToStr(dr[dsNotes.Notes.UserNameColumn.ColumnName]);
                        model.Receiver = MDVUtility.ToStr(dr[dsNotes.Notes.ReceiverColumn.ColumnName]);
                        model.Caller = MDVUtility.ToStr(dr[dsNotes.Notes.CallerColumn.ColumnName]);
                        model.UserId = MDVUtility.ToStr(dr[dsNotes.Notes.UserIdColumn.ColumnName]);
                        #endregion
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            NotesFill_JSON = js.Serialize(model),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.No_Record_Message,

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

        public string getClinical_Note_Component_HTML(Int64 NotesID, string ComponentName)
        {
            try
            {
                if (NotesID < 1)
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

                    List<NoteComponentModel> NoteComponentList = null;
                    BLObject<List<NoteComponentModel>> obj;

                    obj = BLLClinicalObj.loadNoteComponents(NotesID, ComponentName);
                    NoteComponentList = obj.Data;

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    //var privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(MDVision.IEHR.Common.AppPrivileges.GetFormSecurity("Notes_Notes", "DELETE")).ToString();
                    //privilegasMessage = privilegasMessage == "" ? "Yes" : "No";
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.No_Record_Message,
                        NoteComponentListCount = NoteComponentList.Count,
                        NoteComponentListFill_JSON = NoteComponentList,
                        //HasDeleteRights = privilegasMessage

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

        public string fillClinical_Note_By_Id(Int64 NotesID, Int64 PatientId)
        {
            try
            {
                if (NotesID < 1 && PatientId < 1)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };

                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }

                BLObject<ClinicalNotesModel> modelNote = BLLClinicalObj.loadClinical_Note_By_NoteID(PatientId, NotesID);
                if (modelNote != null)
                {
                    List<NoteComponentModel> NoteComponentList = null;
                    BLObject<List<NoteComponentModel>> obj;

                    obj = BLLClinicalObj.loadNoteComponents(NotesID);
                    NoteComponentList = obj.Data;

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(MDVision.IEHR.Common.AppPrivileges.GetFormSecurity("Notes_Notes", "DELETE")).ToString();
                    privilegasMessage = privilegasMessage == "" ? "Yes" : "No";


                    var response = new
                    {
                        status = true,
                        NotesFill_JSON = js.Serialize(modelNote.Data),
                        Message = AppPrivileges.Save_Message,
                        NoteComponentListCount = NoteComponentList.Count,
                        NoteComponentListFill_JSON = NoteComponentList,
                        HasDeleteRights = privilegasMessage
                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = false,
                        Message = modelNote.Message,
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

        public string loadClinical_Note_By_Id(Int64 NotesID, Int64 PatientId, Int64 ProviderId, long OrderSetId, bool PreviousNoteROS, bool PreviousNotePE, bool PreviousNoteComplaints, bool PreviousNoteProblems)
        {
            try
            {
                if (NotesID < 1 && PatientId < 1)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };

                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }

                #region Medication Upload
                if (OrderSetId > 0)
                {
                    ClinicalNotesWaper model = new ClinicalNotesWaper();
                    model.ClinicalNotesModel = new DALNotes().loadClinical_Note_By_NoteID(PatientId, NotesID);

                    if (model.ClinicalNotesModel.NoteText.Contains("{{ Clinical Medication }}"))
                    {

                        var MedicationIDs = "";
                        var ExistsMedicationsDrugName = "";
                        DSOrderSet ds = new DSOrderSet();

                        BLObject<List<OS_MedicationModel>> obj1 = null;

                        obj1 = new BLLOrderSet().LoadMedication(MDVUtility.ToStr(OrderSetId), "", 1, 1000);
                        List<OS_MedicationModel> model1 = new List<OS_MedicationModel>();
                        if (obj1.Data != null)
                        {
                            model1 = obj1.Data;
                            if (model1.Count > 0)
                            {
                                MedicationIDs = string.Join(",", model1.Select(p => p.OS_MedicationId));
                            }
                        }

                        if (!string.IsNullOrEmpty(MedicationIDs))
                        {
                            List<OS_MedicationModel> MedicationList = null;
                            BLObject<List<OS_MedicationModel>> ExistsOrNotExistsMedicationobj;
                            ExistsOrNotExistsMedicationobj = new BLLOrderSet().ExistsOrNotExistsMedication(MedicationIDs, PatientId);
                            MedicationList = ExistsOrNotExistsMedicationobj.Data;

                            if (MedicationList.Count > 0)
                            {
                                var NotExistsMedicationIds = "";
                                var ExistsMedicationIds = "";

                                ExistsMedicationIds = string.Join(",", MedicationList.Where(p => p.alreadyExists == true).Select(p => p.OS_MedicationId));
                                NotExistsMedicationIds = string.Join(",", MedicationList.Where(p => p.alreadyExists == false).Select(p => p.OS_MedicationId));
                                var existsMedicsDrugName = string.Join(", ", MedicationList.Where(p => p.alreadyExists == true).Select(p => p.BrandName));
                                if (existsMedicsDrugName != "")
                                {
                                    ExistsMedicationsDrugName = ExistsMedicationsDrugName + "," + existsMedicsDrugName;
                                }
                                if (!string.IsNullOrEmpty(NotExistsMedicationIds))
                                {
                                    RcopiaHelper rcopiaHelper = new RcopiaHelper();
                                    dynamic ResponseOfUploadMedicationOnDrFirst = JObject.Parse(rcopiaHelper.UploadMedicationOnDrFirst(MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), PatientId, MDVUtility.ToStr(OrderSetId), NotesID, NotExistsMedicationIds));
                                }
                            }
                        }
                    }
                }
                #endregion

                BLObject<ClinicalNotesWaper> modelNote = BLLClinicalObj.load_Clinical_Progress_Note(PatientId, NotesID, ProviderId, OrderSetId, PreviousNoteROS, PreviousNotePE, PreviousNoteComplaints, PreviousNoteProblems);

                if (modelNote.Data != null)
                {
                    List<NoteComponentModel> NoteComponentList = null;
                    BLObject<List<NoteComponentModel>> obj;

                    obj = BLLClinicalObj.loadNoteComponents(NotesID);
                    if (obj.Data == null)
                        throw new Exception(obj.Message);

                    NoteComponentList = obj.Data;

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes_Notes", "DELETE")).ToString();
                    privilegasMessage = privilegasMessage == "" ? "Yes" : "No";

                    string HPIJson = "[]";
                    if (modelNote.Data.ClinicalNoteComponents.Complaints.HPINotesFindings.Count > 0)
                    {
                        var HPITemplateList = (from a in modelNote.Data.ClinicalNoteComponents.Complaints.HPINotesFindings
                                               select new
                                               {
                                                   HPITemplateId = a.HPITemplateId,
                                                   TemplateName = a.TemplateName,
                                                   Comments = a.Comments,
                                               }).Distinct().ToList();
                        HPIJson = js.Serialize(HPITemplateList);
                    }

                    var response = new
                    {
                        status = true,
                        NotesFill_JSON = js.Serialize(modelNote.Data.ClinicalNotesModel),
                        Message = AppPrivileges.Save_Message,
                        NoteComponentListCount = NoteComponentList.Count,
                        NoteComponentListFill_JSON = NoteComponentList,
                        HasDeleteRights = privilegasMessage,

                        NotesComplaints_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Complaints),
                        NewComplaintId = modelNote.Data.ClinicalNoteComponents.Complaints.ComplaintId,
                        NotesHPIFindings_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Complaints.HPINotesFindings),
                        NotesHPITemplate_JSON = HPIJson,

                        NotesAllergies_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Allergies),
                        NotesSocialHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.SocialHistory),
                        NotesMedicalHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.MedicalHistory),
                        NotesBirthHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.BirthHistory),
                        NotesFamilyHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.FamilyHistory),
                        NotesSurgicalHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.SurgicalHistory),
                        NotesHospitalizationHistory_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.HospitalizationHistory),

                        NotesPrescription_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Prescription),
                        NotesReviewofSystem_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.ReviewofSystem),


                        NotesVitals_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Vitals.VitalsModel),
                        Notes_Attached_VitalSigns_JSON = modelNote.Data.ClinicalNoteComponents.Vitals.Vitals_JSON,

                        Notes_ProblemList_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Problems.ProblemListModel),
                        Notes_Attached_ProblemList_JSON = modelNote.Data.ClinicalNoteComponents.Problems.Attatched_ProblemList_JSON,

                        Notes_Procedures_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Procedures.Procedures),
                        Notes_Attached_Procedures_JSON = modelNote.Data.ClinicalNoteComponents.Procedures.Attatched_Procedures_JSON,

                        Notes_LabOrder_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.ClinicalLabOrder),
                        Notes_DiagnosticImagingOrder_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.ClinicalDiagnosticImagingOrder),
                        Notes_ProcedureOrder_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.ClinicalProcedureOrder),
                        Notes_Immunization_Vaccines_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Immunization.Vaccines),
                        Notes_Immunization_Injections_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Immunization.Injections),
                        Notes_Attached_Immunization_Vaccines_JSON = modelNote.Data.ClinicalNoteComponents.Immunization.Attached_Vaccine_JSON,
                        Notes_Attached_Immunization_Injections_JSON = modelNote.Data.ClinicalNoteComponents.Immunization.Attached_TherInjection_JSON,
                        NotesSocPsyandBehaviorHx_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.SocPsyandBehaviorHx),
                        //Notes_Medications_JSON = OrderSetId > 0 ? MDVUtility.JSON_DataTable(ds.Tables[ds.Medication.TableName]) : js.Serialize(modelNote.Data.ClinicalNoteComponents.Medications.Medications),
                        Notes_Medications_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.Medications.Medications),
                        Notes_Attached_Medications_JSON = modelNote.Data.ClinicalNoteComponents.Medications.Attached_Medication_JSON,

                        PETemplate_JSON = modelNote.Data.ClinicalNoteComponents.PhysicalExam.Notes_PETemplate_JSON,
                        PETemplateCount = modelNote.Data.ClinicalNoteComponents.PhysicalExam.Notes_PETemplateCount,
                        PETemplateSystems_JSON = modelNote.Data.ClinicalNoteComponents.PhysicalExam.Notes_PETemplateSystems_JSON,
                        PETemplateSystemsCount = modelNote.Data.ClinicalNoteComponents.PhysicalExam.Notes_PETemplateSystemsCount,
                        IsPatientComplaintExists = modelNote.Data.ClinicalNoteComponents.Complaints.isComplaintExists,
                        PreviousComplaintSoapText = modelNote.Data.ClinicalNoteComponents.Complaints.PrevComplaintFreeText,

                        PatientEducationSoap_JSON = js.Serialize(modelNote.Data.ClinicalNoteComponents.PatientEducation.PatientEducation),
                        PatientEducationSoapCount = modelNote.Data.ClinicalNoteComponents.PatientEducation.PatientEducation.Count,

                        FollowUp = js.Serialize(modelNote.Data.ClinicalNoteComponents.FollowUp),
                        TreatmentPlanComment = js.Serialize(modelNote.Data.ClinicalNoteComponents.TreatmentPlanCommentModel),
                    };

                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    bool UnDo = false;
                    if (modelNote.ErrorCode == "RenderNoteTemplateTags")
                    {
                        SaveNoteSessionData(NotesID, "RenderNoteTemplateTags", modelNote.Message);
                        modelNote.Message = "Unable to load one of the Note Component.";
                        UnDo = true;
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var response = new
                    {
                        status = false,
                        UnDo = UnDo,
                        Message = modelNote.Message,

                    };

                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    UnDo = false,
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
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSPatient dsPatient = null;
                    PatientDemographicModel response_model = new PatientDemographicModel();
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    BLObject<DSPatient> obj = BLLClinicalObj.FillPatientInfo(PatientId);
                    dsPatient = obj.Data;
                    if (dsPatient.Tables[dsPatient.Patients.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsPatient.Tables[dsPatient.Patients.TableName].Rows[0];


                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                        ser.MaxJsonLength = Int32.MaxValue;

                        response_model.Provider = MDVUtility.ToStr(dr[dsPatient.Patients.ProviderNameColumn.ColumnName]);
                        response_model.ProviderID = MDVUtility.ToStr(dr[dsPatient.Patients.ProviderIdColumn.ColumnName]);
                        response_model.Facility = MDVUtility.ToStr(dr[dsPatient.Patients.FacilityNameColumn.ColumnName]);
                        response_model.FacilityID = MDVUtility.ToStr(dr[dsPatient.Patients.FacilityIdColumn.ColumnName]);
                        response_model.RefProvider = MDVUtility.ToStr(dr[dsPatient.Patients.ReferringProviderNameColumn.ColumnName]);
                        response_model.RefProviderID = MDVUtility.ToStr(dr[dsPatient.Patients.ReferringProviderIdColumn.ColumnName]);
                        var response = new
                        {
                            status = true,
                            DemographicFill_JSON = js.Serialize(response_model),
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


        /// <summary>
        /// This Function will Update Notes Status
        /// </summary>
        /// <param name="NotesID"></param>
        /// <param name="pageNo"></param>
        /// <param name="rpp"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public string updateClinical_NotesIsActive(Int64 NotesID, Int64 pageNo, Int64 rpp, Int64 IsActive)
        {
            try
            {
                if (NotesID > 0)
                {

                    DSNotes dsNotes = null;
                    BLObject<DSNotes> obj = BLLClinicalObj.loadClinical_Notes(0, NotesID, 0, 1, 1);
                    dsNotes = obj.Data;
                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsNotes.Tables[dsNotes.Notes.TableName].Rows[0];
                        dr[dsNotes.Notes.IsActiveColumn.ColumnName] = IsActive;

                        BLObject<DSNotes> objNotes = BLLClinicalObj.updateClinical_Notes(dsNotes);
                        string successMsg;
                        if (objNotes.Data != null)
                        {
                            if (IsActive == 0)
                                successMsg = Common.AppPrivileges.Inactive_Message;
                            else
                                successMsg = Common.AppPrivileges.Active_Message;
                            var response = new
                            {
                                status = true,
                                Message = successMsg
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objNotes.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
                        Message = "Provider Note not found to update."
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
        /// This Function will detach Vital sign from notes
        /// </summary>
        /// <param name="VitalSignId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string detach_VitalSign_From_Notes(string VitalSignId, long PatientID, long VisitId, long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(VitalSignId)) || PatientID < 1)
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
                    BLObject<string> obj = BLLClinicalObj.detachVitalSignFromNotes(VitalSignId, PatientID, VisitId, NotesId);
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
        /// This Function will attach vital sign to notes
        /// </summary>
        /// <param name="VitalSignId"></param>
        /// <param name="PatientID"></param>
        /// <param name="VisitId"></param>
        /// <param name="NotesId"></param>
        /// <returns></returns>
        internal string attach_VitalSign_From_Notes(string VitalSignId, long PatientID, long VisitId, long NotesId)
        {
            try
            {
                DSVitals dsVitals = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(VitalSignId)) || PatientID < 1)
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
                    BLObject<DSVitals> obj = BLLClinicalObj.attachVitalSignFromNotes(VitalSignId, PatientID, VisitId, NotesId, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                    if (obj.Data != null)
                    {
                        dsVitals = obj.Data;
                        var response = new
                        {
                            status = true,
                            VitalsCount = dsVitals.Tables[dsVitals.VitalSigns.TableName].Rows.Count,
                            iTotalDisplayRecords = (dsVitals.VitalSigns.Rows.Count > 0) ? dsVitals.VitalSigns.Rows[0][dsVitals.VitalSigns.RecordCountColumn.ColumnName] : 0,
                            VitalsLoad_JSON = MDVUtility.JSON_DataTable(dsVitals.Tables[dsVitals.VitalSigns.TableName]),
                            Message = Common.AppPrivileges.Update_Message
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
        /// Get Appointments whoes notes are not yet created
        /// </summary>
        /// <param name="PatientID"></param>
        /// <returns></returns>
        internal string getLinkedAppointment_Notes(long PatientID, long ProviderId)
        {
            try
            {

                DSNotes dsNotes = null;
                BLObject<DSNotes> obj = BLLClinicalObj.fillLinkedAppointment_Notes(PatientID, ProviderId);
                dsNotes = obj.Data;
                if (obj.Data != null)
                {
                    if (dsNotes.Tables[dsNotes.Notes_LinkAppointment.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            LinkedAppointmentCount = dsNotes.Tables[dsNotes.Notes_LinkAppointment.TableName].Rows.Count,
                            LinkedAppointment_JSON = MDVUtility.JSON_DataTable(dsNotes.Tables[dsNotes.Notes_LinkAppointment.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            LinkedAppointmentCount = 0,
                            LinkedAppointment_JSON = MDVUtility.JSON_DataTable(dsNotes.Tables[dsNotes.Notes_LinkAppointment.TableName]),
                            Message = Common.AppPrivileges.No_Record_Message
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

        internal string copyPreviousNote_Patient(long PatientID)
        {
            try
            {

                DSNotes dsNotes = null;
                BLObject<DSNotes> obj = BLLClinicalObj.copyPreviousNote_Patient(PatientID);
                dsNotes = obj.Data;
                if (obj.Data != null)
                {
                    if (dsNotes.Tables[dsNotes.Notes_PreviousNote.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsNotes.Tables[dsNotes.Notes_PreviousNote.TableName].Rows[0];
                        ClinicalNotesModel model = new ClinicalNotesModel();

                        model.NoteDate = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.NoteDateColumn.ColumnName]);
                        model.CheifComplaint = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.ChiefComplaintColumn.ColumnName]);
                        model.NoteText = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.NoteTextColumn.ColumnName]);
                        model.PrevNotesId = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.PrevNotesIdColumn.ColumnName]);
                        model.FacilityId = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.FacilityIdColumn.ColumnName]);
                        model.ProviderId = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.ProviderIdColumn.ColumnName]);
                        model.NoteType = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.TemplateTypeIdColumn.ColumnName]);
                        model.NoteTemplate = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.TemplateIdColumn.ColumnName]);
                        model.RefProviderId = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.RefProviderIdColumn.ColumnName]);
                        model.VisitReason = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.SchReasonIdColumn.ColumnName]);
                        model.RoomNo = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.RoomsIdColumn.ColumnName]);

                        model.Provider = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.ProviderNameColumn.ColumnName]);
                        model.FacilityName = model.Facility = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.FacilityNameColumn.ColumnName]);
                        model.RefProvider = MDVUtility.ToStr(dr[dsNotes.Notes_PreviousNote.RefProviderNameColumn.ColumnName]);

                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            PreviousNoteCount = dsNotes.Tables[dsNotes.Notes_PreviousNote.TableName].Rows.Count,
                            PreviousNote_JSON = js.Serialize(model),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    else
                    {
                        var response = new
                        {
                            status = true,
                            PreviousNoteCount = 0,
                            Message = Common.AppPrivileges.No_Record_Message
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


        internal string previewClinical_Notes(Int64 NotesID)
        {
            try
            {
                if (NotesID > 0)
                {
                    BLObject<byte[]> objLoad = BLLClinicalObj.previewClinical_Notes(0, NotesID, 0, 1, 15);
                    if (objLoad.Data != null)
                    {

                        var response = new
                        {
                            status = true,
                            NotesHTML = Convert.ToBase64String(objLoad.Data),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            NotesHTML = "",
                            Message = "No Notes Found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        NotesHTML = "",
                        Message = "Notes not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Notes = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string printClinical_Notes(Int64 NotesID)
        {
            try
            {
                if (NotesID > 0)
                {
                    BLLClinicalObj.printClinical_Notes(NotesID);
                    var response = new
                    {
                        status = true,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Notes not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Notes = ex.Message,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string Create_Notes_HL7(Int64 PatientID, Int64 NotesID, string Type, Int64 ProviderId)
        {
            try
            {
                string dischargeType = "";

                if (Type.Equals("01"))
                {
                    Type = "Discharge";
                    dischargeType = "01";
                }
                else if (Type.Equals("20"))
                {
                    Type = "Discharge";
                    dischargeType = "20";
                }
                else if (Type.Equals("09"))
                {
                    Type = "Discharge";
                    dischargeType = "09";
                }


                if (string.IsNullOrEmpty(MDVUtility.ToStr(NotesID)) && string.IsNullOrEmpty(MDVUtility.ToStr(PatientID)))
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
                    BLObject<Syndromic> resultSet = BLLClinicalObj.GetSyndromicSurveillanceData(NotesID);
                    if (resultSet.Data != null)
                    {
                        ///////////////////ADT Segment/////////////////
                        NHapi.Model.V25.Message.ADT_A01 adt = new NHapi.Model.V25.Message.ADT_A01();

                        #region MSH
                        adt.MSH.FieldSeparator.Value = "|";
                        adt.MSH.EncodingCharacters.Value = @"^~\&";
                        adt.MSH.SendingApplication.NamespaceID.Value = "";
                        adt.MSH.SendingFacility.NamespaceID.Value = resultSet.Data.SyndromicFacilityModel != null && resultSet.Data.SyndromicFacilityModel.Count > 0 ? resultSet.Data.SyndromicFacilityModel[0].FacilityName != null ? resultSet.Data.SyndromicFacilityModel[0].FacilityName : "" : "";
                        adt.MSH.SendingFacility.UniversalID.Value = resultSet.Data.SyndromicFacilityModel != null && resultSet.Data.SyndromicFacilityModel.Count > 0 ? resultSet.Data.SyndromicFacilityModel[0].NPI != null ? resultSet.Data.SyndromicFacilityModel[0].NPI : "" : "";
                        adt.MSH.SendingFacility.UniversalIDType.Value = "NPI";
                        adt.MSH.ReceivingFacility.NamespaceID.Value = "";
                        adt.MSH.DateTimeOfMessage.Time.SetLongDate(DateTime.Now);  // otherwise DateTime.Now.ToString("yyyyMMddHHmmss"); // case sensitive

                        adt.MSH.MessageType.MessageCode.Value = "ADT";

                        if (Type.Equals("Admit"))
                        {
                            adt.MSH.MessageType.TriggerEvent.Value = "A01";
                            adt.MSH.MessageType.MessageStructure.Value = "ADT_A01";
                        }
                        else if (Type.Equals("Discharge"))
                        {
                            adt.MSH.MessageType.TriggerEvent.Value = "A03";
                            adt.MSH.MessageType.MessageStructure.Value = "ADT_A03";
                        }
                        else if (Type.Equals("Register"))
                        {
                            adt.MSH.MessageType.TriggerEvent.Value = "A04";
                            adt.MSH.MessageType.MessageStructure.Value = "ADT_A01";
                        }
                        else if (Type.Equals("Update"))
                        {
                            adt.MSH.MessageType.TriggerEvent.Value = "A08";
                            adt.MSH.MessageType.MessageStructure.Value = "ADT_A01";
                        }

                        if (dischargeType == "20" || dischargeType == "09" || dischargeType == "01")
                            adt.MSH.MessageControlID.Value = "NIST-SS-001.21";
                        else
                            adt.MSH.MessageControlID.Value = "NIST-SS-001.11";

                        adt.MSH.ProcessingID.ProcessingID.Value = "P";
                        adt.MSH.VersionID.VersionID.Value = "2.5.1";
                        adt.MSH.AcceptAcknowledgmentType.Value = "AL";
                        adt.MSH.ApplicationAcknowledgmentType.Value = "AL";

                        // MSH 21
                        adt.MSH.GetMessageProfileIdentifier(0).EntityIdentifier.Value = "PH_SS-Ack";
                        adt.MSH.GetMessageProfileIdentifier(0).NamespaceID.Value = "SS Sender";
                        adt.MSH.GetMessageProfileIdentifier(0).UniversalID.Value = "2.16.840.1.114222.4.10.3";
                        adt.MSH.GetMessageProfileIdentifier(0).UniversalIDType.Value = "ISO";
                        #endregion

                        #region EVN
                        adt.EVN.RecordedDateTime.Time.Value = DateTime.Now.ToString("yyyyMMddHHmmss");
                        adt.EVN.EventFacility.NamespaceID.Value = resultSet.Data.SyndromicProviderModel != null && resultSet.Data.SyndromicProviderModel.Count > 0 ? resultSet.Data.SyndromicProviderModel[0].ProviderName != null ? resultSet.Data.SyndromicProviderModel[0].ProviderName : "" : "";
                        adt.EVN.EventFacility.UniversalID.Value = resultSet.Data.SyndromicProviderModel != null && resultSet.Data.SyndromicProviderModel.Count > 0 ? resultSet.Data.SyndromicProviderModel[0].NPI != null ? resultSet.Data.SyndromicProviderModel[0].NPI : "" : "";
                        adt.EVN.EventFacility.UniversalIDType.Value = "NPI";
                        #endregion

                        #region PID
                        adt.PID.SetIDPID.Value = "1";

                        adt.PID.GetPatientIdentifierList(0).IDNumber.Value = resultSet.Data.SyndromicPatientModel != null && resultSet.Data.SyndromicPatientModel.Count > 0 ? resultSet.Data.SyndromicPatientModel[0].AccountNumber : "";
                        adt.PID.GetPatientIdentifierList(0).AssigningAuthority.NamespaceID.Value = "ISO";
                        adt.PID.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value = resultSet.Data.SyndromicPatientModel != null && resultSet.Data.SyndromicPatientModel.Count > 0 ? resultSet.Data.SyndromicPatientModel[0].AccountNumber : "";
                        adt.PID.GetPatientIdentifierList(0).AssigningAuthority.UniversalIDType.Value = "ISO";
                        adt.PID.GetPatientIdentifierList(0).IdentifierTypeCode.Value = "MR";
                        adt.PID.GetPatientName(0).NameTypeCode.Value = "";
                        adt.PID.GetPatientName(1).NameTypeCode.Value = "S";

                        string sex = "U";
                        if (resultSet.Data.SyndromicPatientModel != null && resultSet.Data.SyndromicPatientModel.Count > 0 && resultSet.Data.SyndromicPatientModel[0].Gender != null)
                        {
                            if (resultSet.Data.SyndromicPatientModel[0].Gender.ToLower().Equals("male"))
                                sex = "M";
                            else if (resultSet.Data.SyndromicPatientModel[0].Gender.ToLower().Equals("female"))
                                sex = "F";
                        }

                        adt.PID.AdministrativeSex.Value = sex;

                        int rCount = 0;
                        var raceArray = resultSet.Data.SyndromicPatientModel != null && resultSet.Data.SyndromicPatientModel.Count > 0 ? resultSet.Data.SyndromicPatientModel[0].RaceCodeAndName != null ? resultSet.Data.SyndromicPatientModel[0].RaceCodeAndName.TrimEnd(',').Split(',') : new string[] { } : new string[] { };
                        foreach (var item in raceArray)
                        {
                            var arr = item.Split('|'); //ElementAtOrDefault

                            adt.PID.GetRace(rCount).Identifier.Value = arr.Length >= 1 ? arr[0].Trim() == "NA" ? "" : arr[0] : ""; //code
                            adt.PID.GetRace(rCount).Text.Value = arr.Length >= 2 ? arr[1] : ""; //name
                            adt.PID.GetRace(rCount).NameOfCodingSystem.Value = "CDCREC";
                            adt.PID.GetRace(rCount).AlternateIdentifier.Value = "";
                            adt.PID.GetRace(rCount).AlternateText.Value = "";
                            adt.PID.GetRace(rCount).NameOfAlternateCodingSystem.Value = "";
                            rCount++;
                        }
                        rCount = 0;
                        var ethnicityArray = resultSet.Data.SyndromicPatientModel != null && resultSet.Data.SyndromicPatientModel.Count > 0 ? resultSet.Data.SyndromicPatientModel[0].EthnicityCodeAndName != null ? resultSet.Data.SyndromicPatientModel[0].EthnicityCodeAndName.TrimEnd(',').Split(',') : new string[] { } : new string[] { };
                        foreach (var item in ethnicityArray)
                        {
                            var arr = item.Split('|');
                            adt.PID.GetEthnicGroup(rCount).Identifier.Value = arr.Length >= 1 ? arr[0].Trim() == "NA" ? "" : arr[0] : ""; //code 
                            adt.PID.GetEthnicGroup(rCount).Text.Value = arr.Length >= 2 ? arr[1] : ""; // name
                            adt.PID.GetEthnicGroup(rCount).NameOfCodingSystem.Value = "CDCREC";
                            adt.PID.GetEthnicGroup(rCount).AlternateIdentifier.Value = "";
                            adt.PID.GetEthnicGroup(rCount).AlternateText.Value = "";
                            adt.PID.GetEthnicGroup(rCount).NameOfAlternateCodingSystem.Value = "";
                            rCount++;
                        }
                        adt.PID.GetPatientAddress(0).City.Value = resultSet.Data.SyndromicPatientModel != null && resultSet.Data.SyndromicPatientModel.Count > 0 ? resultSet.Data.SyndromicPatientModel[0].City != null ? resultSet.Data.SyndromicPatientModel[0].City : "" : "";
                        adt.PID.GetPatientAddress(0).StateOrProvince.Value = resultSet.Data.SyndromicPatientModel != null && resultSet.Data.SyndromicPatientModel.Count > 0 ? resultSet.Data.SyndromicPatientModel[0].State != null ? resultSet.Data.SyndromicPatientModel[0].State : "" : "";
                        adt.PID.GetPatientAddress(0).ZipOrPostalCode.Value = resultSet.Data.SyndromicPatientModel != null && resultSet.Data.SyndromicPatientModel.Count > 0 ? resultSet.Data.SyndromicPatientModel[0].ZipCode != null ? resultSet.Data.SyndromicPatientModel[0].ZipCode : "" : "";
                        adt.PID.GetPatientAddress(0).Country.Value = "USA";
                        adt.PID.GetPatientAddress(0).CountyParishCode.Value = resultSet.Data.SyndromicPatientModel != null && resultSet.Data.SyndromicPatientModel.Count > 0 ? resultSet.Data.SyndromicPatientModel[0].CountyParishCode != null ? resultSet.Data.SyndromicPatientModel[0].CountyParishCode : "" : "";
                        #endregion

                        #region PV1
                        adt.PV1.SetIDPV1.Value = "1";
                        adt.PV1.PatientClass.Value = "O";
                        adt.PV1.VisitNumber.IDNumber.Value = resultSet.Data.SyndromicNotesModel != null && resultSet.Data.SyndromicNotesModel.Count > 0 ? resultSet.Data.SyndromicNotesModel[0].VisitId != null ? resultSet.Data.SyndromicNotesModel[0].VisitId : "" : "";
                        adt.PV1.VisitNumber.IdentifierTypeCode.Value = "VN";
                        adt.PV1.VisitNumber.AssigningAuthority.NamespaceID.Value = "ISO";
                        adt.PV1.VisitNumber.AssigningAuthority.UniversalID.Value = resultSet.Data.SyndromicPatientModel != null && resultSet.Data.SyndromicPatientModel.Count > 0 ? resultSet.Data.SyndromicPatientModel[0].AccountNumber : "";
                        adt.PV1.VisitNumber.AssigningAuthority.UniversalIDType.Value = "ISO";

                        var VisitDate = resultSet.Data.SyndromicNotesModel != null && resultSet.Data.SyndromicNotesModel.Count > 0 ? resultSet.Data.SyndromicNotesModel[0].NoteCreationDate != null ? resultSet.Data.SyndromicNotesModel[0].NoteCreationDate : "" : "";

                        adt.PV1.AdmitDateTime.Time.Value = Convert.ToDateTime(VisitDate).ToString("yyyyMMdd") + Convert.ToDateTime(VisitDate).ToString("HHmm");
                        if (dischargeType == "20" || dischargeType == "09" || dischargeType == "01")
                        {
                            adt.PV1.DischargeDisposition.Value = dischargeType;
                            adt.PV1.GetDischargeDateTime(0).Time.SetLongDate(System.DateTime.Now);
                        }
                        #endregion

                        #region PV2
                        var visitReason = resultSet.Data.SyndromicNotesModel != null && resultSet.Data.SyndromicNotesModel.Count > 0 ? resultSet.Data.SyndromicNotesModel[0].EncounterReason != null ? resultSet.Data.SyndromicNotesModel[0].EncounterReason : "" : "";
                        adt.PV2.AdmitReason.Text.Value = visitReason;
                        #endregion

                        #region OBX
                        // OBX 1 /////////
                        adt.GetOBX(0).SetIDOBX.Value = "1";
                        adt.GetOBX(0).ValueType.Value = "CWE";
                        adt.GetOBX(0).ObservationIdentifier.Identifier.Value = "SS003";
                        adt.GetOBX(0).ObservationIdentifier.Text.Value = "Facility/Visit Type";
                        adt.GetOBX(0).ObservationIdentifier.NameOfCodingSystem.Value = "PHINQUESTION";
                        NHapi.Model.V25.Datatype.CWE obxCWE = new NHapi.Model.V25.Datatype.CWE(adt);
                        obxCWE.Identifier.Value = "261QU0200X";//facilityPOSNUCCCode;// POS_NUCC_Code[noteRow.FacilityPOSCode];
                        obxCWE.Text.Value = "Urgent Care";//noteRow.FacilityPOSDesc;// noteRow.VisitType;
                        obxCWE.NameOfCodingSystem.Value = "HCPTNUCC";//"NUCC";
                        obxCWE.OriginalText.Value = "Urgent Care Center";
                        adt.GetOBX(0).GetObservationValue(0).Data = obxCWE;
                        adt.GetOBX(0).ObservationResultStatus.Value = "F";


                        // OBX 2 /////////
                        adt.GetOBX(1).SetIDOBX.Value = "2";
                        adt.GetOBX(1).ValueType.Value = "NM";
                        adt.GetOBX(1).ObservationIdentifier.Identifier.Value = "21612-7";
                        adt.GetOBX(1).ObservationIdentifier.Text.Value = "Age at Time Patient Reported"; //"AGE TIME PATIENT REPORTED";
                        adt.GetOBX(1).ObservationIdentifier.NameOfCodingSystem.Value = "LN";
                        NHapi.Model.V25.Datatype.NM obxNM = new NHapi.Model.V25.Datatype.NM(adt);

                        string DOB = resultSet.Data.SyndromicPatientModel != null && resultSet.Data.SyndromicPatientModel.Count > 0 ? resultSet.Data.SyndromicPatientModel[0].DOB != null ? resultSet.Data.SyndromicPatientModel[0].DOB : "" : "";
                        DateTime dateofBirth = string.IsNullOrEmpty(DOB) ? DateTime.Now : Convert.ToDateTime(DOB);
                        DateTime today = DateTime.Now;
                        int months = (today.Year * 12 + today.Month) - (dateofBirth.Month + dateofBirth.Year * 12);

                        obxNM.Value = months.ToString();
                        adt.GetOBX(1).GetObservationValue(0).Data = obxNM;
                        adt.GetOBX(1).Units.Identifier.Value = "mo";
                        adt.GetOBX(1).Units.Text.Value = "month";
                        adt.GetOBX(1).Units.NameOfCodingSystem.Value = "UCUM";
                        adt.GetOBX(1).ObservationResultStatus.Value = "F";

                        // OBX 3 /////////
                        adt.GetOBX(2).SetIDOBX.Value = "3";
                        adt.GetOBX(2).ValueType.Value = "TX";//"CWE";
                        adt.GetOBX(2).ObservationIdentifier.Identifier.Value = "8661-1";
                        adt.GetOBX(2).ObservationIdentifier.Text.Value = resultSet.Data.SyndromicVitalsModel != null && resultSet.Data.SyndromicVitalsModel.Count > 0 ? resultSet.Data.SyndromicVitalsModel[0].ComplaintDescription != null ? resultSet.Data.SyndromicVitalsModel[0].ComplaintDescription : "" : "";
                        adt.GetOBX(2).ObservationIdentifier.NameOfCodingSystem.Value = "LN";

                        var CheifComplaint = resultSet.Data.SyndromicVitalsModel != null && resultSet.Data.SyndromicVitalsModel.Count > 0 ? resultSet.Data.SyndromicVitalsModel[0].OverallComments != null ? resultSet.Data.SyndromicVitalsModel[0].OverallComments : "" : "";
                        NHapi.Model.V25.Datatype.CWE obxCWE2 = new NHapi.Model.V25.Datatype.CWE(adt);
                        obxCWE2.Identifier.Value = CheifComplaint;
                        adt.GetOBX(2).GetObservationValue(0).Data = obxCWE2;
                        adt.GetOBX(2).ObservationResultStatus.Value = "F";

                        // OBX 4 /////////
                        adt.GetOBX(3).SetIDOBX.Value = "4";
                        adt.GetOBX(3).ValueType.Value = "NM";
                        adt.GetOBX(3).ObservationIdentifier.Identifier.Value = "8302-2";
                        adt.GetOBX(3).ObservationIdentifier.Text.Value = "Height";
                        adt.GetOBX(3).ObservationIdentifier.NameOfCodingSystem.Value = "LN";
                        NHapi.Model.V25.Datatype.NM obxNM2 = new NHapi.Model.V25.Datatype.NM(adt);
                        obxNM2.Value = resultSet.Data.SyndromicVitalsModel != null && resultSet.Data.SyndromicVitalsModel.Count > 0 ? resultSet.Data.SyndromicVitalsModel[0].Height != null ? resultSet.Data.SyndromicVitalsModel[0].Height : "0" : "0";
                        adt.GetOBX(3).GetObservationValue(0).Data = obxNM2;
                        adt.GetOBX(3).Units.Identifier.Value = "[in_us]";
                        adt.GetOBX(3).Units.Text.Value = "inch";
                        adt.GetOBX(3).Units.NameOfCodingSystem.Value = "UCUM";
                        adt.GetOBX(3).ObservationResultStatus.Value = "F";


                        // OBX 5 /////////
                        adt.GetOBX(4).SetIDOBX.Value = "5";
                        adt.GetOBX(4).ValueType.Value = "NM";
                        adt.GetOBX(4).ObservationIdentifier.Identifier.Value = "3141-9";
                        adt.GetOBX(4).ObservationIdentifier.Text.Value = "Weight";
                        adt.GetOBX(4).ObservationIdentifier.NameOfCodingSystem.Value = "LN";
                        NHapi.Model.V25.Datatype.NM obxNM3 = new NHapi.Model.V25.Datatype.NM(adt);
                        obxNM3.Value = resultSet.Data.SyndromicVitalsModel != null && resultSet.Data.SyndromicVitalsModel.Count > 0 ? resultSet.Data.SyndromicVitalsModel[0].Weight != null ? resultSet.Data.SyndromicVitalsModel[0].Weight : "0" : "0";
                        adt.GetOBX(4).GetObservationValue(0).Data = obxNM3;
                        adt.GetOBX(4).Units.Identifier.Value = "[lb_av]";
                        adt.GetOBX(4).Units.Text.Value = "pound";
                        adt.GetOBX(4).Units.NameOfCodingSystem.Value = "UCUM";
                        adt.GetOBX(4).ObservationResultStatus.Value = "F";


                        // OBX 6 /////////
                        adt.GetOBX(5).SetIDOBX.Value = "6";
                        adt.GetOBX(5).ValueType.Value = "CWE";
                        adt.GetOBX(5).ObservationIdentifier.Identifier.Value = "72166-2";
                        adt.GetOBX(5).ObservationIdentifier.Text.Value = "Tobacco Smoking Status";
                        adt.GetOBX(5).ObservationIdentifier.NameOfCodingSystem.Value = "LN";
                        NHapi.Model.V25.Datatype.CWE obxCWE3 = new NHapi.Model.V25.Datatype.CWE(adt);
                        obxCWE3.Identifier.Value = resultSet.Data.SyndromicObservationModel != null && resultSet.Data.SyndromicObservationModel.Count > 0 ? resultSet.Data.SyndromicObservationModel[0].TobaccoStatusSCT != null ? resultSet.Data.SyndromicObservationModel[0].TobaccoStatusSCT : "" : "";
                        obxCWE3.Text.Value = resultSet.Data.SyndromicObservationModel != null && resultSet.Data.SyndromicObservationModel.Count > 0 ? resultSet.Data.SyndromicObservationModel[0].TobaccoStatus != null ? resultSet.Data.SyndromicObservationModel[0].TobaccoStatus : "" : "";

                        obxCWE3.NameOfCodingSystem.Value = "SCT";
                        adt.GetOBX(5).GetObservationValue(0).Data = obxCWE3;
                        adt.GetOBX(5).ObservationResultStatus.Value = "F";
                        #endregion
                        rCount = 0;
                        foreach (var row in resultSet.Data.SyndromicObservationModel)
                        {
                            #region DG1
                            if (!Type.ToLower().Equals("admit"))
                            {
                                adt.GetDG1(rCount).SetIDDG1.Value = (rCount + 1).ToString();
                                adt.GetDG1(rCount).DiagnosisCodeDG1.Identifier.Value = row.ICD10Code == null ? "" : row.ICD10Code;
                                adt.GetDG1(rCount).DiagnosisCodeDG1.Text.Value = row.ICD10Description == null ? "" : row.ICD10Description;
                                adt.GetDG1(rCount).DiagnosisCodeDG1.NameOfCodingSystem.Value = "I10C";
                                adt.GetDG1(rCount).DiagnosisType.Value = GetDiagnosisType(Type);
                                // would change later according to admit dischage message type
                            }
                            #endregion
                            rCount++;
                        }
                        NHapi.Base.Parser.PipeParser parser = new NHapi.Base.Parser.PipeParser();
                        string contents = parser.Encode(adt);
                        string hl7Message = Get_Formatted_And_Reordered_Message(contents, Type);

                        var response = new
                        {
                            status = true,
                            Message = "HL7 file generated successfully.",
                            HL7Message = hl7Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Some Error Occured."
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
                    Message = ex.Message,   // "Please attach appointment with this visit",
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string GetDiagnosisType(string type)
        {
            string diagnosisType = "";
            switch (type.ToLower())
            {
                case "register":
                    diagnosisType = "W";
                    break;
                case "admit":
                    diagnosisType = "A";
                    break;
                case "update":
                    diagnosisType = "W";
                    break;
                case "discharge":
                    diagnosisType = "F";
                    break;
            }
            return diagnosisType;
        }

        public string Get_Formatted_And_Reordered_Message(string contents, string type)
        {
            if (type.ToLower() == "discharge")
            {
                List<string> completearray = contents.Split('\r').ToList();
                completearray.RemoveAt(completearray.Count - 1);
                var dgSegments = Array.FindAll(completearray.ToArray(), s => s.StartsWith("OBX"));
                completearray.RemoveAll(s => s.StartsWith("OBX"));
                List<string> Newarray = completearray.ToList();
                foreach (var dgSegment in dgSegments)
                {
                    Newarray.Add(dgSegment);
                }
                contents = string.Join("\r\n", Newarray);
            }
            else
            {
                contents = contents.Replace("\r", "\r\n");
            }
            return contents;
        }

        public string ResonsAutoComplete(string ShortName)
        {
            try
            {
                DSScheduleLookups dsReason = null;
                BLLClinical BLLClinicalObj = new BLLClinical();
                BLObject<DSScheduleLookups> obj;
                obj = BLLClinicalObj.LookupReasonsAutoComplete("1", ShortName);

                dsReason = obj.Data;
                if (obj.Data != null)
                {
                    if (dsReason.Tables[dsReason.ScheduleReasons.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ResonsCount = dsReason.Tables[dsReason.ScheduleReasons.TableName].Rows.Count,
                            ResonsLoad_JSON = HttpUtility.HtmlDecode(MDVUtility.JSON_DataTable(dsReason.Tables[dsReason.ScheduleReasons.TableName])),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ReferringProviderCount = 0,
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

        // Author:  Muhammad Arshad
        // Created Date: 01/17/2017
        //OverView: loads CQM with reasoning
        public string loadCQMWWithReasoning(Int64 providerId, string from, string to, string VisitId, string patientId = null, Int64 reportType = 0, string cqmId = null, int eitherDetail = 0, Int64 NoteId = 0, string isVBP = "0")
        {
            try
            {

                DSCQMReasoning dsCQMReasoning = null;
                BLObject<DSCQMReasoning> obj = null;

                if (NoteId <= 0)
                {
                    if (isVBP != "1")
                    {
                        obj = BLLCQMObj.Load_CQMWithReasoning(providerId, from, to, patientId, reportType, cqmId, eitherDetail);
                    }
                    else if (isVBP == "1")
                    {
                        obj = BLLCQMObj.Load_VBPWithReasoning(providerId, from, to, patientId, reportType, cqmId, eitherDetail);
                    }
                }
                else
                {
                    if (isVBP != "1")
                    {
                        obj = BLLCQMObj.Load_CQMWithNoteReasoning(providerId, from, to, patientId, reportType, cqmId, eitherDetail, NoteId);
                    }
                    else if (isVBP == "1")
                    {
                        obj = BLLCQMObj.Load_VBPWithNoteReasoning(providerId, from, to, patientId, reportType, cqmId, eitherDetail, NoteId);
                    }
                }

                dsCQMReasoning = obj.Data;

                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var BillingInfoFill_JSON = js.Serialize(BillingInfo_SELECT_By_VisitId(MDVUtility.ToInt64(VisitId)));

                if (obj.Data != null)
                {
                    if (dsCQMReasoning.Tables[dsCQMReasoning.AllMeasures.TableName].Rows.Count > 0)
                    {

                        var response = new
                        {
                            status = true,
                            //ClinicalNotesCount = dsCQMReasoning.Tables[dsNotes.Notes.TableName].Rows.Count,
                            //iTotalDisplayRecords = dsCQMReasoning.Notes.Rows[0][dsCQMReasoning.Notes.RecordCountColumn.ColumnName],
                            AllMeasuresCount = dsCQMReasoning.Tables[dsCQMReasoning.AllMeasures.TableName].Rows.Count,
                            AllMeasuresDetailCount = dsCQMReasoning.Tables[dsCQMReasoning.AllMeasuresDetail.TableName].Rows.Count,
                            AllMeasuresReasoningDetailCount = dsCQMReasoning.Tables[dsCQMReasoning.AllMeasuresReasoningDetail.TableName].Rows.Count,

                            AllMeasuresLoad_JSON = MDVUtility.JSON_DataTable(dsCQMReasoning.Tables[dsCQMReasoning.AllMeasures.TableName]),
                            AllMeasuresDetailLoad_JSON = MDVUtility.JSON_DataTable(dsCQMReasoning.Tables[dsCQMReasoning.AllMeasuresDetail.TableName]),
                            AllMeasuresReasoningDetailLoad_JSON = MDVUtility.JSON_DataTable(dsCQMReasoning.Tables[dsCQMReasoning.AllMeasuresReasoningDetail.TableName]),
                            BillingInfoFill_JSON = BillingInfoFill_JSON,
                            CQMMeasuresCount = dsCQMReasoning.Tables[dsCQMReasoning.CQMMeasures.TableName].Rows.Count,
                            CQMMeasures_JSON = MDVUtility.JSON_DataTable(dsCQMReasoning.Tables[dsCQMReasoning.CQMMeasures.TableName]),

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else if (dsCQMReasoning.Tables[dsCQMReasoning.CQMMeasures.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            AllMeasuresCount = 0,
                            AllMeasuresDetailCount = 0,
                            AllMeasuresReasoningDetailCount = 0,
                            CQMMeasuresCount = dsCQMReasoning.Tables[dsCQMReasoning.CQMMeasures.TableName].Rows.Count,
                            CQMMeasures_JSON = MDVUtility.JSON_DataTable(dsCQMReasoning.Tables[dsCQMReasoning.CQMMeasures.TableName]),
                            BillingInfoFill_JSON = BillingInfoFill_JSON

                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            AllMeasuresCount = 0,
                            AllMeasuresDetailCount = 0,
                            AllMeasuresReasoningDetailCount = 0,
                            CQMMeasuresCount = 0,
                            Message = Common.AppPrivileges.No_Record_Message,
                            BillingInfoFill_JSON = BillingInfoFill_JSON
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
        private BillingInformationModel BillingInfo_SELECT_By_VisitId(long visitId)
        {
            BillingInformationModel result = new BillingInformationModel();
            try
            {
                if (visitId > 0)
                {
                    DSBillingInformation dsBillingInformation = null;
                    BLObject<DSBillingInformation> obj = BLLClinicalObj.BillingInfo_SELECT_By_VisitId(visitId);
                    dsBillingInformation = obj.Data;
                    if (dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows[0];

                        result.BillingInfoId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.BillingInfoIdColumn.ColumnName]);
                        result.ENMTypeId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMTypeIdColumn.ColumnName]);
                        result.ENMTimeId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMTimeIdColumn.ColumnName]);
                        result.ENMCPTCode = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTCodeColumn.ColumnName]);
                        result.ENMCPTDescription = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDescriptionColumn.ColumnName]);
                        result.ENMCPTUnit = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTUnitColumn.ColumnName]);
                        result.ENMCPTDOSFrom = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDOSFromColumn.ColumnName]);
                        result.ENMCPTDOSTo = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDOSToColumn.ColumnName]);
                        result.PatientId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.PatientIdColumn.ColumnName]);
                        result.NotesId = MDVUtility.ToInt64(dr[dsBillingInformation.BillingInfo.NotesIdColumn.ColumnName]);
                        result.VisitId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.VisitIdColumn.ColumnName]);
                        result.ProviderId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ProviderIdColumn.ColumnName]);
                        result.Status = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.StatusColumn.ColumnName]);
                        result.IsActive = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.IsActiveColumn.ColumnName]);
                        result.CreatedBy = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.CreatedByColumn.ColumnName]);
                        result.CreatedOn = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.CreatedOnColumn.ColumnName]);
                        result.ModifiedBy = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ModifiedByColumn.ColumnName]);
                        result.ModifiedOn = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ModifiedOnColumn.ColumnName]);
                        result.SoapText = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.SoapTextColumn.ColumnName]);

                        result.Facility = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.FacilityColumn.ColumnName]);
                        result.FacilityId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.FacilityIdColumn.ColumnName]);
                        result.Provider = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ProviderColumn.ColumnName]);

                        result.ResourceProviderId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ResourceProviderIdColumn.ColumnName]);
                        result.ResourceProvider = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ResourceProviderColumn.ColumnName]);



                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        // Author:  Muhammad Arshad
        // Created Date: 01/20/2017
        //OverView: loads Patient Data Based on PatientIds
        public string loadCQMPatientData(string PatientIds)
        {
            try
            {

                DSCQMReasoning dsCQMReasoning = null;
                BLObject<DSCQMReasoning> obj;
                obj = BLLCQMObj.Load_Patients_CQM(PatientIds);

                dsCQMReasoning = obj.Data;
                if (obj.Data != null)
                {
                    if (dsCQMReasoning.Tables[dsCQMReasoning.Patients_CQM.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            AllPatientsCount = dsCQMReasoning.Tables[dsCQMReasoning.Patients_CQM.TableName].Rows.Count,
                            AllPatientsLoad_JSON = MDVUtility.JSON_DataTable(dsCQMReasoning.Tables[dsCQMReasoning.Patients_CQM.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            AllPatientsCount = 0,
                            AllPatientsLoad_JSON = "[]",
                            Message = Common.AppPrivileges.No_Record_Message
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

        // Author:  Muhammad Arshad
        // Created Date: 01/23/2017
        //OverView: loads Patient Data Based on PatientIds
        public string saveCQMReasonValue(CQMModel cqmModel)
        {
            try
            {

                DSCQMReasoning dsCQMReasoning = new DSCQMReasoning();
                DSCQMReasoning.CQM_ReasonValueRow dr = dsCQMReasoning.CQM_ReasonValue.NewCQM_ReasonValueRow();
                dr.cqmReasonId = -1;
                dr.PatientId = MDVUtility.ToInt64(cqmModel.PatientId);
                dr.NoteId = MDVUtility.ToInt64(cqmModel.NoteId);
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.Systolic = cqmModel.Systolic;
                dr.SystolicLOINC = cqmModel.SystolicLOINC;
                dr.Diastolic = cqmModel.Diastolic;
                dr.DiastolicLOINC = cqmModel.DiastolicLOINC;
                dr.SNOMED = cqmModel.SNOMED;
                dr.CPT = cqmModel.CPT;
                dr.CVX = cqmModel.CVX;
                dr.HCPCS = cqmModel.HCPCS;
                dr.RXNORM = cqmModel.RXNORM;
                dr.LOINC = cqmModel.LOINC;
                dr.MeasureId = cqmModel.MeasureId;
                dr.ReportFromDate = MDVUtility.ToDateTime(cqmModel.ReportFromDate);
                dr.ReportToDate = MDVUtility.ToDateTime(cqmModel.ReportToDate);
                dr.ICD9CM = cqmModel.ICD9CM;
                dr.ICD10CM = cqmModel.ICD10CM;
                dr.BMI = cqmModel.BMI;
                dr.BMILOINC = cqmModel.BMILOINC;
                dr.ActionResult = cqmModel.ActionResult;
                dr.bSignNote = cqmModel.bSignNote;

                #region Database Insertion
                dsCQMReasoning.CQM_ReasonValue.AddCQM_ReasonValueRow(dr);
                BLObject<DSCQMReasoning> obj = BLLCQMObj.Insert_CQM_Reason_Value(dsCQMReasoning);
                dsCQMReasoning = obj.Data;

                if (obj.Data != null)
                {
                    Int64 cqmReasonId = MDVUtility.ToInt64(dsCQMReasoning.Tables[dsCQMReasoning.CQM_ReasonValue.TableName].Rows[0][dsCQMReasoning.CQM_ReasonValue.cqmReasonIdColumn.ColumnName]);
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        cqmReasonId = MDVUtility.ToInt64(dsCQMReasoning.Tables[dsCQMReasoning.CQM_ReasonValue.TableName].Rows[0][dsCQMReasoning.CQM_ReasonValue.cqmReasonIdColumn.ColumnName]),
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

        // Author:  Muhammad Arshad
        // Created Date: 01/3/2017
        //OverView: loads Patient BMI
        public string loadPatientBMI(CQMModel cqmModel)
        {
            try
            {
                string patientBMI = null;
                patientBMI = BLLCQMObj.loadPatientBMI(MDVUtility.ToInt64(cqmModel.PatientId), MDVUtility.ToInt64(cqmModel.NoteId), cqmModel.ReportFromDate, cqmModel.ReportToDate);
                var response = new
                {
                    status = true,
                    BMI = patientBMI
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

        // Author:  Muhammad Arshad
        // Created Date: 14/3/2017
        //OverView: get Recent Note of Patient
        public string getPatientRecentNote(CQMModel cqmModel)
        {
            try
            {
                string patientRecentNoteId = null;
                patientRecentNoteId = BLLCQMObj.getPatientRecentNote(MDVUtility.ToInt64(cqmModel.PatientId), cqmModel.NoteId);
                var response = new
                {
                    status = true,
                    NoteId = patientRecentNoteId
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

        // Author:  Muhammad Arshad
        // Created Date: 12 April 2017
        //OverView: get VBP Measure Questionnaire Answers
        public string loadVBPMeasureQuestionnaireAnswers(CQMModel model)
        {
            try
            {
                DataTable dtVBPMeasureQuestionnaireAnswers = new DataTable();

                dtVBPMeasureQuestionnaireAnswers = BLLCQMObj.loadVBPMeasureQuestionnaireAnswers(model.MeasureId, model.NoteId, model.PatientId);
                if (dtVBPMeasureQuestionnaireAnswers.Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        VBPMeasureQuestionnaireAnswersCount = dtVBPMeasureQuestionnaireAnswers.Rows.Count,
                        VBPMeasureQuestionnaireAnswersLoad_JSON = MDVUtility.JSON_DataTable(dtVBPMeasureQuestionnaireAnswers),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        VBPMeasureQuestionnaireAnswersCount = 0,
                        VBPMeasureQuestionnaireAnswersLoad_JSON = "[]",
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

        // Author:  Muhammad Arshad
        // Created Date: 14 April 2017
        //OverView: Saves Patient VBP values
        public string saveVBPReasonValue(VBPModel vbpModel)
        {
            try
            {
                string CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                DateTime CreatedOn = DateTime.Now;
                string ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                DateTime ModifiedOn = DateTime.Now;
                string IsActive = "true";

                #region Database Insertion
                string result = BLLCQMObj.Insert_VBP_Reason_Value(vbpModel.MeasureId, vbpModel.QuestionAnswersId, MDVUtility.ToInt64(vbpModel.ProviderId), MDVUtility.ToInt64(vbpModel.PatientId), MDVUtility.ToInt64(vbpModel.NoteId), vbpModel.MeasureQuestionnaireId
                                          , vbpModel.CPT, vbpModel.Score, IsActive, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy);

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                #endregion

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string saveVBPDepressionValue(VBPModel vbpModel)
        {
            try
            {
                string CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                DateTime CreatedOn = DateTime.Now;
                string ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                DateTime ModifiedOn = DateTime.Now;
                string IsActive = "true";

                #region Database Insertion
                string result = BLLCQMObj.Insert_VBP_Depression_Value(vbpModel.MeasureId, vbpModel.QuestionAnswersId, MDVUtility.ToInt64(vbpModel.ProviderId), MDVUtility.ToInt64(vbpModel.PatientId), MDVUtility.ToInt64(vbpModel.NoteId), vbpModel.MeasureQuestionnaireId
                                          , IsActive, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, vbpModel.MeasureType, vbpModel.Comments);

                var response = new
                {
                    status = true,
                    message = Common.AppPrivileges.Save_Message,
                    MUAlertsCount= result
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                #endregion

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        // Author:  Muhammad Arshad
        // Created Date: 24 April 2017
        //OverView: get VBP Score for CurrentNote
        public string loadVBPScore(Int64 NotesId, string MeasureNumber)
        {
            try
            {
                DataTable dtVBPScore = new DataTable();
                dtVBPScore = BLLCQMObj.loadVBPScore(NotesId, MeasureNumber);
                if (dtVBPScore.Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        VBPScoreCount = dtVBPScore.Rows.Count,
                        VBPScoreLoad_JSON = MDVUtility.JSON_DataTable(dtVBPScore),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        VBPScoreCount = 0,
                        VBPScoreLoad_JSON = "[]",
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
        public string loadProviderVBPMeasures(Int64 ProviderID)
        {
            try
            {
                string allMeasures;
                allMeasures = BLLCQMObj.loadProviderVBPMeasures(ProviderID);
                if (allMeasures != "")
                {
                    var response = new
                    {
                        status = true,
                        Measures = allMeasures,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Measures = "",
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




        #region MetaData

        public static readonly Dictionary<string, string> POS_NUCC_Code = new Dictionary<string, string>
        {
           {"20"  , "261QU0200X" } ,
           {"23"  , "261QE0002X" }

        };

        #endregion

        #region NoteDocuments
        public string Detach_DocumentsFromNote(string patientDocumentIds, long NotesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(patientDocumentIds)) || string.IsNullOrEmpty(MDVUtility.ToStr(NotesId)))
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
                    BLObject<string> obj = BLLClinicalObj.DetachDocumentsFromNote(patientDocumentIds, NotesId);
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

        public string DetachPatientDocumentFromNote(long patientDocumentId, long notesId)
        {
            try
            {
                if (patientDocumentId <= 0 && notesId <= 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.No_Record_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {

                    BLObject<string> obj = BLLPatientObj.DeleteNoteDocument(patientDocumentId, notesId, 0);
                    if (string.IsNullOrEmpty(obj.Data))
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

        public string NoteAttachmentExists(long notesId)
        {
            try
            {
                BLObject<string> obj = BLLClinicalObj.NoteAttachmentExists(notesId);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        IsAttachmentExists = obj.Data,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        IsAttachmentExists = false
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
        #endregion
        #endregion

        #region Notes Extra Info
        /// <summary>
        /// Saves the Clinical Notes Extra Info.
        /// </summary>
        /// <param name="model">DTO Object Having Notes Extra Info Information</param>
        /// <param name="PatientID"></param>
        /// <returns>Successful/UnsucessFul Message</returns>
        public string SaveNotesExtraInfo(ClinicalNotesExtraInfoModel model)
        {
            try
            {


                DSNotes dsNotes = new DSNotes();

                BLObject<DSNotes> obj;
                obj = BLLClinicalObj.GetReferenceData();
                dsNotes = obj.Data;
                if (obj.Data != null)
                {
                    foreach (DSNotes.ReferenceDataRow dr in dsNotes.ReferenceData)
                    {
                        DSNotes.PatientMUSettingRow drPatientMUSetting = dsNotes.PatientMUSetting.NewPatientMUSettingRow();
                        drPatientMUSetting.PatientMUSettingId = -1;
                        drPatientMUSetting.NoteId = MDVUtility.ToLong(model.NoteId);
                        drPatientMUSetting.PatientId = MDVUtility.ToLong(model.PatientId);
                        drPatientMUSetting.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        drPatientMUSetting.CreatedOn = DateTime.Now;
                        drPatientMUSetting.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        drPatientMUSetting.ModifiedOn = DateTime.Now;

                        if (dr.Description == "IsMedicationReconcile")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;

                            if (!string.IsNullOrEmpty(model.MedicalReconciliation))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.MedicalReconciliation);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }


                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "IsAllergiesReconcile")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;


                            if (!string.IsNullOrEmpty(model.AllergiesReconciliation))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.AllergiesReconciliation);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }

                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "IsProblemReconcile")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;


                            if (!string.IsNullOrEmpty(model.ProblemsReconciliation))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.ProblemsReconciliation);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }

                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "Patient education")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;


                            if (!string.IsNullOrEmpty(model.PatientEducation))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.PatientEducation);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }

                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "Locate CCDA")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;
                            if (!string.IsNullOrEmpty(model.LocateCCDA))
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.LocateCCDA);
                            else
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "Summery of care (A)")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;

                            if (!string.IsNullOrEmpty(model.SummaryOfCareA))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.SummaryOfCareA);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }
                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "Summery of care (B)")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;

                            if (!string.IsNullOrEmpty(model.SummaryOfCareB))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.SummaryOfCareB);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }

                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "Summary of care (C)")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;

                            if (!string.IsNullOrEmpty(model.SummaryOfCareC))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.SummaryOfCareC);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }

                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "VDT (Timely Access)")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;

                            if (!string.IsNullOrEmpty(model.VDTTimelyAccess))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.VDTTimelyAccess);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }

                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "Secure messages sent to provider")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;

                            if (!string.IsNullOrEmpty(model.SecureMessage))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.SecureMessage);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }

                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "VDT API (Timely Access)")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;

                            if (!string.IsNullOrEmpty(model.VDTAPITimelyAccess))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.VDTAPITimelyAccess);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }

                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "Transitions of Care")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;

                            if (!string.IsNullOrEmpty(model.TransitionsOfCare))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.TransitionsOfCare);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }

                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                        else if (dr.Description == "VDT API (Patient)")
                        {
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;

                            if (!string.IsNullOrEmpty(model.VDTAPIPatient))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.VDTAPIPatient);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }

                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                    }
                    #region Database Insertion
                    BLObject<DSNotes> objSave = BLLClinicalObj.InsertPatientMUSetting(dsNotes);
                    if (objSave.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Save_Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objSave.Message
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

        public string UpdateNotesExtraInfo(ClinicalNotesExtraInfoModel model)
        {
            try
            {

                DSNotes dsNotes = null;
                BLObject<DSNotes> obj;
                obj = BLLClinicalObj.LoadPatientMUSetting(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId));
                dsNotes = obj.Data;
                ClinicalNotesExtraInfoModel NotesExtraInfoModel = new ClinicalNotesExtraInfoModel();
                DSNotes dsPatientMuInfoUpdate = new DSNotes();

                if (obj.Data != null)
                {
                    if (dsNotes.Tables[dsNotes.PatientMUSetting.TableName].Rows.Count > 0)
                    {
                        DSNotes dsReferralData = new DSNotes();
                        BLObject<DSNotes> objReferralData;
                        objReferralData = BLLClinicalObj.GetReferenceData();
                        dsReferralData = objReferralData.Data;
                        if (objReferralData.Data != null)
                        {
                            foreach (DSNotes.ReferenceDataRow dr in dsReferralData.ReferenceData)
                            {
                                if (dr.Description == "IsMedicationReconcile")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        drPatientMUSetting.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                        drPatientMUSetting.ModifiedOn = DateTime.Now;
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.MedicalReconciliation))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.MedicalReconciliation);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            //dsPatientMuInfoUpdate.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "IsAllergiesReconcile")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        drPatientMUSetting.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                        drPatientMUSetting.ModifiedOn = DateTime.Now;
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.AllergiesReconciliation))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.AllergiesReconciliation);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            //dsPatientMuInfoUpdate.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "IsProblemReconcile")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        drPatientMUSetting.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                        drPatientMUSetting.ModifiedOn = DateTime.Now;
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.ProblemsReconciliation))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.ProblemsReconciliation);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            //dsPatientMuInfoUpdate.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Locate CCDA")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        drPatientMUSetting.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                        drPatientMUSetting.ModifiedOn = DateTime.Now;
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.LocateCCDA))
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.LocateCCDA);
                                            else
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Patient education")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.PatientEducation))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.PatientEducation);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            //dsPatientMuInfoUpdate.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Summery of care (A)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.SummaryOfCareA))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.SummaryOfCareA);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            //dsPatientMuInfoUpdate.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Summery of care (B)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.SummaryOfCareB))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.SummaryOfCareB);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            //dsPatientMuInfoUpdate.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Summary of care (C)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.SummaryOfCareC))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.SummaryOfCareC);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "VDT (Timely Access)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.VDTTimelyAccess))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.VDTTimelyAccess);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            //dsPatientMuInfoUpdate.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Secure messages sent to provider")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.SecureMessage))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.SecureMessage);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            //dsPatientMuInfoUpdate.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                                            break;
                                        }
                                    }

                                }
                                else if (dr.Description == "VDT API (Timely Access)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.VDTAPITimelyAccess))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.VDTAPITimelyAccess);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            //dsPatientMuInfoUpdate.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Transitions of Care")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.TransitionsOfCare))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.TransitionsOfCare);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            //dsPatientMuInfoUpdate.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "VDT API (Patient)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            if (!string.IsNullOrEmpty(model.VDTAPIPatient))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.VDTAPIPatient);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }
                                            //dsPatientMuInfoUpdate.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                                            break;
                                        }
                                    }
                                }

                            }
                            #region Database Updation
                            BLObject<DSNotes> objUpdate = BLLClinicalObj.UpdatePatientMenuSetting(dsNotes);
                            if (objUpdate.Data != null)
                            {
                                var response = new
                                {
                                    status = true,
                                    Message = Common.AppPrivileges.Update_Message
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = objUpdate.Message
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
                                Message = objReferralData.Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.No_Record_Message
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

        public string SearchNotesExtraInfo(ClinicalNotesExtraInfoModel model)
        {
            try
            {

                DSNotes dsNotes = null;
                BLObject<DSNotes> obj;
                obj = BLLClinicalObj.LoadPatientMUSetting(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId));
                dsNotes = obj.Data;
                ClinicalNotesExtraInfoModel NotesExtraInfoModel = new ClinicalNotesExtraInfoModel();

                if (obj.Data != null)
                {
                    if (dsNotes.Tables[dsNotes.PatientMUSetting.TableName].Rows.Count > 0)
                    {
                        DSNotes dsReferralData = new DSNotes();
                        BLObject<DSNotes> objReferralData;
                        objReferralData = BLLClinicalObj.GetReferenceData();
                        dsReferralData = objReferralData.Data;
                        if (objReferralData.Data != null)
                        {
                            foreach (DSNotes.ReferenceDataRow dr in dsReferralData.ReferenceData)
                            {
                                if (dr.Description == "IsMedicationReconcile")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.MedicalReconciliation = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "IsAllergiesReconcile")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.AllergiesReconciliation = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "IsProblemReconcile")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.ProblemsReconciliation = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Patient education")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.PatientEducation = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Summery of care (A)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.SummaryOfCareA = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Summery of care (B)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.SummaryOfCareB = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Summary of care (C)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.SummaryOfCareC = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "VDT (Timely Access)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.VDTTimelyAccess = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Secure messages sent to provider")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.SecureMessage = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "VDT API (Timely Access)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.VDTAPITimelyAccess = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Transitions of Care")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.TransitionsOfCare = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "VDT API (Patient)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.VDTAPIPatient = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "Locate CCDA")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.LocateCCDA = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                            }
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                Found = true,
                                NotesExtraData = js.Serialize(NotesExtraInfoModel)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objReferralData.Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Found = false,
                            Message = Common.AppPrivileges.No_Record_Message
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
        #endregion

        #region Notes Seperate Component Implementation

        public string insertNoteComponent(NoteComponentModel model)
        {
            try
            {
                BLObject<string> obj = BLLClinicalObj.insertNoteComponent(model);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        NoteComponentId = obj.Data.ToString(),
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
            catch (Exception ex)
            {
                var response = new
                {
                    status = true,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string updateNoteComponent(NoteComponentModel model)
        {
            try
            {
                BLObject<string> obj = BLLClinicalObj.updateNoteComponent(model);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        NoteComponentId = obj.Data.ToString(),
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
            catch (Exception ex)
            {
                var response = new
                {
                    status = true,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string loadNoteComponents(NoteComponentModel model)
        {
            try
            {
                List<NoteComponentModel> NoteComponentList = null;
                BLObject<List<NoteComponentModel>> obj;

                obj = BLLClinicalObj.loadNoteComponents(model.NotesId);
                NoteComponentList = obj.Data;
                if (obj.Data != null)
                {
                    if (NoteComponentList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            NoteComponentListCount = NoteComponentList.Count,
                            NoteComponentListFill_JSON = NoteComponentList,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            NoteComponentListCount = 0,
                            Message = "Record not found."
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

        public string deleteNoteComponent(NoteComponentModel model)
        {
            try
            {
                BLObject<string> obj = BLLClinicalObj.deleteNoteComponent(MDVUtility.ToInt64(model.NoteComponentId), model.NotesId);
                if (obj.Data != null && obj.Data == "")
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

        public string setNoteComponentsOrder(string noteComponentIds)
        {
            try
            {
                BLObject<string> obj = BLLClinicalObj.SetNotesComponentsOrder(noteComponentIds);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        NoteComponentId = obj.Data.ToString(),
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
            catch (Exception ex)
            {
                var response = new
                {
                    status = true,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string insertNoteComponentsBulk(List<NoteComponentModel> ComponentsModelList, bool IsNoteUpdate)
        {
            try
            {
                List<NoteComponentModel> NoteComponentList = null;
                BLObject<List<NoteComponentModel>> obj;

                string NoteComponentsXml = createNoteComponentsXml(ComponentsModelList);
                obj = BLLClinicalObj.insertNoteComponentsBulk(NoteComponentsXml, null, IsNoteUpdate);
                if (obj.Data != null)
                {
                    NoteComponentList = obj.Data;
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        NoteComponentListCount = NoteComponentList.Count,
                        NoteComponentListFill_JSON = NoteComponentList,
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
        public static string createNoteComponentsXml(List<NoteComponentModel> ComponentsModel)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NoteComponentModel>));
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, ComponentsModel);
                return textWriter.ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        #endregion

        #region Notes Component Audit

        public string loadNoteComponentAudit(NoteComponentAuditModel model)
        {
            try
            {
                List<NoteComponentAuditModel> NoteComponentList = null;
                BLObject<List<NoteComponentAuditModel>> obj;

                obj = BLLClinicalObj.loadNoteComponentAudit(model.NotesId, model.NoteComponentAuditId, model.PageNumber, model.RowsPerPage);
                if (obj.Data != null)
                {
                    NoteComponentList = obj.Data;
                    if (NoteComponentList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ComponentAuditCount = NoteComponentList.Count,
                            iTotalDisplayRecords = NoteComponentList.FirstOrDefault().RecordCount,
                            ComponentAudit_JSON = NoteComponentList,
                        };

                        var jsonSettings = new JsonSerializerSettings();
                        jsonSettings.DateFormatString = "MM/dd/yyyy hh:mm:ss";

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response, jsonSettings));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            ComponentAuditCount = 0,
                            Message = "Record not found."
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

        #endregion

        #region Legacy Notes

        public string LoadLegacyNoteAndRenderTemplate(List<NotesComponents> objListNotesComponents, List<string> ExcludedImages, bool IsSaveDiagnosticResult, string NotesPreviewStyle = null)
        {
            try
            {
                LegacyNotesViewModel objLegacyNotesViewModel = new LegacyNotesViewModel();
                string strNotes = string.Empty;
                objLegacyNotesViewModel = BLLClinicalObj.LoadLegacyNoteAndRenderTemplate(objListNotesComponents);
                string NotesHtml = string.Empty;
                int counterLeftPan = 0;
                int counterRightPan = 0;
                List<NotesComponent> objList_NotesComponent = objLegacyNotesViewModel.NotesComponent;
                if (objList_NotesComponent == null)
                {
                    objList_NotesComponent = new List<NotesComponent>();
                }

                var dsReportHeader = new MDVision.DataAccess.DAL.ReportHeader.DALReportHeader().getReportHeaderTagsValue(objListNotesComponents.FirstOrDefault().PatientId, objListNotesComponents.FirstOrDefault().ProviderId, objListNotesComponents.FirstOrDefault().NotesId, "Notes", NotesPreviewStyle);

                DSReportHeader.ReportHeaderTagsRow drReportHeader = (DSReportHeader.ReportHeaderTagsRow)dsReportHeader.ReportHeaderTags.Rows[0];
                drReportHeader.DOS.Replace("12:00AM", "");

                string NoteReason = string.Empty;
                if (objLegacyNotesViewModel.NoteHeaderData != null && objLegacyNotesViewModel.NoteHeaderData.Count() > 0)
                {
                    var obhNoteHeader = objLegacyNotesViewModel.NoteHeaderData.FirstOrDefault(m => m.Type == "Note");
                    if (obhNoteHeader != null)
                    {
                        NoteReason = obhNoteHeader.NoteReason;
                    }
                    NotesHtml += NotesHeader(objLegacyNotesViewModel.NoteHeaderData, drReportHeader);
                }

                List<NotesComponent> FreeTextComponents = new List<NotesComponent>();
                //FreeTextComponents = objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "free text").OrderBy(m => m.OrderNo).ToList();
                FreeTextComponents = objList_NotesComponent.OrderBy(m => m.OrderNo).ToList();
                int OrderNoComponent = -1;


                NotesHtml += "<div style='font-family:arial unicode ms;'><table style='width:100%;border-top-left-radius: 5px;'><tr style='border-top-left-radius: 5px;'><td style='border-left:solid 1px #EFEFEF;border-right:solid 1px #EFEFEF;border-bottom:solid 1px #EFEFEF;border-top:3px solid #163a6e;border-top-left-radius: 5px;'><table style='width:100%;border-top-left-radius: 5px;'><tr style='border-top-left-radius: 5px;'><td style='width:40%;vertical-align:top;border-top-left-radius: 5px;'>";
                NotesHtml += "<table style='width:100%;border-top-left-radius: 5px;'>";

                //if (objListNotesComponents.FirstOrDefault(m => m.Component == "Medications") != null || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medications") != null)
                //{
                //    counterLeftPan = 1;
                //    NotesHtml += "<tr><td>" + NotesMedicationHx(objLegacyNotesViewModel.MedicationHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "medications").ToList()) + "</td></tr>";
                //}

                if (objListNotesComponents.FirstOrDefault(m => m.Component == "Medications") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medications") != null)
                {
                    NotesHtml += "<tr><td>" + NotesMedication(objLegacyNotesViewModel.MedicationHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "medications").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medications") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medications").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "medications").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }
                if (objListNotesComponents.FirstOrDefault(m => m.Component == "SocialHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socialhx") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesSocialHx(objLegacyNotesViewModel.SocialHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "socialhx").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socialhx") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socialhx").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "socialhx").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }
                if (objListNotesComponents.FirstOrDefault(m => m.Component == "Social,PsychologicalandBehaviorHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socpsyandbehaviorhx") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesSocPsyandBehaviorHx(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "socpsyandbehaviorhx").ToList(), objListNotesComponents.FirstOrDefault().NotesId) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socpsyandbehaviorhx") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socpsyandbehaviorhx").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "socpsyandbehaviorhx").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }
                if (objListNotesComponents.FirstOrDefault(m => m.Component == "MedicalHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medicalhx") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesMedicalHx(objLegacyNotesViewModel.MedicalHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "medicalhx").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medicalhx") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medicalhx").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "medicalhx").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if (objListNotesComponents.FirstOrDefault(m => m.Component == "FamilyHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "familyhx") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesFamilyHx(objLegacyNotesViewModel.FamilyHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "familyhx").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "familyhx") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "familyhx").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "familyhx").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if (objListNotesComponents.FirstOrDefault(m => m.Component == "SurgicalHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "surgicalhx") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesSurgicalHx(objLegacyNotesViewModel.SurgicalHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "surgicalhx").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "surgicalhx") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "surgicalhx").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "surgicalhx").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if (objListNotesComponents.FirstOrDefault(m => m.Component == "BirthHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "birthhx") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesBirthHx(objLegacyNotesViewModel.BirthHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "birthhx").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "birthhx") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "birthhx").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "birthhx").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if (objListNotesComponents.FirstOrDefault(m => m.Component == "HospitalizationHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "hospitalizationhx") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesHospitalizationHx(objLegacyNotesViewModel.HospitalizationHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "hospitalizationhx").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "hospitalizationhx") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "hospitalizationhx").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "hospitalizationhx").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if (objListNotesComponents.FirstOrDefault(m => m.Component == "Allergies") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "allergies") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesAlleryHx(objLegacyNotesViewModel.AllergyHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "allergies").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "allergies") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "allergies").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "allergies").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if (objListNotesComponents.FirstOrDefault(m => m.Component.ToLower() == "reviewofsystems") != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "review of systems").Count() > 0)
                {
                    NotesHtml += "<tr><td><br/>" + NotesROS(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "review of systems").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "review of systems") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "review of systems").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "review of systems").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if (counterLeftPan == 0)
                {
                    NotesHtml += "<tr><td>&nbsp;</td></tr>";
                }
                NotesHtml += "</table>";
                NotesHtml += "</td><td style='width:60%;vertical-align:top;border-left:2px solid #EFEFEF;'>";
                NotesHtml += "<table style='width:100%;marging-top:5px;'>";

                if (objLegacyNotesViewModel.NotesComponent != null && objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                {
                    counterRightPan = 1;

                    NotesHtml += "<tr><td>" + NotesAppointmentReason(objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "free text").ToList(), NoteReason) + "</td></tr>";
                    if (objLegacyNotesViewModel.NotesComponent != null && objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "custom").Count() > 0)
                    {
                        List<NotesComponent> CustomTextComponents = new List<NotesComponent>();
                        CustomTextComponents = objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "custom").ToList();
                        HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();

                        foreach (NotesComponent obj in CustomTextComponents)
                        {
                            try
                            {
                                document.LoadHtml(obj.SOAPText);
                                HtmlAgilityPack.HtmlNode node = document.DocumentNode.SelectSingleNode("//li[contains(@class, 'CustomComponent')]/ul/li/span");

                                if (node != null)
                                {
                                    OrderNoComponent = obj.OrderNo;
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        foreach (var obj2 in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "custom").OrderBy(m => m.OrderNo))
                                        {
                                            if (obj2.OrderNo >= OrderNoComponent && obj2.ComponentName.ToLower() == "free text")
                                            {
                                                if (FreeTextComponents.Where(m => m.OrderNo == obj2.OrderNo).Count() > 0)
                                                {
                                                    counterRightPan = 1;
                                                    NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj2.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                    FreeTextComponents.RemoveAll(m => m.OrderNo == obj2.OrderNo);
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    counterLeftPan = 1;
                                }
                                OrderNoComponent = -1;
                            }
                            catch
                            {

                            }
                        }


                    }
                }

                if ((objLegacyNotesViewModel.Complaints != null && objLegacyNotesViewModel.Complaints.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "complaints") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesComplaints(objLegacyNotesViewModel.Complaints, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "complaints").ToList(), objListNotesComponents.FirstOrDefault().NotesId) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "complaints") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "complaints").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "complaints").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.VitalSigns != null && objLegacyNotesViewModel.VitalSigns.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "vitals") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesVitalSigns(objLegacyNotesViewModel.VitalSigns, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "vitals").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "vitals") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "vitals").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "vitals").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.PhysicalExam != null && objLegacyNotesViewModel.PhysicalExam.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "physical exam") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesPhysicalExam(objLegacyNotesViewModel.PhysicalExam, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "physical exam").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "physical exam") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "physical exam").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "physical exam").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.LabOrderResult != null && objLegacyNotesViewModel.LabOrderResult.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab results") != null)
                {
                    NotesHtml += "<tr><td style='padding-left:12px;padding-top:5px;padding-bottom:5px;padding-right:8px;'><br/>" + NotesLabOrderResult(objLegacyNotesViewModel.LabOrderResult, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "lab results").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab results") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab results").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "lab results").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.RadOrderResult != null && objLegacyNotesViewModel.RadOrderResult.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "diagnostic imaging results") != null)
                {
                    NotesHtml += "<tr><td style='padding-left:12px;padding-top:5px;padding-bottom:5px;padding-right:8px;'><br/>" + NotesRadOrderResult(objLegacyNotesViewModel.RadOrderResult, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "diagnostic imaging results").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "diagnostic imaging results") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "diagnostic imaging results").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "diagnostic imaging results").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.ProblemHx != null) && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "problems") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesProblemHx(objLegacyNotesViewModel.ProblemHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "problems").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "problems") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "problems").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "problems").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }
                // Start Added by Zia Mehmood
                if (objLegacyNotesViewModel.NotesComponent != null && objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "order sets" || m.ComponentName.ToLower() == "order sets").Count() > 0)
                {
                    NotesHtml += "<tr><td><br/>" + NotesOrderSets(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "order sets").ToList(), objListNotesComponents.FirstOrDefault().NotesId, objListNotesComponents) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "order sets") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "order sets").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "order sets").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }
                // End Added by Zia Mehmood
                if ((objLegacyNotesViewModel.Prescription != null && objLegacyNotesViewModel.Prescription.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "prescription") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesPrescription(objLegacyNotesViewModel.Prescription, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "prescription").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "prescription") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "prescription").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "prescription").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.LabOrder != null && objLegacyNotesViewModel.LabOrder.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab order") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesLabOrder(objLegacyNotesViewModel.LabOrder, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "lab order" || m.ComponentName.ToLower() == "lab orders").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab order") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab order").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "lab order").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }
                if ((objLegacyNotesViewModel.PatientLetter != null && objLegacyNotesViewModel.PatientLetter.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "letter") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesPatLetter(objLegacyNotesViewModel.PatientLetter, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "letter").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "letter") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "letter").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "letter").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.RadOrder != null && objLegacyNotesViewModel.RadOrder.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "diagnostic imaging order") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesRadOrder(objLegacyNotesViewModel.RadOrder, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "diagnostic imaging order").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "diagnostic imaging order") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "diagnostic imaging order").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "diagnostic imaging order").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.ProcedureOrder != null && objLegacyNotesViewModel.ProcedureOrder.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedure order") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesProcedureOrder(objLegacyNotesViewModel.ProcedureOrder, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "procedure order").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedure order") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedure order").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "procedure order").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.ConsultationOrder != null && objLegacyNotesViewModel.ConsultationOrder.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "consultation order") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesConsultationOrder(objLegacyNotesViewModel.ConsultationOrder, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "consultation order").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "consultation order") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "consultation order").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "consultation order").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.ImmunizationHx != null && objLegacyNotesViewModel.ImmunizationHx.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "immunization") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesImmunizationHx(objLegacyNotesViewModel.ImmunizationHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "immunization").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "immunization") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "immunization").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "immunization").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.Procedure != null && objLegacyNotesViewModel.Procedure.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedures") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesProcedures(objLegacyNotesViewModel.Procedure, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "procedures").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedures") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedures").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "procedures").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.Referrals != null && objLegacyNotesViewModel.Referrals.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "referrals") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesReferrals(objLegacyNotesViewModel.Referrals, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "referrals").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "referrals") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "referrals").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "referrals").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.PatientEducation != null && objLegacyNotesViewModel.PatientEducation.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "patient education") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesPatientEducation(objLegacyNotesViewModel.PatientEducation, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "patient education").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "patient education") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "patient education").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "patient education").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if (objListNotesComponents.FirstOrDefault(m => m.Component == "FollowUp") != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "follow up").Count() > 0)
                {
                    NotesHtml += "<tr><td><br/>" + NotesFollowUp(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "follow up").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "follow up") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "follow up").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "follow up").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if ((objLegacyNotesViewModel.FunctionalCognitive != null && objLegacyNotesViewModel.FunctionalCognitive.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "functional and cognitive") != null)
                {
                    NotesHtml += "<tr><td><br/>" + NotesFunctionalCognitive(objLegacyNotesViewModel.FunctionalCognitive, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "functional and cognitive").ToList()) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "functional and cognitive") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "functional and cognitive").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "functional and cognitive").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if (objList_NotesComponent != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "custom forms").Count() > 0)
                {
                    NotesHtml += "<tr><td><br/>" + NotesCustomForm(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "custom forms").ToList(), objListNotesComponents) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "custom forms") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "custom forms").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "custom forms").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }

                if (objListNotesComponents.FirstOrDefault(m => m.Component.ToLower() == "treatment") != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "treatment").Count() > 0)
                {
                    NotesHtml += "<tr><td><br/>" + NotesTreatment(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "treatment").ToList(), objListNotesComponents.FirstOrDefault().NotesId) + "</td></tr>";
                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                    {
                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "treatment") != null)
                        {
                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "treatment").OrderNo;
                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "treatment").OrderBy(m => m.OrderNo))
                            {
                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                {
                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                    {
                                        counterRightPan = 1;
                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        OrderNoComponent = -1;
                    }
                    counterLeftPan = 1;
                }
                //if (objLegacyNotesViewModel.eSuperbill != null && objLegacyNotesViewModel.eSuperbill.Count() > 0)
                //{
                //    counterRightPan = 1;
                //    NotesHtml += "<tr><td>" + NoesESuperbill(objLegacyNotesViewModel.eSuperbill) + "</td></tr>";
                //}

                if (FreeTextComponents != null && FreeTextComponents.Count() > 0)
                {
                    counterRightPan = 1;
                    NotesHtml += "<tr><td><br/>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                }
                //if (objLegacyNotesViewModel.NotesComponent != null && objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "custom").Count() > 0)
                //{
                //    counterRightPan = 1;
                //    NotesHtml += "<tr><td>" + NotesCustomFreeText(objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "custom").ToList()) + "</td></tr>";
                //}
                //if (objLegacyNotesViewModel.NotesComponent != null && objLegacyNotesViewModel.NotesComponent.Count() > 0)
                //{
                //    counterRightPan = 1;
                //    NotesHtml += "<tr><td>" + NotesCustomTagInsertedFreeText(objLegacyNotesViewModel.NotesComponent) + "</td></tr>";
                //}

                if (objList_NotesComponent != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "images").Count() > 0)
                {
                    counterRightPan = 1;
                    NotesHtml += "<tr><td>";
                    NotesHtml += NotesImages(objListNotesComponents.FirstOrDefault().NotesId, ExcludedImages);
                    NotesHtml += "</td></tr>";
                }

                if (objList_NotesComponent != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "signature").Count() > 0)
                {
                    counterRightPan = 1;
                    NotesHtml += "<tr><td>";
                    NotesHtml += NotesSignature(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "signature").ToList(), objLegacyNotesViewModel.NoteHeaderData.Where(m => m.Type == "Provider").ToList());
                    NotesHtml += "</td></tr>";
                }
                if (objLegacyNotesViewModel.NotesComponent != null && objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "cosign" || m.ComponentName.ToLower() == "co-sign").Count() > 0)
                {
                    counterRightPan = 1;
                    NotesHtml += "<tr><td>" + NotesCoSign(objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "cosign" || m.ComponentName.ToLower() == "co-sign").ToList()) + "</td></tr>";
                }
                if (objList_NotesComponent != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "amendment").Count() > 0)
                {
                    counterRightPan = 1;
                    NotesHtml += "<tr><td>" + NotesAmendment(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "amendment").ToList()) + "</td></tr>";
                }
                if (counterRightPan == 0)
                {
                    NotesHtml += "<tr><td>&nbsp;</td></tr>";
                }
                NotesHtml += "</table>";
                NotesHtml += "</td></tr>";

                if (IsSaveDiagnosticResult)
                {
                    string esignedBy = "<p id='signedBy'>e-Signed By: " + MDVSession.Current.AppUserFullName + " on " + DateTime.Today.ToLongDateString() + " at " + DateTime.Today.ToLongTimeString() + "</p>";
                    NotesHtml += "</table></td></tr></table>" + esignedBy + "</div>";
                }
                else
                {
                    NotesHtml += "</table></td></tr></table></div>";
                }

                NotesHtml = NotesHtml.Replace("(Info)", ""); //NotesHtml.Split('(Info)').join("");
                string CSS = CssFiles();
                NotesHtml = SetSpacing(NotesHtml);
                NotesHtml = ReplaceSpecialCharacters(NotesHtml);
                byte[] strBytes = ConvertHtmlToPdf(NotesHtml, CSS, objLegacyNotesViewModel.NoteHeaderData, drReportHeader);
                var response = new
                {
                    status = false,
                    Message = Convert.ToBase64String(strBytes),
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

        public string SetSpacing(string NotesHTML)
        {
            NotesHTML = NotesHTML.Replace("</strong>", " </strong>");
            NotesHTML = NotesHTML.Replace("</b>", " </b>");
            NotesHTML = NotesHTML.Replace("  </b>", " </b>");
            NotesHTML = NotesHTML.Replace("<br />", " <br />");
            NotesHTML = NotesHTML.Replace("</li>", " </li>");
            NotesHTML = NotesHTML.Replace("</div>", " </div>");
            NotesHTML = NotesHTML.Replace("</p>", " </p>");
            NotesHTML = NotesHTML.Replace("</section>", " </section>");
            NotesHTML = NotesHTML.Replace("/*", "/ *");
            return NotesHTML;
        }

        public string ReplaceSpecialCharacters(string NotesHTML)
        {
            string[] sc = SpecialCharacters();
            for (int i = 0; i < sc.Length; i++)
            {
                NotesHTML = NotesHTML.Replace(sc[i], System.Web.HttpUtility.HtmlEncode(sc[i]));
            }

            return NotesHTML;
        }

        public string[] SpecialCharacters()
        {
            string[] sc = new string[] { "!","(Info)", "#", "$", "%", "(", ")", "=", "?", "@", "^"
                                    ,"|","~","÷"};

            return sc;
        }

        public string NotesMedication(List<MedicationHx> MedicationHx, List<NotesComponent> objListNotesComponent)
        {
            var MedicationHxHTML = string.Empty;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            MedicationHxHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            MedicationHxHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Medications </b> </li>";
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Medications");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                MedicationHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            string CurrentMedication = string.Empty, PastMedication = string.Empty, DiscontinuedMedication = string.Empty, PrescribedMedication = string.Empty, ReconciledMedication = string.Empty, RenewedMedication = string.Empty, ReconciledCurrMedication = string.Empty;
            MedicationHxHTML += "</ul>";
            if ((MedicationHx == null || MedicationHx.Count() == 0) && string.IsNullOrWhiteSpace(upComment))
            {
                MedicationHxHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                MedicationHxHTML += "<li> No Active Medications </li>";
                MedicationHxHTML += "</ul>";
            }
            else if (MedicationHx != null || MedicationHx.Count() > 0)
            {
                string CurrDivId = "Section_CurrentMedications";
                foreach (NotesComponent obj in objListNotesComponent)
                {
                    document.LoadHtml(obj.SOAPText);
                    CurrentMedication += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                    try
                    {
                        int counterCurrMedication = 0;
                        foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//div[@id='" + CurrDivId + "']/div/div/section"))
                        {
                            var curDivId = div.Attributes["id"].Value;
                            curDivId = curDivId.Replace("s_Main", "_");
                            if (counterCurrMedication == 0)
                            {
                                CurrentMedication += "<li style='color: #0088cc;'><b>Current Medications </b> </li>";
                            }
                            try
                            {
                                foreach (HtmlAgilityPack.HtmlNode li in div.SelectNodes("//div[@id='" + curDivId + "']/ul/li"))
                                {
                                    CurrentMedication += "<li>" + li.InnerHtml + " </li>";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            counterCurrMedication = counterCurrMedication + 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        CurrentMedication += "<li> </li>";
                    }
                    CurrentMedication += "</ul>";
                }
                string PassDivId = "Section_PastMedications";
                foreach (NotesComponent obj in objListNotesComponent)
                {
                    document.LoadHtml(obj.SOAPText);
                    PastMedication += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                    try
                    {
                        int counterPassMedication = 0;
                        foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//div[@id='" + PassDivId + "']/div/div/section"))
                        {
                            string curDivId = div.Attributes["id"].Value;
                            curDivId = curDivId.Replace("s_Main", "_");
                            if (counterPassMedication == 0)
                            {
                                PastMedication += "<li style='color: #bd0e09;'><b>Past Medications </b> </li>";
                            }
                            try
                            {
                                foreach (HtmlAgilityPack.HtmlNode li in div.SelectNodes("//div[@id='" + curDivId + "']/ul/li"))
                                {
                                    PastMedication += "<li>" + li.InnerHtml + " </li>";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            counterPassMedication = counterPassMedication + 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        PastMedication += "<li> </li>";
                    }
                    PastMedication += "</ul>";
                }
                //------------------ Start EMR-6591 Reconciled View on Note
                string RecCurrDivId = "Section_MedicationCurrent";
                foreach (NotesComponent obj in objListNotesComponent)
                {
                    document.LoadHtml(obj.SOAPText);
                    ReconciledCurrMedication += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                    try
                    {
                        int counterRecCurrMedication = 0;
                        foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//div[@id='" + RecCurrDivId + "']/div/div/section"))
                        {
                            var curRecDivId = div.Attributes["id"].Value;
                            curRecDivId = curRecDivId.Replace("s_Main", "_");
                            if (counterRecCurrMedication == 0)
                            {
                                ReconciledCurrMedication += "<li style='color: #4B0082;'><b>Current Medications </b> </li>";
                            }
                            try
                            {
                                foreach (HtmlAgilityPack.HtmlNode li in div.SelectNodes(".//div[@id='" + curRecDivId + "']/ul/li"))
                                {
                                    ReconciledCurrMedication += "<li>" + li.InnerHtml + " </li>";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            counterRecCurrMedication = counterRecCurrMedication + 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        ReconciledCurrMedication += "<li> </li>";
                    }
                    ReconciledCurrMedication += "</ul>";
                }
                string DiscontinuedDivId = "Section_MedicationStop";
                foreach (NotesComponent obj in objListNotesComponent)
                {
                    document.LoadHtml(obj.SOAPText);
                    DiscontinuedMedication += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                    try
                    {
                        int counterDisMedication = 0;
                        foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//div[@id='" + DiscontinuedDivId + "']/div/div/section"))
                        {
                            var DisDivId = div.Attributes["id"].Value;
                            DisDivId = DisDivId.Replace("s_Main", "_");
                            if (counterDisMedication == 0)
                            {
                                DiscontinuedMedication += "<li style='color: #4B0082;'><b>Discontinued Medications </b> </li>";
                            }
                            try
                            {
                                foreach (HtmlAgilityPack.HtmlNode li in div.SelectNodes(".//div[@id='" + DisDivId + "']/ul/li"))
                                {
                                    DiscontinuedMedication += "<li>" + li.InnerHtml + " </li>";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            counterDisMedication = counterDisMedication + 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        DiscontinuedMedication += "<li> </li>";
                    }
                    DiscontinuedMedication += "</ul>";
                }
                string PrescribedDivId = "Section_MedicationPrescribed";
                foreach (NotesComponent obj in objListNotesComponent)
                {
                    document.LoadHtml(obj.SOAPText);
                    PrescribedMedication += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                    try
                    {
                        int counterPresMedication = 0;
                        foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//div[@id='" + PrescribedDivId + "']/div/div/section"))
                        {
                            var PresDivId = div.Attributes["id"].Value;
                            PresDivId = PresDivId.Replace("s_Main", "_");
                            if (counterPresMedication == 0)
                            {
                                PrescribedMedication += "<li style='color: #4B0082;'><b>Prescribed Medications </b> </li>";
                            }
                            try
                            {
                                foreach (HtmlAgilityPack.HtmlNode li in div.SelectNodes(".//div[@id='" + PresDivId + "']/ul/li"))
                                {
                                    PrescribedMedication += "<li>" + li.InnerHtml + " </li>";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            counterPresMedication = counterPresMedication + 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        PrescribedMedication += "<li> </li>";
                    }
                    PrescribedMedication += "</ul>";
                }
                string RenewedDivId = "Section_MedicationRenew";
                foreach (NotesComponent obj in objListNotesComponent)
                {
                    document.LoadHtml(obj.SOAPText);
                    RenewedMedication += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                    try
                    {
                        int counterRenewMedication = 0;
                        foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//div[@id='" + RenewedDivId + "']/div/div/section"))
                        {
                            var RenewDivId = div.Attributes["id"].Value;
                            RenewDivId = RenewDivId.Replace("s_Main", "_");
                            if (counterRenewMedication == 0)
                            {
                                RenewedMedication += "<li style='color: #4B0082;'><b>Renewed Medications </b> </li>";
                            }
                            try
                            {
                                foreach (HtmlAgilityPack.HtmlNode li in div.SelectNodes(".//div[@id='" + RenewDivId + "']/ul/li"))
                                {
                                    RenewedMedication += "<li>" + li.InnerHtml + " </li>";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            counterRenewMedication = counterRenewMedication + 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        RenewedMedication += "<li> </li>";
                    }
                    RenewedMedication += "</ul>";
                }
                string ReconciledDivId = "Section_MedicationReconciled";
                foreach (NotesComponent obj in objListNotesComponent)
                {
                    document.LoadHtml(obj.SOAPText);
                    ReconciledMedication += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                    try
                    {
                        int counterRecMedication = 0;
                        foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//div[@id='" + ReconciledDivId + "']/div/div/section"))
                        {
                            var RecDivId = div.Attributes["id"].Value;
                            RecDivId = RecDivId.Replace("s_Main", "_");
                            if (counterRecMedication == 0)
                            {
                                ReconciledMedication += "<li style='color: #4B0082;'><b>Reconciled Medications </b> </li>";
                            }
                            try
                            {
                                foreach (HtmlAgilityPack.HtmlNode li in div.SelectNodes(".//div[@id='" + RecDivId + "']/ul/li"))
                                {
                                    ReconciledMedication += "<li>" + li.InnerHtml + " </li>";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            counterRecMedication = counterRecMedication + 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        ReconciledMedication += "<li> </li>";
                    }
                    ReconciledMedication += "</ul>";
                }
                //------------------ End EMR-6591 Reconciled View on Note
            }
            string ReviewdByDivId = "Cli_Medications_ReviewByMedication";
            string ReviewedByHTML = string.Empty;
            foreach (NotesComponent obj in objListNotesComponent)
            {
                document.LoadHtml(obj.SOAPText);
                try
                {
                    ReviewedByHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                    foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//section[@id='" + ReviewdByDivId + "']"))
                    {
                        ReviewedByHTML += div.InnerHtml;
                    }
                    ReviewedByHTML += "</ul>";
                }
                catch (Exception ex)
                {
                    ReviewedByHTML = string.Empty;
                }
            }

            MedicationHxHTML += CurrentMedication;
            MedicationHxHTML += PastMedication;
            MedicationHxHTML += ReviewedByHTML;

            //------------------ Start EMR-6591 Reconciled View on Note
            MedicationHxHTML += ReconciledCurrMedication;
            MedicationHxHTML += DiscontinuedMedication;
            MedicationHxHTML += PrescribedMedication;
            MedicationHxHTML += RenewedMedication;
            MedicationHxHTML += ReconciledMedication;
            //------------------ End EMR-6591 Reconciled View on Note

            return MedicationHxHTML;
        }

        public string NotesCoSign(List<NotesComponent> objListNotesComponent)
        {
            string CoSignHTML = string.Empty;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            int counter = 0;
            string ulID = "coSignedByProvider";
            foreach (NotesComponent obj in objListNotesComponent)
            {
                document.LoadHtml(obj.SOAPText);
                try
                {
                    foreach (HtmlAgilityPack.HtmlNode li in document.DocumentNode.SelectNodes("//ul[@id='" + ulID + "']/li"))
                    {
                        if (li.Attributes["id"] != null)
                        {
                            if (li.Attributes["id"].Value == "coSignedBy")
                            {

                                CoSignHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                                CoSignHTML += "<li>" + li.InnerHtml + " </li>";
                                CoSignHTML += "</ul>";
                                if (li.Attributes["coSignedProviderId"] != null)
                                {
                                    var valProviderId = li.Attributes["coSignedProviderId"].Value;
                                    CommonSearch objCommonSearch = new CommonSearch();
                                    objCommonSearch.ProviderId = MDVUtility.ToInt64(valProviderId);
                                    List<MDVision.Model.Clinical.LegacyNotes.Provider> resultProviderData = BLLClinicalObj.ProviderDataSelect(objCommonSearch);
                                    if (resultProviderData != null && resultProviderData.Count() > 0)
                                    {
                                        MDVision.Model.Clinical.LegacyNotes.Provider objProvider = resultProviderData.FirstOrDefault(m => m.ProviderId == objCommonSearch.ProviderId);
                                        if (objProvider != null && objProvider.eSignature != null)
                                        {
                                            byte[] eSignature = objProvider.eSignature;
                                            CoSignHTML += "<div><img src='data:image/gif;base64," + Convert.ToBase64String(eSignature) + "' /></div>";
                                        }
                                    }
                                }
                                counter = counter + 1;
                            }
                        }
                        else
                        {
                            CoSignHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                            CoSignHTML += "<li>" + li.InnerHtml + " </li>";
                            CoSignHTML += "</ul>";
                            counter = counter + 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (counter == 0)
                        CoSignHTML += "<li>&nbsp; </li>";
                }
                CoSignHTML = CoSignHTML.Replace("<br>", "<br />");
            }
            return CoSignHTML;
        }

        public string NotesROS(List<NotesComponent> objListNotesComponent)
        {
            string ROSHTML = string.Empty;
            string OverAllComments = string.Empty;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            ROSHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            ROSHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Review of Systems </b> </li>";
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "ReviewofSystems");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                ROSHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            foreach (NotesComponent obj in objListNotesComponent)
            {
                document.LoadHtml(obj.SOAPText);
                try
                {
                    foreach (HtmlAgilityPack.HtmlNode li in document.DocumentNode.SelectNodes("//li/section/div/ul/li/div"))
                    {
                        string ROSSystemHTML = li.SelectNodes(".//a[@title='ROS System']")[0].InnerHtml;
                        string ROSSystemText = li.SelectNodes(".//a[@title='ROS System']")[0].InnerText.Split(':')[0];
                        ROSSystemHTML = ROSSystemHTML.Replace(": ", "<br/>");
                        ROSSystemHTML = ROSSystemHTML.Replace(ROSSystemText, ROSSystemText.ToUpper());
                        ROSSystemHTML = ROSSystemHTML.Replace("<strong>", "<strong class='underline' style='color:#51B451'>");
                        li.SelectNodes(".//a[@title='ROS System']")[0].InnerHtml = ROSSystemHTML;

                        ROSHTML += "<li>" + li.InnerHtml + " </li>";
                    }
                }
                catch (Exception ex)
                { }
                try
                {
                    var lis = document.DocumentNode.SelectNodes("//li/section/div/ul/li");
                    foreach (var li in lis)
                    {
                        var clone = li.Clone(); // to avoid modification of original html
                        foreach (var span in clone.SelectNodes("div"))
                            clone.RemoveChild(span);

                        OverAllComments = System.Text.RegularExpressions.Regex.Replace(clone.InnerText, @"\s+", " ");
                    }
                }
                catch (Exception ex)
                { }
            }
            ROSHTML += "</ul>";
            if (!string.IsNullOrWhiteSpace(OverAllComments))
            {
                ROSHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                ROSHTML += "<li>" + OverAllComments + " </li>";
                ROSHTML += "</ul>";
            }
            ROSHTML = ROSHTML.Replace("<br>", "<br />");
            return ROSHTML;
        }

        public string NotesAmendment(List<NotesComponent> objListNotesComponent)
        {
            var AmendmentHTML = string.Empty;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            AmendmentHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            foreach (NotesComponent obj in objListNotesComponent)
            {
                document.LoadHtml(obj.SOAPText);
                try
                {
                    foreach (HtmlAgilityPack.HtmlNode li in document.DocumentNode.SelectNodes("//ul/li"))
                    {
                        if (!string.IsNullOrWhiteSpace(li.InnerText))
                            AmendmentHTML += "<li>" + System.Web.HttpUtility.HtmlEncode(li.InnerText) + " </li>";
                    }
                }
                catch (Exception ex)
                {
                }
            }
            AmendmentHTML += "</ul>";
            AmendmentHTML = AmendmentHTML.Replace("<br>", "<br />");
            return AmendmentHTML;
        }

        public string NotesSignature(List<NotesComponent> objListNotesComponent, List<NoteHeaderData> objList_NoteHeaderData)
        {
            var SignatureHTML = string.Empty;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(objListNotesComponent.FirstOrDefault().SOAPText);
            try
            {
                foreach (HtmlAgilityPack.HtmlNode li in document.DocumentNode.SelectNodes("//img[@id='img_eSignature_ProgressNotes']"))
                {
                    string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                    System.Text.RegularExpressions.MatchCollection matchesImgSrc = System.Text.RegularExpressions.Regex.Matches(li.OuterHtml, regexImgSrc, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
                    foreach (System.Text.RegularExpressions.Match m in matchesImgSrc)
                    {
                        string src = m.Groups[1].Value;
                        if (src != "data:System.Byte[];base64,")
                            SignatureHTML += "<div><img src='data:image/gif;base64," + src.Split(',')[1] + "' /></div>";
                    }
                }

            }
            catch (Exception ex)
            {
                if (objList_NoteHeaderData.FirstOrDefault() != null && objList_NoteHeaderData.FirstOrDefault().eSignature != null)
                {
                    byte[] eSignature = objList_NoteHeaderData.FirstOrDefault().eSignature;
                    SignatureHTML += "<div><img src='data:image/gif;base64," + Convert.ToBase64String(eSignature) + "' /></div>";
                }
            }

            try
            {
                foreach (HtmlAgilityPack.HtmlNode li in document.DocumentNode.SelectNodes("//ul/li"))
                {
                    SignatureHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                    if (!string.IsNullOrWhiteSpace(li.InnerText))
                        SignatureHTML += "<li>" + System.Web.HttpUtility.HtmlEncode(li.InnerText) + " </li>";
                    else
                        SignatureHTML += "<li>" + li.InnerText + " </li>";
                    SignatureHTML += "</ul>";
                }
            }
            catch (Exception ex)
            {
            }

            return SignatureHTML;
        }

        public string NotesImages(long NoteID, List<string> ExcludedImages)
        {
            List<NoteDocumentModel> NoteDocs = new List<NoteDocumentModel>();
            string ExcludedImageIds = string.Empty;
            if (ExcludedImages != null)
                if (ExcludedImages.Count > 0)
                    ExcludedImageIds = String.Join(",", ExcludedImages);

            NoteDocs = BLLClinicalObj.loadNoteDocsForClassicView(NoteID, ExcludedImageIds);
            string ComponentDocsHtml = string.Empty;
            if (NoteDocs.Count > 0)
            {
                ComponentDocsHtml = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                ComponentDocsHtml += "<p style='color: #0088cc;'><b>Images </b><br/></p>";
                try
                {
                    foreach (var item in NoteDocs)
                    {
                        byte[] file;

                        string FilePath = WebConfigurationManager.AppSettings["PatientFilesPath"] + item.DocumentURL;  //HttpContext.Current.Server.MapPath("../content/images/SHS-nav-logo-small-100.png");
                        // LogoPath = LogoPath.Replace("\\api", "");
                        using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        ComponentDocsHtml += "<p><img src='data:image/png;base64," + Convert.ToBase64String(file) + "' style='width:175px;height:150px;' /><br/></p>";
                        // ComponentDocsHtml += "<div style='width:200px;height:80px;'><img src='data:image/png;base64," + Convert.ToBase64String(file) + "' style='width:200px;height:80px;' /></div>";

                    }
                    ComponentDocsHtml += "</ul>";
                }
                catch (Exception ex)
                {
                    MDVLogger.DALErrorLog("ClinicalNotesHelper::NotesImages", "ClassicNotesImage", ex);
                }
            }
            return ComponentDocsHtml;
        }

        public string NotesPrescription(List<Prescription> objListPrescription, List<NotesComponent> objListNotesComponent)
        {
            var PrescriptionHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Prescription");
            if ((objListPrescription != null && objListPrescription.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                PrescriptionHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                PrescriptionHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Prescription </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    PrescriptionHTML += "<li style='padding-top:2px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                if (objListPrescription != null && objListPrescription.Count() > 0)
                {
                    foreach (Prescription obj in objListPrescription)
                    {
                        PrescriptionHTML += "<li>";
                        if (!string.IsNullOrWhiteSpace(obj.DrugDescription))
                        {
                            PrescriptionHTML += "<b>" + System.Web.HttpUtility.HtmlEncode(obj.DrugDescription) + " </b>";
                        }
                        if (!string.IsNullOrWhiteSpace(obj.Action))
                        {
                            PrescriptionHTML += " " + obj.Action;
                        }
                        if (!string.IsNullOrWhiteSpace(obj.Dose))
                        {
                            PrescriptionHTML += " " + obj.Dose;
                        }
                        if (!string.IsNullOrWhiteSpace(obj.DoseUnit))
                        {
                            PrescriptionHTML += " " + obj.DoseUnit;
                        }
                        if (!string.IsNullOrWhiteSpace(obj.Routeby))
                        {
                            PrescriptionHTML += " " + obj.Routeby;
                        }
                        if (!string.IsNullOrWhiteSpace(obj.DoseTiming))
                        {
                            PrescriptionHTML += " " + obj.DoseTiming;
                        }
                        if (obj.Duration > 0)
                        {
                            PrescriptionHTML += " for " + obj.Duration + " Day(s)";
                        }
                        if (!string.IsNullOrWhiteSpace(obj.Quantity))
                        {
                            PrescriptionHTML += " Quantity " + obj.Quantity + " " + obj.DoseUnit + "(s)";
                        }
                        if (!string.IsNullOrWhiteSpace(obj.Quantity))
                        {
                            PrescriptionHTML += " Dispensed as written.";
                        }
                        PrescriptionHTML += " Prescribed on " + obj.CreatedDate.ToString("MM/dd/yyyy");

                        if (!string.IsNullOrWhiteSpace(obj.ProviderName))
                        {
                            PrescriptionHTML += " by " + obj.ProviderName;
                        }
                        PrescriptionHTML += " </li>";
                        string Comments = NotesCommentContents(objListNotesComponent, "Prescription_" + obj.PrescriptionID);
                        if (!string.IsNullOrWhiteSpace(Comments))
                        {
                            PrescriptionHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                        }
                    }
                }
                PrescriptionHTML += "</ul>";
            }
            return PrescriptionHTML;
        }

        public string NotesFollowUp(List<NotesComponent> objListNotesComponent)
        {
            var FollowUpHTML = string.Empty;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(objListNotesComponent.FirstOrDefault().SOAPText);
            FollowUpHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            FollowUpHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Follow Up </b> </li>";
            int counter = 0;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "FollowUp");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                FollowUpHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                counter = counter + 1;
            }
            try
            {
                foreach (HtmlAgilityPack.HtmlNode li in document.DocumentNode.SelectNodes("//section/div/ul/div/div"))
                {
                    string FollowupText = li.InnerText.Replace("\n", "");
                    string[] FollowUp = FollowupText.Split(new string[] { "Comments" }, StringSplitOptions.None);

                    FollowUpHTML += "<li><p>";
                    for (int FlwCount = 0; FlwCount < FollowUp.Length; FlwCount++)
                    {
                        if (FlwCount == 0)
                        {

                            FollowUpHTML += FollowUp[FlwCount];
                        }
                        else
                            FollowUpHTML += "<br/><b>Comments </b>" + FollowUp[FlwCount] + "";


                    }
                    counter = counter + 1;
                    FollowUpHTML += "</p> </li>";
                }
            }
            catch (Exception ex)
            {
            }

            FollowUpHTML += "</ul>";
            FollowUpHTML = FollowUpHTML.Replace("<br>", "<br />");
            if (counter == 0)
            {
                FollowUpHTML = string.Empty;
            }
            return FollowUpHTML;
        }

        public bool isTemplateNamePresent(List<NotesComponent> objListNotesComponent, string id)
        {
            string ComponentHtml = string.Empty;
            if (objListNotesComponent != null && objListNotesComponent.Count() > 0)
                ComponentHtml = objListNotesComponent.FirstOrDefault().SOAPText;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(ComponentHtml);
            bool content = false;
            string liId = "cli_physicalexam_" + id;
            try
            {
                HtmlNode node = document.DocumentNode.SelectSingleNode("//" + liId + "[@id='" + id + "']").SelectSingleNode("a");
                if (node != null)
                {
                    if (node.HasAttributes)
                    {
                        HtmlAttribute att = node.Attributes["style"];
                        if (att != null)
                        {
                            foreach (var item in att.Value.Split(';'))
                            {
                                var values = item.Split(':');
                                if (values.Length > 0)
                                {
                                    if (values[0].ToLower().Trim() == "display")
                                    {
                                        if (values.Length > 1)
                                        {
                                            if (values[1].ToLower().Trim() == "none")
                                                content = false;
                                            else
                                                content = true;
                                        }
                                        else
                                            content = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    content = false;
            }
            catch (Exception ex)
            {
                content = false;
            }
            return content;
        }
        public string NotesCustomForm(List<NotesComponent> objListNotesComponent, List<NotesComponents> objListNotesComponents)
        {
            var CustomFormHTML = string.Empty;
            CustomFormHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            if (objListNotesComponents.FirstOrDefault(x => x.Component == "CustomForms") != null)
            {
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                foreach (var objcusform in objListNotesComponent)
                {
                    string SOAPText = objcusform.SOAPText;
                    SOAPText = System.Text.RegularExpressions.Regex.Replace(SOAPText, "<input.*?>", String.Empty);
                    document.LoadHtml(SOAPText);
                    try
                    {
                        string CustomFormHTMLCustom = string.Empty;
                        foreach (HtmlAgilityPack.HtmlNode node in document.DocumentNode.SelectNodes("//li"))
                        {
                            if (node.ChildNodes.FirstOrDefault(x => x.OriginalName == "section") != null)
                            {
                                foreach (HtmlAgilityPack.HtmlNode div in node.ChildNodes)
                                {
                                    if (div.OriginalName == "header")
                                        CustomFormHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;padding-top: 5px !important;'><b>" + div.InnerText + " </b> </li>";
                                    else if (div.OriginalName == "section")
                                    {
                                        try
                                        {
                                            foreach (HtmlAgilityPack.HtmlNode li in div.ChildNodes[0].ChildNodes[0].ChildNodes)
                                            {
                                                if (li.OriginalName == "li" && li.OuterHtml != "")
                                                {
                                                    if (li.InnerHtml.Contains("</div>"))
                                                    {
                                                        string DivHtml = li.InnerHtml;
                                                        DivHtml = System.Text.RegularExpressions.Regex.Replace(DivHtml, "</div>", String.Empty);
                                                        DivHtml = System.Text.RegularExpressions.Regex.Replace(DivHtml, "<div.*?>", String.Empty);
                                                        DivHtml = DivHtml.Replace(@"</p>", "</li>");
                                                        DivHtml = DivHtml.Replace(@"<p>", "<br><li style='padding-top: 5px !important;'>");
                                                        li.InnerHtml = DivHtml;
                                                    }
                                                    CustomFormHTML += "<li style='padding-top: 5px !important;'>" + li.InnerHtml.Replace("&nbsp;", " ") + "</li>";
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {

                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CustomFormHTML += "<li>&nbsp; </li>";
                    }
                }
            }
            CustomFormHTML += "</ul>";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(CustomFormHTML);
            var imgs = doc.DocumentNode.SelectNodes("//img");
            if (imgs != null)
            {
                foreach (var item in imgs)
                {
                    item.SetAttributeValue("style", "width:410px !important; max-width:100%;height;");
                }

            }
            MemoryStream ms = new MemoryStream();
            System.Xml.XmlWriter xml = System.Xml.XmlWriter.Create(ms);
            doc.OptionOutputAsXml = true;
            doc.Save(xml);
            CustomFormHTML = doc.DocumentNode.OuterHtml;
            CustomFormHTML = CustomFormHTML.Replace("(Info)", " ");
            CustomFormHTML = CustomFormHTML.Replace("<br>", "<br />");
            return CustomFormHTML;

        }
        public string NotesCommentContentsUpper(List<NotesComponent> objListNotesComponent, string id)
        {
            // System.Diagnostics.Debugger.Launch();
            string ComponentHtml = string.Empty;
            if (objListNotesComponent != null && objListNotesComponent.Count() > 0)
            {
                ComponentHtml = objListNotesComponent.FirstOrDefault().SOAPText;
            }
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(ComponentHtml);
            string content = string.Empty;
            string liId = "Comments_" + id;
            try
            {
                HtmlAgilityPack.HtmlNodeCollection node = document.DocumentNode.SelectNodes("//li[@id='" + liId + "']/p");
                if (node != null)
                {
                    foreach (var Singleitem in node)
                    {

                        content = content + "<p>" + Singleitem.InnerHtml.ToString() + "</p><br />";
                    }
                }
                content = content.Replace("<br>", "<br />");
            }
            catch (Exception ex)
            {
            }
            return content;
        }

        public string NotesCommentContents(List<NotesComponent> objListNotesComponent, string id)
        {
            string ComponentHtml = string.Empty;
            if (objListNotesComponent != null && objListNotesComponent.Count() > 0)
            {
                ComponentHtml = objListNotesComponent.FirstOrDefault().SOAPText;
            }
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(ComponentHtml);
            string content = string.Empty;
            string liId = "Comments_Cli_" + id;
            try
            {
                foreach (HtmlAgilityPack.HtmlNode node in document.DocumentNode.SelectNodes("//li[@id='" + liId + "']/p"))
                {
                    try
                    {
                        if (node != null)
                        {
                            content += node.InnerHtml.ToString() + "<br />";
                        }
                    }
                    catch (Exception ex)
                    { }
                }

                // HtmlAgilityPack.HtmlNode node = document.DocumentNode.SelectSingleNode("//li[@id='" + liId + "']/p");
                //if (node != null)
                //{
                //    content = node.InnerHtml.ToString();
                //}
                content = content.Replace("<br>", "<br />");
            }
            catch (Exception ex)
            {
            }
            return content;
        }

        public string NotesComponentInnerHTML(string NotesComponent, string id)
        {
            string ComponentHtml = string.Empty;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(NotesComponent);
            string content = string.Empty;
            string liId = id;
            try
            {
                HtmlAgilityPack.HtmlNode node = document.DocumentNode.SelectSingleNode("//div[@id='" + liId + "']");
                if (node != null)
                {
                    content = node.InnerHtml.ToString();
                }
                content = content.Replace("<br>", "<br />");
            }
            catch (Exception ex)
            {
            }
            return content;
        }

        public string NotesCommentContentsRef(List<NotesComponent> objListNotesComponent, string id)
        {
            string ComponentHtml = string.Empty;
            if (objListNotesComponent != null && objListNotesComponent.Count() > 0)
            {
                ComponentHtml = objListNotesComponent.FirstOrDefault().SOAPText;
            }

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(ComponentHtml);
            string content = string.Empty;
            string liId = "Comments_" + id;
            try
            {
                HtmlAgilityPack.HtmlNode node = document.DocumentNode.SelectSingleNode("//li[@id='" + liId + "']/p");
                if (node != null)
                {
                    content = node.InnerHtml.ToString();
                }
                content = content.Replace("<br>", "<br />");
            }
            catch (Exception ex)
            {
            }
            return content;
        }

        public string NotesAppointmentReason(List<NotesComponent> objListNotesComponent, string NoteReason)
        {
            var AppointmentReasonHTML = string.Empty;
            AppointmentReasonHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            AppointmentReasonHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Appointment Reason </b> </li>";
            string liClass = "CustomComponent";
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            int counter = 0;
            foreach (NotesComponent obj in objListNotesComponent.OrderBy(m => m.OrderNo))
            {
                try
                {
                    //liClass = "CustomComponent";
                    string strReason = string.Empty;
                    document.LoadHtml(obj.SOAPText);
                    HtmlAgilityPack.HtmlNode node = document.DocumentNode.SelectSingleNode("//li[contains(@class, '" + liClass + "')]/ul/li/span");
                    if (node == null)
                    {
                        liClass = "appReason";
                        node = document.DocumentNode.SelectSingleNode("//li[contains(@class, '" + liClass + "')]/span");
                    }
                    if (node != null)
                    {
                        strReason = string.Empty;
                        strReason = node.InnerText;
                        if (!string.IsNullOrWhiteSpace(strReason) && (strReason.Trim() != "Appointment Reason:" && strReason.Trim() != "Appointment Reason"))
                        {
                            string innerHTML = node.InnerHtml;
                            innerHTML = innerHTML.Replace("Appointment Reason:", "");
                            innerHTML = innerHTML.Replace("Appointment Reason", "");
                            AppointmentReasonHTML += "<li>" + innerHTML + " </li>";
                            counter = counter + 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (counter == 0)
            {
                AppointmentReasonHTML += "<li>" + System.Web.HttpUtility.HtmlEncode(NoteReason) + " </li>";
            }
            AppointmentReasonHTML += "</ul>";
            AppointmentReasonHTML = AppointmentReasonHTML.Replace("<br>", "<br />");
            return AppointmentReasonHTML;
        }

        public string NotesAppointmentFreeText(List<NotesComponent> objListNotesComponent)
        {
            var FreeTextHTML = string.Empty;
            FreeTextHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            string liClass = "FreeTextComponent";
            int counter = 0;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            foreach (NotesComponent obj in objListNotesComponent.OrderBy(m => m.OrderNo))
            {
                try
                {
                    document.LoadHtml(obj.SOAPText);
                    HtmlAgilityPack.HtmlNode node = document.DocumentNode.SelectSingleNode("//li[contains(@class, '" + liClass + "')]/span");
                    if (node != null)
                    {
                        if (!string.IsNullOrWhiteSpace(node.InnerText))
                            FreeTextHTML += "<li>" + System.Web.HttpUtility.HtmlEncode(node.InnerText) + " </li>";
                        else
                            FreeTextHTML += "<li>" + node.InnerText + " </li>";
                        counter = 1;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (counter == 0)
            {
                FreeTextHTML += "<li>&nbsp; </li>";
            }
            FreeTextHTML += "</ul>";
            FreeTextHTML = FreeTextHTML.Replace("<br>", "<br />");
            return FreeTextHTML;
        }

        public string NotesCustomFreeText(List<NotesComponent> objListNotesComponent)
        {
            var FreeTextHTML = string.Empty;
            FreeTextHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            string liClass = "FreeTextComponent";
            int counter = 0;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            foreach (NotesComponent obj in objListNotesComponent.OrderBy(m => m.OrderNo))
            {
                try
                {
                    document.LoadHtml(obj.SOAPText);
                    HtmlAgilityPack.HtmlNode node = document.DocumentNode.SelectSingleNode("//li[contains(@class, '" + liClass + "')]");
                    if (node != null)
                    {
                        FreeTextHTML += "<li>" + node.InnerHtml + " </li>";
                        counter = 1;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (counter == 0)
            {
                FreeTextHTML += "<li>&nbsp; </li>";
            }
            FreeTextHTML += "</ul>";
            FreeTextHTML = FreeTextHTML.Replace("<br>", "<br />");
            return FreeTextHTML;
        }

        public string NotesCustomTagInsertedFreeText(List<NotesComponent> objListNotesComponent)
        {
            var FreeTextHTML = string.Empty;
            FreeTextHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            string liClass = "FreeTextComponent";
            int counter = 0;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            foreach (NotesComponent obj in objListNotesComponent.OrderBy(m => m.OrderNo))
            {
                try
                {
                    document.LoadHtml(obj.SOAPText);
                    HtmlAgilityPack.HtmlNode node = document.DocumentNode.SelectSingleNode("//li[contains(@class, '" + liClass + "')]");
                    if (node != null)
                    {
                        FreeTextHTML += "<li>" + node.InnerHtml + " </li>";
                        counter = 1;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (counter == 0)
            {
                FreeTextHTML += "<li>&nbsp; </li>";
            }
            FreeTextHTML += "</ul>";
            FreeTextHTML = FreeTextHTML.Replace("<br>", "<br />");
            return FreeTextHTML;
        }

        public string NoesESuperbill(List<eSuperbill> objListeSuperbill)
        {
            var ESuperbillHTML = string.Empty;
            ESuperbillHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            ESuperbillHTML += "<li style='color: #0088cc;'><b>eSuperbill </b> </li>";
            ESuperbillHTML += "<li><b>Visit Code </b> </li>";
            foreach (eSuperbill objeSuperbill in objListeSuperbill.Where(m => m.BillingInfoTimeId != "" && m.Type == "Procedure Codes"))
            {
                ESuperbillHTML += "<li>Patient underwent";
                ESuperbillHTML += " " + objeSuperbill.Units + " unit(s) of";
                ESuperbillHTML += " " + objeSuperbill.CPTDescription + " (" + objeSuperbill.CPTCode + ")";
                ESuperbillHTML += " from " + objeSuperbill.DOSFrom.ToString("MM/dd/yyyy");
                ESuperbillHTML += " to " + objeSuperbill.DOSTo.ToString("MM/dd/yyyy");
                ESuperbillHTML += " .</li>";
            }
            ESuperbillHTML += "<li><b>Procedure Codes </b> </li>";
            foreach (eSuperbill objeSuperbill in objListeSuperbill.Where(m => m.Type == "Procedure Codes" && (m.BillingInfoTimeId == null || m.BillingInfoTimeId == "")))
            {
                ESuperbillHTML += "<li>Patient underwent";
                ESuperbillHTML += " " + objeSuperbill.Units + " unit(s) of";
                ESuperbillHTML += " " + objeSuperbill.CPTDescription + " (" + objeSuperbill.CPTCode + ")";
                ESuperbillHTML += " from " + objeSuperbill.DOSFrom.ToString("MM/dd/yyyy");
                ESuperbillHTML += " to " + objeSuperbill.DOSTo.ToString("MM/dd/yyyy");
                ESuperbillHTML += " .</li>";
            }
            ESuperbillHTML += "<li><b>Diagnosis Codes </b> </li>";
            foreach (eSuperbill objeSuperbill in objListeSuperbill.Where(m => m.Type == "Diagnosis Codes").OrderByDescending(m => m.ProblemOrder))
            {
                ESuperbillHTML += "<li>";
                ESuperbillHTML += objeSuperbill.ICD10 + " - " + objeSuperbill.ICD10Description;
                if (!string.IsNullOrWhiteSpace(objeSuperbill.Severity))
                {
                    ESuperbillHTML += " (" + objeSuperbill.Severity + ")";
                }
                if (!string.IsNullOrWhiteSpace(objeSuperbill.ChronicityLevel))
                {
                    ESuperbillHTML += " " + objeSuperbill.ChronicityLevel + ".";
                }
                if (objeSuperbill.StartDate != null)
                {
                    ESuperbillHTML += " Start on " + Convert.ToDateTime(objeSuperbill.StartDate).ToString("MM/dd/yyyy");
                }
                if (objeSuperbill.EndDate != null)
                {
                    ESuperbillHTML += " and ended on " + Convert.ToDateTime(objeSuperbill.EndDate).ToString("MM/dd/yyyy") + ".";
                }
                if (objeSuperbill.ModifiedOn != null)
                {
                    ESuperbillHTML += " Modified on " + Convert.ToDateTime(objeSuperbill.ModifiedOn).ToString("MM/dd/yyyy") + ".";
                }
                ESuperbillHTML += " </li>";

            }
            ESuperbillHTML += "</ul>";

            return ESuperbillHTML;
        }

        public string NotesPhysicalExam(List<MDVision.Model.Clinical.LegacyNotes.PhysicalExam> objListPhysicalExam, List<NotesComponent> objListNotesComponent)
        {
            var PhysicalExamHTML = string.Empty;
            string upComment = "";// NotesCommentContentsUpper(objListNotesComponent, "PhysicalExam");
            if ((objListPhysicalExam != null && objListPhysicalExam.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                PhysicalExamHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                PhysicalExamHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Physical Exam </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    PhysicalExamHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                if (objListPhysicalExam != null && objListPhysicalExam.Count() > 0)
                {
                    foreach (MDVision.Model.Clinical.LegacyNotes.PhysicalExam obj in objListPhysicalExam.GroupBy(m => new { m.TemplateName }).Select(g => g.OrderBy(m => m.TemplateName).FirstOrDefault()))
                    {
                        if (isTemplateNamePresent(objListNotesComponent, obj.PETemplateId))
                        {
                            if (!string.IsNullOrWhiteSpace(obj.TemplateName))
                            {
                                PhysicalExamHTML += "<li><b style='color:#bd0e09;'>" + System.Web.HttpUtility.HtmlEncode(obj.TemplateName) + " </b> </li>";
                            }
                        }

                        foreach (MDVision.Model.Clinical.LegacyNotes.PhysicalExam objSys in objListPhysicalExam.Where(m => m.TemplateName == obj.TemplateName))
                        {
                            string sysName = String.IsNullOrWhiteSpace(objSys.SystemName) ? "" : System.Web.HttpUtility.HtmlEncode(objSys.SystemName);
                            string sysDescription = String.IsNullOrWhiteSpace(objSys.Description) ? "" : System.Web.HttpUtility.HtmlEncode(objSys.Description);
                            PhysicalExamHTML += "<li><b class='underline bold' style='color:#51b451;'>" + sysName.ToUpper() + " </b><br/> " + objSys.Description + " </li>";
                        }
                    }
                }
                PhysicalExamHTML += "</ul>";
            }
            return PhysicalExamHTML;

        }

        public string NotesPatientEducation(List<PatientEducation> objListPatientEducation, List<NotesComponent> objListNotesComponent)
        {
            var PatientEducationHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "PatientEducation");

            if ((objListPatientEducation != null && objListPatientEducation.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                PatientEducationHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                PatientEducationHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Patient Education </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    PatientEducationHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                if (objListPatientEducation != null && objListPatientEducation.Count() > 0)
                {
                    if (objListPatientEducation.Where(m => m.Type == "Info Education").Count() > 0)
                    {
                        PatientEducationHTML += "<li><b>Info Education Documents </b> </li>";
                        foreach (PatientEducation obj in objListPatientEducation.Where(m => m.Type == "Info Education"))
                        {
                            PatientEducationHTML += "<li>";
                            if (!string.IsNullOrWhiteSpace(obj.FilePath))
                                PatientEducationHTML += "  " + System.Web.HttpUtility.HtmlEncode(obj.FilePath);
                            PatientEducationHTML += " added on " + obj.CreatedOn.ToString("MM/dd/yyyy");
                            PatientEducationHTML += " </li>";
                            string Comments = NotesCommentContents(objListNotesComponent, "PatientEducation_" + obj.PatEducationId);
                            if (!string.IsNullOrWhiteSpace(Comments))
                            {
                                PatientEducationHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                            }
                        }
                    }
                    if (objListPatientEducation.Where(m => m.Type == "Non Info Education").Count() > 0)
                    {
                        PatientEducationHTML += "<li><b>Non-Info Education Documents </b> </li>";
                        foreach (PatientEducation obj in objListPatientEducation.Where(m => m.Type == "Non Info Education"))
                        {
                            PatientEducationHTML += "<li>";
                            if (!string.IsNullOrWhiteSpace(obj.FilePath))
                                PatientEducationHTML += "  " + System.Web.HttpUtility.HtmlEncode(obj.FilePath);
                            PatientEducationHTML += " added on " + obj.CreatedOn.ToString("MM/dd/yyyy");
                            PatientEducationHTML += " </li>";
                            string Comments = NotesCommentContents(objListNotesComponent, "PatientEducation_" + obj.PatEducationId);
                            if (!string.IsNullOrWhiteSpace(Comments))
                            {
                                PatientEducationHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                            }
                        }
                    }
                    PatientEducationHTML += "<li>Education was printed and given to patient. </li>";
                }
                PatientEducationHTML += "</ul>";
            }


            return PatientEducationHTML;
        }

        public string NotesFunctionalCognitive(List<FunctionalCognitive> objListFunctionalCognitive, List<NotesComponent> objListNotesComponent)
        {
            var FunctionalCognitiveHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "FunctionalAndCognitive");
            if ((objListFunctionalCognitive != null && objListFunctionalCognitive.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                FunctionalCognitiveHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                FunctionalCognitiveHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Functional And Cognitive </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    FunctionalCognitiveHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                if (objListFunctionalCognitive != null && objListFunctionalCognitive.Count() > 0)
                {
                    foreach (FunctionalCognitive obj in objListFunctionalCognitive.Where(m => m.Type == "Cognitive Status"))
                    {
                        string ICD10CodeDescription = string.IsNullOrWhiteSpace(obj.ICD10CodeDescription) ? "" : System.Web.HttpUtility.HtmlEncode(obj.ICD10CodeDescription);
                        FunctionalCognitiveHTML += "<li><b>Cognitive Status: </b> </li>";
                        FunctionalCognitiveHTML += "<li>Cognitive Status, " + ICD10CodeDescription + " </li>";
                        FunctionalCognitiveHTML += "<li>Note, " + obj.Instruction + " </li>";
                    }
                    foreach (FunctionalCognitive obj in objListFunctionalCognitive.Where(m => m.Type == "Functional Status"))
                    {
                        string ICD10CodeDescription = string.IsNullOrWhiteSpace(obj.ICD10CodeDescription) ? "" : System.Web.HttpUtility.HtmlEncode(obj.ICD10CodeDescription);
                        FunctionalCognitiveHTML += "<li><b>Functional Status: </b> </li>";
                        FunctionalCognitiveHTML += "<li>Functional Status, " + ICD10CodeDescription + " </li>";
                        FunctionalCognitiveHTML += "<li>Note, " + obj.Instruction + " </li>";
                    }
                    foreach (FunctionalCognitive obj in objListFunctionalCognitive.Where(m => m.Type == "Mental Status"))
                    {
                        string ICD10CodeDescription = string.IsNullOrWhiteSpace(obj.ICD10CodeDescription) ? "" : System.Web.HttpUtility.HtmlEncode(obj.ICD10CodeDescription);
                        FunctionalCognitiveHTML += "<li><b>Mental Status: </b> </li>";
                        FunctionalCognitiveHTML += "<li>Mental Status, " + ICD10CodeDescription + " </li>";
                        FunctionalCognitiveHTML += "<li>Note, " + obj.Instruction + " </li>";
                    }

                    string Comments = NotesCommentContents(objListNotesComponent, "Cognitive_" + objListFunctionalCognitive.FirstOrDefault().CognitiveId);
                    if (!string.IsNullOrWhiteSpace(Comments))
                    {
                        FunctionalCognitiveHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                    }
                }
                FunctionalCognitiveHTML += "</ul>";
            }

            return FunctionalCognitiveHTML;
        }

        public string NotesFollowUp(List<MDVision.Model.Clinical.LegacyNotes.FollowUp> objListFollowUp)
        {
            var FollowUpHTML = string.Empty;
            FollowUpHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            FollowUpHTML += "<li style='color: #0088cc;'><b>Follow Up </b> </li>";
            foreach (MDVision.Model.Clinical.LegacyNotes.FollowUp obj in objListFollowUp)
            {
                FollowUpHTML += obj.SOAPText;
            }
            FollowUpHTML += "</ul>";
            return FollowUpHTML;
        }

        public string NotesConsultationOrder(List<ConsultationOrder> objListConsultationOrder, List<NotesComponent> objListNotesComponent)
        {
            var ConsultationOrderHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "ConsultationOrder");
            if ((objListConsultationOrder != null && objListConsultationOrder.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                ConsultationOrderHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                ConsultationOrderHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Consultation Order </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    ConsultationOrderHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                if (objListConsultationOrder != null && objListConsultationOrder.Count() > 0)
                {
                    foreach (ConsultationOrder objConsultationOrder in objListConsultationOrder.Where(m => m.Type == "Problem"))
                    {
                        ConsultationOrderHTML += "<li>" + System.Web.HttpUtility.HtmlEncode(objConsultationOrder.ProblemName) + " </li>";
                    }
                    foreach (ConsultationOrder objConsultationOrder in objListConsultationOrder.Where(m => m.Type == "Test"))
                    {
                        ConsultationOrderHTML += "<li>" + System.Web.HttpUtility.HtmlEncode(objConsultationOrder.CPTCodeDescription);
                        if (string.IsNullOrWhiteSpace(objConsultationOrder.Urgency))
                        {
                            ConsultationOrderHTML += " , Urgency: " + objConsultationOrder.Urgency;
                        }
                        ConsultationOrderHTML += " </li>";
                    }
                    ConsultationOrder obj = objListConsultationOrder.FirstOrDefault();
                    if (obj != null)
                    {
                        ConsultationOrderHTML += "<li>Added On " + obj.CreatedOn.ToString("MM/dd/yyyy") + " </li>";
                    }
                    string Comments = NotesCommentContents(objListNotesComponent, "ConsultationOrderDetail_" + obj.ConsultationOrderId);
                    if (!string.IsNullOrWhiteSpace(Comments))
                    {
                        ConsultationOrderHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                    }
                }
                ConsultationOrderHTML += "</ul>";
            }
            return ConsultationOrderHTML;
        }

        public string NotesProcedureOrder(List<ProcedureOrder> objListProcedureOrder, List<NotesComponent> objListNotesComponent)
        {
            var ProcedureOrderHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "ProcedureOrder");
            if ((objListProcedureOrder != null && objListProcedureOrder.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                ProcedureOrderHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                ProcedureOrderHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Procedure Order </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    ProcedureOrderHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                if (objListProcedureOrder != null && objListProcedureOrder.Count() > 0)
                {
                    foreach (ProcedureOrder objProcedureOrder in objListProcedureOrder.GroupBy(m => new { m.ProcedureOrderId }).Select(g => g.OrderBy(m => m.ProcedureOrderId).FirstOrDefault()))
                    {
                        if (objListNotesComponent != null && objListNotesComponent.Count() > 0)
                        {
                            string strSoapText = NotesProcedureOrderTemplateSoapContents(objListNotesComponent, objProcedureOrder.ProcedureOrderId.ToString());
                            if (!string.IsNullOrWhiteSpace(strSoapText))
                            {
                                ProcedureOrderHTML += "<li>";
                                ProcedureOrderHTML += strSoapText;
                                ProcedureOrderHTML += " </li>";
                            }
                        }
                        //foreach (ProcedureOrder objProcedureOrderInner in objListProcedureOrder.Where(m => m.ProcedureOrderId == objProcedureOrder.ProcedureOrderId))
                        //{
                        //    ProcedureOrderHTML += "<li><b>" + objProcedureOrderInner.CPTCodeDescription + " </b>";
                        //    if (!string.IsNullOrWhiteSpace(objProcedureOrderInner.Urgency))
                        //    {
                        //        ProcedureOrderHTML += " , Urgency: " + objProcedureOrderInner.Urgency;
                        //    }
                        //    ProcedureOrderHTML += " </li>";
                        //    if (!string.IsNullOrWhiteSpace(objProcedureOrderInner.SoapText))
                        //    {
                        //        string strSoapText = NotesOrderTestContents(objProcedureOrderInner.SoapText);
                        //        if (!string.IsNullOrWhiteSpace(strSoapText))
                        //        {
                        //            ProcedureOrderHTML += "<li><ul><li>";
                        //            ProcedureOrderHTML += strSoapText;
                        //            ProcedureOrderHTML += " </li></ul> </li>";
                        //        }
                        //    }
                        //}
                        //ProcedureOrderHTML += "<li style='margin-bottom:10px;'>Added On: " + objProcedureOrder.OrderDate.ToString("MM/dd/yyyy") + " </li>";
                        //string Comments = NotesCommentContents(objListNotesComponent, "ProcedureOrderDetail_" + objProcedureOrder.ProcedureOrderId);
                        //if (!string.IsNullOrWhiteSpace(Comments))
                        //{
                        //    ProcedureOrderHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                        //}
                    }
                }
                ProcedureOrderHTML += "</ul>";
            }
            return ProcedureOrderHTML;
        }

        public string NotesProcedureOrderTemplateSoapContents(List<NotesComponent> objListNotesComponent, string id)
        {
            string content = string.Empty;
            if (!string.IsNullOrWhiteSpace(objListNotesComponent.FirstOrDefault().SOAPText))
            {
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(objListNotesComponent.FirstOrDefault().SOAPText);
                string liId = "Cli_ProcedureOrderDetail_" + id;
                try
                {
                    foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//li/section/div[@id='" + liId + "']/ul/li"))
                    {
                        try
                        {
                            content += div.InnerHtml + "<br />";
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                catch (Exception ex)
                { }
                if (!string.IsNullOrWhiteSpace(content))
                    content = content.Replace("<br>", "<br />");
            }
            return content;
        }

        public string getNotesReferralVisitName(long visitId)
        {
            string typeName = string.Empty;
            switch (visitId)
            {
                case 1:
                    typeName = "Follow Up";
                    break;
                case 2:
                    typeName = "New Patient";
                    break;
                case 3:
                    typeName = "Physical";
                    break;
                case 4:
                    typeName = "Surgical Clearance";
                    break;
                case 5:
                    typeName = "Consult";
                    break;
                case 6:
                    typeName = "Office Visit";
                    break;
                case 7:
                    typeName = "Immunization";
                    break;
                case 8:
                    typeName = "Injection";
                    break;
                case 9:
                    typeName = "Bloodwork";
                    break;
                case 10:
                    typeName = "Ultrasound";
                    break;
                case 11:
                    typeName = "Procedure";
                    break;
                case 12:
                    typeName = "Surgery";
                    break;
                case 13:
                    typeName = "Infusion";
                    break;
                case 14:
                    typeName = "Test";
                    break;
                default:
                    break;
            }

            return typeName;
        }

        public string NotesReferrals(List<Referrals> objListReferrals, List<NotesComponent> objListNotesComponent)
        {
            var ReferralsHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Referrals");
            if ((objListReferrals != null && objListReferrals.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                ReferralsHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                ReferralsHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;padding-left: 0px !important;margin-left: 0px !important;'><b>Referrals </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    ReferralsHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                ReferralsHTML += "</ul>";
                if (objListReferrals != null && objListReferrals.Count() > 0)
                {
                    ReferralsHTML += "<table style='padding-left:8px;padding-right:8px;width:100%;word-wrap: break-word;font-size:12px;'>";
                    foreach (Referrals objReferrals in objListReferrals.Where(m => m.Value == "Referral"))
                    {
                        ReferralsHTML += "<tr><td style='width:20%;padding-left:20px;padding-top:5px;padding-bottom:5px;'><b>Date: </b></td>";
                        ReferralsHTML += "<td style='width:80%;padding:5px;' colspan='3'>" + objReferrals.RefDate.ToString("MM/dd/yyyy") + " " + DateTime.Today.Add(objReferrals.RefTime).ToString("hh:mm tt") + "</td></tr>";

                        ReferralsHTML += "<tr><td style='width:20%;padding-left:20px;padding-top:5px;padding-bottom:5px;'><b>Category: </b></td>";
                        ReferralsHTML += "<td style='width:80%;padding:5px;' colspan='3'>" + objReferrals.TYPE + "</td></tr>";

                        ReferralsHTML += "<tr><td style='width:20%;padding-left:20px;padding-top:5px;padding-bottom:5px;'><b>Type: </b></td>";
                        ReferralsHTML += "<td style='width:30%;padding:5px;'>" + getNotesReferralVisitName(objReferrals.VisitTypeId) + "</td>";
                        ReferralsHTML += "<td style='width:30%;padding:5px;'><b>Referral To Speciality: </b></td>";
                        ReferralsHTML += "<td style='width:20%;padding:5px;'>" + objReferrals.ToSpecialty + "</td></tr>";

                        ReferralsHTML += "<tr><td style='width:20%;padding-left:20px;padding-top:5px;padding-bottom:5px;'><b>Referral To: </b></td>";
                        ReferralsHTML += "<td style='width:30%;padding:5px;'>" + objReferrals.ReferralTo + "</td>";
                        ReferralsHTML += "<td style='width:30%;padding:5px;'><b>Referral From: </b></td>";
                        ReferralsHTML += "<td style='width:20%;padding:5px;'>" + objReferrals.ReferralFrom + "</td></tr>";

                        ReferralsHTML += "<tr><td style='width:20%;padding-left:20px;padding-top:5px;padding-bottom:5px;'><b>Facility To: </b></td>";
                        ReferralsHTML += "<td style='width:30%;padding:5px;'>" + objReferrals.FacilityTo + "</td>";
                        ReferralsHTML += "<td style='width:30%;padding:5px;'><b>Facility From: </b></td>";
                        ReferralsHTML += "<td style='width:20%;padding:5px;'>" + objReferrals.FacilityFrom + "</td></tr>";

                        if (objListReferrals.FirstOrDefault(m => m.Value == "Procedure" && m.ReferralId == objReferrals.ReferralId) != null)
                        {
                            ReferralsHTML += "<tr><td style='width:20%;vertical-align:top;padding-left:20px;padding-top:5px;padding-bottom:5px;'><b>Procedure(s): </b></td>";
                            ReferralsHTML += "<td style='width:80%;padding:5px;vertical-align:top;' colspan='3'>";

                            foreach (Referrals objReferralsP in objListReferrals.Where(m => m.Value == "Procedure" && m.ReferralId == objReferrals.ReferralId))
                            {
                                if (!string.IsNullOrWhiteSpace(objReferralsP.CPTCodeDescription))
                                    ReferralsHTML += "<p>" + System.Web.HttpUtility.HtmlEncode(objReferralsP.CPTCodeDescription) + "</p>";
                            }
                            ReferralsHTML += "</td></tr>";
                        }
                        if (objListReferrals.FirstOrDefault(m => m.Value == "Problem" && m.ReferralId == objReferrals.ReferralId) != null)
                        {
                            ReferralsHTML += "<tr><td style='width:20%;vertical-align:top;padding-left:20px;padding-top:5px;padding-bottom:5px;'><b>Problem(s): </b></td>";
                            ReferralsHTML += "<td style='width:80%;padding:5px;vertical-align:top;' colspan='3'>";

                            foreach (Referrals objReferralsP in objListReferrals.Where(m => m.Value == "Problem" && m.ReferralId == objReferrals.ReferralId))
                            {
                                if (!string.IsNullOrWhiteSpace(objReferralsP.ProblemName))
                                    ReferralsHTML += "<p>" + System.Web.HttpUtility.HtmlEncode(objReferralsP.ProblemName) + "</p>";
                            }
                            ReferralsHTML += "</td></tr>";
                        }
                        if (!string.IsNullOrWhiteSpace(objReferrals.Comments))
                        {
                            ReferralsHTML += "<tr><td style='width:20%;vertical-align:top;padding-left:20px;padding-top:8px;padding-bottom:8px;'><b>Comments: </b></td>";
                            if (!string.IsNullOrWhiteSpace(objReferrals.Comments))
                                ReferralsHTML += "<td style='width:80%;vertical-align:top;padding-top:8px;padding-bottom:8px;' colspan='3'>" + System.Web.HttpUtility.HtmlEncode(objReferrals.Comments) + "</td>";
                            else
                                ReferralsHTML += "<td style='width:80%;vertical-align:top;padding-top:8px;padding-bottom:8px;' colspan='3'>" + objReferrals.Comments + "</td>";
                            ReferralsHTML += "</tr>";
                        }
                        string Comments = NotesCommentContentsRef(objListNotesComponent, "Patient_Referrals_" + objReferrals.ReferralId);
                        if (!string.IsNullOrWhiteSpace(Comments))
                        {
                            ReferralsHTML += "<tr><td style='padding-left:20px;padding-top:8px;padding-bottom:8px;' colspan='4'>" + Comments + "</td></tr>";
                        }
                    }
                    ReferralsHTML += "</table>";
                }
            }
            return ReferralsHTML;

        }

        public string NotesProcedures(List<Procedure> objList_Procedure, List<NotesComponent> objListNotesComponent)
        {
            var ProceduresHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Procedures");
            if ((objList_Procedure != null && objList_Procedure.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                ProceduresHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                ProceduresHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Procedures </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    ProceduresHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                if (objList_Procedure != null && objList_Procedure.Count() > 0)
                {
                    foreach (Procedure objProcedure in objList_Procedure)
                    {
                        ProceduresHTML += "<li><b>" + System.Web.HttpUtility.HtmlEncode(objProcedure.CPTCode) + " - </b>";
                        string ProcedureHTML = string.Empty;
                        if (!string.IsNullOrWhiteSpace(objProcedure.CPTDescription))
                        {
                            ProcedureHTML += "<b> " + System.Web.HttpUtility.HtmlEncode(objProcedure.CPTDescription) + " </b>";
                        }
                        //  if (!string.IsNullOrWhiteSpace(objProcedure.CPTCode))
                        //  {
                        //      ProcedureHTML += " (" + objProcedure.CPTCode + ")";
                        //  }
                        if (!string.IsNullOrWhiteSpace(objProcedure.ICD10))
                        {
                            ProcedureHTML += "  based on the following assessment: " + System.Web.HttpUtility.HtmlEncode(objProcedure.ICD10);
                        }
                        if (!string.IsNullOrWhiteSpace(objProcedure.ICD10Description))
                        {
                            ProcedureHTML += " " + System.Web.HttpUtility.HtmlEncode(objProcedure.ICD10Description);
                        }
                        //ProcedureHTML = System.Web.HttpUtility.HtmlEncode(ProcedureHTML);
                        if (!objProcedure.IsActive)
                        {
                            ProcedureHTML += " <span style='color:red'> (Inactive) </span>";
                        }
                        if (!string.IsNullOrWhiteSpace(objProcedure.Comments))
                        {
                            ProcedureHTML += "<br/>Comments: " + System.Web.HttpUtility.HtmlEncode(objProcedure.Comments);
                        }
                        //if (objProcedure.StartDate != null && objProcedure.EndDate != null)
                        //{
                        //    if (Convert.ToDateTime(objProcedure.StartDate).Date.Equals(Convert.ToDateTime(objProcedure.EndDate).Date))
                        //    {
                        //        ProcedureHTML += " on " + Convert.ToDateTime(objProcedure.StartDate).ToString("MM/dd/yyyy") + ".";
                        //    }
                        //    else
                        //    {
                        //        ProceduresHTML += " from " + Convert.ToDateTime(objProcedure.StartDate).ToString("MM/dd/yyyy") + " to " + Convert.ToDateTime(objProcedure.EndDate).ToString("MM/dd/yyyy") + ".";
                        //    }
                        //}
                        //else if (objProcedure.StartDate != null || objProcedure.EndDate != null)
                        //{
                        //    if (objProcedure.StartDate != null)
                        //    {
                        //        ProcedureHTML += " on " + Convert.ToDateTime(objProcedure.StartDate).ToString("MM/dd/yyyy") + ".";
                        //    }
                        //    else if (objProcedure.EndDate != null)
                        //    {
                        //        ProcedureHTML += " on " + Convert.ToDateTime(objProcedure.EndDate).ToString("MM/dd/yyyy") + ".";
                        //    }
                        //}
                        ProceduresHTML += ProcedureHTML;
                        ProceduresHTML += " </li>";
                        if (objListNotesComponent != null && objListNotesComponent.Count() > 0)
                        {
                            string strSoapText = NotesProceduresTemplateSoapContents(objListNotesComponent, objProcedure.ProcedureId.ToString());
                            if (!string.IsNullOrWhiteSpace(strSoapText))
                            {
                                ProceduresHTML += "<li>";
                                ProceduresHTML += strSoapText;
                                ProceduresHTML += " </li>";
                            }
                        }
                        string Comments = NotesCommentContents(objListNotesComponent, "Procedures_" + objProcedure.ProcedureId);
                        if (!string.IsNullOrWhiteSpace(Comments))
                        {
                            ProceduresHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                        }
                    }
                }
                ProceduresHTML += "</ul>";
            }
            return ProceduresHTML;

        }

        public string NotesProceduresTemplateSoapContents(List<NotesComponent> objListNotesComponent, string id)
        {
            string content = string.Empty;
            if (!string.IsNullOrWhiteSpace(objListNotesComponent.FirstOrDefault().SOAPText))
            {
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(objListNotesComponent.FirstOrDefault().SOAPText);
                string liId = "Cli_Procedures_" + id;
                try
                {
                    foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//li//section//div[@id='" + liId + "']//ul/section"))
                    {
                        try
                        {
                            content += div.InnerHtml + "<br />";
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                catch (Exception ex)
                { }
                if (!string.IsNullOrWhiteSpace(content))
                    content = content.Replace("<br>", "<br />");
            }
            return content;
        }

        public string NotesRadOrderResult(List<RadOrderResult> objList_RadOrderResult, List<NotesComponent> objListNotesComponent)
        {
            var RadOrderResultHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "DiagnosticImagingResults");
            if ((objList_RadOrderResult != null && objList_RadOrderResult.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                RadOrderResultHTML += "<table style='width:100%;word-wrap: break-word;font-size:12px;'>";
                RadOrderResultHTML += "<tr><th class='font-xs bold text-primary' colspan='6' style='padding-left:3px;padding-top:5px;padding-bottom:10px;color: #0088cc;'><b>Diagnostic Imaging Results</b></th></tr>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    RadOrderResultHTML += "<tr><td colspan='6' style='padding-left:3px;padding-top:5px;padding-bottom:10px;'>" + upComment + "</td></tr>";
                }
                if (objList_RadOrderResult != null && objList_RadOrderResult.Count() > 0)
                {
                    foreach (RadOrderResult objRadOrderResultOuter in objList_RadOrderResult.GroupBy(m => new { m.RadiologyOrderResultId }).Select(g => g.OrderBy(m => m.RadiologyOrderResultId).FirstOrDefault()))
                    {
                        foreach (RadOrderResult objRadOrderResult in objList_RadOrderResult.Where(m => m.RadiologyOrderResultId == objRadOrderResultOuter.RadiologyOrderResultId).GroupBy(m => new { m.CPTCode }).Select(g => g.OrderBy(m => m.CPTCode).FirstOrDefault()))
                        {
                            RadOrderResultHTML += "<tr style='background: #468cec;color: #FFFFFF;'><th colspan='6' style='padding:5px;border: 1px solid #ddd;'>" + System.Web.HttpUtility.HtmlEncode(objRadOrderResult.CPTCode) + " " + System.Web.HttpUtility.HtmlEncode(objRadOrderResult.CPTCodeDescription) + "</th></tr>";
                            RadOrderResultHTML += "<tr style='background-color:#0188CC;color: #FFFFFF;'><th style='padding:3px;border: 1px solid #ddd;'>Date & Time</th><th style='padding:3px;border: 1px solid #ddd;'>Observation</th><th style='padding:3px;border: 1px solid #ddd;'>Result</th><th style='padding:3px;border: 1px solid #ddd;'>UoM</th><th style='padding:3px;border: 1px solid #ddd;'>Flag</th><th style='padding:3px;border: 1px solid #ddd;'>Range</th></tr>";
                            foreach (RadOrderResult objResult in objList_RadOrderResult.Where(m => m.CPTCode == objRadOrderResult.CPTCode && m.RadiologyOrderResultId == objRadOrderResult.RadiologyOrderResultId))
                            {
                                RadOrderResultHTML += "<tr><td style='padding:3px;border-left: 1px solid #ddd;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>" + objResult.ObservationDate.ToString("MM/dd/yyyy hh:mm tt") + "</td>";
                                RadOrderResultHTML += "<td style='padding:3px;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>" + objResult.LOINCDescription + "</td>";
                                RadOrderResultHTML += "<td style='padding:3px;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>" + objResult.Result + "</td>";
                                RadOrderResultHTML += "<td style='padding:3px;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>" + objResult.UoM + "</td>";
                                RadOrderResultHTML += "<td style='padding:3px;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>" + objResult.Flag + "</td>";
                                if (!string.IsNullOrWhiteSpace(objResult.Range))
                                {
                                    RadOrderResultHTML += "<td style='padding:3px;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;clear: both;'>" + System.Web.HttpUtility.HtmlEncode(objResult.Range) + "</td>";
                                }
                                else
                                {
                                    RadOrderResultHTML += "<td style='padding:3px;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;clear: both;'>&nbsp;</td>";
                                }
                                RadOrderResultHTML += "</tr>";
                            }
                        }
                        if (!string.IsNullOrEmpty(objRadOrderResultOuter.Status))
                            RadOrderResultHTML += "<tr><td colspan='6' style='padding-left:3px;padding-top:10px;padding-bottom:5px;'><b>Status: </b>" + System.Web.HttpUtility.HtmlEncode(objRadOrderResultOuter.Status) + "</td></tr>";
                        if (!string.IsNullOrEmpty(objRadOrderResultOuter.Remarks))
                            RadOrderResultHTML += "<tr><td colspan='6' style='padding-left:3px;padding-top:5px;padding-bottom:5px;'><b>Remarks: </b>" + System.Web.HttpUtility.HtmlEncode(objRadOrderResultOuter.Remarks) + "</td></tr>";
                        if (!string.IsNullOrEmpty(objRadOrderResultOuter.Comments))
                        {
                            objRadOrderResultOuter.Comments = objRadOrderResultOuter.Comments.Replace("<br>", "<br />");
                            objRadOrderResultOuter.Comments = objRadOrderResultOuter.Comments.Replace("</div><div>", "<br />");
                            objRadOrderResultOuter.Comments = objRadOrderResultOuter.Comments.Replace("<div>", "<br />");
                            objRadOrderResultOuter.Comments = objRadOrderResultOuter.Comments.Replace("</div>", "<br />");
                            RadOrderResultHTML += "<tr><td colspan='6' style='padding-left:3px;padding-top:5px;padding-bottom:5px;'><b>Comments: </b>" + objRadOrderResultOuter.Comments + "</td></tr>";
                        }
                        string Comments = NotesCommentContents(objListNotesComponent, "RadiologyResultDetail_" + objRadOrderResultOuter.RadiologyOrderResultId);
                        if (!string.IsNullOrWhiteSpace(Comments))
                        {
                            RadOrderResultHTML += "<tr><td colspan='6' style='padding-left:3px;padding-top:5px;padding-bottom:7px;'>" + Comments + "</td></tr>";
                        }
                    }


                }
                RadOrderResultHTML += "</table>";
            }
            return RadOrderResultHTML;
        }

        public string NotesRadOrder(List<RadOrder> objList_RadOrder, List<NotesComponent> objListNotesComponent)
        {
            var RadOrderHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "DiagnosticImagingOrder");
            if ((objList_RadOrder != null && objList_RadOrder.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                RadOrderHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                RadOrderHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Diagnostic Imaging Order </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    RadOrderHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                if (objList_RadOrder != null && objList_RadOrder.Count() > 0)
                {
                    foreach (RadOrder objRadOrder in objList_RadOrder.Where(m => m.Type == "Tests").GroupBy(m => new { m.RadiologyOrderId }).Select(g => g.OrderBy(m => m.RadiologyOrderId).FirstOrDefault()))
                    {
                        if (objListNotesComponent != null && objListNotesComponent.Count() > 0)
                        {
                            string strSoapText = NotesRadiologyOrderTemplateSoapContents(objListNotesComponent, objRadOrder.RadiologyOrderId.ToString());
                            if (!string.IsNullOrWhiteSpace(strSoapText))
                            {
                                RadOrderHTML += "<li>";
                                RadOrderHTML += strSoapText;
                                RadOrderHTML += " </li>";
                            }
                        }
                        //RadOrderHTML += "<li><b>" + objRadOrder.CPTCode + " " + objRadOrder.CPTCodeDescription + " </b>,";
                        //if (!string.IsNullOrWhiteSpace(objRadOrder.Urgency))
                        //{
                        //    RadOrderHTML += " Urgency: " + objRadOrder.Urgency;
                        //}
                        //RadOrderHTML += " </li>";
                        //if (!string.IsNullOrWhiteSpace(objRadOrder.SoapText))
                        //{
                        //    string strSoapText = NotesOrderTestContents(objRadOrder.SoapText);
                        //    if (!string.IsNullOrWhiteSpace(strSoapText))
                        //    {
                        //        RadOrderHTML += "<li style='margin-left:3px !important;padding-left:3px !important;'><ul style='padding-left:3px !important;margin-left:3px !important;'><li>";
                        //        RadOrderHTML += strSoapText;
                        //        RadOrderHTML += " </li></ul> </li>";
                        //    }
                        //}
                        //if (objList_RadOrder.Where(m => m.RadiologyOrderId == objRadOrder.RadiologyOrderId).FirstOrDefault(m => m.Type == "Problem List") != null)
                        //{
                        //    RadOrderHTML += "<li>Associated Problem(s) </li>";
                        //}
                        //foreach (RadOrder objRadOrderInner in objList_RadOrder.Where(m => m.Type == "Problem List" && m.RadiologyOrderId == objRadOrder.RadiologyOrderId).OrderByDescending(m => m.SortOrder))
                        //{
                        //    RadOrderHTML += "<li><b>" + objRadOrderInner.ICD10 + " " + objRadOrderInner.ICD10Description + " </b> </li>";
                        //}
                        //RadOrder obj = objList_RadOrder.Where(m => m.RadiologyOrderId == objRadOrder.RadiologyOrderId).FirstOrDefault();
                        //if (obj != null)
                        //{
                        //    RadOrderHTML += "<li>Last Updated On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                        //    string Comments = NotesCommentContents(objListNotesComponent, "RadiologyOrderDetail_" + obj.RadiologyOrderId);
                        //    if (!string.IsNullOrWhiteSpace(Comments))
                        //    {
                        //        RadOrderHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                        //    }
                        //}
                    }
                }
                RadOrderHTML += "</ul>";
            }
            return RadOrderHTML;
        }

        public string NotesRadiologyOrderTemplateSoapContents(List<NotesComponent> objListNotesComponent, string id)
        {
            string content = string.Empty;
            if (!string.IsNullOrWhiteSpace(objListNotesComponent.FirstOrDefault().SOAPText))
            {
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(objListNotesComponent.FirstOrDefault().SOAPText);
                string liId = "Cli_RadiologyOrderDetail_" + id;
                try
                {
                    foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//li/section/div[@id='" + liId + "']/ul/li"))
                    {
                        try
                        {
                            content += div.InnerHtml + "<br />";
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                catch (Exception ex)
                { }
                if (!string.IsNullOrWhiteSpace(content))
                {
                    content = content.Replace("<br>", "<br />");
                    content = content.Replace("</div><div>", "<br />");
                    content = content.Replace("<div>", "<br />");
                    content = content.Replace("</div>", "<br />");
                }
            }
            return content;
        }

        public string NotesLabOrderResult(List<LabOrderResult> objList_LabOrderResult, List<NotesComponent> objListNotesComponent)
        {
            var LabOrderResultHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "LabResults");
            if ((objList_LabOrderResult != null && objList_LabOrderResult.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                LabOrderResultHTML += "<table style='width:100%;word-wrap: break-word;font-size:12px;'>";
                LabOrderResultHTML += "<tr><th class='font-xs bold text-primary' colspan='6' style='padding-left:3px;padding-top:5px;padding-bottom:10px;color: #0088cc;'><b>Lab Results </b></th></tr>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    LabOrderResultHTML += "<tr><td colspan='6' style='padding-left:3px;padding-top:5px;padding-bottom:10px;'>" + upComment + "</td></tr>";
                }
                if (objList_LabOrderResult != null && objList_LabOrderResult.Count() > 0)
                {
                    foreach (LabOrderResult objLabOrderResultOuter in objList_LabOrderResult.GroupBy(m => new { m.LabOrderResultId }).Select(g => g.OrderBy(m => m.LabOrderResultId).FirstOrDefault()))
                    {
                        LabOrderResultHTML += "<tr style='background-color:#0188CC;color: #FFFFFF;'><th style='padding:3px;border: 1px solid #ddd;'>Date & Time</th><th style='padding:3px;border: 1px solid #ddd;'>Observation</th><th style='padding:3px;border: 1px solid #ddd;'>Result</th><th style='padding:3px;border: 1px solid #ddd;'>UoM</th><th style='padding:3px;border: 1px solid #ddd;'>Flag</th><th style='padding:3px;border: 1px solid #ddd;'>Range</th></tr>";
                        foreach (LabOrderResult objLabOrderResult in objList_LabOrderResult.Where(m => m.LabOrderResultId == objLabOrderResultOuter.LabOrderResultId).GroupBy(m => new { m.CPTCode }).Select(g => g.OrderBy(m => m.CPTCode).FirstOrDefault()))
                        {
                            LabOrderResultHTML += "<tr><th colspan='6' style='padding-left:3px;padding-top:5px;padding-bottom:5px;border: 1px solid #ddd;'><b>" + System.Web.HttpUtility.HtmlEncode(objLabOrderResult.CPTCode) + " " + System.Web.HttpUtility.HtmlEncode(objLabOrderResult.CPTCodeDescription) + " </b></th></tr>";
                            foreach (LabOrderResult objResult in objList_LabOrderResult.Where(m => m.CPTCode == objLabOrderResult.CPTCode && m.LabOrderResultId == objLabOrderResult.LabOrderResultId))
                            {
                                LabOrderResultHTML += "<tr><td style='padding-left:3px;padding-top:3px;padding-bottom:3px;border-left: 1px solid #ddd;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>" + objResult.ObservationDate.ToString("MM/dd/yyyy hh:mm tt") + "</td>";
                                LabOrderResultHTML += "<td style='padding:3px;border-left: 1px solid #ddd;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>" + objResult.LOINCDescription + "</td>";
                                LabOrderResultHTML += "<td style='padding:3px;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>" + objResult.Result + "</td>";
                                LabOrderResultHTML += "<td style='padding:3px;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>" + objResult.UoM + "</td>";
                                LabOrderResultHTML += "<td style='padding:3px;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>" + objResult.Flag + "</td>";
                                if (!string.IsNullOrWhiteSpace(objResult.Range))
                                {
                                    LabOrderResultHTML += "<td style='padding:3px;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>" + System.Web.HttpUtility.HtmlEncode(objResult.Range) + "</td>";
                                }
                                else
                                {
                                    LabOrderResultHTML += "<td style='padding:3px;border-top: 1px solid #ddd;border-right: 1px solid #ddd;border-bottom: 1px solid #ddd;'>&nbsp;</td>";
                                }
                                LabOrderResultHTML += "</tr>";
                            }
                        }
                        if (!string.IsNullOrEmpty(objLabOrderResultOuter.Status))
                            LabOrderResultHTML += "<tr><td colspan='6' style='padding-left:3px;padding-top:10px;padding-bottom:5px;'><b>Status: </b>" + System.Web.HttpUtility.HtmlEncode(objLabOrderResultOuter.Status) + "</td></tr>";
                        if (!string.IsNullOrEmpty(objLabOrderResultOuter.Remarks))
                            LabOrderResultHTML += "<tr><td colspan='6' style='padding-left:3px;padding-top:5px;padding-bottom:5px;'><b>Remarks: </b>" + System.Web.HttpUtility.HtmlEncode(objLabOrderResultOuter.Remarks) + "</td></tr>";
                        if (!string.IsNullOrEmpty(objLabOrderResultOuter.Comments))
                        {
                            objLabOrderResultOuter.Comments = objLabOrderResultOuter.Comments.Replace("<br>", "<br />");
                            objLabOrderResultOuter.Comments = objLabOrderResultOuter.Comments.Replace("</div><div>", "<br />");
                            objLabOrderResultOuter.Comments = objLabOrderResultOuter.Comments.Replace("<div>", "<br />");
                            objLabOrderResultOuter.Comments = objLabOrderResultOuter.Comments.Replace("</div>", "<br />");
                            LabOrderResultHTML += "<tr><td colspan='6' style='padding-left:3px;padding-top:5px;padding-bottom:5px;'><b>Comments: </b>" + objLabOrderResultOuter.Comments + "</td></tr>";
                        }

                        string Comments = NotesCommentContents(objListNotesComponent, "LabResultDetail_" + objLabOrderResultOuter.LabOrderResultId);
                        if (!string.IsNullOrWhiteSpace(Comments))
                        {
                            LabOrderResultHTML += "<tr><td colspan='6' style='padding-left:3px;padding-top:5px;padding-bottom:7px;'>" + Comments + "</td></tr>";
                        }
                    }

                }
                LabOrderResultHTML += "</table>";
            }
            return LabOrderResultHTML;
        }

        public string NotesLabOrder(List<LabOrder> objList_LabOrder, List<NotesComponent> objListNotesComponent)
        {
            var LabOrderHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "LabOrder");
            if ((objList_LabOrder != null && objList_LabOrder.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                LabOrderHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                LabOrderHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Lab Order </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    LabOrderHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                if (objList_LabOrder != null && objList_LabOrder.Count() > 0)
                {
                    foreach (LabOrder objLabOrder in objList_LabOrder.Where(m => m.Type == "Tests").GroupBy(m => new { m.LabOrderId }).Select(g => g.OrderBy(m => m.LabOrderId).FirstOrDefault()))
                    {
                        //LabOrderHTML += "<li><b>" + objLabOrder.CPTCode + " " + objLabOrder.CPTCodeDescription + " </b>,";
                        //if (!string.IsNullOrWhiteSpace(objLabOrder.Urgency))
                        //{
                        //    LabOrderHTML += " Urgency: " + objLabOrder.Urgency;
                        //}
                        //LabOrderHTML += " </li>";
                        //if (!string.IsNullOrWhiteSpace(objLabOrder.SoapText))
                        //{
                        //    string strSoapText = NotesOrderTestContents(objLabOrder.SoapText);
                        //    if (!string.IsNullOrWhiteSpace(strSoapText))
                        //    {
                        //        LabOrderHTML += "<li style='margin-left:3px !important;padding-left:3px !important;'><ul style='padding-left:3px !important;margin-left:3px !important;'><li>";
                        //        LabOrderHTML += strSoapText;
                        //        LabOrderHTML += " </li></ul> </li>";
                        //    }
                        //}
                        if (objListNotesComponent != null && objListNotesComponent.Count() > 0)
                        {
                            string strSoapText = NotesLabOrderTemplateSoapContents(objListNotesComponent, objLabOrder.LabOrderId.ToString());
                            if (!string.IsNullOrWhiteSpace(strSoapText))
                            {
                                LabOrderHTML += "<li>";
                                LabOrderHTML += strSoapText;
                                LabOrderHTML += " </li>";
                            }
                        }
                        //if (objList_LabOrder.Where(m => m.LabOrderId == objLabOrder.LabOrderId).FirstOrDefault(m => m.Type == "Problem List") != null)
                        //{
                        //    LabOrderHTML += "<li>Associated Problem(s) </li>";
                        //}
                        //foreach (LabOrder objLabOrderInner in objList_LabOrder.Where(m => m.LabOrderId == objLabOrder.LabOrderId && m.Type == "Problem List").OrderByDescending(m => m.SortOrder))
                        //{
                        //    LabOrderHTML += "<li><b>" + objLabOrderInner.ICD10 + " " + objLabOrderInner.ICD10Description + " </b> </li>";
                        //}

                        //LabOrder obj = objList_LabOrder.Where(m => m.LabOrderId == objLabOrder.LabOrderId).FirstOrDefault();
                        //if (obj != null)
                        //{
                        //    //LabOrderHTML += "<li>Last Updated On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                        //    string Comments = NotesCommentContents(objListNotesComponent, "LabOrderDetail_" + obj.LabOrderId);
                        //    if (!string.IsNullOrWhiteSpace(Comments))
                        //    {
                        //        LabOrderHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                        //    }
                        //}
                    }
                }
                LabOrderHTML += "</ul>";
            }
            return LabOrderHTML;
        }
        public string NotesPatLetter(List<PatientLetter> objList_PatientLetter, List<NotesComponent> objListNotesComponent)
        {
            var PatLetterHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Letter");
            if ((objList_PatientLetter != null && objList_PatientLetter.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                PatLetterHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                PatLetterHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Letter </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                    PatLetterHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                foreach (PatientLetter objPatLetter in objList_PatientLetter)
                {
                    string strSoapText = NotesPatLetterTemplateSoapContents(objListNotesComponent, objPatLetter.LetterId.ToString());
                    if (!string.IsNullOrWhiteSpace(strSoapText))
                    {
                        PatLetterHTML += "<li>";
                        PatLetterHTML += strSoapText;
                        PatLetterHTML += " </li>";
                    }
                }
                PatLetterHTML += "</ul>";
            }
            return PatLetterHTML;
        }
        public string NotesPatLetterTemplateSoapContents(List<NotesComponent> objListNotesComponent, string id)
        {
            string content = string.Empty;
            if (!string.IsNullOrWhiteSpace(objListNotesComponent.FirstOrDefault().SOAPText))
            {
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(objListNotesComponent.FirstOrDefault().SOAPText);
                string liId = "Cli_PatientLetter_Main" + id;
                try
                {
                    HtmlNode sec = document.DocumentNode.SelectNodes("//li/section[@id='" + liId + "']/header/a").FirstOrDefault();
                    if (sec != null)
                        content += sec.InnerText + "<br />";
                }
                catch (Exception ex)
                { }
                if (!string.IsNullOrWhiteSpace(content))
                    content = content.Replace("<br>", "<br />");
            }
            return content;
        }

        public string NotesLabOrderTemplateSoapContents(List<NotesComponent> objListNotesComponent, string id)
        {
            string content = string.Empty;
            if (!string.IsNullOrWhiteSpace(objListNotesComponent.FirstOrDefault().SOAPText))
            {
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(objListNotesComponent.FirstOrDefault().SOAPText);
                string liId = "Cli_LabOrderDetail_" + id;
                try
                {
                    foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//li/section/div[@id='" + liId + "']/ul/li"))
                    {
                        try
                        {
                            content += div.InnerHtml + "<br />";
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                catch (Exception ex)
                { }
                if (!string.IsNullOrWhiteSpace(content))
                    content = content.Replace("<br>", "<br />");
            }
            return content;
        }

        public string NotesOrderTestContents(string SOAPText)
        {
            string content = string.Empty;
            if (!string.IsNullOrWhiteSpace(SOAPText))
            {
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(SOAPText);
                try
                {
                    foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//section"))
                    {
                        try
                        {
                            content += div.InnerHtml + "<br />";
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                catch (Exception ex)
                { }
                if (!string.IsNullOrWhiteSpace(content))
                    content = content.Replace("<br>", "<br />");
            }
            return content;
        }

        public string NotesVitalSigns(List<VitalSigns> objListVitalSigns, List<NotesComponent> objListNotesComponent)
        {
            var VitalSignsHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Vitals");
            if ((objListVitalSigns != null && objListVitalSigns.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                VitalSignsHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                VitalSignsHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Vitals </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    VitalSignsHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                if (objListVitalSigns != null && objListVitalSigns.Count() > 0)
                {
                    var groupedVitalsList = objListVitalSigns.GroupBy(u => u.VitalSignId).Select(g => g.OrderBy(m => m.VitalSignId)).ToList();


                    for (int i = 0; i < groupedVitalsList.Count; i++)
                    {
                        VitalSignsHTML += "<li>";
                        VitalSignsHTML += " </li>";
                        VitalSignsHTML += "<li>";
                        foreach (VitalSigns objVitalSignsBP in groupedVitalsList[i].Where(m => m.Type == "BloodPressure"))
                        {
                            int CounterBP = 0;
                            if (objVitalSignsBP.Systolic > 0)
                            {
                                VitalSignsHTML += "BP: " + objVitalSignsBP.Systolic;
                                CounterBP = 1;
                            }
                            if (objVitalSignsBP.Diastolic > 0)
                            {
                                VitalSignsHTML += "/" + objVitalSignsBP.Diastolic + "";
                                CounterBP = 1;
                            }
                            if (CounterBP > 0)
                            {
                                VitalSignsHTML += " mmHg,";
                            }

                        }
                        foreach (VitalSigns objVitalSignsP in groupedVitalsList[i].Where(m => m.Type == "Pulse"))
                        {
                            if (!string.IsNullOrWhiteSpace(objVitalSignsP.Pulse))
                            {
                                VitalSignsHTML += " Pulse: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsP.Pulse) + " bpm,";
                            }
                            if (!string.IsNullOrWhiteSpace(objVitalSignsP.Rhythm))
                            {
                                VitalSignsHTML += " Rhythm: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsP.Rhythm) + ",";
                            }
                        }

                        foreach (VitalSigns objVitalSignsT in groupedVitalsList[i].Where(m => m.Type == "Temprature"))
                        {
                            if (!string.IsNullOrWhiteSpace(objVitalSignsT.Temprature))
                            {
                                VitalSignsHTML += " Temprature: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsT.Temprature) + " F,";
                            }
                           
                           
                        }

                        foreach (VitalSigns objVitalSignsR in groupedVitalsList[i].Where(m => m.Type == "Respiration"))
                        {
                            if (!string.IsNullOrWhiteSpace(objVitalSignsR.Rate))
                            {
                                VitalSignsHTML += " Respiration Rate: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsR.Rate) + " rpm,";
                            }
                            if (!string.IsNullOrWhiteSpace(objVitalSignsR.Pattern))
                            {
                                VitalSignsHTML += " Pattern: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsR.Pattern) + ",";
                            }
                        }
                        foreach (VitalSigns objVitalSigns in groupedVitalsList[i].Where(m => m.Type == "Vital Sign"))
                        {

                            if (objVitalSigns.Weight > 0)
                            {
                                VitalSignsHTML += " Weight: " + objVitalSigns.Weight + " lbs,";
                            }
                            if (!string.IsNullOrWhiteSpace(objVitalSigns.Height))
                            {
                                VitalSignsHTML += " Height: " + objVitalSigns.Height + " in,";
                            }
                            if (objVitalSigns.BSA > 0)
                            {
                                VitalSignsHTML += " BSA: " + objVitalSigns.BSA + " m2,";
                            }
                            if (objVitalSigns.BMI > 0)
                            {
                                if (objVitalSigns.BMI >= 25.00)
                                {
                                    VitalSignsHTML += " BMI: " + "<span style='color:red'>" + objVitalSigns.BMI + " kg/m2,</span>";
                                }
                                else
                                {
                                    VitalSignsHTML += " BMI: " + "<span style='color:black'>" + objVitalSigns.BMI + " kg/m2,</span> ";
                                }
                            }
                            if (objVitalSigns.HeadCr > 0)
                            {
                                VitalSignsHTML += " Head Circumference: " + objVitalSigns.HeadCr + " cm,";
                            }
                            foreach (VitalSigns objVitalSignsBG in groupedVitalsList[i].Where(m => m.Type == "BloodPressure"))
                            {
                                if (!string.IsNullOrEmpty(objVitalSignsBG.BloodType))
                                {
                                    VitalSignsHTML += " Blood Group:" + objVitalSignsBG.BloodType + ",";
                                }

                            }
                            if (!string.IsNullOrWhiteSpace(objVitalSigns.SPO2))
                            {
                                VitalSignsHTML += " SPO2: " + System.Web.HttpUtility.HtmlEncode(objVitalSigns.SPO2) + " %,";
                            }
                            if (!string.IsNullOrWhiteSpace(objVitalSigns.OxygenSource))
                            {
                                VitalSignsHTML += " Oxygen Source: " + System.Web.HttpUtility.HtmlEncode(objVitalSigns.OxygenSource) + ",";
                            }
                            if (!string.IsNullOrWhiteSpace(objVitalSigns.PeakFlow))
                            {
                                VitalSignsHTML += " Peak Flow: " + System.Web.HttpUtility.HtmlEncode(objVitalSigns.PeakFlow) + " L/min,";
                            }
                            if (!string.IsNullOrWhiteSpace(objVitalSigns.Comments))
                            {
                                objVitalSigns.Comments = objVitalSigns.Comments.Replace("<br>", "< br />");
                                objVitalSigns.Comments = objVitalSigns.Comments.Replace("</div><div>", "<br/>");
                                objVitalSigns.Comments = objVitalSigns.Comments.Replace("<div>", "<br/>");
                                objVitalSigns.Comments = objVitalSigns.Comments.Replace("</div>", "<br/>");
                                VitalSignsHTML += " Comments: " + objVitalSigns.Comments;
                            }
                            VitalSignsHTML += " on " + MDVUtility.ToStr(objVitalSigns.VitalSignDate).Split(' ')[0] + " at " + objVitalSigns.VitalSignTime;
                        }

                        VitalSignsHTML += " </li>";
                    }
                    string Comments = NotesCommentContents(objListNotesComponent, "Vitals_" + objListVitalSigns.FirstOrDefault().VitalSignId);
                    if (!string.IsNullOrWhiteSpace(Comments))
                    {
                        VitalSignsHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                    }
                }
                VitalSignsHTML += "</ul>";
            }

            return VitalSignsHTML;
        }

        //public string NotesVitalSigns(List<VitalSigns> objListVitalSigns, List<NotesComponent> objListNotesComponent)
        //{
        //    var VitalSignsHTML = string.Empty;
        //    string upComment = NotesCommentContentsUpper(objListNotesComponent, "Vitals");
        //    if ((objListVitalSigns != null && objListVitalSigns.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
        //    {
        //        VitalSignsHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
        //        VitalSignsHTML += "<li style='color: #0088cc;'><b>Vitals </b> </li>";
        //        if (!string.IsNullOrWhiteSpace(upComment))
        //        {
        //            VitalSignsHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
        //        }
        //        if (objListVitalSigns != null && objListVitalSigns.Count() > 0)
        //        {
        //            foreach (VitalSigns objVitalSigns in objListVitalSigns.Where(m => m.Type == "Vital Sign"))
        //            {
        //                VitalSignsHTML += "<li>";
        //                //VitalSignsHTML += "Vitals were taken by " + objVitalSigns.CreatedBy + " on " + objVitalSigns.VitalSignDate.ToString("MM/dd/yyyy") + " at " + DateTime.Today.Add(objVitalSigns.VitalSignTime).ToString("hh:mm tt");
        //                VitalSignsHTML += " </li>";
        //                VitalSignsHTML += "<li>";
        //                foreach (VitalSigns objVitalSignsBP in objListVitalSigns.Where(m => m.Type == "BloodPressure"))
        //                {
        //                    int CounterBP = 0;
        //                    if (objVitalSignsBP.Systolic > 0)
        //                    {
        //                        VitalSignsHTML += "BP: " + objVitalSignsBP.Systolic;
        //                        CounterBP = 1;
        //                    }
        //                    if (objVitalSignsBP.Diastolic > 0)
        //                    {
        //                        VitalSignsHTML += "/" + objVitalSignsBP.Diastolic + "";
        //                        CounterBP = 1;
        //                    }
        //                    if (CounterBP > 0)
        //                    {
        //                        VitalSignsHTML += " mmHg,";
        //                    }
        //                    //if (!string.IsNullOrWhiteSpace(objVitalSignsBP.Position))
        //                    //{
        //                    //    VitalSignsHTML += " Position: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsBP.Position) + ",";
        //                    //}
        //                    //if (!string.IsNullOrWhiteSpace(objVitalSignsBP.CuffLocation))
        //                    //{
        //                    //    VitalSignsHTML += " Cuff Location: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsBP.CuffLocation) + ",";
        //                    //}
        //                    //if (!string.IsNullOrWhiteSpace(objVitalSignsBP.CuffSize))
        //                    //{
        //                    //    VitalSignsHTML += " Cuff Size: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsBP.CuffSize) + ",";
        //                    //}
        //                }
        //                foreach (VitalSigns objVitalSignsP in objListVitalSigns.Where(m => m.Type == "Pulse"))
        //                {
        //                    if (!string.IsNullOrWhiteSpace(objVitalSignsP.Pulse))
        //                    {
        //                        VitalSignsHTML += " Pulse: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsP.Pulse) + " bpm,";
        //                    }
        //                    if (!string.IsNullOrWhiteSpace(objVitalSignsP.Rhythm))
        //                    {
        //                        VitalSignsHTML += " Rhythm: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsP.Rhythm) + ",";
        //                    }
        //                }

        //                foreach (VitalSigns objVitalSignsT in objListVitalSigns.Where(m => m.Type == "Temprature"))
        //                {
        //                    if (!string.IsNullOrWhiteSpace(objVitalSignsT.Temprature))
        //                    {
        //                        VitalSignsHTML += " Temprature: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsT.Temprature) + " F,";
        //                    }
        //                    if (!string.IsNullOrWhiteSpace(objVitalSignsT.Method))
        //                    {
        //                        //VitalSignsHTML += " Method: " + objVitalSignsT.Method;
        //                    }
        //                    else
        //                    {
        //                        VitalSignsHTML += ",";
        //                    }
        //                }

        //                foreach (VitalSigns objVitalSignsR in objListVitalSigns.Where(m => m.Type == "Respiration"))
        //                {
        //                    if (!string.IsNullOrWhiteSpace(objVitalSignsR.Rate))
        //                    {
        //                        VitalSignsHTML += " Respiration Rate: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsR.Rate) + " rpm,";
        //                    }
        //                    if (!string.IsNullOrWhiteSpace(objVitalSignsR.Pattern))
        //                    {
        //                        VitalSignsHTML += " Pattern: " + System.Web.HttpUtility.HtmlEncode(objVitalSignsR.Pattern) + ",";
        //                    }
        //                }
        //                if (objVitalSigns.Weight > 0)
        //                {
        //                    VitalSignsHTML += " Weight: " + objVitalSigns.Weight + " lbs,";
        //                }
        //                if (!string.IsNullOrWhiteSpace(objVitalSigns.Height))
        //                {
        //                    VitalSignsHTML += " Height: " + objVitalSigns.Height + " in,";
        //                }
        //                if (objVitalSigns.BSA > 0)
        //                {
        //                    VitalSignsHTML += " BSA: " + objVitalSigns.BSA + " m2,";
        //                }
        //                if (objVitalSigns.BMI > 0)
        //                {
        //                    if (objVitalSigns.BMI >= 25.00)
        //                    {
        //                        VitalSignsHTML += " BMI: " + "<span style='color:red'>" + objVitalSigns.BMI + " kg/m2,</span>";
        //                    }
        //                    else
        //                    {
        //                        VitalSignsHTML += " BMI: " + "<span style='color:black'>" + objVitalSigns.BMI + " kg/m2,</span> ";
        //                    }
        //                }
        //                if (objVitalSigns.HeadCr > 0)
        //                {
        //                    VitalSignsHTML += " Head Circumference: " + objVitalSigns.HeadCr + " cm,";
        //                }
        //                if (objListVitalSigns.FirstOrDefault(m => m.Type == "BloodPressure" && m.BloodType != null) != null)
        //                {
        //                    if (!string.IsNullOrEmpty(objListVitalSigns.FirstOrDefault(m => m.Type == "BloodPressure").BloodType))
        //                    {
        //                        VitalSignsHTML += " Blood Group:" + objListVitalSigns.FirstOrDefault(m => m.Type == "BloodPressure").BloodType + ",";
        //                    }
        //                }
        //                if (!string.IsNullOrWhiteSpace(objVitalSigns.SPO2))
        //                {
        //                    VitalSignsHTML += " SPO2: " + System.Web.HttpUtility.HtmlEncode(objVitalSigns.SPO2) + " %,";
        //                }
        //                if (!string.IsNullOrWhiteSpace(objVitalSigns.OxygenSource))
        //                {
        //                    VitalSignsHTML += " Oxygen Source: " + System.Web.HttpUtility.HtmlEncode(objVitalSigns.OxygenSource) + ",";
        //                }
        //                if (!string.IsNullOrWhiteSpace(objVitalSigns.PeakFlow))
        //                {
        //                    VitalSignsHTML += " Peak Flow: " + System.Web.HttpUtility.HtmlEncode(objVitalSigns.PeakFlow) + " L/min,";
        //                }
        //                if (!string.IsNullOrWhiteSpace(objVitalSigns.Comments))
        //                {
        //                    objVitalSigns.Comments = objVitalSigns.Comments.Replace("<br>", "< br />");
        //                    objVitalSigns.Comments = objVitalSigns.Comments.Replace("</div><div>", "<br/>");
        //                    objVitalSigns.Comments = objVitalSigns.Comments.Replace("<div>", "<br/>");
        //                    objVitalSigns.Comments = objVitalSigns.Comments.Replace("</div>", "<br/>");
        //                    VitalSignsHTML += " Comments: " + objVitalSigns.Comments + ", on " + MDVUtility.ToStr(objVitalSigns.VitalSignDate).Split(' ')[0] + " at " + objVitalSigns.VitalSignTime;
        //                }
        //                VitalSignsHTML += " </li>";
        //            }
        //            string Comments = NotesCommentContents(objListNotesComponent, "Vitals_" + objListVitalSigns.FirstOrDefault().VitalSignId);
        //            if (!string.IsNullOrWhiteSpace(Comments))
        //            {
        //                VitalSignsHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
        //            }
        //        }
        //        VitalSignsHTML += "</ul>";
        //    }

        //    return VitalSignsHTML;
        //}

        public string NotesHospitalizationHx(List<HospitalizationHx> objListHospitalizationHx, List<NotesComponent> objListNotesComponent)
        {
            var HospitalizationHxHTML = string.Empty;
            HospitalizationHxHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            HospitalizationHxHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Hospitalization Hx </b> </li>";
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "HospitalizationHx");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                HospitalizationHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            if (objListHospitalizationHx != null && objListHospitalizationHx.Count() > 0)
            {
                foreach (HospitalizationHx objHospitalizationHx in objListHospitalizationHx)
                {
                    HospitalizationHxHTML += "<li>";
                    if (objHospitalizationHx.bUnremarkable == true)
                    {
                        HospitalizationHxHTML += "Unremarkable";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(objHospitalizationHx.Hospital))
                        {
                            HospitalizationHxHTML += "Patient was admitted to " + System.Web.HttpUtility.HtmlEncode(objHospitalizationHx.Hospital) + ",";
                        }
                        else
                        {
                            HospitalizationHxHTML += "Patient was admitted to health care delivery facility,";
                        }
                        if (objHospitalizationHx.AdmissionDate != null)
                        {
                            HospitalizationHxHTML += " on " + Convert.ToDateTime(objHospitalizationHx.AdmissionDate).ToString("MM/dd/yyyy") + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objHospitalizationHx.CPTDescription))
                        {
                            HospitalizationHxHTML += " For " + System.Web.HttpUtility.HtmlEncode(objHospitalizationHx.CPTDescription) + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objHospitalizationHx.FreeTextICD))
                        {
                            HospitalizationHxHTML += " based on the following assessment: <b>" + System.Web.HttpUtility.HtmlEncode(objHospitalizationHx.FreeTextICD) + " </b>,";
                        }
                        if (!string.IsNullOrWhiteSpace(objHospitalizationHx.Status))
                        {
                            HospitalizationHxHTML += " which was " + objHospitalizationHx.Status + ",";
                        }
                        if (objHospitalizationHx.DischargeDate != null)
                        {
                            HospitalizationHxHTML += " and was discharged on " + Convert.ToDateTime(objHospitalizationHx.AdmissionDate).ToString("MM/dd/yyyy") + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objHospitalizationHx.Comments))
                        {
                            HospitalizationHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objHospitalizationHx.Comments) + ",";
                        }
                    }
                    HospitalizationHxHTML += " </li>";
                }
                HospitalizationHx obj = objListHospitalizationHx.FirstOrDefault();
                if (obj != null)
                {
                    if (obj.CreatedOn == obj.ModifiedOn)
                        HospitalizationHxHTML += "<li>Added On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                    else
                        HospitalizationHxHTML += "<li>Last Updated On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                }
                string Comments = NotesCommentContents(objListNotesComponent, "HospitalizationHx_" + obj.HospitalizationHxId);
                if (!string.IsNullOrWhiteSpace(Comments))
                {
                    HospitalizationHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                }
            }
            else if ((objListHospitalizationHx == null || objListHospitalizationHx.Count() == 0) && string.IsNullOrWhiteSpace(upComment))
            {
                HospitalizationHxHTML += "<li>No Hospitalization Hx </li>";
            }
            HospitalizationHxHTML += "</ul>";

            return HospitalizationHxHTML;
        }

        public string NotesSurgicalHx(List<SurgicalHx> objListSurgicalHx, List<NotesComponent> objListNotesComponent)
        {
            var SurgicalHxHTML = string.Empty;
            SurgicalHxHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            SurgicalHxHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Surgical Hx </b> </li>";
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "SurgicalHx");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                SurgicalHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            if (objListSurgicalHx != null && objListSurgicalHx.Count() > 0)
            {
                foreach (SurgicalHx objSurgicalHx in objListSurgicalHx)
                {
                    if (objSurgicalHx.bUnremarkable == true)
                    {
                        SurgicalHxHTML += "<li>Unremarkable </li>";
                    }

                    else if (objSurgicalHx.bUnremarkable == false)
                    {
                        SurgicalHxHTML += "<li>Patient underwent ";
                        if (!string.IsNullOrWhiteSpace(objSurgicalHx.CPTDescription))
                        {
                            SurgicalHxHTML += " " + System.Web.HttpUtility.HtmlEncode(objSurgicalHx.CPTDescription) + ",";
                        }
                        if (objSurgicalHx.SurgeryDate != null)
                        {
                            SurgicalHxHTML += " on " + Convert.ToDateTime(objSurgicalHx.SurgeryDate).ToString("MM/dd/yyyy") + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objSurgicalHx.AgeAtSurgery))
                        {
                            SurgicalHxHTML += " at the age of " + System.Web.HttpUtility.HtmlEncode(objSurgicalHx.AgeAtSurgery) + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objSurgicalHx.SurgeryReason))
                        {
                            SurgicalHxHTML += " for the following reason: " + System.Web.HttpUtility.HtmlEncode(objSurgicalHx.SurgeryReason) + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objSurgicalHx.Status))
                        {
                            SurgicalHxHTML += " Status was " + objSurgicalHx.Status + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objSurgicalHx.Location))
                        {
                            SurgicalHxHTML += " Surgery location was " + System.Web.HttpUtility.HtmlEncode(objSurgicalHx.Location) + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objSurgicalHx.OrderingProvider))
                        {
                            SurgicalHxHTML += " Ordering Provider was " + objSurgicalHx.OrderingProvider + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objSurgicalHx.PerformingProvider))
                        {
                            SurgicalHxHTML += " Performing Provider was " + objSurgicalHx.PerformingProvider + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objSurgicalHx.Comments))
                        {
                            SurgicalHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objSurgicalHx.Comments) + ".";
                        }
                        SurgicalHxHTML += " </li>";
                    }
                }
                SurgicalHx obj = objListSurgicalHx.FirstOrDefault();
                if (obj != null)
                {
                    if (obj.CreatedOn == obj.ModifiedOn)
                        SurgicalHxHTML += "<li>Added On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                    else
                        SurgicalHxHTML += "<li>Last Updated On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                }
                string Comments = NotesCommentContents(objListNotesComponent, "SurgicalHx_" + obj.SurgicalHxId);
                if (!string.IsNullOrWhiteSpace(Comments))
                {
                    SurgicalHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                }
            }
            else if ((objListSurgicalHx == null || objListSurgicalHx.Count() == 0) && string.IsNullOrWhiteSpace(upComment))
            {
                SurgicalHxHTML += "<li>No Surgical History </li>";
            }
            SurgicalHxHTML += "</ul>";

            return SurgicalHxHTML;
        }

        public string NotesFamilyHx(List<FamilyHx> objListFamilyHx, List<NotesComponent> objListNotesComponent)
        {
            var FamilyHxHTML = string.Empty;
            FamilyHxHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            FamilyHxHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Family Hx </b> </li>";
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "FamilyHx");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                FamilyHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            if (objListFamilyHx != null && objListFamilyHx.Count() > 0)
            {
                foreach (FamilyHx objFamilyHx in objListFamilyHx)
                {
                    if (objFamilyHx.bUnremarkable == true)
                    {
                        FamilyHxHTML += "<li>Unremarkable </li>";
                    }
                    else
                    {
                        FamilyHxHTML += "<li><b>" + objFamilyHx.Relation + " </b> </li>";
                        FamilyHxHTML += "<li>";

                        if (!string.IsNullOrWhiteSpace(objFamilyHx.ICD10CodeDescription) && objFamilyHx.ICD10CodeDescription != "-1")
                        {
                            FamilyHxHTML += "" + System.Web.HttpUtility.HtmlEncode(objFamilyHx.ICD10CodeDescription) + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objFamilyHx.HealthStatus))
                        {
                            if (objFamilyHx.HealthStatus.ToLower() == "isalive")
                                FamilyHxHTML += " Health Status is Alive.";
                            else
                                FamilyHxHTML += " Health Status is " + objFamilyHx.HealthStatus + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objFamilyHx.IsRelativeDied))
                        {
                            if (objFamilyHx.IsRelativeDied.ToLower() == "true")
                                FamilyHxHTML += " Death has occurred due to specified problem.";
                            else
                                FamilyHxHTML += " Death has not occurred due to specified problem.";
                        }
                        if (!string.IsNullOrWhiteSpace(objFamilyHx.BirthYear))
                        {
                            FamilyHxHTML += " Year of birth is " + objFamilyHx.BirthYear + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objFamilyHx.AgeAtDeath))
                        {
                            FamilyHxHTML += " Age at death is " + System.Web.HttpUtility.HtmlEncode(objFamilyHx.AgeAtDeath) + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objFamilyHx.AgeAtDiagnosis))
                        {
                            FamilyHxHTML += " Age at diagnosis was (approx) " + System.Web.HttpUtility.HtmlEncode(objFamilyHx.AgeAtDiagnosis) + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objFamilyHx.Comments))
                        {
                            FamilyHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objFamilyHx.Comments) + ",";
                        }
                        FamilyHxHTML += " </li>";
                    }
                }

                FamilyHx obj = objListFamilyHx.FirstOrDefault();
                if (obj != null)
                {
                    if (obj.CreatedOn == obj.ModifiedOn)
                        FamilyHxHTML += "<li>Added On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                    else
                        FamilyHxHTML += "<li>Last Updated On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                }
                string Comments = NotesCommentContents(objListNotesComponent, "FamilyHx_" + obj.FamilyHxId);
                if (!string.IsNullOrWhiteSpace(Comments))
                {
                    FamilyHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                }
            }
            else if ((objListFamilyHx == null || objListFamilyHx.Count() == 0) && string.IsNullOrWhiteSpace(upComment))
            {
                FamilyHxHTML += "<li>No Family History </li>";
            }
            FamilyHxHTML += "</ul>";

            return FamilyHxHTML;
        }

        public string NotesProblemHx(List<ProblemHx> objListProblemHx, List<NotesComponent> objListNotesComponent)
        {
            var ProblemHxHTML = string.Empty;
            ProblemHxHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            ProblemHxHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Problems </b> </li>";
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Problems");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                ProblemHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            if (objListProblemHx != null && objListProblemHx.Count() > 0)
            {
                foreach (ProblemHx objProblemHx in objListProblemHx)
                {
                    ProblemHxHTML += "<li>";
                    if (!string.IsNullOrWhiteSpace(objProblemHx.ICD10))
                    {
                        ProblemHxHTML += "<b>" + System.Web.HttpUtility.HtmlEncode(objProblemHx.ICD10) + " </b>";
                    }
                    if (!string.IsNullOrWhiteSpace(objProblemHx.ICD10Description))
                    {
                        ProblemHxHTML += "<b> - " + System.Web.HttpUtility.HtmlEncode(objProblemHx.ICD10Description) + " </b>";
                    }
                    if (!objProblemHx.IsActive)
                    {
                        ProblemHxHTML += " <span style='color:red'> (Inactive) </span>";
                    }
                    if (!string.IsNullOrWhiteSpace(objProblemHx.ChronicityLevel))
                    {
                        ProblemHxHTML += " Chronicity: " + objProblemHx.ChronicityLevel;
                    }
                    if (!string.IsNullOrWhiteSpace(objProblemHx.ChronicityLevel) && !string.IsNullOrWhiteSpace(objProblemHx.Severity))
                        ProblemHxHTML += ", ";
                    if (!string.IsNullOrWhiteSpace(objProblemHx.Severity))
                    {
                        string clr = "";
                        if (objProblemHx.Severity == "Mild Intermittent" || objProblemHx.Severity == "Mild Persistent")
                        {
                            clr = "color:green";
                        }
                        else if (objProblemHx.Severity == "Severe Persistent" || objProblemHx.Severity == "Unspecified Severity")
                        {
                            clr = "color:red";
                        }
                        else if (objProblemHx.Severity == "Moderate Persistent")
                        {
                            clr = "color:orange";
                        }
                        ProblemHxHTML += " <span style='" + @clr + "'>Severity: " + objProblemHx.Severity + " </span>";
                    }
                    if (objProblemHx.StartDate != null)
                    {
                        ProblemHxHTML += " Started on " + Convert.ToDateTime(objProblemHx.StartDate).ToString("MM/dd/yyyy");
                    }
                    if (objProblemHx.EndDate != null)
                    {
                        ProblemHxHTML += " and ended on " + Convert.ToDateTime(objProblemHx.EndDate).ToString("MM/dd/yyyy") + ".";
                    }
                    if (objProblemHx.ModifiedOn != null)
                    {
                        ProblemHxHTML += " Modified on " + Convert.ToDateTime(objProblemHx.ModifiedOn).ToString("MM/dd/yyyy") + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objProblemHx.Comments))
                    {
                        ProblemHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objProblemHx.Comments) + ".";
                    }
                    ProblemHxHTML += " </li>";
                    string Comments = NotesCommentContents(objListNotesComponent, "ProblemList_" + objProblemHx.ProblemListId);
                    if (!string.IsNullOrWhiteSpace(Comments))
                    {
                        ProblemHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                    }
                }
            }
            else if ((objListProblemHx == null || objListProblemHx.Count() == 0) && string.IsNullOrWhiteSpace(upComment))
            {
                ProblemHxHTML += "<li>No Problems. </li>";
            }
            ProblemHxHTML += "</ul>";

            return ProblemHxHTML;
        }

        public string NotesImmunizationHx(List<ImmunizationHx> objListImmunizationHx, List<NotesComponent> objListNotesComponent)
        {
            var ImmunizationHxHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Immunization");
            if ((objListImmunizationHx != null && objListImmunizationHx.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                ImmunizationHxHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                ImmunizationHxHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Immunization </b> </li>";
                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    ImmunizationHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                ImmunizationHxHTML += "<li><b>Administration Vaccine </b> </li>";
                if (objListImmunizationHx != null && objListImmunizationHx.Count() > 0)
                {
                    foreach (ImmunizationHx objImmunizationHx in objListImmunizationHx.Where(m => m.Type == "Immunization"))
                    {
                        ImmunizationHxHTML += "<li>";
                        if (!string.IsNullOrWhiteSpace(objImmunizationHx.CVXShortDescription))
                        {
                            ImmunizationHxHTML += "<b>" + System.Web.HttpUtility.HtmlEncode(objImmunizationHx.CVXShortDescription) + "</b>";
                        }
                        if (!string.IsNullOrWhiteSpace(objImmunizationHx.CPTCode))
                        {
                            ImmunizationHxHTML += " <b>(" + System.Web.HttpUtility.HtmlEncode(objImmunizationHx.CPTCode) + ")</b>,";
                        }
                        if (objImmunizationHx.Dose > 0)
                        {
                            string amount = (!string.IsNullOrEmpty(System.Web.HttpUtility.HtmlEncode(objImmunizationHx.Amount)) ? System.Web.HttpUtility.HtmlEncode(objImmunizationHx.Amount) : "");
                            ImmunizationHxHTML += " Dose: " + objImmunizationHx.Dose + " " + amount + ",";
                        }
                        if (!string.IsNullOrWhiteSpace(objImmunizationHx.RouteDescription))
                            ImmunizationHxHTML += " Route: " + objImmunizationHx.RouteDescription + ",";
                        if (!string.IsNullOrWhiteSpace(objImmunizationHx.SiteDescription))
                            ImmunizationHxHTML += " Site: " + objImmunizationHx.SiteDescription + ",";
                        if (!string.IsNullOrWhiteSpace(objImmunizationHx.LotNumber))
                            ImmunizationHxHTML += " Lot Number: " + objImmunizationHx.LotNumber + ",";
                        if (!string.IsNullOrWhiteSpace(objImmunizationHx.ManufacturerName))
                            ImmunizationHxHTML += " Manufacturer: " + objImmunizationHx.ManufacturerName + ",";
                        if (!string.IsNullOrWhiteSpace(objImmunizationHx.ProviderName))
                        {
                            ImmunizationHxHTML += " administrated  by " + objImmunizationHx.ProviderName;
                        }
                        if (objImmunizationHx.AdministrationDate != null)
                        {
                            ImmunizationHxHTML += " on " + objImmunizationHx.AdministrationDate.ToString("MM/dd/yyyy, hh:mm:ss tt");
                        }
                        ImmunizationHxHTML += " </li>";
                        string Comments = NotesCommentContents(objListNotesComponent, "Immunization_" + objImmunizationHx.VaccineHxId);
                        if (!string.IsNullOrWhiteSpace(Comments))
                        {
                            ImmunizationHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                        }
                    }
                    if (objListImmunizationHx.Where(m => m.Type == "TherapeuticInjection").Count() > 0)
                    {
                        ImmunizationHxHTML += "<li><b>Therapeutic Injection </b> </li>";
                        foreach (ImmunizationHx objImmunizationHx in objListImmunizationHx.Where(m => m.Type == "TherapeuticInjection"))
                        {
                            ImmunizationHxHTML += "<li>";
                            if (!string.IsNullOrWhiteSpace(objImmunizationHx.CPTCode))
                            {
                                ImmunizationHxHTML += "<b>" + System.Web.HttpUtility.HtmlEncode(objImmunizationHx.CPTCode) + "</b>";
                            }
                            if (!string.IsNullOrWhiteSpace(objImmunizationHx.TherapeuticInjection))
                            {
                                ImmunizationHxHTML += "<b> - " + System.Web.HttpUtility.HtmlEncode(objImmunizationHx.TherapeuticInjection) + "</b>";
                            }
                            if (objImmunizationHx.Dose > 0)
                            {
                                string amount = (!string.IsNullOrEmpty(System.Web.HttpUtility.HtmlEncode(objImmunizationHx.Amount)) ? System.Web.HttpUtility.HtmlEncode(objImmunizationHx.Amount) : "");
                                ImmunizationHxHTML += " Dose: " + objImmunizationHx.Dose + " " + amount + ",";
                            }
                            if (!string.IsNullOrWhiteSpace(objImmunizationHx.RouteDescription))
                                ImmunizationHxHTML += " Route: " + objImmunizationHx.RouteDescription + ",";
                            if (!string.IsNullOrWhiteSpace(objImmunizationHx.SiteDescription))
                                ImmunizationHxHTML += " Site: " + objImmunizationHx.SiteDescription + ",";
                            if (!string.IsNullOrWhiteSpace(objImmunizationHx.LotNumber))
                                ImmunizationHxHTML += " Lot Number: " + objImmunizationHx.LotNumber + ",";
                            if (!string.IsNullOrWhiteSpace(objImmunizationHx.ManufacturerName))
                                ImmunizationHxHTML += " Manufacturer: " + objImmunizationHx.ManufacturerName + ",";
                            if (!string.IsNullOrWhiteSpace(objImmunizationHx.ProviderName))
                                ImmunizationHxHTML += " administrated  by " + objImmunizationHx.ProviderName;
                            if (objImmunizationHx.AdministrationDate != null)
                                ImmunizationHxHTML += " on " + objImmunizationHx.AdministrationDate.ToString("MM/dd/yyyy, hh:mm:ss tt");
                            ImmunizationHxHTML += " </li>";
                            string Comments = NotesCommentContents(objListNotesComponent, "Immunization_" + objImmunizationHx.ImmTherInjectionId + "thera");
                            if (!string.IsNullOrWhiteSpace(Comments))
                            {
                                ImmunizationHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                            }
                        }
                    }
                }

                ImmunizationHxHTML += "</ul>";
            }
            return ImmunizationHxHTML;
        }

        public string NotesMedicationHx(List<MedicationHx> objListMedicationHx, List<NotesComponent> objListNotesComponent)
        {
            var MedicationHxHTML = string.Empty;
            MedicationHxHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            MedicationHxHTML += "<li style='color: #0088cc;'><b>Medications </b> </li>";
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Medications");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                MedicationHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            if (objListMedicationHx != null && objListMedicationHx.Count() > 0)
            {
                //MedicationHx objMedicationHxCur = objListMedicationHx.FirstOrDefault(m => m.StopDate == null || Convert.ToDateTime(m.StopDate).Date >= m.CreatedOn.Date);
                if (objListMedicationHx.Where(m => (m.StopDate == null || Convert.ToDateTime(m.StopDate).Date >= DateTime.Now.Date) && m.IsDeleted == false && m.IsActive == true).Count() > 0)
                {
                    MedicationHxHTML += "<li style='color: #0088cc;'><b>Current Medications </b> </li>";
                    foreach (MedicationHx objMedicationHx in objListMedicationHx.Where(m => (m.StopDate == null || Convert.ToDateTime(m.StopDate).Date >= DateTime.Now.Date) && m.IsDeleted == false && m.IsActive == true))
                    {
                        MedicationHxHTML += "<li>";
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.BrandName))
                        {
                            MedicationHxHTML += " <b>" + System.Web.HttpUtility.HtmlEncode(objMedicationHx.BrandName) + " </b>";
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.GenericName))
                        {
                            MedicationHxHTML += " <b>(" + System.Web.HttpUtility.HtmlEncode(objMedicationHx.GenericName) + ") </b>";
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Strength))
                        {
                            MedicationHxHTML += " <b>" + objMedicationHx.Strength + " </b>";
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Form))
                        {
                            MedicationHxHTML += " <b>" + objMedicationHx.Form + " </b>";
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Action))
                        {
                            MedicationHxHTML += " take " + objMedicationHx.Action;
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Dose))
                        {
                            MedicationHxHTML += " " + System.Web.HttpUtility.HtmlEncode(objMedicationHx.Dose);
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.DoseUnit))
                        {
                            MedicationHxHTML += " " + objMedicationHx.DoseUnit;
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Routeby))
                        {
                            MedicationHxHTML += " " + objMedicationHx.Routeby;
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.DoseTiming))
                        {
                            MedicationHxHTML += " " + objMedicationHx.DoseTiming;
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.DoseOther))
                        {
                            MedicationHxHTML += " " + System.Web.HttpUtility.HtmlEncode(objMedicationHx.DoseOther);
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Duration))
                        {
                            MedicationHxHTML += " for " + objMedicationHx.Duration + " days,";
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Quantity))
                        {
                            MedicationHxHTML += " Quantity " + objMedicationHx.Quantity;
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.QuantityUnit))
                        {
                            MedicationHxHTML += " " + objMedicationHx.QuantityUnit + "(s)";
                        }
                        MedicationHxHTML += " </li>";
                        string Comments = NotesCommentContents(objListNotesComponent, "Medication_" + objMedicationHx.MedicationID);
                        if (!string.IsNullOrWhiteSpace(Comments))
                        {
                            MedicationHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                        }
                    }
                }
                //m.StopDate.Date < m.CreatedOn.Date
                //MedicationHx objMedicationHxPast = objListMedicationHx.FirstOrDefault(m => m.StopDate.Date < m.CreatedOn.Date);
                if (objListMedicationHx.Where(m => ((m.StopDate == null || Convert.ToDateTime(m.StopDate).Date <= DateTime.Now.Date) && ((m.IsDeleted == true && m.IsActive == true) || (m.IsDeleted == false && m.IsActive == false) || (m.IsDeleted == true && m.IsActive == false))) || (m.StopDate == null || Convert.ToDateTime(m.StopDate).Date < DateTime.Now.Date) && (m.IsDeleted == false && m.IsActive == true)).Count() > 0)
                {
                    MedicationHxHTML += "<li style='color: #bd0e09;'><b>Past Medications </b> </li>";
                    foreach (MedicationHx objMedicationHx in objListMedicationHx.Where(m => ((m.StopDate == null || Convert.ToDateTime(m.StopDate).Date <= DateTime.Now.Date) && ((m.IsDeleted == true && m.IsActive == true) || (m.IsDeleted == false && m.IsActive == false) || (m.IsDeleted == true && m.IsActive == false))) || (m.StopDate != null && Convert.ToDateTime(m.StopDate).Date < DateTime.Now.Date) && (m.IsDeleted == false && m.IsActive == true)))
                    {
                        MedicationHxHTML += "<li>";
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.BrandName))
                        {
                            MedicationHxHTML += " <b>" + System.Web.HttpUtility.HtmlEncode(objMedicationHx.BrandName) + " </b>";
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.GenericName))
                        {
                            MedicationHxHTML += " <b>(" + System.Web.HttpUtility.HtmlEncode(objMedicationHx.GenericName) + ") </b>";
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Strength))
                        {
                            MedicationHxHTML += " <b>" + objMedicationHx.Strength + " </b>";
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Form))
                        {
                            MedicationHxHTML += " <b>" + objMedicationHx.Form + " </b>";
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Action))
                        {
                            MedicationHxHTML += " take " + objMedicationHx.Action;
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Dose))
                        {
                            MedicationHxHTML += " " + System.Web.HttpUtility.HtmlEncode(objMedicationHx.Dose);
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.DoseUnit))
                        {
                            MedicationHxHTML += " " + objMedicationHx.DoseUnit;
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Routeby))
                        {
                            MedicationHxHTML += " " + objMedicationHx.Routeby;
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.DoseTiming))
                        {
                            MedicationHxHTML += " " + objMedicationHx.DoseTiming;
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.DoseOther))
                        {
                            MedicationHxHTML += " " + System.Web.HttpUtility.HtmlEncode(objMedicationHx.DoseOther);
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Duration))
                        {
                            MedicationHxHTML += " for " + objMedicationHx.Duration + " days,";
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.Quantity))
                        {
                            MedicationHxHTML += " Quantity " + objMedicationHx.Quantity;
                        }
                        if (!string.IsNullOrWhiteSpace(objMedicationHx.QuantityUnit))
                        {
                            MedicationHxHTML += " " + objMedicationHx.QuantityUnit + "(s)";
                        }
                        MedicationHxHTML += " </li>";
                    }
                }
            }
            else if ((objListMedicationHx == null || objListMedicationHx.Count() == 0) && string.IsNullOrWhiteSpace(upComment))
            {
                MedicationHxHTML += "<li> No Active Medications </li>";
            }
            MedicationHxHTML += "</ul>";

            return MedicationHxHTML;
        }

        public string NotesAlleryHx(List<AllergyHx> objListAllergyHx, List<NotesComponent> objListNotesComponent)
        {
            var AlleryHxHTML = string.Empty;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Allergies");

            AlleryHxHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            AlleryHxHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Allergies </b> </li>";
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                AlleryHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            if (objListAllergyHx != null && objListAllergyHx.Count() > 0)
            {
                foreach (AllergyHx objAllergyHx in objListAllergyHx)
                {
                    AlleryHxHTML += "<li>";
                    if (!string.IsNullOrWhiteSpace(objAllergyHx.Allergen))
                    {
                        AlleryHxHTML += " <b>" + System.Web.HttpUtility.HtmlEncode(objAllergyHx.Allergen) + " </b>";
                    }
                    if (!string.IsNullOrWhiteSpace(objAllergyHx.InActiveCheckBoxValue))
                    {
                        AlleryHxHTML += " <span style='color:red'> (Inactive) </span>,";
                    }
                    if (objAllergyHx.OnSetDate != null)
                    {
                        AlleryHxHTML += " OnSet: " + Convert.ToDateTime(objAllergyHx.OnSetDate).ToString("MM/dd/yyyy") + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objAllergyHx.Reaction))
                    {
                        AlleryHxHTML += " Reaction: " + System.Web.HttpUtility.HtmlEncode(objAllergyHx.Reaction) + ",";
                    }
                    AlleryHxHTML += " </li>";
                    string Comments = NotesCommentContents(objListNotesComponent, "Allergy_" + objAllergyHx.AllergyId);
                    if (!string.IsNullOrWhiteSpace(Comments))
                    {
                        AlleryHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                    }
                }
            }
            else if ((objListAllergyHx == null || objListAllergyHx.Count() == 0) && string.IsNullOrWhiteSpace(upComment))
            {
                AlleryHxHTML += "<li>No Known Drug Allergies (NKDA). </li>";
            }
            AlleryHxHTML += "</ul>";
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            string ReviewdByDivId = "Cli_Allergies_ReviewByAllergy";
            string ReviewedByHTML = string.Empty;
            foreach (NotesComponent obj in objListNotesComponent)
            {
                document.LoadHtml(obj.SOAPText);
                ReviewedByHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                try
                {
                    foreach (HtmlAgilityPack.HtmlNode div in document.DocumentNode.SelectNodes("//section[@id='" + ReviewdByDivId + "']"))
                    {
                        ReviewedByHTML += div.InnerHtml;
                    }
                }
                catch (Exception ex)
                {
                    ReviewedByHTML += "<li> </li>";
                }
                ReviewedByHTML += "</ul>";
            }
            AlleryHxHTML += ReviewedByHTML;
            return AlleryHxHTML;
        }

        public string NotesBirthHx(List<BirthHx> objListBirthHx, List<NotesComponent> objListNotesComponent)
        {
            var BirthHxHTML = string.Empty;
            BirthHxHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            BirthHxHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Birth Hx </b> </li>";
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "BirthHx");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                BirthHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            if ((objListBirthHx == null || objListBirthHx.Count() == 0) && string.IsNullOrWhiteSpace(upComment))
            {
                BirthHxHTML += "<li>No Birth History. </li>";
            }
            else if (objListBirthHx != null && objListBirthHx.Count() > 0)
            {
                foreach (BirthHx objbirthHx in objListBirthHx.Where(m => m.BirthHxName == "Unremarkable"))
                {
                    BirthHxHTML += "<li>Unremarkable </li>";
                }
                BirthHx objBirthHxGenaral = objListBirthHx.FirstOrDefault(m => m.ID == 1);

                if (objBirthHxGenaral != null)
                {
                    BirthHxHTML += "<li><b>General: </b>";
                    if (!string.IsNullOrWhiteSpace(objBirthHxGenaral.HospitalName))
                    {
                        BirthHxHTML += " The patient was born at " + System.Web.HttpUtility.HtmlEncode(objBirthHxGenaral.HospitalName) + ".";
                    }
                    if (objBirthHxGenaral.PatientDOB != null)
                    {
                        BirthHxHTML += " Patient's DOB is " + Convert.ToDateTime(objBirthHxGenaral.PatientDOB).ToString("MM/dd/yyyy") + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxGenaral.LengthStayatHospital))
                    {
                        BirthHxHTML += " Length of Stay at the Hospital was " + System.Web.HttpUtility.HtmlEncode(objBirthHxGenaral.LengthStayatHospital) + " days.";
                    }
                    if (objBirthHxGenaral.DateAdmitted != null)
                    {
                        BirthHxHTML += " Date of Admission was " + Convert.ToDateTime(objBirthHxGenaral.DateAdmitted).ToString("MM/dd/yyyy") + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxGenaral.ObstetricianName))
                    {
                        BirthHxHTML += " The Obstetrician was " + objBirthHxGenaral.ObstetricianName + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxGenaral.PediatricianName))
                    {
                        BirthHxHTML += " The Pediatrician was " + objBirthHxGenaral.PediatricianName + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxGenaral.ResponsiblePhysicianName))
                    {
                        BirthHxHTML += " The Responsible Physician was " + objBirthHxGenaral.ResponsiblePhysicianName + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxGenaral.Comments))
                    {
                        BirthHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objBirthHxGenaral.Comments) + ".";
                    }
                    BirthHxHTML += " </li>";
                }
                BirthHx objBirthHxMD = objListBirthHx.FirstOrDefault(m => m.ID == 2);
                if (objBirthHxMD != null)
                {
                    BirthHxHTML += "<li><b>" + HttpUtility.HtmlEncode("Maternal & Delivery") + ": </b>";
                    if (!string.IsNullOrWhiteSpace(objBirthHxMD.Gestation))
                    {
                        BirthHxHTML += " Gestation: " + objBirthHxMD.Gestation + " weeks.";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxMD.NumberOfFetuses))
                    {
                        BirthHxHTML += " Number of Fetuses: " + objBirthHxMD.NumberOfFetuses + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxMD.NumberOfLivingFetuses))
                    {
                        BirthHxHTML += " Number of Living Fetuses: " + objBirthHxMD.NumberOfLivingFetuses + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxMD.LaborLength))
                    {
                        BirthHxHTML += " Labor Length: " + objBirthHxMD.LaborLength + " Hours.";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxMD.DeliveryMethod))
                    {
                        BirthHxHTML += " Delivery Method: " + objBirthHxMD.DeliveryMethod + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxMD.DeliveryPresentation))
                    {
                        BirthHxHTML += " Delivery Presentation: " + objBirthHxMD.DeliveryPresentation + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxMD.MaternalHistory))
                    {
                        BirthHxHTML += " Maternal History: " + objBirthHxMD.MaternalHistory + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxMD.Comments))
                    {
                        BirthHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objBirthHxMD.Comments) + ".";
                    }
                    BirthHxHTML += " </li>";
                }
                BirthHx objBirthHxNI = objListBirthHx.FirstOrDefault(m => m.ID == 3);
                if (objBirthHxNI != null)
                {
                    BirthHxHTML += "<li><b>Newborn Information: </b>";
                    if (!string.IsNullOrWhiteSpace(objBirthHxNI.HeadCircumference))
                    {
                        BirthHxHTML += " Head Circumference is: " + objBirthHxNI.HeadCircumference + " inches.";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxNI.ChestCircumference))
                    {
                        BirthHxHTML += " Chest Circumference is: " + objBirthHxNI.ChestCircumference + " inches.";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxNI.WeightAtBirth))
                    {
                        BirthHxHTML += " Weight at Birth: " + objBirthHxNI.WeightAtBirth + " lbs.";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxNI.LengthAtBirth))
                    {
                        BirthHxHTML += " Length at Birth: " + objBirthHxNI.LengthAtBirth + " inches.";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxNI.ApgarAtBirth))
                    {
                        BirthHxHTML += " Apgar at Birth: " + objBirthHxNI.ApgarAtBirth + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxNI.ApgarAt5Minutes))
                    {
                        BirthHxHTML += "  Apgar at 5 Minutes: " + objBirthHxNI.ApgarAt5Minutes + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxNI.WeightReleased))
                    {
                        BirthHxHTML += " Weight Released: " + objBirthHxNI.WeightReleased + " lbs.";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxNI.PatientBloodType))
                    {
                        BirthHxHTML += " Patient's Blood Type: " + objBirthHxNI.PatientBloodType + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxNI.ProblemsAtBirth))
                    {
                        BirthHxHTML += " Problems at Birth: " + objBirthHxNI.ProblemsAtBirth + ".";
                    }
                    if (objBirthHxNI.bFetalDistress)
                    {
                        BirthHxHTML += " Fetal Distress: Present.";
                    }
                    else
                    {
                        BirthHxHTML += " Fetal Distress: Absent.";
                    }
                    if (!string.IsNullOrWhiteSpace(objBirthHxNI.Comments))
                    {
                        BirthHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objBirthHxNI.Comments) + ".";
                    }
                    BirthHxHTML += " </li>";
                }
                BirthHx obj = objListBirthHx.FirstOrDefault();
                if (obj != null)
                {
                    if (obj.CreatedOn == obj.ModifiedOn)
                        BirthHxHTML += "<li>Added On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                    else
                        BirthHxHTML += "<li>Last Updated On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                }
                string Comments = NotesCommentContents(objListNotesComponent, "BirthHx_" + obj.BirthHxId);
                if (!string.IsNullOrWhiteSpace(Comments))
                {
                    BirthHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                }
            }
            BirthHxHTML += "</ul>";

            return BirthHxHTML;
        }

        public string NotesMedicalHx(List<MedicalHx> objListMedicalHx, List<NotesComponent> objListNotesComponent)
        {
            var MedicalHxHTML = string.Empty;
            MedicalHxHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            MedicalHxHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Medical Hx </b> </li>";
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "MedicalHx");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                MedicalHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            if ((objListMedicalHx == null || objListMedicalHx.Count() == 0) && string.IsNullOrWhiteSpace(upComment))
            {
                MedicalHxHTML += "<li>No Medical History. </li>";
            }
            else if (objListMedicalHx != null && objListMedicalHx.Count() > 0)
            {
                int counter = 0;
                MedicalHxHTML += "<li>";
                foreach (MedicalHx obj_MedicalHx in objListMedicalHx)
                {
                    if (obj_MedicalHx.bUnremarkable == true)
                    {
                        MedicalHxHTML += "<li>Unremarkable </li>";

                    }
                }
                foreach (MedicalHx obj_MedicalHx in objListMedicalHx)
                {
                    if (counter == 0 && !string.IsNullOrWhiteSpace(obj_MedicalHx.CPTCodeDescription))
                    {
                        MedicalHxHTML += "The patient underwent";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(obj_MedicalHx.CPTCodeDescription))
                            MedicalHxHTML += " The patient also underwent";
                    }
                    if (!string.IsNullOrWhiteSpace(obj_MedicalHx.CPTCodeDescription))
                    {
                        MedicalHxHTML += " <b>" + System.Web.HttpUtility.HtmlEncode(obj_MedicalHx.CPTCodeDescription) + " </b>,";
                    }
                    if (!string.IsNullOrWhiteSpace(obj_MedicalHx.ICD10CodeDescription) && !string.IsNullOrWhiteSpace(obj_MedicalHx.CPTCodeDescription))
                    {
                        //PRD-269 by:MAHmAD
                        MedicalHxHTML += " based on the following assessment: " + System.Web.HttpUtility.HtmlEncode(obj_MedicalHx.ICD10CodeDescription);
                        //PRD-269 by:MAHmAD
                    }
                    else
                    {
                        string mhICD10CodeDescription = string.IsNullOrWhiteSpace(obj_MedicalHx.ICD10CodeDescription) ? "" : System.Web.HttpUtility.HtmlEncode(obj_MedicalHx.ICD10CodeDescription);
                        if (counter == 0)
                            MedicalHxHTML += "" + mhICD10CodeDescription;
                        else
                            MedicalHxHTML += ", " + mhICD10CodeDescription;
                    }
                    if (!string.IsNullOrWhiteSpace(obj_MedicalHx.Status))
                    {
                        MedicalHxHTML += " (" + obj_MedicalHx.Status + "),";
                    }
                    if (obj_MedicalHx.FromDate != null)
                    {
                        MedicalHxHTML += " from " + Convert.ToDateTime(obj_MedicalHx.FromDate).ToString("MM/dd/yyyy");
                    }
                    if (obj_MedicalHx.ToDate != null)
                    {
                        MedicalHxHTML += " to " + Convert.ToDateTime(obj_MedicalHx.ToDate).ToString("MM/dd/yyyy") + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(obj_MedicalHx.TestResult))
                    {
                        MedicalHxHTML += " The Test Result is " + System.Web.HttpUtility.HtmlEncode(obj_MedicalHx.TestResult) + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(obj_MedicalHx.Onset))
                    {
                        MedicalHxHTML += " Onset is " + obj_MedicalHx.Onset + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(obj_MedicalHx.Duration))
                    {
                        MedicalHxHTML += " Duration is " + " " + (string.IsNullOrWhiteSpace(obj_MedicalHx.DurationLength) ? "" : obj_MedicalHx.DurationLength) + " " + obj_MedicalHx.Duration + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(obj_MedicalHx.Severity))
                    {
                        MedicalHxHTML += " Severity is " + obj_MedicalHx.Severity + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(obj_MedicalHx.Pattern))
                    {
                        MedicalHxHTML += " Pattern is " + obj_MedicalHx.Pattern + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(obj_MedicalHx.AggravatedBy))
                    {
                        MedicalHxHTML += " Aggravated By " + obj_MedicalHx.AggravatedBy + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(obj_MedicalHx.Location))
                    {
                        MedicalHxHTML += " Location is " + System.Web.HttpUtility.HtmlEncode(obj_MedicalHx.Location) + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(obj_MedicalHx.Comments))
                    {
                        MedicalHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(obj_MedicalHx.Comments) + ".";
                    }
                    counter++;
                }
                MedicalHxHTML += " </li>";
                MedicalHx obj = objListMedicalHx.FirstOrDefault();
                if (obj != null)
                {
                    if (!string.IsNullOrWhiteSpace(obj.OverAllComments))
                        MedicalHxHTML += "<li> " + System.Web.HttpUtility.HtmlEncode(obj.OverAllComments) + " </li>";
                    if (obj.CreatedOn == obj.ModifiedOn)
                        MedicalHxHTML += "<li>Added On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                    else
                        MedicalHxHTML += "<li>Last Updated On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                }
                string Comments = NotesCommentContents(objListNotesComponent, "MedicalHx_" + obj.MedicalHxId);
                if (!string.IsNullOrWhiteSpace(Comments))
                {
                    MedicalHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                }
            }
            MedicalHxHTML += "</ul>";
            return MedicalHxHTML;
        }

        public string NotesSocialHx(List<SocialHx> objListSocialHx, List<NotesComponent> objListNotesComponent)
        {
            var SocialHxHTML = string.Empty;
            SocialHxHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            SocialHxHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;padding-left: 0px !important;'><b>Social Hx </b> </li>";

            string upComment = NotesCommentContentsUpper(objListNotesComponent, "SocialHx");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                SocialHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            if (objListSocialHx != null && objListSocialHx.Count() > 0)
            {
                SocialHxHTML += "<li>";
                foreach (SocialHx objSocialHx in objListSocialHx.Where(m => m.HistoryType == "Unremarkable"))
                {
                    SocialHxHTML += "Unremarkable";
                }
                foreach (SocialHx objSocialHx in objListSocialHx.Where(m => m.HistoryType == "Tobacco" || m.HistoryType == "Alcohol"))
                {
                    SocialHxHTML += "<b>" + objSocialHx.HistoryType + " </b>";
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Description))
                    {
                        SocialHxHTML += " " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Description) + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Type))
                    {
                        SocialHxHTML += " " + objSocialHx.Type + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Frequency))
                    {
                        SocialHxHTML += " " + objSocialHx.Frequency;
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.UsagePeriod))
                    {
                        SocialHxHTML += " for " + objSocialHx.UsagePeriod + ",";
                    }
                    else
                    {
                        SocialHxHTML += ",";
                    }
                    if ((!string.IsNullOrWhiteSpace(objSocialHx.bNotReadyToQuit) && objSocialHx.bNotReadyToQuit != "0") || (!string.IsNullOrWhiteSpace(objSocialHx.bWouldQuit) && objSocialHx.bWouldQuit != "0") || (!string.IsNullOrWhiteSpace(objSocialHx.bRecentlyQuit) && objSocialHx.bRecentlyQuit != "0"))
                    {
                        SocialHxHTML += " Patient ";
                        if (objSocialHx.bNotReadyToQuit == "1")
                        {
                            SocialHxHTML += "not ready to quit,";
                        }
                        else if (objSocialHx.bWouldQuit == "1")
                        {
                            SocialHxHTML += "would quit,";
                        }
                        else if (objSocialHx.bRecentlyQuit == "1")
                        {
                            SocialHxHTML += "recently quit,";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.CounsellingPeriod))
                    {
                        SocialHxHTML += " Counselling: " + objSocialHx.CounsellingPeriod;
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.CounsellingTopic))
                    {
                        SocialHxHTML += " for " + objSocialHx.CounsellingTopic + ",";
                    }
                    else
                    {
                        SocialHxHTML += ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Comments))
                    {
                        SocialHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Comments);
                    }
                    SocialHxHTML += "<br/>";
                }
                foreach (SocialHx objSocialHx in objListSocialHx.Where(m => m.HistoryType == "Drug Abuse"))
                {
                    SocialHxHTML += "<b>" + objSocialHx.HistoryType + " </b>";
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Description))
                    {
                        SocialHxHTML += " " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Description);
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Drug))
                    {
                        SocialHxHTML += " of " + objSocialHx.Drug;
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Route))
                    {
                        SocialHxHTML += " by " + objSocialHx.Route + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Frequency))
                    {
                        SocialHxHTML += " for " + objSocialHx.Frequency;
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.UsagePeriod))
                    {
                        SocialHxHTML += " " + objSocialHx.UsagePeriod + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.CessationLength))
                    {
                        SocialHxHTML += " Patient has quit " + objSocialHx.CessationLength;
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.CessationPeriod))
                    {
                        SocialHxHTML += " " + objSocialHx.CessationPeriod + " ago,";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Comments))
                    {
                        SocialHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Comments);
                    }
                    SocialHxHTML += "<br/>";
                }

                foreach (SocialHx objSocialHx in objListSocialHx.Where(m => m.HistoryType == "Sexual Hx"))
                {
                    SocialHxHTML += "<b>" + objSocialHx.HistoryType + " </b>";
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Description))
                    {
                        SocialHxHTML += " " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Description) + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Preference))
                    {
                        SocialHxHTML += " Prefers " + objSocialHx.Preference + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.bUSingProtection))
                    {
                        SocialHxHTML += " Using Protection: ";
                        if (objSocialHx.bUSingProtection == "1")
                        {
                            SocialHxHTML += "Yes,";
                        }
                        else
                        {
                            SocialHxHTML += "No,";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.ProtectionMethod))
                    {
                        SocialHxHTML += " Method: " + objSocialHx.ProtectionMethod + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.ProtectionPeriod))
                    {
                        SocialHxHTML += " How often: " + objSocialHx.ProtectionPeriod + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Complaint))
                    {
                        SocialHxHTML += " Patient has complaints of  " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Complaint) + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.bExposedToSTD))
                    {
                        SocialHxHTML += " Exposed to STD: ";
                        if (objSocialHx.bExposedToSTD == "1")
                        {
                            SocialHxHTML += "Yes,";
                        }
                        else
                        {
                            SocialHxHTML += "No,";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.STDIds))
                    {
                        SocialHxHTML += " STD: " + objSocialHx.STDIds + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.bPainWithIntercourse))
                    {
                        SocialHxHTML += " Experiences pain with intercourse: ";
                        if (objSocialHx.bPainWithIntercourse == "1")
                        {
                            SocialHxHTML += "Yes,";
                        }
                        else
                        {
                            SocialHxHTML += "No,";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.bPregnancyStatus))
                    {
                        if (objSocialHx.bPregnancyStatus == "1")
                        {
                            SocialHxHTML += " Pregnant";
                        }
                        else
                        {
                            SocialHxHTML += " Not pregnant";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.PregnancyDuration))
                    {
                        SocialHxHTML += " since " + objSocialHx.PregnancyDuration;
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.bSexuallyAbused))
                    {
                        SocialHxHTML += ", Abused Sexually: ";
                        if (objSocialHx.bSexuallyAbused == "1")
                        {
                            SocialHxHTML += "Yes,";
                        }
                        else
                        {
                            SocialHxHTML += "No,";
                        }
                    }
                    if (objSocialHx.LMP != null)
                    {
                        SocialHxHTML += " Last Menstrual Period is " + Convert.ToDateTime(objSocialHx.LMP).ToString("MM/dd/yyyy") + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Comments))
                    {
                        SocialHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Comments);
                    }
                    SocialHxHTML += "<br/>";
                }

                foreach (SocialHx objSocialHx in objListSocialHx.Where(m => m.HistoryType == "Occupation"))
                {
                    SocialHxHTML += "<b>Occupation </b>";
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Description))
                    {
                        SocialHxHTML += " Patient " + objSocialHx.Description.ToLower();
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Present))
                    {
                        SocialHxHTML += " Present: " + objSocialHx.Present + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Past))
                    {
                        SocialHxHTML += " Past: " + objSocialHx.Past + ",";
                    }
                    if (objSocialHx.occupationStartDate != DateTime.MinValue && objSocialHx.occupationEndDate != DateTime.MinValue)
                    {
                        SocialHxHTML += " from " + objSocialHx.occupationStartDate.ToString("MM/dd/yyyy") + " to " + objSocialHx.occupationEndDate.ToString("MM/dd/yyyy") + ",";
                    }
                    else if (objSocialHx.occupationStartDate != DateTime.MinValue)
                    {
                        SocialHxHTML += " since " + objSocialHx.occupationStartDate.ToString("MM/dd/yyyy") + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Comments))
                    {
                        SocialHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Comments);
                    }
                    SocialHxHTML += "<br/>";
                }

                foreach (SocialHx objSocialHx in objListSocialHx.Where(m => m.HistoryType == "Sleep"))
                {
                    SocialHxHTML += "<b>Sleep </b>";
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Description))
                    {
                        SocialHxHTML += " Patient sleeps " + objSocialHx.Description + ",";
                    }
                    //if (!string.IsNullOrWhiteSpace(objSocialHx.SleepHours))
                    //{
                    SocialHxHTML += " Sleeping hours are" + objSocialHx.SleepHours + ", ";
                    //}
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Comments))
                    {
                        SocialHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Comments);
                    }
                    SocialHxHTML += "<br/>";
                }
                foreach (SocialHx objSocialHx in objListSocialHx.Where(m => m.HistoryType == "Exercises"))
                {
                    SocialHxHTML += "<b>Exercises </b>";
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Description))
                    {
                        SocialHxHTML += " Patient exercises " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Description) + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Type))
                    {
                        SocialHxHTML += " with " + objSocialHx.Type + " intensity.";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Diet))
                    {
                        SocialHxHTML += " Patient diet is " + objSocialHx.Diet + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Comments))
                    {
                        SocialHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Comments);
                    }
                    SocialHxHTML += "<br/>";
                }
                foreach (SocialHx objSocialHx in objListSocialHx.Where(m => m.HistoryType == "Housing"))
                {
                    SocialHxHTML += "<b>Housing </b>";
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Description))
                    {
                        SocialHxHTML += " Housing status: " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Description) + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Present))
                    {
                        SocialHxHTML += " Present: " + objSocialHx.Present + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Past))
                    {
                        SocialHxHTML += " Past: " + objSocialHx.Past + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Comments))
                    {
                        SocialHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Comments) + ".";
                    }
                    SocialHxHTML += "<br/>";
                }
                foreach (SocialHx objSocialHx in objListSocialHx.Where(m => m.HistoryType == "CaffeineIntak"))
                {
                    SocialHxHTML += "<b>Caffeine Intake </b>";
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Description))
                    {
                        if (objSocialHx.Description.ToLower() == "does not use")
                            SocialHxHTML += " Patient does not intake caffeine " + ".";
                        else
                            SocialHxHTML += " Patient intakes caffeine " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Description) + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Frequency))
                    {
                        SocialHxHTML += " Frequency: " + objSocialHx.Frequency + ".";
                    }
                    if (!string.IsNullOrWhiteSpace(objSocialHx.Comments))
                    {
                        SocialHxHTML += " Comments: " + System.Web.HttpUtility.HtmlEncode(objSocialHx.Comments) + ".";
                    }
                    SocialHxHTML += "<br/>";
                }
                SocialHxHTML += " </li>";



                SocialHx obj = objListSocialHx.FirstOrDefault();
                if (obj != null && !string.IsNullOrWhiteSpace(obj.OverAllComments))
                {
                    string OverAllComments = NotesComponentInnerHTML(obj.OverAllComments, "overallcomment");
                    SocialHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + OverAllComments + " </li>";
                }
                if (obj != null)
                {
                    if (obj.CreatedOn == obj.ModifiedOn)
                        SocialHxHTML += "<li>Added On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                    else
                        SocialHxHTML += "<li>Last Updated On " + obj.ModifiedOn.ToString("MM/dd/yyyy") + " </li>";
                }

                string Comments = NotesCommentContents(objListNotesComponent, "SocialHx_" + obj.SocialHxId);
                if (!string.IsNullOrWhiteSpace(Comments))
                {
                    SocialHxHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + System.Web.HttpUtility.HtmlEncode(Comments) + " </li>";
                }
            }
            else if ((objListSocialHx == null || objListSocialHx.Count() == 0) && string.IsNullOrWhiteSpace(upComment))
            {
                SocialHxHTML += "<li>No Social History. </li>";
            }
            SocialHxHTML += "</ul>";
            return SocialHxHTML;
        }

        public string NotesComplaints(List<Complaints> objListComplaints, List<NotesComponent> objListNotesComponent, Int64 NoteID)
        {
            var ComplainsHTML = string.Empty;
            var OverAllComments = string.Empty;
            var Comments = string.Empty;
            bool IsNonHpiData = false;
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Complaints");
            if ((objListComplaints != null && objListComplaints.Count() > 0) || !string.IsNullOrWhiteSpace(upComment))
            {
                ComplainsHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                ComplainsHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Complaints </b> </li>";

                if (!string.IsNullOrWhiteSpace(upComment))
                {
                    ComplainsHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
                }
                if (objListComplaints != null && objListComplaints.Count() > 0)
                {
                    foreach (Complaints objComplaints in objListComplaints)
                    {
                        if (!string.IsNullOrWhiteSpace(objComplaints.ComplaintDescription))
                            ComplainsHTML += "<li>" + System.Web.HttpUtility.HtmlEncode(objComplaints.ComplaintDescription) + " </li>";
                        else
                            ComplainsHTML += "<li>&nbsp; </li>";
                    }
                    int counter = 0;
                    ComplainsHTML += "<li style='color: #0088cc;'><b>History of Present Illness </b> </li>";
                    ComplainsHTML += "<li>";
                    string detail = "";
                    foreach (Complaints objComplaints in objListComplaints)
                    {
                        detail = "";
                        if (counter == 0)
                        {
                            string Age = CalculatePatientAge(Convert.ToDateTime(objComplaints.DOB));
                            ComplainsHTML += "A " + Age + " old " + objComplaints.Gender + " presents with ";
                        }
                        else if(objListComplaints.Count > 1 && counter == (objListComplaints.Count() - 1))
                            ComplainsHTML += " and ";
                        else
                        {
                            ComplainsHTML += "";
                        }
                        
                        
                        if (!string.IsNullOrWhiteSpace(objComplaints.PreviousHistory))
                        {
                            detail += " Previous History: " + System.Web.HttpUtility.HtmlEncode(objComplaints.PreviousHistory) + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.Case))
                        {
                            detail += " Case is " + objComplaints.Case + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.Location))
                        {
                            detail += " Location: " + objComplaints.Location + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.Radiation))
                        {
                            detail += " Radiates to: " + objComplaints.Radiation + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.Quality))
                        {
                            detail += " Quality is " + objComplaints.Quality + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.Severity))
                        {
                            detail += " Severity is " + objComplaints.Severity + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.Onset))
                        {
                            detail += " Onset: " + objComplaints.Onset + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.Duration))
                        {
                            detail += " Duration is " + System.Web.HttpUtility.HtmlEncode(objComplaints.Duration) + " " + objComplaints.DurationDesc + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.Frequency))
                        {
                            detail += " Frequency is " + objComplaints.Frequency + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.Context))
                        {
                            detail += " Context: " + objComplaints.Context + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.Character))
                        {
                            detail += " Character is " + objComplaints.Character + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.AssociatedWith))
                        {
                            detail += " Associated With " + objComplaints.AssociatedWith + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.PrecipitatedBy))
                        {
                            detail += " Precipitated by " + objComplaints.PrecipitatedBy + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.AggravatedBy))
                        {
                            detail += " Aggravated by " + objComplaints.AggravatedBy + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.RelievedBy))
                        {
                            detail += " Relieved by " + objComplaints.RelievedBy + ".";
                        }
                        if (!string.IsNullOrWhiteSpace(objComplaints.Comments))
                        {
                            objComplaints.Comments = objComplaints.Comments.Replace("<br>", "< br />");
                            objComplaints.Comments = objComplaints.Comments.Replace("</div><div>", "<br/>");
                            objComplaints.Comments = objComplaints.Comments.Replace("<div>", "<br/>");
                            objComplaints.Comments = objComplaints.Comments.Replace("</div>", "<br/>");
                            detail += " " + objComplaints.Comments;
                        }
                        counter++;
                        string formattedDetail = "";
                        if (!string.IsNullOrWhiteSpace(detail.Trim()))
                        {
                            string lastch = (counter == objListComplaints.Count() ? "." : ";");
                            formattedDetail = "<b>(</b>" + detail.Trim() + "<b>)" + lastch + "</b>";
                        }
                        string endingLiteral = "";
                        if (string.IsNullOrEmpty(detail.Trim()) )
                            endingLiteral = ";";
                        else if (string.IsNullOrEmpty(detail.Trim()) && objListComplaints.Count > 1 && counter == objListComplaints.Count())
                            endingLiteral = ".";
                        if (string.IsNullOrEmpty(detail.Trim()) && counter == objListComplaints.Count())
                            endingLiteral = ".";
                        ComplainsHTML += "<b>" + System.Web.HttpUtility.HtmlEncode(objComplaints.ComplaintDescription) + endingLiteral + " </b>";
                        ComplainsHTML += formattedDetail;
                    }
                    

                    ComplainsHTML += " </li>";
                    if (!string.IsNullOrWhiteSpace(objListComplaints.FirstOrDefault().OverallComments))
                    {
                        OverAllComments = objListComplaints.FirstOrDefault().OverallComments;
                        // ComplainsHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + objListComplaints.FirstOrDefault().OverallComments + " </li>";
                    }
                    Comments = NotesCommentContents(objListNotesComponent, "Complaint_" + objListComplaints.FirstOrDefault().ComplaintId);
                    if (!string.IsNullOrWhiteSpace(Comments))
                    {
                        ComplainsHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li>";
                    }
                }
                ComplainsHTML += "</ul>";
                IsNonHpiData = true;
            }
            BLLClinical blClncl = new BLLClinical();
            List<ComplaintsHPI> HpiNotesList = new List<ComplaintsHPI>();
            HpiNotesList = blClncl.NotesComplaintsHPISelect(NoteID);
            if ((HpiNotesList != null && HpiNotesList.Count() > 0))
            {
                ComplainsHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                if (!IsNonHpiData)
                    ComplainsHTML += "<li style='color: #0088cc;'><b>Complaints </b> </li>";

                string CurrentTemplateName = "";
                int counter = 1;
                int TemplateLength = 0;
                foreach (ComplaintsHPI objComplaints in HpiNotesList)
                {
                    if (!string.IsNullOrWhiteSpace(objComplaints.HPITemplateName) && CurrentTemplateName != objComplaints.HPITemplateName)
                    {
                        TemplateLength = HpiNotesList.Where(x => x.HPITemplateName == objComplaints.HPITemplateName).ToList().Count();
                        ComplainsHTML += "<li style='padding-top:1px;padding-bottom:1px; color:#4b0575;'>" + System.Web.HttpUtility.HtmlEncode(objComplaints.HPITemplateName) + " </li>";
                    }
                    if (!string.IsNullOrWhiteSpace(objComplaints.HPISymptomName))
                        ComplainsHTML += "<li style='padding-top:1px;padding-bottom:1px; color:green;'>" + System.Web.HttpUtility.HtmlEncode(objComplaints.HPISymptomName) + " </li>";
                    ComplainsHTML += "<li>" + objComplaints.Description.Replace("div", "span") + " </li>";

                    if (counter == TemplateLength)
                    {
                        if (!string.IsNullOrWhiteSpace(objComplaints.Comments))
                            ComplainsHTML += "<li style='padding-top:-5px!important;padding-bottom:5px;'>" + System.Web.HttpUtility.HtmlEncode(objComplaints.Comments) + " </li>";
                        else
                            ComplainsHTML += "<li style='padding-top:-5px!important;padding-bottom:5px;'>&nbsp; </li>";
                        counter = 1;
                    }
                    else
                    {
                        counter++;
                    }
                    CurrentTemplateName = objComplaints.HPITemplateName;
                }
                ComplainsHTML += "</ul>";
            }

            if (!string.IsNullOrWhiteSpace(OverAllComments))
            {
                ComplainsHTML += "<ul style='list-style-type:none;padding-left: 0px ;!important;padding-top:0px!important;margin-top:0px!important;word-wrap: break-word;font-size:12px;'>";
                ComplainsHTML += "<li style='padding-top:-5px!important;padding-bottom:5px;'>" + System.Web.HttpUtility.HtmlEncode(OverAllComments) + " </li>";
                ComplainsHTML += "</ul>";
            }
            else if (HpiNotesList.Count > 0 && !string.IsNullOrWhiteSpace(HpiNotesList.FirstOrDefault().OverallComments))
            {
                ComplainsHTML += "<ul style='list-style-type:none;padding-left: 0px ;!important;padding-top:0px!important;margin-top:0px!important;word-wrap: break-word;font-size:12px;'>";
                ComplainsHTML += "<li style='padding-top:-5px!important;padding-bottom:5px;'>" + System.Web.HttpUtility.HtmlEncode(HpiNotesList.FirstOrDefault().OverallComments) + " </li>";
                ComplainsHTML += "</ul>";
            }

            return ComplainsHTML;
        }

        public string NotesTreatment(List<NotesComponent> objListNotesComponent, long NotesId)
        {
            string TreatmentHTML = string.Empty;
            string Referral_SoapText = string.Empty;
            string OverAllComments = string.Empty;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            TreatmentHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            TreatmentHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Treatment </b> </li>";
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Treatment");
            if (!string.IsNullOrWhiteSpace(upComment))
            {
                TreatmentHTML += "<li style='padding-top:5px;padding-bottom:5px;'>" + upComment + " </li>";
            }
            TreatmentHTML += "</ul>";
            foreach (NotesComponent obj in objListNotesComponent)
            {
                document.LoadHtml(obj.SOAPText);
                try
                {
                    foreach (HtmlAgilityPack.HtmlNode li in document.DocumentNode.SelectNodes("//li/div/section/ul/li"))
                    {

                        TreatmentHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                        TreatmentHTML += "<li>" + li.FirstChild.OuterHtml + " </li>";
                        TreatmentHTML += "</ul>";
                        TreatmentHTML += "<ul style='list-style-type:none;padding-left: 10px !important;word-wrap: break-word;font-size:12px;'>";
                        foreach (HtmlAgilityPack.HtmlNode div in li.LastChild.ChildNodes)
                        {
                            if (div.Id == "Lab_SoapText" || div.Id == "Radiology_SoapText")
                            {
                                foreach (HtmlAgilityPack.HtmlNode innerChildLab in div.ChildNodes)
                                {
                                    TreatmentHTML += "<li>" + innerChildLab.ChildNodes[0].InnerHtml + " </li>";
                                }
                            }
                            else if (div.Id == "Immunization_SoapText")
                            {
                                foreach (HtmlAgilityPack.HtmlNode innerNode in div.ChildNodes)
                                {
                                    foreach (var item in innerNode.ChildNodes)
                                    {
                                        if (item.OriginalName == "h6")
                                        {
                                            TreatmentHTML += "<li>" + item.OuterHtml + " </li>";
                                        }
                                        else if (item.OriginalName == "div")
                                        {
                                            foreach (var section in item.ChildNodes[0].ChildNodes)
                                            {
                                                TreatmentHTML += "<li>" + section.ChildNodes[0].InnerHtml + " </li>";
                                            }
                                        }
                                    }
                                }
                            }
                            else if (div.Id == "Referral_SoapText")
                            {
                                string padding, styles, separator;
                                foreach (HtmlAgilityPack.HtmlNode section in div.ChildNodes)
                                {
                                    foreach (var tbl in section.ChildNodes[0].ChildNodes[0].ChildNodes)
                                    {
                                        Referral_SoapText += "<table style='padding-left:8px;padding-right:8px;width:100%;word-wrap: break-word;font-size:12px;'>";

                                        foreach (var tr in tbl.ChildNodes[0].ChildNodes)
                                        {
                                            Referral_SoapText += "<tr>";
                                            foreach (var td in tr.ChildNodes)
                                            {
                                                if (td.ChildNodes.Count > 0 && td.FirstChild.Name == "strong")
                                                {
                                                    if (td.InnerText == "Procedure(s)" || td.InnerText == "Problem(s)")
                                                    {
                                                        padding = "vertical-align:top;padding-left:20px;padding-top:3px;padding-bottom:3px;";
                                                        styles = td.GetAttributeValue("style", null);
                                                        separator = (styles == null ? null : "; ");
                                                        td.SetAttributeValue("style", styles + separator + padding);
                                                    }
                                                    else
                                                    {
                                                        padding = "padding-left:20px;padding-top:3px;padding-bottom:3px;";
                                                        styles = td.GetAttributeValue("style", null);
                                                        separator = (styles == null ? null : "; ");
                                                        td.SetAttributeValue("style", styles + separator + padding);
                                                    }
                                                }
                                                else
                                                {
                                                    padding = "padding:3px;";
                                                    styles = td.GetAttributeValue("style", null);
                                                    separator = (styles == null ? null : "; ");
                                                    td.SetAttributeValue("style", styles + separator + padding);
                                                }
                                                Referral_SoapText += td.OuterHtml;
                                            }
                                            Referral_SoapText += "</tr>";
                                        }
                                        Referral_SoapText += "</table>";
                                    }

                                }
                            }
                            else if (div.OriginalName == "div" && div.Id == "" && div.FirstChild.Name == "#text")//div.OriginalName == "p")
                            {
                                OverAllComments = div.InnerHtml;
                                //OverAllComments = System.Text.RegularExpressions.Regex.Replace(div.InnerText, @"\s+", " ");
                            }
                            else if (div.OriginalName == "div" && div.Id == "" && div.FirstChild.Name != "#text")
                            {
                                foreach (HtmlAgilityPack.HtmlNode item in div.ChildNodes)
                                {
                                    if (item.OriginalName == "div")
                                    {
                                        TreatmentHTML += "<li>" + item.ChildNodes[0].InnerHtml + " </li>";
                                    }
                                    else
                                    {
                                        TreatmentHTML += "<li>" + item.InnerHtml + " </li>";
                                    }
                                }
                            }
                        }

                        TreatmentHTML += "</ul>";
                        if (!string.IsNullOrWhiteSpace(Referral_SoapText))
                        {
                            TreatmentHTML += Referral_SoapText;
                            Referral_SoapText = string.Empty;
                        }
                        if (!string.IsNullOrWhiteSpace(OverAllComments))
                        {
                            TreatmentHTML += "<ul style='list-style-type:none;padding-left: 0px !important;padding-top:3px;word-wrap: break-word;font-size:12px;'>";
                            TreatmentHTML += "<br><li>" + OverAllComments + " </li>";
                            TreatmentHTML += "</ul>";
                            OverAllComments = string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                { }

            }


            string Comments = NotesCommentContents(objListNotesComponent, "Treatment" + NotesId);
            if (!string.IsNullOrWhiteSpace(Comments))
            {
                TreatmentHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'><li style='padding-top:5px;padding-bottom:5px;'>" + Comments + " </li></ul>";
            }
            TreatmentHTML = TreatmentHTML.Replace("<br>", "<br />");
            return TreatmentHTML;
        }
        public string NotesOrderSets(List<NotesComponent> objListNotesComponent, long NotesId, List<NotesComponents> objListNotesComponents)
        {
            string OrderSetstHTML = string.Empty;
            string Referral_SoapText = string.Empty;
            string OverAllComments = string.Empty;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            OrderSetstHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            if (objListNotesComponents.FirstOrDefault(x => x.Component == "OrderSets") != null)
            {
                foreach (NotesComponent obj in objListNotesComponent)
                {
                    document.LoadHtml(obj.SOAPText);
                    try
                    {
                        OrderSetstHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Order Sets</b> </li>";
                        foreach (HtmlAgilityPack.HtmlNode section in document.DocumentNode.SelectNodes("//section"))
                        {
                            foreach (HtmlAgilityPack.HtmlNode div in section.ChildNodes)
                            {
                                if (div.OriginalName == "div")
                                {
                                    try
                                    {
                                        foreach (HtmlAgilityPack.HtmlNode li in div.ChildNodes)
                                        {
                                            if (li.OriginalName == "ul" && li.InnerHtml != "")
                                                OrderSetstHTML += "<li>" + li.InnerHtml.Replace("br", "br/") + "</li>";
                                        }
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                }
            }

            OrderSetstHTML += "</ul>";

            return OrderSetstHTML;
        }
        public string NotesSocPsyandBehaviorHx(List<NotesComponent> objListNotesComponent, long NotesId)
        {
            string OrderSetstHTML = string.Empty;
            string Referral_SoapText = string.Empty;
            string OverAllComments = string.Empty;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            OrderSetstHTML = "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
            string upComment = NotesCommentContentsUpper(objListNotesComponent, "Treatment");
            foreach (NotesComponent obj in objListNotesComponent)
            {
                document.LoadHtml(obj.SOAPText);
                try
                {
                    OrderSetstHTML += "<li class='font-xs bold text-primary' style='color: #0088cc;'><b>Social, Psychological and Behavior Hx</b> </li>";
                    foreach (HtmlAgilityPack.HtmlNode ul in document.DocumentNode.SelectNodes("//section/div/ul"))
                    {
                        foreach (HtmlAgilityPack.HtmlNode li in ul.ChildNodes)
                        {
                            if (li.OriginalName == "li" && li.InnerHtml != "")
                                OrderSetstHTML += "<li>" + li.InnerHtml.Replace("br", "br/") + "</li>";
                        }


                    }

                }
                catch (Exception ex)
                {

                }
            }
            OrderSetstHTML += "</ul>";
            return OrderSetstHTML;
        }
        public string NotesHeader(List<NoteHeaderData> objListNoteHeaderData, DSReportHeader.ReportHeaderTagsRow drReportHeader)
        {
            string NotesHeaderHTML = string.Empty;


            if (string.IsNullOrEmpty(drReportHeader["HeaderLogo"].ToString()) && string.IsNullOrEmpty(drReportHeader["PracticeText"].ToString())
                && string.IsNullOrEmpty(drReportHeader["PatientText"].ToString()) && string.IsNullOrEmpty(drReportHeader["ProviderText"].ToString()))
            {
                NotesHeaderHTML = "<table border='0' style='width:100%;font-size:12px;margin-top:-20px;'><tr style='padding:0px;'><td style='width:50%;vertical-align:top;padding:0px;border-bottom:4px solid #468aea;'>";
                if (string.IsNullOrEmpty(drReportHeader["HeaderLogo"].ToString()))
                {
                    byte[] file;
                    string LogoPath = HttpContext.Current.Server.MapPath("../content/images/SHS-nav-logo-small-100.png");
                    LogoPath = LogoPath.Replace("\\api", "");
                    using (var stream = new FileStream(LogoPath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            file = reader.ReadBytes((int)stream.Length);
                        }
                    }
                    // NotesHeaderHTML += "<div style='width:100px;height:80px;'><img src='data:image/png;base64," + Convert.ToBase64String(file) + "' style='width:100px;height:80px;' /></div>";
                    NotesHeaderHTML += "<div style='width:200px;height:80px;'><img src='data:image/png;base64," + Convert.ToBase64String(file) + "' style='width:200px;height:80px;' /></div>";
                }
                else
                {
                    //  NotesHeaderHTML += "<div style='width:200px;height:80px;'><img src='" + drReportHeader["HeaderLogo"].ToString() + "' style='width:200px;height:80px;' /></div>";
                }

                NotesHeaderHTML += "</td><td style='padding:0px;width:50%;vertical-align:top;text-align:right;border-bottom:4px solid #468aea;'>";
                if (string.IsNullOrEmpty(drReportHeader["PracticeText"].ToString()))
                {
                    foreach (NoteHeaderData obj in objListNoteHeaderData.Where(m => m.Type == "Practice"))
                    {
                        NotesHeaderHTML += "<p style='padding: 0px !important;'>";
                        if (!string.IsNullOrWhiteSpace(obj.ShortName))
                        {
                            NotesHeaderHTML += obj.ShortName + "<br />";
                        }
                        if (!string.IsNullOrWhiteSpace(obj.Address))
                        {
                            NotesHeaderHTML += obj.Address + "<br />";
                        }
                        NotesHeaderHTML += obj.City + ", " + obj.State + ", " + obj.ZIPCode + "<br />";
                        if (!string.IsNullOrWhiteSpace(obj.PhoneNo))
                        {
                            NotesHeaderHTML += obj.PhoneNo;
                        }
                        NotesHeaderHTML += "</p>";
                    }
                }
                else
                {
                    //  NotesHeaderHTML += "<li style='padding: 0px !important;'>" + drReportHeader["PracticeText"].ToString() + " </li>";
                }
                // NotesHeaderHTML += "</ul>";
                NotesHeaderHTML += "</td></tr>";

                NotesHeaderHTML += "<tr><td style='vertical-align:top;'>";
                if (string.IsNullOrEmpty(drReportHeader["PatientText"].ToString()))
                {
                    foreach (NoteHeaderData obj in objListNoteHeaderData.Where(m => m.Type == "Patient"))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.FirstName))
                        {
                            NotesHeaderHTML += "<p style='padding: 0px !important;'><label class='font - xs bold m-xs text - primary'>" + obj.FirstName + " " + obj.LastName + "</label><br />";
                        }
                        //NotesHeaderHTML += "<p style='padding: 0px !important;'>";
                        int Years = new DateTime(DateTime.Now.Subtract(Convert.ToDateTime(obj.DOB)).Ticks).Year - 1;
                        NotesHeaderHTML += Years + " Y, ";
                        if (!string.IsNullOrWhiteSpace(obj.Gender))
                        {
                            NotesHeaderHTML += obj.Gender + ",";
                        }
                        if (obj.DOB != null)
                        {
                            NotesHeaderHTML += " DOB: " + Convert.ToDateTime(obj.DOB).ToString("MM/dd/yyyy").Replace("12:00AM", "");
                        }
                        NotesHeaderHTML += "<br />";
                        if (!string.IsNullOrWhiteSpace(obj.AccountNumber))
                        {
                            NotesHeaderHTML += "Acc #: " + obj.AccountNumber + "<br />";
                        }
                        if (!string.IsNullOrWhiteSpace(obj.Address1))
                        {
                            NotesHeaderHTML += "" + obj.Address1 + "</p>";
                        }
                        else
                        {
                            NotesHeaderHTML += "</p>";
                        }
                    }
                }
                else
                {
                    // drReportHeader["PatientText"] = drReportHeader["PatientText"].ToString().Replace("12:00AM", "");
                    // NotesHeaderHTML += "<li style='padding: 0px !important;'>" + drReportHeader["PatientText"].ToString() + " </li>";
                }
                // NotesHeaderHTML += "</ul>";

                NotesHeaderHTML += "</td><td style='width:50%;vertical-align:top;text-align:right;'>";
                if (string.IsNullOrEmpty(drReportHeader["ProviderText"].ToString()))
                {
                    foreach (NoteHeaderData obj in objListNoteHeaderData.Where(m => m.Type == "Provider"))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.FirstName))
                        {
                            NotesHeaderHTML += "<p style='padding: 0px !important;'><label class='font - xs bold m-xs text - primary'>" + obj.FirstName + " " + obj.LastName + "</label><br />";
                        }
                        if (!string.IsNullOrWhiteSpace(obj.SpecialtyName))
                        {
                            NotesHeaderHTML += "" + obj.SpecialtyName + "<br />";
                        }
                    }
                    foreach (NoteHeaderData obj in objListNoteHeaderData.Where(m => m.Type == "Note"))
                    {
                        if (obj.Notedate != null)
                        {
                            NotesHeaderHTML += "" + Convert.ToDateTime(obj.Notedate).ToString("MM/dd/yyyy hh:mm tt").Replace("12:00 AM", "") + "<br />";
                        }
                        if (!string.IsNullOrWhiteSpace(obj.NoteReason))
                        {
                            NotesHeaderHTML += "" + obj.NoteReason + "</p>";
                        }
                    }
                }
                else
                {
                    //  NotesHeaderHTML += "<li style='padding: 0px !important;'>" + drReportHeader["ProviderText"].ToString() + " </li>";
                }

                // NotesHeaderHTML += "</ul>";
                NotesHeaderHTML += "</td></tr></table>";
            }
            else
            {
                NotesHeaderHTML = "<table border='0' style='width:100%;font-size:12px;margin-top:-20px;'><tr style='padding:0px;'><td style='width:50%;vertical-align:top;padding:0px;border-bottom:4px solid #468aea;border-top:3px solid #163a6e;'>";
                if (string.IsNullOrEmpty(drReportHeader["HeaderLogo"].ToString()))
                {
                    //byte[] file;
                    //string LogoPath = HttpContext.Current.Server.MapPath("../content/images/SHS-nav-logo-small-100.png");
                    //LogoPath = LogoPath.Replace("\\api", "");
                    //using (var stream = new FileStream(LogoPath, FileMode.Open, FileAccess.Read))
                    //{
                    //    using (var reader = new BinaryReader(stream))
                    //    {
                    //        file = reader.ReadBytes((int)stream.Length);
                    //    }
                    //}
                    //NotesHeaderHTML += "<div style='width:100px;height:50px;'><img src='data:image/png;base64," + Convert.ToBase64String(file) + "' style='width:100px;height:50px;' /></div>";
                }
                else
                {
                    NotesHeaderHTML += "<div style='width:200px;height:80px;'><img src='" + drReportHeader["HeaderLogo"].ToString() + "' style='width:200px;height:80px;' /></div>";
                }

                NotesHeaderHTML += "</td><td style='padding:0px;width:50%;vertical-align:top;text-align:right;border-bottom:4px solid #468aea;border-top:3px solid #163a6e;'>";
                if (string.IsNullOrEmpty(drReportHeader["PracticeText"].ToString()))
                {
                    //foreach (NoteHeaderData obj in objListNoteHeaderData.Where(m => m.Type == "Practice"))
                    //{
                    //    NotesHeaderHTML += "<li style='padding: 0px !important;'>";
                    //    if (!string.IsNullOrWhiteSpace(obj.ShortName))
                    //    {
                    //        NotesHeaderHTML += obj.ShortName + "<br />";
                    //    }
                    //    if (!string.IsNullOrWhiteSpace(obj.Address))
                    //    {
                    //        NotesHeaderHTML += obj.Address + "<br />";
                    //    }
                    //    NotesHeaderHTML += obj.City + ", " + obj.State + ", " + obj.ZIPCode + "<br />";
                    //    if (!string.IsNullOrWhiteSpace(obj.PhoneNo))
                    //    {
                    //        NotesHeaderHTML += obj.PhoneNo;
                    //    }
                    //    NotesHeaderHTML += " </li>";
                    //}
                }
                else
                {
                    NotesHeaderHTML += "<p style='padding: 0px !important;'>" + drReportHeader["PracticeText"].ToString() + "</p>";
                }
                //  NotesHeaderHTML += "</ul>";
                NotesHeaderHTML += "</td></tr>";

                NotesHeaderHTML += "<tr><td style='vertical-align:top;border-bottom:4px solid #468aea'><br/>";
                if (string.IsNullOrEmpty(drReportHeader["PatientText"].ToString()))
                {
                    //foreach (NoteHeaderData obj in objListNoteHeaderData.Where(m => m.Type == "Patient"))
                    //{
                    //    if (!string.IsNullOrWhiteSpace(obj.FirstName))
                    //    {
                    //        NotesHeaderHTML += "<li style='padding: 0px !important;'>" + obj.FirstName + " " + obj.LastName + " </li>";
                    //    }
                    //    NotesHeaderHTML += "<li style='padding: 0px !important;'>";
                    //    int Years = new DateTime(DateTime.Now.Subtract(Convert.ToDateTime(obj.DOB)).Ticks).Year - 1;
                    //    NotesHeaderHTML += Years + " Y, ";
                    //    if (!string.IsNullOrWhiteSpace(obj.Gender))
                    //    {
                    //        NotesHeaderHTML += obj.Gender + ",";
                    //    }
                    //    if (obj.DOB != null)
                    //    {
                    //        NotesHeaderHTML += " DOB: " + Convert.ToDateTime(obj.DOB).ToString("MM/dd/yyyy");
                    //    }
                    //    NotesHeaderHTML += " </li>";
                    //    if (!string.IsNullOrWhiteSpace(obj.AccountNumber))
                    //    {
                    //        NotesHeaderHTML += "<li style='padding: 0px !important;'>Acc #: " + obj.AccountNumber + " </li>";
                    //    }
                    //    if (!string.IsNullOrWhiteSpace(obj.Address1))
                    //    {
                    //        NotesHeaderHTML += "<li style='padding: 0px !important;'>" + obj.Address1 + " </li>";
                    //    }
                    //}
                }
                else
                {
                    drReportHeader["PatientText"] = drReportHeader["PatientText"].ToString().Replace("12:00AM", "");
                    NotesHeaderHTML += "<p style='padding: 0px !important;'>" + drReportHeader["PatientText"].ToString() + "</p>";
                }
                //   NotesHeaderHTML += "</ul>";

                NotesHeaderHTML += "</td><td style='width:50%;vertical-align:top;text-align:right;border-bottom:4px solid #468aea'><br/>";
                if (string.IsNullOrEmpty(drReportHeader["ProviderText"].ToString()))
                {
                    //foreach (NoteHeaderData obj in objListNoteHeaderData.Where(m => m.Type == "Provider"))
                    //{
                    //    if (!string.IsNullOrWhiteSpace(obj.FirstName))
                    //    {
                    //        NotesHeaderHTML += "<li style='padding: 0px !important;'>" + obj.FirstName + " " + obj.LastName + " </li>";
                    //    }
                    //    if (!string.IsNullOrWhiteSpace(obj.SpecialtyName))
                    //    {
                    //        NotesHeaderHTML += "<li style='padding: 0px !important;'>" + obj.SpecialtyName + " </li>";
                    //    }
                    //}
                    //foreach (NoteHeaderData obj in objListNoteHeaderData.Where(m => m.Type == "Note"))
                    //{
                    //    if (obj.Notedate != null)
                    //    {
                    //        NotesHeaderHTML += "<li style='padding: 0px !important;'>" + Convert.ToDateTime(obj.Notedate).ToString("MM/dd/yyyy hh:mm tt") + " </li>";
                    //    }
                    //    if (!string.IsNullOrWhiteSpace(obj.NoteReason))
                    //    {
                    //        NotesHeaderHTML += "<li style='padding: 0px !important;'>" + obj.NoteReason + " </li>";
                    //    }
                    //}
                }
                else
                {
                    drReportHeader["ProviderText"] = drReportHeader["ProviderText"].ToString().Replace("12:00:00 AM", "");
                    // drReportHeader["ProviderText"] = drReportHeader["ProviderText"].ToString().Replace("< br />", "");

                    NotesHeaderHTML += "<p style='padding: 0px !important;'>" + drReportHeader["ProviderText"].ToString() + "</p>";
                }
                NotesHeaderHTML += "</td></tr>";

                NotesHeaderHTML += "<tr style='padding-top:90px;'><td padding-top:.5em; colspan='2'> <br/>&nbsp;";
                NotesHeaderHTML += "</td></tr>";

                NotesHeaderHTML += "</table>";
            }
            //  NotesHeaderHTML += "</ul>";


            return NotesHeaderHTML;
        }

        public byte[] ConvertHtmlToPdf(string xHtml, string css, List<NoteHeaderData> objListNoteHeaderData, DSReportHeader.ReportHeaderTagsRow drReportHeader)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                using (var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 30f, 35f))
                {
                    var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, stream);
                    if (drReportHeader != null)
                    {
                        string FooterGeneratedBy = drReportHeader["FooterText"].ToString();
                        iTextSharp.text.pdf.PdfPTable footer = new iTextSharp.text.pdf.PdfPTable(1);
                        Rectangle pageSize = document.PageSize;

                        footer = setFooterPDF(FooterGeneratedBy);
                        writer.PageEvent = new MDVision.Common.Utilities.MDVUtility.PDFFooterHeader(ReportHeaderTable(document.PageSize, drReportHeader), true, null, footer);
                    }
                    document.Open();

                    var tagProcessorFactory = (iTextSharp.tool.xml.html.DefaultTagProcessorFactory)iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory();
                    tagProcessorFactory.RemoveProcessor(iTextSharp.tool.xml.html.HTML.Tag.IMG);
                    tagProcessorFactory.AddProcessor(iTextSharp.tool.xml.html.HTML.Tag.IMG, new CustomImageTagProcessor());

                    var htmlPipelineContext = new iTextSharp.tool.xml.pipeline.html.HtmlPipelineContext(null);
                    htmlPipelineContext.SetTagFactory(tagProcessorFactory);

                    var pdfWriterPipeline = new iTextSharp.tool.xml.pipeline.end.PdfWriterPipeline(document, writer);
                    var htmlPipeline = new iTextSharp.tool.xml.pipeline.html.HtmlPipeline(htmlPipelineContext, pdfWriterPipeline);

                    var charset = System.Text.Encoding.UTF8;
                    // get an ICssResolver and add the custom CSS
                    var cssResolver = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                    cssResolver.AddCssFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/Default/bootstrap.css"), true);
                    cssResolver.AddCssFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/Blue/theme-custom.css"), true);
                    cssResolver.AddCss(css, "utf-8", true);
                    var cssResolverPipeline = new iTextSharp.tool.xml.pipeline.css.CssResolverPipeline(
                        cssResolver, htmlPipeline
                    );

                    var worker = new iTextSharp.tool.xml.XMLWorker(cssResolverPipeline, true);
                    var parser = new iTextSharp.tool.xml.parser.XMLParser(worker);
                    using (var stringReader = new System.IO.StringReader(xHtml))
                    {
                        parser.Parse(stringReader);
                    }

                    document.Close();
                    MemoryStream stream2 = new MemoryStream(stream.ToArray());
                    PdfReader npdf = new PdfReader(stream2);
                    MemoryStream outstream = new MemoryStream();
                    iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE);
                    PdfPTable table2 = new PdfPTable(2);
                    using (PdfStamper stamper = new PdfStamper(npdf, outstream, '\0', true))
                    {
                        stamper.Writer.CloseStream = false;
                        int PageCount = npdf.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                            Paragraph para = new Paragraph(String.Format("Page {0} of {1}", i, PageCount), bodyFont);

                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, para, 555, 17, 0);
                        }
                    }
                    return outstream.GetBuffer();
                }
            }
        }

        private PdfPTable setFooterPDF(string generatedBy = "")
        {
            PdfPTable footer = new PdfPTable(1);
            footer.TotalWidth = 575;
            footer.LockedWidth = true;
            footer.HorizontalAlignment = Element.ALIGN_CENTER;
            footer.DefaultCell.Border = Rectangle.NO_BORDER;
            footer.SpacingBefore = 5f;

            PdfPCell footerCell = new PdfPCell();
            var color = System.Drawing.ColorTranslator.FromHtml("#005da9");
            footerCell.BackgroundColor = new BaseColor(color);
            color = System.Drawing.ColorTranslator.FromHtml("#fff");
            var fontcolor = new BaseColor(color);
            Font componentFooterFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, fontcolor);

            Paragraph footerPara = new Paragraph(generatedBy == "" ? "Generated by: MDVision PMS EMR" : "Generated by: " + generatedBy, componentFooterFont);
            footerPara.SpacingAfter = 5f;
            footerCell.AddElement(footerPara);
            footer.AddCell(footerCell);
            // pdfDocument.Add(footer);
            return footer;
        }

        public PdfPTable ReportHeaderTable(Rectangle pageSize, DSReportHeader.ReportHeaderTagsRow drReportHeader)
        {

            PdfPTable HeaderTable = new PdfPTable(2);
            var color = System.Drawing.ColorTranslator.FromHtml("#005da9");
            HeaderTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            HeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;
            HeaderTable.TotalWidth = pageSize.Width - 20;

            var fontColour = new BaseColor(102, 178, 255);
            Font componentHeadingFont = FontFactory.GetFont("Arial", 8, Font.BOLD, fontColour);
            Font HeaderFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);

            //NoteHeaderData objPatient = objListNoteHeaderData.FirstOrDefault(m => m.Type == "Patient");
            //NoteHeaderData objProvider = objListNoteHeaderData.FirstOrDefault(m => m.Type == "Provider");
            //NoteHeaderData objNote = objListNoteHeaderData.FirstOrDefault(m => m.Type == "Note");

            var PatNamelblChunk = new Chunk("Patient Name: ", componentHeadingFont);
            var PatNameChunk = new Chunk(drReportHeader["PatientName"].ToString(), HeaderFont);
            var phrasePatName = new Phrase(PatNamelblChunk);
            phrasePatName.Add(PatNameChunk);

            PdfPCell HeaderLeftCell = new PdfPCell(phrasePatName);
            HeaderLeftCell.Padding = 0;
            HeaderLeftCell.PaddingBottom = 3;
            HeaderLeftCell.BorderWidth = 0;
            HeaderTable.AddCell(HeaderLeftCell);

            var ProNamelblChunk = new Chunk("Provider Name: ", componentHeadingFont);
            var ProNameChunk = new Chunk(drReportHeader["ProviderName"].ToString(), HeaderFont);
            var phraseProName = new Phrase(ProNamelblChunk);
            phraseProName.Add(ProNameChunk);

            PdfPCell HeaderRightCell = new PdfPCell(phraseProName);
            HeaderRightCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            HeaderRightCell.Padding = 0;
            HeaderRightCell.PaddingBottom = 3;
            HeaderRightCell.BorderWidth = 0;
            HeaderTable.AddCell(HeaderRightCell);

            var DOBlblChunk = new Chunk("DOB: ", componentHeadingFont);
            var DOBChunk = new Chunk(Convert.ToDateTime(drReportHeader["PatientDOB"].ToString()).ToString("MM/dd/yyyy"), HeaderFont);
            var phraseDOB = new Phrase(DOBlblChunk);
            phraseDOB.Add(DOBChunk);

            PdfPCell HeaderLeftCell1 = new PdfPCell(phraseDOB);
            HeaderLeftCell1.Padding = 0;
            HeaderLeftCell1.PaddingBottom = 3;
            HeaderLeftCell1.BorderWidth = 0;
            HeaderTable.AddCell(HeaderLeftCell1);

            var DOSlblChunk = new Chunk("DOS: ", componentHeadingFont);
            var DOSChunk = new Chunk(Convert.ToDateTime(drReportHeader["DOS"].ToString()).ToString("MM/dd/yyyy"), HeaderFont);
            var phraseDOS = new Phrase(DOSlblChunk);
            phraseDOS.Add(DOSChunk);

            PdfPCell HeaderRightCell1 = new PdfPCell(phraseDOS);
            HeaderRightCell1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            HeaderRightCell1.Padding = 0;
            HeaderRightCell1.PaddingBottom = 3;
            HeaderRightCell1.BorderWidth = 0;
            HeaderTable.AddCell(HeaderRightCell1);

            return HeaderTable;
        }

        public string CssFiles()
        {
            string CSS = @"

                    hr.style15 {
	                                border-top: 4px double #8c8b8b;
	                                text-align: center;
                                }
                                hr.style15:after {
	                                content: '\002665';
	                                display: inline-block;
	                                position: relative;
	                                top: -15px;
	                                padding: 0 10px;
	                                background: #f0f0f0;
	                                color: #8c8b8b;
	                                font-size: 18px;
                                }
                            ul{

                                white-space: nowrap;
                                width:300px;
                            }
                            li{
                                display: inline-block;
                                float:left;
                                width:300px;
                            }
                            .img-responsive,
                            .thumbnail > img,
                            .thumbnail a > img,
                            .carousel-inner > .item > img,
                            .carousel-inner > .item > a > img {
                                display: block;
                                width: 100% \9;
                                max-width: 100%;
                                height: auto;
                            }
                            .list-unstyled {
                                    list-style: none;
                                    padding: 0;
                                }
                                .splitter {
                                    background: #468aea;
                                    color: #fff;
                                    padding: 1px 0 1px 20px;
                                    font-size: 14px;
                                    margin: -4px -15px 10px -15px;
                                }
                                .spacer3 {
                                    clear: both;
                                    display: block;
                                    height: 3px;
                                }
                                .mt-xs {
                                    margin-top: 5px !important;
                                }
                                .m-none {
                                    margin: 0 !important;
                                }
                            .red{
                            color: red !important;
                            }
                            .green{
                                color: green !important;
                                }
                        ";
            return CSS;
        }

        #endregion Legacy Notes

        public string GetAssociatedAttachmentsOfNote(long NotesId)
        {
            try
            {
                List<NoteDocumentModel> documentsList = null;
                BLObject<List<NoteDocumentModel>> obj = BLLClinicalObj.GetAssociatedAttachmentsOfNote(NotesId);
                documentsList = obj.Data;
                if (obj.Data != null)
                {
                    if (documentsList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            AttachedDocsCount = documentsList.Count,
                            AttachedDocs = documentsList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            AmendmentNoteCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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

        public string SaveAndAttachOrderReport(ClinicalNotesFillModel model) //(string OrderIds, string OrderType, long PatientId, long notesId)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.OrderIds))
                {
                    string base64string = "";
                    string[] orders = model.OrderIds.Split(',');
                    List<long> patDocIds = new List<long>();
                    List<PatientDocumentModel> patDocList = new List<PatientDocumentModel>();
                    PatientDocumentModel patDocObj = null;
                    DSRadiologyOrder dsRadiology = null;
                    BLObject<DSRadiologyOrder> objrad;
                    if (model.OrderType == "Radiology Order")
                    {
                        foreach (var orderId in orders)
                        {
                            BLObject<byte[]> obj = BLLClinicalObj.previewRadiologyOrder(MDVUtility.ToInt64(orderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToBool(model.IsFindingUpdated), MDVUtility.ToLong(model.NotesId));
                            base64string = Convert.ToBase64String(obj.Data);
                            var fname = model.FileName;
                            objrad = BLLClinicalObj.LoadRadiologyOrderTest(MDVUtility.ToInt64(orderId), 0, 0, "1", "2000");
                            dsRadiology = objrad.Data;
                            if (dsRadiology != null && dsRadiology.RadiologyOrderTest.Rows.Count > 0)
                            {

                                string dt = dsRadiology.RadiologyOrderTest.Rows[0][dsRadiology.RadiologyOrderTest.TestDateColumn].ToString();
                                string tm = dsRadiology.RadiologyOrderTest.Rows[0][dsRadiology.RadiologyOrderTest.TestTimeColumn].ToString();
                                string test = dsRadiology.RadiologyOrderTest.Rows[0][dsRadiology.RadiologyOrderTest.CPTCodeDescriptionColumn].ToString();
                                //AST-355 by:MAHMAD
                                if (test.Length > 140)
                                {
                                    test = test.Substring(0, 140);
                                }
                                //AST-355 by:MAHMAD
                                dt = dt.Split(' ')[0];
                                fname = dt + tm;
                                fname = fname.Replace("/", "-").Replace(":", "");
                                var mnth = fname.Split('-')[0];
                                var day = fname.Split('-')[1];
                                var year = fname.Split('-')[2].Substring(0, 4);
                                mnth = mnth.Length == 1 ? "0" + mnth : mnth;
                                day = day.Length == 1 ? "0" + day : day;
                                var mnt = fname.Split('-')[2].Substring(4, 6).Replace(" ", "");
                                mnt = mnt.Length == 5 ? "0" + mnt : mnt;
                                //fname = mnth + "-" + day + "-" + year + " " + mnt + "_" + test + ".pdf";
                                fname = mnth + "." + day + "." + year + "_" + test + ".pdf";

                            }

                            string docResponse = new Patient_Document().SaveSignedDocument("", MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(orderId), "Rad Ord Report", base64string, model.FileType, fname, model.FolderName);
                            PatientDocumentResponse result = JsonConvert.DeserializeObject<PatientDocumentResponse>(docResponse);

                            patDocIds.Add(MDVUtility.ToLong(result.PatDocId));
                            patDocObj = new PatientDocumentModel();
                            patDocObj.PatDocId = result.PatDocId;
                            patDocObj.OrderId = orderId;
                            patDocList.Add(patDocObj);
                        }
                    }
                    else if (model.OrderType == "Procedure Order")
                    {
                        foreach (var orderId in orders)
                        {
                            BLObject<byte[]> obj = BLLClinicalObj.previewProcedureOrder(MDVUtility.ToInt64(orderId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToBool(model.IsFindingUpdated), MDVUtility.ToLong(model.NotesId));
                            base64string = Convert.ToBase64String(obj.Data);

                            string docResponse = new Patient_Document().SaveSignedDocument("", MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(orderId), "Proc Ord Report", base64string, model.FileType, model.FileName, model.FolderName);
                            PatientDocumentResponse result = JsonConvert.DeserializeObject<PatientDocumentResponse>(docResponse);

                            patDocIds.Add(MDVUtility.ToLong(result.PatDocId));
                            patDocObj = new PatientDocumentModel();
                            patDocObj.PatDocId = result.PatDocId;
                            patDocObj.OrderId = orderId;
                            patDocList.Add(patDocObj);
                        }
                    }

                    if (patDocIds.Count > 0)
                    {
                        string documentIds = string.Join(",", patDocIds);
                        string attachDocRes = new Patient_Document_Image_Annotation().AttachPatientDocumentToNote(documentIds, MDVUtility.ToInt64(model.NotesId));
                        PatientDocumentResponse attachResult = JsonConvert.DeserializeObject<PatientDocumentResponse>(attachDocRes);

                        if (attachResult.status == true)
                        {
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                            var response = new
                            {
                                status = true,
                                Message = attachResult.Message,
                                PatDocOrdersList_Count = patDocList.Count,
                                PatDocOrdersList = js.Serialize(patDocList)
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = attachResult.Message
                            };
                            return (JsonConvert.SerializeObject(response));
                        }

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = "No Order Found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = "No Order Found."
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



        #region Template Data Print

        public string NotesTemplateDataSelect(CommonSearch objCommonSearch)
        {
            MDVision.Model.Clinical.Notes.Notes.TemplateData objTemplateData = new MDVision.Model.Clinical.Notes.Notes.TemplateData();
            MDVision.Model.Clinical.Notes.Notes.TemplateData objTemplateDataSoap = new MDVision.Model.Clinical.Notes.Notes.TemplateData();
            try
            {
                objCommonSearch.UserID = Convert.ToInt64(MDVSession.Current.AppUserId);
                objTemplateData = BLLClinicalObj.NotesTemplateDataSelect(objCommonSearch);
                objTemplateDataSoap = BLLClinicalObj.NotesTemplateDataWithTagsSelect(objCommonSearch);
                objTemplateDataSoap.ROSDataTemptId = objTemplateData.ROSDataTemptId;
                objTemplateDataSoap.PEDataTemptId = objTemplateData.PEDataTemptId;
                string NotesHtml = TemplateDataHTML(objTemplateDataSoap);
                var response = new
                {
                    status = false,
                    Message = NotesHtml,
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

        public string TemplateDataHTML(MDVision.Model.Clinical.Notes.Notes.TemplateData objTemplateData)
        {
            string TemplateHTML = string.Empty;
            TemplateHTML = objTemplateData.b;

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(TemplateHTML);

            try
            {
                var query = document.DocumentNode.Descendants("input");
                foreach (var item in query.ToList())
                {
                    if (item.Attributes["value"] != null)
                    {
                        string inputValue = item.Attributes["value"].Value;
                        inputValue = inputValue.Replace("{{", string.Empty);
                        inputValue = inputValue.Replace("}}", string.Empty);

                        inputValue = inputValue.Replace("Clinical", string.Empty);
                        if (inputValue.Trim().ToLower().Contains("custom form"))
                        {
                            inputValue = inputValue.Replace("Custom Form", string.Empty);
                        }
                        var newNodeStr = "<ul style='list-style-type:none;padding: 5px !important;word-wrap: break-word;font-size:12px;'>";
                        newNodeStr += "<li style='color: #0088cc;padding:5px; !important'><b>" + inputValue + " </b></li>";
                        if (inputValue.Trim() == "Review of System")
                        {
                            string rosHTML = string.Empty;
                            if (objTemplateData.ROSDataTemptId > 0)
                            {
                                List<MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem> objList_ReviewOfSystem = new List<MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem>();
                                CommonSearch objCommonSearch = new CommonSearch();
                                objCommonSearch.ROSDataTemplateId = objTemplateData.ROSDataTemptId;
                                objList_ReviewOfSystem = BLLClinicalObj.ReviewOfSystemDataSelect(objCommonSearch);
                                if (objList_ReviewOfSystem != null && objList_ReviewOfSystem.Count() > 0)
                                {
                                    rosHTML += TemplateReviewofSystemHTML(objList_ReviewOfSystem);
                                }
                                newNodeStr += rosHTML;
                            }
                        }
                        else if (inputValue.Trim() == "Physical Exam")
                        {
                            string PEHTML = string.Empty;
                            if (objTemplateData.PEDataTemptId > 0)
                            {
                                List<MDVision.Model.Clinical.LegacyNotes.PhysicalExam> objList_PhysicalExam = new List<MDVision.Model.Clinical.LegacyNotes.PhysicalExam>();
                                CommonSearch objCommonSearch = new CommonSearch();
                                objCommonSearch.TemplateId = objTemplateData.PEDataTemptId;
                                objList_PhysicalExam = BLLClinicalObj.PhysicalExamDataSelect(objCommonSearch);
                                if (objList_PhysicalExam != null && objList_PhysicalExam.Count() > 0)
                                {
                                    PEHTML += TemplatePhysicalExamHTML(objList_PhysicalExam);
                                }
                                newNodeStr += PEHTML;
                            }
                        }
                        newNodeStr += "<li>&nbsp;</li>";
                        newNodeStr += "</ul>";
                        var newNode = HtmlAgilityPack.HtmlNode.CreateNode(newNodeStr);
                        item.ParentNode.ReplaceChild(newNode, item);
                    }
                }
            }
            catch (Exception ex)
            { }
            TemplateHTML = document.DocumentNode.SelectSingleNode("//html").OuterHtml;

            TemplateHTML = TemplateHTML.Replace("{FT}", string.Empty);
            TemplateHTML = TemplateHTML.Replace("{/FT}", string.Empty);
            return TemplateHTML;
        }


        public string TemplateDataHTML2(MDVision.Model.Clinical.Notes.Notes.TemplateData objTemplateData)
        {
            string TemplateHTML = string.Empty;
            int counter = 0;
            TemplateHTML += "<table style='width:100%;border-top-left-radius: 5px;'>";
            bool PEExists = false;
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Complaints }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Complaints </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Social Hx }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Social Hx </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Medical Hx }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Medical Hx </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Birth Hx }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Birth Hx </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Family Hx }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Family Hx </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Surgical Hx }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Surgical Hx </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Hospitalization Hx }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Hospitalization Hx </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Review of System }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Review of System </b></li>";

                counter = counter + 1;
                if (objTemplateData.ROSDataTemptId > 0)
                {
                    List<MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem> objList_ReviewOfSystem = new List<MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem>();
                    CommonSearch objCommonSearch = new CommonSearch();
                    objCommonSearch.ROSDataTemplateId = objTemplateData.ROSDataTemptId;
                    objList_ReviewOfSystem = BLLClinicalObj.ReviewOfSystemDataSelect(objCommonSearch);
                    if (objList_ReviewOfSystem != null && objList_ReviewOfSystem.Count() > 0)
                    {
                        TemplateHTML += TemplateReviewofSystemHTML(objList_ReviewOfSystem);
                    }
                }
                TemplateHTML += "</ul></td></tr>";
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Allergies }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Allergies </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Medication }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Medication </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Vitals }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Vitals </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Physical Exam }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Physical Exam </b></li>";

                counter = counter + 1;
                if (objTemplateData.PEDataTemptId > 0)
                {
                    List<MDVision.Model.Clinical.LegacyNotes.PhysicalExam> objList_PhysicalExam = new List<MDVision.Model.Clinical.LegacyNotes.PhysicalExam>();
                    CommonSearch objCommonSearch = new CommonSearch();
                    objCommonSearch.TemplateId = objTemplateData.PEDataTemptId;
                    objList_PhysicalExam = BLLClinicalObj.PhysicalExamDataSelect(objCommonSearch);
                    if (objList_PhysicalExam != null && objList_PhysicalExam.Count() > 0)
                    {
                        TemplateHTML += TemplatePhysicalExamHTML(objList_PhysicalExam);
                    }
                }
                TemplateHTML += "</ul></td></tr>";
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Lab Results }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Lab Results </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Radiology Results }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Radiology Results </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Problems }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Problems </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Prescription }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Prescription </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Lab Order }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Lab Order </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Radiology Order }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Radiology Order </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Procedure Order }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Procedure Order </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Consultation Order }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Consultation Order </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Immunization }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Immunization </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Procedures }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Procedures </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Referrals }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Referrals </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Patient Education }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Patient Education </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Follow Up }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Follow Up </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Notes Extra Info }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Notes Extra Info </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }
            if (objTemplateData.HTMLTemplate.Contains("{{ Clinical Clinical Functional and Cognitive }}"))
            {
                TemplateHTML += "<tr><td>";
                TemplateHTML += "<ul style='list-style-type:none;padding-left: 0px !important;word-wrap: break-word;font-size:12px;'>";
                TemplateHTML += "<li style='color: #0088cc;'><b>Clinical Functional and Cognitive </b></li>";
                TemplateHTML += "</ul></td></tr>";
                counter = counter + 1;
            }



            if (counter == 0)
            {
                TemplateHTML += "<tr><td>&nbsp;</td></tr>";
            }

            TemplateHTML += "</table>";

            return TemplateHTML;
        }

        public string TemplateReviewofSystemHTML(List<MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem> objList_ReviewOfSystem)
        {
            string ReviewofSystemHTML = string.Empty;
            if (objList_ReviewOfSystem != null && objList_ReviewOfSystem.Count() > 0)
            {
                foreach (MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem obj in objList_ReviewOfSystem.Where(m => m.Type == "System").GroupBy(m => new { m.ROSSystemId }).Select(g => g.OrderBy(m => m.ROSSystemId).FirstOrDefault()))
                {
                    if (objList_ReviewOfSystem.Where(m => m.ROSSystemId == obj.ROSSystemId && m.Type == "Characteristics").Count() > 0)
                    {
                        ReviewofSystemHTML += "<li><b>" + obj.Name + " </b>: ";
                        foreach (MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem sysetm in objList_ReviewOfSystem.Where(m => m.ROSSystemId == obj.ROSSystemId && m.Type == "Characteristics"))
                        {
                            ReviewofSystemHTML += " " + sysetm.CharacteristicsName + "";
                            if (sysetm.IsPositive)
                            {
                                ReviewofSystemHTML += " <span style='color:red;font-weight:bold;'> ( + ), </span>";
                            }
                            else
                            {
                                ReviewofSystemHTML += "<span style='color:green;font-weight:bold;'> ( - ), </span>";
                            }

                            MDVision.Model.Clinical.LegacyNotes.ReviewOfSystem objHistory = objList_ReviewOfSystem.FirstOrDefault(m => m.ROSDataSystemCharcID == sysetm.ROSDataSystemCharcID && m.Type == "Charc Detail");
                            if (objHistory != null)
                            {
                                if (!string.IsNullOrWhiteSpace(objHistory.PreviousHistory))
                                    ReviewofSystemHTML += " Previous History is " + objHistory.PreviousHistory + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.DetailStatusName))
                                    ReviewofSystemHTML += " Status is " + objHistory.DetailStatusName + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.Onset))
                                    ReviewofSystemHTML += " Onset is " + objHistory.Onset + ".";

                                if (objHistory.Duration > 0)
                                    ReviewofSystemHTML += " Duration is " + objHistory.Duration + " " + objHistory.DurationName + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.PatternName))
                                    ReviewofSystemHTML += " Pattern is " + objHistory.PatternName + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.SeverityName))
                                    ReviewofSystemHTML += " Severity is " + objHistory.SeverityName + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.CourseName))
                                    ReviewofSystemHTML += " Course is " + objHistory.CourseName + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.RadiationName))
                                    ReviewofSystemHTML += " Radiation to " + objHistory.RadiationName + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.FrequencyName))
                                    ReviewofSystemHTML += " Frequency is " + objHistory.FrequencyName + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.ContextName))
                                    ReviewofSystemHTML += " Context is " + objHistory.ContextName + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.CharacterCSZName))
                                    ReviewofSystemHTML += " Character is " + objHistory.CharacterCSZName + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.AggravedByName))
                                    ReviewofSystemHTML += " It is Aggravated by " + objHistory.AggravedByName + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.RelievedByName))
                                    ReviewofSystemHTML += " It is Relieved by " + objHistory.RelievedByName + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.Location))
                                    ReviewofSystemHTML += " Location is " + objHistory.Location + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.PrecipitatedBY))
                                    ReviewofSystemHTML += " Precipitated by " + objHistory.PrecipitatedBY + ".";

                                if (!string.IsNullOrWhiteSpace(objHistory.AssociatedWith))
                                    ReviewofSystemHTML += " Associated feature are " + objHistory.AssociatedWith + ".";
                            }
                        }
                        ReviewofSystemHTML += "</li>";
                    }
                }
            }
            return ReviewofSystemHTML;
        }

        public string TemplatePhysicalExamHTML(List<MDVision.Model.Clinical.LegacyNotes.PhysicalExam> objList_PhysicalExam)
        {
            string PhysicalExamHTML = string.Empty;
            if (objList_PhysicalExam != null && objList_PhysicalExam.Count() > 0)
            {
                foreach (MDVision.Model.Clinical.LegacyNotes.PhysicalExam obj in objList_PhysicalExam.GroupBy(m => new { m.TemplateName }).Select(g => g.OrderBy(m => m.TemplateName).FirstOrDefault()))
                {
                    PhysicalExamHTML += "<li><b style='color:#bd0e09;'>" + obj.TemplateName + " </b></li>";
                    foreach (MDVision.Model.Clinical.LegacyNotes.PhysicalExam objSys in objList_PhysicalExam.GroupBy(m => new { m.PETemplateSystemId }).Select(g => g.OrderBy(m => m.PETemplateSystemId).FirstOrDefault()))
                    {
                        PhysicalExamHTML += "<li><b style='color:#51b451;'>" + objSys.SystemName + " </b>: ";
                        foreach (MDVision.Model.Clinical.LegacyNotes.PhysicalExam observation in objList_PhysicalExam.Where(m => m.PETemplateSystemId == objSys.PETemplateSystemId))
                        {

                            PhysicalExamHTML += " " + observation.ObservationName + ",";
                        }
                        PhysicalExamHTML += "</li>";
                    }
                }
            }

            return PhysicalExamHTML;
        }

        #endregion Template Data Print

        #region " Note Session Data "

        internal string RollBackNote(long NoteId)
        {
            //Delete Note
            BLObject<string> obj = BLLClinicalObj.deleteClinical_Notes(MDVUtility.ToStr(NoteId));
            if (obj.Data == "")
            {
                // Remove From Session
                RemoveNoteSessionData(NoteId);

                var response = new
                {
                    status = true,
                    IsNoteDeleted = true,
                    Message = "OK"
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
            else
                throw new Exception(obj.Data);
        }
        internal string CheckNoteSessionData(long NoteId, bool IsDirectRollBack)
        {
            try
            {
                if (IsDirectRollBack)
                {
                    return RollBackNote(NoteId);
                }
                else
                {

                    if (HttpContext.Current.Session["NoteSessionModelList"] != null)
                    {
                        List<NoteSessionModel> noteList = (List<NoteSessionModel>)HttpContext.Current.Session["NoteSessionModelList"];
                        List<NoteSessionModel> list_ = noteList.Where(p => p.NoteId == NoteId).ToList<NoteSessionModel>();
                        if (list_.Count > 0)
                        {
                            RollBackNote(NoteId);

                            var response = new
                            {
                                status = true,
                                IsNoteComponentsAttached = false,
                                Message = list_.FirstOrDefault().ComponentName + " has Error: " + list_.FirstOrDefault().ErrorMessage
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                IsNoteComponentsAttached = true,
                                Message = "OK"
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {

                        var response = new
                        {
                            status = true,
                            IsNoteComponentsAttached = true,
                            Message = "OK"
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
                    IsNoteComponentsAttached = true,
                    Message = ex.Message
                };
                MDVLogger.PresentationErrorLog("BLLClinical::RollBackClinical_Notes::NoteId:" + NoteId, ex, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }

        }
        internal void SaveNoteSessionData(long NoteId, string ComponentName, string ErrorMessage)
        {
            NoteSessionModel model = new NoteSessionModel();
            model.ComponentName = ComponentName;
            model.ErrorMessage = ErrorMessage;
            model.IsErrored = true;
            model.NoteId = NoteId;
            List<NoteSessionModel> noteList = new List<NoteSessionModel>();

            if (HttpContext.Current.Session["NoteSessionModelList"] != null)
            {
                noteList = (List<NoteSessionModel>)HttpContext.Current.Session["NoteSessionModelList"];
                noteList.Add(model);
                HttpContext.Current.Session["NoteSessionModelList"] = noteList;
            }
            else
            {
                noteList.Add(model);
                HttpContext.Current.Session["NoteSessionModelList"] = noteList;
            }
        }
        internal void RemoveNoteSessionData(long NoteId)
        {
            if (HttpContext.Current.Session["NoteSessionModelList"] != null)
            {
                List<NoteSessionModel> noteList = (List<NoteSessionModel>)HttpContext.Current.Session["NoteSessionModelList"];
                List<NoteSessionModel> list_ = noteList.Where(p => p.NoteId == NoteId).ToList<NoteSessionModel>();
                foreach (var item in list_)
                    noteList.Remove(item);
                HttpContext.Current.Session["NoteSessionModelList"] = noteList;
            }
        }

        public class NoteSessionModel
        {
            public string ComponentName { get; set; }
            public string ErrorMessage { get; set; }
            public bool IsErrored { get; set; }
            public long NoteId { get; set; }
        }

        #endregion

        public static string CalculatePatientAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;

            if (Years > 0)
                return Convert.ToString(Years) + " Year(s)";
            else if (Months > 0)
                return Convert.ToString(Months) + " Month(s)";
            else if (Days > 0)
                return Convert.ToString(Days) + " Day(s)";
            else
                return "0";


            //  return String.Format("Age: {0} Year(s) {1} Month(s) {2} Day(s) {3} Hour(s) {4} Second(s)",
            //  Years, Months, Days, Hours, Seconds);
        }

        #region SignNotes
        public string Sign_Note(Int64 NotesId, string FromCCM, long VisitId, string VisitDate, long ProviderId, bool IsFromProgressNote, bool ConfirmSign, string NoteMissingDataReason)
        {
            try
            {
                //string status = "Signed";
                //string SignatureSOAPText = "";
                NoteComponentModel NoteComponentModel = null;
                BLObject<NoteComponentModel> obj;
                if (NotesId > 0)
                {

                    #region com
                    //  BLObject<long> obj = BLLClinicalObj.Signed_BillingInfo(NotesId, MDVUtility.ToInt64(PatientId), BillingInfoId, "");
                    /*
                    #region LoadVBP and CQM
                    string responseVBP = loadCQMWWithReasoning(MDVUtility.ToLong(ProviderId), VisitDate, VisitDate, MDVUtility.ToStr(VisitId), MDVUtility.ToStr(PatientId), 0, null, 0, NotesId, "1");
                    if (true)
                    {
                        //return responseVBP;
                    }

                    string responseCQM = loadCQMWWithReasoning(MDVUtility.ToLong(ProviderId), VisitDate, VisitDate, MDVUtility.ToStr(VisitId), MDVUtility.ToStr(PatientId), 0, null, 0, NotesId);
                    if (true)
                    {
                        // return responseCQM;
                    }
                    #endregion LoadVBP and CQM
                    */
                    /*
                    #region Billinginfo
                    DSBillingInformation dsBillingInformation = null;
                    BLObject<DSBillingInformation> objBinfo = BLLClinicalObj.BillingInfo_SELECT_By_VisitId(VisitId);
                    dsBillingInformation = objBinfo.Data;
                    BillingInformationModel result = new BillingInformationModel();
                    if (dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsBillingInformation.Tables[dsBillingInformation.BillingInfo.TableName].Rows[0];
                        BillingInfoId = MDVUtility.ToLong(dr[dsBillingInformation.BillingInfo.BillingInfoIdColumn.ColumnName]);

                        #region BillingInfo Fill For update
                        result.BillingInfoId = MDVUtility.ToStr(BillingInfoId);
                        result.ENMTypeId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMTypeIdColumn.ColumnName]);
                        result.ENMTimeId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMTimeIdColumn.ColumnName]);
                        result.ENMCPTCode = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTCodeColumn.ColumnName]);
                        result.ENMCPTDescription = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDescriptionColumn.ColumnName]);
                        result.ENMCPTUnit = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTUnitColumn.ColumnName]);
                        result.ENMCPTDOSFrom = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDOSFromColumn.ColumnName]);
                        result.ENMCPTDOSTo = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ENMCPTDOSToColumn.ColumnName]);
                        result.PatientId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.PatientIdColumn.ColumnName]);
                        result.NotesId = MDVUtility.ToInt64(dr[dsBillingInformation.BillingInfo.NotesIdColumn.ColumnName]);
                        result.VisitId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.VisitIdColumn.ColumnName]);
                        result.ProviderId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ProviderIdColumn.ColumnName]);
                        result.Status = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.StatusColumn.ColumnName]);
                        result.IsActive = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.IsActiveColumn.ColumnName]);
                        result.CreatedBy = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.CreatedByColumn.ColumnName]);
                        result.CreatedOn = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.CreatedOnColumn.ColumnName]);
                        result.ModifiedBy = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ModifiedByColumn.ColumnName]);
                        result.ModifiedOn = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ModifiedOnColumn.ColumnName]);
                        result.SoapText = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.SoapTextColumn.ColumnName]);
                        result.POS = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.POSColumn.ColumnName]);
                        result.Facility = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.FacilityColumn.ColumnName]);
                        result.FacilityId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.FacilityIdColumn.ColumnName]);
                        result.Provider = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ProviderColumn.ColumnName]);

                        result.ResourceProviderId = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ResourceProviderIdColumn.ColumnName]);
                        result.ResourceProvider = MDVUtility.ToStr(dr[dsBillingInformation.BillingInfo.ResourceProviderColumn.ColumnName]);
                        #endregion BillingInfo Fill For update

                        BLObject<DSBillingInformation> objICD = BLLClinicalObj.BillingInfoICD_SELECT(BillingInfoId);
                        dsBillingInformation.Merge(objICD.Data);
                        BLObject<DSBillingInformation> objCPT = BLLClinicalObj.BillingInfoCPT_SELECT(BillingInfoId);
                        dsBillingInformation.Merge(objCPT.Data);
                    }
                    else
                    {
                        #region billingInfo save
                        DSBillingInformation.BillingInfoRow cdsRow = null;
                        cdsRow = dsBillingInformation.BillingInfo.NewBillingInfoRow();
                        if (cdsRow != null)
                        {
                            cdsRow[dsBillingInformation.BillingInfo.ENMTypeIdColumn.ColumnName] = DBNull.Value;
                            cdsRow[dsBillingInformation.BillingInfo.ENMTimeIdColumn.ColumnName] = DBNull.Value;
                            cdsRow[dsBillingInformation.BillingInfo.ENMCPTCodeColumn.ColumnName] = DBNull.Value;
                            cdsRow[dsBillingInformation.BillingInfo.ENMCPTDescriptionColumn.ColumnName] = DBNull.Value;
                            cdsRow[dsBillingInformation.BillingInfo.ENMCPTUnitColumn.ColumnName] = DBNull.Value;
                            cdsRow[dsBillingInformation.BillingInfo.ENMCPTDOSFromColumn.ColumnName] = DBNull.Value;
                            cdsRow[dsBillingInformation.BillingInfo.ENMCPTDOSToColumn.ColumnName] = DBNull.Value;
                            if (PatientId == 0)
                                cdsRow.PatientId = PatientId;
                            else
                                cdsRow[dsBillingInformation.BillingInfo.PatientIdColumn.ColumnName] = DBNull.Value;
                            if (NotesId == 0)
                            {
                                cdsRow[dsBillingInformation.BillingInfo.NotesIdColumn.ColumnName] = NotesId;
                            }
                            else
                            {
                                cdsRow[dsBillingInformation.BillingInfo.NotesIdColumn.ColumnName] = DBNull.Value;
                            }
                            if (VisitId == 0)
                                cdsRow.VisitId = VisitId;
                            else
                                cdsRow[dsBillingInformation.BillingInfo.VisitIdColumn.ColumnName] = DBNull.Value;
                            if (ProviderId == 0)
                                cdsRow.ProviderId = ProviderId;
                            else
                                cdsRow[dsBillingInformation.BillingInfo.ProviderIdColumn.ColumnName] = DBNull.Value;
                            cdsRow.Status = status;
                            cdsRow[dsBillingInformation.BillingInfo.BillingInfoTypeColumn.ColumnName] = DBNull.Value;
                            cdsRow[dsBillingInformation.BillingInfo.FacilityIdColumn.ColumnName] = FacilityId;
                            cdsRow[dsBillingInformation.BillingInfo.ResourceProviderIdColumn.ColumnName] = DBNull.Value;
                            cdsRow.IsActive = true;
                            cdsRow.SoapText = "";
                            cdsRow.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsRow.CreatedOn = DateTime.Now;
                            cdsRow.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            cdsRow.ModifiedOn = DateTime.Now;

                            dsBillingInformation.BillingInfo.AddBillingInfoRow(cdsRow);

                        }
                        #endregion billinginfo save
                    }

                    #region JSON
                    Dictionary<string, string> objJSON = new Dictionary<string, string>
                        {
                        {"dtpHoldTill", ""},
                        {"txtPatientName", ""},
                        {"txtPatientFullName", ""},
                        {"dtpAppointmentDate", ""},
                        {"dtpEncounterSignOffDate", ""},
                        {"dtpDOSFrom", ""},
                        {"dtpDOSTo", ""},
                        {"dtpSubmittedDate", ""},
                        {"txtSubmittedBy", ""},
                        {"txtReferralNumber", ""},
                        {"txtCaseNumber", ""},
                        {"txtBatchNumber", ""},
                        {"txtClaimNumber", ""},
                        {"txtProvider", ""},
                        {"txtFacility", ""},
                        {"txtRefProvider", ""},
                        {"txtResourceProvider", ""},
                        {"txtSupervisingProvider", ""},
                        {"dtpClaimDate", ""},
                        {"txtVisitCopayment", "0"},
                        {"txtPriorAuthNumber", ""},
                        {"txtClaimComments", ""},
                        {"PatientOutstanding_SelectedDataKeys", ""},
                        {"txtPatCharges", "0.00"},
                        {"txtInsCharges", "0.00"},
                        {"ddlBox24BShaded", "1"},
                        {"ddlBox24IJShaded", "1"},
                        {"txtTotalCharges", "0.00"},
                        {"txtPatPayments", "0.00"},
                        {"txtInsPayments", "0.00"},
                        {"txtTotalPayments", "0.00"},
                        {"txtPatAdjust", "0.00"},
                        {"txtInsAdjust", "0.00"},
                        {"txtTotalAdjust", "0.00"},
                        {"txtPatBalances", "0.00"},
                        {"txtInsBalances", "0.00"},
                        {"txtTotalBalances", "0.00"},
                        {"txtAnesthesiologist", ""},
                        {"txtAnesthesiologistStartTime", ""},
                        {"txtAnesthesiologistEndTime", ""},
                        {"txtCRNA", ""},
                        {"txtCRNAStartTime", ""},
                        {"txtCRNAEndTime", ""},
                        {"txtRiskUnits", ""},
                        {"txtAnesthesiaComments", ""},
                        {"txtICD1", ""},
                        {"hfICD1", ""},
                        {"hfICD101", ""},
                        {"hfICDDescription1", ""},
                        {"hfSNOMED1", ""},
                        {"hfSNOMEDDescription1", ""},
                        {"txtICD10Description1", ""},
                        {"txtICD4", ""},
                        {"hfICD4", ""},
                        {"hfICD104", ""},
                        {"hfICDDescription4", ""},
                        {"hfSNOMED4", ""},
                        {"hfSNOMEDDescription4", ""},
                        {"txtICD10Description4", ""},
                        {"txtICD7", ""},
                        {"hfICD7", ""},
                        {"hfICD107", ""},
                        {"hfICDDescription7", ""},
                        {"hfSNOMED7", ""},
                        {"hfSNOMEDDescription7", ""},
                        {"txtICD10Description7", ""},
                        {"txtICD10", ""},
                        {"hfICD10", ""},
                        {"hfICD1010", ""},
                        {"hfICDDescription10", ""},
                        {"hfSNOMED10", ""},
                        {"hfSNOMEDDescription10", ""},
                        {"txtICD10Description10", ""},
                        {"txtICD2", ""},
                        {"hfICD2", ""},
                        {"hfICD102", ""},
                        {"hfICDDescription2", ""},
                        {"hfSNOMED2", ""},
                        {"hfSNOMEDDescription2", ""},
                        {"txtICD10Description2", ""},
                        {"txtICD5", ""},
                        {"hfICD5", ""},
                        {"hfICD105", ""},
                        {"hfICDDescription5", ""},
                        {"hfSNOMED5", ""},
                        {"hfSNOMEDDescription5", ""},
                        {"txtICD10Description5", ""},
                        {"txtICD8", ""},
                        {"hfICD8", ""},
                        {"hfICD108", ""},
                        {"hfICDDescription8", ""},
                        {"hfSNOMED8", ""},
                        {"hfSNOMEDDescription8", ""},
                        {"txtICD10Description8", ""},
                        {"txtICD11", ""},
                        {"hfICD11", ""},
                        {"hfICD1011", ""},
                        {"hfICDDescription11", ""},
                        {"hfSNOMED11", ""},
                        {"hfSNOMEDDescription11", ""},
                        {"txtICD10Description11", ""},
                        {"txtICD3", ""},
                        {"hfICD3", ""},
                        {"hfICD103", ""},
                        {"hfICDDescription3", ""},
                        {"hfSNOMED3", ""},
                        {"hfSNOMEDDescription3", ""},
                        {"txtICD10Description3", ""},
                        {"txtICD6", ""},
                        {"hfICD6", ""},
                        {"hfICD106", ""},
                        {"hfICDDescription6", ""},
                        {"hfSNOMED6", ""},
                        {"hfSNOMEDDescription6", ""},
                        {"txtICD10Description6", ""},
                        {"txtICD9", ""},
                        {"hfICD9", ""},
                        {"hfICD109", ""},
                        {"hfICDDescription9", ""},
                        {"hfSNOMED9", ""},
                        {"hfSNOMEDDescription9", ""},
                        {"txtICD10Description9", ""},
                        {"txtICD12", ""},
                        {"hfICD12", ""},
                        {"hfICD1012", ""},
                        {"hfICDDescription12", ""},
                        {"hfSNOMED12", ""},
                        {"hfSNOMEDDescription12", ""},
                        {"txtICD10Description12", ""},
                        {"VisitCharge_SelectedDataKeys", ""},
                        {"dtpDOSFrom", ""},
                        {"dtpDOSTo", ""},
                        {"txtCPT", ""},
                        {"hfCPT", ""},
                        {"hfCPTDescription", ""},
                        {"txtModifier1", ""},
                        {"txtModifier2", ""},
                        {"txtModifier3", ""},
                        {"txtModifier4", ""},
                        {"txtICDPointer1", ""},
                        {"txtICDPointer2", ""},
                        {"txtICDPointer3", ""},
                        {"txtICDPointer4", ""},
                        {"txtPOS", ""},
                        {"hfPOSId", "0"},
                        {"txtTOS", ""},
                        {"hfTOSId", "0"},
                        {"txtPrimaryFEE", "0.00"},
                        {"hfPrimaryFee", "0"},
                        {"txtFEE", "0.00"},
                        {"hfFee", "0.00"},
                        {"hfExpectedFee", "0.00"},
                        {"hfTotalBalance", "0.00"},
                        {"hfAssignBenefits", ""},
                        {"hfCurrentChargeCPT", ""},
                        {"txtUnits", "1"},
                        {"hfUnits", "1"},
                        {"txtTotalFEE", "0.00"},
                        {"txtINSCharges", "0.00"},
                        {"txtPATCharges", "0.00"},
                        {"txtCOPAY", "0.00"},
                        {"txtPriorAuthorization", ""},
                        {"hfAuthorizationId", ""},
                        {"txtExpectedFee", "0.00"},
                        {"txtNDC", ""},
                        {"txtNDCUnit", ""},
                        {"txtNDCUnitPrice", "0"},
                        {"txtComments", ""},
                        {"txtCLIA", ""},
                        {"dtpUnableToWorkFrom", ""},
                        {"dtpUnableToWorkTo", ""},
                        {"dtpLMPDate", ""},
                        {"txtOrderingProvider", ""},
                        {"dtpCurrentIllnessDate", ""},
                        {"txtICN_DCN", ""},
                        {"dtpAdmissionDate", ""},
                        {"dtpDischargeDate", ""},
                        {"dtpLastSeenDate", ""},
                        {"txtComments", ""},
                        {"dtpInjuryDate", ""},
                        {"txtState", ""},
                        {"txtControlNumber", ""},
                        {"dtpDocumentSentDate", ""},
                        {"hfPatientId", ""},
                        {"hfProvider", ""},
                        {"hfPractice", ""},
                        {"hfRefProvider", ""},
                        {"hfOrderingProvider", ""},
                        {"hfSupervisingProvider", ""},
                        {"hfResourceProvider", ""},
                        {"hfFacility", ""},
                        {"hfInsurancePlan", ""},
                        {"hfCaseId", ""},
                        {"hfPOS", ""},
                        {"hfIsSubmitted", ""},
                        {"hfBatchId", ""},
                        {"hfChargeRowId", ""},
                        {"hfPOSId", ""},
                        {"hfVisitDate", ""},
                        {"hfMasterVisitId", ""},
                        {"hfPatientReferralId", ""},
                        {"hfReferralNumerId", ""},
                        {"hfAuthorizeId", ""},
                        {"hfPVICDIdsLength", ""},
                        {"hfAnesthesiologist", ""},
                        {"hfCRNA", ""},
                        {"hfIsAnesthesiaBilling", "false"},
                        {"chkBillToPatientForIns", "false"},
                        {"chkIsCleanclaim", "false"},
                        {"radAnesthesiologist", "false"},
                        {"radCRNA", "true"},
                        {"radPCP", "true"},
                        {"radSpecialist", "false"},
                        {"chkPaid", "false"},
                        {"chkAllCharges", "false"},
                        {"chkActive", "true"},
                        {"chkEMG", "false"},
                        {"chkPrintonHCFAF19", "false"},
                        {"chkAssgBenefits", "false"},
                        {"chkIsReportNPI", "false"},
                        {"RadEmploymentYes", "false"},
                        {"RadEmploymentNo", "true"},
                        {"RadAutoYes", "false"},
                        {"RadAutoNo", "true"},
                        {"RadOtherYes", "false"},
                        {"RadOtherNo", "true"},
                        {"chkBillToPatient", "false"},
                        {"ddlSubmitStatus", "1"},
                        {"ddlSubmitStatus_text", "Pending"},
                        {"ddlBillingProvider", ""},
                        {"ddlBillingProvider_text", "- Select -"},
                        {"ddlClaimType", "1"},
                        {"ddlClaimType_text", "Professional Medical"},
                        {"ddlLastVisits", ""},
                        {"ddlLastVisits_text", "- Select -"},
                        {"ddlAnesthesiaType", ""},
                        {"ddlAnesthesiaType_text", "- Select -"},
                        {"ddlASA", ""},
                        {"ddlASA_text", "- Select -"},
                        {"ddlServiceType", ""},
                        {"ddlServiceType_text", "- Select -"},
                        {"ddlNDCMeasurement", ""},
                        {"ddlNDCMeasurement_text", "- Select -"},
                        {"ddlDelayReason", ""},
                        {"ddlDelayReason_text", "- Select -"},
                        {"ddlClaimFrequency", ""},
                        {"ddlClaimFrequency_text", "- Select -"},
                        {"ddlReportTypeCode", ""},
                        {"ddlReportTypeCode_text", "- Select -"},
                        {"ddlTransmissionCode", ""},
                        {"ddlTransmissionCode_text", "- Select -"},
                        };
                    #endregion JSON
                    BLObject<DSBillingInformation> objBInfoS = BLLClinicalObj.BillingInfo_Save(dsBillingInformation, MDVUtility.ToStr(PatientId));
                    BLObject<DSBillingInformation> obj = BLLClinicalObj.BillingInfoICD_SELECT(MDVUtility.ToInt64(BillingInfoId));
                    #endregion Billinginfo

                    #region LoadCPT and ICD
                    DSProblemLists dSProblemLists = null;
                    DSProcedures dsProcedure = new DSProcedures();
                    BLObject<DSProblemLists> objPrbPro = BLLClinicalObj.LoadProblemAndProcedureList(NotesId, PatientId);
                    dSProblemLists = objPrbPro.Data;

                    #region Fill BillingInfo ICD
                    List<BillingInfoICDModel> lstICD = new List<BillingInfoICDModel>();
                    for (int r = 0; r < dSProblemLists.Tables[dSProblemLists.ProblemList.TableName].Rows.Count; r++)
                    {
                        BillingInfoICDModel icd = new BillingInfoICDModel();
                        DataRow dr = dSProblemLists.Tables[dSProblemLists.ProblemList.TableName].Rows[r];
                        icd.ICDCode10 = MDVUtility.ToStr(dr[dSProblemLists.ProblemList.ICD10Column.ColumnName]);
                        icd.ICDDescription10 = MDVUtility.ToStr(dr[dSProblemLists.ProblemList.ICD10_DescriptionColumn.ColumnName]);
                        icd.ICDCode9 = MDVUtility.ToStr(dr[dSProblemLists.ProblemList.ICD9Column.ColumnName]);
                        icd.ICDDescription9 = MDVUtility.ToStr(dr[dSProblemLists.ProblemList.ICD9_DescriptionColumn.ColumnName]);
                        icd.SNOMEDCode = MDVUtility.ToStr(dr[dSProblemLists.ProblemList.SNOMEDIDColumn.ColumnName]);
                        icd.SNOMEDDescription = MDVUtility.ToStr(dr[dSProblemLists.ProblemList.SNOMED_DESCRIPTIONColumn.ColumnName]);
                        lstICD.Add(icd);
                    }

                    #endregion Fill BillingInfo ICD

                    #region Fill BillingInfo CPT
                    List<BillingInfoCPTModel> lstCPT = new List<BillingInfoCPTModel>();
                    for (int r = 0; r < dSProblemLists.Tables[dsProcedure.Procedures.TableName].Rows.Count; r++)
                    {
                        BillingInfoCPTModel cpt = new BillingInfoCPTModel();
                        DataRow dr = dSProblemLists.Tables[dsProcedure.Procedures.TableName].Rows[r];

                        cpt.CPTCode = MDVUtility.ToStr(dr[dsProcedure.Procedures.CPTCodeColumn.ColumnName]);
                        cpt.CPTDescription = MDVUtility.ToStr(dr[dsProcedure.Procedures.CPT_DESCRIPTIONColumn.ColumnName]);
                        cpt.Modifier1 = MDVUtility.ToStr(dr[dsProcedure.Procedures.Modifier1Column.ColumnName]);
                        cpt.Modifier2 = "";
                        cpt.Modifier3 = "";
                        cpt.Modifier4 = "";
                        cpt.UnitsId = MDVUtility.ToStr(dr[dsProcedure.Procedures.UnitColumn.ColumnName]);
                        cpt.DOSFrom = VisitDate;
                        cpt.DOSTo = VisitDate;
                        cpt.CPTSNOMEDCodeId = MDVUtility.ToStr(dr[dsProcedure.Procedures.SNOMEDIDColumn.ColumnName]);
                        cpt.CPTSNOMEDDescription = MDVUtility.ToStr(dr[dsProcedure.Procedures.SNOMED_DESCRIPTIONColumn.ColumnName]);
                        cpt.POS = POS;
                        lstCPT.Add(cpt);
                    }
                    #endregion Fill BillingInfo CPT
                    #endregion LoadCPT and ICD
                    */
                    #endregion

                    DSNotes dsNotes = new DSNotes();
                    DSNotes dsHeaderNotes = new DSNotes();
                    DSPatient dsPatientInfo = new DSPatient();
                    DSProfile dsProvider = new DSProfile();
                    ReportHeader_TagsSelectModel model = new ReportHeader_TagsSelectModel();
                    #region Database Insertion/Updation
                    obj = BLLClinicalObj.Sign_Note(NotesId, FromCCM, ConfirmSign, NoteMissingDataReason);
                    List<NoteComponentModel> NoteComponentList = null;
                    if (obj.Data != null)
                    {
                        if (obj.Data.ErrorMessage != "This note is already signed.")
                        {
                            NoteComponentModel = obj.Data;
                            if (ConfirmSign == true || (NoteComponentModel.IsProblemMissed == false && NoteComponentModel.IsProcedureMissed == false))
                            {
                                BLObject<List<NoteComponentModel>> objComp;
                                objComp = BLLClinicalObj.loadNoteComponents(NotesId);
                                NoteComponentList = objComp.Data;

                                BLObject<DSNotes> objHeaderLoad = BLLClinicalObj.loadClinicalNoteHeaderData(NoteComponentModel.PatientId, ProviderId, NotesId);
                                dsHeaderNotes = objHeaderLoad.Data;
                                dsNotes.Merge(dsHeaderNotes);

                                model = ReportHeaderHelper.Instance().getReportHeaderTagsHTML(NoteComponentModel.PatientId, ProviderId, NotesId, "Notes");

                                if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                                {
                                    if (NoteComponentModel.PatientId > 0)
                                    {
                                        var response = new
                                        {
                                            status = true,
                                            iTotalDisplayRecords = dsNotes.Notes.Rows[0][dsNotes.Notes.RecordCountColumn],
                                            NoteStatus = dsNotes.Notes.Rows[0][dsNotes.Notes.NoteStatusColumn],
                                            IsReviewed = Convert.ToBoolean(dsNotes.Notes.Rows[0][dsNotes.Notes.IsReviewedColumn]),
                                            NotesHTML = HttpUtility.HtmlDecode(dsNotes.Notes.Rows[0][dsNotes.Notes.NoteTextColumn.ColumnName].ToString()),
                                            NotesLoad_JSON = dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0 ? MDVUtility.JSON_DataTable(dsNotes.Tables[dsNotes.Notes.TableName]) : "[]",
                                            NoteHeaderPatientData = MDVUtility.JSON_DataTable(dsNotes.Tables[dsPatientInfo.Patients.TableName]),
                                            NoteHeaderProviderData = MDVUtility.JSON_DataTable(dsNotes.Tables[dsProvider.Provider.TableName]),
                                            NoteHeaderPracticeData = MDVUtility.JSON_DataTable(dsNotes.Tables[dsProvider.Practice.TableName]),
                                            ReportHeaderInfo = HttpUtility.HtmlDecode(model.Header),
                                            ReportFooterInfo = HttpUtility.HtmlDecode(model.Footer),
                                            PatientName = model.PatientName,
                                            PatienDOB = model.PatientDOB.ToString("MM/dd/yyyy"),
                                            ProviderName = model.ProviderName.ToString(),
                                            DOS = model.DOS.ToString("MM/dd/yyyy"),
                                            NoteComponentModel_JSON = NoteComponentModel,
                                            NoteComponentListFill_JSON = NoteComponentList,
                                        };
                                        return (JsonConvert.SerializeObject(response));
                                    }
                                    else
                                    {
                                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        var response = new
                                        {
                                            status = true,
                                            iTotalDisplayRecords = dsNotes.Notes.Rows[0][dsNotes.Notes.RecordCountColumn],
                                            NoteStatus = dsNotes.Notes.Rows[0][dsNotes.Notes.NoteStatusColumn],
                                            IsReviewed = Convert.ToBoolean(dsNotes.Notes.Rows[0][dsNotes.Notes.IsReviewedColumn]),
                                            NotesHTML = HttpUtility.HtmlDecode(dsNotes.Notes.Rows[0][dsNotes.Notes.NoteTextColumn.ColumnName].ToString()),
                                            NotesLoad_JSON = dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0 ? MDVUtility.JSON_DataTable(dsNotes.Tables[dsNotes.Notes.TableName]) : "[]",
                                            NoteHeaderPatientData = js.Serialize("[]"),
                                            NoteHeaderProviderData = js.Serialize("[]"),
                                            NoteHeaderPracticeData = js.Serialize("[]"),
                                            ReportHeaderInfo = HttpUtility.HtmlDecode(model.Header),
                                            ReportFooterInfo = HttpUtility.HtmlDecode(model.Footer),
                                            PatientName = model.PatientName.ToString(),
                                            PatienDOB = model.PatientDOB.ToString(),
                                            NoteComponentModel_JSON = NoteComponentModel,
                                            NoteComponentListFill_JSON = NoteComponentList,
                                        };
                                        return (JsonConvert.SerializeObject(response));
                                    }
                                }
                                else
                                {
                                    var response = new
                                    {
                                        status = true,
                                        MessageCount = 0,
                                        iTotalDisplayRecords = 0,
                                        Message = "No Notes Found."
                                    };

                                    return (JsonConvert.SerializeObject(response));
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    NoteComponentModel_JSON = NoteComponentModel,
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = obj.Data.ErrorMessage
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
                    #endregion Database Insertion/Updation

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Please select a Provider Note to update."
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

        // Get notes data for pdf generation for bulk sign note
        public string Sign_Not_get(string NotesIds)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(NotesIds))
                {
                    #region Database Insertion/Updation
                    BLObject<NotesViewModel> objComp;
                    objComp = BLLClinicalObj.LoadNotesDataBulkSign(NotesIds, MDVSession.Current.AppUserId, MDVUtility.ToInt64(MDVSession.Current.EntityId), 1);
                    if (objComp.Data != null)
                    {
                        if (objComp.Data.RptHdrTagsList.Any())
                        {
                            objComp.Data.RptHdrTagsList = ReportHeaderHelper.Instance().generateHeaderFooterHTMLBulk(objComp.Data.RptHdrTagsList);
                        }
                        if (objComp.Data.NotesModelList.Any())
                        {
                            objComp.Data.NotesModelList.ForEach(m => m.NoteText = HttpUtility.HtmlDecode(m.NoteText));
                        }
                        var response = new
                        {
                            status = true,
                            NotesLoad_JSON = objComp.Data.NotesModelList,
                            NoteHeaderPatientData = objComp.Data.PatientList,
                            NoteHeaderProviderData = objComp.Data.ProviderList,
                            NoteHeaderPracticeData = objComp.Data.PracticeList,
                            ReportHeaderInfo = objComp.Data.RptHdrTagsList,
                            NoteComponentListFill_JSON = objComp.Data.NoteComponentModelList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = false,
                            Message = objComp.Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    #endregion Database Insertion/Updation
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Please select a Provider Note to update."
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

        public string Sign_Note_Multiple(string NotesIds, string FromCCM, bool IsFromProgressNote, string NoteMissingDataReason)
        {
            try
            {
                BLObject<List<NoteComponentModel>> obj;
                if (!string.IsNullOrWhiteSpace(NotesIds))
                {
                    DataTable dtNotesIds = new DataTable();
                    dtNotesIds = MDVUtility.ConvertCommaSepatedValuesToDataTable(NotesIds);
                    #region Database Insertion/Updation
                    obj = BLLClinicalObj.Sign_Note_Multiple(FromCCM, IsFromProgressNote, dtNotesIds, NoteMissingDataReason);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            NoteComponentModel = obj.Data,
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
                    #endregion Database Insertion/Updation
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Please select a provider note to sign."
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

        public string NoteReadytoSign_multiple(string NotesIds, string FromCCM, bool IsFromProgressNote)
        {
            try
            {
                BLObject<List<NoteComponentModel>> obj;
                if (!string.IsNullOrWhiteSpace(NotesIds))
                {
                    DataTable dtNotesIds = new DataTable();
                    dtNotesIds = MDVUtility.ConvertCommaSepatedValuesToDataTable(NotesIds);
                    #region Database Insertion/Updation
                    obj = BLLClinicalObj.NoteReadytoSign_multiple(dtNotesIds);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            NoteComponentModel = obj.Data,
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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
                    #endregion Database Insertion/Updation
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Please select a provider note to mark as ready to sign."
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

        private string GeteSignature(bool IsPhoneEncounter, bool FromCCM, string ProviderName, long ResourceProviderId, string ResourceProvider)
        {
            string signature = "";
            string userName = MDVSession.Current.AppUserFullName;
            string signeDateTime = String.Format("{0:F}", DateTime.Now.ToLocalTime());
            string ResourceproQualification = "";
            string SignProvider = "";
            string ProvQualification = "";
            signature = "<ul class='list-unstyled SignatureComponent' NoteComponentId='NCDummyId'><li> e-Signed by: " + MDVSession.Current.AppUserFullName + " on " + String.Format("{0:F}", DateTime.Now.ToLocalTime()) + "</li></ul>";
            if (!IsPhoneEncounter)
            {
                if (!FromCCM)
                {
                    if (ResourceProviderId > 0)
                    {
                        signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled'" + "NoteComponentId='NCDummyId'><li id='signedBy'>e-Signed by: " + userName + " on " + signeDateTime + "</li><li " + "id='ResourceProvidersign'>" + ResourceProvider + " " + ResourceproQualification + " I, " + SignProvider + " " + ProvQualification + " agree with " + ResourceProvider + " regarding the findings and plan of care as documented note above.</li></ul>";
                    }
                    else
                    {
                        signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled'" + "NoteComponentId='NCDummyId'><li id='signedBy'>e-Signed by: " + userName + " on " + signeDateTime + "</li></ul>";
                    }
                }
                else
                {
                    // CCM
                    if (userName == ProviderName)
                    {
                        if (ResourceProviderId > 0)
                        {
                            signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled' NoteComponentId='NCDummyId'><li id='signedBy'>e-Signed by: " + userName + " on " + signeDateTime + "</li><li id='ResourceProvidersign'>" + ResourceProvider + " " + ResourceproQualification + " I, " + SignProvider + " " + ProvQualification + " agree with " + ResourceProvider + " regarding the findings and plan of care as documented note above.</li></ul>";
                        }
                        else
                        {
                            signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled'" + "NoteComponentId='NCDummyId'><li id='signedBy'>e-Signed by: " + userName + " on " + signeDateTime + "</li></ul>";
                        }
                    }
                    else
                    {
                        if (ResourceProviderId > 0)
                        {
                            signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled' NoteComponentId='NCDummyId'><li id='signedBy'>e-Signed by: " + userName + " on " + signeDateTime + "</li><li id='ResourceProvidersign'>" + ResourceProvider + " " + ResourceproQualification + " I, " + SignProvider + " " + ProvQualification + " agree with " + ResourceProvider + " regarding the findings and plan of care as documented note above.</li></ul>";
                        }
                        else
                        {
                            signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled' NoteComponentId='NCDummyId'><li id='signedBy'>Chronic Care Manager completed 20 minutes or greater of non face to face contact with this patient during the course of the month. The patient\"s care plan was reviewed and I " + ProviderName + " agree with " + userName + " regarding instructions provided to this patient.</li></ul>";

                        }
                    }
                }
            }
            else
            {
                if (!FromCCM)
                {
                    if (ResourceProviderId > 0)
                    {
                        signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled' NoteComponentId='NCDummyId'><li id='signedBy'>e-Signed by: " + userName + " on " + signeDateTime + "</li><li id='ResourceProvidersign'>" + ResourceProvider + " " + ResourceproQualification + " I, " + SignProvider + " " + ProvQualification + " agree with " + ResourceProvider + " regarding the findings and plan of care as documented note above.</li></ul>";
                    }
                    else
                    {
                        signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled' NoteComponentId='NCDummyId'><li id='signedBy'>e-Signed by: " + userName + " on " + signeDateTime + "</li></ul>";
                    }
                }
                else
                {
                    // CCM
                    if (userName == ProviderName)
                    {
                        if (ResourceProviderId > 0)
                        {
                            signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled' NoteComponentId='NCDummyId'><li id='signedBy'>e-Signed by: " + userName + " on " + signeDateTime + "</li><li id='ResourceProvidersign'>" + ResourceProvider + " " + ResourceproQualification + " I, " + SignProvider + " " + ProvQualification + " agree with " + ResourceProvider + " regarding the findings and plan of care as documented note above.</li></ul>";
                        }
                        else
                        {
                            signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled' NoteComponentId='NCDummyId'><li id='signedBy'>e-Signed by: " + userName + " on " + signeDateTime + "</li></ul>";
                        }
                    }
                    else
                    {
                        if (ResourceProviderId > 0)
                        {
                            signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled' NoteComponentId='NCDummyId'><li id='signedBy'>e-Signed by: " + userName + " on " + signeDateTime + "</li><li id='ResourceProvidersign'>" + ResourceProvider + " " + ResourceproQualification + " I, " + SignProvider + " " + ProvQualification + " agree with " + ResourceProvider + " regarding the findings and plan of care as documented note above.</li></ul>";
                        }
                        else
                        {
                            signature = "<ul id='signedByProvider' class='SignatureComponent list-unstyled' NoteComponentId='NCDummyId'><li id='signedBy'>Chronic Care Manager completed 20 minutes or greater of non face to face contact with this patient during the course of the month. The patient\"s care plan was reviewed and I " + ProviderName + " agree with " + userName + " regarding instructions provided to this patient.</li></ul>";
                        }
                    }
                }
            }
            return signature;
        }

        private string GetSignature(string imageBase64)
        {
            return "<div class='SignatureComponent' NoteComponentId='NCDummyId' style='max-height:350px; overflow-y:auto;margin-top:15px;' >" +
                    "<img id='img_eSignature_ProgressNotes' src='" + imageBase64.ToString() + "' " +
                    "alt='' style='height: 125px; width: 315px;border:none;' " +
                    "class='img-responsive img-center mt-lg img-thumbnail'/></div>";
        }

        #endregion SignNotes

        #region "NotesAccess"
        public string SaveNotesAccess(ClinicalNotesExtraInfoModel model)
        {
            try
            {
                DSNotes dsNotes = new DSNotes();

                BLObject<DSNotes> obj;
                obj = BLLClinicalObj.GetReferenceData();
                dsNotes = obj.Data;
                if (obj.Data != null)
                {
                    DSNotes.PatientMUSettingRow drPatientMUSetting = dsNotes.PatientMUSetting.NewPatientMUSettingRow();
                    foreach (DSNotes.ReferenceDataRow dr in dsNotes.ReferenceData)
                    {

                        if (dr.Description == model.Description)
                        {
                            drPatientMUSetting.PatientMUSettingId = -1;
                            drPatientMUSetting.NoteId = MDVUtility.ToLong(model.NoteId);
                            drPatientMUSetting.PatientId = MDVUtility.ToLong(model.PatientId);
                            drPatientMUSetting.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            drPatientMUSetting.CreatedOn = DateTime.Now;
                            drPatientMUSetting.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            drPatientMUSetting.ModifiedOn = DateTime.Now;
                            drPatientMUSetting.ReferenceDataId = dr.ReferenceDataId;

                            if (!string.IsNullOrEmpty(model.ValueSettingId))
                            {
                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.ValueSettingId);
                            }
                            else
                            {
                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                            }

                            dsNotes.PatientMUSetting.AddPatientMUSettingRow(drPatientMUSetting);
                        }
                    }
                    #region Database Insertion
                    BLObject<DSNotes> objSave = BLLClinicalObj.InsertPatientMUSettingWithMissingReference(dsNotes);
                    if (objSave.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = AppPrivileges.Save_Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objSave.Message
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
                        Message = obj.Message
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

        public string UpdateNotesAccess(ClinicalNotesExtraInfoModel model)
        {
            try
            {

                DSNotes dsNotes = null;
                BLObject<DSNotes> obj;
                obj = BLLClinicalObj.LoadPatientMUSetting(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId));
                dsNotes = obj.Data;
                ClinicalNotesExtraInfoModel NotesExtraInfoModel = new ClinicalNotesExtraInfoModel();
                DSNotes dsPatientMuInfoUpdate = new DSNotes();

                if (obj.Data != null)
                {
                    if (dsNotes.Tables[dsNotes.PatientMUSetting.TableName].Rows.Count > 0)
                    {
                        DSNotes dsReferralData = new DSNotes();
                        BLObject<DSNotes> objReferralData;
                        objReferralData = BLLClinicalObj.GetReferenceData();
                        dsReferralData = objReferralData.Data;
                        if (objReferralData.Data != null)
                        {
                            foreach (DSNotes.ReferenceDataRow dr in dsReferralData.ReferenceData)
                            {
                                if (dr.Description == model.Description)
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            drPatientMUSetting.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                                            drPatientMUSetting.ModifiedOn = DateTime.Now;
                                            if (!string.IsNullOrEmpty(model.ValueSettingId))
                                            {
                                                drPatientMUSetting.ValueSettingId = MDVUtility.ToInt32(model.ValueSettingId);
                                            }
                                            else
                                            {
                                                drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn] = DBNull.Value;
                                            }

                                            DSNotes.PatientMUSettingDataTable dt = new DSNotes.PatientMUSettingDataTable();
                                            dt.ImportRow(drPatientMUSetting);
                                            dsNotes.PatientMUSetting.Clear();
                                            dsNotes.PatientMUSetting.ImportRow(dt.Rows[0]);
                                            break;
                                        }
                                    }
                                    break;
                                }

                            }
                            #region Database Updation
                            BLObject<DSNotes> objUpdate = BLLClinicalObj.UpdatePatientMenuSetting(dsNotes);
                            if (objUpdate.Data != null)
                            {
                                var response = new
                                {
                                    status = true,
                                    Message = Common.AppPrivileges.Update_Message
                                };
                                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    Message = objUpdate.Message
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
                                Message = objReferralData.Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = Common.AppPrivileges.No_Record_Message
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

        public string SearchNotesAccess(ClinicalNotesExtraInfoModel model)
        {
            try
            {

                DSNotes dsNotes = null;
                BLObject<DSNotes> obj;
                obj = BLLClinicalObj.LoadPatientMUSetting(MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId));
                dsNotes = obj.Data;
                ClinicalNotesExtraInfoModel NotesExtraInfoModel = new ClinicalNotesExtraInfoModel();

                if (obj.Data != null)
                {
                    if (dsNotes.Tables[dsNotes.PatientMUSetting.TableName].Rows.Count > 0)
                    {
                        DSNotes dsReferralData = new DSNotes();
                        BLObject<DSNotes> objReferralData;
                        objReferralData = BLLClinicalObj.GetReferenceData();
                        dsReferralData = objReferralData.Data;
                        if (objReferralData.Data != null)
                        {
                            foreach (DSNotes.ReferenceDataRow dr in dsReferralData.ReferenceData)
                            {
                                if (dr.Description == "VDT (Patient)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.VDTPatient = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                                else if (dr.Description == "VDT API (Patient)")
                                {
                                    foreach (DSNotes.PatientMUSettingRow drPatientMUSetting in dsNotes.PatientMUSetting)
                                    {
                                        if (drPatientMUSetting.ReferenceDataId == dr.ReferenceDataId)
                                        {
                                            NotesExtraInfoModel.VDTAPIPatient = MDVUtility.ToStr(drPatientMUSetting[dsNotes.PatientMUSetting.ValueSettingIdColumn.ColumnName]);
                                            break;
                                        }
                                    }
                                }
                            }
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                Found = true,
                                NotesExtraData = js.Serialize(NotesExtraInfoModel)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objReferralData.Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Found = false,
                            Message = Common.AppPrivileges.No_Record_Message
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
        #endregion

        public string CreateNotesPDFFixesFiles(string NotesIds)
        {
            try
            {
                LegacyNotesViewModel objLegacyNotesViewModel = new LegacyNotesViewModel() { NotesComponent = new List<NotesComponent>() };
                List<NotesComponents> objListNotesComponents = new List<NotesComponents>();
                long notesid = 0;
                long patientid = 0;
                long providerid = 0;
                string ExistingFilePath = "";
                string ExistingFileName = "";
                dynamic response = null;
                BLObject<List<NotesPDFModel>> mdl = BLLClinicalObj.loadNoteFilePaths4DataFixPDF(NotesIds);

                BLObject<List<NoteComponentModel>> objct;
                string strNotes = string.Empty;
                foreach (var NotesID in NotesIds.Split(','))
                {
                    if (mdl.Data != null)
                    {
                        var pdfmdl = mdl.Data.FirstOrDefault(x => x.NotesId == NotesID);
                        if (pdfmdl != null)
                        {
                            ExistingFilePath = pdfmdl.NotePDF_FilePath;
                            ExistingFileName = pdfmdl.NotePDF_FileName;

                            objct = BLLClinicalObj.loadNoteComponentsDataFixPDF(MDVUtility.ToInt64(NotesID));
                            if (objct.Data != null)
                            {
                                foreach (var item in objct.Data)
                                {
                                    notesid = item.NotesId;
                                    patientid = item.PatientId;
                                    providerid = item.ProviderId;
                                    objListNotesComponents.Add(new NotesComponents() { Component = item.ComponentName, NotesId = item.NotesId, ProviderId = item.ProviderId, PatientId = item.PatientId });
                                }
                                objListNotesComponents.Add(new NotesComponents() { Component = "NotesComponent", NotesId = notesid, ProviderId = providerid, PatientId = patientid });
                                objLegacyNotesViewModel = BLLClinicalObj.LoadLegacyNoteAndRenderTemplate(objListNotesComponents);
                                #region Components
                                List<NotesComponent> objList_NotesComponent = objLegacyNotesViewModel.NotesComponent;
                                if (objList_NotesComponent == null)
                                    objList_NotesComponent = new List<NotesComponent>();

                                string NotesHtml = string.Empty;
                                int counterLeftPan = 0;
                                int counterRightPan = 0;

                                var dsReportHeader = new MDVision.DataAccess.DAL.ReportHeader.DALReportHeader().getReportHeaderTagsValue(objListNotesComponents.FirstOrDefault().PatientId, objListNotesComponents.FirstOrDefault().ProviderId, objListNotesComponents.FirstOrDefault().NotesId, "Notes");

                                DSReportHeader.ReportHeaderTagsRow drReportHeader = (DSReportHeader.ReportHeaderTagsRow)dsReportHeader.ReportHeaderTags.Rows[0];
                                drReportHeader.DOS.Replace("12:00AM", "");

                                string NoteReason = string.Empty;
                                if (objLegacyNotesViewModel.NoteHeaderData != null && objLegacyNotesViewModel.NoteHeaderData.Count() > 0)
                                {
                                    var obhNoteHeader = objLegacyNotesViewModel.NoteHeaderData.FirstOrDefault(m => m.Type == "Note");
                                    if (obhNoteHeader != null)
                                    {
                                        NoteReason = obhNoteHeader.NoteReason;
                                    }
                                    NotesHtml += NotesHeader(objLegacyNotesViewModel.NoteHeaderData, drReportHeader);
                                }

                                List<NotesComponent> FreeTextComponents = new List<NotesComponent>();
                                FreeTextComponents = objList_NotesComponent.OrderBy(m => m.OrderNo).ToList();
                                int OrderNoComponent = -1;


                                NotesHtml += "<div style='font-family:arial unicode ms;'><table style='width:100%;border-top-left-radius: 5px;'><tr style='border-top-left-radius: 5px;'><td style='border-left:solid 1px #EFEFEF;border-right:solid 1px #EFEFEF;border-bottom:solid 1px #EFEFEF;border-top:3px solid #163a6e;border-top-left-radius: 5px;'><table style='width:100%;border-top-left-radius: 5px;'><tr style='border-top-left-radius: 5px;'><td style='width:40%;vertical-align:top;border-top-left-radius: 5px;'>";
                                NotesHtml += "<table style='width:100%;border-top-left-radius: 5px;'>";
                                if (objListNotesComponents.FirstOrDefault(m => m.Component == "Medications") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medications") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesMedication(objLegacyNotesViewModel.MedicationHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "medications").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medications") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medications").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "medications").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }
                                if (objListNotesComponents.FirstOrDefault(m => m.Component == "SocialHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socialhx") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesSocialHx(objLegacyNotesViewModel.SocialHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "socialhx").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socialhx") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socialhx").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "socialhx").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }
                                if (objListNotesComponents.FirstOrDefault(m => m.Component == "Social,PsychologicalandBehaviorHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socpsyandbehaviorhx") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesSocPsyandBehaviorHx(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "socpsyandbehaviorhx").ToList(), objListNotesComponents.FirstOrDefault().NotesId) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socpsyandbehaviorhx") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "socpsyandbehaviorhx").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "socpsyandbehaviorhx").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }
                                if (objListNotesComponents.FirstOrDefault(m => m.Component == "MedicalHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medicalhx") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesMedicalHx(objLegacyNotesViewModel.MedicalHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "medicalhx").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medicalhx") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "medicalhx").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "medicalhx").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if (objListNotesComponents.FirstOrDefault(m => m.Component == "FamilyHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "familyhx") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesFamilyHx(objLegacyNotesViewModel.FamilyHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "familyhx").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "familyhx") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "familyhx").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "familyhx").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if (objListNotesComponents.FirstOrDefault(m => m.Component == "SurgicalHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "surgicalhx") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesSurgicalHx(objLegacyNotesViewModel.SurgicalHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "surgicalhx").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "surgicalhx") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "surgicalhx").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "surgicalhx").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if (objListNotesComponents.FirstOrDefault(m => m.Component == "BirthHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "birthhx") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesBirthHx(objLegacyNotesViewModel.BirthHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "birthhx").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "birthhx") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "birthhx").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "birthhx").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if (objListNotesComponents.FirstOrDefault(m => m.Component == "HospitalizationHx") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "hospitalizationhx") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesHospitalizationHx(objLegacyNotesViewModel.HospitalizationHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "hospitalizationhx").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "hospitalizationhx") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "hospitalizationhx").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "hospitalizationhx").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if (objListNotesComponents.FirstOrDefault(m => m.Component == "Allergies") != null && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "allergies") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesAlleryHx(objLegacyNotesViewModel.AllergyHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "allergies").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "allergies") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "allergies").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "allergies").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if (objListNotesComponents.FirstOrDefault(m => m.Component.ToLower() == "reviewofsystems") != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "review of systems").Count() > 0)
                                {
                                    NotesHtml += "<tr><td>" + NotesROS(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "review of systems").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "review of systems") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "review of systems").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "review of systems").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if (counterLeftPan == 0)
                                {
                                    NotesHtml += "<tr><td>&nbsp;</td></tr>";
                                }
                                NotesHtml += "</table>";
                                NotesHtml += "</td><td style='width:60%;vertical-align:top;border-left:2px solid #EFEFEF;'>";
                                NotesHtml += "<table style='width:100%;marging-top:5px;'>";

                                if (objLegacyNotesViewModel.NotesComponent != null && objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                {
                                    counterRightPan = 1;

                                    NotesHtml += "<tr><td>" + NotesAppointmentReason(objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "free text").ToList(), NoteReason) + "</td></tr>";
                                    if (objLegacyNotesViewModel.NotesComponent != null && objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "custom").Count() > 0)
                                    {
                                        List<NotesComponent> CustomTextComponents = new List<NotesComponent>();
                                        CustomTextComponents = objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "custom").ToList();
                                        HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();

                                        foreach (NotesComponent obj in CustomTextComponents)
                                        {
                                            try
                                            {
                                                document.LoadHtml(obj.SOAPText);
                                                HtmlAgilityPack.HtmlNode node = document.DocumentNode.SelectSingleNode("//li[contains(@class, 'CustomComponent')]/ul/li/span");

                                                if (node != null)
                                                {
                                                    OrderNoComponent = obj.OrderNo;
                                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                                    {
                                                        foreach (var obj2 in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "custom").OrderBy(m => m.OrderNo))
                                                        {
                                                            if (obj2.OrderNo >= OrderNoComponent && obj2.ComponentName.ToLower() == "free text")
                                                            {
                                                                if (FreeTextComponents.Where(m => m.OrderNo == obj2.OrderNo).Count() > 0)
                                                                {
                                                                    counterRightPan = 1;
                                                                    NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj2.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                                    FreeTextComponents.RemoveAll(m => m.OrderNo == obj2.OrderNo);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    counterLeftPan = 1;
                                                }
                                                OrderNoComponent = -1;
                                            }
                                            catch
                                            {

                                            }
                                        }


                                    }
                                }

                                if ((objLegacyNotesViewModel.Complaints != null && objLegacyNotesViewModel.Complaints.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "complaints") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesComplaints(objLegacyNotesViewModel.Complaints, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "complaints").ToList(), objListNotesComponents.FirstOrDefault().NotesId) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "complaints") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "complaints").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "complaints").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.VitalSigns != null && objLegacyNotesViewModel.VitalSigns.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "vitals") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesVitalSigns(objLegacyNotesViewModel.VitalSigns, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "vitals").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "vitals") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "vitals").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "vitals").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.PhysicalExam != null && objLegacyNotesViewModel.PhysicalExam.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "physical exam") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesPhysicalExam(objLegacyNotesViewModel.PhysicalExam, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "physical exam").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "physical exam") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "physical exam").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "physical exam").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.LabOrderResult != null && objLegacyNotesViewModel.LabOrderResult.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab results") != null)
                                {
                                    NotesHtml += "<tr><td style='padding-left:12px;padding-top:5px;padding-bottom:5px;padding-right:8px;'>" + NotesLabOrderResult(objLegacyNotesViewModel.LabOrderResult, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "lab results").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab results") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab results").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "lab results").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.RadOrderResult != null && objLegacyNotesViewModel.RadOrderResult.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "radiology results") != null)
                                {
                                    NotesHtml += "<tr><td style='padding-left:12px;padding-top:5px;padding-bottom:5px;padding-right:8px;'>" + NotesRadOrderResult(objLegacyNotesViewModel.RadOrderResult, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "radiology results").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "radiology results") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "radiology results").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "radiology results").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.ProblemHx != null) && objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "problems") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesProblemHx(objLegacyNotesViewModel.ProblemHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "problems").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "problems") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "problems").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "problems").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }
                                // Start Added by Zia Mehmood
                                if (objLegacyNotesViewModel.NotesComponent != null && objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "order sets" || m.ComponentName.ToLower() == "order sets").Count() > 0)
                                {
                                    NotesHtml += "<tr><td>" + NotesOrderSets(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "order sets").ToList(), objListNotesComponents.FirstOrDefault().NotesId, objListNotesComponents) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "order sets") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "order sets").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "order sets").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }
                                // End Added by Zia Mehmood
                                if ((objLegacyNotesViewModel.Prescription != null && objLegacyNotesViewModel.Prescription.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "prescription") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesPrescription(objLegacyNotesViewModel.Prescription, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "prescription").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "prescription") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "prescription").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "prescription").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.LabOrder != null && objLegacyNotesViewModel.LabOrder.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab order") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesLabOrder(objLegacyNotesViewModel.LabOrder, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "lab order" || m.ComponentName.ToLower() == "lab orders").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab order") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "lab order").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "lab order").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.RadOrder != null && objLegacyNotesViewModel.RadOrder.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "diagnostic imaging order") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesRadOrder(objLegacyNotesViewModel.RadOrder, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "diagnostic imaging order").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "diagnostic imaging order") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "diagnostic imaging order").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "diagnostic imaging order").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.ProcedureOrder != null && objLegacyNotesViewModel.ProcedureOrder.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedure order") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesProcedureOrder(objLegacyNotesViewModel.ProcedureOrder, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "procedure order").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedure order") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedure order").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "procedure order").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.ConsultationOrder != null && objLegacyNotesViewModel.ConsultationOrder.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "consultation order") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesConsultationOrder(objLegacyNotesViewModel.ConsultationOrder, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "consultation order").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "consultation order") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "consultation order").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "consultation order").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.ImmunizationHx != null && objLegacyNotesViewModel.ImmunizationHx.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "immunization") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesImmunizationHx(objLegacyNotesViewModel.ImmunizationHx, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "immunization").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "immunization") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "immunization").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "immunization").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.Procedure != null && objLegacyNotesViewModel.Procedure.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedures") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesProcedures(objLegacyNotesViewModel.Procedure, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "procedures").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedures") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "procedures").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "procedures").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.Referrals != null && objLegacyNotesViewModel.Referrals.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "referrals") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesReferrals(objLegacyNotesViewModel.Referrals, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "referrals").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "referrals") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "referrals").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "referrals").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.PatientEducation != null && objLegacyNotesViewModel.PatientEducation.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "patient education") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesPatientEducation(objLegacyNotesViewModel.PatientEducation, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "patient education").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "patient education") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "patient education").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "patient education").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if (objListNotesComponents.FirstOrDefault(m => m.Component == "FollowUp") != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "follow up").Count() > 0)
                                {
                                    NotesHtml += "<tr><td>" + NotesFollowUp(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "follow up").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "follow up") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "follow up").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "follow up").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if ((objLegacyNotesViewModel.FunctionalCognitive != null && objLegacyNotesViewModel.FunctionalCognitive.Count() > 0) || objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "functional and cognitive") != null)
                                {
                                    NotesHtml += "<tr><td>" + NotesFunctionalCognitive(objLegacyNotesViewModel.FunctionalCognitive, objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "functional and cognitive").ToList()) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "functional and cognitive") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "functional and cognitive").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "functional and cognitive").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if (objList_NotesComponent != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "custom forms").Count() > 0)
                                {
                                    NotesHtml += "<tr><td>" + NotesCustomForm(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "custom forms").ToList(), objListNotesComponents) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "custom forms") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "custom forms").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "custom forms").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }

                                if (objListNotesComponents.FirstOrDefault(m => m.Component.ToLower() == "treatment") != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "treatment").Count() > 0)
                                {
                                    NotesHtml += "<tr><td>" + NotesTreatment(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "treatment").ToList(), objListNotesComponents.FirstOrDefault().NotesId) + "</td></tr>";
                                    if (FreeTextComponents != null && FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").Count() > 0)
                                    {
                                        if (objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "treatment") != null)
                                        {
                                            OrderNoComponent = objList_NotesComponent.FirstOrDefault(m => m.ComponentName.ToLower() == "treatment").OrderNo;
                                            foreach (var obj in FreeTextComponents.Where(m => m.OrderNo >= OrderNoComponent && m.ComponentName.ToLower() != "treatment").OrderBy(m => m.OrderNo))
                                            {
                                                if (obj.OrderNo >= OrderNoComponent && obj.ComponentName.ToLower() == "free text")
                                                {
                                                    if (FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo).Count() > 0)
                                                    {
                                                        counterRightPan = 1;
                                                        NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.OrderNo == obj.OrderNo && m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                                        FreeTextComponents.RemoveAll(m => m.OrderNo == obj.OrderNo);
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        OrderNoComponent = -1;
                                    }
                                    counterLeftPan = 1;
                                }
                                if (FreeTextComponents != null && FreeTextComponents.Count() > 0)
                                {
                                    counterRightPan = 1;
                                    NotesHtml += "<tr><td>" + NotesAppointmentFreeText(FreeTextComponents.Where(m => m.ComponentName.ToLower() == "free text").ToList()) + "</td></tr>";
                                }
                                if (objList_NotesComponent != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "images").Count() > 0)
                                {
                                    counterRightPan = 1;
                                    NotesHtml += "<tr><td>";
                                    NotesHtml += NotesImages(objListNotesComponents.FirstOrDefault().NotesId, null);
                                    NotesHtml += "</td></tr>";
                                }

                                if (objList_NotesComponent != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "signature").Count() > 0)
                                {
                                    counterRightPan = 1;
                                    NotesHtml += "<tr><td>";
                                    NotesHtml += NotesSignature(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "signature").ToList(), objLegacyNotesViewModel.NoteHeaderData.Where(m => m.Type == "Provider").ToList());
                                    NotesHtml += "</td></tr>";
                                }
                                if (objLegacyNotesViewModel.NotesComponent != null && objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "cosign" || m.ComponentName.ToLower() == "co-sign").Count() > 0)
                                {
                                    counterRightPan = 1;
                                    NotesHtml += "<tr><td>" + NotesCoSign(objLegacyNotesViewModel.NotesComponent.Where(m => m.ComponentName.ToLower() == "cosign" || m.ComponentName.ToLower() == "co-sign").ToList()) + "</td></tr>";
                                }
                                if (objList_NotesComponent != null && objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "amendment").Count() > 0)
                                {
                                    counterRightPan = 1;
                                    NotesHtml += "<tr><td>" + NotesAmendment(objList_NotesComponent.Where(m => m.ComponentName.ToLower() == "amendment").ToList()) + "</td></tr>";
                                }
                                if (counterRightPan == 0)
                                {
                                    NotesHtml += "<tr><td>&nbsp;</td></tr>";
                                }
                                NotesHtml += "</table>";
                                NotesHtml += "</td></tr>";


                                NotesHtml += "</table></td></tr></table></div>";


                                NotesHtml = NotesHtml.Replace("(Info)", ""); //NotesHtml.Split('(Info)').join("");
                                string CSS = CssFiles();
                                NotesHtml = SetSpacing(NotesHtml);
                                NotesHtml = ReplaceSpecialCharacters(NotesHtml);

                                byte[] strBytes = ConvertHtmlToPdf(NotesHtml, CSS, objLegacyNotesViewModel.NoteHeaderData, drReportHeader);
                                string ServerPath = System.Configuration.ConfigurationManager.AppSettings["PatientFilesPath"];
                                string completeFilePath = ServerPath + ExistingFilePath;
                                string TempServerPathForDataFixPDF = @"\\DataFixPdfTemp";
                                string completeFilePathTemp = ServerPath + TempServerPathForDataFixPDF;

                                if (!System.IO.Directory.Exists(completeFilePathTemp))
                                    System.IO.Directory.CreateDirectory(completeFilePathTemp);
                                if (File.Exists(completeFilePath))
                                {
                                    string newFilePath = completeFilePathTemp + @"\\" + notesid + "_" + Path.GetFileName(completeFilePath);
                                    File.Delete(newFilePath);
                                    File.Move(completeFilePath, newFilePath);
                                }
                                if (!System.IO.Directory.Exists(completeFilePath))
                                    System.IO.Directory.CreateDirectory(Path.GetDirectoryName(completeFilePath));

                                File.WriteAllBytes(completeFilePath, strBytes);
                                response = new
                                {
                                    status = true,
                                    Message = Common.AppPrivileges.Save_Message,
                                };
                                #endregion
                            }
                            else
                            {
                                response = new
                                {
                                    status = false,
                                    Message = "Got Error Here: BLLClinicalObj.loadNoteComponentsDataFixPDF",
                                };
                            }
                        }
                        else
                        {
                            response = new
                            {
                                status = false,
                                Message = "Got Error Here: var pdfmdl = mdl.Data.FirstOrDefault(x => x.NotesId == " + notesid + ")",
                            };
                        }

                    }
                    else
                    {
                        response = new
                        {
                            status = false,
                            Message = "Could'nt load data here:  BLLClinicalObj.loadNoteFilePaths4DataFixPDF()",
                        };
                    }
                }
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("CreateNotesPDFFixesFiles -> " + NotesIds, ex);
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string loadNoteComponentsName(Int64 NotesID)
        {
            try
            {
                List<NoteComponentModel> NoteComponentList = null;
                BLObject<List<NoteComponentModel>> obj;

                obj = BLLClinicalObj.loadNoteComponentsName(NotesID);

                if (obj.Data != null)
                {
                    NoteComponentList = obj.Data;
                    if (NoteComponentList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            NoteComponentsCount = NoteComponentList.Count,
                            NoteComponents_JSON = NoteComponentList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MedicationCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
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





        public List<NotesComponentDataFixModel> loadClinicalSignedNotesForPDFFix()
        {
            return BLLClinicalObj.loadClinicalSignedNotesForPDFFix();
        }

        public void updateClinicalSignedNotesForPDFFix(string noteid, string error)
        {
            BLLClinicalObj.updateClinicalSignedNotesForPDFFix(noteid, error);
        }

        public string UpdateIsNonBillableInfo(Int64 NotesId, bool IsNonBilable)
        {
            try
            {
                BLLClinicalObj.UpdateIsNonBillableInfo(NotesId, IsNonBilable);
                var response = new
                {
                    status = true,
                    Message = AppPrivileges.Update_Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string GetIsNonBillableInfo(Int64 NotesId)
        {
            try
            {
                string IsNonBillable = BLLClinicalObj.GetIsNonBillableInfo(NotesId);

                var response = new
                {
                    status = true,
                    IsNonBillable = IsNonBillable,
                    Message = AppPrivileges.Update_Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return JsonConvert.SerializeObject(response);
            }
        }

        public string SaveDiagnosticResultInPatDocs(ClinicalNotesFillModel model)
        {
            try
            {
                DSRadiologyResult dsDiagnosticResult = null;
                BLObject<DSRadiologyResult> objrad;

                if (!string.IsNullOrEmpty(model.Base64String))
                {
                    objrad = BLLClinicalObj.LoadRadiologyResultDetail(0, MDVUtility.ToLong(model.ResultId));
                    dsDiagnosticResult = objrad.Data;
                    if (dsDiagnosticResult != null && dsDiagnosticResult.RadiologyOrderResultDetail.Rows.Count > 0)
                    {
                        string resultName = dsDiagnosticResult.RadiologyOrderResultDetail.Rows[0][dsDiagnosticResult.RadiologyOrderResultDetail.CPTCodeDescriptionColumn].ToString();

                        if (resultName.Length > 140)
                        {
                            resultName = resultName.Substring(0, 140);
                        }

                        model.FileName = DateTime.Today.ToString("MM.dd.yyyy") + "_" + resultName + ".pdf";
                    }


                    string docResponse = new Patient_Document().SaveSignedDocument("", MDVUtility.ToInt64(model.PatientId), model.TransitionId, "Notes Diagnostic Imaging Report", model.Base64String, model.FileType, model.FileName, model.FolderName);
                    PatientDocumentResponse result = JsonConvert.DeserializeObject<PatientDocumentResponse>(docResponse);

                    if (result.status == true)
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                        var response = new
                        {
                            status = true,
                            Message = result.Message,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = result.Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = "No Result Found."
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

    }
}