using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.DataAccess.DAL.Admin;
using MDVision.Datasets;
using System.Configuration;
using MDVision.Common.Logging;

namespace MDVision.Business.BLL
{
    public class BLLAdminPatientEligibilityService
    {
        //#region Variable

        //#endregion

        //#region Constructors
        ///// <summary>
        ///// Initializes a new instance of the <see cref="BLLAdminEDI"/> class.
        ///// </summary>
        //public BLLAdminEDI()
        //{
        //    //SharedVariable 
        //    //This call is required by the Web Services Designer.
        //    InitializeComponent();
        //    // this. = ;
        //    //Add your own initialization code after the InitializeComponent() call

        //}

        //private IContainer components;
        ////NOTE: The following procedure is required by the Web Services Designer
        ////It can be modified using the Web Services Designer.  
        ////Do not modify it using the code editor.
        //[System.Diagnostics.DebuggerStepThrough()]
        //private void InitializeComponent()
        //{
        //    components = new System.ComponentModel.Container();
        //}
        //#endregion

        ////#region EDI Submit Insurance
        ///// <summary>
        ///// Loads the edi submit insurance.
        ///// </summary>
        ///// <param name="EDISubmitId">The edi submit identifier.</param>
        ///// <param name="CHouseId">The c house identifier.</param>
        ///// <param name="SubmitInsName">Name of the submit ins.</param>
        ///// <param name="PayorId">The payor identifier.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> LoadEDISubmitInsurance(long EDISubmitId, string CHouseId, string SubmitInsName, string PayorId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        //{
        //    try
        //    {
        //        DSPatientEligibilityService ds = new DSPatientEligibilityService();
        //        ds = new DALPatientEligibilityService().LoadPatientEligibilityService(EDISubmitId, CHouseId, SubmitInsName, PayorId, IsActive, PageNumber, RowsPerPage);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LoadEDISubmitInsurance", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Updates the edi submit insurance.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> UpdateEDISubmitInsurance(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALPatientEligibilityService().UpdateEDISubmitInsurance(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::UpdateEDISubmitInsurance", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Deletes the edi submit insurance.
        ///// </summary>
        ///// <param name="SpecialtyIds">The specialty ids.</param>
        ///// <returns></returns>
        //public BLObject<string> DeleteEDISubmitInsurance(string SpecialtyIds)
        //{
        //    try
        //    {
        //        SpecialtyIds = new DALPatientEligibilityService().DeleteEDISubmitInsurance(SpecialtyIds);
        //        return new BLObject<string>(SpecialtyIds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::DeleteEDISubmitInsurance", ex);
        //        return new BLObject<string>("", ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Inserts the edi submit insurance.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> InsertEDISubmitInsurance(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALPatientEligibilityService().InsertEDISubmitInsurance(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::InsertEDISubmitInsurance", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}
        //#endregion

        //#region EDI Claim Status Insurance
        ///// <summary>
        ///// Loads the edi claim status insurance.
        ///// </summary>
        ///// <param name="EDIStatusId">The edi status identifier.</param>
        ///// <param name="CHouseId">The c house identifier.</param>
        ///// <param name="EDIStatusInsName">Name of the edi status ins.</param>
        ///// <param name="PayorId">The payor identifier.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> LoadEDIClaimStatusInsurance(long EDIStatusId, string CHouseId, string EDIStatusInsName, string PayorId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        //{
        //    try
        //    {
        //        DSPatientEligibilityService ds = new DSPatientEligibilityService();
        //        ds = new DALEDIClaimStatusInsurance().LoadEDIClaimStatusInsurance(EDIStatusId, CHouseId, EDIStatusInsName, PayorId, IsActive, PageNumber, RowsPerPage);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LoadEDIClaimStatusInsurance", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Updates the edi claim status insurance.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> UpdateEDIClaimStatusInsurance(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALEDIClaimStatusInsurance().UpdateEDIClaimStatusInsurance(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::UpdateEDIClaimStatusInsurance", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Deletes the edi claim status insurance.
        ///// </summary>
        ///// <param name="EDIStatusIds">The edi status ids.</param>
        ///// <returns></returns>
        //public BLObject<string> DeleteEDIClaimStatusInsurance(string EDIStatusIds)
        //{
        //    try
        //    {
        //        EDIStatusIds = new DALEDIClaimStatusInsurance().DeleteEDIClaimStatusInsurance(EDIStatusIds);
        //        return new BLObject<string>(EDIStatusIds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::DeleteEDIClaimStatusInsurance", ex);
        //        return new BLObject<string>("", ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Inserts the edi claim status insurance.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> InsertEDIClaimStatusInsurance(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALEDIClaimStatusInsurance().InsertEDIClaimStatusInsurance(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::InsertEDIClaimStatusInsurance", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}
        //#endregion

