using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MDVision.Business.BCommon;
using MDVision.Model.Clinical.Templates;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.Common.Logging;
using MDVision.DataAccess.DCommon;
using MDVision.Model.Clinical.HPI;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;

namespace MDVision.Business.BLL
{
    public class BLLHPI
    {
        #region " Constructors "
        public BLLHPI()
        {
            InitializeComponent();
        }

        private IContainer components;
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region " Lookup's "

        public BLObject<List<HPIFindingsModel>> LookupHPIFindings(string IsActive)
        {
            try
            {
                var result = new DALHPI().LookupHPIFindings(IsActive);
                return new BLObject<List<HPIFindingsModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::LookupHPIFindings", ex);
                return new BLObject<List<HPIFindingsModel>>(null, ex.Message);

            }
        }
        public BLObject<List<HPITemplateModel>> LookupHPITemplate()
        {
            try
            {
                var result = new DALHPI().LookupHPITemplate();
                return new BLObject<List<HPITemplateModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::LookupHPITemplate", ex);
                return new BLObject<List<HPITemplateModel>>(null, ex.Message);

            }
        }

        #endregion

        #region " HPI Finding "
        public BLObject<List<HPIFindingsModel>> LoadHPIFindings(long HPIFindingsId, string IsActive, string Name, long PageNumber = 1, long RowspPage = 15)
        {
            try
            {
                var result = new DALHPI().LoadHPIFindings(HPIFindingsId, IsActive, Name, PageNumber, RowspPage);
                return new BLObject<List<HPIFindingsModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::LoadHPIFindings", ex);
                return new BLObject<List<HPIFindingsModel>>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteHPIFindings(string HPIFindingsId)
        {
            try
            {
                HPIFindingsId = new DALHPI().DeleteHPIFindings(Common.Utilities.MDVUtility.ToLong(HPIFindingsId));
                return new BLObject<string>(HPIFindingsId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::DeleteHPIFindings", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> SaveHPIFindings(HPIFindingsModel model)
        {
            try
            {
                var HPIFindingsId = new DALHPI().SaveHPIFindings(model);
                return new BLObject<string>(HPIFindingsId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::SaveHPIFindings", ex);
                throw ex;
            }
        }
        public BLObject<string> UpdateHPIFindings(HPIFindingsModel model)
        {
            try
            {
                var returnVal = new DALHPI().UpdateHPIFindings(model);
                return new BLObject<string>(returnVal);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::UpdateHPIFindings", ex);
                throw ex;
            }
        }

        #endregion

        #region " HPI Symptom "
        public BLObject<List<HPISymptomsModel>> LoadHPISymptoms(long HPISymptomsId, string IsActive, string Name, long PageNumber = 1, long RowspPage = 15)
        {
            try
            {
                var result = new DALHPI().LoadHPISymptoms(HPISymptomsId, IsActive, Name, PageNumber, RowspPage);
                return new BLObject<List<HPISymptomsModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::LoadHPISymptoms", ex);
                return new BLObject<List<HPISymptomsModel>>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteHPISymptoms(string HPISymptomsId)
        {
            try
            {
                HPISymptomsId = new DALHPI().DeleteHPISymptoms(Common.Utilities.MDVUtility.ToLong(HPISymptomsId));
                return new BLObject<string>(HPISymptomsId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::DeleteHPISymptoms", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<List<HPISymptomsModel>> SaveHPISymptoms(HPISymptomsModel model)
        {
            try
            {
                var HPIFindingsId = new DALHPI().SaveHPISymptoms(model);
                return new BLObject<List<HPISymptomsModel>>(HPIFindingsId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::SaveHPISymptoms", ex);
                throw ex;
            }
        }
        public BLObject<string> UpdateHPISymptoms(HPISymptomsModel model)
        {
            try
            {
                var returnVal = new DALHPI().UpdateHPISymptoms(model);
                return new BLObject<string>(returnVal);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::UpdateHPISymptoms", ex);
                throw ex;
            }
        }

        #endregion

        #region " HPI Symptom Finding "
        public BLObject<List<HPIFindingsModel>> LoadHPISymptomFinding(long HPISymptomsId)
        {
            try
            {
                var result = new DALHPI().LoadHPISymptomFinding(HPISymptomsId);
                return new BLObject<List<HPIFindingsModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::LoadHPISymptomFinding", ex);
                return new BLObject<List<HPIFindingsModel>>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteHPISymptomFindings(string HPISymptomFindingsId)
        {
            try
            {
                HPISymptomFindingsId = new DALHPI().DeleteHPISymptomFindings(Common.Utilities.MDVUtility.ToLong(HPISymptomFindingsId));
                return new BLObject<string>(HPISymptomFindingsId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::DeleteHPISymptomFindings", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<List<HPISymptomFindingModel>> SaveHPISymptomFinding(HPISymptomFindingModel model)
        {
            try
            {
                List<HPISymptomFindingModel> result = new DALHPI().SaveHPISymptomFinding(model);
                return new BLObject<List<HPISymptomFindingModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::SaveHPISymptomFinding", ex);
                return new BLObject<List<HPISymptomFindingModel>>(null, ex.Message);
            }
        }

        #endregion

        #region HPI Template       
        public BLObject<List<HPITemplateModel>> loadHPITemplates(long hpiTemplateId, int entityId, int isactive)
        {
            try
            {
                var result = new DALHPI().loadHPITemplates(hpiTemplateId, entityId, isactive);
                return new BLObject<List<HPITemplateModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::loadHPITemplates", ex);
                return new BLObject<List<HPITemplateModel>>(null, ex.Message);
            }
        }

        public BLObject<string> insertupdateHPITemplate(HPITemplateModel model)
        {
            try
            {
                string hpiTemplateI = new DALHPI().insertupdateHPITemplate(model);
                return new BLObject<string>(hpiTemplateI);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::insertupdateHPITemplate", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<List<HPISymptomsLookupModel>> lookupHPISymptoms(int? isactive)
        {
            try
            {
                var result = new DALHPI().lookupHPISymptoms(isactive);
                return new BLObject<List<HPISymptomsLookupModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::lookupHPISymptoms", ex);
                return new BLObject<List<HPISymptomsLookupModel>>(null, ex.Message);
            }
        }

        public BLObject<List<HPITemplateModel>> fillHPITemplate(long hpiTemplateId)
        {
            try
            {
                var result = new DALHPI().fillHPITemplate(hpiTemplateId);
                return new BLObject<List<HPITemplateModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::lookupHPISymptoms", ex);
                return new BLObject<List<HPITemplateModel>>(null, ex.Message);
            }
        }
        public BLObject<List<HPITemplateSymptomFindingsModel>> LoadHPISymptomsFindings(long hpiTemplateId, long symptomsId)
        {
            try
            {
                var result = new DALHPI().LoadHPISymptomsFindings(hpiTemplateId, symptomsId);
                return new BLObject<List<HPITemplateSymptomFindingsModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::LoadHPISymptomsFindings", ex);
                return new BLObject<List<HPITemplateSymptomFindingsModel>>(null, ex.Message);
            }
        }

        public BLObject<List<HPITemplateSymptomFindingsModel>> LoadHPISymptomsFindingsDetail(long hpiSymptomDetailId, long hpiTemplteSymptomsId)
        {
            try
            {
                var result = new DALHPI().LoadHPISymptomsFindingsDetail(hpiSymptomDetailId, hpiTemplteSymptomsId);
                return new BLObject<List<HPITemplateSymptomFindingsModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::LoadHPISymptomsFindingsDetail", ex);
                return new BLObject<List<HPITemplateSymptomFindingsModel>>(null, ex.Message);
            }
        }
        public BLObject<List<HPITemplateModel>> loadHPIForProvider(long providerId)
        {
            try
            {
                var result = new DALHPI().loadHPIForProvider(providerId);
                return new BLObject<List<HPITemplateModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::loadHPIForProvider", ex);
                return new BLObject<List<HPITemplateModel>>(null, ex.Message);
            }
        }
        public BLObject<List<HPINotesFindings>> loadHPITempSymptomFindingsForNotes(long notesId)
        {
            try
            {
                var result = new DALHPI().loadHPITempSymptomFindingsForNotes(notesId);
                return new BLObject<List<HPINotesFindings>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::loadHPITempSymptomFindingsForNotes", ex);
                return new BLObject<List<HPINotesFindings>>(null, ex.Message);
            }
        }
        public BLObject<string> deleteHPITemplateSymptom(long HPITemplateSymptomId)
        {
            try
            {
                var result = new DALHPI().deleteHPITemplateSymptom(HPITemplateSymptomId);
                return new BLObject<string>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::deleteHPITemplateSymptom", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> deleteHPISymptomFinding(long symptomFindingId)
        {
            try
            {
                var result = new DALHPI().deleteHPISymptomFinding(symptomFindingId);
                return new BLObject<string>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::deleteHPISymptomFinding", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<List<HPITemplateModel>> loadHPITempSymptomFindingNote(long TemplateId, long SystemId, long? NotesId = null)
        {
            try
            {
                var result = new DALHPI().loadHPITempSymptomFindingNote(TemplateId, SystemId, NotesId);
                return new BLObject<List<HPITemplateModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::loadHPITempSymptomFindingNote", ex);
                return new BLObject<List<HPITemplateModel>>(null, ex.Message);
            }
        }
        public BLObject<List<HPITemplateSymptomsModel>> loadHPITemplateSymptoms(long templateId, long entityId, int? IsActive = 1, int? isSelected = 1)
        {
            try
            {
                var result = new DALHPI().loadHPITemplateSymptoms(templateId, entityId, IsActive, isSelected);
                return new BLObject<List<HPITemplateSymptomsModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::loadHPITempSymptomFindingNote", ex);
                return new BLObject<List<HPITemplateSymptomsModel>>(null, ex.Message);
            }
        }
        public BLObject<List<HPINotesFindings>> saveHPIComplaintNotesFinding(string xmlNotesFindings)
        {
            try
            {
                var result = new DALHPI().saveHPIComplaintNotesFinding(xmlNotesFindings, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                return new BLObject<List<HPINotesFindings>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::saveHPIComplaintNotesFinding", ex);
                return new BLObject<List<HPINotesFindings>>(null, ex.Message);
            }
        }

        public BLObject<string> isHPIComplaint(long notesId)
        {
            try
            {
                var result = new DALHPI().isHPIComplaint(notesId);
                return new BLObject<string>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::isHPIComplaint", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> deleteHPITemplate(long hpiTemplateId)
        {
            try
            {
                string returnValue = new DALHPI().deleteHPITemplate(hpiTemplateId);
                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::deleteHPITemplate", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> HPITemplateIsActive(long templateID, long IsActive)
        {
            try
            {
                string returnValue = new DALHPI().HPITemplateIsActive(templateID, IsActive);
                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::HPITemplateIsActive", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> associateHPISymptomAndTemplate(HPITemplateSymptomsModel model)
        {
            try
            {
                var returnValue = new DALHPI().associateHPISymptomAndTemplate(model);
                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::associateHPISymptomAndTemplate", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> UpdateHPINotesSymptomFindingDesc(long HPINotesFindingId, string Desc)
        {
            try
            {
                string returnValue = new DALHPI().UpdateHPINotesSymptomFindingDesc(HPINotesFindingId, Desc);
                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLHPI::UpdateHPINotesSymptomFindingDesc", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion
    }
}
