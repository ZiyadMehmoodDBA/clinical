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
using MDVision.Model.AuditableEvents;

namespace MDVision.Business.BLL
{
    /// <summary>
    /// Author: Zia Mehmood
    /// Created Date: 31-05-2017
    /// Overview: Business Logic Layer for new Review of system
    /// </summary>
    public class BLLAuditbleEventsActivityLog
    {
        #region " Constructors "
        public BLLAuditbleEventsActivityLog()
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
        public List<ActivityLog> loadAcitivityLogComponents(ActivityLog model)
        {

            List<ActivityLog> objList_ActivityLog = new List<ActivityLog>();
            try
            {

                objList_ActivityLog = new DALAuditbleEventsActivityLog().loadAcitivityLogComponents(model);
            }

            catch (Exception ex)
            {

                MDVLogger.BLLErrorLog("BLLAuditbleEventsActivityLog::loadAcitivityLogComponents", ex);
                throw ex.InnerException;

            }
            return objList_ActivityLog;
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
        public List<ActivityLog> loadCheckInAppUser(ActivityLog model)
        {

            List<ActivityLog> objList_ActivityLog = new List<ActivityLog>();
            try
            {

                objList_ActivityLog = new DALAuditbleEventsActivityLog().loadCheckInAppUser(model);
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
