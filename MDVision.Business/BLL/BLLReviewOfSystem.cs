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
using MDVision.IEHR.EMR.Model;
using MDVision.Model.Clinical.ReviewOfSystem;
using System.Data;

namespace MDVision.Business.BLL
{
    /// <summary>
    /// Author: Zia Mehmood
    /// Created Date: 31-05-2017
    /// Overview: Business Logic Layer for new Review of system
    /// </summary>
    public class BLLReviewOfSystem
    {
        #region " Constructors "
        public BLLReviewOfSystem()
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
        #region " Review of System Charatristics"

        public List<ROSCharacteristics> lookupROSSystems(string IsActive)
        {

            List<ROSCharacteristics> objList_ROSCharatristics = new List<ROSCharacteristics>();
            try
            {

                objList_ROSCharatristics = new DALReviewofSystems().lookupROSSystems(IsActive);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamObservation", ex);
                throw ex.InnerException;

            }
            return objList_ROSCharatristics;
        }

        public BLObject<ROSCharacteristics> InsertROSCharatriscticandUpadatesystem(ROSCharacteristics model)
        {
            try
            {
                ROSCharacteristics RosCharatristic = new DALReviewofSystems().InsertROSCharatriscticandUpadatesystem(model);
                return new BLObject<ROSCharacteristics>(RosCharatristic);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::InsertROSCharatrisctic", ex);
                return new BLObject<ROSCharacteristics>(null, ex.Message);
            }
        }

