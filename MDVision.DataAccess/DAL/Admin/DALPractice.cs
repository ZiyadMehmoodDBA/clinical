using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALPractice
    {
        #region Variable

        #endregion
        #region " Stored Procedure Names"
        private const string PROC_PRACTICE_INSERT = "Provider.sp_PracticeInsert";
        private const string PROC_PRACTICE_UPDATE = "Provider.sp_PracticeUpdate";
        private const string PROC_PRACTICE_DELETE = "Provider.sp_PracticeDelete";
        private const string PROC_PRACTICE_SELECT = "Provider.sp_PracticeSelect";
        private const string PROC_DEMOGRAPHIC_PRACTICE_SELECT = "Provider.sp_DemographicPracticeSelect";

        private const string PROC_PRACTICE_LOOKUP = "Provider.sp_PracticeLookup";




        #endregion

        #region "Parameters"
        private const string PARM_PRACTICE_ID = "@PracticeId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_PHONE_NO = "@PhoneNo";
        private const string PARM_PHONE_EXT = "@PhoneExt";
        private const string PARM_FAX = "@Fax";
        private const string PARM_ENTITY = "@EntityId";
        private const string PARM_EIN = "@EIN";
        private const string PARM_TAXONOMY = "@TaxonomyCode";
        private const string PARM_NPI = "@NPI";
        private const string PARM_FEE_GROUP_ID = "@FeeGroupId";
        private const string PARM_BASIC_FEE_GROUP_ID = "@BasicFeeGroupId";
        private const string PARM_ADDRESS = "@Address";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIPCODE = "@ZIPCode";
        private const string PARM_ZIPCODE_EXT = "@ZIPCodeExt";
        private const string PARM_EMAIL = "@EmailAddress";
        private const string PARM_NOTES = "@Notes";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_PAY_TO_ADDRESS1 = "@Address1";
        private const string PARM_PAY_TO_ADDRESS2 = "@Address2";
        private const string PARM_PAY_TO_CITY = "@Address2City";
        private const string PARM_PAY_TO_STATE = "@Address2State";
        private const string PARM_PAY_TO_ZIP = "@Address2ZIPCode";
        private const string PARM_PAY_TO_ZIP_EXT = "@Address2ZIPCodeExt";
        private const string PARM_WEBSITE = "@Website";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_FACILITY_ID = "@FacilityId";
        private const string PARM_IS_PAY_TO_ADDRESS = "@IsPayToAddress";
        private const string PARM_SCAN = "@Scan";
        private const string PARM_OCR = "@OCR";


        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        public struct Parameters
        {
            public int ID;
            public string FNAME;
            public string LNAME;
        }
        #endregion

        #region Constructors
        //private static DALPractice _instance = null;
        ///// <summary>
        ///// Singleton context
        ///// </summary>
        //public static DALPractice Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new DALPractice();
        //        return _instance;
        //    }
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="DALPractice"/> class.
        /// </summary>
        /// <param name="Obj">The Shared Variable.</param>
        public DALPractice()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();

        }
        public DALPractice(SharedVariable SharedVariable)
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
            dbManager.CreateParameters(35);


            dbManager.AddParameters(0, PARM_SHORT_NAME, ds.Practice.ShortNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(1, PARM_DESCRIPTION, ds.Practice.DescriptionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_PHONE_NO, ds.Practice.PhoneNoColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_PHONE_EXT, ds.Practice.PhoneExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(4, PARM_FAX, ds.Practice.FaxColumn.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_ENTITY, ds.Practice.EntityIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(6, PARM_EIN, ds.Practice.EINColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_TAXONOMY, ds.Practice.TaxonomyCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_NPI, ds.Practice.NPIColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_FEE_GROUP_ID, ds.Practice.FeeGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(10, PARM_BASIC_FEE_GROUP_ID, ds.Practice.BasicFeeGroupIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(11, PARM_ADDRESS, ds.Practice.AddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_CITY, ds.Practice.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_STATE, ds.Practice.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_ZIPCODE, ds.Practice.ZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_ZIPCODE_EXT, ds.Practice.ZIPCodeExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(16, PARM_EMAIL, ds.Practice.EmailAddressColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_WEBSITE, ds.Practice.WebSiteColumn.ColumnName, DbType.String);
            dbManager.AddParameters(18, PARM_NOTES, ds.Practice.NotesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_START_DATE, ds.Practice.StartDateColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(20, PARM_PAY_TO_ADDRESS1, ds.Practice.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(21, PARM_PAY_TO_ADDRESS2, ds.Practice.Address2Column.ColumnName, DbType.String);
            dbManager.AddParameters(22, PARM_PAY_TO_CITY, ds.Practice.Address2CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(23, PARM_PAY_TO_STATE, ds.Practice.Address2StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(24, PARM_PAY_TO_ZIP, ds.Practice.Address2ZIPCodeColumn.ColumnName, DbType.String);
            dbManager.AddParameters(25, PARM_PAY_TO_ZIP_EXT, ds.Practice.Address2ZIPCodeExtColumn.ColumnName, DbType.String);
            dbManager.AddParameters(26, PARM_IS_ACTIVE, ds.Practice.IsActiveColumn.ColumnName, DbType.String);
            dbManager.AddParameters(27, PARM_CREATED_BY, ds.Practice.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(28, PARM_CREATED_ON, ds.Practice.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(29, PARM_MODIFIED_BY, ds.Practice.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(30, PARM_MODIFIED_ON, ds.Practice.ModifiedOnColumn.ColumnName, DbType.DateTime);
            if (IsInsert == true)
                dbManager.AddParameters(31, PARM_PRACTICE_ID, ds.Practice.PracticeIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(31, PARM_PRACTICE_ID, ds.Practice.PracticeIdColumn.ColumnName, DbType.Int64);

            dbManager.AddParameters(32, PARM_IS_PAY_TO_ADDRESS, ds.Practice.IsPayToAddressColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(33, PARM_SCAN, ds.Practice.ScanColumn.ColumnName, DbType.String);
            dbManager.AddParameters(34, PARM_OCR, ds.Practice.OCRColumn.ColumnName, DbType.String);
        }

        /// <summary>
        /// Creates the insert update parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        private void CreateInsertUpdateParameters(IDBManager dbManager, DSProfile ds)
        {

            dbManager.CreateInsertUpdateDeleteParameters(ds.Tables[ds.Practice.TableName].Columns.Count, 1);
            dbManager.AddInsertUpdateDeleteParameters(0, PARM_PRACTICE_ID, ds.Practice.PracticeIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            //dbManager.AddInsertUpdateDeleteParameters(1, PARM_SHORT_NAME, ds.Practice.ShortNameColumn.ColumnName, DbType.String);


        }
        #endregion

        #region "Insert, delete, update and get using dataset Functions"
        /// <summary>
        /// Loads the practice.
        /// </summary>
        /// <param name="PracticeId">The practice identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="Entity">The entity.</param>
        /// <param name="EIN">The ein.</param>
        /// <returns></returns>
        public DSProfile LoadPractice(long PracticeId, string ShortName, string Description, string Entity, string EIN, string Active, int PageNumber = 1, int RowsPerPage = 1000, SharedVariable sharedVariable = null)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            long AppUserId = 0;
            string username = string.Empty;
            string EntityId = string.Empty;
            if (sharedVariable != null)
            {
                AppUserId = sharedVariable.AppUserId;
                username = sharedVariable.UserName;
                EntityId = sharedVariable.EntityId;
            }
            else
            {
                AppUserId = MDVSession.Current.AppUserId;
                username = MDVSession.Current.AppUserName;
                EntityId = MDVSession.Current.EntityId;
            }
            try
            {

                if (ShortName == "")
                    ShortName = null;

                if (Description == "")
                    Description = null;

                if (Entity == "")
                    Entity = null;

                if (EIN == "")
                    EIN = null;

                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(10);

                if (PracticeId <= 0)
                    dbManager.AddParameters(0, PARM_PRACTICE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PRACTICE_ID, PracticeId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_DESCRIPTION, Description);
                if (Entity == null)
                {
                    if (ClientConfiguration.DecryptFrom64(username).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(3, PARM_ENTITY, Entity);
                    else
                        dbManager.AddParameters(3, PARM_ENTITY, EntityId);
                }
                else
                    dbManager.AddParameters(3, PARM_ENTITY, Entity);

                dbManager.AddParameters(4, PARM_EIN, EIN);
                dbManager.AddParameters(5, PARM_USER_ID, AppUserId);
                dbManager.AddParameters(6, PARM_IS_ACTIVE, Active);

                if (PageNumber == 0)
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(7, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(8, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(9, PARM_RECORD_COUNT, ds.Practice.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRACTICE_SELECT, ds, ds.Practice.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                //MDVLogger.DALErrorLog("DALPractice::LoadPractice", PROC_PRACTICE_SELECT, ex);
                MDVLogger.SendExcepToDB(ex, "DALPractice::LoadPractice", PROC_PRACTICE_SELECT);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }

        public DSProfile LoadDemographicPractice(long PracticeId, string Entity, int PageNumber = 1, int RowsPerPage = 1000)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Entity == "")
                    Entity = null;

                dbManager.Open();
                dbManager.CreateParameters(6);

                if (PracticeId <= 0)
                    dbManager.AddParameters(0, PARM_PRACTICE_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PRACTICE_ID, PracticeId);


                if (Entity == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(1, PARM_ENTITY, Entity);
                    else
                        dbManager.AddParameters(1, PARM_ENTITY, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(1, PARM_ENTITY, Entity);


                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);


                if (PageNumber == 0)
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(3, PARM_PAGE_NUMBER, PageNumber);
                if (RowsPerPage == 0)
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, null);
                else
                    dbManager.AddParameters(4, PARM_ROWSP_PAGE, RowsPerPage);
                dbManager.AddParameters(5, PARM_RECORD_COUNT, ds.DemographicPractice.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);

                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_DEMOGRAPHIC_PRACTICE_SELECT, ds, ds.DemographicPractice.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPractice::LoadDemographicPractice", PROC_DEMOGRAPHIC_PRACTICE_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }

        /// <summary>
        /// Lookups the practice.
        /// </summary>
        /// <returns></returns>
        public DSProfileLookup LookupPractice(string Active)
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
                //if (FacilityId == 0)
                //    dbManager.AddParameters(0, PARM_FACILITY_ID, null);
                //else
                //    dbManager.AddParameters(0, PARM_FACILITY_ID, FacilityId);

                if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                    dbManager.AddParameters(1, PARM_ENTITY, null);
                else
                    dbManager.AddParameters(1, PARM_ENTITY, MDVSession.Current.EntityId);

                dbManager.AddParameters(2, PARM_IS_ACTIVE, Active);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRACTICE_LOOKUP, ds, ds.Practice.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPractice::LookupPractice", PROC_PRACTICE_LOOKUP, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfileLookup LookupPractice(string Active, string EntityId, string ShortName)
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                if (Active == "")
                    Active = null;

                dbManager.Open();
                dbManager.CreateParameters(4);

                if (EntityId == null)
                {
                    if (ClientConfiguration.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() == ClientConfiguration.DefaultUser.ToUpper())
                        dbManager.AddParameters(0, PARM_ENTITY, null);
                    else
                        dbManager.AddParameters(0, PARM_ENTITY, MDVSession.Current.EntityId);
                }
                else
                    dbManager.AddParameters(0, PARM_ENTITY, EntityId);

                dbManager.AddParameters(1, PARM_IS_ACTIVE, Active);

                if (string.IsNullOrEmpty(ShortName))
                    dbManager.AddParameters(2, PARM_SHORT_NAME, null);
                else
                    dbManager.AddParameters(2, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(3, PARM_USER_ID, MDVSession.Current.AppUserId);

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PRACTICE_LOOKUP, ds, ds.Practice.TableName);


                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfile::LookupPractice", PROC_PRACTICE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the practice.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile UpdatePractice(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Practice.GetChanges();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSProfile)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PRACTICE_UPDATE, ds, ds.Practice.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Practice.Rows[0][ds.Practice.PracticeIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPractice::UpdatePractice", PROC_PRACTICE_UPDATE, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }


        }

        /// <summary>
        /// Deletes the practice.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile DeletePractice(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Practice;
                dbManager.Open();
                dbManager.CreateParameters(1);
                dbManager.AddParameters(0, PARM_PRACTICE_ID, ds.Practice.PracticeIdColumn.ColumnName, DbType.Int64);
                ds = (DSProfile)dbManager.DeleteDataSet(CommandType.StoredProcedure, PROC_PRACTICE_DELETE, ds, ds.Practice.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null && ds.Practice.Rows.Count > 0)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Practice.Rows[0][ds.Practice.PracticeIdColumn].ToString(), "", false, false, true);
                //    dsDBAudit.AcceptChanges();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPractice::DeletePractice", PROC_PRACTICE_DELETE, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the practice.
        /// </summary>
        /// <param name="PracticeIds">The practice ids.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeletePractice(string PracticeIds)
        {
            string returnValue = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DSProfile ds = LoadPractice(Convert.ToInt64(PracticeIds), null, null, null, null, null);
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Practice;
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_PRACTICE_ID, PracticeIds);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PRACTICE_DELETE);
                ////dbManager.AddParameters(0, PARM_PRACTICE_ID, ds.Practice.PracticeIdColumn.ColumnName, DbType.Int64);
                ////ds = (DSEntity)dbManager.DeleteDataSet(CommandType.StoredProcedure, PROC_PRACTICE_DELETE, ds, ds.Practice.TableName);
                ////ds.AcceptChanges();
                ////return ds;
                returnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_PRACTICE_DELETE).ToString();
                if (returnValue != "" && returnValue.ToString() != "")
                {
                    throw new Exception(returnValue);
                }
                //else
                //{
                //    if (dtTemp != null && ds.Practice.Rows.Count > 0)
                //    {
                //        dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Practice.Rows[0][ds.Practice.PracticeIdColumn].ToString(), "", false, false, true);
                //        dsDBAudit.AcceptChanges();
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPractice::DeletePractice", PROC_PRACTICE_DELETE, ex);
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
        /// Inserts the practice.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile InsertPractice(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                //DALUsersActivity obj = new DALUsersActivity();
                //DSDBAudit dsDBAudit = new DSDBAudit();
                //DataTable dtTemp = ds.Practice.GetChanges();
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PRACTICE_INSERT, ds, ds.Practice.TableName);
                ds.AcceptChanges();
                //if (dtTemp != null)
                //{
                //    dsDBAudit = new DBActivityAudit().InsertDBAuditAdmin(dtTemp, dbManager, ds.Practice.Rows[0][ds.Practice.PracticeIdColumn].ToString());
                //    dsDBAudit.AcceptChanges();
                //}
                //obj.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.Insert, DALUsersActivity.TITLE_USER, true, "Insert User", ds.Tables[ds.Practice.TableName].Rows[0][ds.Practice.PracticeIdColumn.ColumnName].ToString());
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPractice::InsertPractice", PROC_PRACTICE_INSERT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }

        }

        /// <summary>
        /// Inserts the and update practice.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSProfile InsertAndUpdatePractice(ref DSProfile ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateInsertUpdateParameters(dbManager, ds);
                ds = (DSProfile)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PRACTICE_INSERT, PROC_PRACTICE_UPDATE, PROC_PRACTICE_DELETE, ds, ds.Practice.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPractice::InsertAndUpdatePractice", PROC_PRACTICE_INSERT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #region "use for transaction with dataset"
        /// <summary>
        /// Inserts the practice.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="dbManager">The database manager.</param>
        /// <returns></returns>
        public DSProfile InsertPractice(ref DSProfile ds, IDBManager dbManager)
        {

            try
            {
                CreateParameters(dbManager, ds, true);
                ds = (DSProfile)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PRACTICE_INSERT, ds, ds.Practice.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPractice::InsertPractice", PROC_PRACTICE_INSERT, ex);
                throw ex;
                //Usual code              
            }


        }

        /// <summary>
        /// Inserts the and update practice.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="dbManager">The database manager.</param>
        /// <returns></returns>
        public DSProfile InsertAndUpdatePractice(ref DSProfile ds, IDBManager dbManager)
        {
            try
            {
                this.CreateInsertUpdateParameters(dbManager, ds);
                ds = (DSProfile)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PRACTICE_INSERT, PROC_PRACTICE_UPDATE, PROC_PRACTICE_DELETE, ds, ds.Practice.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALPractice::InsertAndUpdatePractice", PROC_PRACTICE_INSERT, ex);
                throw ex;
            }
        }
        #endregion
        #endregion
    }
}
