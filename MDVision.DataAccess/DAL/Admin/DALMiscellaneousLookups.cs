using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALMiscellaneousLookups
    {
        //#region Variable
        //
        //#endregion

        //#region " Stored Procedure Names"
        //private const string PROC_CLAIM_FLAG_LOOKUP = "Provider.sp_ClaimFlagLookup";
        //private const string PROC_CLAIIM_TYPE_LOOKUP = "Provider.sp_ClaimTypeLookup";
        //private const string PROC_CLAIM_SCRUBBING_PROFILE_LOOKUP = "Provider.sp_ClaimScrubbingProfileLookup";
        //private const string PROC_PLAN_FEE_LINK_LOOKUP = "Provider.sp_PlanFeeLinkLookup";
        //private const string PROC_PLAN_TYPE_LOOKUP = "Provider.sp_PlanTypeLookup";
        //#endregion

        //#region "Parameters"
        //#endregion

        //#region Constructors
        //public DALMiscellaneousLookups()
        //{
        //    InitializeComponent();
        //    ClientConfiguration.SetClientObject();
        //   
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

        //#region "Insert, delete, update and get using dataset Functions"
        ///// <summary>
        ///// Lookups the claim flag.
        ///// </summary>
        ///// <returns></returns>
        //public DSCodeLookup LookupClaimFlag()
        //{
        //    DSCodeLookup ds = new DSCodeLookup();
        //    IDBManager dbManager =ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_FLAG_LOOKUP, ds, ds.ClaimFlag.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALMiscellaneousLookups::LookupClaimFlag", PROC_CLAIM_FLAG_LOOKUP, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        ///// <summary>
        ///// Lookups the type of the claim.
        ///// </summary>
        ///// <returns></returns>
        //public DSCodeLookup LookupClaimType()
        //{
        //    DSCodeLookup ds = new DSCodeLookup();
        //    IDBManager dbManager =ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIIM_TYPE_LOOKUP, ds, ds.ClaimType.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALMiscellaneousLookups::LookupClaimType", PROC_CLAIIM_TYPE_LOOKUP, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        ///// <summary>
        ///// Lookups the claim scrubbing profile.
        ///// </summary>
        ///// <returns></returns>
        //public DSCodeLookup LookupClaimScrubbingProfile()
        //{
        //    DSCodeLookup ds = new DSCodeLookup();
        //    IDBManager dbManager =ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CLAIM_SCRUBBING_PROFILE_LOOKUP, ds, ds.ClaimScrubbingProfile.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALMiscellaneousLookups::LookupClaimScrubbingProfile", PROC_CLAIM_SCRUBBING_PROFILE_LOOKUP, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        ///// <summary>
        ///// Lookups the plan fee link.
        ///// </summary>
        ///// <returns></returns>
        //public DSCodeLookup LookupPlanFeeLink()
        //{
        //    DSCodeLookup ds = new DSCodeLookup();
        //    IDBManager dbManager =ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PLAN_FEE_LINK_LOOKUP, ds, ds.PlanFeeLink.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALMiscellaneousLookups::LookupPlanFeeLink", PROC_PLAN_FEE_LINK_LOOKUP, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        ///// <summary>
        ///// Lookups the type of the plan.
        ///// </summary>
        ///// <returns></returns>
        //public DSCodeLookup LookupPlanType()
        //{
        //    DSCodeLookup ds = new DSCodeLookup();
        //    IDBManager dbManager =ClientConfiguration.GetDBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        ds = (DSCodeLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PLAN_TYPE_LOOKUP, ds, ds.PlanType.TableName);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("DALMiscellaneousLookups::LookupPlanType", PROC_PLAN_TYPE_LOOKUP, ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}
        //#endregion
    }
}