        //#region EDI Eligibility Insurance
        ///// <summary>
        ///// Loads the edi eligibility insurance.
        ///// </summary>
        ///// <param name="EDIEligibilityId">The edi eligibility identifier.</param>
        ///// <param name="CHouseId">The c house identifier.</param>
        ///// <param name="EDIStatusInsName">Name of the edi status ins.</param>
        ///// <param name="PayorId">The payor identifier.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> LoadEDIEligibilityInsurance(long EDIEligibilityId, string CHouseId, string EDIStatusInsName, string PayorId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        //{
        //    try
        //    {
        //        DSPatientEligibilityService ds = new DSPatientEligibilityService();
        //        ds = new DALEDIEligibilityInsurance().LoadEDIEligibilityInsurance(EDIEligibilityId, CHouseId, EDIStatusInsName, PayorId,IsActive, PageNumber, RowsPerPage);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LoadEDIEligibilityInsurance", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Updates the edi eligibility insurance.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> UpdateEDIEligibilityInsurance(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALEDIEligibilityInsurance().UpdateEDIEligibilityInsurance(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::UpdateEDIEligibilityInsurance", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Deletes the edi eligibility insurance.
        ///// </summary>
        ///// <param name="EDIStatusIds">The edi status ids.</param>
        ///// <returns></returns>
        //public BLObject<string> DeleteEDIEligibilityInsurance(string EDIStatusIds)
        //{
        //    try
        //    {
        //        EDIStatusIds = new DALEDIEligibilityInsurance().DeleteEDIEligibilityInsurance(EDIStatusIds);
        //        return new BLObject<string>(EDIStatusIds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::DeleteEDIEligibilityInsurance", ex);
        //        return new BLObject<string>("", ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Inserts the edi eligibility insurance.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> InsertEDIEligibilityInsurance(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALEDIEligibilityInsurance().InsertEDIEligibilityInsurance(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::InsertEDIEligibilityInsurance", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}
        //#endregion

        //#region Clearing House
        ///// <summary>
        ///// Loads the clearing house.
        ///// </summary>
        ///// <param name="ClearingHouseId">The clearing house identifier.</param>
        ///// <param name="ShortName">The short name.</param>
        ///// <param name="Type">The type.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> LoadClearingHouse(long ClearingHouseId, string ShortName, string Type, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        //{
        //    try
        //    {
        //        DSPatientEligibilityService ds = new DSPatientEligibilityService();
        //        ds = new DALClearingHouse().LoadClearingHouse(ClearingHouseId, ShortName, Type, EntityId, IsActive,PageNumber, RowsPerPage);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LoadClearingHouse", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Updates the clearing house.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> UpdateClearingHouse(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALClearingHouse().UpdateClearingHouse(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::UpdateClearingHouse", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Deletes the clearing house.
        ///// </summary>
        ///// <param name="ClearingHouseIds">The clearing house ids.</param>
        ///// <returns></returns>
        //public BLObject<string> DeleteClearingHouse(string ClearingHouseIds)
        //{
        //    try
        //    {
        //        ClearingHouseIds = new DALClearingHouse().DeleteClearingHouse(ClearingHouseIds);
        //        return new BLObject<string>(ClearingHouseIds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::DeleteClearingHouse", ex);
        //        return new BLObject<string>("", ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Inserts the clearing house.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> InsertClearingHouse(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALClearingHouse().InsertClearingHouse(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::InsertClearingHouse", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}
        //#endregion

