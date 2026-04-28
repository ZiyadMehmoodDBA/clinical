using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Admin;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.Business.BLL
{

    public class BLLFeeSchedule
    {
        #region Constructors
        public BLLFeeSchedule()
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call

        }

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Variable
        
        #endregion

        #region "PlanFeeLink"
        /// <summary>
        /// Loads the plan fee link.
        /// </summary>
        /// <param name="PlanFeeLinkId">The plan fee link identifier.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> LoadPlanFeeLink(long PlanFeeLinkId, string Name, string Description, string EntityId,string IsActive,int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSFeeSchedule ds = new DSFeeSchedule();
                ds = new DALPlanFeeLink().LoadPlanFeeLink(PlanFeeLinkId, Name, Description, EntityId,IsActive, PageNumber, RowsPerPage);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::LoadPlanFeeLink", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the PlanFeeLink.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> InsertPlanFeeLink(ref DSFeeSchedule ds)
        {
            try
            {
                ds = new DALPlanFeeLink().InsertPlanFeeLink(ref ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::InsertPlanFeeLink", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the PlanFeeLink.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// 
        public BLObject<DSFeeSchedule> UpdatePlanFeeLink(ref DSFeeSchedule ds)
        {
            try
            {
                ds = new DALPlanFeeLink().UpdatePlanFeeLink(ref ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::UpdatePlanFeeLink", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the plan fee link.
        /// </summary>
        /// <param name="PlanFeeLinkIds">The plan fee link ids.</param>
        /// <returns></returns>
        public BLObject<string> DeletePlanFeeLink(string PlanFeeLinkId)
        {
            try
            {
                PlanFeeLinkId = new DALPlanFeeLink().DeletePlanFeeLink(PlanFeeLinkId);
                return new BLObject<string>(PlanFeeLinkId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::DeletePlanFeeLink", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "FeeGroup"
        public BLObject<DSFeeSchedule> LoadFeeGroup(long FeeGroupId, string Name, string Description, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSFeeSchedule ds = new DSFeeSchedule();
                ds = new DALFeeGroup().LoadFeeGroup(FeeGroupId, Name, Description, EntityId,IsActive, PageNumber, RowsPerPage);

                return new BLObject<DSFeeSchedule>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::LoadLedgerAccount", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }

        }

        /// <summary>
        /// Inserts the FeeGroup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> InsertFeeGroup(ref DSFeeSchedule ds)
        {
            try
            {

                ds = new DALFeeGroup().InsertFeeGroup(ref ds);

                return new BLObject<DSFeeSchedule>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::InsertFeeGroup", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }

        }

        /// <summary>
        /// Updates the FeeGroup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// 
        public BLObject<DSFeeSchedule> UpdateFeeGroup(ref DSFeeSchedule ds)
        {
            try
            {

                ds = new DALFeeGroup().UpdateFeeGroup(ref ds);

                return new BLObject<DSFeeSchedule>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::UpdateFeeGroup", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }

        }
        /// <summary>
        /// Deletes the FeeGroup.
        /// </summary>
        /// <param name="UserIds">The user ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteFeeGroup(string FeeGroupIds)
        {
            try
            {

                FeeGroupIds = new DALFeeGroup().DeleteFeeGroup(FeeGroupIds);

                return new BLObject<string>(FeeGroupIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::DeleteFeeGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        //<summary>
        //Lookups the Procedure Category.
        //</summary>
        //<returns></returns>
        public BLObject<DSFeeScheduleLookup> LookupFeeGroup()
        {
            try
            {
                DSFeeScheduleLookup ds = new DSFeeScheduleLookup();
                ds = new DALFeeGroup().LookupFeeGroup();

                return new BLObject<DSFeeScheduleLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::LookupFeeGroup", ex);
                return new BLObject<DSFeeScheduleLookup>(null, ex.Message);
            }

        }

        #endregion

        #region "BasicFeeGroup"
        public BLObject<DSFeeSchedule> LoadBasicFeeGroup(long BasicFeeGroupId, string Name, string Description, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSFeeSchedule ds = new DSFeeSchedule();
                ds = new DALBasicFeeGroup().LoadBasicFeeGroup(BasicFeeGroupId, Name, Description, EntityId,IsActive, PageNumber,RowsPerPage);

                return new BLObject<DSFeeSchedule>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::LoadBasicFeeGroup", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }

        }

        /// <summary>
        /// Inserts the BasicFeeGroup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> InsertBasicFeeGroup(ref DSFeeSchedule ds)
        {
            try
            {

                ds = new DALBasicFeeGroup().InsertBasicFeeGroup(ref ds);

                return new BLObject<DSFeeSchedule>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::InsertBasicFeeGroup", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }

        }

        /// <summary>
        /// Updates the BasicFeeGroup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// 
        public BLObject<DSFeeSchedule> UpdateBasicFeeGroup(ref DSFeeSchedule ds)
        {
            try
            {

                ds = new DALBasicFeeGroup().UpdateBasicFeeGroup(ref ds);

                return new BLObject<DSFeeSchedule>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::UpdateBasicFeeGroup", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }

        }
        /// <summary>
        /// Deletes the BasicFeeGroup.
        /// </summary>
        /// <param name="UserIds">The user ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteBasicFeeGroup(string BasicFeeGroupIds)
        {
            try
            {

                BasicFeeGroupIds = new DALBasicFeeGroup().DeleteBasicFeeGroup(BasicFeeGroupIds);

                return new BLObject<string>(BasicFeeGroupIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::DeleteBasicFeeGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        //<summary>
        //Lookups the Procedure Category.
        //</summary>
        //<returns></returns>
        public BLObject<DSFeeScheduleLookup> LookupBasicFeeGroup()
        {
            try
            {
                DSFeeScheduleLookup ds = new DSFeeScheduleLookup();
                ds = new DALBasicFeeGroup().LookupBasicFeeGroup();

                return new BLObject<DSFeeScheduleLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeShedule::LookupBasicFeeGroup", ex);
                return new BLObject<DSFeeScheduleLookup>(null, ex.Message);
            }

        }
        #endregion

        #region Basic Fee Schedule
        /// <summary>
        /// Loads the basic fee schedule.
        /// </summary>
        /// <param name="BasicFeeScheduleId">The basic fee schedule identifier.</param>
        /// <param name="BasicFeeGroupId">The basic fee group identifier.</param>
        /// <param name="CPTCode">The CPT identifier.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> LoadBasicFeeSchedule(long BasicFeeScheduleId, string BasicFeeGroupId, string CPTCode, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSFeeSchedule ds = new DSFeeSchedule();
                ds = new DALBasicFeeSchedule().LoadBasicFeeSchedule(BasicFeeScheduleId, BasicFeeGroupId, CPTCode,IsActive, PageNumber, RowsPerPage);

                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::LoadBasicFeeSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the basic fee schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> UpdateBasicFeeSchedule(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALBasicFeeSchedule().UpdateBasicFeeSchedule(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::UpdateBasicFeeSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the basic fee schedule.
        /// </summary>
        /// <param name="BasicFeeScheduleId">The basic fee schedule identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeleteBasicFeeSchedule(string BasicFeeScheduleId)
        {
            try
            {
                BasicFeeScheduleId = new DALBasicFeeSchedule().DeleteBasicFeeSchedule(BasicFeeScheduleId);
                return new BLObject<string>(BasicFeeScheduleId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::DeleteBasicFeeSchedule", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the basic fee schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> InsertBasicFeeSchedule(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALBasicFeeSchedule().InsertBasicFeeSchedule(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::InsertBasicFeeSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        #region "Plan Specific Info"
        /// <summary>
        /// Loads the BFS plan information.
        /// </summary>
        /// <param name="BFSId">The BFS identifier.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> LoadBFSPlanInfo(long BFSId, long BFSPlanId)
        {
            try
            {
                DSFeeSchedule ds = new DSFeeSchedule();
                ds = new DALBasicFeeSchedule().LoadBFSPlan(BFSId,  BFSPlanId);

                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::LoadBFSPlanInfo", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the BFS plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> UpdateBFSPlanInfo(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALBasicFeeSchedule().UpdateBFSPlan(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::UpdateBFSPlanInfo", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the BFS plan information.
        /// </summary>
        /// <param name="BFSPlanId">The BFS plan identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeleteBFSPlanInfo(string BFSPlanId)
        {
            try
            {
                BFSPlanId = new DALBasicFeeSchedule().DeleteBFSPlan(BFSPlanId);
                return new BLObject<string>(BFSPlanId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::DeleteBFSPlanInfo", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the BFS plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> InsertBFSPlanInfo(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALBasicFeeSchedule().InsertBFSPlan(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::InsertBFSPlanInfo", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }
        #endregion
        #endregion

        #region Fee Group Procedural Schedule
        /// <summary>
        /// Loads the fee group procedural schedule.
        /// </summary>
        /// <param name="FeeGroupProcSchId">The fee group proc SCH identifier.</param>
        /// <param name="FeeGroupId">The fee group identifier.</param>
        /// <param name="PlanFeeLinkId">The plan fee link identifier.</param>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> LoadFeeGroupProceduralSchedule(long FeeGroupProcSchId, string FeeGroupId, string PlanFeeLinkId, string CPTCode, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSFeeSchedule ds = new DSFeeSchedule();
                ds = new DALFeeGroupProceduralSchedule().LoadFeeGroupProceduralSchedule(FeeGroupProcSchId, FeeGroupId, PlanFeeLinkId, CPTCode, IsActive,PageNumber, RowsPerPage);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::LoadFeeGroupProceduralSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the fee group procedural schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> UpdateFeeGroupProceduralSchedule(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALFeeGroupProceduralSchedule().UpdateFeeGroupProceduralSchedule(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::UpdateFeeGroupProceduralSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the fee group procedural schedule.
        /// </summary>
        /// <param name="FeeGroupProcSchId">The fee group proc SCH identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeleteFeeGroupProceduralSchedule(string FeeGroupProcSchId)
        {
            try
            {
                FeeGroupProcSchId = new DALFeeGroupProceduralSchedule().DeleteFeeGroupProceduralSchedule(FeeGroupProcSchId);
                return new BLObject<string>(FeeGroupProcSchId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::DeleteFeeGroupProceduralSchedule", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the fee group procedural schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> InsertFeeGroupProceduralSchedule(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALFeeGroupProceduralSchedule().InsertFeeGroupProceduralSchedule(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::InsertFeeGroupProceduralSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        #region "Plan Specific Modifier Info"
        public BLObject<DSFeeSchedule> LoadFeeGroupProceduralModifierSchedule(long FeeGroupProcId,long FeeGroupProcModifierFeeSchId)
        {
            try
            {
                DSFeeSchedule ds = new DSFeeSchedule();
                ds = new DALFeeGroupProceduralSchedule().LoadFeeGroupProceduralModifierSchedule(FeeGroupProcId,FeeGroupProcModifierFeeSchId);

                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::LoadFeeGroupProceduralModifierSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        public BLObject<DSFeeSchedule> UpdateFeeGroupProceduralModifierSchedule(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALFeeGroupProceduralSchedule().UpdateFeeGroupProceduralModifierSchedule(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::UpdateFeeGroupProceduralModifierSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        public BLObject<string> DeleteFeeGroupProceduralModifierSchedule(string FeeGroupProcModifierFeeSchId)
        {
            try
            {
                FeeGroupProcModifierFeeSchId = new DALFeeGroupProceduralSchedule().DeleteFeeGroupProceduralModifierSchedule(FeeGroupProcModifierFeeSchId);
                return new BLObject<string>(FeeGroupProcModifierFeeSchId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::DeleteFeeGroupProceduralModifierSchedule", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        public BLObject<DSFeeSchedule> InsertFeeGroupProceduralModifierSchedule(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALFeeGroupProceduralSchedule().InsertFeeGroupProceduralModifierSchedule(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::InsertFeeGroupProceduralModifierSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }
        #endregion
        #endregion

        #region Fee Group POS Schedule
        /// <summary>
        /// Loads the fee group position schedule.
        /// </summary>
        /// <param name="FeeGroupPOSScheduleId">The fee group position schedule identifier.</param>
        /// <param name="FeeGroupId">The fee group identifier.</param>
        /// <param name="PlanFeeLinkId">The plan fee link identifier.</param>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <param name="POSId">The position identifier.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> LoadFeeGroupPOSSchedule(long FeeGroupPOSScheduleId, string FeeGroupId, string PlanFeeLinkId, string CPTCode, string POSId,  string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSFeeSchedule ds = new DSFeeSchedule();
                ds = new DALFeeGroupPOSSchedule().LoadFeeGroupPOSSchedule(FeeGroupPOSScheduleId, FeeGroupId, PlanFeeLinkId, CPTCode, POSId,IsActive, PageNumber, RowsPerPage);

                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::LoadFeeGroupPOSSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the fee group position schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> UpdateFeeGroupPOSSchedule(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALFeeGroupPOSSchedule().UpdateFeeGroupPOSSchedule(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::UpdateFeeGroupPOSSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the fee group position schedule.
        /// </summary>
        /// <param name="FeeGroupPOSSchIds">The fee group position SCH ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteFeeGroupPOSSchedule(string FeeGroupPOSSchIds)
        {
            try
            {
                FeeGroupPOSSchIds = new DALFeeGroupPOSSchedule().DeleteFeeGroupPOSSchedule(FeeGroupPOSSchIds);
                return new BLObject<string>(FeeGroupPOSSchIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::DeleteFeeGroupPOSSchedule", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the fee group position schedule.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSFeeSchedule> InsertFeeGroupPOSSchedule(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALFeeGroupPOSSchedule().InsertFeeGroupPOSSchedule(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::InsertFeeGroupPOSSchedule", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        #region "Plan Specific Info"
        public BLObject<DSFeeSchedule> LoadPOSPlanInfo(long POSId, long POSPlanId)
        {
            try
            {
                DSFeeSchedule ds = new DSFeeSchedule();
                ds = new DALFeeGroupPOSSchedule().LoadPOSPlan(POSId, POSPlanId);

                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::LoadPOSPlanInfo", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        public BLObject<DSFeeSchedule> UpdatePOSPlanInfo(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALFeeGroupPOSSchedule().UpdatePOSPlan(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::UpdatePOSPlanInfo", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }

        public BLObject<string> DeletePOSPlanInfo(string POSPlanIds)
        {
            try
            {
                POSPlanIds = new DALFeeGroupPOSSchedule().DeletePOSPlan(POSPlanIds);
                return new BLObject<string>(POSPlanIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::DeletePOSPlanInfo", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        public BLObject<DSFeeSchedule> InsertPOSPlanInfo(DSFeeSchedule ds)
        {
            try
            {
                ds = new DALFeeGroupPOSSchedule().InsertPOSPlan(ds);
                return new BLObject<DSFeeSchedule>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLFeeSchedule::InsertPOSPlanInfo", ex);
                return new BLObject<DSFeeSchedule>(null, ex.Message);
            }
        }
        #endregion
        #endregion

        
    }
}


