using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MDVision.Business.BLL
{
    /// <summary>
    /// Author: Arsalan Javed
    /// Created Date: 14-02-2017
    /// Overview: Business Logic Layer for new Physical Exam copy as ECW
    /// </summary>
    public class BLLPhysicalExamECW
    {
        #region " Constructors "
        public BLLPhysicalExamECW()
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

        #region " PhysicalExamECW System "

        public BLObject<DSPhysicalExamECW> insertPhysicalExamSystem(DSPhysicalExamECW ds)
        {
            try
            {
                ds = new DALPhysicalExamECW().insertPhysicalExamSystem(ds);
                return new BLObject<DSPhysicalExamECW>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::insertPhysicalExamSystem", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> updatePhysicalExamSystem(DSPhysicalExamECW ds)
        {
            try
            {
                ds = new DALPhysicalExamECW().updatePhysicalExamSystem(ds);
                return new BLObject<DSPhysicalExamECW>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::updatePhysicalExamSystem", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }

        public BLObject<string> deletePhysicalExamSystem(string SystemId)
        {
            try
            {
                SystemId = new DALPhysicalExamECW().deletePhysicalExamSystem(MDVUtility.ToLong(SystemId));
                return new BLObject<string>(SystemId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::deletePhysicalExamSystem", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> loadPhysicalExamSystem(long SystemId, string IsActive, string Name, long PageNumber, long RowspPage)
        {
            try
            {
                DSPhysicalExamECW ds = new DSPhysicalExamECW();
                ds = new DALPhysicalExamECW().loadPhysicalExamSystem(SystemId, IsActive, Name, PageNumber, RowspPage);
                return new BLObject<DSPhysicalExamECW>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamSystem", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }

        #endregion

        #region " PhysicalExamECW Observations "

        public BLObject<DSPhysicalExamECW> insertPhysicalExamObservation(DSPhysicalExamECW ds)
        {
            try
            {
                ds = new DALPhysicalExamECW().insertPhysicalExamObservation(ds);
                return new BLObject<DSPhysicalExamECW>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::insertPhysicalExamObservation", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> updatePhysicalExamObservation(DSPhysicalExamECW ds)
        {
            try
            {
                ds = new DALPhysicalExamECW().updatePhysicalExamObservation(ds);
                return new BLObject<DSPhysicalExamECW>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::updatePhysicalExamObservation", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<string> deletePhysicalExamObservation(string ObservationId)
        {
            try
            {
                ObservationId = new DALPhysicalExamECW().deletePhysicalExamObservation(MDVUtility.ToLong(ObservationId));
                return new BLObject<string>(ObservationId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::deletePhysicalExamObservation", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> loadPhysicalExamObservation(long ObservationId, string IsActive, string Name, long PageNumber, long RowspPage)
        {
            try
            {
                DSPhysicalExamECW ds = new DSPhysicalExamECW();
                ds = new DALPhysicalExamECW().loadPhysicalExamObservation(ObservationId, IsActive, Name, PageNumber, RowspPage);
                return new BLObject<DSPhysicalExamECW>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamObservation", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }

        #endregion

        #region " PhycsicalExamECW System Observation "

        public BLObject<DSPhysicalExamECW> loadPhysicalExamSystemObservation(long SystemId)
        {
            try
            {
                DSPhysicalExamECW ds = new DSPhysicalExamECW();
                ds = new DALPhysicalExamECW().loadPhysicalExamSystemObservation(SystemId);
                return new BLObject<DSPhysicalExamECW>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamSystemObservation", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }

        public BLObject<DSPhysicalExamECW> insertPhysicalExamSystemObservation(DSPhysicalExamECW ds)
        {
            try
            {
                ds = new DALPhysicalExamECW().insertPhysicalExamSystemObservation(ds);
                return new BLObject<DSPhysicalExamECW>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::insertPhysicalExamSystemObservation", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }

        public BLObject<string> deletePhysicalExamSystemObservation(string SystemObservationId)
        {
            try
            {
                SystemObservationId = new DALPhysicalExamECW().deletePhysicalExamSystemObservation(MDVUtility.ToLong(SystemObservationId));
                return new BLObject<string>(SystemObservationId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::deletePhysicalExamSystemObservation", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion


        #region " Lookup's "

        public BLObject<DSPhysicalExamECWLookup> lookupPESystem(string IsActive)
        {
            try
            {
                DSPhysicalExamECWLookup ds = new DSPhysicalExamECWLookup();
                ds = new DALPhysicalExamECW().lookupPESystem(IsActive);
                return new BLObject<DSPhysicalExamECWLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::lookupPESystem", ex);
                return new BLObject<DSPhysicalExamECWLookup>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECWLookup> lookupPEObservation(string IsActive)
        {
            try
            {
                DSPhysicalExamECWLookup ds = new DSPhysicalExamECWLookup();
                ds = new DALPhysicalExamECW().lookupPEObservation(IsActive);
                return new BLObject<DSPhysicalExamECWLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::lookupPEObservation", ex);
                return new BLObject<DSPhysicalExamECWLookup>(null, ex.Message);
            }
        }


        #endregion

        #region "PhysicalExamECW, Template Systems"

        public BLObject<DSPhysicalExamECW> loadPhysicalExamTemplatesECW(long templateId, long entityId, int? IsActive = null, int? isSelected = 1)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().loadPhysicalExamTemplatesECW(templateId, entityId, IsActive, isSelected);
                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadPhysicalExamTemplatesECW", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }

        public BLObject<DSPhysicalExamECWLookup> loadPhysicalExamSystemsECW(int? IsActive = 1)
        {
            try
            {
                DSPhysicalExamECWLookup dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().loadPhysicalExamSystemsECW(IsActive);
                return new BLObject<DSPhysicalExamECWLookup>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamTemplatesECW", ex);
                return new BLObject<DSPhysicalExamECWLookup>(null, ex.Message);
            }
        }

        public BLObject<DSPhysicalExamECW> loadPhysicalExamSystemObservatiosECW(long templateId, long systemId)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().loadPhysicalExamSystemObservation(systemId);
                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadPhysicalExamSystemObservatiosECW", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> SavePhysicalExamSystemObservatiosECW(DSPhysicalExamECW dsPhysicalExam)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam_ = null;
                dsPhysicalExam_ = new DALPhysicalExamECW().SavePhysicalExamSystemObservatiosECW(dsPhysicalExam);
                return new BLObject<DSPhysicalExamECW>(dsPhysicalExam_);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::SavePhysicalExamSystemObservatiosECW", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> loadPhysicalExamTemplatesECW(long templateId, long entityId, int? IsActive = null)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam_ = null;
                dsPhysicalExam_ = new DALPhysicalExamECW().loadPhysicalExamTemplatesECW(templateId, entityId, IsActive);
                return new BLObject<DSPhysicalExamECW>(dsPhysicalExam_);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::SavePhysicalExamSystemObservatiosECW", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<string> PETemplateIsActive(long templateID, long IsActive)
        {
            try
            {
                string PhysicalExam = new DALPhysicalExamECW().PETemplateIsActive(templateID, IsActive);
                return new BLObject<string>(PhysicalExam);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::PETemplateIsActive", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> deletePETemplate(long templateId)
        {
            try
            {
                string returnValue = new DALPhysicalExamECW().deletePETemplate(templateId);

                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::deletePETemplate", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        public BLObject<DSPhysicalExamECW> loadPhysicalExamTempSysObservations(long templateId)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().loadPhysicalExamTempSysObservations(templateId);

                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamTemplatesFillECW", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> loadPhysicalExamECW(long TemplateId, int? IsActive = 1)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().loadPhysicalExamECW(TemplateId, IsActive);
                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadAllPhysicalExamECW", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<string> deletePhysicalExamTemplateSystem(string PETemplateSystemId)
        {
            try
            {
                PETemplateSystemId = new DALPhysicalExamECW().deletePhysicalExamTemplateSystem(MDVUtility.ToLong(PETemplateSystemId));
                return new BLObject<string>(PETemplateSystemId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::deletePhysicalExamTemplateSystem", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> SavePhysicalExamNotesObservation(string xmlNotesObservation)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().SavePhysicalExamNotesObservation(xmlNotesObservation, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName));
                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::SavePhysicalExamNotesObservationECW", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> loadPETempSystemObservatiosECW(long templateId, long systemId)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().loadPETempSystemObservation(templateId, systemId);
                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadPETempSystemObservatiosECW", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }

        public BLObject<DSPhysicalExamECW> loadPETempSystemObservationNote(long templateId, long systemId, long? notesId = null)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().loadPETempSystemObservationNote(templateId, systemId, notesId);
                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadPETempSystemObservationNote", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }

        public BLObject<string> detachPhysicalExamTemplateFromNotes(string NotesId,string PETemplateId="")
        {
            try
            {
                NotesId = new DALPhysicalExamECW().detachPhysicalExamTemplateFromNotes(MDVUtility.ToLong(NotesId), MDVUtility.ToLong(PETemplateId));
                return new BLObject<string>(NotesId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::deletePhysicalExamTemplateSystem", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> loadPETempSystemObservationForNotes(long NotesId)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().loadPETempSystemObservationForNotes(NotesId);
                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPETempSystemObservationForNotes", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }

        //public BLObject<DSPhysicalExamECW> GetSpecialtyProvider(long SpecialtyId)
        //{
        //    try
        //    {
        //        DSPhysicalExamECW ds = new DSPhysicalExamECW();
        //        ds = new DALPhysicalExamECW().GetSpecialtyProvider(SpecialtyId);

        //        return new BLObject<DSPhysicalExamECW>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLPhysicalExamECW::GetSpecialtyProvider", ex);
        //        return new BLObject<DSPhysicalExamECW>(null, ex.Message);
        //    }
        //}
        public BLObject<DSPhysicalExamECW> loadPESystemObservationForNotes(long NotesId, long PETemplateSystemId)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().loadPESystemObservationForNotes(NotesId, PETemplateSystemId);
                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadPESystemObservationForNotes", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<string> updatePENotesDescription(long PENotesObservationId, string Desr)
        {
            try
            {
                string dsEntityTemplates = new DALPhysicalExamECW().updatePENotesDescription(PENotesObservationId, Desr);
                return new BLObject<string>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::updatePENotesDescription", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> insertPETemplateSystem(DSPhysicalExamECW ds)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().insertPETemplateSystem(ds);
                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::insertPETemplateSystem", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> loadPhysicalExamForProvider(long providerId = 0)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().loadPhysicalExamForProvider(providerId);
                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamForProvider", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> SavePhysicalExamForProvider(DSPhysicalExamECW dsPhysicalExam)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam_ = null;
                dsPhysicalExam_ = new DALPhysicalExamECW().SavePhysicalExamForProvider(dsPhysicalExam);
                return new BLObject<DSPhysicalExamECW>(dsPhysicalExam_);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::SavePhysicalExamForProvider", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECWLookup> GetPETemplate()
        {
            try
            {
                DSPhysicalExamECWLookup ds = new DSPhysicalExamECWLookup();
                ds = new DALPhysicalExamECW().GetPETemplate();
                return new BLObject<DSPhysicalExamECWLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::GetPEDataTemplate", ex);
                return new BLObject<DSPhysicalExamECWLookup>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> LoadPhyscialExamForSOAPNote(long templateId)
        {
            try
            {
                DSPhysicalExamECW dsEntityTemplates = null;
                dsEntityTemplates = new DALPhysicalExamECW().LoadPhyscialExamForSOAPNote(templateId);

                return new BLObject<DSPhysicalExamECW>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamTemplatesFillECW", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
    }
}
