using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Clinical;
using System;
using MDVision.DataAccess.DAL.Admin.FollowUp;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Model.Billing.FollowUp;
using System.Collections.Generic;

namespace MDVision.Business.BLL
{
    public class BLLAdminFollowUp
    {

        #region Constructors
        public BLLAdminFollowUp()
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

        #region Admin FollowUp Reason

        #region Admin FollowUp Reason CRUD

        /// <summary>
        /// Load the FollowUpReason
        /// </summary>
        /// <param name="ReasonId"></param>
        /// <param name="ShortName"></param>
        /// <param name="Description"></param>
        /// <param name="ARTypeID"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public BLObject<DSFollowUp> LoadFollowUpReasons(long ReasonId, string ShortName, string Description, string ARTypeID, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpReason().LoadFollowUpReasons(ReasonId, ShortName, Description, ARTypeID, IsActive, PageNumber,RowsPerPage);

                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpReason::LoadFollowUpReasons", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }

        /// <summary>
        /// Insert the FollowUpReason
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSFollowUp> InsertFollowupReason(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpReason().InsertFollowupReason(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpReason::InsertFollowupReason", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }

        /// <summary>
        /// Update the FollowUpReason
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSFollowUp> UpdateFollowUpReason(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpReason().UpdateFollowUpReason(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpReason::UpdateClinicalQuestions", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }

       /// <summary>
        /// Delete The FollowUpReason
       /// </summary>
       /// <param name="reasonId"></param>
       /// <returns></returns>
        public BLObject<string> DeleteFollowUpReason(string reasonId)
        {
            try
            {
                reasonId = new DALFollowUpReason().DeleteFollowUpReason(reasonId);

                return new BLObject<string>(reasonId);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLQuestion::DeleteQuestion", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        #endregion

        #region Admin FollowUp Reason lookUp
        /// <summary>
        /// Lookup ActionReasonType
        /// </summary>
        /// <returns></returns>
        public BLObject<DSFollowUp> LookupActionReasonType()
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpReason().LookupActionReasonType();
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpReason::LookupActionReasonType", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }

        public BLObject<DSFollowUp> LookupFollowUpReasons()
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpReason().LookupFollowUpReasons();
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpReason::LookupFollowUpReasons", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }

        #endregion

        #endregion

        #region Admin FollowUp ARType

        #region Admin FollowUp ARType CRUD

        /// <summary>
        /// Load the FollowUpARType
        /// </summary>
        /// <param name="ReasonId"></param>
        /// <param name="ShortName"></param>
        /// <param name="Description"></param>
        /// <param name="ARTypeID"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public BLObject<DSFollowUp> LoadFollowUpARTypes(long ARTypeId, string ShortName, string Description, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpARType().LoadFollowUpARType(ARTypeId, ShortName, Description, IsActive, PageNumber,RowsPerPage);

                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpARType::LoadFollowUpARTypes", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }

        /// <summary>
        /// Insert the FollowUpARType
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSFollowUp> InsertFollowUpARType(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARType().InsertFollowUpARType(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpARType::InsertFollowUpARType", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }

        /// <summary>
        /// Update the FollowUpARType
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSFollowUp> UpdateFollowUpARType(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARType().UpdateFollowUpARType(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpARType::UpdateClinicalQuestions", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }

        /// <summary>
        /// Delete The FollowUpARType
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        public BLObject<string> DeleteFollowUpARType(string reasonId)
        {
            try
            {
                reasonId = new DALFollowUpARType().DeleteFollowUpARType(reasonId);

                return new BLObject<string>(reasonId);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLQuestion::DeleteQuestion", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        #endregion

        #endregion

        #region Admin FollowUp Action
        /// <summary>
        /// Lookup AutoAction
        /// </summary>
        /// <returns></returns>
        public BLObject<DSFollowUp> LookupAutoAction()
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpAction().LookupAutoAction();
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LookupAutoAction", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> LookupFollowUpAction()
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpAction().LookupFollowUpAction();
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LookupFollowUpAction", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> LoadFollowUpAction(Int64 FollowupActionId, string shortName, string Description, Int64 ARTypeId, string isActive, int pageNumber, int recordsPerPage)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpAction().LoadFollowUpAction(FollowupActionId,shortName,Description,ARTypeId,isActive,pageNumber,recordsPerPage);

                return new BLObject<DSFollowUp>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LoadFollowUpAction", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }

        }
        public BLObject<DSFollowUp> InsertFollowUpAction(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpAction().InsertFollowUpAction(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::InsertFollowUpAction", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateFollowUpAction(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpAction().UpdateFollowUpAction(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::UpdateFollowUpAction", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteFollowUpAction(Int64 FollowupActionId)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpAction().DeleteFollowUpAction(FollowupActionId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::DeleteFollowUpAction", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region Admin FollowUp Remittance Code
        public BLObject<DSFollowUp> LoadRemittanceCode(int RemittanceId, string Code, string Description, string Rejection, int PageNumber = 1, int RowsPerPage = 1000, string IsActive = "")
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpRemittanceCode().LoadRemittanceCode(RemittanceId, Code, Description, Rejection, PageNumber, RowsPerPage, IsActive);

                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpReason::LoadRemittanceCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> InsertRemittanceCode(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpRemittanceCode().InsertRemittanceCode(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpReason::InsertRemittanceCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateRemittanceCode(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpRemittanceCode().UpdateRemittanceCode(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpReason::UpdateRemittanceCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteRemittanceCode(string RemittanceId)
        {
            try
            {
                RemittanceId = new DALFollowUpRemittanceCode().DeleteRemittanceCode(RemittanceId);

                return new BLObject<string>(RemittanceId);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLQuestion::DeleteRemittanceCode", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        #endregion

        #region Admin FollowUp Adjustment Reason Code
        public BLObject<DSFollowUp> LoadAdjustmentCode(int AdjustmentId, string Code, string Description, string Active, int PageNumber = 1, int RowsPerPage = 15)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpAdjustmentReasonCode().LoadAdjustmentReasonCode(AdjustmentId, Code, Description, Active, PageNumber, RowsPerPage);

                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpAdjustmentReasonCode::LoadAdjustmentReasonCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> InsertAdjustmentCode(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpAdjustmentReasonCode().InsertAdjustmentReasonCode(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpAdjustmentReasonCode::InsertAdjustmentReasonCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateAdjustmentCode(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpAdjustmentReasonCode().UpdateAdjustmentReasonCode(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpAdjustmentReasonCode::UpdateAdjustmentReasonCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteAdjustmentCode(string AdjustmentId)
        {
            try
            {
                AdjustmentId = new DALFollowUpAdjustmentReasonCode().DeleteAdjustmentReasonCode(AdjustmentId);

                return new BLObject<string>(AdjustmentId);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUpAdjustmentReasonCode::DeleteAdjustmentReasonCode", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        #endregion

        #region Admin FollowUp Claim Status Code
        public BLObject<DSFollowUp> LoadClaimStatusCode(int CSCodeId, string Code, string Description, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpClaimStatusCode().LoadClaimStatusCode(CSCodeId, Code, Description, Active, PageNumber,RowsPerPage);

                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LoadClaimStatusCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> InsertClaimStatusCode(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpClaimStatusCode().InsertClaimStatusCode(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::InsertClaimStatusCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateClaimStatusCode(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpClaimStatusCode().UpdateClaimStatusCode(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::UpdateClaimStatusCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteClaimStatusCode(string CSCodeId)
        {
            try
            {
                CSCodeId = new DALFollowUpClaimStatusCode().DeleteClaimStatusCode(CSCodeId);

                return new BLObject<string>(CSCodeId);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::DeleteClaimStatusCode", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        public BLObject<DSFollowUp> LookupClaimStatusCode()
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpClaimStatusCode().LookupClaimStatusCode();
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LookupClaimStatusCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        #endregion

        #region Admin FollowUp Claim Status Category Code
        public BLObject<DSFollowUp> LoadClaimStatusCategoryCode(int CSCatCodeId, string Code, string Description, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpClaimStatusCategoryCode().LoadClaimStatusCategoryCode(CSCatCodeId, Code, Description, Active, PageNumber,RowsPerPage);

                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LoadClaimStatusCategoryCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> InsertClaimStatusCategoryCode(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpClaimStatusCategoryCode().InsertClaimStatusCategoryCode(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::InsertClaimStatusCategoryCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateClaimStatusCategoryCode(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpClaimStatusCategoryCode().UpdateClaimStatusCategoryCode(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::UpdateClaimStatusCategoryCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteClaimStatusCategoryCode(string CSCatCodeId)
        {
            try
            {
                CSCatCodeId = new DALFollowUpClaimStatusCategoryCode().DeleteClaimStatusCategoryCode(CSCatCodeId);

                return new BLObject<string>(CSCatCodeId);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::DeleteClaimStatusCategoryCode", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        public BLObject<DSFollowUp> LookupClaimStatusCategoryCode()
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpClaimStatusCategoryCode().LookupClaimStatusCategoryCode();
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LookupClaimStatusCategoryCode", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        #endregion

        #region Admin FollowUp Codes Mapping
        public BLObject<DSFollowUp> LoadCodesMapping(long CodesMappingId, int ClaimStatusCodeId, int ClaimStatusCategoryCodeId, int ActionId, int ReasonId, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpCodesMapping().LoadCodesMapping(CodesMappingId, ClaimStatusCodeId, ClaimStatusCategoryCodeId, ActionId, ReasonId, Active, PageNumber,RowsPerPage);

                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LoadCodesMapping", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> InsertCodesMapping(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpCodesMapping().InsertCodesMapping(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::InsertCodesMapping", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateCodesMapping(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpCodesMapping().UpdateCodesMapping(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::UpdateCodesMapping", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteCodesMapping(string CSCatCodeId)
        {
            try
            {
                CSCatCodeId = new DALFollowUpCodesMapping().DeleteCodesMapping(CSCatCodeId);

                return new BLObject<string>(CSCatCodeId);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::DeleteCodesMapping", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }
        #endregion

        #region Admin FollowUp AR Group
        public BLObject<DSFollowUp> LookupARGroup()
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpARGroup().LookupARGroup();
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LookupARGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> LoadARGroup(Int64 ARGroupId, string shortName ="", string Description ="", string isActive="", int pageNumber = 1, int RecordPerPage = 15)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpARGroup().LoadARGroup(ARGroupId, shortName, Description, isActive, pageNumber, RecordPerPage);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LoadARGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }

        }
        public BLObject<DSFollowUp> InsertARGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARGroup().InsertARGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::InsertARGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateARGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARGroup().UpdateARGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::UpdateARGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteARGroup(Int64 ARGroupId)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpARGroup().DeleteARGroup(ARGroupId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::DeleteARGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region Admin FollowUp AR Facility Group

        public BLObject<DSFollowUp> LoadARFacilityGroup(Int64 ARFacilityGroupId, Int64 ARGroupId, int pageNumber = 0, int RecordPerPage = 0)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpARFacilityGroup().LoadARFacilityGroup(ARFacilityGroupId , ARGroupId, pageNumber, RecordPerPage);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LoadARFacilityGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }

        }
        public BLObject<DSFollowUp> InsertARFacilityGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARFacilityGroup().InsertARFacilityGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::InsertARFacilityGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateARFacilityGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARFacilityGroup().UpdateARFacilityGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::UpdateARFacilityGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteARFacilityGroup(Int64 ARGroupFacilityId)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpARFacilityGroup().DeleteARFacilityGroup(ARGroupFacilityId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::DeleteARFacilityGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region Admin FollowUp AR Plan Category Group

        public BLObject<DSFollowUp> LoadARPlanCategoryGroup(Int64 ARPlanCategoryGroupId, Int64 ARGroupId, int pageNumber, int RecordPerPage)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpARPlanCategoryGroup().LoadARPlanCategoryGroup(ARPlanCategoryGroupId ,ARGroupId, pageNumber, RecordPerPage);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LoadARPlanCategoryGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }

        }
        public BLObject<DSFollowUp> InsertARPlanCategoryGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARPlanCategoryGroup().InsertARPlanCategoryGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::InsertARPlanCategoryGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateARPlanCategoryGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARPlanCategoryGroup().UpdateARPlanCategoryGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::UpdateARPlanCategoryGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteARPlanCategoryGroup(Int64 ARPlanCategoryGroupId)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpARPlanCategoryGroup().DeleteARPlanCategoryGroup(ARPlanCategoryGroupId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::DeleteARPlanCategoryGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region Admin FollowUp AR Plan Type Group

        public BLObject<DSFollowUp> LoadARPTGroup(Int64 ARPTGroupId, Int64 ARGroupId, int pageNumber, int RecordPerPage)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpARPlanTypeGroup().LoadARPTGroup(ARPTGroupId ,ARGroupId, pageNumber, RecordPerPage);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LoadARPTGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }

        }
        public BLObject<DSFollowUp> InsertARPTGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARPlanTypeGroup().InsertARPTGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::InsertARPTGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateARPTGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARPlanTypeGroup().UpdateARPTGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::UpdateARPTGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteARPTGroup(Int64 ARPTGroupId)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpARPlanTypeGroup().DeleteARPTGroup(ARPTGroupId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::DeleteARPTGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region Admin FollowUp AR POS Group

        public BLObject<DSFollowUp> LoadARPOSGroup(Int64 ARPOSGroup, Int64 ARGroupId, int pageNumber, int RecordPerPage)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpARPOSGroup().LoadARPOSGroup(ARPOSGroup, ARGroupId, pageNumber, RecordPerPage);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LoadARPOSGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }

        }
        public BLObject<DSFollowUp> InsertARPOSGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARPOSGroup().InsertARPOSGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::InsertARPOSGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateARPOSGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARPOSGroup().UpdateARPOSGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::UpdateARPOSGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteARPOSGroup(Int64 ARPOSGroup)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpARPOSGroup().DeleteARPOSGroup(ARPOSGroup));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::DeleteARPOSGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region Admin FollowUp AR Provider Group

        public BLObject<DSFollowUp> LoadARProviderGroup(Int64 ARProviderGroupId, Int64 ARGroupId, int pageNumber, int RecordPerPage)
        {
            try
            {
                DSFollowUp ds = new DSFollowUp();
                ds = new DALFollowUpARProviderGroup().LoadARProviderGroup(ARProviderGroupId, ARGroupId, pageNumber, RecordPerPage);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::LoadARProviderGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }

        }
        public BLObject<DSFollowUp> InsertARProviderGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARProviderGroup().InsertARProviderGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::InsertARProviderGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<DSFollowUp> UpdateARProviderGroup(DSFollowUp ds)
        {
            try
            {
                ds = new DALFollowUpARProviderGroup().UpdateARProviderGroup(ds);
                return new BLObject<DSFollowUp>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::UpdateARProviderGroup", ex);
                return new BLObject<DSFollowUp>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteARProviderGroup(Int64 ARProviderGroupId)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpARProviderGroup().DeleteARProviderGroup(ARProviderGroupId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::DeleteARProviderGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion
        #region FollowUpComments
        public List<FollowUpARComments> GetFollowUpComments(Int64 FollowUpCommentId = 0, Int64 VisitId = 0)
        {
            try
            {
                return (new DALFollowUpAction().GetFollowUpComments(FollowUpCommentId, VisitId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::GetFollowUpComments", ex);
                return new List<FollowUpARComments>() ;
            }

        }
        public BLObject<string> InsertNewFollowUpComments(FollowUpARComments model)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpAction().InsertNewFollowUpComments(model));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::InsertNewFollowUpComments", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> UpdateNewFollowUpComments(FollowUpARComments model)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpAction().UpdateNewFollowUpComments(model));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::UpdateNewFollowUpComments", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteNewFollowUpComments(Int64 FollowUpCommentId)
        {
            try
            {
                return new BLObject<string>(new DALFollowUpAction().DeleteNewFollowUpComments(FollowUpCommentId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminFollowUp::DeleteNewFollowUpComments", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion
    }
}
