using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MDVision.Model.Clinical.AOETemplates;
namespace MDVision.Business.BLL
{
    public class BLLProcedureTemplates
    {
        #region " Constructors "
        public BLLProcedureTemplates()
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

        public BLObject<DSProcedureTemplate> LookupTempAssociation()
        {
            try
            {
                DSProcedureTemplate ds = new DSProcedureTemplate();
                ds = new DALProcedureTemplate().TempAssociationLookup();
                return new BLObject<DSProcedureTemplate>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::GetLabCodeSystem", ex);
                return new BLObject<DSProcedureTemplate>(null, ex.Message);
            }
        }

        public BLObject<DSProcedureTemplate> LookupTempNoteView()
        {
            try
            {
                DSProcedureTemplate ds = new DSProcedureTemplate();
                ds = new DALProcedureTemplate().TempNoteViewLookup();
                return new BLObject<DSProcedureTemplate>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::GetLabCodeSystem", ex);
                return new BLObject<DSProcedureTemplate>(null, ex.Message);
            }
        }


        public BLObject<DSProcedureTemplate> SaveProcedureTemplateSystemObservatios(DSProcedureTemplate dsPhysicalExam)
        {
            try
            {
                DSProcedureTemplate dsPhysicalExam_ = null;
                dsPhysicalExam_ = new DALProcedureTemplate().SaveProcedureSystemObservations(dsPhysicalExam);
                return new BLObject<DSProcedureTemplate>(dsPhysicalExam_);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::SavePhysicalExamSystemObservatiosECW", ex);
                return new BLObject<DSProcedureTemplate>(null, ex.Message);
            }
        }
        public List<ProcedureTemplatesModel> LoadProcedureTemplates(ProcedureTemplatesModel model)
        {
            try
            {
                List<ProcedureTemplatesModel> list = new DALProcedureTemplate().LoadProcedureTemplates(model);
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::LoadProcedureTemplates", ex);
                return null;
            }
        }
        public BLObject<DSProcedureTemplate> insertProcedureSystemObservation(DSProcedureTemplate ds)
        {
            try
            {
                ds = new DALProcedureTemplate().insertProcedureSystemObservation(ds);
                return new BLObject<DSProcedureTemplate>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::insertProcedureSystemObservation", ex);
                return new BLObject<DSProcedureTemplate>(null, ex.Message);
            }
        }
        public string ActiveInActiveProcedureTemplate(ProcedureTemplatesModel model)
        {
            try
            {
                string list = new DALProcedureTemplate().ActiveInActiveProcedureTemplate(model);
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::LoadProcedureTemplates", ex);
                return null;
            }
        }
        public ProcedureTemplatesModel LoadProcedureTemplateTests(ProcedureTemplatesModel model)
        {
            try
            {
                ProcedureTemplatesModel list = new DALProcedureTemplate().LoadProcedureTemplateTests(model);
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::LoadProcedureTemplateTests", ex);
                return null;
            }
        }
        public BLObject<string> deleteProcedureTemplate(long templateId)
        {
            try
            {
                string returnValue = new DALProcedureTemplate().deleteProcedureTemplate(templateId);

                return new BLObject<string>(returnValue);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLProcedureTemplate::deleteProcedureTemplate", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSProcedureTemplate> loadProcedureTempSysObservations(long templateId)
        {
            try
            {
                DSProcedureTemplate dsEntityTemplates = null;
                dsEntityTemplates = new DALProcedureTemplate().loadProcedureTempSysObservations(templateId);

                return new BLObject<DSProcedureTemplate>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadProcedureTemplates", ex);
                return new BLObject<DSProcedureTemplate>(null, ex.Message);
            }
        }


        public string GetProcedureTemplateSoapText(int ProcedureId, long NotesId)
        {
            try
            {
                return new DALProcedureTemplate().GetProcedureTemplateSoapText(ProcedureId, NotesId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLProceduretemp::GetProcedureTemplateSoapText", ex);
                return null;
            }
        }
        public BLObject<DSProcedureTemplate> loadProcedureSystemObservationForNotes(long NotesId, long ProcedureTemplateSystemId, int ProcedureId)
        {
            try
            {
                DSProcedureTemplate dsEntityTemplates = null;
                dsEntityTemplates = new DALProcedureTemplate().loadProcedureSystemObservationForNotes(NotesId, ProcedureTemplateSystemId, ProcedureId);
                return new BLObject<DSProcedureTemplate>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadProcedureSystemObservationForNotes", ex);
                return new BLObject<DSProcedureTemplate>(null, ex.Message);
            }
        }
        public BLObject<DSProcedureTemplate> loadProcedureOrderSystemObservationForNotes(long NotesId, long ProcedureTemplateSystemId, long ProcedureOrderId)
        {
            try
            {
                DSProcedureTemplate dsEntityTemplates = null;
                dsEntityTemplates = new DALProcedureTemplate().loadProcedureOrderSystemObservationForNotes(NotesId, ProcedureTemplateSystemId, ProcedureOrderId);
                return new BLObject<DSProcedureTemplate>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadProcedureSystemObservationForNotes", ex);
                return new BLObject<DSProcedureTemplate>(null, ex.Message);
            }
        }
        public BLObject<DSProcedureTemplate> loadProcedureSystemRadiologyObservationForNotes(long NotesId, long ProcedureTemplateSystemId)
        {
            try
            {
                DSProcedureTemplate dsEntityTemplates = null;
              //  dsEntityTemplates = new DALProcedureTemplate().loadProcedureSystemRadiologyObservationForNotes(NotesId, ProcedureTemplateSystemId);
                return new BLObject<DSProcedureTemplate>(null);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::loadProcedureSystemRadiologyObservationForNotes", ex);
                return new BLObject<DSProcedureTemplate>(null, ex.Message);
            }
        }
        public BLObject<string> updateProcedureNotesDescription(long ProcedureNotesObservationId, string Desr)
        {
            try
            {
                string dsEntityTemplates = new DALProcedureTemplate().updateProcedureNotesDescription(ProcedureNotesObservationId, Desr);
                return new BLObject<string>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::updateProcedureNotesDescription", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> updateProcedureOrderNotesDescription(long ProcedureNotesObservationId, string Desr)
        {
            try
            {
                string dsEntityTemplates = new DALProcedureTemplate().updateProcedureOrderNotesDescription(ProcedureNotesObservationId, Desr);
                return new BLObject<string>(dsEntityTemplates);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::updateProcedureNotesDescription", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

    }
}
