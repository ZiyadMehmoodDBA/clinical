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
using System.Data;
using MDVision.Model.Batch.ExportCCDA;
using MDVision.DataAccess.DAL.Batch;

namespace MDVision.Business.BLL
{
    /// <summary>
    /// Author: Zia Mehmood
    /// Created Date: 31-05-2017
    /// Overview: Business Logic Layer for new Review of system
    /// </summary>
    public class BLLExportCCDA
    {
        #region " Constructors "
        public BLLExportCCDA()
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
        #region " Methods"

        public List<ExportCCDAModel> Fill_Paitent_Lookpup(ExportCCDAModel model)
        {

            List<ExportCCDAModel> objList_ExportCCDA= new List<ExportCCDAModel>();
            try
            {

                objList_ExportCCDA = new DALExportCCDA().Fill_Paitent_Lookpup(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLExportCCDA::Fill_Paitent_Lookpup", ex);
                throw ex.InnerException;

            }
            return objList_ExportCCDA;
        }
        public List<ExportCCDAModel> Get_Scheduled_PatientVisits(ExportCCDAModel model)
        {

            List<ExportCCDAModel> objList_ExportCCDA = new List<ExportCCDAModel>();
            try
            {

                objList_ExportCCDA = new DALExportCCDA().Get_Scheduled_PatientVisits(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLExportCCDA::Get_Scheduled_PatientVisits", ex);
                throw ex.InnerException;

            }
            return objList_ExportCCDA;
        }
        public List<ExportCCDAModel> Fill_NoteComponent_Lookpup(ExportCCDAModel model)
        {

            List<ExportCCDAModel> objList_ExportCCDA = new List<ExportCCDAModel>();
            try
            {

                objList_ExportCCDA = new DALExportCCDA().Fill_NoteComponent_Lookpup(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLExportCCDA::Fill_NoteComponent_Lookpup", ex);
                throw ex.InnerException;

            }
            return objList_ExportCCDA;
        }
        public ExportCCDAModel Insert_CCDA_Schedule(ExportCCDAModel model)
        {

            ExportCCDAModel objList_ExportCCDA = new ExportCCDAModel();
            try
            {

                objList_ExportCCDA = new DALExportCCDA().Insert_CCDA_Schedule(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLExportCCDA::Insert_CCDA_Schedule", ex);
                throw ex.InnerException;

            }
            return objList_ExportCCDA;
        }

        public List<ExportCCDAModel> Load_CCDA_Schedule(ExportCCDAModel model)
        {

            List<ExportCCDAModel> objList_ExportCCDA = new List<ExportCCDAModel>();
            try
            {

                objList_ExportCCDA = new DALExportCCDA().Load_CCDA_Schedule(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLExportCCDA::Load_CCDA_Schedule", ex);
                throw ex.InnerException;

            }
            return objList_ExportCCDA;
        }
        public List<ExportCCDAModel> Select_CCDA_Schedule(ExportCCDAModel model)
        {

            List<ExportCCDAModel> objList_ExportCCDA = new List<ExportCCDAModel>();
            try
            {

                objList_ExportCCDA = new DALExportCCDA().Select_CCDA_Schedule(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLExportCCDA::Load_CCDA_Schedule", ex);
                throw ex.InnerException;

            }
            return objList_ExportCCDA;
        }
        public BLObject<string> Delete_CCDA_Schedule(string SchedulerId)
        {
            try
            {
                SchedulerId = new DALExportCCDA().Delete_CCDA_Schedule(MDVUtility.ToLong(SchedulerId));
                return new BLObject<string>(SchedulerId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::deletePhysicalExamObservation", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> ActiveInactive_CCDA_Schedule(string SchedulerId, string IsActive, string ModifiedBy)
        {
            try
            {
                SchedulerId = new DALExportCCDA().ActiveInactive_CCDA_Schedule(MDVUtility.ToLong(SchedulerId), IsActive, ModifiedBy);
                return new BLObject<string>(SchedulerId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPhysicalExamECW::activeinactive_reviewofsystem_template", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

    }
}