        public BLObject<string> InsertROSCharatrisctic(ROSCharacteristics model)
        {
            try
            {
                string RosCharatristicId = new DALReviewofSystems().InsertROSCharatrisctic(model);
                return new BLObject<string>(RosCharatristicId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::InsertROSCharatrisctic", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public List<ROSCharacteristics> loadROSCharatrisctic(ROSCharacteristics model)
        {
            List<ROSCharacteristics> objList_ROSCharatristics = new List<ROSCharacteristics>();
            try
            {

                objList_ROSCharatristics = new DALReviewofSystems().loadROSCharatrisctic(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamObservation", ex);
                throw ex.InnerException;

            }
            return objList_ROSCharatristics;
        }

        public BLObject<string> deleteROSCharatristics(string ROSCharacteristicsId)
        {
            try
            {
                ROSCharacteristicsId = new DALReviewofSystems().deleteROSCharatristics(MDVUtility.ToLong(ROSCharacteristicsId));
                return new BLObject<string>(ROSCharacteristicsId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::deletePhysicalExamObservation", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        public BLObject<string> updateROSCharatristics(ROSCharacteristics model)
        {
            try
            {
                string val = new DALReviewofSystems().updateROSCharatristics(model);
                return new BLObject<string>(val);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReviewofSystem::updateROSCharatristics", ex);
                throw ex.InnerException;
            }
        }
        public List<ROSCharacteristics> lookupPEObservation(string IsActive)
        {

            List<ROSCharacteristics> objList_ROSCharatristics = new List<ROSCharacteristics>();
            try
            {

                objList_ROSCharatristics = new DALReviewofSystems().lookupROSCharatristics(IsActive);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamObservation", ex);
                throw ex.InnerException;

            }
            return objList_ROSCharatristics;
        }





        #endregion

        #region " Review of System System "

        public List<ROSCharacteristics> loadROSSystemCharatristics(string ROSSystemId)
        {
            List<ROSCharacteristics> objList_ROSCharatristics = new List<ROSCharacteristics>();
            try
            {

                objList_ROSCharatristics = new DALReviewofSystems().loadROSSystemCharatristics(ROSSystemId);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadROSSystemCharatristics", ex);
                throw ex.InnerException;

            }
            return objList_ROSCharatristics;
        }
        public List<ROSCharacteristics> insert_reviewofsystem_system(ROSCharacteristics model, bool isInsert = true)
        {
            List<ROSCharacteristics> objList_ROSSystems = new List<ROSCharacteristics>();
            try
            {

                objList_ROSSystems = new DALReviewofSystems().insert_reviewofsystem_system(model, isInsert);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadROSSystem", ex);
                throw ex.InnerException;

            }
            return objList_ROSSystems;
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

        public BLObject<string> delete_reviewofsystem_system(string ROSSystemId)
        {
            try
            {
                ROSSystemId = new DALReviewofSystems().delete_reviewofsystem_system(MDVUtility.ToLong(ROSSystemId));
                return new BLObject<string>(ROSSystemId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::deletePhysicalExamObservation", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> ActiveInActive_ROSSystem(string ROSSystemId, string IsActive)
        {
            try
            {
                ROSSystemId = new DALReviewofSystems().ActiveInActive_ROSSystem(MDVUtility.ToLong(ROSSystemId), IsActive);
                return new BLObject<string>(ROSSystemId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::deletePhysicalExamObservation", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }


        public List<ROSSystems> loadROSSystem(ROSCharacteristics model)
        {
            List<ROSSystems> objList_ROSCharatristics = new List<ROSSystems>();
            try
            {

                objList_ROSCharatristics = new DALReviewofSystems().loadROSSystem(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadROSSystem", ex);
                throw ex.InnerException;

            }
            return objList_ROSCharatristics;
        }
        //try
        //{
        //    DSPhysicalExamECW ds = new DSPhysicalExamECW();
        //    ds = new DALReviewofSystems().loadROSSystem(model);
        //    return new BLObject<DSPhysicalExamECW>(ds);
        //}
        //catch (Exception ex)
        //{
        //    MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamSystem", ex);
        //    return new BLObject<DSPhysicalExamECW>(null, ex.Message);
        //}



        #endregion

        // ROS Revamp Methods
        #region ROS ---- Template
        public List<ROSLookUps> loadROSLookUps()
        {
            List<ROSLookUps> objList_ROSLookUps = new List<ROSLookUps>();
            try
            {

                objList_ROSLookUps = new DALReviewofSystems().loadROSLookUps();
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::loadPhysicalExamObservation", ex);
                throw ex.InnerException;

            }
            return objList_ROSLookUps;
        }
        public BLObject<string> Insert_reviewofsystem_Template(ROSTemplateModel model)
        {
            try
            {
                string RosCharatristicId = new DALReviewofSystems().Insert_reviewofsystem_Template(model);
                return new BLObject<string>(RosCharatristicId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::Insert_reviewofsystem_Template", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> update_reviewofsystem_Template(ROSTemplateModel model)
        {
            try
            {
                string RosCharatristicId = new DALReviewofSystems().update_reviewofsystem_Template(model);
                return new BLObject<string>(RosCharatristicId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::update_reviewofsystem_Template", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> delete_reviewofsystem_template(string ROSTemplateId)
        {
            try
            {
                ROSTemplateId = new DALReviewofSystems().delete_reviewofsystem_template(MDVUtility.ToLong(ROSTemplateId));
                return new BLObject<string>(ROSTemplateId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::delete_reviewofsystem_template", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DataSet> select_reviewofsystem_template(string ROSTemplateId, string NotesId = null)
        {
            try
            {
                DataSet ds = new DALReviewofSystems().select_reviewofsystem_template(MDVUtility.ToLong(ROSTemplateId), MDVUtility.ToLong(NotesId));
                return new BLObject<DataSet>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::select_reviewofsystem_template", ex);
                return new BLObject<DataSet>(null, ex.Message);
            }
        }
        public BLObject<string> activeinactive_reviewofsystem_template(string ROSTemplateId, string IsActive, string ModifiedBy)
        {
            try
            {
                ROSTemplateId = new DALReviewofSystems().activeinactive_reviewofsystem_template(MDVUtility.ToLong(ROSTemplateId), IsActive, ModifiedBy);
                return new BLObject<string>(ROSTemplateId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::activeinactive_reviewofsystem_template", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        public BLObject<DSPhysicalExamECW> loadROSRevampTemplates(long templateId, long entityId, int? IsActive = null)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam_ = null;
                dsPhysicalExam_ = new DALReviewofSystems().loadROSRevampTemplates(templateId, entityId, IsActive);
                return new BLObject<DSPhysicalExamECW>(dsPhysicalExam_);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::SavePhysicalExamSystemObservatiosECW", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        #region ROSTemplateRevmap Note
        public BLObject<string> Insert_reviewofsystem_Template_Note(ROSTemplateModel model)
        {
            try
            {
                string RosCharatristicId = new DALReviewofSystems().Insert_reviewofsystem_Template_Note(model);
                return new BLObject<string>(RosCharatristicId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReviewOfSystem::Insert_reviewofsystem_Template_Note", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<DSPhysicalExamECW> load_ROSRevampTemplates_Note(ROSTemplateModel Model)
        {
            try
            {
                DSPhysicalExamECW dsPhysicalExam_ = null;
                dsPhysicalExam_ = new DALReviewofSystems().load_ROSRevampTemplates_Note(Model);
                return new BLObject<DSPhysicalExamECW>(dsPhysicalExam_);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReviewOfSystem::SavePhysicalExamSystemObservatiosECW", ex);
                return new BLObject<DSPhysicalExamECW>(null, ex.Message);
            }
        }
        public BLObject<DataSet> select_reviewofsystem_template_note(string ROSTemplateId, string NotesId = null, string SystemId = null)
        {
            try
            {
                DataSet ds = new DALReviewofSystems().select_reviewofsystem_template_note(MDVUtility.ToLong(ROSTemplateId), MDVUtility.ToLong(NotesId), MDVUtility.ToLong(SystemId));
                return new BLObject<DataSet>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLReviewOfSystem::select_reviewofsystem_template_note", ex);
                return new BLObject<DataSet>(null, ex.Message);
            }
        }
        public BLObject<string> delete_reviewofsystem_template_note(string ROSTemplateId, string NotesId)
        {
            try
            {
                ROSTemplateId = new DALReviewofSystems().delete_reviewofsystem_template_note(MDVUtility.ToLong(ROSTemplateId), MDVUtility.ToLong(NotesId));
                return new BLObject<string>(ROSTemplateId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::delete_reviewofsystem_template_note", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> toggle_Characteristics_note(string NotesID, string TemplateId, string ROSSystemId, string ROSCharacteristicsId, string IsPositive)
        {
            try
            {
                ROSSystemId = new DALReviewofSystems().toggle_Characteristics_note(MDVUtility.ToLong(NotesID), MDVUtility.ToLong(TemplateId), MDVUtility.ToLong(ROSSystemId), MDVUtility.ToLong(ROSCharacteristicsId), IsPositive);
                return new BLObject<string>(ROSSystemId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::deletePhysicalExamObservation", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion
    }
}
