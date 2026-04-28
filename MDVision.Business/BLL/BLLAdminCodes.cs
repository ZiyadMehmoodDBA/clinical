using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Admin;
using MDVision.DataAccess.DAL.iTrack;
using MDVision.Model.Lookups;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.DataAccess.DCommon;
using MDVision.Model.Admin.Codes;

namespace MDVision.Business.BLL
{
    public class BLLAdminCodes
    {
        #region Constructors
        public BLLAdminCodes()
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

        #region "Functions"
        #region Lookups
        /// <summary>
        /// Lookups the place of service.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodeLookup> LookupPlaceOfService()
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALPlaceOfService().LookupPlaceOfService();

                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LookupPlaceOfService", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }

        //<summary>
        //Lookups the Type Of Service.
        //</summary>
        //<returns></returns>
        public BLObject<DSCodeLookup> LookupTypeOfService(string IsActive)
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALTypeOfService().LookupTypeOfService(IsActive);

                return new BLObject<DSCodeLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LookupTypeOfService", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the NDC Measurement Code.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodeLookup> LookupNDCMeasurementCode()
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALCodes().LookupNDCMeasurementCode();

                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCodes::LookupPlaceOfService", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the gender.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodes> LookupGender()
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALCodes().LookupGender();

                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCodes::LookupGender", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        public BLObject<List<GenderModel>> LookupGenderDemographic()
        {
            try
            {
                var ds = new DALCodes().LookupGenderDemographic();
                return new BLObject<List<GenderModel>>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCodes::LookupGenderDemographic", ex);
                return new BLObject<List<GenderModel>>(null, ex.Message);
            }
        }
        public BLObject<List<ProviderParticipationStatusModel>> LookupParticipentProvider()
        {
            try
            {
                var ds = new DALCodes().LookupParticipentStatus();
                return new BLObject<List<ProviderParticipationStatusModel>>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCodes::LookupParticipentProvider", ex);
                return new BLObject<List<ProviderParticipationStatusModel>>(null, ex.Message);
            }
        }
        //
        public BLObject<List<CaseAdjusterLookup>> LookupCaseAdjuster()
        {
            try
            {
                var ds = new DALCodes().LookupCaseAdjuster();
                return new BLObject<List<CaseAdjusterLookup>>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCodes::LookupParticipentProvider", ex);
                return new BLObject<List<CaseAdjusterLookup>>(null, ex.Message);
            }
        }
        public BLObject<List<TreatmentModel>> LookupPQRSTreatmentType()
        {
            try
            {
                var ds = new DALCodes().LookupPQRSTreatmentType();
                return new BLObject<List<TreatmentModel>>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCodes::LookupPQRSTreatmentType", ex);
                return new BLObject<List<TreatmentModel>>(null, ex.Message);
            }
        }
        public BLObject<List<TreatmentModel>> LookupPQRSReasonType()
        {
            try
            {
                var ds = new DALCodes().LookupPQRSReasonType();
                return new BLObject<List<TreatmentModel>>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCodes::LookupPQRSReasonType", ex);
                return new BLObject<List<TreatmentModel>>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the states.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodes> LookupStates()
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALCodes().LookupStates();

                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCodes::LookupStates", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        public BLObject<DSCodes> LookupServiceType(string IsActive)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALCodes().LookupServiceType(IsActive);

                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCodes::LookupServiceType", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }
        
        /// <summary>
        /// Lookups the revenue code.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodeLookup> LookupRevenueCode()
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALRevenueCode().LookupRevenueCode();

                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCodes::LookupRevenueCode", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the CPT code.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodeLookup> LookupCPTCode(string EntityId, string CPTCode, int IsEqule = 0)
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALCPTCode().LookupCPTCode(EntityId, CPTCode, IsEqule);
                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LookupCPTCode", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }
        public BLObject<DSCodeLookup> ValidateCPTCode(string CPTCode)
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALCPTCode().ValidateCPTCode(CPTCode);
                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::ValidateCPTCode", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the ICD code.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodeLookup> LookupICDCode(string EntityId,string ICDCode, int IsEqule = 0)
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALICD().LookupICDCode(EntityId,ICDCode, IsEqule);
                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LookupICDCode", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }
        

        /// <summary>
        /// Lookups the modifier.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodeLookup> LookupModifier(string ModifierCode, string IsActive, int IsEqule = 0 )
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALModifier().LookupModifier(ModifierCode, IsEqule, IsActive);
                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LookupModifier", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the modifier.
        /// </summary>
        /// <returns></returns>
        public List<TestTypeModel> LookupTestType(string IsActive)
        {
            try
            {
                 return new DALTestType().LookupTestType(IsActive);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LookupTestType", ex);
                throw ex;
            }
        }
        #endregion

        #region Modifier
        /// <summary>
        /// Loads the modifier.
        /// </summary>
        /// <param name="ModifierId">The modifier identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public BLObject<DSCodes> LoadModifier(long ModifierId, string ModifierCode, string Description, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALModifier().LoadModifier(ModifierId, ModifierCode, Description,IsActive, PageNumber,RowsPerPage);

                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadModifier", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the modifier.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> UpdateModifier(ref DSCodes ds)
        {
            try
            {
                ds = new DALModifier().UpdateModifier(ref ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdateModifier", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the modifier.
        /// </summary>
        /// <param name="ModifierIds">The modifier ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteModifier(string ModifierIds)
        {
            try
            {
                ModifierIds = new DALModifier().DeleteModifier(ModifierIds);
                return new BLObject<string>(ModifierIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::DeleteModifier", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the modifier.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> InsertModifier(ref DSCodes ds)
        {
            try
            {
                ds = new DALModifier().InsertModifier(ref ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertModifier", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }
        #endregion

        #region DrugCodeCost
        public BLObject<DSCodes> LoadDrugCodeCostAll(long CPTCodeCostID, string CPTCode, string Cost , string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALDrugCodeCost().LoadDrugCodeCostAll(CPTCodeCostID, CPTCode, Cost, Active, PageNumber, RowsPerPage);

                return new BLObject<DSCodes>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadDrugCodeCost", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        public BLObject<DSCodes> GeDrugCodeCost()
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALDrugCodeCost().GetDrugCodeCost();

                return new BLObject<DSCodes>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdmin::GetRemindersType", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }

        }
        public BLObject<string> DeleteDrugCodeCost(string DrugCodeCostIds)
        {
            try
            {

                DrugCodeCostIds = new DALDrugCodeCost().DeleteDrugCodeCost(DrugCodeCostIds);

                return new BLObject<string>(DrugCodeCostIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteDrugCodeCost", ex);
                return new BLObject<string>("", ex.Message);
            }

        }
        public BLObject<DSCodes> InsertDrugCodeCost(ref DSCodes ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            string CPTCodeCostID = "";
            try
            {
                ds = new DALDrugCodeCost().InsertDrugCodeCost(ref ds);
                if (ds.CPTCodeCost.Rows.Count > 0)
                    CPTCodeCostID = ds.CPTCodeCost.Rows[0][ds.CPTCodeCost.CPTCodeCostIDColumn].ToString();

                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DRUG_CODE_COST, true, "Drug Code Cost saved", CPTCodeCostID);

                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during Drug Code Cost save : " + ex.ToString());
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertDrugCodeCost", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }
        public BLObject<DSCodes> LoadDrugCodeCost(long CPTCodeCostID, string CTPCode,string Cost, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALDrugCodeCost().LoadDrugCodeCost(CPTCodeCostID, CTPCode, Cost, Active, PageNumber, RowsPerPage);

                return new BLObject<DSCodes>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadDrugCodeCost", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }
        
        public BLObject<DSCodes> UpdateDrugCodeCost(ref DSCodes ds)
        {
            DALUsersActivity obj = new DALUsersActivity();
            string CPTCodeCostID = "";
            try
            {
                ds = new DALDrugCodeCost().UpdateDrugCodeCost(ref ds);
                if (ds.CPTCodeCost.Rows.Count > 0)
                    CPTCodeCostID = ds.CPTCodeCost.Rows[0][ds.CPTCodeCost.CPTCodeCostIDColumn].ToString();

                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_DRUG_CODE_COST, true, "Drug Code Cost updated", CPTCodeCostID);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Update, DALUsersActivity.TITLE_DEMOGRAPHIC, false, "Error during Drug Code Cost update : " + ex.ToString());
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdateDrugCodeCost", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        #endregion
        #region ICD
        /// <summary>
        /// Loads the icd.
        /// </summary>
        /// <param name="ICDId">The icd identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="ICD9">The ic d9.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public BLObject<DSCodes> LoadICD(long ICDId, string ShortName, string Description, string ICD9, string ICD10Description, string ICD10, string SNOMEDDescription, string SNOMED, string IsActive, Int64 PageNumber, Int64 RowsPerPage)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALICD().LoadICD(ICDId, ShortName, Description, ICD9,ICD10Description,ICD10,SNOMEDDescription,SNOMED, IsActive,  PageNumber, RowsPerPage);

                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadICD", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the icd.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> UpdateICD(ref DSCodes ds)
        {
            try
            {
                ds = new DALICD().UpdateICD(ref ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdateICD", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the icd.
        /// </summary>
        /// <param name="ICDIds">The icd ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteICD(string ICDIds)
        {
            try
            {
                ICDIds = new DALICD().DeleteICD(ICDIds);
                return new BLObject<string>(ICDIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::DeleteICD", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the icd.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> InsertICD(ref DSCodes ds)
        {
            try
            {
                ds = new DALICD().InsertICD(ref ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertICD", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }
        #endregion

        #region ICD LookUp

        /// <summary>
        /// InsertICDLookup
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSCodeLookup> InsertICDLookup(ref DSCodeLookup ds)
        {
            try
            {
                ds = new DALICD().InsertICDLookup(ref ds);
                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertICDLookup", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }
        #endregion

        #region "Type Of Service"
        /// <summary>
        /// Loads the type of service.
        /// </summary>
        /// <param name="TOSId">The tos identifier.</param>
        /// <param name="Code">The code.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public BLObject<DSCodes> LoadTypeOfService(long TOSId, string Code, string Name, string Description, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALTypeOfService().LoadTypeOfService(TOSId, Code, Name, Description,IsActive, PageNumber,RowsPerPage);

                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadTypeOfService", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the type of service.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> UpdateTypeOfService(ref DSCodes ds)
        {
            try
            {
                ds = new DALTypeOfService().UpdateTypeOfService(ref ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdateTypeOfService", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the type of service.
        /// </summary>
        /// <param name="TOSIds">The tos ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteTypeOfService(string TOSIds)
        {
            try
            {
                TOSIds = new DALTypeOfService().DeleteTypeOfService(TOSIds);
                return new BLObject<string>(TOSIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::DeleteTypeOfService", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the type of service.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> InsertTypeOfService(ref DSCodes ds)
        {
            try
            {
                ds = new DALTypeOfService().InsertTypeOfService(ref ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertTypeOfService", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        #region "Plan Specific Info"
        /// <summary>
        /// Loads the tos plan information.
        /// </summary>
        /// <param name="TOSId">The tos identifier.</param>
        /// <returns></returns>
        public BLObject<DSCodes> LoadTOSPlanInfo(long TOSId, long TOSPlanId)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALTypeOfService().LoadTOSPlan(TOSId, TOSPlanId);

                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadTOSPlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the tos plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> UpdateTOSPlanInfo(ref DSCodes ds)
        {
            try
            {
                ds = new DALTypeOfService().UpdateTOSPlan(ref ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdateTOSPlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the tos plan information.
        /// </summary>
        /// <param name="TOSPlanIds">The tos plan ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteTOSPlanInfo(string TOSPlanIds)
        {
            try
            {
                TOSPlanIds = new DALTypeOfService().DeleteTOSPlan(TOSPlanIds);
                return new BLObject<string>(TOSPlanIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::DeleteTOSPlanInfo", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the tos plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> InsertTOSPlanInfo(ref DSCodes ds)
        {
            try
            {
                ds = new DALTypeOfService().InsertTOSPlan(ref ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertTOSPlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }
        #endregion
        #endregion

        #region "ProcedureCategory"
        public BLObject<DSCodes> LoadProcedureCategory(long ProcCategoryId, string Name, string Description, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALProcedureCategory().LoadProcedureCategory(ProcCategoryId, Name, Description,IsActive, PageNumber,RowsPerPage);

                return new BLObject<DSCodes>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadProcedureCategory", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }

        }

        /// <summary>
        /// Inserts the ProcedureCategory.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> InsertProcedureCategory(ref DSCodes ds)
        {
            try
            {

                ds = new DALProcedureCategory().InsertProcedureCategory(ref ds);

                return new BLObject<DSCodes>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertProcedureCategory", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }

        }

        /// <summary>
        /// Updates the ProcedureCategory.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// 
        public BLObject<DSCodes> UpdateProcedureCategory(ref DSCodes ds)
        {
            try
            {

                ds = new DALProcedureCategory().UpdateProcedureCategory(ref ds);

                return new BLObject<DSCodes>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdateProcedureCategory", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }

        }
        /// <summary>
        /// Deletes the ProcedureCategory.
        /// </summary>
        /// <param name="UserIds">The user ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteProcedureCategory(string UserIds)
        {
            try
            {

                UserIds = new DALProcedureCategory().DeleteProcedureCategory(UserIds);

                return new BLObject<string>(UserIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::DeleteProcedureCategory", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        //<summary>
        //Lookups the Procedure Category.
        //</summary>
        //<returns></returns>
        public BLObject<DSCodeLookup> LookupProcedureCategory(string Active)
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALProcedureCategory().LookupProcedureCategory(Active);

                return new BLObject<DSCodeLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LookupProcedureCategory", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }

        }
        public BLObject<DSiTrack> LookupImprovementActivity(string Active)
        {
            try
            {
                DSiTrack ds = new DSiTrack();
                ds = new DALiTrack().LookupImprovementActivity(Active);

                return new BLObject<DSiTrack>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LookupProcedureCategory", ex);
                return new BLObject<DSiTrack>(null, ex.Message);
            }

        }
        #endregion

        #region "CPTCodes"
        /// <summary>
        /// Loads the CPT code.
        /// </summary>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="CPT">The CPT.</param>
        /// <param name="Description">The description.</param>
        /// <param name="TOSId">The tos identifier.</param>
        /// <param name="SpecialtyId">The specialty identifier.</param>
        /// <param name="Discontinued">The discontinued.</param>
        /// <returns></returns>
        public BLObject<DSCodes> LoadCPTCode(long CPTCodeId, string ShortName, string CPT, string Description, string TOSId, string SpecialtyId, string IsActive, string Discontinued, string EntityId, Int64 PageNumber, Int64 RowsPerPage)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALCPTCode().LoadCPTCode(CPTCodeId, ShortName, CPT, Description, TOSId, SpecialtyId, IsActive, Discontinued, EntityId, PageNumber, RowsPerPage);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadCPTCode", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the CPTCode.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>

        public BLObject<DSCodes> LoadHPCSCode(string CPTCode)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALCPTCode().LoadHPCSCode(CPTCode);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadHPCSCode", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }
        public BLObject<DSCodes> InsertCPTCode(ref DSCodes ds)
        {
            try
            {
                ds = new DALCPTCode().InsertCPTCode(ref ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertCPTCode", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the CPTCode.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// 
        public BLObject<DSCodes> UpdateCPTCode(ref DSCodes ds)
        {
            try
            {
                ds = new DALCPTCode().UpdateCPTCode(ref ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdateCPTCode", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the CPTCode.
        /// </summary>
        /// <param name="UserIds">The user ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteCPTCode(string CPTCodeId)
        {
            try
            {
                CPTCodeId = new DALCPTCode().DeleteCPTCode(CPTCodeId);
                return new BLObject<string>(CPTCodeId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::DeleteCPTCode", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #region "CPT Plan Info"
        /// <summary>
        /// Loads the CPT plan information.
        /// </summary>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <returns></returns>
        public BLObject<DSCodes> LoadCPTPlanInfo(long CPTCodeId, long CPTPlanId)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALCPTCode().LoadCPTPlanInfo(CPTCodeId, CPTPlanId);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadCPTPlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the CPT plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> UpdateCPTPlanInfo(DSCodes ds)
        {
            try
            {
                ds = new DALCPTCode().UpdateCPTPlanInfo(ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateCPTPlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the CPT plan information.
        /// </summary>
        /// <param name="CPTPlanId">The CPT plan identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeleteCPTPlanInfo(string CPTPlanId)
        {
            try
            {
                CPTPlanId = new DALCPTCode().DeleteCPTPlanInfo(CPTPlanId);
                return new BLObject<string>(CPTPlanId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteCPTPlanInfo", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the CPT plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> InsertCPTPlanInfo(DSCodes ds)
        {
            try
            {
                ds = new DALCPTCode().InsertCPTPlanInfo(ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertCPTPlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }
        #endregion
        #endregion

        #region "RevenueCode"
        /// <summary>
        /// Loads the revenue code.
        /// </summary>
        /// <param name="RevenueCodeId">The revenue code identifier.</param>
        /// <param name="RevenueCode">The revenue code.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public BLObject<DSCodes> LoadRevenueCode(long RevenueCodeId, string RevenueCode, string Description, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALRevenueCode().LoadRevenueCode(RevenueCodeId, RevenueCode, Description, EntityId,IsActive, PageNumber,RowsPerPage);

                return new BLObject<DSCodes>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadRevenueCode", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }

        }

        /// <summary>
        /// Inserts the RevenueCode.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> InsertRevenueCode(ref DSCodes ds)
        {
            try
            {

                ds = new DALRevenueCode().InsertRevenueCode(ref ds);

                return new BLObject<DSCodes>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertRevenueCode", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }

        }

        public BLObject<DSClinicalSummary> InsertOccupationStatus(ref DSClinicalSummary ds)
        {
            try
            {
                ds = new DALOccupationStatus().InsertOccupationStatus(ref ds);
                return new BLObject<DSClinicalSummary>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertOccupationStatus", ex);
                return new BLObject<DSClinicalSummary>(null, ex.Message);
            }
        }

        public BLObject<DSClinicalSummary> LoadOccupationStatus(long StatusId, string ConceptCode, string Description, string IsOccupation, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSClinicalSummary ds = new DSClinicalSummary();
                ds = new DALOccupationStatus().LoadOccupationStatus(StatusId, ConceptCode, Description, IsOccupation, PageNumber, RowsPerPage);
                return new BLObject<DSClinicalSummary>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadOccupationStatus", ex);
                return new BLObject<DSClinicalSummary>(null, ex.Message);
            }
        }

        public BLObject<string> DeleteOccupationStatus(string StatusId)
        {
            try
            {
                StatusId = new DALOccupationStatus().DeleteOccupationStatus(StatusId);
                return new BLObject<string>(StatusId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::DeleteOccupationStatus", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Updates the RevenueCode.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// 
        public BLObject<DSCodes> UpdateRevenueCode(ref DSCodes ds)
        {
            try
            {

                ds = new DALRevenueCode().UpdateRevenueCode(ref ds);

                return new BLObject<DSCodes>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdateRevenueCode", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }

        }
        /// <summary>
        /// Deletes the RevenueCode.
        /// </summary>
        /// <param name="UserIds">The user ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteRevenueCode(string UserIds)
        {
            try
            {

                UserIds = new DALRevenueCode().DeleteRevenueCode(UserIds);

                return new BLObject<string>(UserIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::DeleteRevenueCode", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        #region Revenue Code Plan Info
        /// <summary>
        /// Loads the revenue code plan information.
        /// </summary>
        /// <param name="CPTCodeId">The CPT code identifier.</param>
        /// <returns></returns>
        public BLObject<DSCodes> LoadRevenueCodePlanInfo(long RevenueCodeId, long RevenueCodePlanId)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALRevenueCode().LoadRevenueCodePlan(RevenueCodeId, RevenueCodePlanId);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadRevenueCodePlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the revenue code plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> UpdateRevenueCodePlanInfo(DSCodes ds)
        {
            try
            {
                ds = new DALRevenueCode().UpdateRevenueCodePlan(ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdateRevenueCodePlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the revenue code plan information.
        /// </summary>
        /// <param name="RevenueCodePlanId">The revenue code plan identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeleteRevenueCodePlanInfo(string RevenueCodePlanId)
        {
            try
            {
                RevenueCodePlanId = new DALRevenueCode().DeleteRevenueCodePlan(RevenueCodePlanId);
                return new BLObject<string>(RevenueCodePlanId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::DeleteRevenueCodePlanInfo", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the revenue code plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> InsertRevenueCodePlanInfo(DSCodes ds)
        {
            try
            {
                ds = new DALRevenueCode().InsertRevenueCodePlan(ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertRevenueCodePlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }
        #endregion
        #endregion

        #region Place Of Service
        /// <summary>
        /// Loads the place of service.
        /// </summary>
        /// <param name="POSId">The position identifier.</param>
        /// <param name="Code">The code.</param>
        /// <returns></returns>
        public BLObject<DSCodes> LoadPlaceOfService(long POSId, string Code,string description,string IsAtive, int PageNumber = 1, int RowsPerPage = 15)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALPlaceOfService().LoadPlaceOfService(POSId, Code, description, IsAtive, PageNumber, RowsPerPage);

                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadPlaceOfService", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the place of service.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> UpdatePlaceOfService(DSCodes ds)
        {
            try
            {
                ds = new DALPlaceOfService().UpdatePlaceOfService(ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdatePlaceOfService", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the place of service.
        /// </summary>
        /// <param name="POSIds">The position ids.</param>
        /// <returns></returns>
        public BLObject<string> DeletePlaceOfService(string POSIds)
        {
            try
            {
                POSIds = new DALPlaceOfService().DeletePlaceOfService(POSIds);
                return new BLObject<string>(POSIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::DeletePlaceOfService", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the place of service.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> InsertPlaceOfService(DSCodes ds)
        {
            try
            {
                ds = new DALPlaceOfService().InsertPlaceOfService(ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertPlaceOfService", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        #region "Plan Specific Info"
        /// <summary>
        /// Loads the position plan information.
        /// </summary>
        /// <param name="POSId">The position identifier.</param>
        /// <returns></returns>
        public BLObject<DSCodes> LoadPOSPlanInfo(long POSId, long POSPlanId)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALPlaceOfService().LoadPOSPlan(POSId, POSPlanId);

                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadPOSPlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the position plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> UpdatePOSPlanInfo(DSCodes ds)
        {
            try
            {
                ds = new DALPlaceOfService().UpdatePOSPlan(ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdatePOSPlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the position plan information.
        /// </summary>
        /// <param name="POSPlanIds">The position plan ids.</param>
        /// <returns></returns>
        public BLObject<string> DeletePOSPlanInfo(string POSPlanIds)
        {
            try
            {
                POSPlanIds = new DALPlaceOfService().DeletePOSPlan(POSPlanIds);
                return new BLObject<string>(POSPlanIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::DeletePOSPlanInfo", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the position plan information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSCodes> InsertPOSPlanInfo(DSCodes ds)
        {
            try
            {
                ds = new DALPlaceOfService().InsertPOSPlan(ds);
                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::InsertPOSPlanInfo", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }
        #endregion
        #endregion
        #endregion

        #region "SCTProcedures"

        public BLObject<DSCodes> LoadSCTProcedures(string SCTProcedures)
        {
            try
            {
                DSCodes ds = new DSCodes();
                ds = new DALCPTCode().LoadSCTProcedures(SCTProcedures);

                return new BLObject<DSCodes>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadSCTProcedures", ex);
                return new BLObject<DSCodes>(null, ex.Message);
            }
        }

        #endregion
        #region NDC
        public string SaveNDCInfo(CPTNdcModel model)
        {
            try
            {
                return new DALCPTCode().SaveNDCInfo(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::SaveNDCInfo", ex);
                throw ex;
            }
            finally
            {

            }
        }
        public string UpdateNDCInfo(CPTNdcModel model)
        {
            try
            {
                return new DALCPTCode().UpdateNDCInfo(model);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::UpdateNDCInfo", ex);
                throw ex;
            }
            finally
            {

            }
        }
        
        public BLObject<List<CPTNdcModel>> LoadCptNdc(Int64 CPTCodeId=0,string CptCode="",Int64 CPTNdcID=0,string NdcCode="")
        {
            try
            {
                List<CPTNdcModel> NdcList = new List<CPTNdcModel>();
                NdcList = new DALCPTCode().LoadCptNdc(CPTCodeId,CptCode, CPTNdcID, NdcCode);
                return new BLObject<List<CPTNdcModel>>(NdcList);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminCodes::LoadCptNdc", ex);
                return new BLObject<List<CPTNdcModel>>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteCPTNdcInfo(string CPTNdcId)
        {
            try
            {
                CPTNdcId = new DALCPTCode().DeleteCPTNdcInfo(CPTNdcId);
                return new BLObject<string>(CPTNdcId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteCPTNdcInfo", ex);
                return new BLObject<string>("", ex.Message);
            }
        }
        #endregion
    }
}
