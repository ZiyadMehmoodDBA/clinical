using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;


namespace MDVision.DataAccess.DAL.Admin
{
    public class DALBillingProviderSettings
    {
        #region Variable
        
        #endregion

        #region "Stored Procedure Names"
        private const string PROC_BILLINGPROVIDERSETTINGS_INSERT = "Provider.sp_BillingProviderSettingsInsert";
        private const string PROC_BILLINGPROVIDERSETTINGS_UPDATE = "Provider.sp_BillingProviderSettingsUpdate";
        private const string PROC_BILLINGPROVIDERSETTINGS_DELETE = "Provider.sp_BillingProviderSettingsDelete";
        private const string PROC_BILLINGPROVIDERSETTINGS_SELECT = "Provider.sp_BillingProviderSettingsSelect";
        private const string PROC_LOOP2310B_LOOKUP = "Provider.sp_Loop2310BLookup";


        private const string PROC_BILLINGPROVIDER_INSERT = "Provider.sp_BillingProviderInsert";
        private const string PROC_BILLINGPROVIDER_UPDATE = "Provider.sp_BillingProviderUpdate";
        private const string PROC_BILLINGPROVIDER_DELETE = "Provider.sp_BillingProviderDelete";
        private const string PROC_BILLINGPROVIDER_SELECT = "Provider.sp_BillingProviderSelect";
        private const string PROC_BILLINGPROVIDER_LOOKUP = "Provider.sp_BillingProviderLookup";

        #endregion

