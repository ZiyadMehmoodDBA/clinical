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
    public class BLLAOETemplates
    {
        #region " Constructors "
        public BLLAOETemplates()
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
        public BLObject<DSAOETemplate> SaveAOETemplateSystemObservatios(DSAOETemplate dsPhysicalExam)
        {
            try
            {
                DSAOETemplate dsPhysicalExam_ = null;
                dsPhysicalExam_ = new DALAOETemplate().SaveAOESystemObservations(dsPhysicalExam);
                return new BLObject<DSAOETemplate>(dsPhysicalExam_);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::SavePhysicalExamSystemObservatiosECW", ex);
                return new BLObject<DSAOETemplate>(null, ex.Message);
            }
        }
        public List<AOETemplatesModel> LoadAOETemplates(AOETemplatesModel model)
        {
            try
            {
                List<AOETemplatesModel> list = new DALAOETemplate().LoadAOETemplates(model);
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::LoadAOETemplates", ex);
                return null;
            }
        }
        public BLObject<DSAOETemplate> insertAOESystemObservation(DSAOETemplate ds)
        {
            try
            {
                ds = new DALAOETemplate().insertAOESystemObservation(ds);
                return new BLObject<DSAOETemplate>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::insertAOESystemObservation", ex);
                return new BLObject<DSAOETemplate>(null, ex.Message);
            }
        }
         public string ActiveInActiveAOETemplate(AOETemplatesModel model)
        {
            try
            {
                string list = new DALAOETemplate().ActiveInActiveAOETemplate(model);
                return list;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::LoadAOETemplates", ex);
                return null;
            }
        }
         public BLObject<string> deleteAOETemplate(long templateId)
         {
             try
             {
                 string returnValue = new DALAOETemplate().deleteAOETemplate(templateId);

                 return new BLObject<string>(returnValue);
             }
             catch (Exception ex)
             {
                 MDVLogger.BLLErrorLog("BLLAOETemplate::deleteAOETemplate", ex);
                 return new BLObject<string>(null, ex.Message);
             }
         }
         public BLObject<DSAOETemplate> loadAOETempSysObservations(long templateId)
         {
             try
             {
                 DSAOETemplate dsEntityTemplates = null;
                 dsEntityTemplates = new DALAOETemplate().loadAOETempSysObservations(templateId);

                 return new BLObject<DSAOETemplate>(dsEntityTemplates);
             }
             catch (Exception ex)
             {
                 MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadAOETemplates", ex);
                 return new BLObject<DSAOETemplate>(null, ex.Message);
             }
         }
         public BLObject<DSAOETemplate> loadAOESystemObservationForNotes(long NotesId, long AOETemplateSystemId)
         {
             try
             {
                 DSAOETemplate dsEntityTemplates = null;
                 dsEntityTemplates = new DALAOETemplate().loadAOESystemObservationForNotes(NotesId, AOETemplateSystemId);
                 return new BLObject<DSAOETemplate>(dsEntityTemplates);
             }
             catch (Exception ex)
             {
                 MDVLogger.BLLErrorLog("BLLClinical::loadAOESystemObservationForNotes", ex);
                 return new BLObject<DSAOETemplate>(null, ex.Message);
             }
         }
         public BLObject<DSAOETemplate> loadAOESystemRadiologyObservationForNotes(long NotesId, long AOETemplateSystemId)
         {
             try
             {
                 DSAOETemplate dsEntityTemplates = null;
                 dsEntityTemplates = new DALAOETemplate().loadAOESystemRadiologyObservationForNotes(NotesId, AOETemplateSystemId);
                 return new BLObject<DSAOETemplate>(dsEntityTemplates);
             }
             catch (Exception ex)
             {
                 MDVLogger.BLLErrorLog("BLLClinical::loadAOESystemRadiologyObservationForNotes", ex);
                 return new BLObject<DSAOETemplate>(null, ex.Message);
             }
         }
         public BLObject<string> updateAOENotesDescription(long AOENotesObservationId, string Desr)
         {
             try
             {
                 string dsEntityTemplates = new DALAOETemplate().updateAOENotesDescription(AOENotesObservationId, Desr);
                 return new BLObject<string>(dsEntityTemplates);
             }
             catch (Exception ex)
             {
                 MDVLogger.BLLErrorLog("BLLClinical::updateAOENotesDescription", ex);
                 return new BLObject<string>(null, ex.Message);
             }
         }
         public BLObject<string> updateAOENotesRadiologyDescription(long AOENotesRadiologyObservationId, string Desr)
         {
             try
             {
                 string dsEntityTemplates = new DALAOETemplate().updateAOENotesRadiologyDescription(AOENotesRadiologyObservationId, Desr);
                 return new BLObject<string>(dsEntityTemplates);
             }
             catch (Exception ex)
             {
                 MDVLogger.BLLErrorLog("BLLClinical::updateAOENotesDescription", ex);
                 return new BLObject<string>(null, ex.Message);
             }
         }
    }
}
