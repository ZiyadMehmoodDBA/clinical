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
    public class BLLCityStateZip
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminProfile"/> class.
        /// </summary>
        public BLLCityStateZip()
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
        public BLObject<DSProfileLookup> LookupCity()
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALCityStateZip().GetCityLookup();

                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCityStateZip::LookupCity", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }
        public BLObject<DSProfileLookup> LookupState()
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALCityStateZip().GetStateLookup();

                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCityStateZip::LookupState", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }
        public BLObject<DSProfileLookup> LookupCountry()
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALCityStateZip().GetCountryLookup();

                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCityStateZip::LookupCountry", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }

        public BLObject<DSCityStateZip> GetCityState( string zipcode,string cityname)
        {
            try
            {
                DSCityStateZip ds = new DSCityStateZip();
                ds = new DALCityStateZip().GetCityState(zipcode, cityname);

                return new BLObject<DSCityStateZip>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLCityStateZip::GetCityState", ex);
                return new BLObject<DSCityStateZip>(null, ex.Message);
            }

        }

        #endregion
    }
}
