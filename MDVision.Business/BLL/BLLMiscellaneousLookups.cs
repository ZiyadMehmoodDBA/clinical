using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.DataAccess.DAL;
using MDVision.Datasets;
using System.Configuration;

namespace MDVision.Business.BLL
{
    public class BLLMiscellaneousLookups
    {
        //#region Variable
        //
        //#endregion

        //#region Constructors
        ///// <summary>
        ///// Initializes a new instance of the <see cref="BLLMiscellaneousLookups"/> class.
        ///// </summary>
        //public BLLMiscellaneousLookups()
        //{
        //    //SharedVariable SharedObj
        //    //This call is required by the Web Services Designer.
        //    InitializeComponent();
        //    // this.SharedObj = SharedObj;
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

        //#region "Lookups"
        ///// <summary>
        ///// Lookups the claim flag.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSCodeLookup> LookupClaimFlag()
        //{
        //    try
        //    {
        //        DSCodeLookup ds = new DSCodeLookup();
        //        ds = new DALMiscellaneousLookups(SharedObj).LookupClaimFlag();
        //        return new BLObject<DSCodeLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLMiscellaneousLookups::LookupClaimFlag", ex);
        //        return new BLObject<DSCodeLookup>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Lookups the type of the claim.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSCodeLookup> LookupClaimType()
        //{
        //    try
        //    {
        //        DSCodeLookup ds = new DSCodeLookup();
        //        ds = new DALMiscellaneousLookups(SharedObj).LookupClaimType();
        //        return new BLObject<DSCodeLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLMiscellaneousLookups::LookupClaimType", ex);
        //        return new BLObject<DSCodeLookup>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Lookups the claim scrubbing profile.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSCodeLookup> LookupClaimScrubbingProfile()
        //{
        //    try
        //    {
        //        DSCodeLookup ds = new DSCodeLookup();
        //        ds = new DALMiscellaneousLookups(SharedObj).LookupClaimScrubbingProfile();
        //        return new BLObject<DSCodeLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLMiscellaneousLookups::LookupClaimScrubbingProfile", ex);
        //        return new BLObject<DSCodeLookup>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Lookups the plan fee link.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSCodeLookup> LookupPlanFeeLink()
        //{
        //    try
        //    {
        //        DSCodeLookup ds = new DSCodeLookup();
        //        ds = new DALMiscellaneousLookups(SharedObj).LookupPlanFeeLink();
        //        return new BLObject<DSCodeLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLMiscellaneousLookups::LookupPlanFeeLink", ex);
        //        return new BLObject<DSCodeLookup>(null, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Lookups the type of the plan.
        ///// </summary>
        ///// <returns></returns>
        //public BLObject<DSCodeLookup> LookupPlanType()
        //{
        //    try
        //    {
        //        DSCodeLookup ds = new DSCodeLookup();
        //        ds = new DALMiscellaneousLookups(SharedObj).LookupPlanType();
        //        return new BLObject<DSCodeLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLMiscellaneousLookups::LookupPlanType", ex);
        //        return new BLObject<DSCodeLookup>(null, ex.Message);
        //    }
        //}
        //#endregion
    }
}
