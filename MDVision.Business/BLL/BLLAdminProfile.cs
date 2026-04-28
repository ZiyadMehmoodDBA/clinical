using System;
using System.Collections.Generic;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Admin;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.DataAccess.DAL.BatchFax;
using MDVision.Model.Lookups;
using MDVision.DataAccess.DCommon;
using MDVision.Model.Admin.Provider;
using System.Data;
using MDVision.Model.Clinical.Reports;

namespace MDVision.Business.BLL
{
    public class BLLAdminProfile
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminProfile"/> class.
        /// </summary>
        public BLLAdminProfile()
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call

        }

        public BLLAdminProfile(SharedVariable SharedVariable)
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
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
        #region "Entity"
        /// <summary>
        /// Loads the entity.
        /// </summary>
        /// <param name="EntityId">The entity identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <returns></returns>
        public BLObject<DSProfile> LoadEntity(long EntityId, string ShortName)
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALEntity().LoadEntity(EntityId, ShortName);

                return new BLObject<DSProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadEntity", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the entity.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupEntity()
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALEntity().LookupEntity();

                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupEntity", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }
        public BLObject<DSUsers> LookupCoWorker()
        {
            try
            {
                DSUsers ds = new DSUsers();
                ds = new DALUser().LookupCoWorker();

                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupCoWorker", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }
        //public BLObject<DSProfileLookup> LookupSecurityGroup()
        //{
        //    try
        //    {
        //        DSProfileLookup ds = new DSProfileLookup();
        //        ds = new DALUser().LookupSecurityGroup(0);

        //        return new BLObject<DSProfileLookup>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLAdminProfile::LookupSecurityGroup", ex);
        //        return new BLObject<DSProfileLookup>(null, ex.Message);
        //    }
        //}
        #endregion

        #region "Practice"
        /// <summary>
        /// Loads the practice.
        /// </summary>
        /// <param name="PracticeId">The practice identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="Entity">The entity.</param>
        /// <param name="EIN">The ein.</param>
        /// <returns></returns>
        public BLObject<DSProfile> LoadPractice(long PracticeId, string ShortName, string Description, string Entity, string EIN, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALPractice().LoadPractice(PracticeId, ShortName, Description, Entity, EIN, Active, PageNumber, RowsPerPage);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadPractice", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        public BLObject<DSProfile> LoadDemographicPractice(long PracticeId, string Entity, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALPractice().LoadDemographicPractice(PracticeId, Entity, PageNumber, RowsPerPage);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadDemographicPractice", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the practice.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupPractice(string Active)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALPractice().LookupPractice(Active);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupPractice", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        /// <summary>
        /// Updates the practice.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> UpdatePractice(ref DSProfile ds)
        {
            try
            {

                ds = new DALPractice().UpdatePractice(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdatePractice", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        /// <summary>
        /// Deletes the practice.
        /// </summary>
        /// <param name="PracticeIds">The practice ids.</param>
        /// <returns></returns>
        public BLObject<string> DeletePractice(string PracticeIds)
        {
            try
            {

                PracticeIds = new DALPractice().DeletePractice(PracticeIds);

                return new BLObject<string>(PracticeIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeletePractice", ex);
                return new BLObject<string>("", ex.Message);
            }

        }

        /// <summary>
        /// Inserts the practice.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> InsertPractice(ref DSProfile ds)
        {
            try
            {

                ds = new DALPractice().InsertPractice(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertPractice", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        public BLObject<DSProfileLookup> LookupPractice(string Active, string EntityId = null, string ShortName = "")
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALPractice().LookupPractice(Active, EntityId, ShortName);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupPractice", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        #endregion

        #region "Facility"
        /// <summary>
        /// Loads the facility.
        /// </summary>
        /// <param name="PracticeId">The practice identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="Entity">The entity.</param>
        /// <param name="EIN">The ein.</param>
        /// <returns></returns>
        public BLObject<DSProfile> LoadFacility(long PracticeId, string ShortName, string Description, string Entity, string EIN, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALFacility().LoadFacility(PracticeId, ShortName, Description, Entity, EIN, Active, PageNumber, RowsPerPage);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadFacility", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        /// <summary>
        /// Loads the Facility Outgoing Referral.
        /// </summary>
        /// <param name="PracticeId">The practice identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="Entity">The entity.</param>
        /// <param name="EIN">The ein.</param>
        /// <returns></returns>
        public BLObject<DSProfile> LoadFacilityOutgoingReferral(long PracticeId, string ShortName, string Description, string Entity, string EIN, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALFacility().LoadFacilityOutgoingReferral(PracticeId, ShortName, Description, Entity, EIN, Active, PageNumber, RowsPerPage);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadFacility", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }
        public BLObject<DSProfile> LoadProviderDiagnosticImagingFacility(long FacilityId, string ShortName, string Description, string Entity, string EIN, string Active, long ProviderId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALFacility().LoadProviderDiagnosticImagingFacility(FacilityId, ShortName, Description, Entity, EIN, Active, ProviderId, PageNumber, RowsPerPage);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadProviderDiagnosticImagingFacility", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }
        //Begin Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277
        public BLObject<DSProfileLookup> LookupFacilityOutgoingReferral(string Active, string EntityId = null)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALFacility().LookupFacilityOutgoingReferral(Active, EntityId);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupFacilityOutgoingReferral", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }
        //End Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277
        /// <summary>
        /// Lookups the facility.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupFacility(string Active, string EntityId = null, string ShortName = "")
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALFacility().LookupFacility(Active, EntityId, ShortName);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupFacility", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }
        public BLObject<DSProfileLookup> ProvidersDiagnosticImagingFacilityLookUp(Int64 ProviderId, string Active, string EntityId = null, string ShortName = "")
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALFacility().ProvidersDiagnosticImagingFacilityLookUp(ProviderId, Active, EntityId, ShortName);
                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupFacility", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }

        public BLObject<DSProfileLookup> LookupFacilityDescription(string Active, string EntityId = null, string ShortName = "")
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALFacility().LookupFacilityDescription(Active, EntityId, ShortName);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupFacility", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }
        public BLObject<DSProfileLookup> LookupFacilitySchedular(string Active, string EntityId = null, string ShortName = "")
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALFacility().LookupFacilitySchedular(Active, EntityId, ShortName);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupFacility", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }
        /// <summary>
        /// Lookups the type of the facility.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupFacilityType()
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALFacility().LookupFacilityType();

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupFacilityType", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        public BLObject<DSProfileLookup> LookupLocation()
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALFacility().LookupLocation();

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupLocation", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        /// <summary>
        /// Updates the facility.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> UpdateFacility(ref DSProfile ds)
        {
            try
            {

                ds = new DALFacility().UpdateFacility(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateFacility", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        public BLObject<DSProfileLookup> LookupFacilityByName(string Searchstring)
        {
            DSProfileLookup ds = new DSProfileLookup();
            try
            {

                ds = new DALFacility().LookupFacilityByName(Searchstring);
                ds.AcceptChanges();
                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupFacilityByName", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }

        public BLObject<DSProfileLookup> LookupProviderByName(string Searchstring)
        {
            DSProfileLookup ds = new DSProfileLookup();
            try
            {

                ds = new DALProvider().LookupProviderByName(Searchstring);
                ds.AcceptChanges();
                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupProviderByName", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }
        public BLObject<DSProfileLookup> LookupSpecialtyByName(string Searchstring)
        {
            DSProfileLookup ds = new DSProfileLookup();
            try
            {

                ds = new DALSpecialty().LookupSpecialtyByName(Searchstring);
                ds.AcceptChanges();
                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupSpecialtyByName", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }

        public BLObject<DSProfileLookup> LookupRefProviderByName(string Searchstring)
        {
            DSProfileLookup ds = new DSProfileLookup();
            try
            {

                ds = new DALProvider().LookupRefProviderByName(Searchstring);
                ds.AcceptChanges();
                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupRefProviderByName", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the facility.
        /// </summary>
        /// <param name="FacilityIds">The facility ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteFacility(string FacilityIds)
        {
            try
            {

                FacilityIds = new DALFacility().DeleteFacility(FacilityIds);

                return new BLObject<string>(FacilityIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteFacility", ex);
                return new BLObject<string>("", ex.Message);
            }

        }

        /// <summary>
        /// Inserts the facility.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> InsertFacility(ref DSProfile ds)
        {
            try
            {

                ds = new DALFacility().InsertFacility(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertFacility", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }
        #endregion

        #region "Provider"
        /// <summary>
        /// Loads the provider.
        /// </summary>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="SpecialityId">The speciality identifier.</param>
        /// <param name="NPI">The npi.</param>
        /// <param name="EntityId">The entity identifier.</param>
        /// <returns></returns>
        public BLObject<DSProfile> LoadProvider(long ProviderId, string ShortName, string FirstName, string LastName, string SpecialityId, string NPI, string EntityId, string Active, int PageNumber = 1, int RowspPage = 1000, string ParentCtrl = "")
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALProvider().LoadProvider(ProviderId, ShortName, FirstName, LastName, SpecialityId, NPI, EntityId, Active, PageNumber, RowspPage, ParentCtrl);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadProvider", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }
        public BLObject<List<ProviderModel>> GetProviderCpts(long ProviderId)
        {
            try
            {
                var result = new DALProvider().GetProviderCpts(ProviderId);
                return new BLObject<List<ProviderModel>>(result);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::GetProviderCpts", ex);
                return new BLObject<List<ProviderModel>>(null, ex.Message);
            }

        }
        /// <summary>
        /// Loads the provider for threaded methods.
        /// </summary>
        /// <param name="SharedVariable">SharedVariable.</param>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="SpecialityId">The speciality identifier.</param>
        /// <param name="NPI">The npi.</param>
        /// <param name="EntityId">The entity identifier.</param>
        /// <returns></returns>
        public BLObject<DSProfile> LoadProvider(SharedVariable SharedVariable, long ProviderId, string ShortName, string FirstName, string LastName, string SpecialityId, string NPI, string EntityId, string Active, int PageNumber = 1, int RowspPage = 1000, string ParentCtrl = "")
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALProvider(SharedVariable).LoadProvider(SharedVariable, ProviderId, ShortName, FirstName, LastName, SpecialityId, NPI, EntityId, Active, PageNumber, RowspPage, ParentCtrl);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog(SharedVariable, "BLLAdminProfile::LoadProvider", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }
        public BLObject<DSProfile> LoadProviderEntityBased(long ProviderId, string ShortName, string FirstName, string LastName, string SpecialityId, string NPI, string EntityId, string Active, int PageNumber = 1, int RowspPage = 1000, string ParentCtrl = "")
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALProvider().LoadProviderEntityBased(ProviderId, ShortName, FirstName, LastName, SpecialityId, NPI, EntityId, Active, PageNumber, RowspPage, ParentCtrl);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadProvider", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the type of the provider.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupProviderType()
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALProvider().LookupProviderType();

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupProviderType", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the provider.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupProvider(string Active, bool TIN = false, string ShortName = "")
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALProvider().LookupProvider(Active, TIN, true, ShortName);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupProvider", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }
        public BLObject<DSProfileLookup> LookupProviderWithQualification(string Active, bool TIN = false, string ShortName = "")
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALProvider().LookupProviderWithQualification(Active, TIN, true, ShortName);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupProvider", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }
        public BLObject<DSProfileLookup> LookupAllProviders(string Active)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALProvider().LookupAllProviders(Active);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupAllProviders", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        public BLObject<DSProfileLookup> LookupProviderEntityBased(string Active, string EntityIds, bool TIN = false)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALProvider().LookupProviderEntityBased(Active, TIN, EntityIds);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupProviderEntityBased", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        public BLObject<DSProfileLookup> LookupProvider(string Active, string EntityIds, bool TIN = false)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALProvider().LookupProvider(Active, TIN, EntityIds);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupProvider", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        public BLObject<DSProfileLookup> LookupProviderBasedSpecialty(string Active, string ProvideId)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALProvider().LookupProviderBasedSpecialty(Active, ProvideId);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupProvider", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        public BLObject<List<ProfileLookupModel>> LookupNotesProviders(string patientId)
        {
            try
            {
                List<ProfileLookupModel> ProvidersListList = new List<ProfileLookupModel>();
                ProvidersListList = new DALProvider().LookupNotesProviders(patientId);
                return new BLObject<List<ProfileLookupModel>>(ProvidersListList);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupNotesProviders", ex);
                return new BLObject<List<ProfileLookupModel>>(null, ex.Message);
            }

        }


        public BLObject<DSProfileLookup> LookupAnesthesiologist(string Active, bool TIN = false)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALProvider().LookupAnesthesiologist(Active, TIN);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupProvider", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        public BLObject<DSProfileLookup> LookupCRNA(string Active, bool TIN = false)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALProvider().LookupCRNA(Active, TIN);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupProvider", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }
        /// <summary>
        /// Lookups the type of the profile.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupProfileType()
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALProvider().LookupProfileType();

                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupProfileType", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the provider.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> UpdateProvider(ref DSProfile ds, ref DataTable dtFacility, ref DataTable dtBulkSignException)
        {
            try
            {

                ds = new DALProvider().UpdateProvider(ref ds, ref dtFacility, ref dtBulkSignException);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateProvider", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }
        public BLObject<DSProfile> UpdateProviderIsActive(ref DSProfile ds)
        {
            try
            {
                ds = new DALProvider().UpdateProviderIsActive(ref ds);
                return new BLObject<DSProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateProviderIsActive", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }
        }


        /// <summary>
        /// Deletes the provider.
        /// </summary>
        /// <param name="ProviderIds">The provider ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteProvider(string ProviderIds)
        {
            try
            {
                ProviderIds = new DALProvider().DeleteProvider(ProviderIds);
                return new BLObject<string>(ProviderIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteProvider", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        public BLObject<string> DeleteAssociatedProcedure(string ProcedureListId)
        {
            try
            {
                string result = new DALProvider().DeleteAssociatedProcedure(ProcedureListId);
                return new BLObject<string>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteAssociatedProcedure", ex);
                return new BLObject<string>("", ex.Message);
            }
        }
        /// <summary>
        /// Inserts the provider.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> InsertProvider(ref DSProfile ds, ref DataTable dtFacility, ref DataTable dtBulkSignException)
        {
            try
            {

                ds = new DALProvider().InsertProvider(ref ds, ref dtFacility,ref dtBulkSignException);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertProvider", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }
        #endregion




        #region "Provider License Info"
        /// <summary>
        /// Loads the provider license information.
        /// </summary>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <param name="ProviderLicenseId">The provider license identifier.</param>
        /// <returns></returns>
        public BLObject<DSProfile> LoadProviderLicenseInfo(long ProviderId, long ProviderLicenseId)
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALProvider().LoadProviderLicenseInfo(ProviderId, ProviderLicenseId);
                return new BLObject<DSProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadProviderLicenseInfo", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the provider license info.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> UpdateProviderLicenseInfo(ref DSProfile ds)
        {
            try
            {

                ds = new DALProvider().UpdateProviderLicenseInfo(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateProviderLicenseInfo", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        /// <summary>
        /// Deletes the provider.
        /// </summary>
        /// <param name="ProviderIds">The provider ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteProviderLicenseInfo(string ProviderLicenseIds)
        {
            try
            {
                ProviderLicenseIds = new DALProvider().DeleteProviderLicenseInfo(ProviderLicenseIds);
                return new BLObject<string>(ProviderLicenseIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteProviderLicenseInfo", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the provider license info.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> InsertProviderLicenseInfo(ref DSProfile ds)
        {
            try
            {

                ds = new DALProvider().InsertProviderLicenseInfo(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertProviderLicenseInfo", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        #endregion

        #region "Resources"
        /// <summary>
        /// Loads the resources.
        /// </summary>
        /// <param name="ResourceId">The resource identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <returns></returns>
        public BLObject<DSProfile> LoadResources(long ResourceId, string ShortName, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALResources().LoadResources(ResourceId, ShortName, Active, PageNumber, RowsPerPage);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadResources", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the resources.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupResources(string Active)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALResources().LookupResources(Active);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupResources", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        /// <summary>
        /// Updates the resources.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> UpdateResources(ref DSProfile ds)
        {
            try
            {

                ds = new DALResources().UpdateResources(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateResources", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        /// <summary>
        /// Deletes the resources.
        /// </summary>
        /// <param name="ResourcesIds">The resources ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteResources(string ResourcesIds)
        {
            try
            {

                ResourcesIds = new DALResources().DeleteResources(ResourcesIds);

                return new BLObject<string>(ResourcesIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteResources", ex);
                return new BLObject<string>("", ex.Message);
            }

        }

        /// <summary>
        /// Inserts the resources.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> InsertResources(ref DSProfile ds)
        {
            try
            {

                ds = new DALResources().InsertResources(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertResources", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }
        #endregion

        #region "Specialty"
        /// <summary>
        /// Loads the specialty.
        /// </summary>
        /// <param name="SpecialtyId">The specialty identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public BLObject<DSProfile> LoadSpecialty(long SpecialtyId, string ShortName, string Description, string EntityId, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALSpecialty().LoadSpecialty(SpecialtyId, ShortName, Description, EntityId, Active, PageNumber, RowsPerPage);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadSpecialty", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }
        }

        public BLObject<DSProfile> LoadSpecialtyAll(long SpecialtyId, string ShortName, string Description, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALSpecialty().LoadSpecialtyAll(SpecialtyId, ShortName, Description, Active, PageNumber, RowsPerPage);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadSpecialty", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }
        }


        /// <summary>
        /// Lookups the specialty.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupSpecialty(string Active)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALSpecialty().LookupSpecialty(Active);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupSpecialty", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }
        public BLObject<DSProfileLookup> LookupSpecialtiesAllEntities(string Active)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALSpecialty().LookupSpecialtiesAllEntities(Active);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupSpecialty", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }

        public BLObject<DSProfileLookup> LookupSpecialty(string Active, string EntityId)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALSpecialty().LookupSpecialty(Active, EntityId);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupSpecialty", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the specialty.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> UpdateSpecialty(ref DSProfile ds)
        {
            try
            {

                ds = new DALSpecialty().UpdateSpecialty(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateSpecialty", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        /// <summary>
        /// Deletes the specialty.
        /// </summary>
        /// <param name="SpecialtyIds">The specialty ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteSpecialty(string SpecialtyIds)
        {
            try
            {

                SpecialtyIds = new DALSpecialty().DeleteSpecialty(SpecialtyIds);

                return new BLObject<string>(SpecialtyIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteSpecialty", ex);
                return new BLObject<string>("", ex.Message);
            }

        }

        /// <summary>
        /// Inserts the specialty.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> InsertSpecialty(ref DSProfile ds)
        {
            try
            {

                ds = new DALSpecialty().InsertSpecialty(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertSpecialty", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }
        #endregion

        #region "Roles"
        public BLObject<DSProfileLookup> LookupRoles(string Active, bool IsEmergencyRole = false)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALRoles().LookupRoles(Active, IsEmergencyRole);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupRoles", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }


        public BLObject<DSProfileLookup> LookupAuditReportRoles(string Active, bool IsEmergencyRole = false)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALRoles().LookupAuditReportRoles(Active, IsEmergencyRole);

                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupRoles", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }

        #endregion

        #region "BillingProviderSettings"
        /// <summary>
        /// Loads the billing provider settings.
        /// </summary>
        /// <param name="BillingProviderId">The billing provider identifier.</param>
        /// <param name="InsuranceId">The insurance identifier.</param>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <returns></returns>
        public BLObject<DSBillingProviderSettings> LoadBillingProviderSettings(long BillingProviderId, string InsuranceId, string FacilityId, string ProviderId, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSBillingProviderSettings ds = new DSBillingProviderSettings();
                ds = new DALBillingProviderSettings().LoadBillingProviderSettings(BillingProviderId, InsuranceId, FacilityId, ProviderId, Active, PageNumber, RowsPerPage);

                return new BLObject<DSBillingProviderSettings>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadBillingProviderSettings", ex);
                return new BLObject<DSBillingProviderSettings>(null, ex.Message);
            }

        }

        /// <summary>
        /// Inserts the BillingProviderSettings.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSBillingProviderSettings> InsertBillingProviderSettings(ref DSBillingProviderSettings ds)
        {
            try
            {

                ds = new DALBillingProviderSettings().InsertBillingProviderSettings(ref ds);

                return new BLObject<DSBillingProviderSettings>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertBillingProviderSettings", ex);
                return new BLObject<DSBillingProviderSettings>(null, ex.Message);
            }

        }

        /// <summary>
        /// Updates the BillingProviderSettings.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// 
        public BLObject<DSBillingProviderSettings> UpdateBillingProviderSettings(ref DSBillingProviderSettings ds)
        {
            try
            {

                ds = new DALBillingProviderSettings().UpdateBillingProviderSettings(ref ds);

                return new BLObject<DSBillingProviderSettings>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateBillingProviderSettings", ex);
                return new BLObject<DSBillingProviderSettings>(null, ex.Message);
            }

        }
        /// <summary>
        /// Deletes the BillingProviderSettings.
        /// </summary>
        /// <param name="UserIds">The user ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteBillingProviderSettings(string UserIds)
        {
            try
            {

                UserIds = new DALBillingProviderSettings().DeleteBillingProviderSettings(UserIds);

                return new BLObject<string>(UserIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteBillingProviderSettings", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the insurance.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupInsurance(string Active)
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALInsurance().LookupInsurance(Active);

                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupInsurance", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the loop2310 b.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupLoop2310B()
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALBillingProviderSettings().LookupLoop2310B();

                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupLoop2310B", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }
        #endregion

        #region "BillingProvider"
        /// <summary>
        /// Loads the billing provider settings.
        /// </summary>
        /// <param name="BillingProviderId">The billing provider identifier.</param>
        /// <returns></returns>
        public BLObject<DSBillingProviderSettings> LoadBillingProvider(long BillingProviderId, string ShortName, string EntityId, string IsBilltoEIN, string Active,string NPINumber, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSBillingProviderSettings ds = new DSBillingProviderSettings();
                ds = new DALBillingProviderSettings().LoadBillingProvider(BillingProviderId, ShortName, EntityId, IsBilltoEIN, Active, NPINumber, PageNumber, RowsPerPage);

                return new BLObject<DSBillingProviderSettings>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadBillingProvider", ex);
                return new BLObject<DSBillingProviderSettings>(null, ex.Message);
            }

        }

        public BLObject<DSBillingProviderSettings> InsertBillingProvider(ref DSBillingProviderSettings ds)
        {
            try
            {

                ds = new DALBillingProviderSettings().InsertBillingProvider(ref ds);

                return new BLObject<DSBillingProviderSettings>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertBillingProvider", ex);
                return new BLObject<DSBillingProviderSettings>(null, ex.Message);
            }

        }

        public BLObject<DSBillingProviderSettings> UpdateBillingProvider(ref DSBillingProviderSettings ds)
        {
            try
            {

                ds = new DALBillingProviderSettings().UpdateBillingProvider(ref ds);

                return new BLObject<DSBillingProviderSettings>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateBillingProvider", ex);
                return new BLObject<DSBillingProviderSettings>(null, ex.Message);
            }

        }

        public BLObject<string> DeleteBillingProvider(string UserIds)
        {
            try
            {

                UserIds = new DALBillingProviderSettings().DeleteBillingProvider(UserIds);

                return new BLObject<string>(UserIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteBillingProvider", ex);
                return new BLObject<string>(null, ex.Message);
            }

        }

        public BLObject<DSProfileLookup> LookupLoopBillingProvider(string Active)
        {
            try
            {
                DSProfileLookup ds = new DALBillingProviderSettings().LookupLoopBillingProvider(Active);
                return new BLObject<DSProfileLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupLoopBillingProvider", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }

        }

        #endregion

        #region "ReferringProvider"
        /// <summary>
        /// Loads the referring provider.
        /// </summary>
        /// <param name="ReferringProviderId">The referring provider identifier.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="Active">The active.</param>
        /// <param name="NPI">The npi.</param>
        /// <param name="EntityId">The entity identifier.</param>
        /// <returns></returns>
        public BLObject<DSProfile> LoadReferringProvider(long ReferringProviderId, string FirstName, string LastName, string Active, string NPI, string EntityId, int PageNumber = 1, int RowsPerPage = 1000, string ParentCtrl = "",string Fax="", string Specialty = null, string Phone = "", string IsSovereign = "")
        {
            try
            {
                DSProfile ds = new DSProfile();
                ds = new DALReferringProvider().LoadReferringProvider(ReferringProviderId, FirstName, LastName, Active, NPI, EntityId, PageNumber, RowsPerPage, ParentCtrl, Fax, Specialty, Phone, IsSovereign);

                return new BLObject<DSProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadReferringProvider", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the ReferringProvider.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> UpdateReferringProvider(ref DSProfile ds)
        {
            try
            {
                ds = new DALReferringProvider().UpdateReferringProvider(ref ds);
                return new BLObject<DSProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateReferringProvider", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the ReferringProvider.
        /// </summary>
        /// <param name="SpecialtyIds">The ReferringProvider ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteReferringProvider(string ReferringProviderIds)
        {
            try
            {
                ReferringProviderIds = new DALReferringProvider().DeleteReferringProvider(ReferringProviderIds);
                return new BLObject<string>(ReferringProviderIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteReferringProvider", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the ReferringProvider.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> InsertReferringProvider(ref DSProfile ds)
        {
            try
            {
                ds = new DALReferringProvider().InsertReferringProvider(ref ds);
                return new BLObject<DSProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertReferringProvider", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the referring provider.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSProfileLookup> LookupReferringProvider(string Active, string Name = "")
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALReferringProvider().LookupReferringProvider(Active, Name);
                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupReferringProvider", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }

        public BLObject<DSProfileLookup> LookupReferringProviderOutgoing(string Active, string Name = "")
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALReferringProvider().LookupReferringProviderOutgoing(Active, Name);
                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupReferringProviderOutgoing", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }
        public BLObject<DSProfileLookup> LookupReferringProviderAutoComplete(string Active, string Name = "")
        {
            try
            {
                DSProfileLookup ds = new DSProfileLookup();
                ds = new DALReferringProvider().LookupReferringProviderAutocomplete(Active, Name);
                return new BLObject<DSProfileLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupReferringProvider", ex);
                return new BLObject<DSProfileLookup>(null, ex.Message);
            }
        }
        #endregion

        #region "Provider Fax Settings"
        public string InsertProviderFaxSetting(ref DSProfile ds)
        {
            try
            {
                ds = new DALProvider().InsertProviderFaxSetting(ref ds);

                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public string InsertProviderFaxSettingUsers(ref DSProfile ds)
        {
            try
            {
                ds = new DALProvider().InsertProviderFaxSettingUsers(ref ds);

                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public string InsertFacilityFaxSettingUsers(ref DSProfile ds)
        {
            try
            {
                ds = new DALFacility().InsertFacilityFaxSettingUsers(ref ds);

                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public BLObject<DSProfile> LoadProviderFaxSettings(long ProviderId, int PageNumber = 1, int RowspPage = 1000)
        {
            DSProfile ds = new DSProfile();
            try
            {
                ds = new DALProvider().LoadProviderFaxSettings(ProviderId, PageNumber, RowspPage);

                return new BLObject<DSProfile>(ds);
            }
            catch (Exception e)
            {
                return new BLObject<DSProfile>(ds);
            }
        }
        public BLObject<DSProfile> LoadProviderFaxSettingsUsers(long ProviderId, int PageNumber = 1, int RowspPage = 1000)
        {
            DSProfile ds = new DSProfile();
            try
            {
                ds = new DALProvider().LoadProviderFaxSettingsUsers(ProviderId, 0, PageNumber, RowspPage);

                return new BLObject<DSProfile>(ds);
            }
            catch (Exception e)
            {
                return new BLObject<DSProfile>(ds);
            }
        }
        public BLObject<DSProfile> LoadFacilityFaxSettingsUsers(long FacilityId, int PageNumber = 1, int RowspPage = 1000)
        {
            DSProfile ds = new DSProfile();
            try
            {
                ds = new DALFacility().LoadFacilityFaxSettingsUsers(FacilityId, 0, PageNumber, RowspPage);

                return new BLObject<DSProfile>(ds);
            }
            catch (Exception e)
            {
                return new BLObject<DSProfile>(ds);
            }
        }
        public string UpdateProviderFaxSettings(ref DSProfile ds)
        {
            try
            {

                ds = new DALProvider().UpdateProviderFaxSettings(ref ds);

                return "Success updating";

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateProvider", ex);
                return "";
            }

        }
        public string DeleteProviderFaxSettings(long ProviderId)
        {
            try
            {
                string id = new DALProvider().DeleteProviderFaxSettings(ProviderId);
                return "Success";
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteProviderFaxSettings", ex);
                return "";
            }
        }
        public string DeleteProviderContacts(long ProviderId, long ContactId)
        {
            try
            {
                string id = new DALBatchFax().DeleteProviderFaxContacts(ProviderId, ContactId);
                return "Success";
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteProviderContacts", ex);
                return "";
            }
        }
        public string DeleteFacilityContacts(long FacilityId, long ContactId)
        {
            try
            {
                string id = new DALBatchFax().DeleteFacilityFaxContacts(FacilityId, ContactId);
                return "Success";
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteProviderContacts", ex);
                return "";
            }
        }

        public string DeleteProviderFaxSettingsUsers(long UserId)
        {
            try
            {
                string id = new DALProvider().DeleteProviderFaxSettingsUsers(UserId);
                return "Success";
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteProviderFaxSettings", ex);
                return "";
            }
        }
        public string DeleteFacilityFaxSettingsUsers(long UserId)
        {
            try
            {
                string id = new DALFacility().DeleteFacilityFaxSettingsUsers(UserId);
                return "Success";
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteFacilityFaxSettings", ex);
                return "";
            }
        }
        #endregion


        #region "Facility Fax Settings"
        public string InsertFacilityFaxSetting(ref DSProfile ds)
        {
            try
            {
                ds = new DALFacility().InsertFacilityFaxSetting(ref ds);

                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public BLObject<DSProfile> LoadFacilityFaxSettings(long FacilityId, int PageNumber = 1, int RowspPage = 1000)
        {
            DSProfile ds = new DSProfile();
            try
            {
                ds = new DALFacility().LoadFacilityFaxSettings(FacilityId, PageNumber, RowspPage);

                return new BLObject<DSProfile>(ds);
            }
            catch (Exception e)
            {
                return new BLObject<DSProfile>(ds);
            }
        }
        public string UpdateFacilityFaxSettings(ref DSProfile ds)
        {
            try
            {

                ds = new DALFacility().UpdateFacilityFaxSettings(ref ds);

                return "Success updating";

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateFacility", ex);
                return "";
            }

        }
        public string DeleteFacilityFaxSettings(long FacilityId)
        {
            try
            {
                string id = new DALFacility().DeleteFacilityFaxSettings(FacilityId);
                return "Success";
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteFacilityFaxSettings", ex);
                return "";
            }
        }
        #endregion

        #region "Fax Profiles"

        public BLObject<DSProfile> LoadUsersFaxProfiles(long UserId, int PageNumber = 1, int RowspPage = 1000)
        {
            DSProfile ds = new DSProfile();
            try
            {
                ds = new DALBatchFax().LoadProviderFaxSettingsUsers(UserId, PageNumber, RowspPage);

                return new BLObject<DSProfile>(ds);
            }
            catch (Exception e)
            {
                return new BLObject<DSProfile>(ds);
            }
        }
        public BLObject<DSProfile> LoadProviderFaxContacts(long ProviderId, int PageNumber = 1, int RowspPage = 1000)
        {
            DSProfile ds = new DSProfile();
            try
            {
                ds = new DALBatchFax().LoadProviderContacts(ProviderId, PageNumber, RowspPage);

                return new BLObject<DSProfile>(ds);
            }
            catch (Exception e)
            {
                return new BLObject<DSProfile>(ds);
            }
        }
        public BLObject<DSProfile> LoadFacilityFaxContacts(long FacilityId, int PageNumber = 1, int RowspPage = 1000)
        {
            DSProfile ds = new DSProfile();
            try
            {
                ds = new DALBatchFax().LoadFacilityContacts(FacilityId, PageNumber, RowspPage);

                return new BLObject<DSProfile>(ds);
            }
            catch (Exception e)
            {
                return new BLObject<DSProfile>(ds);
            }
        }
        public BLObject<DSProfile> SearchFaxContacts(bool IsProvider, long ProviderId, long FacilityId, string contactName, int PageNumber = 1, int RowspPage = 1000)
        {
            DSProfile ds = new DSProfile();
            try
            {
                ds = new DALBatchFax().SearchContacts(IsProvider, ProviderId, FacilityId, contactName, PageNumber, RowspPage);

                return new BLObject<DSProfile>(ds);
            }
            catch (Exception e)
            {
                return new BLObject<DSProfile>(ds);
            }
        }
        public BLObject<DSProfile> InsertProviderContact(ref DSProfile ds, string ProviderOrFacility)
        {
            try
            {
                if (ProviderOrFacility == "Provider")
                    ds = new DALBatchFax().SaveProviderContact(ref ds);
                else
                    ds = new DALBatchFax().SaveFacilityContact(ref ds);
                return new BLObject<DSProfile>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertProviderContact", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }
        }

        public string InsertFaxDocument(ref DSDocument ds)
        {
            try
            {
                ds = new DALBatchFax().SaveFaxDocument(ref ds);

                return "Success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public BLObject<DSDocument> LoadFaxConfidentiality(int pagenumber, int rowsppage)
        {
            DSDocument ds = new DSDocument();
            try
            {
                ds = new DALBatchFax().LoadFaxConfidentiality(pagenumber, rowsppage);

                return new BLObject<DSDocument>(ds);
            }
            catch (Exception e)
            {
                return new BLObject<DSDocument>(ds);
            }
        }

        public string InsertFacilityContact(ref DSProfile ds)
        {
            try
            {
                ds = new DALBatchFax().SaveFacilityContact(ref ds);

                return "Success";
            }
            catch (Exception e)
            {
                return "";
            }
        }
        #endregion
        #region ParticipantProvider
        public BLObject<DSProfile> LoadProviderParticipant(long ProviderParticipantId, string Assignment, string Active, string ProviderId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {

                DSProfile ds = new DSProfile();
                ds = new DALParticipantProvider().LoadParticipantProvider(ProviderParticipantId, Assignment, Active, ProviderId, PageNumber, RowsPerPage);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoadParticipantProvider", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }
        }
        /// <summary>
        /// Updates the Provider Participant.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> UpdateParticipantProvider(ref DSProfile ds)
        {
            try
            {

                ds = new DALParticipantProvider().UpdateParticipantProvider(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateParticipantProvider", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }

        /// <summary>
        /// Deletes the ProviderParticipant.
        /// </summary>
        /// <param name="ProviderParticipantIds">The ProviderParticipant ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteParticipantProvider(string ProviderParticipantIds)
        {
            try
            {

                ProviderParticipantIds = new DALParticipantProvider().DeleteParticipantProvider(ProviderParticipantIds);

                return new BLObject<string>(ProviderParticipantIds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::DeleteParticipantProvider", ex);
                return new BLObject<string>("", ex.Message);
            }

        }

        /// <summary>
        /// Inserts the ProviderParticipant.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSProfile> InsertProviderParticipant(ref DSProfile ds)
        {
            try
            {

                ds = new DALParticipantProvider().InsertParticipantProvider(ref ds);

                return new BLObject<DSProfile>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertParticipantProvider", ex);
                return new BLObject<DSProfile>(null, ex.Message);
            }

        }
        #endregion

        #region Lookup Custom Form Name
        public BLObject<List<CustomFormLookupModel>> GetCustomFormLetters()
        {
            try
            {
                var ds = new DALBatchFax().GetCustomFormLetters();
                return new BLObject<List<CustomFormLookupModel>>(ds);


            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLPatient::LookupCustomFormName", ex);
                return new BLObject<List<CustomFormLookupModel>>(null, ex.Message);
            }

        }
        #endregion


        #region "Outgoing Referrals LookUp"
        public BLObject<List<OutgoingReferralsLookupModel>> GetOutgoingReferrals(Int64 noteId, Int64 patientId)
        {
            try
            {
                var ds = new DALBatchFax().GetOutgoingReferrals(noteId, patientId);
                return new BLObject<List<OutgoingReferralsLookupModel>>(ds);


            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::GetOutgoingReferrals", ex);
                return new BLObject<List<OutgoingReferralsLookupModel>>(null, ex.Message);
            }

        }
        public BLObject<List<OutgoingReferralsLookupModel>> GetPatientReferrals(Int64 patientId)
        {
            try
            {
                var ds = new DALBatchFax().GetPatientReferrals(patientId);
                return new BLObject<List<OutgoingReferralsLookupModel>>(ds);


            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::GetOutgoingReferrals", ex);
                return new BLObject<List<OutgoingReferralsLookupModel>>(null, ex.Message);
            }

        }
        #endregion
        #endregion
    }
}
