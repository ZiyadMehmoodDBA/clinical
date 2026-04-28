using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.iTrack;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MDVision.IEHR.EMR.Model;
using MDVision.Model.Clinical.ReviewOfSystem;
using System.Data;
using MDVision.Model.AuditableEvents;
using MDVision.Model.iTrack;
using MDVision.DataAccess.DAL.Clinical;

namespace MDVision.Business.BLL
{
    /// <summary>
    /// Author: Zia Mehmood 
    /// Created Date: 26-06-2017
    /// Overview: Business Logic Layer for new Review of system
    /// </summary>
    public class BLLiTrack
    {
        #region " Constructors "
        public BLLiTrack()
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

        public List<ActivityLog> loadAcitivityLogUser(ActivityLog model)
        {

            List<ActivityLog> objList_ActivityLog= new List<ActivityLog>();
            try
            {

                objList_ActivityLog = new DALAuditbleEventsActivityLog().loadAcitivityLogUser(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogUser", ex);
                throw ex.InnerException;

            }
            return objList_ActivityLog;
        }
        public List<Dashboard> LoadMIPSKPIs(Dashboard model,DataTable dtCQM,DataTable dtMu)
        {

            List<Dashboard> objList_iTrack= new List<Dashboard>();
            try
            {

                objList_iTrack = new DALiTrack().LoadMIPSKPIs(model,dtCQM,dtMu);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogComponents", ex);
                throw ex.InnerException;


            }
            return objList_iTrack;
        }
        public BLObject<List<IndvidualProvider>> SelectIndividualProvider(IndvidualProvider model)
        {
            try
            {
                var result = new DALiTrack().SelectIndividualProvider(model);
                return new BLObject<List<IndvidualProvider>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLiTrack::SelectIndividualProvider", ex);
                return new BLObject<List<IndvidualProvider>>(null, ex.Message);
            }
        }
        public BLObject<List<IndvidualProvider>> LoadIndividualProvider(IndvidualProvider model)
        {

            try
            {

                var result = new DALiTrack().LoadIndividualProvider(model);
                return new BLObject<List<IndvidualProvider>>(result);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLiTrack::LoadIndividualProvider", ex);
                return new BLObject<List<IndvidualProvider>>(null, ex.Message);

            }
            
        }

        public BLObject<List<IndvidualProvider>> LoadIndividualProviderDetail(long categoryId, long pageNumber, long rowsPPage)
        {

            try
            {

                var result = new DALiTrack().LoadIndividualProviderDetail(categoryId, pageNumber, rowsPPage);
                return new BLObject<List<IndvidualProvider>>(result);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLiTrack::LoadIndividualProvider", ex);
                return new BLObject<List<IndvidualProvider>>(null, ex.Message);

            }

        }
        public BLObject<IndvidualProvider> LoadIndividualProviderData(long providerId)
        {
            try
            {
                var result = new DALiTrack().LoadIndividualProviderData(providerId);
                return new BLObject<IndvidualProvider>(result);
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLiTrack::LoadIndividualProviderData", ex);
                return new BLObject<IndvidualProvider>(null, ex.Message);
            }
        }
        public BLObject<IndvidualProvider> LoadGroupDetailData(long GroupId)
        {
            try
            {
                var result = new DALiTrack().LoadGroupDetailData(GroupId);
                return new BLObject<IndvidualProvider>(result);
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLiTrack::LoadGroupDetailData", ex);
                return new BLObject<IndvidualProvider>(null, ex.Message);
            }
        }
        public BLObject<List<IndvidualProvider>> LoadMIPSSummaryGroupDetailData(long GroupId)
        {
            try
            {
                var result = new DALiTrack().LoadMIPSSummaryGroupDetailData(GroupId);
                return new BLObject<List<IndvidualProvider>>(result);
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLiTrack::LoadMIPSSummaryGroupDetailData", ex);
                return new BLObject<List<IndvidualProvider>>(null, ex.Message);
            }
        }
        public BLObject<List<IndvidualProvider>> LoadMIPSSummaryIndividualProviderDetailData(long ProviderId)
        {
            try
            {
                var result = new DALiTrack().LoadMIPSSummaryIndividualProviderDetailData(ProviderId);
                return new BLObject<List<IndvidualProvider>>(result);
            }
            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLiTrack::LoadMIPSSummaryIndividualProviderDetailData", ex);
                return new BLObject<List<IndvidualProvider>>(null, ex.Message);
            }
        }
        
