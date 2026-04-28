using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.ComponentModel;
using System.Data;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Model.Lookups;
using System.Data.SqlClient;
using MDVision.Model.Admin.Provider;
using MDVision.Model.Clinical.Reports;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALProvider
    {
        #region Variable

        #endregion
        #region " Stored Procedure Names"
        private const string PROC_PROVIDER_INSERT = "Provider.sp_ProviderInsert";
        private const string PROC_REFPROVIDER_LOOKUP_BY_NAME = "Provider.sp_ReferringProviderSelectLookup";
        private const string PROC_PROVIDER_UPDATE = "Provider.sp_ProviderUpdate";
        private const string PROC_UPDATEPROVIDER_ISACTIVE = "Provider.UpdateProviderIsActive";
        private const string PROC_PROVIDER_DELETE = "Provider.sp_ProviderDelete";
        private const string PROC_PROVIDER_SELECT = "Provider.sp_ProviderSelect";
        private const string PROC_PROVIDER_TYPE_LOOKUP = "Provider.sp_ProviderTypeLookup";
        private const string PROC_PROFILE_TYPE_LOOKUP = "Provider.sp_ProfileTypeLookup";
        private const string PROC_PROVIDER_LOOKUP = "Provider.sp_ProviderLookup";
        private const string PROC_PROVIDER_WITH_QUALIFICATION_LOOKUP = "Provider.sp_ProviderLookupWithQualification";

        private const string PROC_PROVIDERBASED_SPECIALTY_LOOKUP = "Provider.sp_ProviderBasedSpecialtyLookup";
        private const string PROC_NOTES_PROVIDERS = "Provider.sp_NotesProviderLookup";
        private const string PROC_ANESTHESIOLOGIST_LOOKUP = "Provider.sp_AnesthesiologistLookup";
        private const string PROC_CRNA_LOOKUP = "Provider.sp_CRNALookup";
        private const string PROC_PROVIDER_ENTITY_LOOKUP = "Provider.sp_ProviderEntityLookup";
        private const string PROC_PROVIDER_LICENSE_INFO_DELETE = "Provider.sp_ProvidersLicenseInfoDelete";
        private const string PROC_PROVIDER_LICENSE_INFO_INSERT = "Provider.sp_ProvidersLicenseInfoInsert";
        private const string PROC_PROVIDER_LICENSE_INFO_SELECT = "Provider.sp_ProvidersLicenseInfoSelect";
        private const string PROC_PROVIDER_LICENSE_INFO_UPDATE = "Provider.sp_ProvidersLicenseInfoUpdate";
        private const string PROC_PROVIDER_LOOKUP_BY_NAME = "Provider.sp_ProvidersSelectLookup";
        private const string PROC_PROVIDER_CPT_DELETE = "Provider.sp_ProviderCPTDelete";
        private const string PROC_GET_PROVIDER_CPTS = "Provider.sp_ProviderCPTsSelect";

        // Fax Settings
        private const string PROC_PROVIDER_FAX_SETTING_INSERT = "Provider.sp_ProviderFaxSettingsInsert";
        private const string PROC_PROVIDER_FAX_SETTING_SELECT = "Provider.sp_ProviderFaxSettingsSelect";
        private const string PROC_PROVIDER_FAX_SETTING_UPDATE = "Provider.sp_ProviderFaxSettingsUpdate";
        private const string PROC_PROVIDER_FAX_SETTING_DELETE = "Provider.sp_ProviderFaxSettingsDelete";

        // Users
        private const string PROC_PROVIDER_FAX_SETTING_USER_INSERT = "Provider.sp_ProviderFaxSettingsUserInsert";
        private const string PROC_PROVIDER_FAX_SETTING_USER_SELECT = "Provider.sp_FaxSettingsUsersSelect";
        private const string PROC_PROVIDER_FAX_SETTING_USER_DELETE = "Provider.sp_ProviderFaxSettingsUserDelete";
        private const string PROC_ALLPROVIDER_LOOKUP = "Provider.sp_AllProviders";
        private const string PROC_PROVIDER_SELECT_ENTITY_BASED = "Provider.sp_EntityBasedProviderSelect";
        private const string PROC_PROVIDER_LUSELECT_ENTITY_BASED = "Provider.sp_ProviderLookupEntityBased";

        private const string PROC_SP_LOOKUP = "Clinical.sp_Lookup";

        #endregion

        #region "Parameters"
        private const string PARM_PROVIDER_ID = "@providerId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_MIDDLE_INITIAL = "@MiddleInitial";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_ALIAS = "@Alias";
        private const string PARM_SPECIALITY = "@SpecialtyId";
        private const string PARM_QUALIFICATION = "@Qualification";
        private const string PARM_GENDER = "@Gender";
        private const string PARM_NPI = "@NPI";
        private const string PARM_PROFILE_TYPE = "@ProfileType";
        private const string PARM_SSN = "@SSN";
        private const string PARM_DEA = "@DEA";
        private const string PARM_CLIA = "@CLIA";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_PHONE_EXT = "@PhoneExt";
        private const string PARM_FAX = "@Fax";
        private const string PARM_CELL_NO = "@CellNo";
        private const string PARM_TAXONOMY_CODE = "@TaxonomyCode";
        private const string PARM_HOME_ADDRESS = "@HomeAddress";
        private const string PARM_OFFICE_ADDRESS = "@OfficeAddress";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP_CODE = "@ZIPCode";
        private const string PARM_ZIP_CODE_EXT = "@ZIPCodeExt";
        private const string PARM_COUNTRY = "@Country";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_WEBSITE_URL = "@WebSiteURL";
        private const string PARM_COMMENTS = "@Comments";
        private const string PARM_BASIC_FEE_GROUP_ID = "@BasicFeeGroupId";
        private const string PARM_SUPERVISING_PROVIDER_ID = "@SupervisingProviderId";
        private const string PARM_PROVIDER_TYPE = "@ProviderType";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_USER_ID = "@UserId";

        private const string PARM_PROVIDER_LICENSE_ID = "@ProviderLicenseId";
        //private const string PARM_PROVIDER_ID = "@ProviderId";
        //private const string PARM_STATE = "@State";
        private const string PARM_LICENSE_NO = "@LicenseNo";

        private const string PARM_FEE_GROUP_ID = "@FeeGroupId";
        private const string PARM_IS_SPECIALIST = "@IsSpecialist";
        private const string PARM_IS_SCRUB_CLAIM = "@IsScrubClaim";
        private const string PARM_TIN = "@TIN";

        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";


        //start  |  added by Talha Tanweer 22 july 2016
        private const string PARM_IS_E_SIGNATURED = "@Is_eSignatured";
        private const string PARM_IS_NOTE_SIGN_WO_CPT_CODE = "@IsNoteSignWOCPTCode";
        private const string PARM_IS_NOTE_SIGN_WO_ICD_CODE = "@IsNoteSignWOICDCode";
        private const string PARM_E_SIGNATURE = "@eSignature";
        //End    |  added by Talha Tanweer 22 july 2016

        private const string PARM_REPORT_HEADER = "@ReportHeaderId";
        private const string PARM_RCOPIA_USERNAME = "@RcopiaUserName";
        private const string PARM_VISIT_DURATION_GROUP_ID = "@VisitDurationGroupId";
        private const string PARM_PROVIDER_CPTS_XML = "@ProviderCPTsXML";
        private const string PARM_PROVIDER_CPT_ID = "@ProviderCPTId";
        private const string PARM_FACILITY_IDs = "@FacilityIds";
        private const string LOOKUP_TYPE = "@LookupType";
        private const string PARM_BULKSIGN_IDS = "@BulkSign";
        private const string PARM_MEANINGFUL_USE = "@MeaningfulUse";
        private const string PARM_NEW_PATIENT_COLOR = "@NewPatientColor";
        private const string PARM_ESTABLISHED_PATIENT_COLOR = "@EstablishedPatientColor";
        private const string PARM_BULK_SIGN_GRIDSHOW = "@BulkSignGridShow";
        public struct Parameters
        {
            public int ID;
            public string FNAME;
            public string LNAME;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALProvider"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALProvider()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALProvider(SharedVariable SharedVariable)
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
        private void CreateParameters(IDBManager dbManager, DSProfile ds, Boolean IsInsert, ref DataTable dtFacility, ref DataTable dtBulkSignException)
        {
            dbManager.CreateParameters(56);

            dbManager.AddParameters(0, PARM_SHORT_NAME, ds.Provider.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(1, PARM_FIRST_NAME, ds.Provider.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_MIDDLE_INITIAL, ds.Provider.MiddleInitialColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_LAST_NAME, ds.Provider.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_ALIAS, ds.Provider.AliasColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_SPECIALITY, ds.Provider.SpecialtyIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_QUALIFICATION, ds.Provider.QualificationColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_GENDER, ds.Provider.GenderColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(8, PARM_NPI, ds.Provider.NPIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_PROFILE_TYPE, ds.Provider.ProfileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_SSN, ds.Provider.SSNColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_DEA, ds.Provider.DEAColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CLIA, ds.Provider.CLIAColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ENTITY_ID, ds.Provider.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(14, PARM_PHONE_NO, ds.Provider.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_PHONE_EXT, ds.Provider.PhoneExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_FAX, ds.Provider.FaxColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_CELL_NO, ds.Provider.CellNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_TAXONOMY_CODE, ds.Provider.TaxonomyCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_HOME_ADDRESS, ds.Provider.HomeAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_OFFICE_ADDRESS, ds.Provider.OfficeAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_CITY, ds.Provider.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_STATE, ds.Provider.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_ZIP_CODE, ds.Provider.ZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_ZIP_CODE_EXT, ds.Provider.ZIPCodeExtColumn.ColumnName, DbType.String);
            //dbManager.AddParameters(25, PARM_COUNTRY, ds.Provider.CountryColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_EMAIL_ADDRESS, ds.Provider.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_WEBSITE_URL, ds.Provider.WebSiteURLColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_COMMENTS, ds.Provider.CommentsColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_BASIC_FEE_GROUP_ID, ds.Provider.BasicFeeGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(29, PARM_SUPERVISING_PROVIDER_ID, ds.Provider.SupervisingProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(30, PARM_IS_ACTIVE, ds.Provider.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(31, PARM_CREATED_BY, ds.Provider.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(32, PARM_CREATED_ON, ds.Provider.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(33, PARM_MODIFIED_BY, ds.Provider.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_MODIFIED_ON, ds.Provider.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(35, PARM_PROVIDER_TYPE, ds.Provider.ProviderTypeColumn.ColumnName, DbType.String);
            if (IsInsert == true)
                dbManager.AddParameters(36, PARM_PROVIDER_ID, ds.Provider.ProviderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(36, PARM_PROVIDER_ID, ds.Provider.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(37, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            dbManager.AddParameters(38, PARM_FEE_GROUP_ID, ds.Provider.FeeGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(39, PARM_IS_SPECIALIST, ds.Provider.IsSpecialistColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(40, PARM_IS_SCRUB_CLAIM, ds.Provider.IsScrubClaimColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(41, PARM_TIN, ds.Provider.TINColumn.ColumnName, DbType.String);

            // Start   ||  Talha Tanweer || 22 july 2016
            dbManager.AddParameters(42, PARM_IS_E_SIGNATURED, ds.Provider.Is_eSignaturedColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(43, PARM_E_SIGNATURE, ds.Provider.eSignatureColumn.ColumnName, DbType.Binary);
            // End     ||  Talha Tanweer || 22 july 2016

            dbManager.AddParameters(44, PARM_REPORT_HEADER, ds.Provider.ReportHeaderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(45, PARM_RCOPIA_USERNAME, ds.Provider.RcopiaUserNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(46, PARM_VISIT_DURATION_GROUP_ID, ds.Provider.VisitDurationGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(47, PARM_PROVIDER_CPTS_XML, ds.Provider.ProviderCPTsXMLColumn.ColumnName, DbType.String);
            dbManager.AddParameters(48, PARM_FACILITY_IDs, dtFacility);
            dbManager.AddParameters(49, PARM_IS_NOTE_SIGN_WO_CPT_CODE, ds.Provider.IsNoteSignWOCPTCodeColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(50, PARM_IS_NOTE_SIGN_WO_ICD_CODE, ds.Provider.IsNoteSignWOICDCodeColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(51, PARM_BULKSIGN_IDS, dtBulkSignException);
            dbManager.AddParameters(52, PARM_MEANINGFUL_USE, ds.Provider.MeaningfulUseColumn.ColumnName, DbType.Int32);
            dbManager.AddParameters(53, PARM_NEW_PATIENT_COLOR, ds.Provider.NewPatientColorColumn.ColumnName, DbType.String);
            dbManager.AddParameters(54, PARM_ESTABLISHED_PATIENT_COLOR, ds.Provider.EstablishedPatientColorColumn.ColumnName, DbType.String);
            dbManager.AddParameters(55, PARM_BULK_SIGN_GRIDSHOW, ds.Provider.BulkSignGridShowColumn.ColumnName, DbType.Boolean);
        }
        private void UpdateProviderIsActiveParameters(IDBManager dbManager, DSProfile ds)
        {
            dbManager.CreateParameters(3);
            dbManager.AddParameters(0, PARM_PROVIDER_ID, ds.Provider.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_IS_ACTIVE, ds.Provider.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(2, PARM_MODIFIED_BY, ds.Provider.ModifiedByColumn.ColumnName, DbType.String);
        }

        /// <summary>
        /// Creates the parameters_ provider_ license_ information.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters_Provider_License_Info(IDBManager dbManager, DSProfile ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(4);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_PROVIDER_LICENSE_ID, ds.ProvidersLicenseInfo.ProviderLicenseIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_PROVIDER_LICENSE_ID, ds.ProvidersLicenseInfo.ProviderLicenseIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_PROVIDER_ID, ds.ProvidersLicenseInfo.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_STATE, ds.ProvidersLicenseInfo.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_LICENSE_NO, ds.ProvidersLicenseInfo.LicenseNoColumn.ColumnName, DbType.String);
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
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
        public DSProfile LoadProvider(long ProviderId, string ShortName, string FirstName, string LastName, string SpecialityId, string NPI, string EntityId, string Active, int PageNumber = 1, int RowsPerPage = 1000, string ParentCtrl = "")
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (FirstName == "")
                    FirstName = null;

                if (LastName == "")
                    LastName = null;

                if (SpecialityId == "")
                    SpecialityId = null;

                if (NPI == "")
                    NPI = null;

                if (EntityId == "")
                    EntityId = null;

                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(12);

                if (ProviderId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(3, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(4, PARM_SPECIALITY, SpecialityId);
                dbManager.AddParameters(5, PARM_NPI, NPI);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(6, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, EntityId);

                if (string.Equals(ParentCtrl, "Patient_Referrals_Outgoing_Detail"))
                {
                    dbManager.AddParameters(7, PARM_USER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                }

                dbManager.AddParameters(8, PARM_IS_ACTIVE, Active);

                if (PageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(10, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds.Provider.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                List<string> tableNames = new List<string>();
                tableNames.Add(ds.Provider.TableName);
                tableNames.Add(ds.ProviderDiagnosticImagingFacilities.TableName);

                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_SELECT, ds, tableNames);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LoadProvider", PROC_PROVIDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public List<ProviderModel> GetProviderCpts(long ProviderId)
        {
            ProviderModel providerCPTs = new ProviderModel();
            List<ProviderModel> providerCPTsList = new List<ProviderModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;

            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (ProviderId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_GET_PROVIDER_CPTS);

                ProviderCPTs model = null;
                while (reader.Read())
                {
                    model = new ProviderCPTs();
                    model.CPTId = !String.IsNullOrEmpty(reader["Id"].ToString()) ? reader["Id"].ToString() : "";
                    model.CPTCode = !String.IsNullOrEmpty(reader["CPTCode"].ToString()) ? reader["CPTCode"].ToString() : "";
                    model.CPTCodeDescription = !String.IsNullOrEmpty(reader["CPT_Description"].ToString()) ? reader["CPT_Description"].ToString() : "";
                    model.SNOMEDID = !String.IsNullOrEmpty(reader["SNOMEDID"].ToString()) ? reader["SNOMEDID"].ToString() : "";
                    model.SNOMED_Description = !String.IsNullOrEmpty(reader["SNOMED_Description"].ToString()) ? reader["SNOMED_Description"].ToString() : "";
                    providerCPTs.ListProviderCPTs.Add(model);
                }
                providerCPTsList.Add(providerCPTs);
                return providerCPTsList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::GetProviderCpts", PROC_GET_PROVIDER_CPTS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfileLookup LookupProviderByName(string Searchstring)
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

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_LOOKUP_BY_NAME, ds, ds.Provider.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LookupProviderByName", PROC_PROVIDER_LOOKUP_BY_NAME, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfileLookup LookupRefProviderByName(string Searchstring)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Searchstring == "")
                    Searchstring = null;
                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_SHORT_NAME, Searchstring);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);
                dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFPROVIDER_LOOKUP_BY_NAME, ds, ds.ReferringProvider.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LookupRefProviderByName", PROC_REFPROVIDER_LOOKUP_BY_NAME, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfile LoadProviderEntityBased(long ProviderId, string ShortName, string FirstName, string LastName, string SpecialityId, string NPI, string EntityId, string Active, int PageNumber = 1, int RowsPerPage = 1000, string ParentCtrl = "")
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (FirstName == "")
                    FirstName = null;

                if (LastName == "")
                    LastName = null;

                if (SpecialityId == "")
                    SpecialityId = null;

                if (NPI == "")
                    NPI = null;

                if (EntityId == "")
                    EntityId = null;

                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(12);

                if (ProviderId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(3, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(4, PARM_SPECIALITY, SpecialityId);
                dbManager.AddParameters(5, PARM_NPI, NPI);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(6, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(6, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, EntityId);

                if (string.Equals(ParentCtrl, "Patient_Referrals_Outgoing_Detail"))
                {
                    dbManager.AddParameters(7, PARM_USER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_USER_ID, MDVSession.Current.AppUserId);
                }

                dbManager.AddParameters(8, PARM_IS_ACTIVE, Active);

                if (PageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(10, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds.Provider.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_SELECT_ENTITY_BASED, ds, ds.Provider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LoadProvider", PROC_PROVIDER_SELECT_ENTITY_BASED, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// This Method is for Windows service to pass SharedVabiables directly
        /// </summary>
        /// <param name="SharedVariable"> Variables that are no in MDVSession Context </param>
        /// <param name="ProviderId"></param>
        /// <param name="ShortName"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="SpecialityId"></param>
        /// <param name="NPI"></param>
        /// <param name="EntityId"></param>
        /// <param name="Active"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPerPage"></param>
        /// <returns></returns>
        public DSProfile LoadProvider(SharedVariable SharedVariable, long ProviderId, string ShortName, string FirstName, string LastName, string SpecialityId, string NPI, string EntityId, string Active, int PageNumber = 1, int RowsPerPage = 1000, string ParentCtrl = "")
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                if (ShortName == "")
                    ShortName = null;

                if (FirstName == "")
                    FirstName = null;

                if (LastName == "")
                    LastName = null;

                if (SpecialityId == "")
                    SpecialityId = null;

                if (NPI == "")
                    NPI = null;

                if (EntityId == "")
                    EntityId = null;

                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(12);

                if (ProviderId == 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(3, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(4, PARM_SPECIALITY, SpecialityId);
                dbManager.AddParameters(5, PARM_NPI, NPI);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(SharedVariable.UserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(6, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(6, PARM_ENTITY_ID, SharedVariable.EntityId);
                }
                else
                    dbManager.AddParameters(6, PARM_ENTITY_ID, EntityId);

                if (string.Equals(ParentCtrl, "Patient_Referrals_Outgoing_Detail"))
                {
                    dbManager.AddParameters(7, PARM_USER_ID, null);
                }
                else
                {
                    dbManager.AddParameters(7, PARM_USER_ID, SharedVariable.AppUserId);
                }

                dbManager.AddParameters(8, PARM_IS_ACTIVE, Active);

                if (PageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(10, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds.Provider.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_SELECT, ds, ds.Provider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALProvider::LoadProvider", PROC_PROVIDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        /// <summary>
        /// Updates the provider.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile UpdateProvider(ref DSProfile ds, ref DataTable dtFacility, ref DataTable dtBulkSignException)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Provider.GetChanges();
                this.CreateParameters(dbManager, ds, false, ref dtFacility, ref dtBulkSignException);

                ds = (DSProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROVIDER_UPDATE, ds, ds.Provider.TableName);
                ds.AcceptChanges();

                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Provider.Rows[0][ds.Provider.ProviderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::UpdateProvider", PROC_PROVIDER_UPDATE, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfile UpdateProviderIsActive(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.UpdateProviderIsActiveParameters(dbManager, ds);
                ds = (DSProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_UPDATEPROVIDER_ISACTIVE, ds, ds.Provider.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::UpdateProviderIsActive", PROC_UPDATEPROVIDER_ISACTIVE, ex);
                throw ex;     
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the provider.
        /// </summary>
        /// <param name="ProviderIds">The provider ids.</param>
        /// <returns></returns>
        public string DeleteProvider(string ProviderIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSProfile ds = LoadProvider(Convert.ToInt64(ProviderIds), null, null, null, null, null,null,null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Provider;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PROVIDER_DELETE);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROVIDER_DELETE).ToString();
                if (returnValue != "" && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.Provider.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Provider.Rows[0][ds.Provider.ProviderIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::DeleteProvider", PROC_PROVIDER_DELETE, ex);
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
        public string DeleteProviderFaxSettings(long ProviderId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, "@ProviderId", ProviderId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROVIDER_FAX_SETTING_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::DeleteProviderFaxSettings", PROC_PROVIDER_FAX_SETTING_DELETE, ex);
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
        public string DeleteProviderFaxSettingsUsers(long UserId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, "@ProviderId", null);
                dbManager.AddParameters(1, "@UserId", UserId);
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROVIDER_FAX_SETTING_USER_DELETE).ToString();
                if (returnValue != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::DeleteProviderFaxSettings", PROC_PROVIDER_FAX_SETTING_USER_DELETE, ex);
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
        public string DeleteAssociatedProcedure(string ProcedureListId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PROVIDER_CPT_ID, ProcedureListId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROVIDER_CPT_DELETE).ToString();

                if (returnValue != "" && returnValue.ToString() != "")
                    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::DeleteAssociatedProcedure", PROC_PROVIDER_CPT_DELETE, ex);
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
        /// Inserts the provider.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile InsertProvider(ref DSProfile ds, ref DataTable dtFacility, ref DataTable dtBulkSignException)
        {
            //DALUsersActivity obj = new DALUsersActivity();
            //DSDBAudit dsDBAudit = new DSDBAudit();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DataTable dtTemp = ds.Provider.GetChanges();
                dbManager.Open();
                CreateParameters(dbManager, ds, true, ref dtFacility, ref dtBulkSignException);
                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROVIDER_INSERT, ds, ds.Provider.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Provider.Rows[0][ds.Provider.ProviderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_USER, true, "Insert User", ds.Tables[ds.Provider.TableName].Rows[0][ds.Provider.ProviderIdColumn.ColumnName].ToString());
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::InsertProvider", PROC_PROVIDER_INSERT, ex);
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
        public DSProfile InsertProviderFaxSetting(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ProviderFaxSettings.GetChanges();
                dbManager.Open();
                dbManager.CreateParameters(18);
                dbManager.AddParameters(0, "@ProviderId", ds.ProviderFaxSettings.ProviderIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, "@DisplayName", ds.ProviderFaxSettings.DisplayNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@FirstName", ds.ProviderFaxSettings.FirstNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, "@MiddleName", ds.ProviderFaxSettings.MiddleNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@LastName", ds.ProviderFaxSettings.LastNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, "@CompanyName", ds.ProviderFaxSettings.CompanyNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, "@PhoneNo", ds.ProviderFaxSettings.PhoneNoColumn.ColumnName, DbType.String);
                dbManager.AddParameters(7, "@FaxNo", ds.ProviderFaxSettings.FaxNoColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, "@TimeZone", ds.ProviderFaxSettings.TimeZoneColumn.ColumnName, DbType.String);
                dbManager.AddParameters(9, "@HasCoverPage", ds.ProviderFaxSettings.HasCoverPageColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(10, "@CoverPage", ds.ProviderFaxSettings.CoverPageColumn.ColumnName, DbType.String);
                dbManager.AddParameters(11, "@Is_eSignatured", ds.ProviderFaxSettings.Is_eSignaturedColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(12, "@eSignature", ds.ProviderFaxSettings.eSignatureColumn.ColumnName, DbType.Binary);
                dbManager.AddParameters(13, "@CreatedBy", ds.ProviderFaxSettings.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(14, "@CreatedOn", ds.ProviderFaxSettings.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(15, "@ModifiedBy", ds.ProviderFaxSettings.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(16, "@ModifiedOn", ds.ProviderFaxSettings.ModifiedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(17, "@FaxSettingsId", ds.ProviderFaxSettings.FaxSettingIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROVIDER_FAX_SETTING_INSERT, ds, ds.ProviderFaxSettings.TableName);

                ds.AcceptChanges();

                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ProviderFaxSettings.Rows[0][ds.ProviderFaxSettings.FaxSettingIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::InsertProviderFaxSeTTING", PROC_PROVIDER_FAX_SETTING_INSERT, ex);
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

        public DSProfile InsertProviderFaxSettingUsers(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(6);
                dbManager.AddParameters(0, "@ProviderId", ds.ProviderFaxSettingsUsers.ProviderIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, "@UserId", ds.ProviderFaxSettingsUsers.UserIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(2, "@CreatedBy", ds.ProviderFaxSettingsUsers.CreatedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, "@CreatedOn", ds.ProviderFaxSettingsUsers.CreatedOnColumn.ColumnName, DbType.DateTime);
                dbManager.AddParameters(4, "@ModifiedBy", ds.ProviderFaxSettingsUsers.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, "@ModifiedOn", ds.ProviderFaxSettingsUsers.ModifiedOnColumn.ColumnName, DbType.DateTime);

                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROVIDER_FAX_SETTING_USER_INSERT, ds, ds.ProviderFaxSettingsUsers.TableName);

                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::InsertProviderFaxSettingsUsers", PROC_PROVIDER_FAX_SETTING_USER_INSERT, ex);
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






        public DSProfile UpdateProviderFaxSettings(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ProviderFaxSettings.GetChanges();
                dbManager.CreateParameters(15);
                dbManager.AddParameters(0, "@ProviderId", ds.ProviderFaxSettings.ProviderIdColumn.ColumnName, DbType.Int64);
                dbManager.AddParameters(1, "@DisplayName", ds.ProviderFaxSettings.DisplayNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(2, "@FirstName", ds.ProviderFaxSettings.FirstNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(3, "@MiddleName", ds.ProviderFaxSettings.MiddleNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(4, "@LastName", ds.ProviderFaxSettings.LastNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(5, "@CompanyName", ds.ProviderFaxSettings.CompanyNameColumn.ColumnName, DbType.String);
                dbManager.AddParameters(6, "@PhoneNo", ds.ProviderFaxSettings.PhoneNoColumn.ColumnName, DbType.String);
                dbManager.AddParameters(7, "@FaxNo", ds.ProviderFaxSettings.FaxNoColumn.ColumnName, DbType.String);
                dbManager.AddParameters(8, "@TimeZone", ds.ProviderFaxSettings.TimeZoneColumn.ColumnName, DbType.String);
                dbManager.AddParameters(9, "@HasCoverPage", ds.ProviderFaxSettings.HasCoverPageColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(10, "@CoverPage", ds.ProviderFaxSettings.CoverPageColumn.ColumnName, DbType.String);
                dbManager.AddParameters(11, "@Is_eSignatured", ds.ProviderFaxSettings.Is_eSignaturedColumn.ColumnName, DbType.Boolean);
                dbManager.AddParameters(12, "@eSignature", ds.ProviderFaxSettings.eSignatureColumn.ColumnName, DbType.Binary);
                dbManager.AddParameters(13, "@ModifiedBy", ds.ProviderFaxSettings.ModifiedByColumn.ColumnName, DbType.String);
                dbManager.AddParameters(14, "@ModifiedOn", ds.ProviderFaxSettings.ModifiedOnColumn.ColumnName, DbType.DateTime);
                ds = (DSProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROVIDER_FAX_SETTING_UPDATE, ds, ds.ProviderFaxSettings.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ProviderFaxSettings.Rows[0][ds.ProviderFaxSettings.FaxSettingIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::InsertProviderFaxSeTTING", PROC_PROVIDER_FAX_SETTING_INSERT, ex);
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
        public DSProfile LoadProviderFaxSettings(long ProviderId, int PageNumber, int RowspPage)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, "@ProviderId", ProviderId);
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

                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_FAX_SETTING_SELECT, ds, ds.ProviderFaxSettings.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LoadProviderFaxSetting", PROC_PROVIDER_FAX_SETTING_SELECT, ex);
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
        public DSProfile LoadProviderFaxSettingsUsers(long ProviderId, long UserId, int PageNumber, int RowspPage)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(7);

                dbManager.AddParameters(0, "@ProviderId", ProviderId);

                dbManager.AddParameters(1, "@FacilityId", null);

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
                dbManager.AddParameters(5, "@IsProvider", true);
                dbManager.AddParameters(6, "@IsCompose", false);


                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_FAX_SETTING_USER_SELECT, ds, ds.ProviderFaxSettingsUsers.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LoadProviderFaxSetting", PROC_PROVIDER_FAX_SETTING_USER_SELECT, ex);
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
        /// <summary>
        /// Lookups the type of the provider.
        /// </summary>
        /// <returns></returns>
        public DSProfileLookup LookupProviderType()
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_TYPE_LOOKUP, ds, ds.ProviderType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LookupProviderType", PROC_PROVIDER_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the type of the profile.
        /// </summary>
        /// <returns></returns>
        public DSProfileLookup LookupProfileType()
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROFILE_TYPE_LOOKUP, ds, ds.ProfileType.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupProfileType", PROC_PROFILE_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Lookups the provider.
        /// </summary>
        /// <returns></returns>
        public DSProfileLookup LookupProvider(string Active, bool TIN, bool isprovider, string ShortName)
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
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                if(string.IsNullOrEmpty(ShortName))
                    dbManager.AddParameters(3, PARM_SHORT_NAME, null);
                else
                    dbManager.AddParameters(3, PARM_SHORT_NAME, ShortName);

                dbManager.AddParameters(4, PARM_TIN, TIN);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_LOOKUP, ds, ds.Provider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupProvider", PROC_PROVIDER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfileLookup LookupProviderWithQualification(string Active, bool TIN, bool isprovider, string ShortName)
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
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                if (string.IsNullOrEmpty(ShortName))
                    dbManager.AddParameters(3, PARM_SHORT_NAME, null);
                else
                    dbManager.AddParameters(3, PARM_SHORT_NAME, ShortName);

                dbManager.AddParameters(4, PARM_TIN, TIN);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_WITH_QUALIFICATION_LOOKUP, ds, ds.Provider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupProviderWithQualification", PROC_PROVIDER_WITH_QUALIFICATION_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfileLookup LookupAllProviders(string Active)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ALLPROVIDER_LOOKUP, ds, ds.Provider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupAllProviders", PROC_ALLPROVIDER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfileLookup LookupAnesthesiologist(string Active, bool TIN)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;


                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ANESTHESIOLOGIST_LOOKUP, ds, ds.Provider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupAnesthesiologist", PROC_ANESTHESIOLOGIST_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfileLookup LookupCRNA(string Active, bool TIN)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;


                dbManager.Open();
                dbManager.CreateParameters(3);

                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CRNA_LOOKUP, ds, ds.Provider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupCRNA", PROC_CRNA_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfileLookup LookupProviderEntityBased(string Active, bool TIN, string EntityIds)
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

                if (EntityIds.IndexOf(',') < 0)
                {
                    if (string.IsNullOrEmpty(EntityIds) || Convert.ToInt32(EntityIds) < 1)
                        dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, EntityIds);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_ENTITY_ID, EntityIds);
                }

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);
                dbManager.AddParameters(3, PARM_TIN, TIN);
                if (EntityIds.IndexOf(',') > 0)
                {
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_LUSELECT_ENTITY_BASED, ds, ds.Provider.TableName);
                }
                else
                {
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_LUSELECT_ENTITY_BASED, ds, ds.Provider.TableName);
                }

                return ds;
            }
            catch (Exception ex)
            {
                if (EntityIds.IndexOf(',') > 0)
                {
                    MDVLogger.DALErrorLog("DALProfile::LookupProvider", PROC_PROVIDER_ENTITY_LOOKUP, ex);
                }
                else
                {
                    MDVLogger.DALErrorLog("DALProfile::LookupProvider", PROC_PROVIDER_LOOKUP, ex);
                }
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
             
        public DSProfileLookup LookupProvider(string Active, bool TIN, string EntityIds)
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

                if (EntityIds.IndexOf(',') < 0)
                {
                    if (string.IsNullOrEmpty(EntityIds) || Convert.ToInt32(EntityIds) < 1)
                        dbManager.AddParameters(1, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY_ID, EntityIds);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_ENTITY_ID, EntityIds);
                }

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);
                dbManager.AddParameters(3, PARM_TIN, TIN);
                if (EntityIds.IndexOf(',') > 0)
                {
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_ENTITY_LOOKUP, ds, ds.Provider.TableName);
                }
                else
                {
                    ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_LOOKUP, ds, ds.Provider.TableName);
                }

                return ds;
            }
            catch (Exception ex)
            {
                if (EntityIds.IndexOf(',') > 0)
                {
                    MDVLogger.DALErrorLog("DALProfile::LookupProvider", PROC_PROVIDER_ENTITY_LOOKUP, ex);
                }
                else
                {
                    MDVLogger.DALErrorLog("DALProfile::LookupProvider", PROC_PROVIDER_LOOKUP, ex);
                }
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfileLookup LookupProviderBasedSpecialty(string Active,string ProviderId)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                
                dbManager.Open();
                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDERBASED_SPECIALTY_LOOKUP, ds, ds.Specialty.TableName);
             

                return ds;
            }
            catch (Exception ex)
            {

                MDVLogger.DALErrorLog("DALProfile::LookupProvider", PROC_PROVIDERBASED_SPECIALTY_LOOKUP, ex);
               
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public List<ProfileLookupModel> LookupNotesProviders(string PatientId)
        {
            List<ProfileLookupModel> ProvidersList = new List<ProfileLookupModel>();

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.AddParameters(PARM_PATIENT_ID, PatientId);

                ProvidersList = dbManager.ExecuteReaders<ProfileLookupModel>(PROC_NOTES_PROVIDERS);
                return ProvidersList;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupNotesProviders", PROC_NOTES_PROVIDERS, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions for Provider Licence Info"

        public DSProfile LoadProviderLicenseInfo(long ProviderId, long ProviderLicenseId)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (ProviderId <= 0)
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PROVIDER_ID, ProviderId);
                if (ProviderLicenseId <= 0)
                    dbManager.AddParameters(1, PARM_PROVIDER_LICENSE_ID, null);
                else
                    dbManager.AddParameters(1, PARM_PROVIDER_LICENSE_ID, ProviderLicenseId);
                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PROVIDER_LICENSE_INFO_SELECT, ds, ds.ProvidersLicenseInfo.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LoadProviderLicenseInfo", PROC_PROVIDER_LICENSE_INFO_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the Update Provider License Info.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile UpdateProviderLicenseInfo(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters_Provider_License_Info(dbManager, ds, false);
                ds = (DSProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PROVIDER_LICENSE_INFO_UPDATE, ds, ds.ProvidersLicenseInfo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::UpdateProviderLicenseInfo", PROC_PROVIDER_LICENSE_INFO_UPDATE, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the provider license info.
        /// </summary>
        /// <param name="ProviderIds">The provider ids.</param>
        /// <returns></returns>
        public string DeleteProviderLicenseInfo(string ProviderLicenseIds)
        {
            //string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PROVIDER_LICENSE_ID, ProviderLicenseIds);
                //dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PROVIDER_LICENSE_INFO_DELETE);
                //returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PROVIDER_LICENSE_INFO_DELETE).ToString();
                //if (returnValue != "")
                //    throw new Exception(returnValue);

                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::DeleteProviderLicenseInfo", PROC_PROVIDER_LICENSE_INFO_DELETE, ex);
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
        /// Inserts the provider license info.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile InsertProviderLicenseInfo(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters_Provider_License_Info(dbManager, ds, true);
                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PROVIDER_LICENSE_INFO_INSERT, ds, ds.ProvidersLicenseInfo.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::InsertProviderLicenseInfo", PROC_PROVIDER_LICENSE_INFO_INSERT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        public List<LookupModel> LookupByType(string Searchstring)
        {
            List<LookupModel> ds = new List<LookupModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                System.Data.SqlClient.SqlDataReader reader;
                dbManager.Open();
                dbManager.BeginTransaction();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, LOOKUP_TYPE, Searchstring);
                reader = (System.Data.SqlClient.SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_SP_LOOKUP);
                while (reader.Read())
                {
                    ds.Add(new LookupModel {Id = Convert.ToInt64(reader["LookupId"]) , Name = Convert.ToString(reader["Name"]) });
                }
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProvider::LookupByType", PROC_SP_LOOKUP, ex);
                return ds;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
    }
}
