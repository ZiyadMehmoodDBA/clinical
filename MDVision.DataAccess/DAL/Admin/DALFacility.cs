using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALFacility
    {

        #region Variable

        #endregion

        #region " Stored Procedure Names"
        private const string PROC_FACILITY_INSERT = "Provider.sp_FacilityInsert";
        private const string PROC_FACILITY_UPDATE = "Provider.sp_FacilityUpdate";
        private const string PROC_FACILITY_DELETE = "Provider.sp_FacilityDelete";
        private const string PROC_FACILITY_SELECT = "Provider.sp_FacilitySelect";
        private const string PROC_FACILITY_TYPE_LOOKUP = "Provider.sp_FacilityTypeLookup";
        private const string PROC_LOCATION_LOOKUP = "Provider.sp_LocationLookup";
        private const string PROC_FACILITY_LOOKUP = "Provider.sp_FacilityLookup";
        private const string PROC_FACILITY_ENTITY_LOOKUP = "Provider.sp_FacilityEntityLookup";
        private const string PROC_PROVIDER_DIAGNOSTICIMAGINGFACILITIES_ENTITY_LOOKUP = "Provider.sp_ProviderDiagnosticImagingFacilitiesLookup";
        private const string PROC_PROVIDER_DIAGNOSTICIMAGING_FACILITY_SELECT = "Provider.sp_ProviderDiagnosticImagingFacilitiesSelect";
        private const string PROC_FACILITY_LOOKUP_SCHEDULAR = "Provider.sp_FacilityLookup_Scheduler";
        private const string PROC_FACILITY_LOOKUP_DESCRIPTION = "Provider.sp_FacilityLookupDescription";
        private const string PROC_FACILITY_ENTITY_LOOKUP_SCHEDULAR = "Provider.sp_FacilityEntityLookup__Scheduler";

        // Fax Settings
        private const string PROC_FACILITY_FAX_SETTING_INSERT = "Provider.sp_FacilityFaxSettingsInsert";
        private const string PROC_FACILITY_FAX_SETTING_SELECT = "Provider.sp_FacilityFaxSettingsSelect";
        private const string PROC_FACILITY_FAX_SETTING_UPDATE = "Provider.sp_FacilityFaxSettingsUpdate";
        private const string PROC_FACILITY_FAX_SETTING_DELETE = "Provider.sp_FacilityFaxSettingsDelete";

        private const string PROC_Facility_FAX_SETTING_USER_INSERT = "Provider.sp_FacilityFaxSettingsUserInsert";
        private const string PROC_Facility_FAX_SETTING_USER_SELECT = "Provider.sp_FaxSettingsUsersSelect";
        private const string PROC_Facility_FAX_SETTING_USER_DELETE = "Provider.sp_FacilityFaxSettingsUserDelete";
        private const string PROC_FACILITY_LOOKUP_BY_NAME = "Provider.sp_FacilitySelectLookup";

        #endregion

        #region "Parameters"
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_PHONE_EXT = "@PhoneExt";
        private const string PARM_FAX = "@Fax";
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_POS = "@POS";
        private const string PARM_FACILITY_TYPE_ID = "@FacilityTypeId";
        private const string PARM_LOCATION_ID = "@LocationId";
        private const string PARM_TAXONOMY_CODE = "@TaxonomyCode";
        private const string PARM_NPI = "@NPI";
        private const string PARM_REVENUE_CODE_ID = "@RevenueCodeId";
        private const string PARM_MAMMOGRAPHY_CERTIFICATE_NO = "@MammographyCertificateNo";
        private const string PARM_FEE_GROUP_ID = "@FeeGroupId";
        private const string PARM_BASIC_FEE_GROUP_ID = "@BasicFeeGroupId";
        private const string PARM_NOTES = "@Notes";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_ADDRESS = "@Address";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP_CODE = "@ZIPCode";
        private const string PARM_ZIP_CODE_EXT = "@ZIPCodeExt";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_WEBSITE_URL = "@WebSiteURL";
        private const string PARM_BILL_TO_PRACTICE = "@BillToPractice";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_STMT_GRP_ID = "@StmtGrpId";



        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_COLOR = "@Color";
        private const string PARM_ORG_ID = "@OrgId";
        private const string PARM_CLIA = "@CLIA";

        public struct Parameters
        {
            public int ID;
            public string FNAME;
            public string LNAME;
        }
        #endregion

        #region Constructors
        //private static DALFacility _instance = null;
        ///// <summary>
        ///// Singleton context
        ///// </summary>
        //public static DALFacility Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new DALFacility();
        //        return _instance;
        //    }
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="DALFacility"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALFacility()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }

        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALFacility(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);

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

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSProfile ds, Boolean IsInsert)
        {
            int i = 0;
            dbManager.CreateParameters(35);

            dbManager.AddParameters(i++, PARM_SHORT_NAME, ds.Facility.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_DESCRIPTION, ds.Facility.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_PHONE_NO, ds.Facility.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_PHONE_EXT, ds.Facility.PhoneExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_FAX, ds.Facility.FaxColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_PRACTICE_ID, ds.Facility.PracticeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(i++, PARM_POS, ds.Facility.POSIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(i++, PARM_FACILITY_TYPE_ID, ds.Facility.FacilityTypeIdColumn.ColumnName, DbType.Int64);


            dbManager.AddParameters(i++, PARM_LOCATION_ID, ds.Facility.LocationIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(i++, PARM_ORG_ID, ds.Facility.OrgIdColumn.ColumnName, DbType.String);

            dbManager.AddParameters(i++, PARM_TAXONOMY_CODE, ds.Facility.TaxonomyCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_NPI, ds.Facility.NPIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_REVENUE_CODE_ID, ds.Facility.RevenueCodeIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(i++, PARM_MAMMOGRAPHY_CERTIFICATE_NO, ds.Facility.MammographyCertificateNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_FEE_GROUP_ID, ds.Facility.FeeGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(i++, PARM_BASIC_FEE_GROUP_ID, ds.Facility.BasicFeeGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(i++, PARM_NOTES, ds.Facility.NotesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_START_DATE, ds.Facility.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(i++, PARM_ADDRESS, ds.Facility.AddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_CITY, ds.Facility.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_STATE, ds.Facility.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ZIP_CODE, ds.Facility.ZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_ZIP_CODE_EXT, ds.Facility.ZIPCodeExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_EMAIL_ADDRESS, ds.Facility.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_WEBSITE_URL, ds.Facility.WebSiteURLColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_BILL_TO_PRACTICE, ds.Facility.BillToPracticeColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(i++, PARM_IS_ACTIVE, ds.Facility.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(i++, PARM_CREATED_BY, ds.Facility.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_CREATED_ON, ds.Facility.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(i++, PARM_MODIFIED_BY, ds.Facility.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_MODIFIED_ON, ds.Facility.ModifiedOnColumn.ColumnName, DbType.DateTime);

            if (IsInsert == true)
                dbManager.AddParameters(i++, PARM_FACILITY_ID, ds.Facility.FacilityIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(i++, PARM_FACILITY_ID, ds.Facility.FacilityIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(i++, PARM_STMT_GRP_ID, ds.Facility.StmtGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(i++, PARM_COLOR, ds.Facility.ColorColumn.ColumnName, DbType.String);
            dbManager.AddParameters(i++, PARM_CLIA, ds.Facility.CLIAColumn.ColumnName, DbType.String);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the facility.
        /// </summary>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="Practice">The practice.</param>
        /// <param name="PlaceOfService">The place of service.</param>
        /// <returns></returns>
        public DSProfile LoadFacility(long FacilityId, string ShortName, string Description, string Practice, string PlaceOfService, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (Practice == "")
                    Practice = null;

                if (PlaceOfService == "")
                    PlaceOfService = null;

                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(11);

                if (FacilityId <= 0)
                    dbManager.AddParameters(0, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FACILITY_ID, FacilityId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_PRACTICE_ID, Practice);
                dbManager.AddParameters(4, PARM_POS, PlaceOfService);

                dbManager.AddParameters(5, PARM_USER_ID, MDVSession.Current.AppUserId);

                //if (SharedObj.IsAdmin)
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(7, PARM_IS_ACTIVE, Active);

                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.Practice.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_SELECT, ds, ds.Facility.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::LoadFacility", PROC_FACILITY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Loads the facility for Outgoing Referral.
        /// </summary>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="Practice">The practice.</param>
        /// <param name="PlaceOfService">The place of service.</param>
        /// <returns></returns>
        public DSProfile LoadFacilityOutgoingReferral(long FacilityId, string ShortName, string Description, string Practice, string PlaceOfService, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;
                if (Description == "")
                    Description = null;
                if (Practice == "")
                    Practice = null;
                if (PlaceOfService == "")
                    PlaceOfService = null;
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(11);
                if (FacilityId <= 0)
                    dbManager.AddParameters(0, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FACILITY_ID, FacilityId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_PRACTICE_ID, Practice);
                dbManager.AddParameters(4, PARM_POS, PlaceOfService);
                dbManager.AddParameters(5, PARM_USER_ID, null);
                dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                dbManager.AddParameters(7, PARM_IS_ACTIVE, Active);
                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.Practice.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_SELECT, ds, ds.Facility.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::LoadFacility", PROC_FACILITY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfile LoadProviderDiagnosticImagingFacility(long FacilityId, string ShortName, string Description, string Practice, string PlaceOfService, string Active, long ProviderId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;
                if (Description == "")
                    Description = null;
                if (Practice == "")
                    Practice = null;
                if (PlaceOfService == "")
                    PlaceOfService = null;
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(12);
                if (FacilityId <= 0)
                    dbManager.AddParameters(0, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FACILITY_ID, FacilityId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_PRACTICE_ID, Practice);
                dbManager.AddParameters(4, PARM_POS, PlaceOfService);
                dbManager.AddParameters(5, PARM_USER_ID, null);
                dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                dbManager.AddParameters(7, PARM_IS_ACTIVE, Active);
                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(10, PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds.Practice.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                
                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_DIAGNOSTICIMAGING_FACILITY_SELECT, ds, ds.Facility.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::LoadProviderDiagnosticImagingFacility", PROC_PROVIDER_DIAGNOSTICIMAGING_FACILITY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        //Begin Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277
        public DSProfileLookup LookupFacilityOutgoingReferral(string Active, string EntityId, long FacilityId = 0, string ShortName = "", string Description = "", string Practice = "", string PlaceOfService = "", int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;
                if (Description == "")
                    Description = null;
                if (Practice == "")
                    Practice = null;
                if (PlaceOfService == "")
                    PlaceOfService = null;
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(11);
                if (FacilityId <= 0)
                    dbManager.AddParameters(0, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FACILITY_ID, FacilityId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_PRACTICE_ID, Practice);
                dbManager.AddParameters(4, PARM_POS, PlaceOfService);
                dbManager.AddParameters(5, PARM_USER_ID, null);
                dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                dbManager.AddParameters(7, PARM_IS_ACTIVE, Active);
                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.Facility.FacilityIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_SELECT, ds, ds.Facility.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupFacilityOutgoingReferral", PROC_FACILITY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        //Begin Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="Practice">The practice.</param>
        /// <param name="PlaceOfService">The place of service.</param>
        /// <returns></returns>
        public DSProfile LoadFacility(SharedVariable SharedVariable, long FacilityId, string ShortName, string Description, string Practice, string PlaceOfService, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (Practice == "")
                    Practice = null;

                if (PlaceOfService == "")
                    PlaceOfService = null;

                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(11);

                if (FacilityId <= 0)
                    dbManager.AddParameters(0, PARM_FACILITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_FACILITY_ID, FacilityId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                dbManager.AddParameters(3, PARM_PRACTICE_ID, Practice);
                dbManager.AddParameters(4, PARM_POS, PlaceOfService);

                dbManager.AddParameters(5, PARM_USER_ID, SharedVariable.AppUserId);

                //if (SharedObj.IsAdmin)
                if (ClientConfiguration.DecryptFrom64(SharedVariable.UserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(6, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, SharedVariable.EntityId);

                dbManager.AddParameters(7, PARM_IS_ACTIVE, Active);

                if (PageNumber == 0)
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(8, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(9, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(10, PARM_RECORD_COUNT, ds.Practice.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_SELECT, ds, ds.Facility.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALFacility::LoadFacility", PROC_FACILITY_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Lookups the type of the facility.
        /// </summary>
        /// <returns></returns>
        public DSProfileLookup LookupFacilityType()
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //dbManager.CreateParameters(1);

                //if (POSId <= 0)
                //    dbManager.AddParameters(0, PARM_POS_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_POS_ID, POSId);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_TYPE_LOOKUP, ds, ds.FacilityType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupFacilityType", PROC_FACILITY_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfileLookup LookupLocation()
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOCATION_LOOKUP, ds, ds.Location.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupLocation", PROC_LOCATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }




        /// <summary>
        /// Lookups the facility.
        /// </summary>
        /// <returns></returns>
        public DSProfileLookup LookupFacility(string Active, string EntityId, string ShortName)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                if (string.IsNullOrEmpty(ShortName))
                    dbManager.AddParameters(3, PARM_SHORT_NAME, null);
                else
                    dbManager.AddParameters(3, PARM_SHORT_NAME, ShortName);

                if (EntityId != null && EntityId.IndexOf(",") > -1)
                {
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_ENTITY_LOOKUP, ds, ds.Facility.TableName);
                }
                else
                {
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_LOOKUP, ds, ds.Facility.TableName);
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupFacility", PROC_FACILITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfileLookup ProvidersDiagnosticImagingFacilityLookUp(Int64 providerId, string Active, string EntityId, string ShortName)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                if (string.IsNullOrEmpty(ShortName))
                    dbManager.AddParameters(3, PARM_SHORT_NAME, null);
                else
                    dbManager.AddParameters(3, PARM_SHORT_NAME, ShortName);

                dbManager.AddParameters(4, PARM_PROVIDER_ID, providerId);

                if (EntityId != null && EntityId.IndexOf(",") > -1)
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_DIAGNOSTICIMAGINGFACILITIES_ENTITY_LOOKUP, ds, ds.Facility.TableName);
                else
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_DIAGNOSTICIMAGINGFACILITIES_ENTITY_LOOKUP, ds, ds.Facility.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::ProviderDiagnosticImagingFacilitiesEntityLookup", PROC_PROVIDER_DIAGNOSTICIMAGINGFACILITIES_ENTITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfileLookup LookupFacilityDescription(string Active, string EntityId, string ShortName)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                if (string.IsNullOrEmpty(ShortName))
                    dbManager.AddParameters(3, PARM_SHORT_NAME, null);
                else
                    dbManager.AddParameters(3, PARM_SHORT_NAME, ShortName);

                if (EntityId != null && EntityId.IndexOf(",") > -1)
                {
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_ENTITY_LOOKUP, ds, ds.Facility.TableName);
                }
                else
                {
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_LOOKUP_DESCRIPTION, ds, ds.Facility.TableName);
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupFacility", PROC_FACILITY_LOOKUP_DESCRIPTION, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfileLookup LookupFacilitySchedular(string Active, string EntityId, string ShortName)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                if (string.IsNullOrEmpty(ShortName))
                    dbManager.AddParameters(3, PARM_SHORT_NAME, null);
                else
                    dbManager.AddParameters(3, PARM_SHORT_NAME, ShortName);

                if (EntityId != null && EntityId.IndexOf(",") > -1)
                {
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_ENTITY_LOOKUP_SCHEDULAR, ds, ds.Facility.TableName);
                }
                else
                {
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_LOOKUP_SCHEDULAR, ds, ds.Facility.TableName);
                }

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupFacility", PROC_FACILITY_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the facility.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile UpdateFacility(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Facility.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FACILITY_UPDATE, ds, ds.Facility.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Facility.Rows[0][ds.Facility.FacilityIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::UpdateFacility", PROC_FACILITY_UPDATE, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSProfileLookup LookupFacilityByName(string Searchstring)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Searchstring == "")
                    Searchstring = null;
                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_SHORT_NAME, Searchstring);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_LOOKUP_BY_NAME, ds, ds.Facility.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::LookupFacilityByName", PROC_FACILITY_LOOKUP_BY_NAME, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the facility.
        /// </summary>
        /// <param name="FacilityIds">The facility ids.</param>
        /// <returns></returns>
        public string DeleteFacility(string FacilityIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSProfile ds = LoadFacility(Convert.ToInt64(FacilityIds), null, null, null, null,null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Facility;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_FACILITY_ID, FacilityIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_FACILITY_DELETE);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FACILITY_DELETE).ToString();
                if (returnValue != "" && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.Facility.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Facility.Rows[0][ds.Facility.FacilityIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::DeleteFacility", PROC_FACILITY_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Inserts the facility.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile InsertFacility(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DALUsersActivity obj = new DALUsersActivity();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Facility.GetChanges();
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FACILITY_INSERT, ds, ds.Facility.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Facility.Rows[0][ds.Facility.FacilityIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_USER, true, "Insert User", ds.Tables[ds.Facility.TableName].Rows[0][ds.Facility.FacilityIdColumn.ColumnName].ToString());
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::InsertFacility", PROC_FACILITY_INSERT, ex);

                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        #region Facility Fax Settings

        public DSProfile InsertFacilityFaxSetting(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FacilityFaxSettings.GetChanges();
                dbManager.CreateParameters(13);
                dbManager.AddParameters(0, "@FacilityId", ds.FacilityFaxSettings.FacilityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, "@DisplayName", ds.FacilityFaxSettings.DisplayNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@ShortName", ds.FacilityFaxSettings.ShortNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, "@PhoneNo", ds.FacilityFaxSettings.PhoneNoColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@FaxNo", ds.FacilityFaxSettings.FaxNoColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, "@TimeZone", ds.FacilityFaxSettings.TimeZoneColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, "@HasCoverPage", ds.FacilityFaxSettings.HasCoverPageColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(7, "@CoverPage", ds.FacilityFaxSettings.CoverPageColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, "@CreatedBy", ds.FacilityFaxSettings.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(9, "@CreatedOn", ds.FacilityFaxSettings.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(10, "@ModifiedBy", ds.FacilityFaxSettings.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(11, "@ModifiedOn", ds.FacilityFaxSettings.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(12, "@FaxSettingsId", ds.FacilityFaxSettings.FacilityFaxSettingIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_FACILITY_FAX_SETTING_INSERT, ds, ds.FacilityFaxSettings.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FacilityFaxSettings.Rows[0][ds.FacilityFaxSettings.FacilityFaxSettingIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::InsertFacilityFaxSeTTING", PROC_FACILITY_FAX_SETTING_INSERT, ex);
                //throw ex;
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfile UpdateFacilityFaxSettings(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.FacilityFaxSettings.GetChanges();
                dbManager.CreateParameters(9);
                dbManager.AddParameters(0, "@FacilityId", ds.FacilityFaxSettings.FacilityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, "@DisplayName", ds.FacilityFaxSettings.DisplayNameColumn.ColumnName, DbType.String);
                //dbManager.AddParameters(2, "@ShortName", ds.FacilityFaxSettings.ShortNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@PhoneNo", ds.FacilityFaxSettings.PhoneNoColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, "@FaxNo", ds.FacilityFaxSettings.FaxNoColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@TimeZone", ds.FacilityFaxSettings.TimeZoneColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, "@HasCoverPage", ds.FacilityFaxSettings.HasCoverPageColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(6, "@CoverPage", ds.FacilityFaxSettings.CoverPageColumn.ColumnName, DbType.String);
                dbManager.AddParameters(7, "@ModifiedBy", ds.FacilityFaxSettings.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, "@ModifiedOn", ds.FacilityFaxSettings.ModifiedOnColumn.ColumnName, DbType.DateTime);
                ds = (DSProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_FACILITY_FAX_SETTING_UPDATE, ds, ds.FacilityFaxSettings.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.FacilityFaxSettings.Rows[0][ds.FacilityFaxSettings.FacilityFaxSettingIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::InsertFacilityFaxSeTTING", PROC_FACILITY_FAX_SETTING_INSERT, ex);
                //throw ex;
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfile LoadFacilityFaxSettings(long FacilityId, int PageNumber, int RowspPage)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, "@FacilityId", FacilityId);

                dbManager.AddParameters(1, "@UserId", MDVSession.Current.AppUserId);
                dbManager.AddParameters(2, "@EntityId", MDVSession.Current.EntityId);
                if (PageNumber == 0)
                {
                    dbManager.AddParameters(3, "@PageNumber", null);
                }
                else
                {
                    dbManager.AddParameters(3, "@PageNumber", PageNumber);
                }
                if (RowspPage == 0)
                {
                    dbManager.AddParameters(4, "@RowspPage", null);
                }
                else
                {
                    dbManager.AddParameters(4, "@RowspPage", RowspPage);
                }

                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_FACILITY_FAX_SETTING_SELECT, ds, ds.FacilityFaxSettings.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::LoadFacilityFaxSetting", PROC_FACILITY_FAX_SETTING_SELECT, ex);
                //throw ex
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteFacilityFaxSettings(long FacilityId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@FacilityId", FacilityId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_FACILITY_FAX_SETTING_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::DeleteFacilityFaxSettings", PROC_FACILITY_FAX_SETTING_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSProfile InsertFacilityFaxSettingUsers(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, "@FacilityId", ds.FacilityFaxSettingsUsers.FacilityIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, "@UserId", ds.FacilityFaxSettingsUsers.UserIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, "@CreatedBy", ds.FacilityFaxSettingsUsers.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, "@CreatedOn", ds.FacilityFaxSettingsUsers.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(4, "@ModifiedBy", ds.FacilityFaxSettingsUsers.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, "@ModifiedOn", ds.FacilityFaxSettingsUsers.ModifiedOnColumn.ColumnName, DbType.DateTime);

                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_Facility_FAX_SETTING_USER_INSERT, ds, ds.FacilityFaxSettingsUsers.TableName);

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::InsertFacilityFaxSettingsUsers", PROC_Facility_FAX_SETTING_USER_INSERT, ex);
                //throw ex;
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfile LoadFacilityFaxSettingsUsers(long FacilityId, long UserId, int PageNumber, int RowspPage)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

                dbManager.AddParameters(0, "@ProviderId", null);

                dbManager.AddParameters(1, "@FacilityId", FacilityId);

                dbManager.AddParameters(2, "@UserId", null);



                if (PageNumber == 0)
                {
                    dbManager.AddParameters(3, "@PageNumber", null);
                }
                else
                {
                    dbManager.AddParameters(3, "@PageNumber", PageNumber);
                }
                if (RowspPage == 0)
                {
                    dbManager.AddParameters(4, "@RowspPage", null);
                }
                else
                {
                    dbManager.AddParameters(4, "@RowspPage", RowspPage);
                }
                dbManager.AddParameters(5, "@IsProvider", false);
                dbManager.AddParameters(6, "@IsCompose", false);


                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Facility_FAX_SETTING_USER_SELECT, ds, ds.FacilityFaxSettingsUsers.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::LoadFacilityFaxSetting", PROC_Facility_FAX_SETTING_USER_SELECT, ex);
                //throw ex;
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string DeleteFacilityFaxSettingsUsers(long UserId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@FacilityId", null);
                dbManager.AddParameters(1, "@UserId", UserId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_Facility_FAX_SETTING_USER_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALFacility::DeleteFacilityFaxSettings", PROC_Facility_FAX_SETTING_USER_DELETE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion




        #endregion
    }
}