        //#region EDITaxIDSetup
        ///// <summary>
        ///// Loads the edi tax identifier setup.
        ///// </summary>
        ///// <param name="EDITaxIDSetupId">The edi tax identifier setup identifier.</param>
        ///// <param name="TaxID">The tax identifier.</param>
        ///// <param name="Clearinghouse">The clearinghouse.</param>
        ///// <param name="EntityId">The entity identifier.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> LoadEDITaxIDSetup(long EDITaxIDSetupId, string TaxID, string Clearinghouse, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        //{
        //    try
        //    {
        //        DSPatientEligibilityService ds = new DSPatientEligibilityService();
        //        ds = new DALEDITaxIDSetup().LoadEDITaxIDSetup(EDITaxIDSetupId, TaxID, Clearinghouse, EntityId,IsActive, PageNumber, RowsPerPage);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LoadEDITaxIDSetup", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Updates the EDITaxIDSetup.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> UpdateEDITaxIDSetup(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALEDITaxIDSetup().UpdateEDITaxIDSetup(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::UpdateEDITaxIDSetup", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Deletes the EDITaxIDSetup.
        ///// </summary>
        ///// <param name="EDITaxIDSetupIds">The EDITaxIDSetup ids.</param>
        ///// <returns></returns>
        //public BLObject<string> DeleteEDITaxIDSetup(string EDITaxIDSetupIds)
        //{
        //    try
        //    {
        //        EDITaxIDSetupIds = new DALEDITaxIDSetup().DeleteEDITaxIDSetup(EDITaxIDSetupIds);
        //        return new BLObject<string>(EDITaxIDSetupIds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::DeleteEDITaxIDSetup", ex);
        //        return new BLObject<string>("", ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Inserts the EDITaxIDSetup.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> InsertEDITaxIDSetup(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALEDITaxIDSetup().InsertEDITaxIDSetup(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::InsertEDITaxIDSetup", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}
        //#endregion

        //#region "Submitter Setup"
        ///// <summary>
        ///// Loads the submitter setup.
        ///// </summary>
        ///// <param name="SubmitterSetupId">The submitter setup identifier.</param>
        ///// <param name="OrganizationLastName">Last name of the organization.</param>
        ///// <param name="SubmitterAddress1">The submitter address1.</param>
        ///// <param name="SubmitterAddress2">The submitter address2.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> LoadSubmitterSetup(long SubmitterSetupId, string OrganizationLastName, string ShortName, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        //{
        //    try
        //    {
        //        DSPatientEligibilityService ds = new DSPatientEligibilityService();
        //        ds = new DALSubmitterSetup().LoadSubmitterSetup(SubmitterSetupId, OrganizationLastName, ShortName, EntityId, IsActive, PageNumber, RowsPerPage);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLFeeShedule::LoadSubmitterSetup", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Inserts the submitter setup.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> InsertSubmitterSetup(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALSubmitterSetup().InsertSubmitterSetup(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLFeeShedule::InsertSubmitterSetup", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Updates the submitter setup.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> UpdateSubmitterSetup(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALSubmitterSetup().UpdateSubmitterSetup(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLFeeShedule::UpdateSubmitterSetup", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Deletes the submitter setup.
        ///// </summary>
        ///// <param name="SubmitterSetupId">The submitter setup identifier.</param>
        ///// <returns></returns>
        //public BLObject<string> DeleteSubmitterSetup(string SubmitterSetupId)
        //{
        //    try
        //    {
        //        SubmitterSetupId = new DALSubmitterSetup().DeleteSubmitterSetup(SubmitterSetupId);
        //        return new BLObject<string>(SubmitterSetupId);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLFeeShedule::DeleteSubmitterSetup", ex);
        //        return new BLObject<string>(null, ex.Message);
        //    }
        //}
        //#endregion