        public List<IndvidualProvider> LoadPracticLookup(string  EntityId)
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {

                objList_IPro = new DALiTrack().LoadPracticLookup(EntityId);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_IPro;
        }
        public List<IndvidualProvider> LoadGroupCatLookup()
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {

                objList_IPro = new DALiTrack().LoadGroupCatLookup();
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_IPro;
        }
        public List<IndvidualProvider> LoadIneligibleReasonLookup()
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {

                objList_IPro = new DALiTrack().LoadIneligibleReasonLookup();
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_IPro;
        }
        public List<IndvidualProvider> LoadParticipatingReasonLookup()
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {

                objList_IPro = new DALiTrack().LoadParticipatingReasonLookup();
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLiTrack::LoadParticipatingReasonLookup", ex);
                throw ex.InnerException;

            }
            return objList_IPro;
        }
        public List<IndvidualProvider> LoadGroupNameLookup()
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {

                objList_IPro = new DALiTrack().LoadGroupNameLookup();
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_IPro;
        }
        public List<IndvidualProvider> SaveMIPSPreferencesGroup(IndvidualProvider model,string xml= null)
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {

                objList_IPro = new DALiTrack().SaveMIPSPreferencesGroup(model,xml);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_IPro;
        }
        public List<IndvidualProvider> SaveMIPSPreferencesIndvidual(IndvidualProvider model, string xml = null)
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {

                objList_IPro = new DALiTrack().SaveMIPSPreferencesIndvidual(model, xml);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_IPro; 
        }
        public BLObject<string> UpdateMIPSPreferencesIndvidual(IndvidualProvider model, string xml = null)
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {

                string status = new DALiTrack().UpdateMIPSPreferencesIndvidual(model, xml);
                return new BLObject<string>(status);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLiTrack::UpdateMIPSPreferencesIndvidual", ex);
                return new BLObject<string>("", ex.Message);


            }
            
        }
        public BLObject<string> UpdateiTrackReportingType(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {

                string status = new DALiTrack().UpdateiTrackReportingType(model);
                return new BLObject<string>(status);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLiTrack::UpdateiTrackReportingType", ex);
                return new BLObject<string>("", ex.Message);


            }

        }
        public List<IndvidualProvider> ActiveInActiveMIPSPreferencesGroup(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {

                objList_IPro = new DALiTrack().ActiveInActiveMIPSPreferencesGroup(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_IPro;
        }
        public BLObject<string> UpdateMIPSPreferencesGroup(IndvidualProvider model, string xml = null)
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {
                string status = "";
                status = new DALiTrack().UpdateMIPSPreferencesGroup(model, xml);
                return new BLObject<string>(status);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLiTrack::UpdateMIPSPreferencesGroup", ex);
                return new BLObject<string>("", ex.Message);

            }
           
        }
        public List<IndvidualProvider> LoadMIPSProvider(IndvidualProvider model)
        {

            List<IndvidualProvider> objList_IPro = new List<IndvidualProvider>();
            try
            {

                objList_IPro = new DALiTrack().LoadMIPSProvider(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_IPro;
        }
        public MIPSGrouupPreferenceList SearchMIPSGroupPreferences(IndvidualProvider model)
        {

            MIPSGrouupPreferenceList objList_IPro = new MIPSGrouupPreferenceList();
            try
            {

                objList_IPro = new DALiTrack().SearchMIPSGroupPreferences(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_IPro;
        }
        public MIPSGrouupPreferenceList SearchMIPSProviderPreferences(IndvidualProvider model)
        {

            MIPSGrouupPreferenceList objList_IPro = new MIPSGrouupPreferenceList();
            try
            {

                objList_IPro = new DALiTrack().SearchMIPSProviderPreferences(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_IPro;
        }
        public MIPSGrouupPreferenceList SelectMIPSGroupPreferences(IndvidualProvider model)
        {

            MIPSGrouupPreferenceList objList_IPro = new MIPSGrouupPreferenceList();
            try
            {

                objList_IPro = new DALiTrack().SelectMIPSGroupPreferences(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_IPro;
        }
        public List<ActivityLog> loadAcitivityLogChanges(ActivityLog model)
        {

            List<ActivityLog> objList_ActivityLog = new List<ActivityLog>();
            try
            {

                objList_ActivityLog = new DALAuditbleEventsActivityLog().loadAcitivityLogChanges(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogChanges", ex);
                throw ex.InnerException;

            }
            return objList_ActivityLog;
        }
        #endregion

    }
}
