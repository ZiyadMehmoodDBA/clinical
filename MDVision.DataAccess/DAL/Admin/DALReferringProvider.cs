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
using MDVision.Common.Utilities;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALReferringProvider
    {
        #region Variable
        
        #endregion

        #region " Stored Procedure Names"
        private const string PROC_REFERRING_PROVIDER_INSERT = "Provider.sp_ReferringProviderInsert";
        private const string PROC_REFERRING_PROVIDER_UPDATE = "Provider.sp_ReferringProviderUpdate";
        private const string PROC_REFERRING_PROVIDER_DELETE = "Provider.sp_ReferringProviderDelete";
        private const string PROC_REFERRING_PROVIDER_SELECT = "Provider.sp_ReferringProviderSelect";
        private const string PROC_REFERRING_PROVIDER_LOOKUP = "Provider.sp_ReferringProviderLookup";
        private const string PROC_REFERRING_PROVIDER_AUTOCOMPLETE_LOOKUP = "Provider.sp_ReferringProviderAutoCompleteLookup";
        #endregion

        #region "Parameters"

        private const string PARM_REFERRING_PROVIDER_ID = "@ReferringProviderId";
        private const string PARM_FIRST_NAME = "@FirstName";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_NPI = "@NPI";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_ADDRESS = "@Address";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP_CODE = "@ZipCode";
        private const string PARM_ZIP_CODE_EXT = "@ZipCodeExt";
        private const string PARM_EMAIL_ADDRESS = "@EmailAddress";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_MI = "@MI";
        private const string PARM_SPECIALTY_ID = "@SpecialtyId";
        private const string PARM_TAXONOMY_CODE = "@TaxonomyCode";
        private const string PARM_PROFILE_TYPE = "@ProfileType";
        private const string PARM_STATE_LICENCE = "@Statelicence";
        private const string PARM_ADDRESS2 = "@Address2";
        private const string PARM_TELEPHONE_EXT = "@TelephoneExt";
        private const string PARM_FAX = "@Fax";
        private const string PARM_PHONE = "@Phone";
        private const string PARM_CELL = "@Cell";
        private const string PARM_IS_SOVEREIGN = "@IsSovereign";
        private const string PARM_NAME = "@Name";
        private const string PARM_SPECIALTY= "@Specialty";

        public struct Parameters
        {
            public int ID;
            public string FNAME;
            public string LNAME;
        }

        #endregion


        #region Constructors

        public DALReferringProvider()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }
        /// <summary>
        /// Use only for Threading or Windows services to create object with shared variables
        /// </summary>
        /// <param name="SharedVariable"></param>
        public DALReferringProvider(SharedVariable SharedVariable)
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
            dbManager.CreateParameters(28);

            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_REFERRING_PROVIDER_ID, ds.ReferringProvider.ReferringProviderIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_REFERRING_PROVIDER_ID, ds.ReferringProvider.ReferringProviderIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(1, PARM_FIRST_NAME, ds.ReferringProvider.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_LAST_NAME, ds.ReferringProvider.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_NPI, ds.ReferringProvider.NPIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_PHONE_NO, ds.ReferringProvider.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_ADDRESS, ds.ReferringProvider.AddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CITY, ds.ReferringProvider.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_STATE, ds.ReferringProvider.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ZIP_CODE, ds.ReferringProvider.ZipCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_ZIP_CODE_EXT, ds.ReferringProvider.ZipCodeExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_EMAIL_ADDRESS, ds.ReferringProvider.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_IS_ACTIVE, ds.ReferringProvider.IsActiveColumn.ColumnName, DbType.Byte);
            dbManager.AddParameters(12, PARM_CREATED_BY, ds.ReferringProvider.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_CREATED_ON, ds.ReferringProvider.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(14, PARM_MODIFIED_BY, ds.ReferringProvider.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_MODIFIED_ON, ds.ReferringProvider.ModifiedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(16, PARM_ENTITY_ID, ds.ReferringProvider.EntityIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(17, PARM_MI, ds.ReferringProvider.MIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_SPECIALTY_ID, ds.ReferringProvider.SpecialtyIdColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_TAXONOMY_CODE, ds.ReferringProvider.TaxonomyCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(20, PARM_PROFILE_TYPE, ds.ReferringProvider.ProfileTypeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_STATE_LICENCE, ds.ReferringProvider.StatelicenceColumn.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_ADDRESS2, ds.ReferringProvider.Address2Column.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_TELEPHONE_EXT, ds.ReferringProvider.TelephoneExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_FAX, ds.ReferringProvider.FaxColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_CELL, ds.ReferringProvider.CellColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_IS_SOVEREIGN, ds.ReferringProvider.IsSovereignColumn.ColumnName, DbType.Boolean);

            dbManager.AddParameters(27, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
        }

        #endregion

        #region "Insert, delete, update and get using dataset Functions"
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
        public DSProfile LoadReferringProvider(long ReferringProviderId, string FirstName, string LastName, string Active, string NPI, string EntityId, int PageNumber = 1, int RowsPerPage = 1000, string ParentCtrl = "",string Fax="", string Specialty=null, string Phone = "", string IsSovereign = "")
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (FirstName == "")
                    FirstName = null;

                if (LastName == "")
                    LastName = null;

                if (Active == "")
                    Active = null;

                if (NPI == "")
                    NPI = null;
                if (Specialty == "")
                    Specialty = null;

                if (EntityId == "" || EntityId== "undefined")
                    EntityId = null;

                dbManager.Open();
                dbManager.CreateParameters(14);

                if (ReferringProviderId <= 0)
                    dbManager.AddParameters(0, PARM_REFERRING_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REFERRING_PROVIDER_ID, ReferringProviderId);

                dbManager.AddParameters(1, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(2, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, Active);
                dbManager.AddParameters(4, PARM_NPI, NPI);
                if (string.Equals(ParentCtrl, "Patient_Referrals_Outgoing_Detail") || string.Equals(ParentCtrl, "Patient_Referral"))
                {
                    dbManager.AddParameters(5, PARM_ENTITY_ID, null);
                }
                else if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(5, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(5, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(5, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(6, PARM_USER_ID, MDVSession.Current.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.ReferringProvider.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                if (string.IsNullOrEmpty(Fax))
                    dbManager.AddParameters(10, PARM_FAX, null);
                else
                    dbManager.AddParameters(10, PARM_FAX, Fax);
                dbManager.AddParameters(11, PARM_SPECIALTY, Specialty);
                if (string.IsNullOrEmpty(Phone))
                    dbManager.AddParameters(12, PARM_PHONE, null);
                else
                    dbManager.AddParameters(12, PARM_PHONE, Phone);
                if (string.IsNullOrEmpty(IsSovereign))
                    dbManager.AddParameters(13, PARM_IS_SOVEREIGN, null);
                else
                    dbManager.AddParameters(13, PARM_IS_SOVEREIGN, MDVUtility.ToBool(IsSovereign));

                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRING_PROVIDER_SELECT, ds, ds.ReferringProvider.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReferringProvider::LoadReferringProvider", PROC_REFERRING_PROVIDER_SELECT, ex);
                throw ex;
                //Usual code              
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
        /// <param name="ReferringProviderId">The referring provider identifier.</param>
        /// <param name="FirstName">The first name.</param>
        /// <param name="LastName">The last name.</param>
        /// <param name="Active">The active.</param>
        /// <param name="NPI">The npi.</param>
        /// <param name="EntityId">The entity identifier.</param>
        /// <returns></returns>
        public DSProfile LoadReferringProvider(SharedVariable SharedVariable, long ReferringProviderId, string FirstName, string LastName, string Active, string NPI, string EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager(SharedVariable);
            try
            {
                if (FirstName == "")
                    FirstName = null;

                if (LastName == "")
                    LastName = null;

                if (Active == "")
                    Active = null;

                if (NPI == "")
                    NPI = null;

                if (EntityId == "")
                    EntityId = null;

                dbManager.Open();
                dbManager.CreateParameters(10);

                if (ReferringProviderId <= 0)
                    dbManager.AddParameters(0, PARM_REFERRING_PROVIDER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_REFERRING_PROVIDER_ID, ReferringProviderId);

                dbManager.AddParameters(1, PARM_FIRST_NAME, FirstName);
                dbManager.AddParameters(2, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(3, PARM_IS_ACTIVE, Active);
                dbManager.AddParameters(4, PARM_NPI, NPI);
                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(SharedVariable.UserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(5, PARM_ENTITY_ID, EntityId);
                    else
                        dbManager.AddParameters(5, PARM_ENTITY_ID, SharedVariable.EntityId);
                }
                else
                    dbManager.AddParameters(5, PARM_ENTITY_ID, EntityId);
                dbManager.AddParameters(6, PARM_USER_ID, SharedVariable.AppUserId);
                if (PageNumber == 0)
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.ReferringProvider.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                //dbManager.AddParameters(4, PARM_EIN, EIN);
                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRING_PROVIDER_SELECT, ds, ds.ReferringProvider.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog(SharedVariable, "DALReferringProvider::LoadReferringProvider", PROC_REFERRING_PROVIDER_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }


        /// <summary>
        /// Updates the referring provider.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        public DSProfile UpdateReferringProvider(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ReferringProvider.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_REFERRING_PROVIDER_UPDATE, ds, ds.ReferringProvider.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ReferringProvider.Rows[0][ds.ReferringProvider.ReferringProviderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReferringProvider::UpdateReferringProvider", PROC_REFERRING_PROVIDER_UPDATE, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    throw new Exception(str[1].ToString());
                else
                    throw new Exception(ex.Message);
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }


        }

        /// <summary>
        /// Deletes the referring provider.
        /// </summary>
        /// <param name="ReferringProviderIds">The referring provider ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteReferringProvider(string ReferringProviderIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSProfile ds = LoadReferringProvider(Convert.ToInt64(ReferringProviderIds), null, null, null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ReferringProvider;

                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_REFERRING_PROVIDER_ID, ReferringProviderIds);

                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);

                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_REFERRING_PROVIDER_DELETE).ToString();
                if (returnValue != "" && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.ReferringProvider.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ReferringProvider.Rows[0][ds.ReferringProvider.ReferringProviderIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReferringProvider::DeleteReferringProvider", PROC_REFERRING_PROVIDER_DELETE, ex);
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
        /// Inserts the referring provider.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile InsertReferringProvider(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.ReferringProvider.GetChanges();
                CreateParameters(dbManager, ds, true);
                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_REFERRING_PROVIDER_INSERT, ds, ds.ReferringProvider.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.ReferringProvider.Rows[0][ds.ReferringProvider.ReferringProviderIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReferringProvider::InsertReferringProvider", PROC_REFERRING_PROVIDER_INSERT, ex);
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
        /// Lookups the referring provider.
        /// </summary>
        /// <returns></returns>
        public DSProfileLookup LookupReferringProvider(string Active, string Name = "")
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;
                if (Name == "")
                    Name = null;

                dbManager.Open();
                dbManager.CreateParameters(4);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                dbManager.AddParameters(3, PARM_NAME, Name);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRING_PROVIDER_LOOKUP, ds, ds.ReferringProvider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReferringProvider::LookupReferringProvider", PROC_REFERRING_PROVIDER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfileLookup LookupReferringProviderOutgoing(string Active, string Name = "")
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;
                if (Name == "")
                    Name = null;

                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                dbManager.AddParameters(3, PARM_NAME, Name);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRING_PROVIDER_LOOKUP, ds, ds.ReferringProvider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReferringProvider::LookupReferringProviderOutgoing", PROC_REFERRING_PROVIDER_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfileLookup LookupReferringProviderAutocomplete(string Active, string Name = "")
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;
                if (Name == "")
                    Name = null;

                dbManager.Open();
                dbManager.CreateParameters(4);
                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(1, PARM_USER_ID, MDVSession.Current.AppUserId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                dbManager.AddParameters(3, PARM_NAME, Name);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_REFERRING_PROVIDER_AUTOCOMPLETE_LOOKUP, ds, ds.ReferringProvider.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALReferringProvider::LookupReferringProvider", PROC_REFERRING_PROVIDER_AUTOCOMPLETE_LOOKUP, ex);
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