        //#region "Receiver Setup"
        ///// <summary>
        ///// Loads the receiver setup.
        ///// </summary>
        ///// <param name="EDIReceiverSetupId">The edi receiver setup identifier.</param>
        ///// <param name="ReceiverCode">The receiver code.</param>
        ///// <param name="ShortName">The short name.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> LoadReceiverSetup(long EDIReceiverSetupId, string SubmitterID, string ShortName, string ClearingHouseId, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        //{
        //    try
        //    {
        //        DSPatientEligibilityService ds = new DSPatientEligibilityService();
        //        ds = new DALEDIReceiverSetup().LoadEDIReceiverSetup(EDIReceiverSetupId, SubmitterID, ShortName, ClearingHouseId, EntityId,IsActive, PageNumber, RowsPerPage);
        //        ds.Merge(new DALEDIReceiverX12Setup().LoadReceiverSetupX12(EDIReceiverSetupId));
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LoadReceiverSetup", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Inserts the receiver setup.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> InsertReceiverSetup(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALEDIReceiverSetup().InsertEDIReceiverSetup(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::InsertReceiverSetup", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Updates the receiver setup.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> UpdateReceiverSetup(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALEDIReceiverSetup().UpdateEDIReceiverSetup(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::UpdateReceiverSetup", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Deletes the receiver setup.
        ///// </summary>
        ///// <param name="ReceiverSetupId">The receiver setup identifier.</param>
        ///// <returns></returns>
        //public BLObject<string> DeleteReceiverSetup(string ReceiverSetupId)
        //{
        //    try
        //    {
        //        ReceiverSetupId = new DALEDIReceiverSetup().DeleteEDIReceiverSetup(ReceiverSetupId);
        //        return new BLObject<string>(ReceiverSetupId);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::DeleteReceiverSetup", ex);
        //        return new BLObject<string>(null, ex.Message);
        //    }
        //}
        //#endregion

        //#region "Receiver X12 Setup"
        ///// <summary>
        ///// Loads the receiver setup X12.
        ///// </summary>
        ///// <param name="ReceiverSetupId">The receiver setup identifier.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> LoadReceiverSetupX12(long ReceiverSetupId)
        //{
        //    try
        //    {
        //        DSPatientEligibilityService ds = new DSPatientEligibilityService();
        //        ds = new DALEDIReceiverX12Setup().LoadReceiverSetupX12(ReceiverSetupId);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LoadReceiverSetupX12", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Inserts the receiver setup X12.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> InsertReceiverSetupX12(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALEDIReceiverX12Setup().InsertReceiverSetupX12(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::InsertReceiverSetupX12", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Updates the receiver setup X12.
        ///// </summary>
        ///// <param name="ds">The ds.</param>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityService> UpdateReceiverSetupX12(DSPatientEligibilityService ds)
        //{
        //    try
        //    {
        //        ds = new DALEDIReceiverX12Setup().UpdateReceiverSetupX12(ds);
        //        return new BLObject<DSPatientEligibilityService>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::UpdateReceiverSetupX12", ex);
        //        return new BLObject<DSPatientEligibilityService>(null, ex.Message);
        //    }
        //}
        //#endregion

        //#region EDI Service
        /// <summary>
        /// LoadPatientEligibilityService
        /// </summary>
        /// <param name="eDIServiceHandleId"></param>
        /// <param name="entityId"></param>
        /// <param name="clearingHouseId"></param>
        /// <param name="Case"></param>
        /// <param name="mode"></param>
        /// <param name="time"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public BLObject<DSPatientEligibilityService> LoadPatientEligibilityService(long eDIServiceHandleId, string entityId, string clearingHouseId, string scheduleDays, string mode, string time, string isActive, int PageNumber = 1, int RowsPerPage = 15)
        {
            try
            {
                DSPatientEligibilityService ds = new DSPatientEligibilityService();
                ds = new DALPatientEligibilityService().LoadPatientEligibilityService(eDIServiceHandleId, entityId, clearingHouseId, scheduleDays, mode, time, isActive, PageNumber, RowsPerPage);
                return new BLObject<DSPatientEligibilityService>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminPatientEligibilityService::LoadPatientEligibilityService", ex);
                return new BLObject<DSPatientEligibilityService>(null, ex.Message);
            }
        }
        public BLObject<DSPatientEligibilityService> LoadPatientEligibilityService(SharedVariable SharedVariable, long eDIServiceHandleId, string entityId, string clearingHouseId, string scheduleDays, string mode, string time, string isActive, int PageNumber = 1, int RowsPerPage = 15)
        {
            try
            {
                DSPatientEligibilityService ds = new DSPatientEligibilityService();
                ds = new DALPatientEligibilityService(SharedVariable).LoadPatientEligibilityService(SharedVariable, eDIServiceHandleId, entityId, clearingHouseId, scheduleDays, mode, time, isActive, PageNumber, RowsPerPage);
                return new BLObject<DSPatientEligibilityService>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdminPatientEligibilityService::LoadPatientEligibilityService", ex);
                return new BLObject<DSPatientEligibilityService>(null, ex.Message);
            }
        }

        /// <summary>
        /// UpdatePatientEligibilityService
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSPatientEligibilityService> UpdatePatientEligibilityService(DSPatientEligibilityService ds)
        {
            try
            {
                ds = new DALPatientEligibilityService().UpdatePatientEligibilityService(ds);
                return new BLObject<DSPatientEligibilityService>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminPatientEligibilityService::UpdatePatientEligibilityService", ex);
                return new BLObject<DSPatientEligibilityService>(null, ex.Message);
            }
        }

        /// <summary>
        /// DeletePatientEligibilityService
        /// </summary>
        /// <param name="PatientEligibilityServiceIds"></param>
        /// <returns></returns>
        public BLObject<string> DeletePatientEligibilityService(string PatientEligibilityServiceIds)
        {
            try
            {
                PatientEligibilityServiceIds = new DALPatientEligibilityService().DeletePatientEligibilityService(PatientEligibilityServiceIds);
                return new BLObject<string>(PatientEligibilityServiceIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminPatientEligibilityService::DeletePatientEligibilityService", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// InsertPatientEligibilityService
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSPatientEligibilityService> InsertPatientEligibilityService(DSPatientEligibilityService ds)
        {
            try
            {
                ds = new DALPatientEligibilityService().InsertPatientEligibilityService(ds);
                return new BLObject<DSPatientEligibilityService>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminPatientEligibilityService::InsertPatientEligibilityService", ex);
                return new BLObject<DSPatientEligibilityService>(null, ex.Message);
            }
        }

        //#endregion

        //#region "Lookups"
        ///// <summary>
        ///// Lookups the edi claim status insurance.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityServiceLookup> LookupEDIClaimStatusInsurance()
        //{
        //    try
        //    {
        //        DSPatientEligibilityServiceLookup ds = new DSPatientEligibilityServiceLookup();
        //        ds = new DALEDIClaimStatusInsurance().LookupEDIClaimStatusInsurance();
        //        return new BLObject<DSPatientEligibilityServiceLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LookupEDIClaimStatusInsurance", ex);
        //        return new BLObject<DSPatientEligibilityServiceLookup>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Lookups the edi submit insurance.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityServiceLookup> LookupEDISubmitInsurance()
        //{
        //    try
        //    {
        //        DSPatientEligibilityServiceLookup ds = new DSPatientEligibilityServiceLookup();
        //        ds = new DALPatientEligibilityService().LookupEDISubmitInsurance();
        //        return new BLObject<DSPatientEligibilityServiceLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LookupEDISubmitInsurance", ex);
        //        return new BLObject<DSPatientEligibilityServiceLookup>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Lookups the edi eligibility insurance.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityServiceLookup> LookupEDIEligibilityInsurance()
        //{
        //    try
        //    {
        //        DSPatientEligibilityServiceLookup ds = new DSPatientEligibilityServiceLookup();
        //        ds = new DALEDIEligibilityInsurance().LookupEDIEligibilityInsurance();
        //        return new BLObject<DSPatientEligibilityServiceLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LookupEDIEligibilityInsurance", ex);
        //        return new BLObject<DSPatientEligibilityServiceLookup>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Lookups the clearing house.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityServiceLookup> LookupClearingHouse()
        //{
        //    try
        //    {
        //        DSPatientEligibilityServiceLookup ds = new DSPatientEligibilityServiceLookup();
        //        ds = new DALClearingHouse().LookupClearingHouse();
        //        return new BLObject<DSPatientEligibilityServiceLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LookupClearingHouse", ex);
        //        return new BLObject<DSPatientEligibilityServiceLookup>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Lookups the type of the clearing house.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityServiceLookup> LookupClearingHouseType()
        //{
        //    try
        //    {
        //        DSPatientEligibilityServiceLookup ds = new DSPatientEligibilityServiceLookup();
        //        ds = new DALClearingHouse().LookupClearingHouseType();
        //        return new BLObject<DSPatientEligibilityServiceLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LookupClearingHouseType", ex);
        //        return new BLObject<DSPatientEligibilityServiceLookup>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Lookups the submitter setup.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityServiceLookup> LookupSubmitterSetup()
        //{
        //    try
        //    {
        //        DSPatientEligibilityServiceLookup ds = new DSPatientEligibilityServiceLookup();
        //        ds = new DALSubmitterSetup().LookupSubmitterSetup();
        //        return new BLObject<DSPatientEligibilityServiceLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LookupSubmitterSetup", ex);
        //        return new BLObject<DSPatientEligibilityServiceLookup>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Lookups the edi receiver setup.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSPatientEligibilityServiceLookup> LookupEDIReceiverSetup()
        //{
        //    try
        //    {
        //        DSPatientEligibilityServiceLookup ds = new DSPatientEligibilityServiceLookup();
        //        ds = new DALEDIReceiverSetup().LookupEDIReceiverSetup();
        //        return new BLObject<DSPatientEligibilityServiceLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.BLLErrorLog("BLLAdminEDI::LookupEDIReceiverSetup", ex);
        //        return new BLObject<DSPatientEligibilityServiceLookup>(null, ex.Message);
        //    }
        //}
        //#endregion
    }
}