        #region "Parameters"
        private const string PARM_BILLING_PROVIDER_ID = "@BillingProviderId";
        private const string PARM_INSURANCE_ID = "@InsuranceId";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_PROVIDER_ID = "@ProviderId";
        private const string PARM_BILL_TO_PROVIDER_SSN = "@BillToProviderSSN";
        private const string PARM_ACCEPT_ASSIGNMENT = "@AcceptAssignment";
        private const string PARM_LOOP_2310B = "@Loop2310B";
        private const string PARM_BILL_TO_EIN = "@BillToEIN";
        private const string PARM_IS_EIN = "@ISEIN";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_EIN_SUFFIX = "@EINSuffix";
        private const string PARM_EIN = "@EIN";
        private const string PARM_EIN_NAME = "@EINName";
        private const string PARM_NPI = "@NPI";
        private const string PARM_TAXONOMY_CODE = "@TaxonomyCode";
        private const string PARM_ADDRESS1 = "@Address1";
        private const string PARM_ADDRESS2 = "@Address2";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIPCODE = "@ZIPCode";
        private const string PARM_ZIPCODE_EXT = "@ZIPCodeExt";
        private const string PARM_PAY_TO_ADDRESS1 = "@PayToAddress1";
        private const string PARM_PAY_TO_ADDRESS2 = "@PayToAddress2";
        private const string PARM_PAY_TO_CITY = "@PayToCity";
        private const string PARM_PAY_TO_STATE = "@PayToState";
        private const string PARM_PAY_TO_ZIPCODE = "@PayToZIPCode";
        private const string PARM_PAY_TO_ZIPCODE_EXT = "@PayToZIPCodeExt";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_IS_PAY_TO_ADDRESS = "@IsPayToAddress";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_MI = "@MI";
        private const string PARM_PROVIDER_TYPE = "@ProviderType";


        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALBillingProviderSettings"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALBillingProviderSettings()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           

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
        private void CreateParameters(IDBManager dbManager, DSBillingProviderSettings ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(32);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_BILLING_PROVIDER_ID, ds.BillingProviderSettings.BillingProviderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_BILLING_PROVIDER_ID, ds.BillingProviderSettings.BillingProviderIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_INSURANCE_ID, ds.BillingProviderSettings.InsuranceIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(2, PARM_FACILITY_ID, ds.BillingProviderSettings.FacilityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_PROVIDER_ID, ds.BillingProviderSettings.ProviderIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(4, PARM_BILL_TO_PROVIDER_SSN, ds.BillingProviderSettings.BillToProviderSSNColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(5, PARM_ACCEPT_ASSIGNMENT, ds.BillingProviderSettings.AcceptAssignmentColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(6, PARM_LOOP_2310B, ds.BillingProviderSettings.Loop2310BColumn.ColumnName, DbType.Int16);
            dbManager.AddParameters(7, PARM_BILL_TO_EIN, ds.BillingProviderSettings.BillToEINColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(8, PARM_EIN_SUFFIX, ds.BillingProviderSettings.EINSuffixColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_EIN, ds.BillingProviderSettings.EINColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_EIN_NAME, ds.BillingProviderSettings.EINNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_TAXONOMY_CODE, ds.BillingProviderSettings.TaxonomyCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ADDRESS1, ds.BillingProviderSettings.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_ADDRESS2, ds.BillingProviderSettings.Address2Column.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_CITY, ds.BillingProviderSettings.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_STATE, ds.BillingProviderSettings.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_ZIPCODE, ds.BillingProviderSettings.ZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_ZIPCODE_EXT, ds.BillingProviderSettings.ZIPCodeExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_PAY_TO_ADDRESS1, ds.BillingProviderSettings.PayToAddress1Column.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_PAY_TO_ADDRESS2, ds.BillingProviderSettings.PayToAddress2Column.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_PAY_TO_CITY, ds.BillingProviderSettings.PayToCityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_PAY_TO_STATE, ds.BillingProviderSettings.PayToStateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_PAY_TO_ZIPCODE, ds.BillingProviderSettings.PayToZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_PAY_TO_ZIPCODE_EXT, ds.BillingProviderSettings.PayToZIPCodeExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_IS_ACTIVE, ds.BillingProviderSettings.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(25, PARM_CREATED_BY, ds.BillingProviderSettings.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_CREATED_ON, ds.BillingProviderSettings.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(27, PARM_MODIFIED_BY, ds.BillingProviderSettings.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_MODIFIED_ON, ds.BillingProviderSettings.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(29, PARM_NPI, ds.BillingProviderSettings.NPIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
            dbManager.AddParameters(31, PARM_IS_PAY_TO_ADDRESS, ds.BillingProviderSettings.IsPayToAddressColumn.ColumnName, DbType.Boolean);
        }


        private void CreateParametersBillingProvider(IDBManager dbManager, DSBillingProviderSettings ds, bool IsInsert)
        {
            dbManager.CreateParameters(31);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_BILLING_PROVIDER_ID, ds.BillingProvider.BillingProviderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_BILLING_PROVIDER_ID, ds.BillingProvider.BillingProviderIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_SHORT_NAME, ds.BillingProvider.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_ENTITY_ID, ds.BillingProvider.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(3, PARM_IS_EIN, ds.BillingProvider.ISEINColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(4, PARM_EIN_SUFFIX, ds.BillingProvider.EINSuffixColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_EIN, ds.BillingProvider.EINColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_TAXONOMY_CODE, ds.BillingProvider.TaxonomyCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_ADDRESS1, ds.BillingProvider.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ADDRESS2, ds.BillingProvider.Address2Column.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_CITY, ds.BillingProvider.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_STATE, ds.BillingProvider.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_ZIPCODE, ds.BillingProvider.ZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_ZIPCODE_EXT, ds.BillingProvider.ZIPCodeExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_PAY_TO_ADDRESS1, ds.BillingProvider.PayToAddress1Column.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_PAY_TO_ADDRESS2, ds.BillingProvider.PayToAddress2Column.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_PAY_TO_CITY, ds.BillingProvider.PayToCityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_PAY_TO_STATE, ds.BillingProvider.PayToStateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_PAY_TO_ZIPCODE, ds.BillingProvider.PayToZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_PAY_TO_ZIPCODE_EXT, ds.BillingProvider.PayToZIPCodeExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_IS_ACTIVE, ds.BillingProvider.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(20, PARM_CREATED_BY, ds.BillingProvider.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_CREATED_ON, ds.BillingProvider.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(22, PARM_MODIFIED_BY, ds.BillingProvider.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_MODIFIED_ON, ds.BillingProvider.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(24, PARM_NPI, ds.BillingProvider.NPIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_IS_PAY_TO_ADDRESS, ds.BillingProvider.IsPayToAddressColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(26, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 500);

            dbManager.AddParameters(27, PARM_LAST_NAME, ds.BillingProvider.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_FIRST_NAME, ds.BillingProvider.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(29, PARM_MI, ds.BillingProvider.MIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_PROVIDER_TYPE, ds.BillingProvider.ProviderTypeColumn.ColumnName, DbType.String);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"

        public DSBillingProviderSettings InsertBillingProviderSettings(ref DSBillingProviderSettings ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DALUsersActivity obj = new DALUsersActivity();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.BillingProviderSettings.GetChanges();
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSBillingProviderSettings)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLINGPROVIDERSETTINGS_INSERT, ds, ds.BillingProviderSettings.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.BillingProviderSettings.Rows[0][ds.BillingProviderSettings.BillingProviderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_USER, true, "Insert User", ds.Tables[ds.BillingProviderSettings.TableName].Rows[0][ds.BillingProviderSettings.BillingProviderIdColumn.ColumnName].ToString());
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingProviderSettings::InsertBillingProviderSettings", PROC_BILLINGPROVIDERSETTINGS_INSERT, ex);
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
        /// Loads the billing provider settings.
        /// </summary>
        /// <param name="BillingProviderId">The billing provider identifier.</param>
        /// <param name="InsuranceId">The insurance identifier.</param>
        /// <param name="FacilityId">The facility identifier.</param>
        /// <param name="ProviderId">The provider identifier.</param>
        /// <returns></returns>
        public DSBillingProviderSettings LoadBillingProviderSettings(long BillingProviderId, string InsuranceId, string FacilityId, string ProviderId, string Active, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSBillingProviderSettings ds = new DSBillingProviderSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (InsuranceId == "")
                    InsuranceId = null;

                if (FacilityId == "")
                    FacilityId = null;

                if (ProviderId == "")
                    ProviderId = null;

                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(9);

                if (BillingProviderId <= 0)
                    dbManager.AddParameters(0, PARM_BILLING_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BILLING_PROVIDER_ID, BillingProviderId);

                dbManager.AddParameters(1, PARM_INSURANCE_ID, InsuranceId);
                dbManager.AddParameters(2, PARM_FACILITY_ID, FacilityId);
                dbManager.AddParameters(3, PARM_PROVIDER_ID, ProviderId);
                dbManager.AddParameters(4, PARM_IS_ACTIVE, Active);
                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(7, PARM_RECORD_COUNT, ds.BillingProviderSettings.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(8, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(8, PARM_ENTITY_ID, MDVSession.Current.EntityId);

                ds = (DSBillingProviderSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLINGPROVIDERSETTINGS_SELECT, ds, ds.BillingProviderSettings.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingProviderSettings::LoadBillingProviderSettings", PROC_BILLINGPROVIDERSETTINGS_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the billing provider settings.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        public DSBillingProviderSettings UpdateBillingProviderSettings(ref DSBillingProviderSettings ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.BillingProviderSettings.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSBillingProviderSettings)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLINGPROVIDERSETTINGS_UPDATE, ds, ds.BillingProviderSettings.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.BillingProviderSettings.Rows[0][ds.BillingProviderSettings.BillingProviderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingProviderSettings::UpdateBillingProviderSettings", PROC_BILLINGPROVIDERSETTINGS_UPDATE, ex);
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
        /// Deletes the billing provider settings.
        /// </summary>
        /// <param name="BillingProviderIds">The billing provider ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteBillingProviderSettings(string BillingProviderIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSBillingProviderSettings ds = LoadBillingProviderSettings(Convert.ToInt64(BillingProviderIds), null, null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.BillingProviderSettings;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BILLING_PROVIDER_ID, BillingProviderIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLINGPROVIDERSETTINGS_DELETE).ToString();
                if (returnValue != "" && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.BillingProviderSettings.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.BillingProviderSettings.Rows[0][ds.BillingProviderSettings.BillingProviderIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::DeleteBillingProviderSettings", PROC_BILLINGPROVIDERSETTINGS_DELETE, ex);
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

        #region " Billing Provider ".

        public DSBillingProviderSettings LoadBillingProvider(long BillingProviderId, string ShortName, string EntityId, string IsBilltoEIN, string Active,string NPINumber, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSBillingProviderSettings ds = new DSBillingProviderSettings();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {



                dbManager.Open();
                dbManager.CreateParameters(9);

                if (Active == "")
                    Active = null;

                if (EntityId == "")
                    EntityId = null;

                if (ShortName == "")
                    ShortName = null;

                if (IsBilltoEIN == "")
                    IsBilltoEIN = null;
                if (NPINumber == "")
                    NPINumber = null;
                



                if (BillingProviderId <= 0)
                    dbManager.AddParameters(0, PARM_BILLING_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_BILLING_PROVIDER_ID, BillingProviderId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(2, PARM_ENTITY_ID, null);
                    else
                        dbManager.AddParameters(2, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                {
                    dbManager.AddParameters(2, PARM_ENTITY_ID, EntityId);
                }

                dbManager.AddParameters(3, PARM_IS_ACTIVE, Active);
                dbManager.AddParameters(4, PARM_IS_EIN, IsBilltoEIN);

                if (PageNumber == 0)
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(5, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(6, PARM_ROWSP_PAGE, RowsPerPage);

                dbManager.AddParameters(7, "@NPINumber", NPINumber);

                dbManager.AddParameters(8, PARM_RECORD_COUNT, ds.BillingProvider.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output, null, 500);

                ds = (DSBillingProviderSettings)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLINGPROVIDER_SELECT, ds, ds.BillingProvider.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingProviderSettings::LoadBillingProvider", PROC_BILLINGPROVIDER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSBillingProviderSettings UpdateBillingProvider(ref DSBillingProviderSettings ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.BillingProvider.GetChanges();
                this.CreateParametersBillingProvider(dbManager, ds, false);
                ds = (DSBillingProviderSettings)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_BILLINGPROVIDER_UPDATE, ds, ds.BillingProvider.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.BillingProvider.Rows[0][ds.BillingProvider.BillingProviderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingProviderSettings::UpdateBillingProvider", PROC_BILLINGPROVIDER_UPDATE, ex);
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

        public DSBillingProviderSettings InsertBillingProvider(ref DSBillingProviderSettings ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DALUsersActivity obj = new DALUsersActivity();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.BillingProvider.GetChanges();
                dbManager.Open();
                CreateParametersBillingProvider(dbManager, ds, true);
                ds = (DSBillingProviderSettings)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_BILLINGPROVIDER_INSERT, ds, ds.BillingProvider.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.BillingProvider.Rows[0][ds.BillingProvider.BillingProviderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingProviderSettings::InsertBillingProvider", PROC_BILLINGPROVIDER_INSERT, ex);
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

        public string DeleteBillingProvider(string BillingProviderId)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSBillingProviderSettings ds = LoadBillingProvider(Convert.ToInt64(BillingProviderId), null, null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.BillingProvider;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_BILLING_PROVIDER_ID, BillingProviderId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_BILLINGPROVIDER_DELETE).ToString();
                if (returnValue != "" && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.BillingProvider.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.BillingProvider.Rows[0][ds.BillingProvider.BillingProviderIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALUser::DeleteBillingProvider", PROC_BILLINGPROVIDER_DELETE, ex);
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

        #region Lookups

        public DSProfileLookup LookupLoop2310B()
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_LOOP2310B_LOOKUP, ds, ds.Loop2310B.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingProviderSettings::LookupLoop2310B", PROC_LOOP2310B_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfileLookup LookupLoopBillingProvider(string Active)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (Active == "")
                    Active = null;

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(1, PARM_IS_ACTIVE, Active);


                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_BILLINGPROVIDER_LOOKUP, ds, ds.BillingProvider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALBillingProviderSettings::LookupLoopBillingProvider", PROC_BILLINGPROVIDER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion
    }
}
