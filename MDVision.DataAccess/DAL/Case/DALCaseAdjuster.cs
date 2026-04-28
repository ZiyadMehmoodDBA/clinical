using MDVision.Common.Logging;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DAL.Case
{
    public class DALCaseAdjuster
    {
        #region Variable

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALDocument"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALCaseAdjuster()
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

        #region "Stored Procedure Names"

        private const string PROC_CASE_ADJUSTER_DELETE = "Patient.sp_CaseAdjusterDelete";
        private const string PROC_CASE_ADJUSTER_INSERT = "Patient.sp_CaseAdjusterInsert";
        private const string PROC_CASE_ADJUSTER_SELECT = "Patient.sp_CaseAdjusterSelect";
        private const string PROC_CASE_ADJUSTER_UPDATE = "Patient.sp_CaseAdjusterUpdate";



        #endregion

        #region Parameters
        private const string PARM_CASE_ADJUSTER_ID = "@CaseAdjusterId ";
        private const string PARM_CASE_FIRST_NAME = "@FirstName";
        private const string PARM_LAST_NAME = "@LastName";
        private const string PARM_CASE_DOB = "@DOB ";
        private const string PARM_ADDRESS = "@Address";
        private const string PARM_ADDRESS1 = "@Address1";
        private const string PARM_ADDRESS2 = "@Address2";
        private const string PARM_CITY = "@City";
        private const string PARM_STATE = "@State";
        private const string PARM_ZIP = "@Zip";
        private const string PARM_PHONE = "@Phone";
        private const string PARM_EXTENTION = "@Extention";
        private const string PARM_FAX = "@Fax";
        private const string PARM_EMAIL = "@Email";
        private const string PARM_PREFERENCE = "@Preference";
        private const string PARM_NOTES = "@Notes";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_ON = "@ModifiedOn"; 
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_RECORD_COUNT = "@RecordCount";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWS_PERPAGE = "@RowspPage";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        #endregion

        #region "Support Functions"
        /// <summary>
        /// Creates the parameters.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="IsInsert">if set to <c>true</c> [is insert].</param>
        private void CreateParameters(IDBManager dbManager, DSCase ds, Boolean IsInsert)
        {

            dbManager.CreateParameters(20);
            if (IsInsert == true)
                dbManager.AddParameters(0, PARM_CASE_ADJUSTER_ID, ds.CaseAdjuster.CaseAdjusterIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            else
                dbManager.AddParameters(0, PARM_CASE_ADJUSTER_ID, ds.CaseAdjuster.CaseAdjusterIdColumn.ColumnName, DbType.Int64);
            dbManager.AddParameters(1, PARM_CASE_FIRST_NAME, ds.CaseAdjuster.FirstNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(2, PARM_LAST_NAME, ds.CaseAdjuster.LastNameColumn.ColumnName, DbType.String);
            dbManager.AddParameters(3, PARM_CASE_DOB, ds.CaseAdjuster.DOBColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(4, PARM_ADDRESS1, ds.CaseAdjuster.Address1Column.ColumnName, DbType.String);
            dbManager.AddParameters(5, PARM_ADDRESS2, ds.CaseAdjuster.Address2Column.ColumnName, DbType.String);
            dbManager.AddParameters(6, PARM_CITY, ds.CaseAdjuster.CityColumn.ColumnName, DbType.String);
            dbManager.AddParameters(7, PARM_STATE, ds.CaseAdjuster.StateColumn.ColumnName, DbType.String);
            dbManager.AddParameters(8, PARM_ZIP, ds.CaseAdjuster.ZipColumn.ColumnName, DbType.String);
            dbManager.AddParameters(9, PARM_PHONE, ds.CaseAdjuster.PhoneColumn.ColumnName, DbType.String);
            dbManager.AddParameters(10, PARM_EXTENTION, ds.CaseAdjuster.ExtentionColumn.ColumnName, DbType.String);
            dbManager.AddParameters(11, PARM_FAX, ds.CaseAdjuster.FaxColumn.ColumnName, DbType.String);
            dbManager.AddParameters(12, PARM_EMAIL, ds.CaseAdjuster.EmailColumn.ColumnName, DbType.String);
            dbManager.AddParameters(13, PARM_PREFERENCE, ds.CaseAdjuster.PreferenceColumn.ColumnName, DbType.String);
            dbManager.AddParameters(14, PARM_NOTES, ds.CaseAdjuster.NotesColumn.ColumnName, DbType.String);
            dbManager.AddParameters(15, PARM_IS_ACTIVE, ds.CaseAdjuster.IsActiveColumn.ColumnName, DbType.Boolean);
            dbManager.AddParameters(16, PARM_CREATED_BY, ds.CaseAdjuster.CreatedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(17, PARM_CREATED_ON, ds.CaseAdjuster.CreatedOnColumn.ColumnName, DbType.DateTime);
            dbManager.AddParameters(18, PARM_MODIFIED_BY, ds.CaseAdjuster.ModifiedByColumn.ColumnName, DbType.String);
            dbManager.AddParameters(19, PARM_MODIFIED_ON, ds.CaseAdjuster.ModifiedOnColumn.ColumnName, DbType.DateTime);
        }
        #endregion

        #region "Insert, delete, update and get Documents using dataset Functions"
        /// <summary>
        /// Loads the CaseAdjuster.
        /// </summary>
        /// <param name="CaseAdjusterId">The Case identifier.</param>
        /// <param name="IsActive">The is active.</param>
        /// <returns></returns>
        public DSCase LoadCaseAdjuster(long CaseAdjusterId, string FirstName, string LastName, string Address, string City, string State, string Zip, string Extention, string IsActive, int PageNumber = 0, int RowspPage = 0)
        {
            //WCNFDetailModel model = new WCNFDetailModel();
            DSCase ds = new DSCase();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(12);
                if (CaseAdjusterId <= 0)
                    dbManager.AddParameters(0, PARM_CASE_ADJUSTER_ID, null);
                else
                    dbManager.AddParameters(0, PARM_CASE_ADJUSTER_ID, CaseAdjusterId);
                if (string.IsNullOrEmpty(FirstName))
                    FirstName = null;
                if (string.IsNullOrEmpty(LastName))
                    LastName = null;
                if (string.IsNullOrEmpty(Address))
                    Address = null;
                if (string.IsNullOrEmpty(City))
                    City = null;
                if (string.IsNullOrEmpty(State))
                    State = null;
                if (string.IsNullOrEmpty(Zip))
                    Zip = null;
                if (string.IsNullOrEmpty(Extention))
                    Extention = null;
                if (string.IsNullOrEmpty(IsActive))
                    IsActive = null;
                dbManager.AddParameters(1, PARM_CASE_FIRST_NAME, FirstName);
                dbManager.AddParameters(2, PARM_LAST_NAME, LastName);
                dbManager.AddParameters(3, PARM_ADDRESS, Address);
                dbManager.AddParameters(4, PARM_CITY, City);
                dbManager.AddParameters(5, PARM_STATE, State);
                dbManager.AddParameters(6, PARM_ZIP, Zip);
                dbManager.AddParameters(7, PARM_EXTENTION, Extention);
                dbManager.AddParameters(8, PARM_IS_ACTIVE, IsActive);
                if (PageNumber == 0)
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, null);
                else
                    dbManager.AddParameters(9, PARM_PAGE_NUMBER, PageNumber);

                if (RowspPage == 0)
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, null);
                else
                    dbManager.AddParameters(10, PARM_ROWS_PERPAGE, RowspPage);
                dbManager.AddParameters(11, PARM_RECORD_COUNT, ds.CaseManagement.RecordCountColumn.ColumnName, DbType.Int64, ParamDirection.Output);
                ds = (DSCase)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_CASE_ADJUSTER_SELECT, ds, ds.CaseAdjuster.TableName);
                ds.AcceptChanges();
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::LoadCaseAdjuster", PROC_CASE_ADJUSTER_SELECT, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Updates the CaseAdjuster
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCase UpdateAdjuster(DSCase ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                this.CreateParameters(dbManager, ds, false);
                ds = (DSCase)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_CASE_ADJUSTER_UPDATE, ds, ds.CaseAdjuster.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::UpdateCaseAdjuster", PROC_CASE_ADJUSTER_UPDATE, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// Deletes the CaseAdjuster.
        /// </summary>
        /// <param name="CaseAdjusterId">The case identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteCaseAdjuster(string CaseAdjusterId)
        {
            string returnVal = "";
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);
                dbManager.AddParameters(0, PARM_CASE_ADJUSTER_ID, CaseAdjusterId);
                dbManager.AddParameters(1, PARM_ERROR_MESSAGE, "", DbType.String, ParamDirection.Output, null, 255);
                returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, PROC_CASE_ADJUSTER_DELETE).ToString();
                if (returnVal != "")
                    throw new Exception(returnVal);

                return "";

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::DeleteCaseAdjuster", PROC_CASE_ADJUSTER_DELETE, ex);
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
        /// Inserts the Case Adjuster.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public DSCase InsertCaseAjuster(DSCase ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSCase)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_CASE_ADJUSTER_INSERT, ds, ds.CaseAdjuster.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCase::InsertCaseAdjuster", PROC_CASE_ADJUSTER_INSERT, ex);
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
